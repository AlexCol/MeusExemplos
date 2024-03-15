using PollyBasic.src.Delegate;

namespace PollyBasic.src.Testes;

public static class TestRetry {
  public static void Test() {
    //PollyRetry.WaitAndRetryAsync(FuncTeste.Func);
    // Console.WriteLine("-------------");
    PollyRetry.WaitAndRetry(FuncTeste.Func);
    // Console.WriteLine("-------------");
    PollyRetry.Retry(FuncTeste.Func);
  }
}
