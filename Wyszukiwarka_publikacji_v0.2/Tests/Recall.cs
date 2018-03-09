using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Recall
    {
        /// <summary>
        /// This function allow us to calculate the Recall between cluster C_i and class L_j according  to formulae: 
        /// Recall(C_i, L_j) = m_ij/m_j 
        /// where: mij - count of elements in cluster C_i from class L_j  and m_j - count of elements in class L_j
        /// </summary>
        /// <param name="clusteringResult">Here you must to provide  the clusterization results - clusters with elements.</param>
        /// <param name="Class">This is "natural" class of elements from collection assigned to the same class of elements - for example: documents that has the Arhitecture words in title,abstract or keywords we assign to  one class of dokuments about Architecture.</param>
        /// <returns>float[cluster.Count]Recall_matrix - returns dependency between elements from count of elements in cluster and in the class </returns>
        public static float[] Recall_Calculating(List<Centroid> clusteringResult, List<string> Class)
        { 
            int number_Of_Couple_Elements_in_k = 0;
            float[] Recall_matrix = new float[clusteringResult.Count];

            for (int k=0; k<clusteringResult.Count; k++)
            {
                for(int i=0; i < clusteringResult[k].GroupedDocument.Count; i++)
                {
                    for(int c=0; c< Class.Count; c++)
                    {
                        if (clusteringResult[k].GroupedDocument[i].Content.Contains(Class[c]))
                            number_Of_Couple_Elements_in_k ++;
                    }
                }
                Recall_matrix[k] = number_Of_Couple_Elements_in_k;
                number_Of_Couple_Elements_in_k = 0;
            }

            for (int j = 0; j < Recall_matrix.Length; j++)
                Recall_matrix[j] = Recall_matrix[j] / Class.Count;
            return Recall_matrix;
        }
    }
}
