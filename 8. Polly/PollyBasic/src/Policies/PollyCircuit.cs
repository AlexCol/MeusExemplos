using Polly;
using Polly.CircuitBreaker;

namespace PollyBasic.src;

public static class PollyCircuit {
  public static CircuitBreakerPolicy circuitPolicy = Policy
        .Handle<DivideByZeroException>()
        .CircuitBreaker(3, TimeSpan.FromSeconds(3), (exception, duration) => {
          Console.WriteLine($"Disjuntor de circuito será aberto por {duration.TotalSeconds} segundos devido aos erros: ");
        },
        () => {
          Console.WriteLine("Disjuntor de circuito fechado.");
        },
        () => {
          Console.WriteLine("Tentativa de chamada com o circuito aberto.");
        });

  public static void Circuit(FuncaoDelegateComValor process, int i) {
    try {
      circuitPolicy.Execute(() => { //é preciso deixar a policy em outro local, pois a cada chamada de ciruit, ele estaria recriando ela, e 'comando do zero'
        process(i);
      });
    } catch (BrokenCircuitException) {
      Console.WriteLine("Disjuntor aberto, e não recebe novas requisições.");
    } catch (Exception e) {
      Console.WriteLine($"Erro ao executar a operação: {e.Message}");
    }
  }

}
