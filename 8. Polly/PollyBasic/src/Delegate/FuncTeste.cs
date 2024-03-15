using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyBasic.src.Delegate;

public static class FuncTeste {
  public static void Func() {
    try {
      for (int tries = 0; tries < 2; tries++) {
        int.TryParse(DateTime.Now.ToString("ffff"), out int div);
        div = 0;
        Console.WriteLine($"A divisão é: {1 / (div % 2)}");
      }
      Console.WriteLine("Chegou ao fim com sucesso");
    } catch {
      throw;
    }
  }

  public static void FallbackFunc() {
    Console.WriteLine("Fallback activated");
  }
}
