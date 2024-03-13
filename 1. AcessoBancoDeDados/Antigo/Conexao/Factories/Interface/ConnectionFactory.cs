using System.Data;
using acessoBancoDeDados.Conexao.Emuns;
using acessoBancoDeDados.Factories.Interface;
using Microsoft.Extensions.Configuration;

namespace acessoBancoDeDados.Factories;

public abstract class ConnectionFactory : IConnectionFactory
{
    protected readonly IConfiguration configuration;
    protected IDbConnection connection;
    protected TipoBanco tipoBanco;

    public ConnectionFactory(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public abstract IDbConnection Connect();

    public void Dispose()
    {
        connection.Close();
    }

    public TipoBanco getTipoBanco()
    {
        return tipoBanco;
    }
}
