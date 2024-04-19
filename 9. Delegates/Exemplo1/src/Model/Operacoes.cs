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
    }
}