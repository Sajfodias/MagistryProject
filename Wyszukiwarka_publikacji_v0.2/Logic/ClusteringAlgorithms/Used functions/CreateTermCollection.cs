using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    class CreateTermCollection
    {
        public static List<string> GenerateTermCollection()
        {
            List<string> TermCollection = new List<string>();

            using (var dbContext = new ArticlesDataContainer())
            {
                var resul_PG = dbContext.Terms_Vocabulary.SqlQuery("SELECT * FROM dbo.Terms_Vocabulary").ToList();
                if (resul_PG != null)
                {
                    foreach (var item in resul_PG)
                    {
                        if (item.term_value != null || item.term_value != String.Empty)
                            TermCollection.Add(item.term_value.ToLower());
                    }
                }
            }

            return TermCollection; 
        }
    }
}
