using System.Diagnostics;
using CriandoScaffoldComConsole.Database;
using CriandoScaffoldComConsole.src.Generators;

//! caminho path padrao com base no modo de executação
string path;
if (Debugger.IsAttached)
  path = "../../../src/ScaffoldResult";
else
  path = "./src/ScaffoldResult";
var nameSpace = "Models";

//* cria dbconnection
var connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\BaseDeDados\\CAROL\\DBFAZENDA_CAROL_30.FDB;DataSource=localhost;Port=3050;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;";
var dbConnection = new DatabaseConnection(connectionString);

//!cria o scaffold generator, passando a conexão
var scaffoldGenerator = new ScaffoldGenerator(dbConnection);

//* Lista tabelas as serem buscadas.
//var tableList = new List<string> { "TB_PAISES", "TB_CIDADES", "TB_ESTADOS", "TB_REGIAOUF" };
var tableList = new List<string>();
//var tableList = new List<string> { "TB_PRODUTOS" };
var tabelas = scaffoldGenerator.GetTablesFromList(tableList);

//! Inicia Scaffold.
var totalTabelas = tabelas.Count;
var processadas = 0;
foreach (var tabela in tabelas) {
  Console.Clear();
  processadas++;
  Console.WriteLine($"Processando: {processadas}/{totalTabelas}");
  var classe = scaffoldGenerator.GenerateClasses(tabela, nameSpace); //? Gera a Classe.
  new FileGenerator(path).SaveClass(classe); //? Gera o arquivo da Classe.
}