CREATE DATABASE eventPlus;
USE eventPlus

CREATE TABLE TipoUsuario (
  IdTipoUsuario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Titulo VARCHAR(100)
);

CREATE TABLE Usuario (
  IdUsuario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IdTipoUsuario UNIQUEIDENTIFIER FOREIGN KEY REFERENCES TipoUsuario(IdTipoUsuario),
  Nome VARCHAR(100),
  Email VARCHAR(256) UNIQUE,
  Senha VARCHAR(60)
);

CREATE TABLE TipoEvento (
  IdTipoEvento UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  TituloTipoEvento VARCHAR(100) NOT NULL,
);

CREATE TABLE Instituicao (
  IdInstituicao UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  CNPJ VARCHAR(14) NOT NULL,
  NomeFantasia VARCHAR(100) NOT NULL,
  Endereco VARCHAR(100) NOT NULL
);

CREATE TABLE Evento (
  IdEvento UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IdTipoEvento UNIQUEIDENTIFIER FOREIGN KEY REFERENCES TipoEvento(IdTipoEvento),
  IdInstituicao UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Instituicao(IdInstituicao),
  NomeEvento VARCHAR(100) NOT NULL,
  Descricao TEXT NOT NULL,
  DataEvento DATETIME NOT NULL,
  ImagemUrl VARCHAR(250) NOT NULL
);

CREATE TABLE Comentario (
  IdComentario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IdUsuario UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Usuario(IdUsuario),
  IdEvento UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Evento(IdEvento),
  Descricao VARCHAR(250) NOT NULL,
  DataComentario DATETIME NOT NULL,
  Exibe BIT NOT NULL
);

CREATE TABLE Presenca (
  IdPresenca UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IdUsuario UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Usuario(IdUsuario),
  IdEvento UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Evento(IdEvento),
  Situacao BIT NOT NULL
);

SELECT * FROM Usuario;

INSERT INTO TipoUsuario(Titulo) 
VALUES ('Administrador'),
       ('Espectador'),
       ('Palestrante') 

SELECT* FROM Evento

SELECT* FROM Instituicao

INSERT INTO Usuario(IdTipoUsuario, Nome, Email, Senha) 
VALUES ('3A85F484-FD48-44EB-B07E-1EF105B50578', 'Laura Angélica', 'laura@email.com', 'laura123'),
       ('B25B9ADA-5491-4ECD-B830-AC038DFEB343', 'Maria Eduarda', 'maria@email.com', 'maria123'),
       ('0597D5B8-CBFD-4A2D-A924-D72B01635AE4', 'Emily Garcia', 'emily@email.com', 'emily123')

INSERT INTO TipoEvento(TituloTipoEvento)
VALUES ('Inteligência Artificial') 

INSERT INTO Instituicao(CNPJ, NomeFantasia, Endereco)
VALUES ('03774819008855', 'SENAI', 'R. Niterói, 180 - Centro, São Caetano do Sul - SP')

INSERT INTO Evento(IdTipoEvento, IdInstituicao, NomeEvento, Descricao, DataEvento, ImagemUrl)
VALUES ('58604B4F-B70D-472B-AFFA-638DD717DA50', '40A68469-2EAF-4E01-BA79-90A847F9BCA0', 'Conhecendo a Inteligência Artificial', 'Aprofundando o conhecimento e entendendo sobre a Inteligência Artificial no mercado de trabalho', 2026-08-14, 'https://plus.unsplash.com/premium_photo-1785149414233-134b809eecfb?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D')

INSERT INTO Comentario(IdUsuario, IdEvento, Descricao, DataComentario, Exibe)
VALUES ('9CCF445D-FB59-4A39-9099-2C22F18977E0', 'A8ECEC7E-160C-4A27-8732-CDF028F4FDC5', 'A palestra foi incrível, alcancei um conhecimento muito maior sobre a IA.', 2026-08-14, 0)

INSERT INTO Presenca(IdUsuario, IdEvento, Situacao)
VALUES ('9CCF445D-FB59-4A39-9099-2C22F18977E0', 'A8ECEC7E-160C-4A27-8732-CDF028F4FDC5', 0)

SELECT * FROM TipoEvento

SELECT * FROM TipoUsuario

SELECT * FROM Usuario

SELECT * FROM Evento
