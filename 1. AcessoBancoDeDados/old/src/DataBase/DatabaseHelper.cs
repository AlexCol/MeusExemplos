using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.DataBase;
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