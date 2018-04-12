using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    static class InterclusterDistances
    {
        public static float[,] d_centroids(List<Centroid> result)
        {
            float[,] centroid_distances_matrix = new float[result.Count, result.Count];
            
            for(int i=0; i<result.Count; i++)
            {
                for(int j=0;  j<result.Count; j++)
                {
                    centroid_distances_matrix[i, j] = SimilarityMatrixCalculations.FindEuclideanDistance(result[i].GroupedDocument[0].VectorSpace, result[j].GroupedDocument[0].VectorSpace);
                }
            }
            return centroid_distances_matrix;
        }

        public static float[] d_min_centroids(List<Centroid> result)
        {
            float[] min_dist_between_cluster_elem = new float[result.Count];
            float min_distance = 0;
            float[,] d_min_intercluster;
            
            for(int k=0; k<result.Count; k++)
            {
                for (int k2 = 1; k2 < result.Count; k2++)
                {
                    d_min_intercluster = new float[result[k].GroupedDocument.Count, result[k2].GroupedDocument.Count];
                    for (int i = 0; i < result[k].GroupedDocument.Count; i++)
                    {
                        for (int j = 0; j < result[k2].GroupedDocument.Count; j++)
                        {
                            d_min_intercluster[i, j] = SimilarityMatrixCalculations.FindEuclideanDistance(result[k].GroupedDocument[i].VectorSpace, result[k2].GroupedDocument[j].VectorSpace);
                        }
                    }
                    min_distance = Find_Min_Value_in_array(d_min_intercluster, result[k].GroupedDocument.Count, result[k2].GroupedDocument.Count);
                    min_dist_between_cluster_elem[k] = min_distance;
                }
            }
            
            return min_dist_between_cluster_elem;
        }

        public static float Find_Min_Value_in_array(float[,] matrix, int x_length, int y_length)
        {
            float min = 1.0F;
            float result=0.3F;
            for (int x = 0; x < x_length; x++)
            {
                for (int y = 0; y < y_length; y++)
                {
                    if ((min > matrix[x, y]) & (min != 0) & (matrix[x, y] != 0))
                    {
                        min = matrix[x, y];
                        if (min < result)
                            result = min;
                        else
                            continue;
                    }
                }
            }
            return result;
        }

        public static float Find_Max_Value_in_array(float[,] matrix)
        {
            float result = 0.0001F;
            int x_length = matrix.GetLength(0); //with condition that we has symethrical matrix - 2x2, 3x3, 4x4 - none 3x4 or 2x5.
            for (int x = 0; x < x_length; x++)
            {
                for (int y = 0; y < x_length; y++)
                {
                    if ((result < matrix[x, y]) & (result != 0) & (matrix[x, y] != 0))
                    {
                        result = matrix[x, y];
                    }
                }
            }
            //always are chosing the same points
            return result;
        }
    }
}
