using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("PG_ArticlesAuthor")]
    public class PGArticleAuthor
    {
        [Key, Column(Order = 1)]
        public int PG_Articles_article_Id { get; set; }
        [Key, Column(Order = 2)]
        public int Author_author_Id { get; set; }
        public PGArticle PGArticle { get; set; }
        public Author Author { get; set; }
    }
}
