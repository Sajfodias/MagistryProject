using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Models
{
    [Table("Terms_VocabularyPP_Articles")]
    public partial class TermVocabularyPPArticle
    {
        [Key, Column(Order = 1)]
        public int Terms_Vocabulary_terms_Id { get; set; }
        [Key, Column(Order = 2)]
        public int PP_Articles_article_Id { get; set; }
        public TermVocabulary TermVocabulary { get; set; }
        public PPArticle PGArticle { get; set; }
    }
}
