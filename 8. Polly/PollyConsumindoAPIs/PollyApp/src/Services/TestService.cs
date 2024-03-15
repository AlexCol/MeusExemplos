using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace PollyApp.src.Services;

public class TestService {
  private readonly string _baseUrl;

  public TestService(string baseUrl) {
    _baseUrl = baseUrl;
  }

  public string Test() {
    RestClientOptions options = new RestClientOptions(_baseUrl);
    RestClient client = new RestClient(options);
    RestRequest request = new RestRequest("/test");
    RestResponse response = client.ExecuteGet(request);
    if (!(response.StatusCode >= HttpStatusCode.OK && response.StatusCode <= (HttpStatusCode)299)) {
      if (response.ErrorMessage != null)
        throw new Exception(response.ErrorMessage);
      else
        throw new Exception(response.Content);
    };
    return response.Content;
  }
}
