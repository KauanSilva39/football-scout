using System;
using System.Collections.Generic;
using FootballScout.Models;

namespace FootballScout.Views
{
    public class JogadorView
    {
        // Exibir detalhes de um único jogador
        public static void ExibirJogador(Jogador jogador)
        {
            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ID: {jogador.Id}");
            Console.WriteLine($"Nome: {jogador.Nome}");
            Console.WriteLine($"Idade: {jogador.Idade} anos");
            Console.WriteLine($"Nacionalidade: {jogador.Nacionalidade}");
            Console.WriteLine($"Posição: {jogador.Posicao}");
            Console.WriteLine($"Pé Dominante: {jogador.PeDominante}");
            Console.WriteLine($"Clube Atual: {jogador.ClubeAtual}");
            Console.WriteLine($"Valor de Mercado: R$ {jogador.ValorDeMercado:N2}");

            if (jogador.Estatisticas != null)
            {
                Console.WriteLine("\n=== Estatísticas ===");
                Console.WriteLine($"Jogos: {jogador.Estatisticas.Jogos}");
                Console.WriteLine($"Gols: {jogador.Estatisticas.Gols}");
                Console.WriteLine($"Assistências: {jogador.Estatisticas.Assistencias}");

                if (jogador.Estatisticas.GolsSofridos.HasValue)
                    Console.WriteLine($"Gols Sofridos: {jogador.Estatisticas.GolsSofridos.Value}");

                if (jogador.Estatisticas.CleanSheets.HasValue)
                    Console.WriteLine($"Clean Sheets: {jogador.Estatisticas.CleanSheets.Value}");

                Console.WriteLine($"Média de Gols/Jogo: {jogador.Estatisticas.MediaGolsPorJogo:F2}");
                Console.WriteLine($"Participações em Gols: {jogador.Estatisticas.ParticipacoesEmGols}");
            }

            Console.WriteLine("----------------------------------------");
        }

        // Exibir lista de jogadores
        public static void ExibirListaJogadores(List<Jogador> jogadores)
        {
            Console.WriteLine();
            Console.WriteLine("=== LISTA DE JOGADORES ===");
            Console.WriteLine();

            if (jogadores.Count == 0)
            {
                Console.WriteLine("Nenhum jogador cadastrado.");
                return;
            }

            foreach (var jogador in jogadores)
            {
                ExibirJogador(jogador);
            }

            Console.WriteLine($"Total: {jogadores.Count} jogador(es) cadastrado(s).");
        }

        // Solicitar dados do jogador para cadastro
        public static Jogador SolicitarDadosJogador()
        {
            var jogador = new Jogador();

            Console.Write("Nome: ");
            jogador.Nome = Console.ReadLine() ?? "";

            Console.Write("Idade: ");
            int idade;
            while (!int.TryParse(Console.ReadLine(), out idade) || idade <= 0)
            {
                Console.Write("Idade inválida. Digite um número positivo: ");
            }
            jogador.Idade = idade;

            Console.Write("Nacionalidade: ");
            jogador.Nacionalidade = Console.ReadLine() ?? "";

            Console.Write("Posição (goleiro, zagueiro, lateral, volante, meio-campista, ponta, centroavante): ");
            jogador.Posicao = Console.ReadLine() ?? "";

            Console.Write("Pé Dominante (direito, esquerdo, ambidestro): ");
            jogador.PeDominante = Console.ReadLine() ?? "";

            Console.Write("Clube Atual: ");
            jogador.ClubeAtual = Console.ReadLine() ?? "";

            Console.Write("Valor de Mercado (R$): ");
            decimal valor;
            while (!decimal.TryParse(Console.ReadLine(), out valor) || valor < 0)
            {
                Console.Write("Valor inválido. Digite um valor positivo ou zero: ");
            }
            jogador.ValorDeMercado = valor;

            // Solicitar estatísticas opcionais
            Console.Write("\nDeseja cadastrar estatísticas agora? (S/N): ");
            string resposta = Console.ReadLine()?.ToLower();

            if (resposta == "s" || resposta == "sim")
            {
                jogador.Estatisticas = SolicitarEstatisticas();
            }

            return jogador;
        }

        // Solicitar estatísticas do jogador
        public static EstatisticasJogador SolicitarEstatisticas()
        {
            var stats = new EstatisticasJogador();

            Console.Write("\n=== ESTATÍSTICAS ===");
            Console.WriteLine();

            Console.Write("Número de Jogos: ");
            int jogos;
            while (!int.TryParse(Console.ReadLine(), out jogos) || jogos < 0)
            {
                Console.Write("Valor inválido. Digite um número positivo ou zero: ");
            }
            stats.Jogos = jogos;

            Console.Write("Gols: ");
            int gols;
            while (!int.TryParse(Console.ReadLine(), out gols) || gols < 0)
            {
                Console.Write("Valor inválido. Digite um número positivo ou zero: ");
            }
            stats.Gols = gols;

            Console.Write("Assistências: ");
            int assistencias;
            while (!int.TryParse(Console.ReadLine(), out assistencias) || assistencias < 0)
            {
                Console.Write("Valor inválido. Digite um número positivo ou zero: ");
            }
            stats.Assistencias = assistencias;

            // Perguntar sobre estatísticas de goleiro
            Console.Write("\nEste jogador é goleiro? (S/N): ");
            string resposta = Console.ReadLine()?.ToLower();

            if (resposta == "s" || resposta == "sim")
            {
                Console.Write("Gols Sofridos: ");
                int golsSofridos;
                while (!int.TryParse(Console.ReadLine(), out golsSofridos) || golsSofridos < 0)
                {
                    Console.Write("Valor inválido. Digite um número positivo ou zero: ");
                }
                stats.GolsSofridos = golsSofridos;

                Console.Write("Clean Sheets: ");
                int cleanSheets;
                while (!int.TryParse(Console.ReadLine(), out cleanSheets) || cleanSheets < 0)
                {
                    Console.Write("Valor inválido. Digite um número positivo ou zero: ");
                }
                stats.CleanSheets = cleanSheets;
            }

            return stats;
        }

        // Solicitar ID do jogador
        public static int SolicitarId()
        {
            Console.Write("Digite o ID do jogador: ");
            if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            {
                return id;
            }
            return -1;
        }

        // Exibir busca com resultados
        public static void ExibirBusca(List<Jogador> resultados)
        {
            Console.WriteLine();
            Console.WriteLine("=== RESULTADOS DA BUSCA ===");
            Console.WriteLine();

            if (resultados.Count == 0)
            {
                Console.WriteLine("Nenhum jogador encontrado com este nome.");
                return;
            }

            ExibirListaJogadores(resultados);
        }
    }
}
