using System;
using System.Threading;
using PrimeiroExemplo.src.Model;

namespace PrimeiroExemplo.src.Teste {
    public static class MeuTeste {
        public static void Run() {
            ManualResetEvent startSignal = new ManualResetEvent(false);

            ContaBancaria conta = new ContaBancaria(5.45m);

            Thread t1 = new Thread(() => RealizarSaque(conta, 1m, "Fulano", startSignal));
            Thread t2 = new Thread(() => RealizarSaque(conta, 1m, "Ciclano", startSignal));

            // Inicia as threads
            t1.Start();
            t2.Start();

            // Define o sinal para iniciar as threads
            startSignal.Set();

            // Espera que ambas as threads terminem
            t1.Join();
            t2.Join();

            Console.WriteLine($"Saldo final: {conta.Saldo:C}.");
        }

        private static void RealizarSaque(ContaBancaria conta, decimal valor, string usuario, ManualResetEvent startSignal) {
            startSignal.WaitOne();
            while (conta.Saldo >= valor) {
                conta.Sacar(valor, usuario);
                Thread.Sleep(100); // Simula um pequeno atraso entre os saques
            }
        }
    }
}
