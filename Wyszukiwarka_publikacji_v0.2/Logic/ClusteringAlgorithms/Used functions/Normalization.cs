using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    class Normalization
    {
        public static void Normilize_Term_Frequency(List<DocumentVector> documentCollection)
        {
            List<string> Term_Collection = CreateTermCollection.GenerateTermCollection();
            List<DocumentVector> docCollection = documentCollection;

            for(int i=0; i<=Term_Collection.Count-1; i++)
            {
                float max = 0;
                for(int j=0; j<=docCollection.Count-1; j++)
                {
                    try
                    {
                        if (docCollection[i].VectorSpace[j] >= max)
                            max = docCollection[i].VectorSpace[j];
                    }
                    catch(Exception ex)
                    {
                        System.Windows.MessageBox.Show("Exception: " + ex + " occured.", "Exception!", System.Windows.MessageBoxButton.OK);
                    }

                }

                for(int k=0; k <=docCollection.Count-1; k++)
                {
                    if (max > 0)
                        docCollection[i].VectorSpace[k] = docCollection[i].VectorSpace[k] / max;
                    else
                        docCollection[i].VectorSpace[k] = 0;
                }
            }
        }
    }
}
