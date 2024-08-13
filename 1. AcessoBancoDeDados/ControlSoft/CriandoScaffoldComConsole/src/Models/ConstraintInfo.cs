using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CriandoScaffoldComConsole.src.Models;

public class ConstraintInfo {
  public string ConstraintName { get; set; }
  public string ConstraintType { get; set; }
  public string ColumnName { get; set; }
  public string ReferencedTable { get; set; }

  public static List<ConstraintInfo> FromDataTable(DataTable table) {
    var constraints = new List<ConstraintInfo>();
    foreach (DataRow row in table.Rows) {
      constraints.Add(new ConstraintInfo {
        ConstraintName = row["CONSTRAINT_NAME"].ToString()?.Trim(),
        ConstraintType = row["CONSTRAINT_TYPE"].ToString()?.Trim(),
        ColumnName = row["COLUMN_NAME"].ToString()?.Trim(),
        ReferencedTable = row["REFERENCED_TABLE"].ToString()?.Trim()
      });
    }
    return constraints;
  }
}