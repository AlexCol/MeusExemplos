using System.Data;
using System.Text;
using CriandoScaffoldComConsole.src.Extensions;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public partial class CodeBuilder {
  private void AdicionaCorpoDaClasse(StringBuilder classBuilder, string tableName, DataTable columnsTable, List<ConstraintInfo> constraints, DataTypeMapper dataTypeMapper, List<string> nameofKeys) {
    foreach (DataRow row in columnsTable.Rows) {
      var columnName = row["COLUMN_NAME"].ToString().Trim();
      var columnPropName = columnName.ConvertToClassPropName() + (columnName.ConvertToClassPropName() == tableName.ConvertToClassName() ? "_" : "");
      var dataType = dataTypeMapper.MapFromFirebirdType(row);
      var nullable = dataType.Nullable ? "?" : "";
      var constraintsDaColuna = constraints.Where(c => c.ColumnName == columnName).ToList();

      if (constraintsDaColuna.Count == 0) {
        AdicionaProperty(classBuilder, columnName, dataType, nullable, columnPropName);
      } else {
        AdicionaPropertyComConstraint(classBuilder, constraintsDaColuna, columnName, dataType, nullable, columnPropName, nameofKeys);
      }
    }
  }

  private void AdicionaProperty(StringBuilder classBuilder, string columnName, DBDataType dataType, string nullable, string columnPropName) {
    classBuilder.AppendLine($"    [Column(\"{columnName}\"{dataType.TypeName})]");
    classBuilder.AppendLine($"    public {dataType.PropName}{nullable} {columnPropName} {{ get; set; }}");
    classBuilder.AppendLine();
  }

  private void AdicionaPropertyComConstraint(StringBuilder classBuilder, List<ConstraintInfo> constraintsDaColuna, string columnName, DBDataType dataType, string nullable, string columnPropName, List<string> nameofKeys) {
    var isPKey = constraintsDaColuna.FirstOrDefault(ci => ci.ConstraintType == "PRIMARY KEY") != null;
    var constraintFK = constraintsDaColuna.FirstOrDefault(ci => ci.ConstraintType == "FOREIGN KEY" && ci.ColumnName == columnName);
    var isFKey = constraintFK != null;

    if (isPKey) {
      classBuilder.AppendLine("    [Key]");
      nameofKeys.Add($"\"{columnPropName}\"");
    }

    AdicionaProperty(classBuilder, columnName, dataType, nullable, columnPropName);
    if (isFKey) {
      AddForeignKeyProperty(classBuilder, constraintFK, columnPropName);
    }
  }

  private void AddForeignKeyProperty(StringBuilder classBuilder, ConstraintInfo constraintFK, string columnPropName) {
    var isCompositeKey = constraintFK.ConstraintNumber > 1;
    if (isCompositeKey && constraintFK.PKNumberFromReferenceTable == constraintFK.ConstraintNumber) {
      AddCompositeFK(classBuilder, constraintFK, columnPropName);
    } else {
      AddSimpleFK(classBuilder, constraintFK, columnPropName);
    }
  }

  private void AddCompositeFK(StringBuilder classBuilder, ConstraintInfo constraintFK, string columnPropName) {
    var alreadyMapped = classBuilder.ToString().Contains($"public virtual {constraintFK.ReferencedTable.ConvertToClassName()}");
    if (alreadyMapped) {
      classBuilder.Replace($"\" /*fk - {constraintFK.ReferencedTable}*/", $", {columnPropName}\" /*fk - {constraintFK.ReferencedTable}*/");
    } else {
      classBuilder.AppendLine($"    [ForeignKey(\"{columnPropName}\" /*fk - {constraintFK.ReferencedTable}*/)]");
      classBuilder.AppendLine($"    public virtual {constraintFK.ReferencedTable.ConvertToClassName()} {constraintFK.ReferencedTable.ConvertToClassName()}Obj {{ get; set; }}");
      classBuilder.AppendLine();
    }
  }

  private void AddSimpleFK(StringBuilder classBuilder, ConstraintInfo constraintFK, string columnPropName) {
    classBuilder.AppendLine($"    [ForeignKey(\"{columnPropName}\")]");

    if (constraintFK.CircularReference) classBuilder.AppendLine($"    [NotMapped]");

    classBuilder.AppendLine($"    public virtual {constraintFK.ReferencedTable.ConvertToClassName()} {columnPropName}Obj {{ get; set; }}");
    classBuilder.AppendLine();
  }
}
