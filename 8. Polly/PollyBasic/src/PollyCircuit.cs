using Polly;
using Polly.CircuitBreaker;

namespace PollyBasic.src;

public static class PollyCircuit {
  public static CircuitBreakerPolicy circuitPolicy = Policy
        .Handle<DivideByZeroException>()
        .CircuitBreaker(3, TimeSpan.FromSeconds(3), (exception, duration) => {
          Console.WriteLine($"Disjuntor de circuito aberto por {duration.TotalSeconds} segundos. Erro: {exception.Message}");
        },
        () => {
          Console.WriteLine("Disjuntor de circuito fechado.");
        },
        () => {
          Console.WriteLine("Tentativa de chamada com o circuito aberto.");
        });

  public static void Circuit(FuncaoDelegate process) {
    try {
      circuitPolicy.Execute(() => { //é preciso deixar a policy em outro local, pois a cada chamada de ciruit, ele estaria recriando ela, e 'comando do zero'
        Console.WriteLine("try");
        process();
      });
    } catch (Exception e) {
      Console.WriteLine($"Erro ao executar a operação: {e.Message}");
    }
  }


  public static AsyncCircuitBreakerPolicy circuitPolicyAsync = Policy
      .Handle<DivideByZeroException>()
      .CircuitBreakerAsync(3, TimeSpan.FromSeconds(3), (exception, duration) => {
        Console.WriteLine($"Disjuntor de circuito aberto por {duration.TotalSeconds} segundos. Erro: {exception.Message}");
      }, () => {
        Console.WriteLine("Disjuntor de circuito fechado.");
      });

  public static async void CircuitAsync(FuncaoDelegate process) {
    await circuitPolicyAsync.ExecuteAsync(async () => {
      try {
        Console.WriteLine("try");
        process();
        await Task.Delay(1000);
      } catch {
        Console.WriteLine("Erro.");
      }
    });
  }


}
