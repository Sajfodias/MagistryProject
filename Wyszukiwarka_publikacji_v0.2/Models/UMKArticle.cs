using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("UMK_ArticlesSet")]
    public partial class UMKArticle: Article
    {
        public UMKArticle()
        {
            TermVocabularies = new HashSet<TermVocabulary>();
            Authors = new HashSet<Author>();
        }

        public string article_author_line { get; set; }
        public string article_title { get; set; }
        public string article_language { get; set; }
        public string article_Full_title { get; set; }
        public string article_pl_keywords { get; set; }
        public string article_eng_keywords { get; set; }
        public string article_translated_title { get; set; }
        public string article_url { get; set; }
        public string article_publisher_desc { get; set; }
        public string article_publisher_title { get; set; }
        //public int Terms_Vocabulary_terms_Id { get; set; }

        public virtual ICollection<TermVocabulary> TermVocabularies { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}