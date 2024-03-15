using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PollyBasic.src.Delegate;

namespace PollyBasic.src.Testes;

public static class TestFallback {
  public static void Test() {
    PollyFallback.Fallback(FuncTeste.Func, FuncTeste.FallbackFunc);
  }
}
