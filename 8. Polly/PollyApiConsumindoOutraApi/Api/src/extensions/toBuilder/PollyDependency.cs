using Api.src.polly.policies;
using Api.src.services.httpServices;
using Polly;
using Polly.Extensions.Http;

namespace Api.src.extensions.toBuilder;

public static class PollyDependency {
  public static void AddPolly(this WebApplicationBuilder builder) {
    builder.Services.AddHttpClient<ITestApiRequests, TestApiRequests>() //! ITestApiRequests e TestApiRequests devem ter tido sua injeção já realizada
            .AddPolicyHandler((provider, request) => Policies.GetRetryPolicy(provider, request, 5, 5))
            .SetHandlerLifetime(TimeSpan.FromMinutes(10));
  }
}