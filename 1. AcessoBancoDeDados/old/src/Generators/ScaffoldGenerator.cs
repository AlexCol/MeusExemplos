using System.Data;
using CriandoScaffoldComConsole.Database;
using CriandoScaffoldComConsole.src.Models;
using FirebirdSql.Data.FirebirdClient;

namespace CriandoScaffoldComConsole.src.Generators;

public class ScaffoldGenerator
{
  private readonly DatabaseConnection _dbConnection;
  private readonly CodeBuilder _codeBuilder;
  private readonly DataTypeMapper _dataTypeMapper;

  public ScaffoldGenerator(DatabaseConnection dbConnection)
  {
    _dbConnection = dbConnection;
    _codeBuilder = new CodeBuilder();
    _dataTypeMapper = new DataTypeMapper();
  }

  public void GenerateClasses(string tableName)
  {
    using var connection = _dbConnection.GetConnection();
    connection.Open();

    // Obter informações de colunas e constraints
    var columnsTable = GetColumnsTable(connection, tableName);
    var constraints = GetConstraints(connection, tableName);

    var classCode = _codeBuilder.BuildClassCode(tableName, columnsTable, constraints, _dataTypeMapper);
    Console.WriteLine(classCode);
  }

  private DataTable GetColumnsTable(FbConnection connection, string tableName)
  {
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
            ";
    return DatabaseHelper.ExecuteQuery(connection, query);
  }

  private List<ConstraintInfo> GetConstraints(FbConnection connection, string tableName)
  {
    var query = $@"
                SELECT
                    rc.RDB$CONSTRAINT_NAME AS CONSTRAINT_NAME,
                    rc.RDB$CONSTRAINT_TYPE AS CONSTRAINT_TYPE,
                    iseg.RDB$FIELD_NAME AS COLUMN_NAME,
                    rc.RDB$RELATION_NAME AS TABLE_NAME,
                    replace(refc.RDB$CONST_NAME_UQ, 'PK_', '') AS REFERENCED_TABLE
                FROM
                    RDB$RELATION_CONSTRAINTS rc
                    JOIN RDB$INDEX_SEGMENTS iseg ON rc.RDB$INDEX_NAME = iseg.RDB$INDEX_NAME
                    LEFT JOIN RDB$REF_CONSTRAINTS refc ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME
                WHERE
                    rc.RDB$RELATION_NAME = '{tableName}'
            ";
    var constraintsTable = DatabaseHelper.ExecuteQuery(connection, query);
    return ConstraintInfo.FromDataTable(constraintsTable);
  }
}