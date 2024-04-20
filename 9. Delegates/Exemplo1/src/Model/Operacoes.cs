using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemplo1.src.Model {
  public static class Operacoes {
    public static double Soma(double n1, double n2) {
      var resultado = n1 + n2;
      Console.WriteLine($"Soma executada, e {n1} + {n2} é igual a {resultado}");
      return resultado;
    }

    public static double Subtracao(double n1, double n2) {
      var resultado = n1 - n2;
      Console.WriteLine($"Subtração executada, e {n1} - {n2} é igual a {resultado}");
      return resultado;
    }

    public static double Multiplicacao(double n1, double n2) {
      var resultado = n1 * n2;
      Console.WriteLine($"Multiplicação executada, e {n1} * {n2} é igual a {resultado}");
      return resultado;
    }

    public static double Divisao(double n1, double n2) {
      if (n2 == 0) {
        Console.WriteLine("Divisão inválida. Não se pode dividir por zero.");
        return 0;
      }

      var resultado = n1 / n2;
      Console.WriteLine($"Divisão executada, e {n1} / {n2} é igual a {resultado}");
      return resultado;
    }
  }
}