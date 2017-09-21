using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Logic.eBase;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    public class Authors
    {
        #region Author_Entity_Creation
        /*
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int author_Id { get; set; }

            public int article_Id { get; set; }
            public string author_name { get; set; }
            public string author_surename { get; set; }
            */
        #endregion

        #region Articles_Lists
        /*
            public virtual List<BibtexArticle> PG_Articles { get; set; }
            public virtual List<PPArticle> PP_Articles { get; set; }
            public virtual List<UGArticle> UG_Articles { get; set; }
            public virtual List<UMKArticle> UMK_Articles { get; set; }
            public virtual List<WSBArticle> WSB_Articles { get; set; }
            */
        #endregion
    }
}
