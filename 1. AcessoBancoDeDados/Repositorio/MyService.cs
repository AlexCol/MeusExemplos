using acessoBancoDeDados.Factories.Interface;

namespace acessoBancoDeDados.Repositorio;

public abstract class MyService<T> : IMyService<T>
{
    protected readonly IConnectionFactory connectionFactory;

    public MyService(IConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }


    public abstract T buscaPorCodigo(int codigo);
}