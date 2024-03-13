
using ComConsole.src.Configurator;
using ComConsole.src.Enums;
using ComConsole.src.Factory.Factories;
using ComConsole.src.Model;
using ComConsole.src.Repository.UsuarioRepository;

var _config = Configuracao.getConfiguracao();

//! criando usuario para salvar
var user = new Usuario {
	Nome = "Alexandre",
	Nascimento = new DateTime(1985, 06, 26)
};

//!testando conexão com firebird
var fireBirdFac = new FirebirdFactory(_config);
fireBirdFac.Connect();
fireBirdFac.Dispose();
var fireBirdRepo = new UsuarioRepository(fireBirdFac);

var findedUser = fireBirdRepo.FindById(1);
Console.WriteLine(findedUser.Nascimento);