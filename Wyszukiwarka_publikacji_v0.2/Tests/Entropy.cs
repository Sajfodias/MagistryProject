using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Wyszukiwarka_publikacji_v0._2.Logic.eBase;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;
using Wyszukiwarka_publikacji_v0._2.Tests;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Entropy
    {
        public static float Enthropy_Calculating(List<Centroid> clusteringResult, List<List<string>> Class)
        {
            int Number_Of_Cluster = clusteringResult.Count;
            float Entropy = 0.0F;
            int[] number_Of_Couple_Elements_in_k = new int[Class.Count];
            int[][] Couple_Elements_Matrix = new int[clusteringResult.Count][];
            double[] clusterEntropies = new double[Number_Of_Cluster];

            for (int k = 0; k < clusteringResult.Count; k++)
            {
                for (int i = 0; i < clusteringResult[k].GroupedDocument.Count; i++)
                    for (int c = 0; c < Class.Count; c++)
                    {
                        for (int ci = 0; ci < Class[c].Count; ci++)
                            if (clusteringResult[k].GroupedDocument[i].Content.Contains(Class[c][ci]))
                                number_Of_Couple_Elements_in_k[c]++;

                        Couple_Elements_Matrix[k] = number_Of_Couple_Elements_in_k;
                    }
                number_Of_Couple_Elements_in_k = new int[Class.Count];
            }

            int numer_of_elements = 0;
            for(int i=0; i<clusteringResult.Count; i++)
            {
                numer_of_elements += clusteringResult[i].GroupedDocument.Count;
            }

            double Sum_Of_Probability = 0;
            float first_part = 0;
            for(int c=0;c<Number_Of_Cluster; c++)
            {
                first_part += clusteringResult[c].GroupedDocument.Count / numer_of_elements;
                for(int l=0; l<Class.Count; l++)
                {
                    Sum_Of_Probability += Couple_Elements_Matrix[c][l] / Class[l].Count * Math.Log((float)(Couple_Elements_Matrix[c][l] / Class[l].Count), 2.0F);
                }
                Entropy += (-1.0F) * (float)first_part * (float)Sum_Of_Probability;
            }

            return Entropy;
        }

    }
}
