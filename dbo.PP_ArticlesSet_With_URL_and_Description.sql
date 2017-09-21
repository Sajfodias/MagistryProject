CREATE TABLE [dbo].[PP_ArticlesSet] (
    [article_Id]          INT            IDENTITY (1, 1) NOT NULL,
    [article_author_line] NVARCHAR (MAX) NOT NULL,
    [article_title]       NVARCHAR (MAX) NOT NULL,
    [article_source]      NVARCHAR (MAX) NOT NULL,
    [article_year]        INT            NOT NULL,
    [article_language]    NVARCHAR (MAX) NOT NULL,
    [article_DOI]         NVARCHAR (MAX) NOT NULL,
    [article_details]     NVARCHAR (MAX) NOT NULL,
    [article_URL]         NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PP_ArticlesSet] PRIMARY KEY CLUSTERED ([article_Id] ASC)
);

