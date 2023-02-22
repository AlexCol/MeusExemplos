using System.Net;
using Newtonsoft.Json;
using RestSharp;

public class ChamadasPersonagens
{
    private RestClientOptions options;
    private RestClient client;

    public ChamadasPersonagens(string baseUrl)
    {
        options = new RestClientOptions(baseUrl);
        client = new RestClient(options);
    }

    public void criaPersonagem(string nome, string classe)
    {
        RestRequest request = new RestRequest("api/personagem", Method.Post);
        request.AddBody(new { Nome = nome, Classe = classe }, "text/json");
        RestResponse response = client.ExecutePost(request);
        System.Console.WriteLine($"StatusCode: {response.StatusCode} = {response.Content}");
    }

    public void listaPersonagemPorId(int id)
    {
        RestRequest request = new RestRequest($"api/personagem/{id}", Method.Get);

        RestResponse response = client.ExecuteGet(request);
        PersonagemResponse? personagem = null;
        if (response.Content != null && response.StatusCode == HttpStatusCode.OK)
            personagem = JsonConvert.DeserializeObject<PersonagemResponse>(response.Content);

        System.Console.WriteLine($"StatusCode: {response.StatusCode} = {personagem}");
        //return personagem;
    }

    public void listaPersonagemPorId2(int id)
    {
        RestRequest request = new RestRequest($"api/personagem/{id}", Method.Get);
        RestResponse<PersonagemResponse> response = client.ExecuteGet<PersonagemResponse>(request);
        System.Console.WriteLine($"StatusCode: {response.StatusCode}: {response.Data}");
    }

    public PersonagemResponse? retornaPersonagemPorId(int id)
    {
        RestRequest request = new RestRequest($"api/personagem/{id}", Method.Get);
        RestResponse<PersonagemResponse> response = client.ExecuteGet<PersonagemResponse>(request);
        return response.Data;
    }

    public void listaTodos()
    {
        RestRequest request = new RestRequest($"api/personagem", Method.Get);
        RestResponse<List<ListaDePersonagensResponse>> response = client.ExecuteGet<List<ListaDePersonagensResponse>>(request);
        if (response.Data != null)
        {
            List<ListaDePersonagensResponse>? lista = response.Data;

            foreach (var item in lista)
            {
                System.Console.WriteLine($"{item.nome} - {item.classe} : {item.aliveOrDead}");
            }
        }
    }

    public void combate(int p1, int p2)
    {

    }
}