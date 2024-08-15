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
    classBuilder.AppendLine("using Microsoft.EntityFrameworkCore;");

    if (nameSpace == null || !nameSpace.Equals("")) {
      classBuilder.AppendLine();
      classBuilder.AppendLine($"namespace {nameSpace};");
    }
    classBuilder.AppendLine();
    classBuilder.AppendLine($"[Table(\"{tableName}\")]");
    classBuilder.AppendLine($"/*primary key*/");
    classBuilder.AppendLine($"public class {tableName.ConvertToClassName()}");
    classBuilder.AppendLine("{");

    var nameofKeys = new List<string>();
    foreach (DataRow row in columnsTable.Rows) {
      var columnName = row["COLUMN_NAME"].ToString().Trim();
      var columnPropName = columnName.ConvertToClassPropName() + (columnName.ConvertToClassPropName() == tableName.ConvertToClassName() ? "_" : "");

      var nullable = row["IS_NOT_NULL"].ToString().Trim().Equals("N") ? "?" : "";

      var dataType = dataTypeMapper.MapFromFirebirdType(row);

      var constraint = constraints.Find(c => c.ColumnName == columnName);

      if (constraint != null) {
        if (constraint.ConstraintType == "PRIMARY KEY") {
          classBuilder.AppendLine("    [Key]");
          nameofKeys.Add($"nameof({columnPropName})");
          if (dataType == "int")
            classBuilder.AppendLine("    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
        }

        // if (constraint.ConstraintType == "UNIQUE") {
        //   classBuilder.AppendLine("    [Index(IsUnique = true)]");
        // }

        if (constraint.ConstraintType == "FOREIGN KEY") {
          classBuilder.AppendLine($"    [Column(\"{columnName}\")]");
          classBuilder.AppendLine($"    [ForeignKey(\"{columnName}\")]");
          classBuilder.AppendLine($"    public {dataType}{nullable} {columnPropName}Chave {{ get; set; }}");
          //if (constraint.CircularReference)
          classBuilder.AppendLine("    [NotMapped]");
          classBuilder.AppendLine($"    public virtual {constraint.ReferencedTable.ConvertToClassName()} {columnPropName} {{ get; set; }}");
          classBuilder.AppendLine();
          continue;
        }
      }

      classBuilder.AppendLine($"    [Column(\"{columnName}\")]");
      classBuilder.AppendLine($"    public {dataType}{nullable} {columnPropName} {{ get; set; }}");
      classBuilder.AppendLine();
    }

    classBuilder.AppendLine("}");

    var classStructure = classBuilder.ToString();
    if (nameofKeys.Count == 0) {
      classStructure = classStructure.Replace("/*primary key*/", "[Keyless]");
    } else if (nameofKeys.Count > 1) {
      classStructure = classStructure.Replace("[Key]", "");
      var tagPrimaryKey = "[PrimaryKey(" + String.Join(", ", nameofKeys) + ")]";
      classStructure = classStructure.Replace("/*primary key*/", tagPrimaryKey);
    }

    var classe = new ClassModel {
      ClassName = tableName.ConvertToClassName(),
      ClassStructre = classStructure
    };
    return classe;
  }
}
