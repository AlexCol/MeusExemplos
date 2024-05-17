using SegundoExemplo.src.Servico;

int numberoDeThreads = 2;
int pedidosPorThread = 2;

Thread[] threads = new Thread[numberoDeThreads];

//a Main cria N Threads, e cada uma delas assume o processamento de suas OrdensDeCompra, 
//como se fossem programas indepententes
for (int i = 0; i < numberoDeThreads; i++) {
    int threadIndex = i;
    threads[threadIndex] = new Thread(() => ProcessaPedidos.ProcessarOrders(threadIndex + 1, pedidosPorThread));
    threads[threadIndex].Start();
}


//faz com que a program não avance enquanto todas as Threads não encerrarem
//importante caso deseje que não seja encerrado a Program 
//(se tirar o foreach o console abaixo vai rodar logo de cara, indicando fim da Program)
foreach (var thread in threads) {
    thread.Join();
}

Console.WriteLine("ProgramFinalizou");