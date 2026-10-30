using System;
using FootballScout.Models;

namespace FootballScout.Models
{
    public class BuscaSalva
    {
        public int Id { get; set; }
        public string NomeBusca { get; set; }
        public FiltroBusca Filtro { get; set; }
        public int OrdernacaoPor { get; set; } // 1=Nome, 2=Idade, 3=Valor, 4=Gols
        public string OrdemDirecao { get; set; } // "ASC" ou "DESC"
        public DateTime DataCriacao { get; set; }

        public BuscaSalva()
        {
            Filtro = new FiltroBusca();
            DataCriacao = DateTime.Now;
        }
    }
}
