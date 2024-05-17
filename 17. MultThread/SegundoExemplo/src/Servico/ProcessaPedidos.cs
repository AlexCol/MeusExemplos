using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegundoExemplo.src.Servico;

public static class ProcessaPedidos {
    public static void ProcessarOrders(int threadId, int ordersCount) {
        Console.WriteLine($"Thread Manual {threadId} iniciando.");

        //cada 'programa', vai criar N Tasks, cada uma pra processar os pedidos enviados
        List<Task> pedidos = new List<Task>();
        for (int i = 0; i < ordersCount; i++) {
            int orderId = (threadId - 1) * ordersCount + i + 1; //! pra ter numeros distintos no console
            var processor = new OrdemCompra(orderId);
            pedidos.Add(processor.ProcessarCompraAsync());
        }
        Task.WhenAll(pedidos).Wait();

        Console.WriteLine($"Thread Manual {threadId} finalizando.");
    }
}
