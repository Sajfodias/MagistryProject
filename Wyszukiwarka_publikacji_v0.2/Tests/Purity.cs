using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Purity
    {
        public static float Purity_Calculating(List<Centroid> clusteringResult, List<List<string>> ClassCollection, List<DocumentVector> vSpace)
        {
            float result = 0.0F;
            float Purity = 0.0F;
            int k = clusteringResult.Count;
            int N = vSpace.Count;
            int[,] Confusion_matrix = new int[k, ClassCollection.Count];

            #region Example
            /*
             * 
             *  k=3
             *  N=140
             *  Purity = 1/N* sum^(k)_i=1(max_j|C_i U t_j|) where |C_i U t_j| - count of object from class T_j in cluster C_i
             * 
             * 1st step
             *    |    |  T1 |  T2  |  T3
             *      ---------------------
             *      C1 |  0  |  53  |  10
             *      C2 |  0  |  1   |  60
             *      C3 |  0  |  16  |  0
             *
             * than we can calculate Purity according to the next fromulae: Purity = (53 + 60 + 16) / 140 = 0.92142
             * 
             * 2nd step
             * Find the max value in a row and sum all max rows values
             * sum =(53 + 60 + 16)
             * 
             * 3rd step
             * 
             * Purity = 129/140 = 0.92142
            */
            #endregion

            //firstly create the confusion_matrix
            int Similar_element = 0;
            for (int ki=0; ki<k; ki++)
            {
                for(int i=0; i<clusteringResult[ki].GroupedDocument.Count; i++)
                {
                    for(int Li=0; Li<ClassCollection.Count; Li++)
                    {
                        for(int l=0; l<ClassCollection[Li].Count; l++)
                        {
                            if (clusteringResult[ki].GroupedDocument[i].Content == ClassCollection[Li][l] || clusteringResult[ki].GroupedDocument[i].Content.Contains(ClassCollection[Li][l]))
                                Similar_element++;
                        }
                        Confusion_matrix[ki, Li] = Similar_element;
                        Similar_element = 0;
                    }
                }
            }

            //in this stem we calculate the sum of maximum couple elements
            int sum = 0;
            for(int i=0; i<k; i++)
            {
                int element = 0;
                for(int j=0; j<ClassCollection.Count; j++)
                {
                    if (Confusion_matrix[i, j] > element)
                        element = Confusion_matrix[i, j];
                }
                sum = sum + element;
            }

            Purity = ((float)sum)/N;

            result = Purity;
            return result;
        }
    }
}
