using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace CriandoScaffoldComConsole.Database;
public static class DatabaseHelper
{
    public static DataTable ExecuteQuery(FbConnection connection, string query)
    {
        var command = new FbCommand(query, connection);
        var reader = command.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }
}