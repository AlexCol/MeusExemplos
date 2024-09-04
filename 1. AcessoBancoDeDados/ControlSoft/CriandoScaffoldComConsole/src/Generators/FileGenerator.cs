using System.Diagnostics;
using System.Text;
using CriandoScaffoldComConsole.src.Models;
using CriandoScaffoldComConsole.src.Util;
using Microsoft.Extensions.Configuration;

namespace CriandoScaffoldComConsole.src.Generators;

public class FileGenerator {

  private readonly IConfiguration _config;
  private string _path = new ConfigData().GetConfig()["PastaScaffold"];
  private readonly string _contextPath;
  private readonly string _contextFileName = "MyDbContext";

  public FileGenerator(string path = "") {
    _config = new ConfigData().GetConfig();
    _path = !string.IsNullOrEmpty(path) ? path : _config["PastaScaffold"];
    Directory.CreateDirectory(_path);
    _contextPath = Path.Combine(_path, "context");
    Directory.CreateDirectory(_contextPath);
  }

  public void SaveClass(ClassModel classe, string nameSpace, string baseClass) {
    if (!string.IsNullOrEmpty(baseClass)) {
      VerifyOrCreateBaseClass(baseClass, nameSpace);
    }

    SaveClassFile(classe);
    CreateOrUpdateContext(classe, nameSpace);
  }

  private void VerifyOrCreateBaseClass(string baseClass, string nameSpace) {
    var baseClassPath = Path.Combine(_contextPath, $"{baseClass}.cs");

    // Verificar se a classe base já existe
    if (!File.Exists(baseClassPath)) {
      // Criar a classe base vazia
      var builder = new StringBuilder();
      if (!string.IsNullOrEmpty(nameSpace)) {
        builder.AppendLine($"namespace {nameSpace};");
        builder.AppendLine();
      }
      builder.AppendLine($"public class {baseClass} {{ }}");

      // Salvar a classe base no mesmo diretório do DbContext
      using StreamWriter sw = new StreamWriter(baseClassPath, false);
      sw.Write(builder.ToString());
    }
  }

  private void SaveClassFile(ClassModel classe) {
    var classPath = Path.Combine(_path, $"{classe.ClassName}.cs");
    using StreamWriter sw = new StreamWriter(classPath, false);
    sw.Write(classe.ClassStructre);
  }

  private void CreateOrUpdateContext(ClassModel classe, string nameSpace) {
    if (!File.Exists(Path.Combine(_contextPath, $"{_contextFileName}.cs"))) {
      CreateDbContext(nameSpace);
    }
    UpdateDbSet(classe.ClassName);
  }

  private void UpdateDbSet(string classe) {
    var filePath = Path.Combine(_contextPath, $"{_contextFileName}.cs");
    var conteudo = File.ReadAllText(filePath);
    if (!conteudo.Contains(classe)) {
      var updatedContent = conteudo.Replace("/*atualizar aqui*/", $"public DbSet<{classe}> {classe} {{ get; set; }}\n/*atualizar aqui*/");
      File.WriteAllText(filePath, updatedContent);
    }
  }

  private void CreateDbContext(string nameSpace) {
    var builder = new StringBuilder();
    if (!string.IsNullOrEmpty(nameSpace))
      builder.AppendLine($"using {nameSpace};");

    builder.AppendLine("using Microsoft.EntityFrameworkCore;");
    builder.AppendLine();
    builder.AppendLine("public class MyDbContext : DbContext {");
    builder.AppendLine("  /*atualizar aqui*/");
    builder.AppendLine("  public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }");
    builder.AppendLine("}");
    var contextPath = Path.Combine(_contextPath, $"{_contextFileName}.cs");
    using StreamWriter sw = new StreamWriter(contextPath, false);
    sw.Write(builder.ToString());
  }
}
