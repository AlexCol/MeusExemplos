// File: Database/DatabaseConnection.cs
using FirebirdSql.Data.FirebirdClient;

namespace CriandoScaffoldComConsole.Database;
public class DatabaseConnection {
  private readonly string _connectionString;

  public DatabaseConnection(string connectionString) {
    _connectionString = connectionString;
  }

  public FbConnection GetConnection() {
    return new FbConnection(_connectionString);
  }

  public bool TestConnection(out string message) {
    try {
      using var connection = GetConnection();
      connection.Open();
      message = "Conexão estabelecida com sucesso.";
      return true;
    } catch (Exception ex) {
      message = $"Falha ao estabelecer conexão: {ex.Message}";
      return false;
    }
  }
}