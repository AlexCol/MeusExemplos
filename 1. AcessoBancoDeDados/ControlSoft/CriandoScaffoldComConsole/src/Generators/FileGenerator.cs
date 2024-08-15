using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public class FileGenerator {

  private string _path = Debugger.IsAttached ? "../../../src/ScaffoldResult" : "./src/ScaffoldResult";
  private readonly string _contextPath;
  private readonly string _contextFileName = "MyDbContext";

  public FileGenerator(string path = "") {
    if (path != "")
      _path = path;
    Directory.CreateDirectory(_path);

    _contextPath = Path.Combine(_path, "context");
    Directory.CreateDirectory(_contextPath);
  }

  public void SaveClass(ClassModel classe) {
    SaveClassFile(classe);
    CreateOrUpdateContext(classe);
  }

  private void SaveClassFile(ClassModel classe) {
    var classPath = Path.Combine(_path, $"{classe.ClassName}.cs");
    using (StreamWriter sw = new StreamWriter(classPath, false)) {
      sw.Write(classe.ClassStructre);
    }
  }
  private void CreateOrUpdateContext(ClassModel classe) {
    var contextPath = Path.Combine(_path, "context");
    if (!File.Exists(Path.Combine(contextPath, _contextFileName + ".cs"))) {
      CriaDbContext();
    }
    AtualizaDbSet(classe.ClassName);
  }

  private void AtualizaDbSet(string classe) {
    var filePath = Path.Combine(_contextPath, _contextFileName + ".cs");
    string conteudo = File.ReadAllText(filePath);
    if (!conteudo.Contains(classe)) {
      var atualizado = conteudo.Replace("/*atualizar aqui*/", "public DbSet<" + classe + "> " + classe + " { get; set; }" + '\n' + "/*atualizar aqui*/");
      File.WriteAllText(filePath, atualizado);
    }
  }

  private void CriaDbContext() {
    var builder = new StringBuilder();
    builder.AppendLine("using Models;");
    builder.AppendLine("using Microsoft.EntityFrameworkCore;");
    builder.AppendLine("");
    builder.AppendLine("public class MyDBContext : DbContext {");
    builder.AppendLine("  /*atualizar aqui*/");
    builder.AppendLine("  public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }");
    builder.AppendLine("}");
    var contextPath = Path.Combine(_contextPath, $"{_contextFileName}.cs");
    using (StreamWriter sw = new StreamWriter(contextPath, false)) {
      sw.Write(builder);
    }
  }
}
