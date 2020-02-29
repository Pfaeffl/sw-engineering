
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/05/2020 14:37:30
-- Generated from EDMX file: C:\Users\I2CM Developer\Source\Repos\Lagerverwaltung\LagerverwaltungLib\LagerverwaltungModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Lagerverwaltung];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserMaterial_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserMaterial] DROP CONSTRAINT [FK_UserMaterial_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserMaterial_Material]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserMaterial] DROP CONSTRAINT [FK_UserMaterial_Material];
GO
IF OBJECT_ID(N'[dbo].[FK_MaterialLagerort]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Materials] DROP CONSTRAINT [FK_MaterialLagerort];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Materials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Materials];
GO
IF OBJECT_ID(N'[dbo].[Lagerorts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lagerorts];
GO
IF OBJECT_ID(N'[dbo].[UserMaterial]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserMaterial];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Vorname] nvarchar(max)  NOT NULL,
    [Personalnummer] nvarchar(max)  NOT NULL,
    [Lagerist] bit  NOT NULL,
    [Besteller] bit  NOT NULL,
    [Personal] bit  NOT NULL,
    [Systemadm] bit  NOT NULL
);
GO

-- Creating table 'Materials'
CREATE TABLE [dbo].[Materials] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LagerortId] int  NOT NULL,
    [Materialnummer] nvarchar(max)  NOT NULL,
    [Materialbezeichnung] nvarchar(max)  NOT NULL,
    [Standardpreis] float  NOT NULL,
    [Bestand] int  NOT NULL,
    [Mindestbestand] int  NOT NULL,
    [Bestellmenge] int  NOT NULL
);
GO

-- Creating table 'Lagerorts'
CREATE TABLE [dbo].[Lagerorts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserMaterial'
CREATE TABLE [dbo].[UserMaterial] (
    [Users_Id] int  NOT NULL,
    [Materials_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Materials'
ALTER TABLE [dbo].[Materials]
ADD CONSTRAINT [PK_Materials]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lagerorts'
ALTER TABLE [dbo].[Lagerorts]
ADD CONSTRAINT [PK_Lagerorts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [Materials_Id] in table 'UserMaterial'
ALTER TABLE [dbo].[UserMaterial]
ADD CONSTRAINT [PK_UserMaterial]
    PRIMARY KEY CLUSTERED ([Users_Id], [Materials_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Users_Id] in table 'UserMaterial'
ALTER TABLE [dbo].[UserMaterial]
ADD CONSTRAINT [FK_UserMaterial_User]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Materials_Id] in table 'UserMaterial'
ALTER TABLE [dbo].[UserMaterial]
ADD CONSTRAINT [FK_UserMaterial_Material]
    FOREIGN KEY ([Materials_Id])
    REFERENCES [dbo].[Materials]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserMaterial_Material'
CREATE INDEX [IX_FK_UserMaterial_Material]
ON [dbo].[UserMaterial]
    ([Materials_Id]);
GO

-- Creating foreign key on [LagerortId] in table 'Materials'
ALTER TABLE [dbo].[Materials]
ADD CONSTRAINT [FK_MaterialLagerort]
    FOREIGN KEY ([LagerortId])
    REFERENCES [dbo].[Lagerorts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialLagerort'
CREATE INDEX [IX_FK_MaterialLagerort]
ON [dbo].[Materials]
    ([LagerortId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------