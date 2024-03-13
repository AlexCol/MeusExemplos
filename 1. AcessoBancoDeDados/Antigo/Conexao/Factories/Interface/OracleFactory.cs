using System.Data;
using System.Resources;
using acessoBancoDeDados.Conexao.Emuns;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace acessoBancoDeDados.Factories;

public class OracleFactory : ConnectionFactory
{
    public OracleFactory(IConfiguration _configuration) : base(_configuration)
    {
        tipoBanco = TipoBanco.Oracle;
    }

    public override IDbConnection Connect()
    {
        string connectionString = configuration.GetConnectionString("Oracle");
        connection = new OracleConnection(connectionString);
        connection.Open();
        return connection;
    }
}
