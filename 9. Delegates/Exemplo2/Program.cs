public class MinhaClasse {
  private readonly SemaphoreSlim _semaphore = new(1, 1);

  public async Task MetodoGenericoAsync<T>(T valor) /* where T : class */
  {
    await ExecutarComControleAsync(async (param) => {
      Console.WriteLine($"Método iniciou com valor: {param}");
      await Task.Delay(1500);
      Console.WriteLine($"Método finalizou com valor: {param}");
    }, valor);
  }

  private async Task ExecutarComControleAsync<T>(Func<T, Task> metodo, T parametro) {
    await _semaphore.WaitAsync();
    try {
      await metodo(parametro);
    } finally {
      _semaphore.Release();
    }
  }
}

class Program {
  static async Task Main() {
    var obj = new MinhaClasse();

    var tarefas = new[]
    {
            obj.MetodoGenericoAsync(42),              // int
            obj.MetodoGenericoAsync("Hello"),         // string
            obj.MetodoGenericoAsync(3.14),            // double
            obj.MetodoGenericoAsync(DateTime.Now)     // DateTime
        };

    await Task.WhenAll(tarefas);
  }
}
