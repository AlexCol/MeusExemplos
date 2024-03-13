namespace acessoBancoDeDados.Repositorio;

public interface IMyService<T>
{
    public T buscaPorCodigo(int codigo);
}