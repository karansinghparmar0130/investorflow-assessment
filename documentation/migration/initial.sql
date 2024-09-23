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

CREATE TABLE [Funds] (
    [Id] int NOT NULL IDENTITY,
    [ExternalId] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Funds] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Contacts] (
    [Id] int NOT NULL IDENTITY,
    [ExternalId] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(150) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [FundId] int NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Contacts_Funds_FundId] FOREIGN KEY ([FundId]) REFERENCES [Funds] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ExternalId', N'Name') AND [object_id] = OBJECT_ID(N'[Funds]'))
    SET IDENTITY_INSERT [Funds] ON;
INSERT INTO [Funds] ([Id], [ExternalId], [Name])
VALUES (1, '8c315b74-f063-48b0-a479-35763535075c', N'Global Growth Fund'),
(2, '9ecb2c37-9a59-44f7-9dc4-fcafa2d79389', N'Emerging Markets Fund'),
(3, '6d4e7bf2-dd14-4032-9c72-7eceb0d663a4', N'Small Business Fund');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ExternalId', N'Name') AND [object_id] = OBJECT_ID(N'[Funds]'))
    SET IDENTITY_INSERT [Funds] OFF;
GO

CREATE UNIQUE INDEX [IX_Contacts_ExternalId] ON [Contacts] ([ExternalId]);
GO

CREATE INDEX [IX_Contacts_FundId] ON [Contacts] ([FundId]);
GO

CREATE INDEX [IX_Funds_ExternalId] ON [Funds] ([ExternalId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240925104655_Initial', N'8.0.8');
GO

COMMIT;
GO

