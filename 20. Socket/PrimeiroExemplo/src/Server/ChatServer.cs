using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PrimeiroExemplo.src.Server;

public static class ChatServer {
    private static List<TcpClient> clients = new List<TcpClient>();
    private static object lockObject = new object();
    private const int port = 8888;

    public static void Run() {
        TcpListener listener = new TcpListener(IPAddress.Any, port); //! Cria o servidor que vai ficar ouvindo por mensagens de clientes
        listener.Start(); //! Inicia o servidor
        Console.WriteLine("Servidor iniciado...");

        while (true) { //! Fica em um laço infinito até encerrar o processo
            TcpClient client = listener.AcceptTcpClient(); //! Aguardando novas conexões de clientes
            lock (lockObject) { //! Lock para adicionar clientes à lista de forma segura
                clients.Add(client); //! Adiciona o novo cliente à lista
            }
            Console.WriteLine("Cliente conectado...");
            Task.Run(() => HandleClientAsync(client)); //! Cria uma tarefa para lidar com o novo cliente de forma assíncrona
        }
    }

    private static async Task HandleClientAsync(TcpClient client) {
        NetworkStream stream = client.GetStream(); //! Obtém a stream de rede do cliente
        byte[] buffer = new byte[1024];
        int bytesRead;

        try {
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0) { //! Aguarda mensagens dos clientes
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Recebido: " + message);
                BroadcastMessage(message, client); //! Dispara a mensagem recebida para todos os clientes conectados
            }
        } catch (Exception ex) {
            Console.WriteLine("Erro: " + ex.Message);
        } finally {
            lock (lockObject) {
                clients.Remove(client); //! Remove o cliente da lista ao desconectar
            }
            client.Close();
            Console.WriteLine("Cliente desconectado...");
        }
    }

    private static void BroadcastMessage(string message, TcpClient excludeClient) {
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        lock (lockObject) {
            foreach (var client in clients) { //! Envia a mensagem para todos os clientes
                if (client != excludeClient) { //! Ignora o cliente que enviou a mensagem
                    NetworkStream stream = client.GetStream(); //! Obtém a stream de cada cliente
                    stream.WriteAsync(buffer, 0, buffer.Length); //! Envia a mensagem
                }
            }
        }
    }
}
