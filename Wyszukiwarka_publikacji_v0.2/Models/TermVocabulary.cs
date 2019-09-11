using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("Term_Vocabulary")]
    public partial class TermVocabulary
    {
        public TermVocabulary()
        {
            Authors = new HashSet<Author>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int terms_Id { get; set; }
        [Required]
        public string term_value { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}