using acessoBancoDeDados.Factories.Interface;
using acessoBancoDeDados.Repositorio;
using Dapper;

public class XapTesteService : MyService<XapTeste>
{
    public XapTesteService(IConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public override XapTeste buscaPorCodigo(int codigo)
    {
        return new XapTeste("Nao tem nada");
    }

    public void salvar(XapTeste item)
    {
        var comando = @"insert into xap_teste values (:campo1)";
        comando = comando.TrataComando(connectionFactory.getTipoBanco());
        using (var conexao = connectionFactory.Connect())
        {
            conexao.Execute(
                comando,
                item
            );
        }
    }

    public void deletar(XapTeste item)
    {
        var comando = @"delete from xap_teste where campo1 = :campo1";
        comando = comando.TrataComando(connectionFactory.getTipoBanco());
        using (var conexao = connectionFactory.Connect())
        {
            conexao.Execute(
                comando,
                item
            );
        }
    }

    //aqui passo dois parametros (old e new) pois meu exemplo de tabela tem um campo só, se tiver dois, só precisar passar um
    //desde que ele tenha o ID a ser atualizado, com os dados novos
    public void atualizar(XapTeste newItem, XapTeste oldItem)
    {
        var comando = @"update xap_teste set campo1 = :newCampo1 where campo1 = :oldCampo1";
        comando = comando.TrataComando(connectionFactory.getTipoBanco());
        using (var conexao = connectionFactory.Connect())
        {
            conexao.Execute(
                comando,
                new { newCampo1 = newItem.campo1, oldCampo1 = oldItem.campo1 }
            );
        }
    }
}