namespace Wyszukiwarka_publikacji_v0._2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorSet",
                c => new
                    {
                        author_Id = c.Int(nullable: false, identity: true),
                        author_name = c.String(),
                        author_surename = c.String(),
                    })
                .PrimaryKey(t => t.author_Id);
            
            CreateTable(
                "dbo.PG_ArticlesAuthor",
                c => new
                    {
                        PG_Articles_article_Id = c.Int(nullable: false),
                        Author_author_Id = c.Int(nullable: false),
                        Author_author_Id1 = c.Int(),
                        PGArticle_article_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.PG_Articles_article_Id, t.Author_author_Id })
                .ForeignKey("dbo.AuthorSet", t => t.Author_author_Id1)
                .ForeignKey("dbo.PG_ArticlesSet", t => t.PGArticle_article_Id)
                .Index(t => t.Author_author_Id1)
                .Index(t => t.PGArticle_article_Id);
            
            CreateTable(
                "dbo.PG_ArticlesSet",
                c => new
                    {
                        article_Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        abstractText = c.String(),
                        keywords = c.String(),
                        year = c.String(),
                        country = c.String(),
                        authors = c.String(),
                        organizations = c.String(),
                        url = c.String(),
                    })
                .PrimaryKey(t => t.article_Id);
            
            CreateTable(
                "dbo.PP_ArticlesAuthor",
                c => new
                    {
                        PP_Articles_article_Id = c.Int(nullable: false),
                        Author_author_Id = c.Int(nullable: false),
                        Author_author_Id1 = c.Int(),
                        PPArticle_article_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.PP_Articles_article_Id, t.Author_author_Id })
                .ForeignKey("dbo.AuthorSet", t => t.Author_author_Id1)
                .ForeignKey("dbo.PP_ArticlesSet", t => t.PPArticle_article_Id)
                .Index(t => t.Author_author_Id1)
                .Index(t => t.PPArticle_article_Id);
            
            CreateTable(
                "dbo.PP_ArticlesSet",
                c => new
                    {
                        article_Id = c.Int(nullable: false, identity: true),
                        article_author_line = c.String(),
                        article_title = c.String(),
                        article_source = c.String(),
                        article_year = c.Int(nullable: false),
                        article_language = c.String(),
                        article_DOI = c.String(),
                    })
                .PrimaryKey(t => t.article_Id);
            
            CreateTable(
                "dbo.UG_ArticlesAuthor",
                c => new
                    {
                        UG_Articles_article_Id = c.Int(nullable: false),
                        Author_author_Id = c.Int(nullable: false),
                        Author_author_Id1 = c.Int(),
                        UGArticle_article_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.UG_Articles_article_Id, t.Author_author_Id })
                .ForeignKey("dbo.AuthorSet", t => t.Author_author_Id1)
                .ForeignKey("dbo.UG_ArticlesSet", t => t.UGArticle_article_Id)
                .Index(t => t.Author_author_Id1)
                .Index(t => t.UGArticle_article_Id);
            
            CreateTable(
                "dbo.UG_ArticlesSet",
                c => new
                    {
                        article_Id = c.Int(nullable: false, identity: true),
                        article_author_line = c.String(),
                        article_title = c.String(),
                        article_source = c.String(),
                        article_keywords = c.String(),
                        article_DOI = c.String(),
                    })
                .PrimaryKey(t => t.article_Id);
            
            CreateTable(
                "dbo.UMK_ArticlesAuthor",
                c => new
                    {
                        UMK_Articles_article_Id = c.Int(nullable: false),
                        Author_author_Id = c.Int(nullable: false),
                        Author_author_Id1 = c.Int(),
                        UMKArticle_article_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.UMK_Articles_article_Id, t.Author_author_Id })
                .ForeignKey("dbo.AuthorSet", t => t.Author_author_Id1)
                .ForeignKey("dbo.UMK_ArticlesSet", t => t.UMKArticle_article_Id)
                .Index(t => t.Author_author_Id1)
                .Index(t => t.UMKArticle_article_Id);
            
            CreateTable(
                "dbo.UMK_ArticlesSet",
                c => new
                    {
                        article_Id = c.Int(nullable: false, identity: true),
                        article_author_line = c.String(),
                        article_title = c.String(),
                        article_language = c.String(),
                        article_Full_title = c.String(),
                        article_pl_keywords = c.String(),
                        article_eng_keywords = c.String(),
                        article_translated_title = c.String(),
                        article_url = c.String(),
                        article_publisher_desc = c.String(),
                        article_publisher_title = c.String(),
                    })
                .PrimaryKey(t => t.article_Id);
            
            CreateTable(
                "dbo.WSB_ArticlesAuthor",
                c => new
                    {
                        WSB_Articles_article_Id = c.Int(nullable: false),
                        Author_author_Id = c.Int(nullable: false),
                        Author_author_Id1 = c.Int(),
                        WSBArticle_article_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.WSB_Articles_article_Id, t.Author_author_Id })
                .ForeignKey("dbo.AuthorSet", t => t.Author_author_Id1)
                .ForeignKey("dbo.WSB_ArticlesSet", t => t.WSBArticle_article_Id)
                .Index(t => t.Author_author_Id1)
                .Index(t => t.WSBArticle_article_Id);
            
            CreateTable(
                "dbo.WSB_ArticlesSet",
                c => new
                    {
                        article_Id = c.Int(nullable: false, identity: true),
                        article_authors = c.String(),
                        article_title = c.String(),
                        article_publisher_adres = c.String(),
                        article_common_title = c.String(),
                        article_eng_keywords = c.String(),
                        article_pl_keywords = c.String(),
                        article_title_other_lang = c.String(),
                        article_DOI = c.String(),
                        article_details = c.String(),
                        article_URL = c.String(),
                    })
                .PrimaryKey(t => t.article_Id);
            
            CreateTable(
                "dbo.Term_Vocabulary",
                c => new
                    {
                        terms_Id = c.Int(nullable: false, identity: true),
                        term_value = c.String(),
                    })
                .PrimaryKey(t => t.terms_Id);
            
            CreateTable(
                "dbo.Terms_VocabularyPG_Articles",
                c => new
                    {
                        Terms_Vocabulary_terms_Id = c.Int(nullable: false),
                        PG_Articles_article_Id = c.Int(nullable: false),
                        PGArticle_article_Id = c.Int(),
                        TermVocabulary_terms_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Terms_Vocabulary_terms_Id, t.PG_Articles_article_Id })
                .ForeignKey("dbo.PG_ArticlesSet", t => t.PGArticle_article_Id)
                .ForeignKey("dbo.Term_Vocabulary", t => t.TermVocabulary_terms_Id)
                .Index(t => t.PGArticle_article_Id)
                .Index(t => t.TermVocabulary_terms_Id);
            
            CreateTable(
                "dbo.Terms_VocabularyPP_Articles",
                c => new
                    {
                        Terms_Vocabulary_terms_Id = c.Int(nullable: false),
                        PP_Articles_article_Id = c.Int(nullable: false),
                        PGArticle_article_Id = c.Int(),
                        TermVocabulary_terms_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Terms_Vocabulary_terms_Id, t.PP_Articles_article_Id })
                .ForeignKey("dbo.PP_ArticlesSet", t => t.PGArticle_article_Id)
                .ForeignKey("dbo.Term_Vocabulary", t => t.TermVocabulary_terms_Id)
                .Index(t => t.PGArticle_article_Id)
                .Index(t => t.TermVocabulary_terms_Id);
            
            CreateTable(
                "dbo.Terms_VocabularyUG_Articles",
                c => new
                    {
                        Terms_Vocabulary_terms_Id = c.Int(nullable: false),
                        UG_Articles_article_Id = c.Int(nullable: false),
                        PGArticle_article_Id = c.Int(),
                        TermVocabulary_terms_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Terms_Vocabulary_terms_Id, t.UG_Articles_article_Id })
                .ForeignKey("dbo.UG_ArticlesSet", t => t.PGArticle_article_Id)
                .ForeignKey("dbo.Term_Vocabulary", t => t.TermVocabulary_terms_Id)
                .Index(t => t.PGArticle_article_Id)
                .Index(t => t.TermVocabulary_terms_Id);
            
            CreateTable(
                "dbo.Terms_VocabularyUMK_Articles",
                c => new
                    {
                        Terms_Vocabulary_terms_Id = c.Int(nullable: false),
                        UMK_Articles_article_Id = c.Int(nullable: false),
                        PGArticle_article_Id = c.Int(),
                        TermVocabulary_terms_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Terms_Vocabulary_terms_Id, t.UMK_Articles_article_Id })
                .ForeignKey("dbo.UMK_ArticlesSet", t => t.PGArticle_article_Id)
                .ForeignKey("dbo.Term_Vocabulary", t => t.TermVocabulary_terms_Id)
                .Index(t => t.PGArticle_article_Id)
                .Index(t => t.TermVocabulary_terms_Id);
            
            CreateTable(
                "dbo.Terms_VocabularyWSB_Articles",
                c => new
                    {
                        Terms_Vocabulary_terms_Id = c.Int(nullable: false),
                        WSB_Articles_article_Id = c.Int(nullable: false),
                        PGArticle_article_Id = c.Int(),
                        TermVocabulary_terms_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.Terms_Vocabulary_terms_Id, t.WSB_Articles_article_Id })
                .ForeignKey("dbo.WSB_ArticlesSet", t => t.PGArticle_article_Id)
                .ForeignKey("dbo.Term_Vocabulary", t => t.TermVocabulary_terms_Id)
                .Index(t => t.PGArticle_article_Id)
                .Index(t => t.TermVocabulary_terms_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Terms_VocabularyWSB_Articles", "TermVocabulary_terms_Id", "dbo.Term_Vocabulary");
            DropForeignKey("dbo.Terms_VocabularyWSB_Articles", "PGArticle_article_Id", "dbo.WSB_ArticlesSet");
            DropForeignKey("dbo.Terms_VocabularyUMK_Articles", "TermVocabulary_terms_Id", "dbo.Term_Vocabulary");
            DropForeignKey("dbo.Terms_VocabularyUMK_Articles", "PGArticle_article_Id", "dbo.UMK_ArticlesSet");
            DropForeignKey("dbo.Terms_VocabularyUG_Articles", "TermVocabulary_terms_Id", "dbo.Term_Vocabulary");
            DropForeignKey("dbo.Terms_VocabularyUG_Articles", "PGArticle_article_Id", "dbo.UG_ArticlesSet");
            DropForeignKey("dbo.Terms_VocabularyPP_Articles", "TermVocabulary_terms_Id", "dbo.Term_Vocabulary");
            DropForeignKey("dbo.Terms_VocabularyPP_Articles", "PGArticle_article_Id", "dbo.PP_ArticlesSet");
            DropForeignKey("dbo.Terms_VocabularyPG_Articles", "TermVocabulary_terms_Id", "dbo.Term_Vocabulary");
            DropForeignKey("dbo.Terms_VocabularyPG_Articles", "PGArticle_article_Id", "dbo.PG_ArticlesSet");
            DropForeignKey("dbo.WSB_ArticlesAuthor", "WSBArticle_article_Id", "dbo.WSB_ArticlesSet");
            DropForeignKey("dbo.WSB_ArticlesAuthor", "Author_author_Id1", "dbo.AuthorSet");
            DropForeignKey("dbo.UMK_ArticlesAuthor", "UMKArticle_article_Id", "dbo.UMK_ArticlesSet");
            DropForeignKey("dbo.UMK_ArticlesAuthor", "Author_author_Id1", "dbo.AuthorSet");
            DropForeignKey("dbo.UG_ArticlesAuthor", "UGArticle_article_Id", "dbo.UG_ArticlesSet");
            DropForeignKey("dbo.UG_ArticlesAuthor", "Author_author_Id1", "dbo.AuthorSet");
            DropForeignKey("dbo.PP_ArticlesAuthor", "PPArticle_article_Id", "dbo.PP_ArticlesSet");
            DropForeignKey("dbo.PP_ArticlesAuthor", "Author_author_Id1", "dbo.AuthorSet");
            DropForeignKey("dbo.PG_ArticlesAuthor", "PGArticle_article_Id", "dbo.PG_ArticlesSet");
            DropForeignKey("dbo.PG_ArticlesAuthor", "Author_author_Id1", "dbo.AuthorSet");
            DropIndex("dbo.Terms_VocabularyWSB_Articles", new[] { "TermVocabulary_terms_Id" });
            DropIndex("dbo.Terms_VocabularyWSB_Articles", new[] { "PGArticle_article_Id" });
            DropIndex("dbo.Terms_VocabularyUMK_Articles", new[] { "TermVocabulary_terms_Id" });
            DropIndex("dbo.Terms_VocabularyUMK_Articles", new[] { "PGArticle_article_Id" });
            DropIndex("dbo.Terms_VocabularyUG_Articles", new[] { "TermVocabulary_terms_Id" });
            DropIndex("dbo.Terms_VocabularyUG_Articles", new[] { "PGArticle_article_Id" });
            DropIndex("dbo.Terms_VocabularyPP_Articles", new[] { "TermVocabulary_terms_Id" });
            DropIndex("dbo.Terms_VocabularyPP_Articles", new[] { "PGArticle_article_Id" });
            DropIndex("dbo.Terms_VocabularyPG_Articles", new[] { "TermVocabulary_terms_Id" });
            DropIndex("dbo.Terms_VocabularyPG_Articles", new[] { "PGArticle_article_Id" });
            DropIndex("dbo.WSB_ArticlesAuthor", new[] { "WSBArticle_article_Id" });
            DropIndex("dbo.WSB_ArticlesAuthor", new[] { "Author_author_Id1" });
            DropIndex("dbo.UMK_ArticlesAuthor", new[] { "UMKArticle_article_Id" });
            DropIndex("dbo.UMK_ArticlesAuthor", new[] { "Author_author_Id1" });
            DropIndex("dbo.UG_ArticlesAuthor", new[] { "UGArticle_article_Id" });
            DropIndex("dbo.UG_ArticlesAuthor", new[] { "Author_author_Id1" });
            DropIndex("dbo.PP_ArticlesAuthor", new[] { "PPArticle_article_Id" });
            DropIndex("dbo.PP_ArticlesAuthor", new[] { "Author_author_Id1" });
            DropIndex("dbo.PG_ArticlesAuthor", new[] { "PGArticle_article_Id" });
            DropIndex("dbo.PG_ArticlesAuthor", new[] { "Author_author_Id1" });
            DropTable("dbo.Terms_VocabularyWSB_Articles");
            DropTable("dbo.Terms_VocabularyUMK_Articles");
            DropTable("dbo.Terms_VocabularyUG_Articles");
            DropTable("dbo.Terms_VocabularyPP_Articles");
            DropTable("dbo.Terms_VocabularyPG_Articles");
            DropTable("dbo.Term_Vocabulary");
            DropTable("dbo.WSB_ArticlesSet");
            DropTable("dbo.WSB_ArticlesAuthor");
            DropTable("dbo.UMK_ArticlesSet");
            DropTable("dbo.UMK_ArticlesAuthor");
            DropTable("dbo.UG_ArticlesSet");
            DropTable("dbo.UG_ArticlesAuthor");
            DropTable("dbo.PP_ArticlesSet");
            DropTable("dbo.PP_ArticlesAuthor");
            DropTable("dbo.PG_ArticlesSet");
            DropTable("dbo.PG_ArticlesAuthor");
            DropTable("dbo.AuthorSet");
        }
    }
}
