//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wyszukiwarka_publikacji_v0._2
{
    using System;
    using System.Collections.Generic;
    
    public partial class PG_Articles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PG_Articles()
        {
            this.Author = new HashSet<Author>();
            this.Terms_Vocabulary = new HashSet<Terms_Vocabulary>();
        }
    
        public int article_Id { get; set; }
        public string title { get; set; }
        public string abstractText { get; set; }
        public string keywords { get; set; }
        public int year { get; set; }
        public string country { get; set; }
        public string authors { get; set; }
        public string organizations { get; set; }
        public string url { get; set; }
        public int Terms_Vocabulary_terms_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Author> Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Terms_Vocabulary> Terms_Vocabulary { get; set; }
    }
}