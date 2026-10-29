namespace FootballScout.Models
{
    public class Jogador
    {
        // Propriedades do jogador
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Nacionalidade { get; set; }
        public string Posicao { get; set; }
        public string PeDominante { get; set; }
        public string ClubeAtual { get; set; }
        public decimal ValorDeMercado { get; set; }
        public EstatisticasJogador? Estatisticas { get; set; }

        // Validações
        public bool IsValid(out string mensagemErro)
        {
            mensagemErro = "";

            if (string.IsNullOrWhiteSpace(Nome))
            {
                mensagemErro = "O nome não pode estar vazio.";
                return false;
            }

            if (Idade <= 0)
            {
                mensagemErro = "A idade deve ser um número positivo.";
                return false;
            }

            if (ValorDeMercado < 0)
            {
                mensagemErro = "O valor de mercado não pode ser negativo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(PeDominante))
            {
                mensagemErro = "O pé dominante é obrigatório.";
                return false;
            }

            return true;
        }
    }
}
