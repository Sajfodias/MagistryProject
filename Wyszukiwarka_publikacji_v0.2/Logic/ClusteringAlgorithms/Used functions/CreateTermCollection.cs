using System;
using System.Collections.Generic;
using System.IO;
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

            using (var dbContext = new ArticleDBDataModelContainer())
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

            
            string dictionary_text = File.ReadAllText(@"F:\Magistry files\csv_files\Allowed_term_dictionary.csv");
            string[] allowed_dictionary = dictionary_text.Split(',', '\n');

            for (int i = 0; i <= TermCollection.Count-1; i++)
            {
                for (int j = 0; j <= allowed_dictionary.Length - 1; j++)
                {
                    if (TermCollection[i].Length <= 3 && (!TermCollection[i].Contains(allowed_dictionary[j])))
                    {
                        TermCollection.RemoveAt(i);
                    }
                    else if (TermCollection[i].Contains(")") || TermCollection[i].Contains("("))
                    {
                        TermCollection.RemoveAt(i);
                    }
                    else if (TermCollection[i].Contains("]") || TermCollection[i].Contains("["))
                    {
                        TermCollection.RemoveAt(i);
                    }
                    else if (TermCollection[i].Contains("*") || TermCollection[i].Contains("*"))
                    {
                        TermCollection.RemoveAt(i);
                    }
                    else
                        continue;
                }
            }

            for(int i=0; i<=TermCollection.Count-1; i++)
            {
                for(int j=0; j<=TermCollection.Count-1; j++)
                {
                    if((TermCollection[i]==TermCollection[j]) || TermCollection[i].Contains(TermCollection[j].Substring(0)))
                    {
                        TermCollection.RemoveAt(j);
                    }
                }
            }
            
            return TermCollection; 
        }
    }
}
