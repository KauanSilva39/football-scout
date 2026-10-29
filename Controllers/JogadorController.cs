using System;
using System.Collections.Generic;
using FootballScout.Models;
using FootballScout.Repositories;

namespace FootballScout.Controllers
{
    public class JogadorController
    {
        private readonly JogadorRepository _repository;

        public JogadorController(JogadorRepository repository)
        {
            _repository = repository;
        }

        // Cadastrar um novo jogador
        public bool Cadastrar(Jogador jogador, out string mensagemErro)
        {
            // Validar o jogador
            if (!jogador.IsValid(out mensagemErro))
            {
                return false;
            }

            // Validar estatísticas se existirem
            if (jogador.Estatisticas != null)
            {
                if (!jogador.Estatisticas.IsValid(out mensagemErro))
                {
                    return false;
                }
            }

            return _repository.Inserir(jogador, out mensagemErro);
        }

        // Listar todos os jogadores
        public List<Jogador> ListarTodos()
        {
            return _repository.ListarTodos();
        }

        // Buscar jogador pelo nome (parcial, case-insensitive)
        public List<Jogador> BuscarPorNome(string nome)
        {
            return _repository.BuscarPorNome(nome);
        }

        // Buscar jogador pelo ID
        public Jogador BuscarPorId(int id)
        {
            return _repository.BuscarPorId(id);
        }

        // Editar jogador
        public bool Editar(int id, Jogador jogadorAtualizado, out string mensagemErro)
        {
            jogadorAtualizado.Id = id;
            return _repository.Atualizar(jogadorAtualizado, out mensagemErro);
        }

        // Atualizar apenas estatísticas de um jogador
        public bool AtualizarEstatisticas(int id, Jogador jogadorAtualizado, out string mensagemErro)
        {
            var jogadorExistente = BuscarPorId(id);
            if (jogadorExistente == null)
            {
                mensagemErro = "Jogador não encontrado.";
                return false;
            }

            // Validar estatísticas se existirem
            if (jogadorAtualizado.Estatisticas != null)
            {
                if (!jogadorAtualizado.Estatisticas.IsValid(out mensagemErro))
                {
                    return false;
                }
            }

            jogadorExistente.Estatisticas = jogadorAtualizado.Estatisticas;
            return _repository.Atualizar(jogadorExistente, out mensagemErro);
        }

        // Excluir jogador
        public bool Excluir(int id, out string mensagemErro)
        {
            return _repository.Excluir(id, out mensagemErro);
        }
    }
}
