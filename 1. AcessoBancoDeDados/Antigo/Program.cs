using acessoBancoDeDados.Configuracao;
using acessoBancoDeDados.Entities;
using acessoBancoDeDados.Factories;
using acessoBancoDeDados.Repositorio;

Console.Clear();

//busca a configuração no arquivo appsettings, onde ficam as configurações do sistema (nesse caso, onde estão as strings de conexão)
try
{
    var configuration = Configuracao.getConfiguracao();
    Usuario u;
    XapTeste x = new XapTeste("Oi");
    XapTeste x2 = new XapTeste("Tchau");

    //Testando //!ORACLE
    var ora = new OracleFactory(configuration);
    System.Console.WriteLine("Buscando dados Oracle:");
    u = new UsuarioService(ora).buscaPorCodigo(663290015);
    System.Console.WriteLine(u);
    new XapTesteService(ora).deletar(x2);
    new XapTesteService(ora).atualizar(x2, x);
    new XapTesteService(ora).salvar(x);



    //Testando //!MYSQL
    var mysql = new MySqlFactory(configuration);
    System.Console.WriteLine("Buscando dados MySql:");
    u = new UsuarioService(mysql).buscaPorCodigo(663290015);
    System.Console.WriteLine(u);
    new XapTesteService(mysql).deletar(x2);
    new XapTesteService(mysql).atualizar(x2, x);
    new XapTesteService(mysql).salvar(x);
}
catch (Exception e)
{
    System.Console.WriteLine(e.Message);
}


System.Console.WriteLine("Finish");