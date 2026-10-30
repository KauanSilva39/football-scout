using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FootballScout.Database;
using FootballScout.Models;
using MySqlConnector;

namespace FootballScout.Repositories
{
    public class BuscaSalvaRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public BuscaSalvaRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public bool Inserir(BuscaSalva busca, out string mensagemErro)
        {
            mensagemErro = "";
            try
            {
                using var connection = _dbConnection.GetConnection();
                using var transaction = connection.BeginTransaction();

                string sql = @"
                    INSERT INTO BuscasSalvas (NomeBusca, FiltroPosicao, FiltroNacionalidade, FiltroClube,
                        IdadeMinima, IdadeMaxima, ValorMinimo, ValorMaximo, MoedaFiltro, OrdernacaoPor, OrdemDirecao)
                    VALUES (@NomeBusca, @FiltroPosicao, @FiltroNacionalidade, @FiltroClube,
                        @IdadeMinima, @IdadeMaxima, @ValorMinimo, @ValorMaximo, @MoedaFiltro, @OrdernacaoPor, @OrdemDirecao);
                    SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@NomeBusca", busca.NomeBusca);
                cmd.Parameters.AddWithValue("@FiltroPosicao", busca.Filtro?.Posicao ?? "");
                cmd.Parameters.AddWithValue("@FiltroNacionalidade", busca.Filtro?.Nacionalidade ?? "");
                cmd.Parameters.AddWithValue("@FiltroClube", busca.Filtro?.Clube ?? "");
                cmd.Parameters.AddWithValue("@IdadeMinima", busca.Filtro?.IdadeMinima ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IdadeMaxima", busca.Filtro?.IdadeMaxima ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ValorMinimo", busca.Filtro?.ValorMinimoMilhoes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ValorMaximo", busca.Filtro?.ValorMaximoMilhoes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MoedaFiltro", busca.Filtro?.MoedaFiltro?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@OrdernacaoPor", busca.OrdernacaoPor);
                cmd.Parameters.AddWithValue("@OrdemDirecao", busca.OrdemDirecao);

                var result = cmd.ExecuteScalar();
                busca.Id = Convert.ToInt32(result);

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro ao salvar busca: {ex.Message}";
                return false;
            }
        }

        public List<BuscaSalva> ListarTodos()
        {
            var buscas = new List<BuscaSalva>();
            try
            {
                using var connection = _dbConnection.GetConnection();
                string sql = "SELECT * FROM BuscasSalvas ORDER BY DataCriacao DESC;";

                using var cmd = new MySqlCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var busca = new BuscaSalva
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        NomeBusca = reader["NomeBusca"].ToString(),
                        Filtro = new FiltroBusca
                        {
                            Posicao = reader["FiltroPosicao"]?.ToString(),
                            Nacionalidade = reader["FiltroNacionalidade"]?.ToString(),
                            Clube = reader["FiltroClube"]?.ToString(),
                            IdadeMinima = reader["IdadeMinima"] != DBNull.Value ? Convert.ToInt32(reader["IdadeMinima"]) : null,
                            IdadeMaxima = reader["IdadeMaxima"] != DBNull.Value ? Convert.ToInt32(reader["IdadeMaxima"]) : null,
                            ValorMinimoMilhoes = reader["ValorMinimo"] != DBNull.Value ? Convert.ToDecimal(reader["ValorMinimo"]) : null,
                            ValorMaximoMilhoes = reader["ValorMaximo"] != DBNull.Value ? Convert.ToDecimal(reader["ValorMaximo"]) : null,
                            MoedaFiltro = reader["MoedaFiltro"] != DBNull.Value && Enum.TryParse(reader["MoedaFiltro"].ToString(), out Moeda m) ? m : null
                        },
                        OrdernacaoPor = Convert.ToInt32(reader["OrdernacaoPor"]),
                        OrdemDirecao = reader["OrdemDirecao"].ToString(),
                        DataCriacao = Convert.ToDateTime(reader["DataCriacao"])
                    };
                    buscas.Add(busca);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar buscas salvas: {ex.Message}");
            }
            return buscas;
        }

        public bool Excluir(int id, out string mensagemErro)
        {
            mensagemErro = "";
            try
            {
                using var connection = _dbConnection.GetConnection();
                using var transaction = connection.BeginTransaction();

                string sql = "DELETE FROM BuscasSalvas WHERE Id = @Id;";
                using var cmd = new MySqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro ao excluir busca: {ex.Message}";
                return false;
            }
        }
    }
}