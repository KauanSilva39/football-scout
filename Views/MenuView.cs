using System;

namespace FootballScout.Views
{
    public class MenuView
    {
        public static void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("       FOOTBALL SCOUT - MENU PRINCIPAL");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Cadastrar Jogador");
            Console.WriteLine("2. Listar Jogadores");
            Console.WriteLine("3. Buscar Jogador por Nome");
            Console.WriteLine("4. Busca Avançada com Filtros");
            Console.WriteLine("5. Visualizar Buscas Salvas");
            Console.WriteLine("6. Editar Jogador");
            Console.WriteLine("7. Excluir Jogador");
            Console.WriteLine("8. Atualizar Estatísticas");
            Console.WriteLine("9. Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");
        }

        public static int ObterOpcao()
        {
            if (int.TryParse(Console.ReadLine(), out int opcao) && opcao >= 1 && opcao <= 9)
            {
                return opcao;
            }
            return -1; // Opção inválida
        }

        public static void ExibirMensagem(string mensagem)
        {
            Console.WriteLine();
            Console.WriteLine($"[INFO] {mensagem}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public static void ExibirErro(string mensagem)
        {
            Console.WriteLine();
            Console.WriteLine($"[ERRO] {mensagem}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public static void ExibirMensagemDeConfirmacao(string mensagem)
        {
            Console.WriteLine();
            Console.WriteLine(mensagem);
            Console.Write("Deseja confirmar? (S/N): ");
        }
    }
}
