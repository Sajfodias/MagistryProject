using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("UG_ArticlesSet")]
    public partial class UGArticle: Article
    {
        public UGArticle()
        {
            TermVocabularies = new HashSet<TermVocabulary>();
            Authors = new HashSet<Author>();
        }

        [Required]
        public string article_author_line { get; set; }
        [Required]
        public string article_title { get; set; }
        [Required]
        public string article_source { get; set; }
        [Required]
        public string article_keywords { get; set; }
        [Required]
        public string article_DOI { get; set; }
        //public int Terms_Vocabulary_terms_Id { get; set; }

        public virtual ICollection<TermVocabulary> TermVocabularies { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}