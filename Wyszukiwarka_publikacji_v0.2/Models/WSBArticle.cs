using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("WSB_ArticlesSet")]
    public partial class WSBArticle: Article
    {
        public WSBArticle()
        {
            TermVocabularies = new HashSet<TermVocabulary>();
            Authors = new HashSet<Author>();
        }

        [Required]
        public string article_authors { get; set; }
        [Required]
        public string article_title { get; set; }
        [Required]
        public string article_publisher_adres { get; set; }
        [Required]
        public string article_common_title { get; set; }
        [Required]
        public string article_eng_keywords { get; set; }
        [Required]
        public string article_pl_keywords { get; set; }
        [Required]
        public string article_title_other_lang { get; set; }
        [Required]
        public string article_DOI { get; set; }
        [Required]
        public string article_details { get; set; }
        [Required]
        public string article_URL { get; set; }
        //public int Terms_Vocabulary_terms_Id { get; set; }

        public virtual ICollection<TermVocabulary> TermVocabularies { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}