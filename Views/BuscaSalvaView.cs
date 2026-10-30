using System;
using System.Collections.Generic;
using FootballScout.Models;

namespace FootballScout.Views
{
    public class BuscaSalvaView
    {
        public static void ExibirBuscasSalvas(List<BuscaSalva> buscas)
        {
            Console.WriteLine("\n=== BUSCAS SALVAS ===");
            Console.WriteLine();

            if (buscas.Count == 0)
            {
                Console.WriteLine("Nenhuma busca salva.");
                return;
            }

            foreach (var busca in buscas)
            {
                ExibirBuscaSalva(busca);
                Console.WriteLine();
            }
        }

        public static void ExibirBuscaSalva(BuscaSalva busca)
        {
            Console.WriteLine($"[{busca.Id}] {busca.NomeBusca}");
            Console.WriteLine($"   Criada em: {busca.DataCriacao:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"   Filtros: {busca.Filtro}");
            Console.WriteLine($"   Ordenação: {GetNomeOrdenacao(busca.OrdernacaoPor)} ({busca.OrdemDirecao})");
        }

        public static BuscaSalva SolicitarBuscaSalva()
        {
            var busca = new BuscaSalva();

            Console.Write("Nome da busca (ex: 'Ponta Jovem'): ");
            busca.NomeBusca = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(busca.NomeBusca))
            {
                Console.WriteLine("Nome da busca é obrigatório.");
                return null;
            }

            return busca;
        }

        public static int SolicitarIdBusca()
        {
            Console.Write("Digite o ID da busca: ");
            if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            {
                return id;
            }
            return -1;
        }

        private static string GetNomeOrdenacao(int ordem)
        {
            return ordem switch
            {
                1 => "Nome",
                2 => "Idade",
                3 => "Valor de Mercado",
                4 => "Gols",
                _ => "Nome"
            };
        }
    }
}
