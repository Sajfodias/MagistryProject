using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Wyszukiwarka_publikacji_v0._2.Tests;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class F1Measure
    {
        //https://en.wikipedia.org/wiki/F1_score
        public static float[,] F1_Measure_Calculating(List<Centroid> result1, List<List<string>> classCollection)
        {
            float[,] F1_Measure_result_matrix = new float[classCollection.Count, result1.Count];
            int[][] Precision_matrix = new int[classCollection.Count][];
            float[][] Recall_matrix = new float[classCollection.Count][];

            for (int p=0; p<classCollection.Count; p++)
            {
                var Precision = Tests.Precision.Precision_Calculating(result1, classCollection[p]);
                var Recall = Tests.Recall.Recall_Calculating(result1, classCollection[p]);
                Precision_matrix[p] = Precision;
                Recall_matrix[p] = Recall;
            }

            for (int L_i = 0; L_i < classCollection.Count; L_i++)
            {
                for (int C_j = 0; C_j < result1.Count; C_j++)
                {
                    var first_part = Recall_matrix[L_i][C_j] * (float)Precision_matrix[L_i][C_j];
                    var second_part = Recall_matrix[L_i][C_j] + Precision_matrix[L_i][C_j];
                    if (float.IsNaN(first_part) || float.IsNaN(second_part) || first_part == 0 || second_part==0)
                    {
                        F1_Measure_result_matrix[L_i, C_j] = 0.0F;
                    }
                    else
                    {
                        F1_Measure_result_matrix[L_i, C_j] = (2.0F * first_part) / (second_part);
                    }

                }
            }
            return F1_Measure_result_matrix;
        }


        // Definition of G-Measure:
        //https://en.wikipedia.org/wiki/F1_score
        //also known as Fowlkes–Mallows index:
        //https://en.wikipedia.org/wiki/Fowlkes%E2%80%93Mallows_index
        public static float[,] G_Measure_Calculating(List<Centroid> result1, List<List<string>> classCollection)
        {
            float[,] G_Measure_matrix = new float[classCollection.Count, result1.Count];
            int[][]  Precision_matrix = new int[classCollection.Count][];
            float[][] Recall_matrix = new float[classCollection.Count][];

            for (int p = 0; p < classCollection.Count; p++)
            {
                var Precision = Tests.Precision.Precision_Calculating(result1, classCollection[p]);
                var Recall = Tests.Recall.Recall_Calculating(result1, classCollection[p]);
                Precision_matrix[p] = Precision;
                Recall_matrix[p] = Recall;
            }

            for (int L_i = 0; L_i < classCollection.Count; L_i++)
            {
                for (int C_j = 0; C_j < result1.Count; C_j++)
                {
                    var first_part = Recall_matrix[L_i][C_j] * (float)Precision_matrix[L_i][C_j];
                    var second_part = Recall_matrix[L_i][C_j] + Precision_matrix[L_i][C_j];
                    if (float.IsNaN(first_part) || float.IsNaN(second_part) || first_part == 0 || second_part == 0)
                    {
                        G_Measure_matrix[L_i, C_j] = 0.0F;
                    }
                    else
                    {
                        G_Measure_matrix[L_i, C_j] = (float)Math.Sqrt(Convert.ToDouble(first_part*second_part));
                    }

                }
            }
            return G_Measure_matrix;
        }
    }
}
