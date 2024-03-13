using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ComConsole.src.Enums;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace ComConsole.src.Factory.Factories;

public class FirebirdFactory : ConnectionFactory {
	public FirebirdFactory(IConfiguration config) : base(config, ETipoBanco.Firebird) { }

	public override IDbConnection Connect() {
		string connectionString = getConnectionString();
		_connection = new FbConnection(connectionString);
		_connection.Open();
		return _connection;
	}
}
