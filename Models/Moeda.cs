namespace FootballScout.Models
{
    public enum Moeda
    {
        Real,
        Euro,
        Dolar
    }

    public class ValorDeMercado
    {
        public decimal ValorEmMilhoes { get; set; }
        public Moeda Moeda { get; set; }

        // Taxas de conversão aproximadas (podem ser atualizadas)
        private const decimal TaxaEuroParaReal = 5.7m;
        private const decimal TaxaDolarParaReal = 5.2m;

        public ValorDeMercado(decimal valorEmMilhoes, Moeda moeda)
        {
            ValorEmMilhoes = valorEmMilhoes;
            Moeda = moeda;
        }

        public decimal ValorEmReais()
        {
            return Moeda switch
            {
                Moeda.Real => ValorEmMilhoes,
                Moeda.Euro => ValorEmMilhoes * TaxaEuroParaReal,
                Moeda.Dolar => ValorEmMilhoes * TaxaDolarParaReal,
                _ => ValorEmMilhoes
            };
        }

        public decimal ValorEmEuros()
        {
            return Moeda switch
            {
                Moeda.Euro => ValorEmMilhoes,
                Moeda.Real => ValorEmMilhoes / TaxaEuroParaReal,
                Moeda.Dolar => (ValorEmMilhoes * TaxaDolarParaReal) / TaxaEuroParaReal,
                _ => ValorEmMilhoes
            };
        }

        public decimal ValorEmDolares()
        {
            return Moeda switch
            {
                Moeda.Dolar => ValorEmMilhoes,
                Moeda.Real => ValorEmMilhoes / TaxaDolarParaReal,
                Moeda.Euro => (ValorEmMilhoes * TaxaEuroParaReal) / TaxaDolarParaReal,
                _ => ValorEmMilhoes
            };
        }

        public override string ToString()
        {
            string simbolo = Moeda switch
            {
                Moeda.Real => "R$",
                Moeda.Euro => "€",
                Moeda.Dolar => "$",
                _ => ""
            };

            return $"{simbolo} {ValorEmMilhoes:F1}M";
        }
    }
}
