using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ComConsole.src.Extensions.toString;
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
		var comando = @"select * from usuario where id = ?id";
		comando = comando.TrataComandoPorBanco(_factory.getTipoBanco());
		try {
			using (var conexao = _factory.Connect()) {
				return conexao.Query<Usuario>(
						comando,
						new { id }
				).First();
			}
		} catch (Exception e) {
			throw new Exception("Usuário não encontrado!" + e.Message);
		}
	}

	public Usuario Create(Usuario usuario) {
		var comando = @"insert into usuario (nome, nascimento) values (?nome, ?nascimento);";
		comando = comando.TrataComandoPorBanco(_factory.getTipoBanco());
		try {
			using (var conexao = _factory.Connect()) {
				conexao.Execute(
						comando,
						new {
							nome = usuario.Nome,
							nascimento = usuario.Nascimento
						}
				);
			}
			return FindLatestByUseData(usuario);
		} catch (Exception e) {
			throw new Exception("Erro ao criar o usuário: " + e.Message);
		}
	}

	public Usuario Update(Usuario usuario) {
		var comando = @"update usuario set nome = ?nome, nascimento = ?nascimento where id = ?id;";
		comando = comando.TrataComandoPorBanco(_factory.getTipoBanco());
		try {
			using (var conexao = _factory.Connect()) {
				conexao.Execute(
						comando,
						new {
							nome = usuario.Nome,
							nascimento = usuario.Nascimento,
							id = usuario.Id
						}
				);
			}
			return FindById(usuario.Id);
		} catch (Exception e) {
			throw new Exception("Erro ao criar o usuário: " + e.Message);
		}
	}

	public void Delete(int id) {
		var comando = @"delete from usuario where id = ?id;";
		comando = comando.TrataComandoPorBanco(_factory.getTipoBanco());
		try {
			using (var conexao = _factory.Connect()) {
				conexao.Execute(
						comando,
						new { id }
				);
			}
		} catch (Exception e) {
			throw new Exception("Erro ao excluir o usuário: " + e.Message);
		}
	}


	public Usuario FindLatestByUseData(Usuario usuario) {
		var comando = @"select * from usuario where nome = ?nome and nascimento = ?nascimento order by id desc";
		comando = comando.TrataComandoPorBanco(_factory.getTipoBanco());
		try {
			using (var conexao = _factory.Connect()) {
				return conexao.Query<Usuario>(
							comando,
							new {
								nome = usuario.Nome,
								nascimento = usuario.Nascimento
							}
				).First();
			}
		} catch {
			throw new Exception("Usuário não encontrado!");
		}
	}
}
