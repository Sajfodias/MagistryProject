
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/08/2019 19:56:45
-- Generated from EDMX file: D:\personal_proj\oldMagistryProj\MagistryProject\Wyszukiwarka_publikacji_v0.2\ArticleDBDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ArticleProjDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PG_ArticlesAuthor_Author]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PG_ArticlesAuthor] DROP CONSTRAINT [FK_PG_ArticlesAuthor_Author];
GO
IF OBJECT_ID(N'[dbo].[FK_PG_ArticlesAuthor_PG_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PG_ArticlesAuthor] DROP CONSTRAINT [FK_PG_ArticlesAuthor_PG_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_PP_ArticlesAuthor_Author]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PP_ArticlesAuthor] DROP CONSTRAINT [FK_PP_ArticlesAuthor_Author];
GO
IF OBJECT_ID(N'[dbo].[FK_PP_ArticlesAuthor_PP_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PP_ArticlesAuthor] DROP CONSTRAINT [FK_PP_ArticlesAuthor_PP_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyPG_Articles_PG_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyPG_Articles] DROP CONSTRAINT [FK_Terms_VocabularyPG_Articles_PG_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyPG_Articles_Terms_Vocabulary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyPG_Articles] DROP CONSTRAINT [FK_Terms_VocabularyPG_Articles_Terms_Vocabulary];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyPP_Articles_PP_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyPP_Articles] DROP CONSTRAINT [FK_Terms_VocabularyPP_Articles_PP_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyPP_Articles_Terms_Vocabulary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyPP_Articles] DROP CONSTRAINT [FK_Terms_VocabularyPP_Articles_Terms_Vocabulary];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyUG_Articles_Terms_Vocabulary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyUG_Articles] DROP CONSTRAINT [FK_Terms_VocabularyUG_Articles_Terms_Vocabulary];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyUG_Articles_UG_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyUG_Articles] DROP CONSTRAINT [FK_Terms_VocabularyUG_Articles_UG_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyUMK_Articles_Terms_Vocabulary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyUMK_Articles] DROP CONSTRAINT [FK_Terms_VocabularyUMK_Articles_Terms_Vocabulary];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyUMK_Articles_UMK_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyUMK_Articles] DROP CONSTRAINT [FK_Terms_VocabularyUMK_Articles_UMK_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyWSB_Articles_Terms_Vocabulary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyWSB_Articles] DROP CONSTRAINT [FK_Terms_VocabularyWSB_Articles_Terms_Vocabulary];
GO
IF OBJECT_ID(N'[dbo].[FK_Terms_VocabularyWSB_Articles_WSB_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Terms_VocabularyWSB_Articles] DROP CONSTRAINT [FK_Terms_VocabularyWSB_Articles_WSB_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_UG_ArticlesAuthor_Author]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UG_ArticlesAuthor] DROP CONSTRAINT [FK_UG_ArticlesAuthor_Author];
GO
IF OBJECT_ID(N'[dbo].[FK_UG_ArticlesAuthor_UG_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UG_ArticlesAuthor] DROP CONSTRAINT [FK_UG_ArticlesAuthor_UG_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_UMK_ArticlesAuthor_Author]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UMK_ArticlesAuthor] DROP CONSTRAINT [FK_UMK_ArticlesAuthor_Author];
GO
IF OBJECT_ID(N'[dbo].[FK_UMK_ArticlesAuthor_UMK_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UMK_ArticlesAuthor] DROP CONSTRAINT [FK_UMK_ArticlesAuthor_UMK_Articles];
GO
IF OBJECT_ID(N'[dbo].[FK_WSB_ArticlesAuthor_Author]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WSB_ArticlesAuthor] DROP CONSTRAINT [FK_WSB_ArticlesAuthor_Author];
GO
IF OBJECT_ID(N'[dbo].[FK_WSB_ArticlesAuthor_WSB_Articles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WSB_ArticlesAuthor] DROP CONSTRAINT [FK_WSB_ArticlesAuthor_WSB_Articles];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AuthorSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuthorSet];
GO
IF OBJECT_ID(N'[dbo].[PG_ArticlesAuthor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PG_ArticlesAuthor];
GO
IF OBJECT_ID(N'[dbo].[PG_ArticlesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PG_ArticlesSet];
GO
IF OBJECT_ID(N'[dbo].[PP_ArticlesAuthor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PP_ArticlesAuthor];
GO
IF OBJECT_ID(N'[dbo].[PP_ArticlesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PP_ArticlesSet];
GO
IF OBJECT_ID(N'[dbo].[Terms_Vocabulary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms_Vocabulary];
GO
IF OBJECT_ID(N'[dbo].[Terms_VocabularyPG_Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms_VocabularyPG_Articles];
GO
IF OBJECT_ID(N'[dbo].[Terms_VocabularyPP_Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms_VocabularyPP_Articles];
GO
IF OBJECT_ID(N'[dbo].[Terms_VocabularyUG_Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms_VocabularyUG_Articles];
GO
IF OBJECT_ID(N'[dbo].[Terms_VocabularyUMK_Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms_VocabularyUMK_Articles];
GO
IF OBJECT_ID(N'[dbo].[Terms_VocabularyWSB_Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms_VocabularyWSB_Articles];
GO
IF OBJECT_ID(N'[dbo].[UG_ArticlesAuthor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UG_ArticlesAuthor];
GO
IF OBJECT_ID(N'[dbo].[UG_ArticlesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UG_ArticlesSet];
GO
IF OBJECT_ID(N'[dbo].[UMK_ArticlesAuthor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMK_ArticlesAuthor];
GO
IF OBJECT_ID(N'[dbo].[UMK_ArticlesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMK_ArticlesSet];
GO
IF OBJECT_ID(N'[dbo].[WSB_ArticlesAuthor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WSB_ArticlesAuthor];
GO
IF OBJECT_ID(N'[dbo].[WSB_ArticlesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WSB_ArticlesSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AuthorSet'
CREATE TABLE [dbo].[AuthorSet] (
    [author_Id] int IDENTITY(1,1) NOT NULL,
    [author_name] nvarchar(max)  NOT NULL,
    [author_surename] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PG_ArticlesSet'
CREATE TABLE [dbo].[PG_ArticlesSet] (
    [article_Id] int IDENTITY(1,1) NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [abstractText] nvarchar(max)  NOT NULL,
    [keywords] nvarchar(max)  NOT NULL,
    [year] int  NOT NULL,
    [country] nvarchar(max)  NOT NULL,
    [authors] nvarchar(max)  NOT NULL,
    [organizations] nvarchar(max)  NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'PP_ArticlesSet'
CREATE TABLE [dbo].[PP_ArticlesSet] (
    [article_Id] int IDENTITY(1,1) NOT NULL,
    [article_author_line] nvarchar(max)  NOT NULL,
    [article_title] nvarchar(max)  NOT NULL,
    [article_source] nvarchar(max)  NOT NULL,
    [article_year] int  NOT NULL,
    [article_language] nvarchar(max)  NOT NULL,
    [article_DOI] nvarchar(max)  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'Terms_Vocabulary'
CREATE TABLE [dbo].[Terms_Vocabulary] (
    [terms_Id] int IDENTITY(1,1) NOT NULL,
    [term_value] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'UG_ArticlesSet'
CREATE TABLE [dbo].[UG_ArticlesSet] (
    [article_Id] int IDENTITY(1,1) NOT NULL,
    [article_author_line] nvarchar(max)  NOT NULL,
    [article_title] nvarchar(max)  NOT NULL,
    [article_source] nvarchar(max)  NOT NULL,
    [article_keywords] nvarchar(max)  NOT NULL,
    [article_DOI] nvarchar(max)  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'UMK_ArticlesSet'
CREATE TABLE [dbo].[UMK_ArticlesSet] (
    [article_Id] int IDENTITY(1,1) NOT NULL,
    [article_author_line] nvarchar(max)  NOT NULL,
    [article_title] nvarchar(max)  NOT NULL,
    [article_language] nvarchar(max)  NOT NULL,
    [article_Full_title] nvarchar(max)  NOT NULL,
    [article_pl_keywords] nvarchar(max)  NOT NULL,
    [article_eng_keywords] nvarchar(max)  NOT NULL,
    [article_translated_title] nvarchar(max)  NOT NULL,
    [article_url] nvarchar(max)  NOT NULL,
    [article_publisher_desc] nvarchar(max)  NOT NULL,
    [article_publisher_title] nvarchar(max)  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'WSB_ArticlesSet'
CREATE TABLE [dbo].[WSB_ArticlesSet] (
    [article_Id] int IDENTITY(1,1) NOT NULL,
    [article_authors] nvarchar(max)  NOT NULL,
    [article_title] nvarchar(max)  NOT NULL,
    [article_publisher_adres] nvarchar(max)  NOT NULL,
    [article_common_title] nvarchar(max)  NOT NULL,
    [article_eng_keywords] nvarchar(max)  NOT NULL,
    [article_pl_keywords] nvarchar(max)  NOT NULL,
    [article_title_other_lang] nvarchar(max)  NOT NULL,
    [article_DOI] nvarchar(max)  NOT NULL,
    [article_details] nvarchar(max)  NOT NULL,
    [article_URL] nvarchar(max)  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'PG_ArticlesAuthor'
CREATE TABLE [dbo].[PG_ArticlesAuthor] (
    [AuthorSet_author_Id] int  NOT NULL,
    [PG_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'PP_ArticlesAuthor'
CREATE TABLE [dbo].[PP_ArticlesAuthor] (
    [AuthorSet_author_Id] int  NOT NULL,
    [PP_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'Terms_VocabularyPG_Articles'
CREATE TABLE [dbo].[Terms_VocabularyPG_Articles] (
    [PG_ArticlesSet_article_Id] int  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'Terms_VocabularyPP_Articles'
CREATE TABLE [dbo].[Terms_VocabularyPP_Articles] (
    [PP_ArticlesSet_article_Id] int  NOT NULL,
    [Terms_Vocabulary_terms_Id] int  NOT NULL
);
GO

-- Creating table 'Terms_VocabularyUG_Articles'
CREATE TABLE [dbo].[Terms_VocabularyUG_Articles] (
    [Terms_Vocabulary_terms_Id] int  NOT NULL,
    [UG_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'Terms_VocabularyUMK_Articles'
CREATE TABLE [dbo].[Terms_VocabularyUMK_Articles] (
    [Terms_Vocabulary_terms_Id] int  NOT NULL,
    [UMK_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'Terms_VocabularyWSB_Articles'
CREATE TABLE [dbo].[Terms_VocabularyWSB_Articles] (
    [Terms_Vocabulary_terms_Id] int  NOT NULL,
    [WSB_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'UG_ArticlesAuthor'
CREATE TABLE [dbo].[UG_ArticlesAuthor] (
    [AuthorSet_author_Id] int  NOT NULL,
    [UG_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'UMK_ArticlesAuthor'
CREATE TABLE [dbo].[UMK_ArticlesAuthor] (
    [AuthorSet_author_Id] int  NOT NULL,
    [UMK_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- Creating table 'WSB_ArticlesAuthor'
CREATE TABLE [dbo].[WSB_ArticlesAuthor] (
    [AuthorSet_author_Id] int  NOT NULL,
    [WSB_ArticlesSet_article_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [author_Id] in table 'AuthorSet'
ALTER TABLE [dbo].[AuthorSet]
ADD CONSTRAINT [PK_AuthorSet]
    PRIMARY KEY CLUSTERED ([author_Id] ASC);
GO

-- Creating primary key on [article_Id] in table 'PG_ArticlesSet'
ALTER TABLE [dbo].[PG_ArticlesSet]
ADD CONSTRAINT [PK_PG_ArticlesSet]
    PRIMARY KEY CLUSTERED ([article_Id] ASC);
GO

-- Creating primary key on [article_Id] in table 'PP_ArticlesSet'
ALTER TABLE [dbo].[PP_ArticlesSet]
ADD CONSTRAINT [PK_PP_ArticlesSet]
    PRIMARY KEY CLUSTERED ([article_Id] ASC);
GO

-- Creating primary key on [terms_Id] in table 'Terms_Vocabulary'
ALTER TABLE [dbo].[Terms_Vocabulary]
ADD CONSTRAINT [PK_Terms_Vocabulary]
    PRIMARY KEY CLUSTERED ([terms_Id] ASC);
GO

-- Creating primary key on [article_Id] in table 'UG_ArticlesSet'
ALTER TABLE [dbo].[UG_ArticlesSet]
ADD CONSTRAINT [PK_UG_ArticlesSet]
    PRIMARY KEY CLUSTERED ([article_Id] ASC);
GO

-- Creating primary key on [article_Id] in table 'UMK_ArticlesSet'
ALTER TABLE [dbo].[UMK_ArticlesSet]
ADD CONSTRAINT [PK_UMK_ArticlesSet]
    PRIMARY KEY CLUSTERED ([article_Id] ASC);
GO

-- Creating primary key on [article_Id] in table 'WSB_ArticlesSet'
ALTER TABLE [dbo].[WSB_ArticlesSet]
ADD CONSTRAINT [PK_WSB_ArticlesSet]
    PRIMARY KEY CLUSTERED ([article_Id] ASC);
GO

-- Creating primary key on [AuthorSet_author_Id], [PG_ArticlesSet_article_Id] in table 'PG_ArticlesAuthor'
ALTER TABLE [dbo].[PG_ArticlesAuthor]
ADD CONSTRAINT [PK_PG_ArticlesAuthor]
    PRIMARY KEY CLUSTERED ([AuthorSet_author_Id], [PG_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [AuthorSet_author_Id], [PP_ArticlesSet_article_Id] in table 'PP_ArticlesAuthor'
ALTER TABLE [dbo].[PP_ArticlesAuthor]
ADD CONSTRAINT [PK_PP_ArticlesAuthor]
    PRIMARY KEY CLUSTERED ([AuthorSet_author_Id], [PP_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [PG_ArticlesSet_article_Id], [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyPG_Articles'
ALTER TABLE [dbo].[Terms_VocabularyPG_Articles]
ADD CONSTRAINT [PK_Terms_VocabularyPG_Articles]
    PRIMARY KEY CLUSTERED ([PG_ArticlesSet_article_Id], [Terms_Vocabulary_terms_Id] ASC);
GO

-- Creating primary key on [PP_ArticlesSet_article_Id], [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyPP_Articles'
ALTER TABLE [dbo].[Terms_VocabularyPP_Articles]
ADD CONSTRAINT [PK_Terms_VocabularyPP_Articles]
    PRIMARY KEY CLUSTERED ([PP_ArticlesSet_article_Id], [Terms_Vocabulary_terms_Id] ASC);
GO

-- Creating primary key on [Terms_Vocabulary_terms_Id], [UG_ArticlesSet_article_Id] in table 'Terms_VocabularyUG_Articles'
ALTER TABLE [dbo].[Terms_VocabularyUG_Articles]
ADD CONSTRAINT [PK_Terms_VocabularyUG_Articles]
    PRIMARY KEY CLUSTERED ([Terms_Vocabulary_terms_Id], [UG_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [Terms_Vocabulary_terms_Id], [UMK_ArticlesSet_article_Id] in table 'Terms_VocabularyUMK_Articles'
ALTER TABLE [dbo].[Terms_VocabularyUMK_Articles]
ADD CONSTRAINT [PK_Terms_VocabularyUMK_Articles]
    PRIMARY KEY CLUSTERED ([Terms_Vocabulary_terms_Id], [UMK_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [Terms_Vocabulary_terms_Id], [WSB_ArticlesSet_article_Id] in table 'Terms_VocabularyWSB_Articles'
ALTER TABLE [dbo].[Terms_VocabularyWSB_Articles]
ADD CONSTRAINT [PK_Terms_VocabularyWSB_Articles]
    PRIMARY KEY CLUSTERED ([Terms_Vocabulary_terms_Id], [WSB_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [AuthorSet_author_Id], [UG_ArticlesSet_article_Id] in table 'UG_ArticlesAuthor'
ALTER TABLE [dbo].[UG_ArticlesAuthor]
ADD CONSTRAINT [PK_UG_ArticlesAuthor]
    PRIMARY KEY CLUSTERED ([AuthorSet_author_Id], [UG_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [AuthorSet_author_Id], [UMK_ArticlesSet_article_Id] in table 'UMK_ArticlesAuthor'
ALTER TABLE [dbo].[UMK_ArticlesAuthor]
ADD CONSTRAINT [PK_UMK_ArticlesAuthor]
    PRIMARY KEY CLUSTERED ([AuthorSet_author_Id], [UMK_ArticlesSet_article_Id] ASC);
GO

-- Creating primary key on [AuthorSet_author_Id], [WSB_ArticlesSet_article_Id] in table 'WSB_ArticlesAuthor'
ALTER TABLE [dbo].[WSB_ArticlesAuthor]
ADD CONSTRAINT [PK_WSB_ArticlesAuthor]
    PRIMARY KEY CLUSTERED ([AuthorSet_author_Id], [WSB_ArticlesSet_article_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AuthorSet_author_Id] in table 'PG_ArticlesAuthor'
ALTER TABLE [dbo].[PG_ArticlesAuthor]
ADD CONSTRAINT [FK_PG_ArticlesAuthor_AuthorSet]
    FOREIGN KEY ([AuthorSet_author_Id])
    REFERENCES [dbo].[AuthorSet]
        ([author_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PG_ArticlesSet_article_Id] in table 'PG_ArticlesAuthor'
ALTER TABLE [dbo].[PG_ArticlesAuthor]
ADD CONSTRAINT [FK_PG_ArticlesAuthor_PG_ArticlesSet]
    FOREIGN KEY ([PG_ArticlesSet_article_Id])
    REFERENCES [dbo].[PG_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PG_ArticlesAuthor_PG_ArticlesSet'
CREATE INDEX [IX_FK_PG_ArticlesAuthor_PG_ArticlesSet]
ON [dbo].[PG_ArticlesAuthor]
    ([PG_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [AuthorSet_author_Id] in table 'PP_ArticlesAuthor'
ALTER TABLE [dbo].[PP_ArticlesAuthor]
ADD CONSTRAINT [FK_PP_ArticlesAuthor_AuthorSet]
    FOREIGN KEY ([AuthorSet_author_Id])
    REFERENCES [dbo].[AuthorSet]
        ([author_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PP_ArticlesSet_article_Id] in table 'PP_ArticlesAuthor'
ALTER TABLE [dbo].[PP_ArticlesAuthor]
ADD CONSTRAINT [FK_PP_ArticlesAuthor_PP_ArticlesSet]
    FOREIGN KEY ([PP_ArticlesSet_article_Id])
    REFERENCES [dbo].[PP_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PP_ArticlesAuthor_PP_ArticlesSet'
CREATE INDEX [IX_FK_PP_ArticlesAuthor_PP_ArticlesSet]
ON [dbo].[PP_ArticlesAuthor]
    ([PP_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [PG_ArticlesSet_article_Id] in table 'Terms_VocabularyPG_Articles'
ALTER TABLE [dbo].[Terms_VocabularyPG_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyPG_Articles_PG_ArticlesSet]
    FOREIGN KEY ([PG_ArticlesSet_article_Id])
    REFERENCES [dbo].[PG_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyPG_Articles'
ALTER TABLE [dbo].[Terms_VocabularyPG_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyPG_Articles_Terms_Vocabulary]
    FOREIGN KEY ([Terms_Vocabulary_terms_Id])
    REFERENCES [dbo].[Terms_Vocabulary]
        ([terms_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Terms_VocabularyPG_Articles_Terms_Vocabulary'
CREATE INDEX [IX_FK_Terms_VocabularyPG_Articles_Terms_Vocabulary]
ON [dbo].[Terms_VocabularyPG_Articles]
    ([Terms_Vocabulary_terms_Id]);
GO

-- Creating foreign key on [PP_ArticlesSet_article_Id] in table 'Terms_VocabularyPP_Articles'
ALTER TABLE [dbo].[Terms_VocabularyPP_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyPP_Articles_PP_ArticlesSet]
    FOREIGN KEY ([PP_ArticlesSet_article_Id])
    REFERENCES [dbo].[PP_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyPP_Articles'
ALTER TABLE [dbo].[Terms_VocabularyPP_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyPP_Articles_Terms_Vocabulary]
    FOREIGN KEY ([Terms_Vocabulary_terms_Id])
    REFERENCES [dbo].[Terms_Vocabulary]
        ([terms_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Terms_VocabularyPP_Articles_Terms_Vocabulary'
CREATE INDEX [IX_FK_Terms_VocabularyPP_Articles_Terms_Vocabulary]
ON [dbo].[Terms_VocabularyPP_Articles]
    ([Terms_Vocabulary_terms_Id]);
GO

-- Creating foreign key on [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyUG_Articles'
ALTER TABLE [dbo].[Terms_VocabularyUG_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyUG_Articles_Terms_Vocabulary]
    FOREIGN KEY ([Terms_Vocabulary_terms_Id])
    REFERENCES [dbo].[Terms_Vocabulary]
        ([terms_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UG_ArticlesSet_article_Id] in table 'Terms_VocabularyUG_Articles'
ALTER TABLE [dbo].[Terms_VocabularyUG_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyUG_Articles_UG_ArticlesSet]
    FOREIGN KEY ([UG_ArticlesSet_article_Id])
    REFERENCES [dbo].[UG_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Terms_VocabularyUG_Articles_UG_ArticlesSet'
CREATE INDEX [IX_FK_Terms_VocabularyUG_Articles_UG_ArticlesSet]
ON [dbo].[Terms_VocabularyUG_Articles]
    ([UG_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyUMK_Articles'
ALTER TABLE [dbo].[Terms_VocabularyUMK_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyUMK_Articles_Terms_Vocabulary]
    FOREIGN KEY ([Terms_Vocabulary_terms_Id])
    REFERENCES [dbo].[Terms_Vocabulary]
        ([terms_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UMK_ArticlesSet_article_Id] in table 'Terms_VocabularyUMK_Articles'
ALTER TABLE [dbo].[Terms_VocabularyUMK_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyUMK_Articles_UMK_ArticlesSet]
    FOREIGN KEY ([UMK_ArticlesSet_article_Id])
    REFERENCES [dbo].[UMK_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Terms_VocabularyUMK_Articles_UMK_ArticlesSet'
CREATE INDEX [IX_FK_Terms_VocabularyUMK_Articles_UMK_ArticlesSet]
ON [dbo].[Terms_VocabularyUMK_Articles]
    ([UMK_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [Terms_Vocabulary_terms_Id] in table 'Terms_VocabularyWSB_Articles'
ALTER TABLE [dbo].[Terms_VocabularyWSB_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyWSB_Articles_Terms_Vocabulary]
    FOREIGN KEY ([Terms_Vocabulary_terms_Id])
    REFERENCES [dbo].[Terms_Vocabulary]
        ([terms_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [WSB_ArticlesSet_article_Id] in table 'Terms_VocabularyWSB_Articles'
ALTER TABLE [dbo].[Terms_VocabularyWSB_Articles]
ADD CONSTRAINT [FK_Terms_VocabularyWSB_Articles_WSB_ArticlesSet]
    FOREIGN KEY ([WSB_ArticlesSet_article_Id])
    REFERENCES [dbo].[WSB_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Terms_VocabularyWSB_Articles_WSB_ArticlesSet'
CREATE INDEX [IX_FK_Terms_VocabularyWSB_Articles_WSB_ArticlesSet]
ON [dbo].[Terms_VocabularyWSB_Articles]
    ([WSB_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [AuthorSet_author_Id] in table 'UG_ArticlesAuthor'
ALTER TABLE [dbo].[UG_ArticlesAuthor]
ADD CONSTRAINT [FK_UG_ArticlesAuthor_AuthorSet]
    FOREIGN KEY ([AuthorSet_author_Id])
    REFERENCES [dbo].[AuthorSet]
        ([author_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UG_ArticlesSet_article_Id] in table 'UG_ArticlesAuthor'
ALTER TABLE [dbo].[UG_ArticlesAuthor]
ADD CONSTRAINT [FK_UG_ArticlesAuthor_UG_ArticlesSet]
    FOREIGN KEY ([UG_ArticlesSet_article_Id])
    REFERENCES [dbo].[UG_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UG_ArticlesAuthor_UG_ArticlesSet'
CREATE INDEX [IX_FK_UG_ArticlesAuthor_UG_ArticlesSet]
ON [dbo].[UG_ArticlesAuthor]
    ([UG_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [AuthorSet_author_Id] in table 'UMK_ArticlesAuthor'
ALTER TABLE [dbo].[UMK_ArticlesAuthor]
ADD CONSTRAINT [FK_UMK_ArticlesAuthor_AuthorSet]
    FOREIGN KEY ([AuthorSet_author_Id])
    REFERENCES [dbo].[AuthorSet]
        ([author_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UMK_ArticlesSet_article_Id] in table 'UMK_ArticlesAuthor'
ALTER TABLE [dbo].[UMK_ArticlesAuthor]
ADD CONSTRAINT [FK_UMK_ArticlesAuthor_UMK_ArticlesSet]
    FOREIGN KEY ([UMK_ArticlesSet_article_Id])
    REFERENCES [dbo].[UMK_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UMK_ArticlesAuthor_UMK_ArticlesSet'
CREATE INDEX [IX_FK_UMK_ArticlesAuthor_UMK_ArticlesSet]
ON [dbo].[UMK_ArticlesAuthor]
    ([UMK_ArticlesSet_article_Id]);
GO

-- Creating foreign key on [AuthorSet_author_Id] in table 'WSB_ArticlesAuthor'
ALTER TABLE [dbo].[WSB_ArticlesAuthor]
ADD CONSTRAINT [FK_WSB_ArticlesAuthor_AuthorSet]
    FOREIGN KEY ([AuthorSet_author_Id])
    REFERENCES [dbo].[AuthorSet]
        ([author_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [WSB_ArticlesSet_article_Id] in table 'WSB_ArticlesAuthor'
ALTER TABLE [dbo].[WSB_ArticlesAuthor]
ADD CONSTRAINT [FK_WSB_ArticlesAuthor_WSB_ArticlesSet]
    FOREIGN KEY ([WSB_ArticlesSet_article_Id])
    REFERENCES [dbo].[WSB_ArticlesSet]
        ([article_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WSB_ArticlesAuthor_WSB_ArticlesSet'
CREATE INDEX [IX_FK_WSB_ArticlesAuthor_WSB_ArticlesSet]
ON [dbo].[WSB_ArticlesAuthor]
    ([WSB_ArticlesSet_article_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------