using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace Api.src.polly.policies;

public static class Policies {
  public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceProvider provider, HttpRequestMessage request, int numberOfTries, int secondsBetweenTries) {
    return HttpPolicyExtensions.HandleTransientHttpError()
                               .OrResult(response => {
                                 Log.Error($"Passando no OrResult, o resultado dela não foi 500, mas sim: {(int)response.StatusCode}");
                                 if (response.IsSuccessStatusCode) Log.Warning("Sucesso");
                                 return !response.IsSuccessStatusCode;
                               }) // Inclui códigos 4xx e 5xx
                               .WaitAndRetryAsync(numberOfTries,
                                                  retryAttempt => TimeSpan.FromSeconds(secondsBetweenTries),
                                                  onRetry: (response, delay, retryCount, context) => {
                                                    Log.Fatal($"[GetRetryPolicy] Erro na requisição. Tentativa {retryCount}. URL: {request.RequestUri}, Status Code: {response.Result.StatusCode}");
                                                  });
  }

}
