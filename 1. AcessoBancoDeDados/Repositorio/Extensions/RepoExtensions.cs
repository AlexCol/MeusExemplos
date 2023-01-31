using acessoBancoDeDados.Conexao.Emuns;

public static class RepoExtensions
{
    public static string TrataComando(this string comando, TipoBanco tipoBanco)
    {
        if (tipoBanco == TipoBanco.MySql)
        {
            return comando.Replace(":", "@");
        }
        else
        {
            return comando;
        }
    }
}