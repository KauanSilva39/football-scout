using System;
using System.Collections.Generic;
using System.Data;
using FootballScout.Database;
using FootballScout.Models;
using MySqlConnector;

namespace FootballScout.Repositories
{
    public class JogadorRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public JogadorRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Inserir jogador no banco
        public bool Inserir(Jogador jogador, out string mensagemErro)
        {
            mensagemErro = "";

            try
            {
                using var connection = _dbConnection.GetConnection();
                using var transaction = connection.BeginTransaction();

                // Inserir jogador
                string sqlJogador = @"
                    INSERT INTO Jogadores (Nome, Idade, Nacionalidade, Posicao, PeDominante, ClubeAtual, ValorMercadoMilhoes, Moeda)
                    VALUES (@Nome, @Idade, @Nacionalidade, @Posicao, @PeDominante, @ClubeAtual, @ValorMercadoMilhoes, @Moeda);
                    SELECT LAST_INSERT_ID();";

                using var cmdJogador = new MySqlCommand(sqlJogador, connection, transaction);
                cmdJogador.Parameters.AddWithValue("@Nome", jogador.Nome);
                cmdJogador.Parameters.AddWithValue("@Idade", jogador.Idade);
                cmdJogador.Parameters.AddWithValue("@Nacionalidade", jogador.Nacionalidade);
                cmdJogador.Parameters.AddWithValue("@Posicao", jogador.Posicao);
                cmdJogador.Parameters.AddWithValue("@PeDominante", jogador.PeDominante);
                cmdJogador.Parameters.AddWithValue("@ClubeAtual", jogador.ClubeAtual);
                cmdJogador.Parameters.AddWithValue("@ValorMercadoMilhoes", jogador.ValorDeMercado.ValorEmMilhoes);
                cmdJogador.Parameters.AddWithValue("@Moeda", jogador.ValorDeMercado.Moeda.ToString());

                var result = cmdJogador.ExecuteScalar();
                int novoId = Convert.ToInt32(result);
                jogador.Id = novoId;

                // Inserir estatísticas se existirem
                if (jogador.Estatisticas != null)
                {
                    InserirEstatisticas(connection, transaction, novoId, jogador.Estatisticas);
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro ao cadastrar jogador: {ex.Message}";
                return false;
            }
        }

        private void InserirEstatisticas(MySqlConnection connection, MySqlTransaction transaction, int jogadorId, EstatisticasJogador stats)
        {
            string sqlStats = @"
                INSERT INTO Estatisticas (JogadorId, Jogos, Gols, Assistencias, GolsSofridos, CleanSheets)
                VALUES (@JogadorId, @Jogos, @Gols, @Assistencias, @GolsSofridos, @CleanSheets);";

            using var cmdStats = new MySqlCommand(sqlStats, connection, transaction);
            cmdStats.Parameters.AddWithValue("@JogadorId", jogadorId);
            cmdStats.Parameters.AddWithValue("@Jogos", stats.Jogos);
            cmdStats.Parameters.AddWithValue("@Gols", stats.Gols);
            cmdStats.Parameters.AddWithValue("@Assistencias", stats.Assistencias);
            cmdStats.Parameters.AddWithValue("@GolsSofridos", stats.GolsSofridos.HasValue ? stats.GolsSofridos.Value : DBNull.Value);
            cmdStats.Parameters.AddWithValue("@CleanSheets", stats.CleanSheets.HasValue ? stats.CleanSheets.Value : DBNull.Value);

            cmdStats.ExecuteNonQuery();
        }

        // Listar todos os jogadores com estatísticas
        public List<Jogador> ListarTodos()
        {
            var jogadores = new List<Jogador>();

            try
            {
                using var connection = _dbConnection.GetConnection();
                string sql = @"
                    SELECT j.*, e.Jogos, e.Gols, e.Assistencias, e.GolsSofridos, e.CleanSheets
                    FROM Jogadores j
                    LEFT JOIN Estatisticas e ON j.Id = e.JogadorId
                    ORDER BY j.Id;";

                using var cmd = new MySqlCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var jogador = MapearJogador(reader);
                    jogadores.Add(jogador);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar jogadores: {ex.Message}");
            }

            return jogadores;
        }

        // Buscar por ID
        public Jogador BuscarPorId(int id)
        {
            try
            {
                using var connection = _dbConnection.GetConnection();
                string sql = @"
                    SELECT j.*, e.Jogos, e.Gols, e.Assistencias, e.GolsSofridos, e.CleanSheets
                    FROM Jogadores j
                    LEFT JOIN Estatisticas e ON j.Id = e.JogadorId
                    WHERE j.Id = @Id;";

                using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Id", id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return MapearJogador(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar jogador: {ex.Message}");
            }

            return null;
        }

        // Buscar por nome (parcial, case-insensitive)
        public List<Jogador> BuscarPorNome(string nome)
        {
            var jogadores = new List<Jogador>();

            try
            {
                using var connection = _dbConnection.GetConnection();
                string sql = @"
                    SELECT j.*, e.Jogos, e.Gols, e.Assistencias, e.GolsSofridos, e.CleanSheets
                    FROM Jogadores j
                    LEFT JOIN Estatisticas e ON j.Id = e.JogadorId
                    WHERE LOWER(j.Nome) LIKE LOWER(@Nome)
                    ORDER BY j.Id;";

                using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Nome", $"%{nome}%");

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var jogador = MapearJogador(reader);
                    jogadores.Add(jogador);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar jogadores: {ex.Message}");
            }

            return jogadores;
        }

        // Atualizar jogador
        public bool Atualizar(Jogador jogador, out string mensagemErro)
        {
            mensagemErro = "";

            try
            {
                using var connection = _dbConnection.GetConnection();
                using var transaction = connection.BeginTransaction();

                // Atualizar jogador
                string sqlJogador = @"
                    UPDATE Jogadores
                    SET Nome = @Nome, Idade = @Idade, Nacionalidade = @Nacionalidade,
                        Posicao = @Posicao, PeDominante = @PeDominante, ClubeAtual = @ClubeAtual,
                        ValorMercadoMilhoes = @ValorMercadoMilhoes, Moeda = @Moeda
                    WHERE Id = @Id;";

                using var cmdJogador = new MySqlCommand(sqlJogador, connection, transaction);
                cmdJogador.Parameters.AddWithValue("@Id", jogador.Id);
                cmdJogador.Parameters.AddWithValue("@Nome", jogador.Nome);
                cmdJogador.Parameters.AddWithValue("@Idade", jogador.Idade);
                cmdJogador.Parameters.AddWithValue("@Nacionalidade", jogador.Nacionalidade);
                cmdJogador.Parameters.AddWithValue("@Posicao", jogador.Posicao);
                cmdJogador.Parameters.AddWithValue("@PeDominante", jogador.PeDominante);
                cmdJogador.Parameters.AddWithValue("@ClubeAtual", jogador.ClubeAtual);
                cmdJogador.Parameters.AddWithValue("@ValorMercadoMilhoes", jogador.ValorDeMercado.ValorEmMilhoes);
                cmdJogador.Parameters.AddWithValue("@Moeda", jogador.ValorDeMercado.Moeda.ToString());

                int rowsAffected = cmdJogador.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    mensagemErro = "Jogador não encontrado.";
                    return false;
                }

                // Atualizar ou inserir estatísticas
                if (jogador.Estatisticas != null)
                {
                    AtualizarOuInserirEstatisticas(connection, transaction, jogador.Id, jogador.Estatisticas);
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro ao atualizar jogador: {ex.Message}";
                return false;
            }
        }

        private void AtualizarOuInserirEstatisticas(MySqlConnection connection, MySqlTransaction transaction, int jogadorId, EstatisticasJogador stats)
        {
            // Verificar se já existe estatística
            string sqlCheck = "SELECT COUNT(*) FROM Estatisticas WHERE JogadorId = @JogadorId;";
            using var cmdCheck = new MySqlCommand(sqlCheck, connection, transaction);
            cmdCheck.Parameters.AddWithValue("@JogadorId", jogadorId);
            bool existe = Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0;

            if (existe)
            {
                // Atualizar
                string sqlUpdate = @"
                    UPDATE Estatisticas
                    SET Jogos = @Jogos, Gols = @Gols, Assistencias = @Assistencias,
                        GolsSofridos = @GolsSofridos, CleanSheets = @CleanSheets
                    WHERE JogadorId = @JogadorId;";

                using var cmdUpdate = new MySqlCommand(sqlUpdate, connection, transaction);
                cmdUpdate.Parameters.AddWithValue("@JogadorId", jogadorId);
                cmdUpdate.Parameters.AddWithValue("@Jogos", stats.Jogos);
                cmdUpdate.Parameters.AddWithValue("@Gols", stats.Gols);
                cmdUpdate.Parameters.AddWithValue("@Assistencias", stats.Assistencias);
                cmdUpdate.Parameters.AddWithValue("@GolsSofridos", stats.GolsSofridos.HasValue ? stats.GolsSofridos.Value : DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("@CleanSheets", stats.CleanSheets.HasValue ? stats.CleanSheets.Value : DBNull.Value);
                cmdUpdate.ExecuteNonQuery();
            }
            else
            {
                // Inserir
                InserirEstatisticas(connection, transaction, jogadorId, stats);
            }
        }

        // Excluir jogador
        public bool Excluir(int id, out string mensagemErro)
        {
            mensagemErro = "";

            try
            {
                using var connection = _dbConnection.GetConnection();
                using var transaction = connection.BeginTransaction();

                // Excluir estatísticas primeiro (FK)
                string sqlStats = "DELETE FROM Estatisticas WHERE JogadorId = @Id;";
                using var cmdStats = new MySqlCommand(sqlStats, connection, transaction);
                cmdStats.Parameters.AddWithValue("@Id", id);
                cmdStats.ExecuteNonQuery();

                // Excluir jogador
                string sqlJogador = "DELETE FROM Jogadores WHERE Id = @Id;";
                using var cmdJogador = new MySqlCommand(sqlJogador, connection, transaction);
                cmdJogador.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmdJogador.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    mensagemErro = "Jogador não encontrado.";
                    return false;
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro ao excluir jogador: {ex.Message}";
                return false;
            }
        }

        private Jogador MapearJogador(IDataReader reader)
        {
            var valorMilhoes = Convert.ToDecimal(reader["ValorMercadoMilhoes"]);
            var moedaStr = reader["Moeda"]?.ToString() ?? "Real";

            Moeda moeda = Enum.TryParse(moedaStr, out Moeda m) ? m : Moeda.Real;

            var jogador = new Jogador
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString(),
                Idade = Convert.ToInt32(reader["Idade"]),
                Nacionalidade = reader["Nacionalidade"]?.ToString() ?? "",
                Posicao = reader["Posicao"]?.ToString() ?? "",
                PeDominante = reader["PeDominante"]?.ToString() ?? "",
                ClubeAtual = reader["ClubeAtual"]?.ToString() ?? "",
                ValorDeMercado = new ValorDeMercado(valorMilhoes, moeda)
            };

            // Mapear estatísticas se existirem
            if (reader["Jogos"] != DBNull.Value)
            {
                jogador.Estatisticas = new EstatisticasJogador
                {
                    Jogos = Convert.ToInt32(reader["Jogos"]),
                    Gols = Convert.ToInt32(reader["Gols"]),
                    Assistencias = Convert.ToInt32(reader["Assistencias"]),
                    GolsSofridos = reader["GolsSofridos"] != DBNull.Value ? Convert.ToInt32(reader["GolsSofridos"]) : null,
                    CleanSheets = reader["CleanSheets"] != DBNull.Value ? Convert.ToInt32(reader["CleanSheets"]) : null
                };
            }

            return jogador;
        }
    }
}