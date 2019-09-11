using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("PG_ArticlesSet")]
    public partial class PGArticle: Article
    {
        public PGArticle()
        {
            TermVocabularies = new HashSet<TermVocabulary>();
            Authors = new HashSet<Author>();
        }

        [Required]
        public string title { get; set; }
        [Required]
        public string abstractText { get; set; }
        [Required]
        public string keywords { get; set; }
        [Required]
        public string year { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string authors { get; set; }
        [Required]
        public string organizations { get; set; }
        [Required]
        public string url { get; set; }

        public virtual ICollection<TermVocabulary> TermVocabularies { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}