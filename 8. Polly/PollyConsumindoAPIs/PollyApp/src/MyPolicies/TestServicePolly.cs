using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;

namespace PollyApp.src.MyPolicies;

public static class TestServicePolly {
  public static PolicyWrap GetPolicy() {
    var retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry([
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1),
          TimeSpan.FromSeconds(1)
        ], (exception, timeSpan, retryCount, context) => {
          string message;
          if (exception is BrokenCircuitException)
            message = "Circuito ainda aberto, não pode ser acessado.";
          else
            message = exception.Message;

          Console.WriteLine($"Erro na tentativa {retryCount}: {message} - {DateTime.Now:HH:mm:ss:ffff}");
        });

    var circuitBreakerPolicy = Policy.Handle<Exception>()
        .CircuitBreaker(2, TimeSpan.FromSeconds(5),
        (exception, duration) => {
          Console.WriteLine($"Disjuntor de circuito será aberto por {duration.TotalSeconds} segundos devido aos erros. - {DateTime.Now:HH:mm:ss:ffff}");
        },
        () => {
          Console.WriteLine("Disjuntor fechado.");
        });
    return Policy.Wrap(retryPolicy, circuitBreakerPolicy);
  }
}
