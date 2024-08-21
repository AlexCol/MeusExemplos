using System.Data;

namespace CriandoScaffoldComConsole.src.Models;

public class ConstraintInfo {
  public string ConstraintName { get; set; }
  public string ConstraintType { get; set; }
  public string ColumnName { get; set; }
  public string ReferencedTable { get; set; }
  public int ConstraintNumber { get; set; }
  public bool CircularReference { get; set; } = false;
  public int PKNumberFromReferenceTable { get; set; } = 0;

  public static List<ConstraintInfo> FromDataTable(DataTable table) {
    return table.AsEnumerable().Select(row => new ConstraintInfo {
      ConstraintName = row["CONSTRAINT_NAME"].ToString()?.Trim(),
      ConstraintType = row["CONSTRAINT_TYPE"].ToString()?.Trim(),
      ColumnName = row["COLUMN_NAME"].ToString()?.Trim(),
      ReferencedTable = row["REFERENCED_TABLE"].ToString()?.Trim(),
      ConstraintNumber = int.Parse(row["CONSTRAINT_NUMBER"].ToString()?.Trim())
    }).ToList();
  }
}
