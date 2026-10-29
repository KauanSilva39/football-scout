using System;
using MySqlConnector;

namespace FootballScout.Database
{
    public class DatabaseInitializer
    {
        public DatabaseInitializer() { }

        public bool VerificarConexao()
        {
            try
            {
                // Verifica se consegue conectar ao banco
                var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
                var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
                var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "football_scout";
                var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? string.Empty;

                var connectionString = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password};SslMode=none;AllowPublicKeyRetrieval=true;Connection Timeout=30;";
                Console.WriteLine("Verificando conexão com banco de dados...");

                using var connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("✓ Conexão com banco de dados estabelecida com sucesso!");

                // Verifica se as tabelas existem
                using var cmd = new MySqlCommand("SHOW TABLES;", connection);
                using var reader = cmd.ExecuteReader();
                Console.WriteLine("Tabelas encontradas:");
                int count = 0;
                while (reader.Read())
                {
                    Console.WriteLine($"  - {reader.GetString(0)}");
                    count++;
                }

                if (count == 0)
                {
                    Console.WriteLine("⚠  Nenhuma tabela encontrada. Execute Database/Schema.sql no MySQL Workbench");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Erro ao conectar com banco de dados: {ex.Message}");
                Console.WriteLine("Certifique-se de que:");
                Console.WriteLine("  1. MySQL está instalado e rodando");
                Console.WriteLine("  2. Banco 'football_scout' foi criado");
                Console.WriteLine("  3. Script Database/Schema.sql foi executado");
                Console.WriteLine("  4. Variáveis de ambiente estão configuradas");
                return false;
            }
        }
    }
}