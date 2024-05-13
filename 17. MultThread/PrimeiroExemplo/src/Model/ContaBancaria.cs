using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiroExemplo.src.Model;

public class ContaBancaria {
    private object saldoLock = new object();
    private decimal saldo;

    public ContaBancaria(decimal saldoInicial) {
        saldo = saldoInicial;
    }

    public decimal Saldo {
        get { return saldo; }
    }

    public void Sacar(decimal valor, string usuario) {
        lock (saldoLock) {
            if (saldo >= valor) {
                Console.WriteLine($"{usuario} está sacando {valor:C}. Saldo anterior: {saldo:C}.");
                saldo -= valor;
                Console.WriteLine($"Novo saldo após o saque: {saldo:C}.");
            } else {
                Console.WriteLine($"{usuario}: Saldo insuficiente para sacar {valor:C}. Saldo atual: {saldo:C}.");
            }
        }
    }
}
