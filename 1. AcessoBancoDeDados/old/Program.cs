using CriandoScaffoldComConsole.Database;
using CriandoScaffoldComConsole.src.Generators;

var connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\BaseDeDados\\CAROL\\DBFAZENDA_CAROL_30.FDB;DataSource=localhost;Port=3050;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;";
var dbConnection = new DatabaseConnection(connectionString);
var scaffoldGenerator = new ScaffoldGenerator(dbConnection);

Console.WriteLine("Digite o nome da tabela:");
//var tableName = Console.ReadLine();
var tableName = "TB_CIDADES";

scaffoldGenerator.GenerateClasses(tableName);