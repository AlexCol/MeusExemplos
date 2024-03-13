using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ComConsole.src.Enums;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Npgsql.Internal;

namespace ComConsole.src.Factory.Factories;

public class PostgresFactory : ConnectionFactory {
	public PostgresFactory(IConfiguration config) : base(config, ETipoBanco.Postgres) { }

	public override IDbConnection Connect() {
		string connectionString = getConnectionString();
		_connection = new NpgsqlConnection(connectionString);
		_connection.Open();
		return _connection;
	}
}
