																						  IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Leads] (
    [Id] int NOT NULL IDENTITY,
    [CPF] nvarchar(11) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [Nome] nvarchar(150) NOT NULL,
    [Telefone] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Leads] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Ofertas] (
    [Id] int NOT NULL IDENTITY,
    [Descricao] nvarchar(500) NOT NULL,
    [Nome] nvarchar(150) NOT NULL,
    [VagasDisponiveis] int NOT NULL,
    CONSTRAINT [PK_Ofertas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProcessosSeletivos] (
    [Id] int NOT NULL IDENTITY,
    [DataInicio] datetime2 NOT NULL,
    [DataTermino] datetime2 NOT NULL,
    [Nome] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_ProcessosSeletivos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Inscricoes] (
    [Id] int NOT NULL IDENTITY,
    [Data] datetime2 NOT NULL,
    [LeadId] int NOT NULL,
    [NumeroInscricao] nvarchar(50) NOT NULL,
    [OfertaId] int NOT NULL,
    [ProcessoSeletivoId] int NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Inscricoes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Inscricoes_Leads_LeadId] FOREIGN KEY ([LeadId]) REFERENCES [Leads] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Inscricoes_Ofertas_OfertaId] FOREIGN KEY ([OfertaId]) REFERENCES [Ofertas] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Inscricoes_ProcessosSeletivos_ProcessoSeletivoId] FOREIGN KEY ([ProcessoSeletivoId]) REFERENCES [ProcessosSeletivos] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Inscricoes_LeadId] ON [Inscricoes] ([LeadId]);
GO

CREATE INDEX [IX_Inscricoes_OfertaId] ON [Inscricoes] ([OfertaId]);
GO

CREATE INDEX [IX_Inscricoes_ProcessoSeletivoId] ON [Inscricoes] ([ProcessoSeletivoId]);
GO

CREATE UNIQUE INDEX [IX_Leads_CPF] ON [Leads] ([CPF]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240810165707_AdicionandoEntidades', N'8.0.3');
GO

COMMIT;
GO