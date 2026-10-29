using System;
using FootballScout.Controllers;
using FootballScout.Views;
using FootballScout.Database;
using FootballScout.Repositories;

namespace FootballScout
{
    class Program
    {
        static void Main(string[] args)
        {
            // Verificar conexão com banco de dados
            Console.WriteLine("Verificando banco de dados...");
            var dbInitializer = new DatabaseInitializer();
            if (!dbInitializer.VerificarConexao())
            {
                Console.WriteLine("\nPor favor, configure o banco de dados antes de continuar.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine();

            // Criar conexão com o banco
            var dbConnection = new DatabaseConnection();
            var repository = new JogadorRepository(dbConnection);
            var controller = new JogadorController(repository);
            bool sair = false;

            while (!sair)
            {
                MenuView.ExibirMenu();
                int opcao = MenuView.ObterOpcao();

                switch (opcao)
                {
                    case 1:
                        CadastrarJogador(controller);
                        break;
                    case 2:
                        ListarJogadores(controller);
                        break;
                    case 3:
                        BuscarJogador(controller);
                        break;
                    case 4:
                        EditarJogador(controller);
                        break;
                    case 5:
                        ExcluirJogador(controller);
                        break;
                    case 6:
                        AtualizarEstatisticas(controller);
                        break;
                    case 7:
                        sair = true;
                        Console.WriteLine("Encerrando o Football Scout...");
                        break;
                    default:
                        MenuView.ExibirErro("Opção inválida. Digite um número entre 1 e 7.");
                        break;
                }
            }
        }

        static void CadastrarJogador(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRAR JOGADOR ===");
            Console.WriteLine();

            var jogador = JogadorView.SolicitarDadosJogador();

            if (controller.Cadastrar(jogador, out string mensagemErro))
            {
                MenuView.ExibirMensagem("Jogador cadastrado com sucesso!");
            }
            else
            {
                MenuView.ExibirErro(mensagemErro);
            }
        }

        static void ListarJogadores(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== LISTAR JOGADORES ===");

            var jogadores = controller.ListarTodos();
            JogadorView.ExibirListaJogadores(jogadores);

            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static void BuscarJogador(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR JOGADOR POR NOME ===");
            Console.WriteLine();

            Console.Write("Digite o nome (ou parte do nome): ");
            string nome = Console.ReadLine() ?? "";

            var resultados = controller.BuscarPorNome(nome);
            JogadorView.ExibirBusca(resultados);

            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static void EditarJogador(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== EDITAR JOGADOR ===");
            Console.WriteLine();

            int id = JogadorView.SolicitarId();

            if (id <= 0)
            {
                MenuView.ExibirErro("ID inválido.");
                return;
            }

            var jogadorExistente = controller.BuscarPorId(id);
            if (jogadorExistente == null)
            {
                MenuView.ExibirErro("Jogador não encontrado.");
                return;
            }

            Console.WriteLine("Deixe em branco para manter o valor atual.");
            Console.WriteLine();

            Console.Write($"Nome [{jogadorExistente.Nome}]: ");
            string novoNome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoNome)) jogadorExistente.Nome = novoNome;

            Console.Write($"Idade [{jogadorExistente.Idade}]: ");
            string novaIdade = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novaIdade) && int.TryParse(novaIdade, out int idade) && idade > 0)
            {
                jogadorExistente.Idade = idade;
            }

            Console.Write($"Nacionalidade [{jogadorExistente.Nacionalidade}]: ");
            string novaNacionalidade = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novaNacionalidade)) jogadorExistente.Nacionalidade = novaNacionalidade;

            Console.Write($"Posição [{jogadorExistente.Posicao}]: ");
            string novaPosicao = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novaPosicao)) jogadorExistente.Posicao = novaPosicao;

            Console.Write($"Pé Dominante [{jogadorExistente.PeDominante}]: ");
            string novoPe = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoPe)) jogadorExistente.PeDominante = novoPe;

            Console.Write($"Clube Atual [{jogadorExistente.ClubeAtual}]: ");
            string novoClube = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoClube)) jogadorExistente.ClubeAtual = novoClube;

            Console.Write($"Valor de Mercado [{jogadorExistente.ValorDeMercado}]: ");
            string novoValor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoValor) && decimal.TryParse(novoValor, out decimal valor) && valor >= 0)
            {
                jogadorExistente.ValorDeMercado = valor;
            }

            if (controller.Editar(id, jogadorExistente, out string mensagemErro))
            {
                MenuView.ExibirMensagem("Jogador editado com sucesso!");
            }
            else
            {
                MenuView.ExibirErro(mensagemErro);
            }
        }

        static void ExcluirJogador(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== EXCLUIR JOGADOR ===");
            Console.WriteLine();

            int id = JogadorView.SolicitarId();

            if (id <= 0)
            {
                MenuView.ExibirErro("ID inválido.");
                return;
            }

            var jogador = controller.BuscarPorId(id);
            if (jogador == null)
            {
                MenuView.ExibirErro("Jogador não encontrado.");
                return;
            }

            MenuView.ExibirMensagemDeConfirmacao($"Tem certeza que deseja excluir o jogador {jogador.Nome}?");
            string confirmacao = Console.ReadLine()?.ToLower();

            if (confirmacao == "s" || confirmacao == "sim")
            {
                if (controller.Excluir(id, out string mensagemErro))
                {
                    MenuView.ExibirMensagem("Jogador excluído com sucesso!");
                }
                else
                {
                    MenuView.ExibirErro(mensagemErro);
                }
            }
            else
            {
                MenuView.ExibirMensagem("Operação cancelada.");
            }
        }

        static void AtualizarEstatisticas(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== ATUALIZAR ESTATÍSTICAS ===");
            Console.WriteLine();

            int id = JogadorView.SolicitarId();

            if (id <= 0)
            {
                MenuView.ExibirErro("ID inválido.");
                return;
            }

            var jogador = controller.BuscarPorId(id);
            if (jogador == null)
            {
                MenuView.ExibirErro("Jogador não encontrado.");
                return;
            }

            Console.WriteLine($"\nJogador: {jogador.Nome} (ID: {id})");
            JogadorView.ExibirJogador(jogador);
            Console.WriteLine();

            // Criar novas estatísticas
            var novasEstatisticas = JogadorView.SolicitarEstatisticas();

            // Atualizar estatísticas do jogador
            jogador.Estatisticas = novasEstatisticas;

            if (controller.AtualizarEstatisticas(id, jogador, out string mensagemErro))
            {
                MenuView.ExibirMensagem("Estatísticas atualizadas com sucesso!");
            }
            else
            {
                MenuView.ExibirErro(mensagemErro);
            }
        }
    }
}
