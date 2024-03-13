
using acessoBancoDeDados.Entities;
using acessoBancoDeDados.Factories.Interface;
using Dapper;

namespace acessoBancoDeDados.Repositorio;

class UsuarioService : MyService<Usuario>
{
    public UsuarioService(IConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public override Usuario buscaPorCodigo(int codigo)
    {
        var comando = @"select cd_matricula matricula, nm_segurado nome from usuario where cd_matricula = :codigo";
        comando = comando.TrataComando(connectionFactory.getTipoBanco());
        using (var conexao = connectionFactory.Connect())
        {
            return conexao.Query<Usuario>(
                comando,
                new { codigo }
            ).First();
        }
    }
}