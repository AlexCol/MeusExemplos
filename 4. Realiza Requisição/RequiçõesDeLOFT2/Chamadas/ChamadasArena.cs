using System.Net;
using Newtonsoft.Json;
using RestSharp;

public class ChamadasArena
{
    private RestClientOptions options;
    private RestClient client;

    public ChamadasArena(string baseUrl)
    {
        options = new RestClientOptions(baseUrl);
        client = new RestClient(options);
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

    public void combate(PersonagemResponse? p1, PersonagemResponse? p2)
    {

        int id1 = p1 != null ? p1.id : 0;
        int id2 = p2 != null ? p2.id : 0;

        RestRequest request = new RestRequest($"api/arena/{id1}/{id2}", Method.Put);
        RestResponse<List<string>> response = client.ExecutePut<List<string>>(request);

        System.Console.WriteLine("StatusCode: " + response.StatusCode);

        if (response.Data != null)
        {
            List<string>? lista = response.Data;
            foreach (var item in lista)
                System.Console.WriteLine(item);
        }
        else
        {
            System.Console.WriteLine(response.Content);
        }
    }
}