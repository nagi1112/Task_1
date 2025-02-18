using System;
using Microsoft.Data.Sqlite;

public static class Db
{
    private const string ConnectionString = "Data Source=app.db";

    public static void InsertRequest(string url, string email, int price)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            using (var createCmd = connection.CreateCommand())
            {
                createCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Requests (
                    Id    INTEGER PRIMARY KEY AUTOINCREMENT,
                    Url   TEXT,
                    Email TEXT,
                    Price INTREGER
                );
            ";
                createCmd.ExecuteNonQuery();
            }

            using (var insertCmd = connection.CreateCommand())
            {
                insertCmd.CommandText = @"
                INSERT INTO Requests (Url, Email, Price) VALUES (@url, @email, @price);
            ";
                insertCmd.Parameters.AddWithValue("@url", url);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@price", price);
                insertCmd.ExecuteNonQuery();
            }
        }
    }
}

