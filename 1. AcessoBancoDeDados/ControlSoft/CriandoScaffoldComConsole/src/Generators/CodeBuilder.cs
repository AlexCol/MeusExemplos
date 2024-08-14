using System.Data;
using System.Text;
using CriandoScaffoldComConsole.src.Extensions;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public class CodeBuilder {
  public ClassModel BuildClassCode(string tableName, string nameSpace, DataTable columnsTable, List<ConstraintInfo> constraints, DataTypeMapper dataTypeMapper) {
    var classBuilder = new StringBuilder();
    classBuilder.AppendLine("using System.ComponentModel.DataAnnotations;");
    classBuilder.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
    if (nameSpace == null || !nameSpace.Equals("")) {
      classBuilder.AppendLine();
      classBuilder.AppendLine($"namespace {nameSpace};");
    }
    classBuilder.AppendLine();
    classBuilder.AppendLine($"[Table(\"{tableName}\")]");
    classBuilder.AppendLine($"public class {tableName.ConvertToClassName()}");
    classBuilder.AppendLine("{");

    foreach (DataRow row in columnsTable.Rows) {
      var columnName = row["COLUMN_NAME"].ToString()?.Trim();

      var dataType = dataTypeMapper.MapFromFirebirdType(row);

      var constraint = constraints.Find(c => c.ColumnName == columnName);

      if (constraint != null) {
        if (constraint.ConstraintType == "PRIMARY KEY") {
          classBuilder.AppendLine("    [Key]");
          if (dataType == "int")
            classBuilder.AppendLine("    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
        }

        // if (constraint.ConstraintType == "UNIQUE") {
        //   classBuilder.AppendLine("    [Index(IsUnique = true)]");
        // }

        if (constraint.ConstraintType == "FOREIGN KEY") {
          classBuilder.AppendLine($"    [ForeignKey(\"{columnName}\")]");
          classBuilder.AppendLine($"    public {dataType} ID{columnName.ConvertToClassPropName()} {{ get; set; }}");
          classBuilder.AppendLine($"    public {constraint.ReferencedTable.ConvertToClassName()} {columnName.ConvertToClassPropName()} {{ get; set; }}");
          classBuilder.AppendLine();
          continue;
        }
      }

      classBuilder.AppendLine($"    [Column(\"{columnName}\")]");
      classBuilder.AppendLine($"    public {dataType} {columnName.ConvertToClassPropName()} {{ get; set; }}");
      classBuilder.AppendLine();
    }

    classBuilder.AppendLine("}");
    var classe = new ClassModel {
      ClassName = tableName.ConvertToClassName(),
      ClassStructre = classBuilder.ToString()
    };
    return classe;
  }
}
