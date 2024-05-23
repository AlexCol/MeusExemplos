using System.Net.Sockets;
using System.Text;

namespace PrimeiroExemplo.src.Client;

public class ChatClient {
    private const int maxReconnectAttempts = 10; // Número máximo de tentativas de reconexão
    private const string serverAddress = "127.0.0.1";
    private const int port = 8888;

    public static void Run() {
        int reconnectAttempts = 0; // Contador de tentativas de reconexão
        while (true) { //! Loop infinito para tentar reconectar em caso de falha
            try {
                using TcpClient client = new TcpClient(); //! Define o cliente
                Console.WriteLine("Tentando conectar ao servidor...");
                client.Connect(serverAddress, port); //! Conecta ao servidor na porta especificada
                Console.WriteLine("Conectado ao servidor...");

                NetworkStream stream = client.GetStream(); //! Obtém a stream de rede do cliente
                Task.Run(() => ReceiveMessagesAsync(stream)); //! Cria uma tarefa para receber mensagens do servidor
                SendMessages(stream); //! Inicia o processo de envio de mensagens ao servidor
            } catch (Exception ex) {
                reconnectAttempts++;
                if (reconnectAttempts > maxReconnectAttempts) {
                    Console.WriteLine("Número máximo de tentativas de reconexão atingido. Fechando o cliente.");
                    return; //! Encerra o cliente após atingir o número máximo de tentativas
                } else {
                    Console.WriteLine($"Tentativa {reconnectAttempts}:Erro de conexão: {ex.Message}");
                    Console.WriteLine("Tentando reconectar em 5 segundos...");
                    Task.Delay(5000).Wait(); // Aguarda 5 segundos antes de tentar reconectar                
                }
            }
        }
    }

    private static async Task ReceiveMessagesAsync(NetworkStream stream) {
        byte[] buffer = new byte[1024];
        int bytesRead;

        try {
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0) {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Mensagem: " + message);
            }
        } catch (IOException io) {
            Console.WriteLine("Erro de leitura: " + io.Message + ". Digite enter para reconectar.");
        } catch (Exception ex) {
            Console.WriteLine("Erro de leitura: " + ex.Message);

        }
    }

    private static void SendMessages(NetworkStream stream) {
        try {
            while (true) {
                string message = Console.ReadLine(); //! é uma operação bloqueante, o reconect só ocorrerá após sair daqui
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        } catch (Exception ex) {
            Console.WriteLine("Erro de escrita: " + ex.Message);
        }
    }
}