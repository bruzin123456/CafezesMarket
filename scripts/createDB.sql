--   cadastro   --
CREATE TABLE cliente (
    id bigint NOT NULL IDENTITY(1, 1),
    nome VARCHAR(512) NOT NULL,
    cpf VARCHAR(11) NOT NULL UNIQUE,
    nascimento DATETIME2 NOT NULL,
    email VARCHAR(512) NOT NULL UNIQUE,

    CONSTRAINT PK_cliente PRIMARY KEY (id)
)
CREATE INDEX IDX_cliente_email ON cliente (email);


CREATE TABLE estado (
    id INT NOT NULL IDENTITY(1, 1),
    nome CHAR(2) NOT NULL UNIQUE,

    CONSTRAINT PK_estado PRIMARY KEY (id),
)
INSERT INTO estado (nome) VALUES ('SP'), ('RJ'), ('BH'), ('MG');


CREATE TABLE endereco (
    id bigint NOT NULL IDENTITY(1, 1),
    cliente_id bigint NOT NULL,
    apelido VARCHAR(128) NOT NULL DEFAULT('CASA'),
    logradouro VARCHAR(512) NOT NULL,
    numero INT NOT NULL,
    complemento VARCHAR(1024),
    cidade VARCHAR(512) NOT NULL,
    estado_id INT NOT NULL,

    CONSTRAINT PK_endereco PRIMARY KEY (id),
    CONSTRAINT FK_endereco_cliente FOREIGN KEY (cliente_id) REFERENCES cliente,
    CONSTRAINT FK_endereco_estado FOREIGN KEY (estado_id) REFERENCES estado
)


CREATE TABLE credencial (
    id bigint NOT NULL IDENTITY(1, 1),
    cliente_id bigint NOT NULL,
    senha VARCHAR(1024) NOT NULL,
    salt VARCHAR(512),
    erros INT NOT NULL DEFAULT(0),
    ultimo_erro DATETIME2, 

    CONSTRAINT PK_credencial PRIMARY KEY (id),
    CONSTRAINT FK_credencial_cliente FOREIGN KEY (cliente_id) REFERENCES cliente
)
CREATE INDEX IDX_credencial_senha ON credencial (senha);


--   fim cadastro   --

--   pedido  --


CREATE TABLE produto (
    id bigint NOT NULL IDENTITY(1, 1),
    titulo VARCHAR(128) NOT NULL,
    descricao VARCHAR(2048),
    categoria VARCHAR(128) NOT NULL,
    preco DECIMAL NOT NULL,
    quantidade INT NOT NULL,

    CONSTRAINT PK_produto PRIMARY KEY (id),
    CONSTRAINT CHK_produto_preco CHECK (preco >= 0),
    CONSTRAINT CHK_produto_quantidade CHECK (quantidade >= 0)
)


CREATE TABLE produto_foto (
    id bigint NOT NULL IDENTITY(1, 1),
    produto_id bigint NOT NULL,
    caminho VARCHAR(256) NOT NULL,

    CONSTRAINT PK_produto_foto PRIMARY KEY (id),
    CONSTRAINT FK_pedido_foto_produto FOREIGN KEY (produto_id) REFERENCES produto
)


CREATE TABLE pedido_situacao (
    id INT NOT NULL IDENTITY(1, 1),
    descricao VARCHAR(256) NOT NULL,

    CONSTRAINT PK_pedido_situacao PRIMARY KEY (id)
)
INSERT INTO pedido_situacao (descricao)
VALUES ('Aguardando pagamento'), ('Aguardando separação'), 
    ('Aguardando envio'), ('Pedido enviado'), ('Pedido concluído');


CREATE TABLE pedido (
    id bigint NOT NULL IDENTITY(1, 1),
    cliente_id bigint NOT NULL,
    emissao DATETIME2 NOT NULL DEFAULT(getdate()),
    situacao_id INT NOT NULL

    CONSTRAINT PK_pedido PRIMARY KEY (id),    
    CONSTRAINT FK_pedido_cliente FOREIGN KEY (cliente_id) REFERENCES cliente,
    CONSTRAINT FK_pedido_situacao FOREIGN KEY (situacao_id) REFERENCES pedido_situacao,
)


CREATE TABLE pedido_item (
    id bigint NOT NULL IDENTITY(1, 1),
    pedido_id bigint NOT NULL,
    produto_id bigint NOT NULL,
    preco DECIMAL NOT NULL,
    desconto DECIMAL NOT NULL,
    quantidade INT NOT NULL,

    CONSTRAINT PK_pedido_item PRIMARY KEY (id),
    CONSTRAINT FK_pedido_item_pedido FOREIGN KEY (pedido_id) REFERENCES pedido,
    CONSTRAINT FK_pedido_item_produto FOREIGN KEY (produto_id) REFERENCES produto,
    CONSTRAINT CHK_pedido_item_preco CHECK (preco >= 0),
    CONSTRAINT CHK_pedido_item_quantidade CHECK (quantidade >= 0)
)


--   fim pedido  --