using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace PollyApp.src.MyPolicies;

public static class TestServicePollyNewVersion {
  public static ResiliencePipeline GetPolicy() {
    var pipeline = new ResiliencePipelineBuilder()
      .AddRetry(new RetryStrategyOptions {
        MaxRetryAttempts = 12,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Constant,
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        OnRetry = (retryArguments) => {
          string message;
          if (retryArguments.Outcome.Exception is BrokenCircuitException)
            message = "Circuito ainda aberto, não pode ser acessado.";
          else
            message = retryArguments.Outcome.Exception.Message;

          Console.WriteLine($"Erro na tentativa {retryArguments.AttemptNumber + 1}: {message} - {DateTime.Now:HH:mm:ss:ffff}");
          return ValueTask.CompletedTask;
        }
      })
      .AddCircuitBreaker(new CircuitBreakerStrategyOptions {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        FailureRatio = 0.1,
        MinimumThroughput = 2,
        BreakDuration = TimeSpan.FromSeconds(5),
        OnOpened = (circuitArg) => {
          Console.WriteLine($"Disjuntor de circuito será aberto por {circuitArg.BreakDuration} segundos devido aos erros. - {DateTime.Now:HH:mm:ss:ffff}");
          return ValueTask.CompletedTask;
        },
        OnClosed = (circuitArg) => {
          Console.WriteLine("Disjuntor fechado.");
          return ValueTask.CompletedTask;
        }

      })
      .Build();

    return pipeline;
  }
}


/*
    var circuitBreakerPolicy = Policy.Handle<Exception>()
        .CircuitBreaker(2, TimeSpan.FromSeconds(5),
        (exception, duration) => {
          Console.WriteLine($"Disjuntor de circuito será aberto por {duration.TotalSeconds} segundos devido aos erros. - {DateTime.Now:HH:mm:ss:ffff}");
        },
        () => {
          Console.WriteLine("Disjuntor fechado.");
        });*/