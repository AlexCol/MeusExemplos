using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CriandoScaffoldComConsole.Database;
using CriandoScaffoldComConsole.src.Models;
using FirebirdSql.Data.FirebirdClient;

namespace CriandoScaffoldComConsole.src.Generators {
  public class ScaffoldGenerator {
    private readonly DatabaseConnection _dbConnection;
    private readonly CodeBuilder _codeBuilder;
    private readonly DataTypeMapper _dataTypeMapper;

    public ScaffoldGenerator(DatabaseConnection dbConnection) {
      _dbConnection = dbConnection;
      _codeBuilder = new CodeBuilder();
      _dataTypeMapper = new DataTypeMapper();
    }

    public ClassModel GenerateClasses(string tableName, string nameSpace = "") {
      try {
        using var connection = _dbConnection.GetConnection();
        connection.Open();

        // Obter informações de colunas e constraints
        var columnsTable = GetColumnsTable(connection, tableName);
        if (columnsTable.Rows.Count == 0) throw new InvalidDataException($"Tabela {tableName} não existe.");

        var constraints = GetConstraints(connection, tableName);

        var classCode = _codeBuilder.BuildClassCode(tableName, nameSpace, columnsTable, constraints, _dataTypeMapper);
        return classCode;
      } catch (Exception e) {
        throw new InvalidDataException($"Erro na tabela {tableName}. {e.Message}");
      }
    }

    public List<string> GetTablesFromList(List<string> list) {
      using var connection = _dbConnection.GetConnection();
      connection.Open();

      var query = CreateTablesQuery(list);
      var dataBaseList = GetTablesFromDatabase(connection, query);

      if (list.Count > 0 && list.Count != dataBaseList.Count)
        throw new InvalidDataException($"Tabelas não encontradas: {String.Join(",", list.Except(dataBaseList))}");

      return dataBaseList;
    }

    private string CreateTablesQuery(List<string> list) {
      var query = $@"
                SELECT 
                  TRIM(RDB$RELATION_NAME) AS TABLE_NAME
                FROM RDB$RELATIONS
                WHERE RDB$SYSTEM_FLAG = 0
                  AND RDB$VIEW_BLR IS NULL
                  AND TRIM(RDB$RELATION_NAME) NOT LIKE 'TBT%'
                  AND TRIM(RDB$RELATION_NAME) LIKE 'TB%'
                  /**/
                ORDER BY RDB$RELATION_NAME
            ";

      if (list.Count > 0) {
        var text = String.Join(", ", list.Select(p => $"'{p}'"));
        query = query.Replace("/**/", $"AND TRIM(RDB$RELATION_NAME) IN ({text})");
      }

      return query;
    }

    private List<string> GetTablesFromDatabase(FbConnection connection, string query) {
      var dataBaseList = new List<string>();
      var tables = DatabaseHelper.ExecuteQuery(connection, query);
      foreach (DataRow row in tables.Rows) {
        dataBaseList.Add(row["TABLE_NAME"].ToString()?.Trim());
      }

      return dataBaseList;
    }

    private DataTable GetColumnsTable(FbConnection connection, string tableName) {
      var query = $@"
                SELECT
                    rf.RDB$FIELD_NAME AS COLUMN_NAME,
                    f.RDB$FIELD_TYPE AS FIELD_TYPE,
                    f.RDB$FIELD_SUB_TYPE AS FIELD_SUB_TYPE,
                    f.RDB$FIELD_LENGTH AS FIELD_LENGTH,
                    f.RDB$FIELD_PRECISION AS FIELD_PRECISION,
                    f.RDB$FIELD_SCALE AS FIELD_SCALE
                FROM
                    RDB$RELATION_FIELDS rf
                    JOIN RDB$FIELDS f ON rf.RDB$FIELD_SOURCE = f.RDB$FIELD_NAME
                WHERE
                    rf.RDB$RELATION_NAME = '{tableName}'
                ORDER BY RDB$FIELD_POSITION
            ";
      return DatabaseHelper.ExecuteQuery(connection, query);
    }

    private List<ConstraintInfo> GetConstraints(FbConnection connection, string tableName) {
      var query = $@"
                  SELECT
                      rc.RDB$CONSTRAINT_NAME AS CONSTRAINT_NAME,
                      rc.RDB$CONSTRAINT_TYPE AS CONSTRAINT_TYPE,
                      iseg.RDB$FIELD_NAME AS COLUMN_NAME,
                      rc.RDB$RELATION_NAME AS TABLE_NAME,
                      trim(ref_tbl.RDB$RELATION_NAME) AS REFERENCED_TABLE
                  FROM
                      RDB$RELATION_CONSTRAINTS rc
                      JOIN RDB$INDEX_SEGMENTS iseg ON rc.RDB$INDEX_NAME = iseg.RDB$INDEX_NAME
                      LEFT JOIN RDB$REF_CONSTRAINTS refc ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME
                      LEFT JOIN RDB$RELATION_CONSTRAINTS refc_tbl ON refc.RDB$CONST_NAME_UQ = refc_tbl.RDB$CONSTRAINT_NAME
                      LEFT JOIN RDB$RELATIONS ref_tbl ON refc_tbl.RDB$RELATION_NAME = ref_tbl.RDB$RELATION_NAME
                  WHERE
                    rc.RDB$RELATION_NAME = '{tableName}'
            ";
      var constraintsTable = DatabaseHelper.ExecuteQuery(connection, query);
      return ConstraintInfo.FromDataTable(constraintsTable);
    }
  }
}
