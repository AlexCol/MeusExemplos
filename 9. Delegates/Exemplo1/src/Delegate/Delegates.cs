using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemplo1.src.Delegate {
    public class Delegates {
        private delegate double MinhasOperacoes(double n1, double n2);
        private MinhasOperacoes _operacoes;

        private void AdicionaOperacao(MinhasOperacoes novaOp) {
            _operacoes += novaOp;
        }
        public void ExecutaOperacoes(double n1, double n2) {
            _operacoes(n1, n2);
        }
    }
}