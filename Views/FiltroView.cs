using System;
using FootballScout.Models;

namespace FootballScout.Views
{
    public class FiltroView
    {
        public static FiltroBusca SolicitarFiltros()
        {
            var filtro = new FiltroBusca();

            Console.Clear();
            Console.WriteLine("=== FILTROS AVANÇADOS ===");
            Console.WriteLine("Deixe em branco para ignorar o filtro");
            Console.WriteLine();

            // Nome
            Console.Write("Nome (parcial): ");
            filtro.Nome = Console.ReadLine();

            // Posição
            Console.Write("Posição: ");
            filtro.Posicao = Console.ReadLine();

            // Nacionalidade
            Console.Write("Nacionalidade: ");
            filtro.Nacionalidade = Console.ReadLine();

            // Clube
            Console.Write("Clube: ");
            filtro.Clube = Console.ReadLine();

            // Idade
            Console.Write("Idade mínima: ");
            if (int.TryParse(Console.ReadLine(), out int idadeMin))
                filtro.IdadeMinima = idadeMin;

            Console.Write("Idade máxima: ");
            if (int.TryParse(Console.ReadLine(), out int idadeMax))
                filtro.IdadeMaxima = idadeMax;

            // Valor
            Console.Write("Valor mínimo (em milhões): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valorMin))
                filtro.ValorMinimoMilhoes = valorMin;

            Console.Write("Valor máximo (em milhões): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valorMax))
                filtro.ValorMaximoMilhoes = valorMax;

            // Moeda
            Console.Write("Moeda para filtro (1-Real, 2-Euro, 3-Dolar): ");
            if (int.TryParse(Console.ReadLine(), out int moedaOpcao))
            {
                filtro.MoedaFiltro = moedaOpcao switch
                {
                    1 => Moeda.Real,
                    2 => Moeda.Euro,
                    3 => Moeda.Dolar,
                    _ => null
                };
            }

            // Estatísticas
            Console.Write("Gols mínimos: ");
            if (int.TryParse(Console.ReadLine(), out int golsMin))
                filtro.GolsMinimos = golsMin;

            Console.Write("Assistências mínimas: ");
            if (int.TryParse(Console.ReadLine(), out int assistMin))
                filtro.AssistenciasMinimas = assistMin;

            return filtro;
        }

        public static void ExibirFiltrosAtivos(FiltroBusca filtro)
        {
            if (!filtro.TemFiltros())
            {
                Console.WriteLine("\nNenhum filtro ativo.");
                return;
            }

            Console.WriteLine("\n=== FILTROS ATIVOS ===");
            Console.WriteLine(filtro.ToString());
            Console.WriteLine();
        }
    }
}
