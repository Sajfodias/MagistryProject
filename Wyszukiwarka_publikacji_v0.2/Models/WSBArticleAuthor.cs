﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("WSB_ArticlesAuthor")]
    public partial class WSBArticleAuthor
    {
        [Key, Column(Order = 1)]
        public int WSB_Articles_article_Id { get; set; }
        [Key, Column(Order = 2)]
        public int Author_author_Id { get; set; }
        public WSBArticle WSBArticle { get; set; }
        public Author Author { get; set; }
    }
}