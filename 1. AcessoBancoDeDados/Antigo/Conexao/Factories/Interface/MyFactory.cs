using System.Data;
using System.Resources;
using acessoBancoDeDados.Conexao.Emuns;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;

namespace acessoBancoDeDados.Factories;

public class MySqlFactory : ConnectionFactory
{
    public MySqlFactory(IConfiguration _configuration) : base(_configuration)
    {
        tipoBanco = TipoBanco.MySql;
    }

    public override IDbConnection Connect()
    {
        string connectionString = configuration.GetConnectionString("MySql");
        connection = new MySqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}
