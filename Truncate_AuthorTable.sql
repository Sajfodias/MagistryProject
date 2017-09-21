USE ArticlesDatabase;
GO
SELECT COUNT(*) AS BeforeTruncateCount 
FROM AuthorSet;
GO
TRUNCATE TABLE AuthorSet;
GO
SELECT COUNT(*) AS AfterTruncateCount 
FROM AuthorSet;
GO