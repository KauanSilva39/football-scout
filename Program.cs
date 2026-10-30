using System;
using FootballScout.Controllers;
using FootballScout.Views;
using FootballScout.Database;
using FootballScout.Models;
using FootballScout.Repositories;
using FootballScout.Services;

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
                        BuscaAvancada(controller);
                        break;
                    case 5:
                        ListarBuscasSalvas(controller);
                        break;
                    case 6:
                        EditarJogador(controller);
                        break;
                    case 7:
                        ExcluirJogador(controller);
                        break;
                    case 8:
                        AtualizarEstatisticas(controller);
                        break;
                    case 9:
                        sair = true;
                        Console.WriteLine("Encerrando o Football Scout...");
                        break;
                    default:
                        MenuView.ExibirErro("Opção inválida. Digite um número entre 1 e 9.");
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

        static void BuscaAvancada(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== BUSCA AVANÇADA ===");
            Console.WriteLine();

            // Solicitar filtros
            var filtro = FiltroView.SolicitarFiltros();

            // Exibir filtros ativos
            FiltroView.ExibirFiltrosAtivos(filtro);

            // Aplicar filtros
            var filtroService = new FiltroService();
            var jogadoresFiltrados = filtroService.AplicarFiltros(controller.ListarTodos(), filtro);

            // Perguntar se quer ordenar
            Console.WriteLine();
            Console.Write("Deseja ordenar os resultados? (S/N): ");
            string ordenar = Console.ReadLine()?.ToLower();

            if (ordenar == "s" || ordenar == "sim")
            {
                Console.WriteLine("\nCriterios de ordenacao:");
                Console.WriteLine("1. Nome");
                Console.WriteLine("2. Idade");
                Console.WriteLine("3. Valor de Mercado");
                Console.WriteLine("4. Gols");
                Console.WriteLine("5. Assistencias");
                Console.Write("Escolha: ");

                if (int.TryParse(Console.ReadLine(), out int criterio) && criterio >= 1 && criterio <= 5)
                {
                    Console.Write("Direcao (ASC/DESC): ");
                    string direcao = Console.ReadLine()?.ToUpper() ?? "ASC";

                    var ordenacaoService = new OrdenacaoService();
                    jogadoresFiltrados = ordenacaoService.Ordenar(jogadoresFiltrados, criterio, direcao);
                    Console.WriteLine($"\nOrdenado por {OrdenacaoService.GetNomeCriterio(criterio)} ({direcao})");
                }
            }

            // Exibir resultados
            Console.WriteLine("\n=== RESULTADOS ===");
            JogadorView.ExibirListaJogadores(jogadoresFiltrados);

            // Perguntar se quer salvar a busca
            Console.WriteLine();
            Console.Write("Deseja salvar esta busca para usar depois? (S/N): ");
            string salvar = Console.ReadLine()?.ToLower();

            if (salvar == "s" || salvar == "sim")
            {
                SalvarBusca(controller, filtro, 1, "ASC");
            }

            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static void SalvarBusca(JogadorController controller, FiltroBusca filtro, int ordenacaoPor = 1, string ordemDirecao = "ASC")
        {
            Console.WriteLine();
            Console.Write("Nome da busca: ");
            string nomeBusca = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nomeBusca))
            {
                MenuView.ExibirErro("Nome da busca é obrigatório.");
                return;
            }

            var busca = new BuscaSalva
            {
                NomeBusca = nomeBusca,
                Filtro = filtro,
                OrdernacaoPor = ordenacaoPor,
                OrdemDirecao = ordemDirecao
            };

            var buscaRepo = new BuscaSalvaRepository(new DatabaseConnection());
            if (buscaRepo.Inserir(busca, out string mensagemErro))
            {
                MenuView.ExibirMensagem("Busca salva com sucesso!");
            }
            else
            {
                MenuView.ExibirErro(mensagemErro);
            }
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

            Console.Write($"Valor de Mercado (em milhões) [{jogadorExistente.ValorDeMercado.ValorEmMilhoes}]: ");
            string novoValor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoValor) && decimal.TryParse(novoValor, out decimal valor) && valor >= 0)
            {
                Console.Write($"Moeda (1-Real, 2-Euro, 3-Dolar) [{jogadorExistente.ValorDeMercado.Moeda}]: ");
                string novaMoeda = Console.ReadLine();
                if (int.TryParse(novaMoeda, out int moedaOpcao) && moedaOpcao >= 1 && moedaOpcao <= 3)
                {
                    Moeda moeda = moedaOpcao switch
                    {
                        1 => Moeda.Real,
                        2 => Moeda.Euro,
                        3 => Moeda.Dolar,
                        _ => Moeda.Real
                    };
                    jogadorExistente.ValorDeMercado = new ValorDeMercado(valor, moeda);
                }
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

        static void ListarBuscasSalvas(JogadorController controller)
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAS SALVAS ===");
            Console.WriteLine();

            var buscaRepo = new BuscaSalvaRepository(new DatabaseConnection());
            var buscas = buscaRepo.ListarTodos();

            BuscaSalvaView.ExibirBuscasSalvas(buscas);

            if (buscas.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("1. Usar busca salva");
                Console.WriteLine("2. Excluir busca salva");
                Console.WriteLine("3. Voltar ao menu");
                Console.Write("Escolha: ");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            UsarBuscaSalva(buscaRepo, controller);
                            break;
                        case 2:
                            ExcluirBuscaSalva(buscaRepo);
                            break;
                    }
                }
            }
        }

        static void UsarBuscaSalva(BuscaSalvaRepository buscaRepo, JogadorController controller)
        {
            Console.Write("Digite o ID da busca: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var busca = buscaRepo.ListarTodos().Find(b => b.Id == id);
                if (busca != null)
                {
                    Console.WriteLine($"\nAplicando busca: {busca.NomeBusca}");
                    var filtroService = new FiltroService();
                    var resultados = filtroService.AplicarFiltros(controller.ListarTodos(), busca.Filtro);
                    JogadorView.ExibirListaJogadores(resultados);
                }
            }
        }

        static void ExcluirBuscaSalva(BuscaSalvaRepository buscaRepo)
        {
            Console.Write("Digite o ID da busca: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (buscaRepo.Excluir(id, out string msg))
                {
                    MenuView.ExibirMensagem("Busca excluída com sucesso!");
                }
                else
                {
                    MenuView.ExibirErro(msg);
                }
            }
        }
    }
}
