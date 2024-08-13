using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriandoScaffoldComConsole.src.Models;

namespace CriandoScaffoldComConsole.src.Generators;

public class FileGenerator {
  private readonly string _path;

  public FileGenerator(string path) {
    _path = path;
  }

  public void SaveClass(ClassModel classe) {
    SaveClassFile(classe);
  }

  private void SaveClassFile(ClassModel classe) {
    var classPath = Path.Combine(_path, $"{classe.ClassName}.cs");
    using (StreamWriter sw = new StreamWriter(classPath, false)) {
      sw.Write(classe.ClassStructre);
    }
  }
}
