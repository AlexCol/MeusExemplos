using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemplo1.src.Delegate {
  public static class Delegates {
    public delegate double MinhasOperacoes(double n1, double n2);
    private static MinhasOperacoes _operacoes;

    public static void AdicionaOperacao(MinhasOperacoes newOp) {
      _operacoes += newOp;
    }

    public static void ExecutaOperacoes(double n1, double n2) {
      _operacoes(n1, n2);
    }
  }
}