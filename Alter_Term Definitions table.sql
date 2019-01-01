USE [ArticleProjDB]
ALTER TABLE [dbo].[Terms_Vocabulary]
	ADD CONSTRAINT UQ_Term UNIQUE (term_value);
GO