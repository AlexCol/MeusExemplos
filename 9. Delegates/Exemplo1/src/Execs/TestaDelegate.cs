using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exemplo1.src.Delegate;
using Exemplo1.src.Model;

namespace Exemplo1.src.Execs {
  public static class TestaDelegate {
    public static void Run() {
      Delegates.AdicionaOperacao(Operacoes.Soma);
      Delegates.AdicionaOperacao(Operacoes.Subtracao);
      Delegates.AdicionaOperacao(Operacoes.Multiplicacao);
      Delegates.AdicionaOperacao(Operacoes.Divisao);
      Delegates.ExecutaOperacoes(4, 0);
    }
  }
}