using Polly;

namespace PollyBasic.src;

public static class PollyFallback {
  public static void Fallback(FuncaoDelegate process, FuncaoDelegate fallBackProcess) {
    var fallbackPolicy = Policy
        .Handle<DivideByZeroException>()
        .Fallback(() => {
          fallBackProcess();
        });

    try {
      fallbackPolicy.Execute(() => {
        process();
      });
    } catch {
      Console.WriteLine("Até fallback deu ruim."); //a minha fallback não dá erro, mas se desse, ai cairia no catch mais proximo, que no caso é esse
    }
  }
}
