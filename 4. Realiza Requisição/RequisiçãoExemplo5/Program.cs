try
{
    //usando construtor que recebe dados diretamente
    string baseUrl = "https://localhost:7068";
    string usuario = "usuario";
    string senha = "senha";
    var client = new MeuRestClient(baseUrl, usuario, senha);

    System.Console.WriteLine(client.buscaFrase());
    System.Console.WriteLine(client.buscaFrase("45678"));

    //usando construtor que recebe config
    var client2 = new MeuRestClient(Configuracao.getConfiguracao());

    System.Console.WriteLine(client2.buscaFrase());
    System.Console.WriteLine(client2.buscaFrase("15668"));

}
catch (Exception e)
{
    System.Console.WriteLine(e.Message);
}