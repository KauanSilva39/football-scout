using System.Collections.Generic;
using System.Linq;
using FootballScout.Models;

namespace FootballScout.Services
{
    public class OrdenacaoService
    {
        public List<Jogador> Ordenar(List<Jogador> jogadores, int criterio, string direcao)
        {
            if (jogadores == null) return new List<Jogador>();
            if (jogadores.Count == 0) return jogadores;

            bool ascending = direcao.ToUpper() == "ASC";

            switch (criterio)
            {
                case 1: // Nome
                    return ascending ? jogadores.OrderBy(j => j.Nome).ToList() : jogadores.OrderByDescending(j => j.Nome).ToList();
                case 2: // Idade
                    return ascending ? jogadores.OrderBy(j => j.Idade).ToList() : jogadores.OrderByDescending(j => j.Idade).ToList();
                case 3: // Valor de Mercado
                    return ascending ? jogadores.OrderBy(j => j.ValorDeMercado.ValorEmMilhoes).ToList() : jogadores.OrderByDescending(j => j.ValorDeMercado.ValorEmMilhoes).ToList();
                case 4: // Gols
                    return ascending ? jogadores.OrderBy(j => j.Estatisticas?.Gols ?? 0).ToList() : jogadores.OrderByDescending(j => j.Estatisticas?.Gols ?? 0).ToList();
                case 5: // Assistências
                    return ascending ? jogadores.OrderBy(j => j.Estatisticas?.Assistencias ?? 0).ToList() : jogadores.OrderByDescending(j => j.Estatisticas?.Assistencias ?? 0).ToList();
                default:
                    return jogadores;
            }
        }

        public static string GetNomeCriterio(int criterio)
        {
            return criterio switch
            {
                1 => "Nome",
                2 => "Idade",
                3 => "Valor de Mercado",
                4 => "Gols",
                5 => "Assistências",
                _ => "Nome"
            };
        }
    }
}
