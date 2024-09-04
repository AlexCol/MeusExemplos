using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CriandoScaffoldComConsole.Database;
using CriandoScaffoldComConsole.src.Generators;

namespace CriandoScaffoldComConsole.src.Util;

public static class ProgramUtil {

  public static List<string> AskTables() {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Informe as tabelas que deseja resgatar. Separadas por virgula (vazio busca todas).");
    Console.ResetColor();
    var tablesString = Console.ReadLine();
    var tablesArray = tablesString.Split(",");
    var tablesList = new List<string>();
    foreach (var table in tablesArray) {
      if (!string.IsNullOrEmpty(table))
        tablesList.Add(table.Trim());
    }
    return tablesList;
  }

  public static string AskBaseClass() {
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Classe base? Em branco se nenhuma.");
    Console.ResetColor();
    var baseClass = Console.ReadLine();
    return baseClass.Trim();
  }

  public static string AskNamesSpace() {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Qual o nameSpace para as classes. Vazio se nenhuma.");
    Console.ResetColor();
    return Console.ReadLine();
  }

  public static ScaffoldGenerator GetScafolldGenerator(string connectionString) {
    //* cria dbconnection
    var dbConnection = new DatabaseConnection(connectionString);

    //!cria o scaffold generator, passando a conexão
    return new ScaffoldGenerator(dbConnection, new FileGenerator());
  }

  public static void StartScaffolding(List<string> tabelas, ScaffoldGenerator scaffoldGenerator) {
    var isDebug = Debugger.IsAttached;
    if (tabelas.Count == 0)
      tabelas = scaffoldGenerator.GetTablesFromList(tabelas);

    var totalTabelas = tabelas.Count;
    var processadas = 0;
    foreach (var tabela in tabelas) {
      if (!isDebug) {
        Console.Clear();
        processadas++;

        var processado = ((processadas * 1.0) / totalTabelas) * 100;
        if (processado <= 20)
          Console.ForegroundColor = ConsoleColor.Green;
        else if (processado <= 60)
          Console.ForegroundColor = ConsoleColor.Yellow;
        else
          Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Processando:  {processadas} / {totalTabelas} - {processado.ToString("F2")}%");
        Console.ResetColor();
      }
      scaffoldGenerator.GenerateClasses(tabela); //? Gera a Classe.
    }
  }

  public static void MensagemFinalizado() {
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.BackgroundColor = ConsoleColor.White;
    Console.WriteLine("Processo finalizado.");
    Console.ForegroundColor = ConsoleColor.Red;
    var pasta = new ConfigData().GetConfig()["PastaScaffold"];
    Console.WriteLine($"Arquivos salvos em: {pasta}.");
    Console.ResetColor();
  }
}
