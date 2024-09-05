using System.Data;
using CriandoScaffoldComConsole.Database;
using FirebirdSql.Data.FirebirdClient;

namespace CriandoScaffoldComConsole.src.Generators;

public partial class ScaffoldGenerator {
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

  private DataTable GetColumnsTable(FbConnection connection, string tableName) {
    var query = $@"
                SELECT
                    rf.RDB$FIELD_NAME AS COLUMN_NAME,
                    f.RDB$FIELD_TYPE AS FIELD_TYPE,
                    f.RDB$FIELD_SUB_TYPE AS FIELD_SUB_TYPE,
                    f.RDB$FIELD_LENGTH AS FIELD_LENGTH,
                    f.RDB$FIELD_PRECISION AS FIELD_PRECISION,
                    f.RDB$FIELD_SCALE AS FIELD_SCALE,
                    CASE coalesce(rf.RDB$NULL_FLAG, 0) WHEN 1 THEN 'S' ELSE 'N' END AS IS_NOT_NULL
                FROM
                    RDB$RELATION_FIELDS rf
                    JOIN RDB$FIELDS f ON rf.RDB$FIELD_SOURCE = f.RDB$FIELD_NAME
                WHERE
                    rf.RDB$RELATION_NAME = '{tableName}'
                ORDER BY RDB$FIELD_POSITION
            ";
    return DatabaseHelper.ExecuteQuery(connection, query);
  }

  private string QueryContraints(string tableName) {
    var query = $@"
                  SELECT
                      rc.RDB$CONSTRAINT_NAME AS CONSTRAINT_NAME,
                      rc.RDB$CONSTRAINT_TYPE AS CONSTRAINT_TYPE,
                      iseg.RDB$FIELD_NAME AS COLUMN_NAME,
                      rc.RDB$RELATION_NAME AS TABLE_NAME,
                      trim(ref_tbl.RDB$RELATION_NAME) AS REFERENCED_TABLE,
                      COUNT(*) OVER (PARTITION BY rc.RDB$CONSTRAINT_TYPE, ref_tbl.RDB$RELATION_NAME) AS CONSTRAINT_NUMBER
                  FROM
                      RDB$RELATION_CONSTRAINTS rc
                      JOIN RDB$INDEX_SEGMENTS iseg ON rc.RDB$INDEX_NAME = iseg.RDB$INDEX_NAME
                      LEFT JOIN RDB$REF_CONSTRAINTS refc ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME
                      LEFT JOIN RDB$RELATION_CONSTRAINTS refc_tbl ON refc.RDB$CONST_NAME_UQ = refc_tbl.RDB$CONSTRAINT_NAME
                      LEFT JOIN RDB$RELATIONS ref_tbl ON refc_tbl.RDB$RELATION_NAME = ref_tbl.RDB$RELATION_NAME
                  WHERE
                    rc.RDB$RELATION_NAME = '{tableName}'
            ";
    return query;
  }
}
