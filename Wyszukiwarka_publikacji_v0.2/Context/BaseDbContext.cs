using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Models;

namespace Wyszukiwarka_publikacji_v0._2.Context
{
    public class BaseDbContext: DbContext
    {
        public BaseDbContext(): base("name=ArticleProjDB")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<PGArticle> PGArticles { get; set; }
        public virtual DbSet<PPArticle> PPArticles { get; set; }
        public virtual DbSet<UGArticle> UGArticles { get; set; }
        public virtual DbSet<UMKArticle> UMKArticles { get; set; }
        public virtual DbSet<WSBArticle> WSBArticles { get; set; }
        public virtual DbSet<TermVocabulary> TermVocabularies { get; set; }
        public virtual DbSet<TermVocabularyPGArticle> TermVocabularyPGArticles { get; set; }
        public virtual DbSet<TermVocabularyPPArticle> TermVocabularyPPArticles { get; set; }
        public virtual DbSet<TermVocabularyUGArticle> TermVocabularyUGArticles { get; set; }
        public virtual DbSet<TermVocabularyUMKArticle> TermVocabularyUMKArticles { get; set; }
        public virtual DbSet<TermVocabularyWSBArticle> TermVocabularyWSBArticles { get; set; }
    }
}