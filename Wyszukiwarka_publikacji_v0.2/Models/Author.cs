using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("AuthorSet")]
    public partial class Author
    {
        public Author()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int author_Id { get; set; }
        [Required]
        public string author_name { get; set; }
        [Required]
        public string author_surename { get; set; }

        //public virtual ICollection<PGArticleAuthor> PGArticleAuthors { get; set; }
        //public virtual ICollection<PPArticleAuthor> PPArticleAuthors { get; set; }
        //public virtual ICollection<UGArticleAuthor> UGArticleAuthors { get; set; }
        //public virtual ICollection<UMKArticleAuthor> UMKArticleAuthors { get; set; }
        //public virtual ICollection<WSBArticleAuthor> WSBArticleAuthors { get; set; }
    }
}
