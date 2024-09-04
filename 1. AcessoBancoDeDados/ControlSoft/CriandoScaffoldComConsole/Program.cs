using CriandoScaffoldComConsole.src.Util;
using Microsoft.Extensions.Configuration;

var tabelas = ProgramUtil.AskTables();
var baseClass = ProgramUtil.AskBaseClass();
var nameSpace = ProgramUtil.AskNamesSpace();

var connectionString = new ConfigData().GetConfig().GetConnectionString("Firebird");
var scaffoldGenerator = ProgramUtil.GetScafolldGenerator(connectionString);
scaffoldGenerator.NameSpace = nameSpace;
scaffoldGenerator.BaseClass = baseClass;

ProgramUtil.StartScaffolding(tabelas, scaffoldGenerator);

ProgramUtil.MensagemFinalizado();
