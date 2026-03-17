using Microsoft.Data.Sqlite;

namespace PizzariaFramework.Data;

public static class DatabaseConfig
{
    // O banco será um arquivo chamado 'pizzaria.db' na pasta da aplicação
    private const string ConnectionString = "Data Source=pizzaria.db";

    public static SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}