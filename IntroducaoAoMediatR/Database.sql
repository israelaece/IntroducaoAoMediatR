CREATE TABLE [dbo].[Produto] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [Descricao] VARCHAR (50)    NOT NULL,
    [Valor]     DECIMAL (18, 2) NOT NULL,
    [Imagem]    VARCHAR (MAX)   NULL
);

CREATE TABLE [dbo].[Pedido] (
    [Id]    UNIQUEIDENTIFIER NOT NULL,
    [Data]  DATETIME         NOT NULL,
    [Total] DECIMAL (18, 2)  NOT NULL
);

CREATE TABLE [dbo].[PedidoItem] (
    [PedidoId]   UNIQUEIDENTIFIER NOT NULL,
    [ProdutoId]  INT              NOT NULL,
    [Quantidade] INT              NOT NULL,
    [Total]      DECIMAL (18, 2)  NOT NULL
);

CREATE TABLE [dbo].[NotaFiscal] (
    [Codigo] UNIQUEIDENTIFIER NOT NULL,
    [Data]   DATETIME         NOT NULL,
    [Valor]  DECIMAL (18, 2)  NOT NULL
);
