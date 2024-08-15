using CriandoScaffoldComConsole.src.Util;

var connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\BaseDeDados\\CAROL\\DBFAZENDA_CAROL_30.FDB;DataSource=localhost;Port=3050;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;";

var tabelas = ProgramUtil.AskTables();
var nameSpace = ProgramUtil.AskNamesSpace();
var scaffoldGenerator = ProgramUtil.GetScafolldGenerator(connectionString);
ProgramUtil.StartScaffolding(tabelas, nameSpace, scaffoldGenerator);

