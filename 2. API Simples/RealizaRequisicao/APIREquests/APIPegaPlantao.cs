using RestSharp;

class APIPegaPlantao
{
    public string AuthToken { get; private set; }
    private RestClient client;
    protected readonly PegaPlantaoConfiguration pegaPlantaoConfiguration;

    public APIPegaPlantao(PegaPlantaoConfiguration _pegaPlantaoConfiguration)
    {
        pegaPlantaoConfiguration = _pegaPlantaoConfiguration;
    }

    public void Connect()
    {
        //preparando client
        System.Console.WriteLine("Iniciando a conexão com Pega Plantão");

        var options = new RestClientOptions(pegaPlantaoConfiguration.BaseUrl);

        client = new RestClient(options);

        client.AddDefaultHeader("Authentication", "Basic " + pegaPlantaoConfiguration.Token);

        GetAuthToken();
    }
    private void GetAuthToken()
    {
        System.Console.WriteLine("Autenticando com Pega Plantão");

        try
        {
            var request = new RestRequest(pegaPlantaoConfiguration.UrlToken, Method.Post);

            request.AddHeader("Authorization", "Basic " + pegaPlantaoConfiguration.Token);

            request.AddParameter("username", pegaPlantaoConfiguration.Username);
            request.AddParameter("password", pegaPlantaoConfiguration.Password);
            request.AddParameter("grant_type", "password");

            var response = client.Post<AuthResponse>(request);
            AuthToken = response.access_token;

            System.Console.WriteLine("Autenticado com sucesso: " + AuthToken);

        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public List<Doctor> GetProfissionais()
    {
        System.Console.WriteLine("Recupando profissionais da Api Pega Plantão");

        var request = new RestRequest(pegaPlantaoConfiguration.UrlProfissionais, Method.Get);

        request.AddHeader("Authorization", "Bearer " + AuthToken);

        var response = client.Get<List<Doctor>>(request);

        return response;
    }

}