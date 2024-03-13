using System.Data;

using ComConsole.src.Enums;
using ComConsole.src.Factory.Interface;
using Microsoft.Extensions.Configuration;

namespace ComConsole.src.Factory;

public abstract class ConnectionFactory : IConnectionFactory {
	//necessário principalmente para os casos que precisa tratar quando muda algo nos comandos entre bancos (ex, em mysql é @ onde no oracle é :)
	private ETipoBanco _tipoBanco;
	private readonly IConfiguration _config;
	protected IDbConnection _connection;

	//+ construtor
	public ConnectionFactory(IConfiguration config, ETipoBanco tipoBanco) {
		_config = config;
		_tipoBanco = tipoBanco;
	}

	//+ metodos abstratos
	public abstract IDbConnection Connect();


	//+ metodos concretos publicos
	public void Dispose() {
		_connection.Close();
	}

	public ETipoBanco getTipoBanco() {
		return _tipoBanco;
	}

	//+ metodos concretos protegidos
	protected string getConnectionString() {
		return _config.GetConnectionString(_tipoBanco.ToString());
	}
}
