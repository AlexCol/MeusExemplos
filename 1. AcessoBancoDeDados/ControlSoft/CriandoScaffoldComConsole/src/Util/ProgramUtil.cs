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
    Console.WriteLine("Informe as tabelas que deseja resgatar. Separadas por virgula (vazio busca todas).");
    var tablesString = Console.ReadLine();
    var tablesArray = tablesString.Split(",");
    var tablesList = new List<string>();
    foreach (var table in tablesArray) {
      if (!string.IsNullOrEmpty(table))
        tablesList.Add(table.Trim());
    }
    return tablesList;
  }

  public static string AskNamesSpace() {
    Console.WriteLine("Qual o nameSpace para as classes. Vazio se nenhuma.");
    return Console.ReadLine();
  }

  public static ScaffoldGenerator GetScafolldGenerator() {
    //* cria dbconnection
    var connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\BaseDeDados\\CAROL\\DBFAZENDA_CAROL_30.FDB;DataSource=localhost;Port=3050;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;";
    var dbConnection = new DatabaseConnection(connectionString);

    //!cria o scaffold generator, passando a conexão
    return new ScaffoldGenerator(dbConnection, new FileGenerator());
  }

  public static void StartScaffolding(List<string> tabelas, string nameSpace, ScaffoldGenerator scaffoldGenerator) {
    var isDebug = Debugger.IsAttached;
    if (tabelas.Count == 0)
      tabelas = scaffoldGenerator.GetTablesFromList(tabelas);

    var totalTabelas = tabelas.Count;
    var processadas = 0;
    foreach (var tabela in tabelas) {
      if (!isDebug) {
        Console.Clear();
        processadas++;
        Console.WriteLine($"Processando: {processadas}/{totalTabelas}");
      }
      scaffoldGenerator.GenerateClasses(tabela, nameSpace); //? Gera a Classe.
    }
  }
}
