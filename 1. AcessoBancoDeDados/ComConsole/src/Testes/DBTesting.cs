using ComConsole.src.Configurator;
using ComConsole.src.Enums;
using ComConsole.src.Factory;
using ComConsole.src.Factory.Factories;
using ComConsole.src.Model;
using ComConsole.src.Repository.UsuarioRepository;

namespace ComConsole.src.Testes;

public static class DBTesting {
	public static void Test(ETipoBanco tipoBanco) {
		var _config = Configuracao.getConfiguracao();

		//! criando usuario para salvar
		var novoUsuario = new Usuario {
			Nome = "Alexandre",
			Nascimento = new DateTime(1985, 06, 26)
		};

		//+declara uma factory vazia
		ConnectionFactory factory = null;


		//+inancia a factory desejada
		switch (tipoBanco) {
			case ETipoBanco.Firebird:
				factory = new FirebirdFactory(_config);
				break;
			case ETipoBanco.Postgres:
				factory = new PostgresFactory(_config);
				break;
			default:
				throw new Exception("Banco não definido.");
		}

		try {
			//+com a conexão desejada, abre o repositório
			var UsuarioRepo = new UsuarioRepository(factory);

			//+busca um usuário por id
			var searchedUser = UsuarioRepo.FindById(1);
			Console.WriteLine($"Usuário encontrado {searchedUser.Nome} - Id:{searchedUser.Id}");

			//+cria novo usuário
			var createdUser = UsuarioRepo.Create(novoUsuario);
			Console.WriteLine($"Usuário criado {createdUser.Nome} - Id:{createdUser.Id}");

			//+atualiza o usuário criadocria novo usuário
			var userToUpdate = new Usuario {
				Id = createdUser.Id,
				Nome = "Novo nome",
				Nascimento = new DateTime(2000, 01, 01)
			};
			userToUpdate = UsuarioRepo.Update(userToUpdate);
			Console.WriteLine($"Usuário atualizado {userToUpdate.Nome} - Id:{userToUpdate.Id}");

			//+elimina usuário
			UsuarioRepo.Delete(createdUser.Id);
			Console.WriteLine("Usuario deletado");

		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}

	}
}
