using System;
using System.Collections.Generic;
using System.Linq;
using FootballScout.Models;

namespace FootballScout.Services
{
    public class FiltroService
    {
        public List<Jogador> AplicarFiltros(List<Jogador> jogadores, FiltroBusca filtro)
        {
            if (jogadores == null) return new List<Jogador>();
            if (filtro == null || !filtro.TemFiltros()) return jogadores;

            var resultado = jogadores.AsEnumerable();

            // Filtro por nome (parcial, case-insensitive)
            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                resultado = resultado.Where(j => j.Nome.Contains(filtro.Nome, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por posição
            if (!string.IsNullOrWhiteSpace(filtro.Posicao))
            {
                resultado = resultado.Where(j => j.Posicao.Contains(filtro.Posicao, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por nacionalidade
            if (!string.IsNullOrWhiteSpace(filtro.Nacionalidade))
            {
                resultado = resultado.Where(j => j.Nacionalidade.Contains(filtro.Nacionalidade, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por clube
            if (!string.IsNullOrWhiteSpace(filtro.Clube))
            {
                resultado = resultado.Where(j => j.ClubeAtual.Contains(filtro.Clube, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por idade mínima
            if (filtro.IdadeMinima.HasValue)
            {
                resultado = resultado.Where(j => j.Idade >= filtro.IdadeMinima.Value);
            }

            // Filtro por idade máxima
            if (filtro.IdadeMaxima.HasValue)
            {
                resultado = resultado.Where(j => j.Idade <= filtro.IdadeMaxima.Value);
            }

            // Filtro por valor mínimo
            if (filtro.ValorMinimoMilhoes.HasValue && filtro.MoedaFiltro.HasValue)
            {
                resultado = resultado.Where(j =>
                {
                    decimal valorEmMoedaFiltro = filtro.MoedaFiltro.Value switch
                    {
                        Moeda.Real => j.ValorDeMercado.ValorEmReais(),
                        Moeda.Euro => j.ValorDeMercado.ValorEmEuros(),
                        Moeda.Dolar => j.ValorDeMercado.ValorEmDolares(),
                        _ => j.ValorDeMercado.ValorEmReais()
                    };
                    return valorEmMoedaFiltro >= filtro.ValorMinimoMilhoes.Value;
                });
            }

            // Filtro por valor máximo
            if (filtro.ValorMaximoMilhoes.HasValue && filtro.MoedaFiltro.HasValue)
            {
                resultado = resultado.Where(j =>
                {
                    decimal valorEmMoedaFiltro = filtro.MoedaFiltro.Value switch
                    {
                        Moeda.Real => j.ValorDeMercado.ValorEmReais(),
                        Moeda.Euro => j.ValorDeMercado.ValorEmEuros(),
                        Moeda.Dolar => j.ValorDeMercado.ValorEmDolares(),
                        _ => j.ValorDeMercado.ValorEmReais()
                    };
                    return valorEmMoedaFiltro <= filtro.ValorMaximoMilhoes.Value;
                });
            }

            // Filtro por gols mínimos
            if (filtro.GolsMinimos.HasValue)
            {
                resultado = resultado.Where(j => j.Estatisticas != null && j.Estatisticas.Gols >= filtro.GolsMinimos.Value);
            }

            // Filtro por assistências mínimas
            if (filtro.AssistenciasMinimas.HasValue)
            {
                resultado = resultado.Where(j => j.Estatisticas != null && j.Estatisticas.Assistencias >= filtro.AssistenciasMinimas.Value);
            }

            return resultado.ToList();
        }
    }
}
