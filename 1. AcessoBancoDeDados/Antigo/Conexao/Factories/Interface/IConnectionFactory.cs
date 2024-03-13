using System.Data;
using acessoBancoDeDados.Conexao.Emuns;

namespace acessoBancoDeDados.Factories.Interface;

public interface IConnectionFactory
{
    IDbConnection Connect();
    void Dispose();
    TipoBanco getTipoBanco();
}
