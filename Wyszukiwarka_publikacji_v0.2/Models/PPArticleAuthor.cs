using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("PP_ArticlesAuthor")]
    public partial class PPArticleAuthor
    {
        [Key, Column(Order = 1)]
        public int PP_Articles_article_Id { get; set; }
        [Key, Column(Order = 2)]
        public int Author_author_Id { get; set; }
        public PPArticle PPArticle { get; set; }
        public Author Author { get; set; }
    }
}
