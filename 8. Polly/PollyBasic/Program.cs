using PollyBasic.src;
using PollyBasic.src.Delegate;


//+ exemplos com retries limitados (todos configurados a 3x antes de desistir)
// PollyRetry.WaitAndRetryAsync(FuncTeste.Func);
// Console.WriteLine("-------------");
// PollyRetry.WaitAndRetry(FuncTeste.Func);
// Console.WriteLine("-------------");
// PollyRetry.Retry(FuncTeste.Func);

//PollyFallback.Fallback(FuncTeste.Func, FuncTeste.FallbackFunc);

PollyCircuit.CircuitAsync(FuncTeste.Func);
PollyCircuit.CircuitAsync(FuncTeste.Func);
PollyCircuit.CircuitAsync(FuncTeste.Func);
PollyCircuit.CircuitAsync(FuncTeste.Func);
PollyCircuit.CircuitAsync(FuncTeste.Func);

// Console.WriteLine("Executando 1ª:");
// PollyCircuit.Circuit(FuncTeste.Func);
// Console.WriteLine("Executando 2ª:");
// PollyCircuit.Circuit(FuncTeste.Func);
// Console.WriteLine("Executando 3ª:");
// PollyCircuit.Circuit(FuncTeste.Func);
// Console.WriteLine("Executando 4ª:");
// PollyCircuit.Circuit(FuncTeste.Func);

Thread.Sleep(5000);

// Console.WriteLine("Executando 5ª:");
// PollyCircuit.Circuit(FuncTeste.Func);