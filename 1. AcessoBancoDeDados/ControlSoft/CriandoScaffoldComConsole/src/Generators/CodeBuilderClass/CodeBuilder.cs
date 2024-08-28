using System.Data;
using System.Text;
using CriandoScaffoldComConsole.src.Extensions;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public partial class CodeBuilder {
  private List<string> nameofKeys;
  public ClassModel BuildClassCode(string tableName, string baseClass, string nameSpace, DataTable columnsTable, List<ConstraintInfo> constraints, DataTypeMapper dataTypeMapper) {
    var classBuilder = new StringBuilder();
    nameofKeys = new List<string>();
    AdicionaUsings(classBuilder);
    AdicionaNamespace(classBuilder, nameSpace);
    AdicionaDefinicaoDaClasse(classBuilder, tableName, baseClass);
    AdicionaCorpoDaClasse(classBuilder, tableName, columnsTable, constraints, dataTypeMapper, nameofKeys);
    AdicionaFinalizacaoClasse(classBuilder, nameofKeys);

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
    classBuilder.AppendLine($"public class {tableName.ConvertToClassName()}" + (string.IsNullOrEmpty(baseClass) ? "" : $" : {baseClass}"));
    classBuilder.AppendLine("{");
  }

  /*
  ! AdicionaCorpoDaClasse na classe parcial por ter ficado grande
  */

  private void AdicionaFinalizacaoClasse(StringBuilder classBuilder, List<string> nameofKeys) {
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
