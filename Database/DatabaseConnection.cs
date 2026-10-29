using System;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace FootballScout.Database
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection()
        {
            // Connection string padrão - pode ser sobrescrita via variável de ambiente
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "football_scout";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? string.Empty;

            _connectionString = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password};SslMode=none;AllowPublicKeyRetrieval=true;";
        }

        public MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}