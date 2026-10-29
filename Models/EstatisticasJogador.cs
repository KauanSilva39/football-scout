namespace FootballScout.Models
{
    public class EstatisticasJogador
    {
        public int Jogos { get; set; }
        public int Gols { get; set; }
        public int Assistencias { get; set; }
        public int? GolsSofridos { get; set; }
        public int? CleanSheets { get; set; }

        public decimal MediaGolsPorJogo
        {
            get
            {
                return Jogos > 0 ? (decimal)Gols / Jogos : 0;
            }
        }

        public int ParticipacoesEmGols
        {
            get
            {
                return Gols + Assistencias;
            }
        }

        public bool IsValid(out string mensagemErro)
        {
            mensagemErro = "";

            if (Jogos < 0)
            {
                mensagemErro = "Número de jogos não pode ser negativo.";
                return false;
            }

            if (Gols < 0)
            {
                mensagemErro = "Número de gols não pode ser negativo.";
                return false;
            }

            if (Assistencias < 0)
            {
                mensagemErro = "Número de assistências não pode ser negativo.";
                return false;
            }

            if (GolsSofridos.HasValue && GolsSofridos.Value < 0)
            {
                mensagemErro = "Número de gols sofridos não pode ser negativo.";
                return false;
            }

            if (CleanSheets.HasValue && CleanSheets.Value < 0)
            {
                mensagemErro = "Número de clean sheets não pode ser negativo.";
                return false;
            }

            return true;
        }
    }
}
