using System.Data;
using System.Text;
using CriandoScaffoldComConsole.src.Extensions;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public class CodeBuilder {
  private List<string> nameofKeys;
  public ClassModel BuildClassCode(string tableName, string baseClass, string nameSpace, DataTable columnsTable, List<ConstraintInfo> constraints, DataTypeMapper dataTypeMapper) {
    var classBuilder = new StringBuilder();
    nameofKeys = new List<string>();
    AdicionaUsings(classBuilder);
    AdicionaNamespace(classBuilder, nameSpace);
    AdicionaDefinicaoDaClasse(classBuilder, tableName, baseClass);
    AdicionaCorpoDaClasse(classBuilder, tableName, columnsTable, constraints, dataTypeMapper);
    AdicionaFinalizacaoClasse(classBuilder, constraints);

    return new ClassModel {
      ClassName = tableName.ConvertToClassName(),
      ClassStructre = classBuilder.ToString()
    };
  }
  private void AdicionaUsings(StringBuilder classBuilder) {
    classBuilder.AppendLine("using System.ComponentModel.DataAnnotations;");
    classBuilder.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
    classBuilder.AppendLine("using Microsoft.EntityFrameworkCore;");
  }
  private void AdicionaNamespace(StringBuilder classBuilder, string nameSpace) {
    if (nameSpace == null || !nameSpace.Equals("")) {
      classBuilder.AppendLine();
      classBuilder.AppendLine($"namespace {nameSpace};");
    }
  }

  private void AdicionaDefinicaoDaClasse(StringBuilder classBuilder, string tableName, string baseClass) {
    classBuilder.AppendLine();
    classBuilder.AppendLine($"[Table(\"{tableName}\")]");
    classBuilder.AppendLine($"/*primary key*/");
    classBuilder.AppendLine($"public class {tableName.ConvertToClassName()}" + (string.IsNullOrEmpty(baseClass) ? "" : $": {baseClass}"));
    classBuilder.AppendLine("{");
  }

  private void AdicionaCorpoDaClasse(StringBuilder classBuilder, string tableName, DataTable columnsTable, List<ConstraintInfo> constraints, DataTypeMapper dataTypeMapper) {
    foreach (DataRow row in columnsTable.Rows) {
      var columnName = row["COLUMN_NAME"].ToString().Trim();
      var columnPropName = columnName.ConvertToClassPropName() + (columnName.ConvertToClassPropName() == tableName.ConvertToClassName() ? "_" : "");
      var nullable = row["IS_NOT_NULL"].ToString().Trim().Equals("N") ? "?" : "";
      var dataType = dataTypeMapper.MapFromFirebirdType(row);
      var constraintsDaColuna = constraints.Where(c => c.ColumnName == columnName).ToList();

      if (constraintsDaColuna.Count == 0) {
        AdicionaProperty(classBuilder, columnName, dataType, nullable, columnPropName);
      } else {
        AdicionaPropertyComConstraint(classBuilder, constraintsDaColuna, columnName, dataType, nullable, columnPropName);
      }
    }
  }

  private void AdicionaProperty(StringBuilder classBuilder, string columnName, string dataType, string nullable, string columnPropName) {
    classBuilder.AppendLine($"    [Column(\"{columnName}\")]");
    classBuilder.AppendLine($"    public {dataType}{nullable} {columnPropName} {{ get; set; }}");
    classBuilder.AppendLine();
  }

  private void AdicionaPropertyComConstraint(StringBuilder classBuilder, List<ConstraintInfo> constraintsDaColuna, string columnName, string dataType, string nullable, string columnPropName) {
    var isKey = false;
    var constraintPK = constraintsDaColuna.FirstOrDefault(ci => ci.ConstraintType == "PRIMARY KEY");
    if (constraintPK != null) {
      classBuilder.AppendLine("    [Key]");
      isKey = true;
      nameofKeys.Add("nameof(" + columnPropName + ")");
    }

    var constraintFK = constraintsDaColuna.FirstOrDefault(ci => ci.ConstraintType == "FOREIGN KEY" && ci.ColumnName == columnName);
    if (constraintFK != null && !isKey) {
      classBuilder.AppendLine($"    [Column(\"{columnName}\")]");
      classBuilder.AppendLine($"    [ForeignKey(\"{columnName}\")]");
      classBuilder.AppendLine($"    public {dataType}{nullable} {columnPropName}Chave {{ get; set; }}");
      //if (constraint.CircularReference)
      if (!isKey)
        classBuilder.AppendLine("    [NotMapped]");
      classBuilder.AppendLine($"    public virtual {constraintFK.ReferencedTable.ConvertToClassName()} {columnName} {{ get; set; }}");
      classBuilder.AppendLine();
    } else {
      AdicionaProperty(classBuilder, columnName, dataType, nullable, columnPropName);
    }
  }

  private void AdicionaFinalizacaoClasse(StringBuilder classBuilder, List<ConstraintInfo> constraints) {
    classBuilder.AppendLine("}");
    var classStructure = classBuilder.ToString();
    if (nameofKeys.Count == 0) {
      classStructure = classStructure.Replace("/*primary key*/", "[Keyless]");
    } else if (nameofKeys.Count > 1) {
      classStructure = classStructure.Replace("[Key]", "");
      classStructure = classStructure.Replace("/*primary key*/", $"[PrimaryKey({string.Join(", ", nameofKeys)})]");
    }
    classBuilder.Clear();
    classBuilder.Append(classStructure);
  }
}
