using PrimeiroExemplo.src.Client;
using PrimeiroExemplo.src.Server;

if (args.Length > 0 && args[0] == "server") {
    ChatServer.Run();
} else {
    ChatClient.Run();
}