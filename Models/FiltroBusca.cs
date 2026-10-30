using System.Collections.Generic;
using FootballScout.Models;

namespace FootballScout.Models
{
    public class FiltroBusca
    {
        public string? Nome { get; set; }
        public string? Posicao { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Clube { get; set; }
        public int? IdadeMinima { get; set; }
        public int? IdadeMaxima { get; set; }
        public decimal? ValorMinimoMilhoes { get; set; }
        public decimal? ValorMaximoMilhoes { get; set; }
        public Moeda? MoedaFiltro { get; set; }
        public int? GolsMinimos { get; set; }
        public int? AssistenciasMinimas { get; set; }

        public bool TemFiltros()
        {
            return !string.IsNullOrWhiteSpace(Nome) ||
                   !string.IsNullOrWhiteSpace(Posicao) ||
                   !string.IsNullOrWhiteSpace(Nacionalidade) ||
                   !string.IsNullOrWhiteSpace(Clube) ||
                   IdadeMinima.HasValue ||
                   IdadeMaxima.HasValue ||
                   ValorMinimoMilhoes.HasValue ||
                   ValorMaximoMilhoes.HasValue ||
                   MoedaFiltro.HasValue ||
                   GolsMinimos.HasValue ||
                   AssistenciasMinimas.HasValue;
        }

        public override string ToString()
        {
            var filtros = new List<string>();

            if (!string.IsNullOrWhiteSpace(Nome)) filtros.Add($"Nome: {Nome}");
            if (!string.IsNullOrWhiteSpace(Posicao)) filtros.Add($"Posição: {Posicao}");
            if (!string.IsNullOrWhiteSpace(Nacionalidade)) filtros.Add($"Nacionalidade: {Nacionalidade}");
            if (!string.IsNullOrWhiteSpace(Clube)) filtros.Add($"Clube: {Clube}");
            if (IdadeMinima.HasValue) filtros.Add($"Idade mínima: {IdadeMinima}");
            if (IdadeMaxima.HasValue) filtros.Add($"Idade máxima: {IdadeMaxima}");
            if (ValorMinimoMilhoes.HasValue) filtros.Add($"Valor mín: {ValorMinimoMilhoes}M");
            if (ValorMaximoMilhoes.HasValue) filtros.Add($"Valor máx: {ValorMaximoMilhoes}M");
            if (MoedaFiltro.HasValue) filtros.Add($"Moeda: {MoedaFiltro}");
            if (GolsMinimos.HasValue) filtros.Add($"Gols mín: {GolsMinimos}");
            if (AssistenciasMinimas.HasValue) filtros.Add($"Assistências mín: {AssistenciasMinimas}");

            return string.Join(", ", filtros);
        }
    }
}
