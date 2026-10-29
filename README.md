# Football Scout

Sistema de gerenciamento de jogadores de futebol para atividades de scouting e análise de desempenho.

## 🚀 Versão 3 - Banco de Dados MySQL

Esta é a terceira versão do sistema, que permite:

### Funcionalidades Atuais

- **Cadastrar jogadores** com nome, idade, nacionalidade, posição, pé dominante, clube atual e valor de mercado
- **Listar todos os jogadores** cadastrados no banco de dados
- **Buscar jogadores** pelo nome (pesquisa parcial, não diferencia maiúsculas/minúsculas)
- **Editar jogadores** existentes com atualização no banco
- **Excluir jogadores** com confirmação e remoção dos dados relacionados
- **Cadastrar e atualizar estatísticas** (jogos, gols, assistências, gols sofridos e clean sheets)
- **Calcular médias automaticamente** (média de gols por jogo, participações em gols)
- **Persistência de dados** - dados salvos permanentemente no MySQL
- **Validações** para evitar dados inválidos

## 📋 Requisitos

- .NET 8.0 SDK ou superior
- MySQL Server 8.0+ instalado
- MySQL Workbench (opcional, para gerenciar o banco)
- Visual Studio Code (ou qualquer editor de código)

## 🔧 Como Executar

### 1. Configuração do MySQL

1. Instale o MySQL Server 8.0+ (https://dev.mysql.com/downloads/installer/)
2. Crie o banco de dados:
   ```sql
   CREATE DATABASE football_scout CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```
3. Execute o script `Database/Schema.sql` para criar as tabelas

### 2. Configurar Credenciais

O projeto usa variáveis de ambiente. Configure suas credenciais:

**Opção A: Variáveis de ambiente do sistema**
```bash
# Windows
set DB_HOST=localhost
set DB_PORT=3306
set DB_NAME=football_scout
set DB_USER=root
set DB_PASSWORD=sua_senha
```

**Opção B: Arquivo .env** (recomendado)
Crie um arquivo `.env` na raiz do projeto (baseado em `.env.example`) com suas credenciais.

### 3. Executar o Projeto

### Executar a Aplicação

1. Abra o terminal na pasta do projeto:
   ```bash
   cd FootballScout
   ```

2. Execute o comando:
   ```bash
   dotnet run
   ```

3. Siga as instruções do menu no terminal.

## 📁 Estrutura do Projeto

```
FootballScout/
├── Models/
│   ├── Jogador.cs                  # Entidade Jogador
│   └── EstatisticasJogador.cs      # Entidade de estatísticas
├── Controllers/
│   └── JogadorController.cs        # Lógica de negócio
├── Views/
│   ├── MenuView.cs                 # Exibe o menu principal
│   └── JogadorView.cs              # Exibe dados dos jogadores
├── Database/
│   ├── DatabaseConnection.cs       # Conexão com MySQL
│   ├── DatabaseInitializer.cs      # Inicialização do banco
│   └── Schema.sql                  # Script de criação das tabelas
├── Repositories/
│   └── JogadorRepository.cs        # Acesso aos dados do banco
├── Program.cs                      # Ponto de entrada da aplicação
└── README.md                       # Este arquivo
```

## 🎯 Próximas Versões

### Roadmap do Projeto

- ~~**V1:** Gerenciamento básico de jogadores~~ ✅ Concluído
- ~~**V2:** Estatísticas dos jogadores~~ ✅ Concluído
- ~~**V3:** Banco de dados MySQL~~ ✅ Concluído

### Versões Futuras

- **V4:** Filtros avançados e busca por critérios múltiplos
  - Filtrar por posição, idade, nacionalidade
  - Buscar por valor de mercado
  - Ordenar por estatísticas
  - Combinação de filtros

- **V5:** Comparação e rankings
  - Comparar dois ou mais jogadores lado a lado
  - Ranking por posição
  - Melhores pontuadores e assistentes
  - Estatísticas agregadas por clube

- **V6:** API REST e aplicação web
  - Expor endpoints REST para jogadores
  - Documentação Swagger
  - Interface web com React/Vue
  - Dashboard de estatísticas

- **V7:** Análise de dados com Python
  - Visualizações gráficas de desempenho
  - Análise estatística avançada
  - Machine Learning para previsão de desempenho
  - Relatórios automatizados

## 📝 Tecnologias

- **C# 12** - Linguagem principal
- **.NET 8.0** - Plataforma de desenvolvimento
- **MySQL 8.0** - Banco de dados relacional
- **MySqlConnector** - Driver de conexão com MySQL
- **Console Application** - Interface de linha de comando

## 🏗️ Padrões de Projeto

- **Repository Pattern** - Separação entre acesso a dados e lógica de negócio
- **MVC-inspired** - Model-View-Controller para organização do código
- **Dependency Injection** - Injeção de dependências básica

## 📚 Conceitos Aprendidos

- Classes e propriedades em C#
- Validação de dados
- Listas e coleções
- Padrão Repository
- Conexão com banco de dados
- SQL CRUD operations
- Transações
- Relacionamentos 1:1
- Propriedades calculadas

---

Desenvolvido para aprender desenvolvimento de software na prática, construindo um sistema real de scouting de futebol.
