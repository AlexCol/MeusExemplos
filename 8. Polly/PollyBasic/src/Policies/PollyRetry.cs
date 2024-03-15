using Polly;

namespace PollyBasic.src;

public static class PollyRetry {
  public static void Retry(FuncaoDelegate process) {
    var retryPolicy = Policy
                .Handle<DivideByZeroException>()
                .Retry(3, (exception, retryCount) => { //define que ele vai tentar reexecutar o mesmo código 3 vezes, antes que soltar o erro
                  Console.WriteLine($"Erro na tentativa {retryCount}: {exception.Message}");
                });

    try {
      retryPolicy.Execute(() => {
        process();
      });
    } catch {
      Console.WriteLine("Até Polly desistiu.");
    }
  }

  public static void WaitAndRetry(FuncaoDelegate process) {
    var retryPolicy = Policy
                .Handle<DivideByZeroException>()
                .WaitAndRetry([
                  TimeSpan.FromSeconds(1),
                  TimeSpan.FromSeconds(5),
                  TimeSpan.FromSeconds(10)
                ], (exception, timeSpan, retryCount, context) => {
                  Console.WriteLine($"Erro na tentativa {retryCount}: {exception.Message} - {DateTime.Now.ToString("HH:mm:ss")}");
                });

    try {
      retryPolicy.Execute(() => {
        process();
      });
    } catch {
      Console.WriteLine("Até Polly desistiu.");
    }
  }

  public static async void WaitAndRetryAsync(FuncaoDelegate process) {
    var retryPolicy = Policy
                .Handle<DivideByZeroException>()
                .WaitAndRetryAsync([
                  TimeSpan.FromSeconds(1),
                  TimeSpan.FromSeconds(2),
                  TimeSpan.FromSeconds(3)
                ], (exception, timeSpan, retryCount, context) => {
                  Console.WriteLine($"Erro na tentativa {retryCount}: {exception.Message}");
                });

    try {
      await retryPolicy.ExecuteAsync(async () => {
        process();
        await Task.Delay(1000);
      });
    } catch {
      Console.WriteLine("Até Polly desistiu.");
    }
  }
}
