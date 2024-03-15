using PollyBasic.src.Delegate;

namespace PollyBasic.src.Testes;

public static class TestCiruit {
  public static void Test() {
    Console.WriteLine("Executando 1ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 0);
    Console.WriteLine("Executando 2ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 0);
    Console.WriteLine("Executando 3ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 0);
    Console.WriteLine("Executando 4ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 1);

    Thread.Sleep(5000);

    Console.WriteLine("Executando 5ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 1);
    Console.WriteLine("Executando 6ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 1);
    Console.WriteLine("Executando 7ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 0);
    Console.WriteLine("Executando 8ª:");
    PollyCircuit.Circuit(FuncTeste.TesteValor, 1);
  }
}
