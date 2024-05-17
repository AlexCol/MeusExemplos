using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegundoExemplo.src.Servico;

public class OrdemCompra {
    public int CodCompra { get; set; }

    public OrdemCompra(int codCompra) {
        CodCompra = codCompra;
    }

    //! cada compra vai abrir várias Tasks para processar o pedido
    public async Task ProcessarCompraAsync() {
        Console.WriteLine($"Processo da compra {CodCompra} iniciou na Thread {Thread.CurrentThread.ManagedThreadId}.");

        List<Task> tasks = [
            VerificaEstoqueAsync(),
            ProcessaPagamentoAsync(),
            ExpedirCompraAsync()
        ];

        await Task.WhenAll(tasks.ToArray());

        Console.WriteLine($"Compra {CodCompra} processada completamente na InternalThread {Thread.CurrentThread.ManagedThreadId}.");
    }

    private async Task VerificaEstoqueAsync() {
        Console.WriteLine($"Compra {CodCompra}: Checando estoque... [InternalThread {Thread.CurrentThread.ManagedThreadId}] - {DateTime.Now:hh:mm:ss}");
        await Task.Delay(1000); // Simula o tempo de verificação de estoque
        Console.WriteLine($"Compra {CodCompra}: Produto disponível. [InternalThread {Thread.CurrentThread.ManagedThreadId}] - {DateTime.Now:hh:mm:ss}");
    }

    private async Task ProcessaPagamentoAsync() {
        Console.WriteLine($"Compra {CodCompra}: Processando pagamento... [InternalThread {Thread.CurrentThread.ManagedThreadId}] - {DateTime.Now:hh:mm:ss}");
        await Task.Delay(1500); // Simula o tempo de processamento de pagamento
        Console.WriteLine($"Compra {CodCompra}: Pagamento processado. [InternalThread {Thread.CurrentThread.ManagedThreadId}] - {DateTime.Now:hh:mm:ss}");
    }

    private async Task ExpedirCompraAsync() {
        Console.WriteLine($"Compra {CodCompra}: Enviando pedido... [InternalThread {Thread.CurrentThread.ManagedThreadId}] - {DateTime.Now:hh:mm:ss}");
        await Task.Delay(2000); // Simula o tempo de envio do pedido
        Console.WriteLine($"Compra {CodCompra}: Pedido enviado. [InternalThread {Thread.CurrentThread.ManagedThreadId}] - {DateTime.Now:hh:mm:ss}");
    }
}
