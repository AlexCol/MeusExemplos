namespace Api.src.services.httpServices;

public interface ITestApiRequests {
  Task<string> SendAsync(string uri);
}
public class TestApiRequests : ITestApiRequests {
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _configuration;

  //private readonly IServiceProvider _services;
  private bool _configured = false;

  public TestApiRequests(
    HttpClient httpClient
    , IConfiguration configuration
  //, IServiceProvider services
  ) {
    _httpClient = httpClient;
    _configuration = configuration;
    //_services = services;
  }

  public async Task<string> SendAsync(string uri) {
    ConfigureHttpClient();
    var request = new HttpRequestMessage();
    request.Method = HttpMethod.Get;
    request.RequestUri = new Uri(_httpClient.BaseAddress!, uri);
    var response = await _httpClient.SendAsync(request);
    return $"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
  }

  private void ConfigureHttpClient() {
    if (_configured)
      return;
    /*
    ! para requisições com autenticação, precisaria injetar o serviço responsável pelo controle e armazenamento do jwt, para adicionar
    ! ao trecho comentaro abaixo
    */

    _httpClient.BaseAddress = new Uri(_configuration["BaseUrl"]!);
    _httpClient.Timeout = TimeSpan.FromMinutes(10);
    _httpClient.DefaultRequestHeaders.Accept.Clear();
    _httpClient.DefaultRequestHeaders.Clear();
    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain;q=0.9, text/html;q=0.8,");
    _httpClient.DefaultRequestHeaders.Add("AcceptCharset", "UTF-8, *;q=0.8");
    _httpClient.DefaultRequestHeaders.Add("Accept-Language", "pt-BR");
    _httpClient.DefaultRequestHeaders.Add("x-app-type", "cs-server");
    //? _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authorization.Token}");
    _configured = true;
  }
}
