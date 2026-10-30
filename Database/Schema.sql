-- Criar banco de dados
CREATE DATABASE IF NOT EXISTS football_scout;
USE football_scout;

-- Tabela de Jogadores
CREATE TABLE IF NOT EXISTS Jogadores (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Idade INT NOT NULL,
    Nacionalidade VARCHAR(50),
    Posicao VARCHAR(50),
    PeDominante VARCHAR(20),
    ClubeAtual VARCHAR(100),
    ValorMercadoMilhoes DECIMAL(15, 2) NOT NULL DEFAULT 0.00,
    Moeda VARCHAR(20) NOT NULL DEFAULT 'Real',
    DataCadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Estatísticas (relacionamento 1:1 com Jogadores)
CREATE TABLE IF NOT EXISTS Estatisticas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    JogadorId INT UNIQUE NOT NULL,
    Jogos INT NOT NULL DEFAULT 0,
    Gols INT NOT NULL DEFAULT 0,
    Assistencias INT NOT NULL DEFAULT 0,
    GolsSofridos INT DEFAULT NULL,
    CleanSheets INT DEFAULT NULL,
    DataAtualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (JogadorId) REFERENCES Jogadores(Id) ON DELETE CASCADE
);

-- Inserir alguns dados de exemplo (opcional)
-- INSERT INTO Jogadores (Nome, Idade, Nacionalidade, Posicao, PeDominante, ClubeAtual, ValorDeMercado) VALUES
-- ('Exemplo', 25, 'Brasileiro', 'Atacante', 'Direito', 'Clube Exemplo', 1000000.00);

-- SELECT statement para testar
-- SELECT * FROM Jogadores;
-- SELECT * FROM Estatisticas;