using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("UG_ArticlesAuthor")]
    public partial class UGArticleAuthor
    {
        [Key, Column(Order = 1)]
        public int UG_Articles_article_Id { get; set; }
        [Key, Column(Order = 2)]
        public int Author_author_Id { get; set; }
        public UGArticle UGArticle { get; set; }
        public Author Author { get; set; }
    }
}