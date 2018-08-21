WITH Terms_Duplicate AS(
SELECT *, ROW_NUMBER() OVER(Partition BY term_value order by terms_id) as RowNumber FROM [ArticleProjDB].dbo.Terms_Vocabulary
)

DELETE FROM Terms_VocabularyPG_Articles WHERE Terms_VocabularyPG_Articles.[Terms_Vocabulary_terms_Id] = Terms_Duplicate.terms_Id AND Terms_Duplicate.RowNumber > 1
DELETE FROM Terms_Duplicate where RowNumber > 1

CREATE table #Color(terms_id INTEGER, term_value VARCHAR(MAX), RowNumber INTEGER)
SELECT *, ROW_NUMBER() OVER(Partition BY term_value order by terms_id) as RowNumber FROM [ArticleProjDB].dbo.Terms_Vocabulary

DELETE FROM ArticleProjDB.dbo.Terms_VocabularyPG_Articles WHERE Terms_Vocabulary_terms_Id = #Color.terms_id AND #Color.RowNumber>1

SELECT *,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
       REPLACE(REPLACE(REPLACE(REPLACE([ArticleProjDB].dbo.Terms_Vocabulary.term_value,
        '!',''),'@',''),'#',''),'$',''),'%',''),
        '^',''),'&',''),'*',''),' ','') FROM [ArticleProjDB].dbo.Terms_Vocabulary


SELECT * FROM [ArticleProjDB].dbo.Terms_Vocabulary

--DELETE FROM [ArticleProjDB].dbo.Terms_Vocabulary WHERE term_value LIKE REGEXP ('^[A-Za-z0-9]+$')