using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Precision
    {
        /// <summary>
        /// This function allow us to calculate the Precision between cluster C_i and class L_j according  to formulae: 
        /// Recall(C_i, L_j) = |C_i union L_j|/|C_i|
        /// where:  |C_i union L_j| - count of elements in cluster C_i from class L_j  and |C_i| - count of elements in cluster C_i
        /// </summary>
        /// <param name="clusteringResult">Here you must to provide  the clusterization results - clusters with elements.</param>
        /// <param name="Class">This is "natural" class of elements from collection assigned to the same class of elements - for example: documents that has the Arhitecture words in title,abstract or keywords we assign to  one class of dokuments about Architecture.</param>
        /// <returns>float[cluster.Count]Precision_matrix - returns dependency between elements from count of elements in cluster and in the class </returns>
        public static int[] Precision_Calculating(List<Centroid> clusteringResult, List<string> Class)
        {
            int number_Of_Couple_Elements_in_k = 0;
            int[] Recall_matrix = new int[clusteringResult.Count];

            for (int k = 0; k < clusteringResult.Count; k++)
            {
                for (int i = 0; i < clusteringResult[k].GroupedDocument.Count; i++)
                {
                    for (int c = 0; c < Class.Count; c++)
                    {
                        if (clusteringResult[k].GroupedDocument[i].Content.Contains(Class[c]))
                            number_Of_Couple_Elements_in_k++;
                    }
                }
                Recall_matrix[k] = number_Of_Couple_Elements_in_k;
                number_Of_Couple_Elements_in_k = 0;
            }

            for (int j = 0; j < Recall_matrix.Length; j++)
                    Recall_matrix[j] = Recall_matrix[j] / clusteringResult[j].GroupedDocument.Count;
                
            return Recall_matrix;
        }
    }
}
