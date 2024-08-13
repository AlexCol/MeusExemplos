using System.Data;
using System.Text;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public class CodeBuilder
{
  public string BuildClassCode(string tableName, DataTable columnsTable, List<ConstraintInfo> constraints, DataTypeMapper dataTypeMapper)
  {
    var classBuilder = new StringBuilder();
    classBuilder.AppendLine("using System.ComponentModel.DataAnnotations;");
    classBuilder.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
    classBuilder.AppendLine();
    classBuilder.AppendLine($"[Table(\"{tableName}\")]");
    classBuilder.AppendLine($"public class {tableName}");
    classBuilder.AppendLine("{");

    foreach (DataRow row in columnsTable.Rows)
    {
      var columnName = row["COLUMN_NAME"].ToString()?.Trim();
      var dataType = dataTypeMapper.MapFromFirebirdType(row);

      var constraint = constraints.Find(c => c.ColumnName == columnName);

      if (constraint != null)
      {
        if (constraint.ConstraintType == "PRIMARY KEY")
        {
          classBuilder.AppendLine("    [Key]");
        }

        if (constraint.ConstraintType == "UNIQUE")
        {
          classBuilder.AppendLine("    [Index(IsUnique = true)]");
        }

        if (constraint.ConstraintType == "FOREIGN KEY")
        {
          classBuilder.AppendLine($"    public {constraint.ReferencedTable} {constraint.ReferencedTable} {{ get; set; }}");
          classBuilder.AppendLine($"    public {dataType} {columnName} {{ get; set; }}");
          continue;
        }
      }

      classBuilder.AppendLine($"    [Column(\"{columnName}\")]");
      classBuilder.AppendLine($"    public {dataType} {columnName} {{ get; set; }}");
      classBuilder.AppendLine();
    }

    classBuilder.AppendLine("}");
    return classBuilder.ToString();
  }
}
