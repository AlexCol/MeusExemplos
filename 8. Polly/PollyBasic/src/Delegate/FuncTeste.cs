using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyBasic.src.Delegate;

public static class FuncTeste {
  public static void Func() {
    for (int tries = 0; tries < 2; tries++) {
      int.TryParse(DateTime.Now.ToString("ffff"), out int div);
      div = 2;
      Console.WriteLine($"A divisão é: {1 / (div % 2)}");
    }
    Console.WriteLine("Chegou ao fim com sucesso");
  }

  public static void FallbackFunc() {
    Console.WriteLine("Fallback activated");
  }

  public static void TesteValor(int i) {
    if (i == 0) {
      throw new DivideByZeroException("Zero dá erro.");
    }
    Console.WriteLine("Deu certo");
  }
}
