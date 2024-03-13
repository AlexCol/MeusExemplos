using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComConsole.src.Factory;
using ComConsole.src.Model;
using Dapper;

namespace ComConsole.src.Repository.UsuarioRepository;

public class UsuarioRepository : IUsuarioRepository {
	ConnectionFactory _factory;
	public UsuarioRepository(ConnectionFactory factory) {
		_factory = factory;
	}

	public Usuario FindById(int id) {
		var comando = @"select * from usuario where id = @id";
		using (var conexao = _factory.Connect()) {
			return conexao.Query<Usuario>(
					comando,
					new { id }
			).First();
		}
	}

	public Usuario Create(Usuario usuario) {
		throw new NotImplementedException();
	}

	public void Delete(int id) {
		throw new NotImplementedException();
	}

	public Usuario Update(Usuario usuario) {
		throw new NotImplementedException();
	}
}
