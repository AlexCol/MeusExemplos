using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;


public class MeuRestClient
{
    private readonly RestClientOptions options;
    private readonly RestClient client;

    private readonly string Usuario;
    private readonly string Senha;
    private string Token;

    public MeuRestClient(string baseUrl, string userName, string password)
    {
        options = new RestClientOptions(baseUrl);
        client = new RestClient(options);
        Usuario = userName;
        Senha = password;
    }
    public MeuRestClient(IConfiguration configuration)
    {
        options = new RestClientOptions(configuration["DadosConexao:BaseUrl"]);
        client = new RestClient(options);
        Usuario = configuration["DadosConexao:User"];
        Senha = configuration["DadosConexao:Password"];
    }

    private void buscaToken()
    {
        RestRequest requestToken = new RestRequest("api/token/login", Method.Post);
        requestToken.AddBody(new { Username = this.Usuario, Password = this.Senha }, "text/json");
        RestResponse responseToken = client.ExecutePost(requestToken);

        if (responseToken.StatusCode == HttpStatusCode.Unauthorized)
            throw new Exception("Não autorizado");

        var content = (JObject)JsonConvert.DeserializeObject(responseToken.Content);
        var token = content.SelectToken("token").Value<string>();
        this.Token = token;

        System.Console.WriteLine("Token resitado com sucesso.");
    }

    public string buscaFrase()
    {
        this.buscaToken();
        RestRequest requestFrase = new RestRequest($"api/frases", Method.Get);
        requestFrase.AddHeader("Authorization", $"Bearer {this.Token}");

        RestResponse<string> response = client.ExecuteGet<string>(requestFrase);

        return response.Data;
    }

    public string buscaFrase(string param)
    {
        this.buscaToken();
        RestRequest requestFrase = new RestRequest($"api/frases/{param}", Method.Get);
        requestFrase.AddHeader("Authorization", $"Bearer {this.Token}");

        RestResponse<string> response = client.ExecuteGet<string>(requestFrase);

        return response.Data;
    }




}