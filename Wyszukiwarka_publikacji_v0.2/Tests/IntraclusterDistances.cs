using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    static class IntraclusterDistances
    {
        public static float[] d_min(List<Centroid> result)
        {
            float d_min = 0.0F;
            float[,] intracluster_distance_matrix;
            float[] minimal_distances = new float[result.Count];

            for(int k=0; k<result.Count; k++)
            {
                int document_count = result[k].GroupedDocument.Count;
                intracluster_distance_matrix = new float[document_count, document_count];
                for (int n=0; n<document_count; n++)
                    for(int j=0; j<document_count; j++)
                        intracluster_distance_matrix[n, j] = SimilarityMatrixCalculations.FindEuclideanDistance(result[k].GroupedDocument[n].VectorSpace, result[k].GroupedDocument[j].VectorSpace);

                d_min = Find_Min_Value_in_array(intracluster_distance_matrix);
                minimal_distances[k] = d_min;
            }

            return minimal_distances;
        }

        public static float[] d_max(List<Centroid> result)
        {
            float d_min = 0.0F;
            float[,] intracluster_distance_matrix;
            float[] max_distances = new float[result.Count];

            for (int k = 0; k < result.Count; k++)
            {
                int document_count = result[k].GroupedDocument.Count;
                intracluster_distance_matrix = new float[document_count, document_count];
                for (int n = 0; n < document_count; n++)
                    for (int j = 0; j < document_count; j++)
                        intracluster_distance_matrix[n, j] = SimilarityMatrixCalculations.FindEuclideanDistance(result[k].GroupedDocument[n].VectorSpace, result[k].GroupedDocument[j].VectorSpace);

                d_min = Find_Max_Value_in_array(intracluster_distance_matrix);
                max_distances[k] = d_min;
            }

            return max_distances;
        }

        public static float[] d_sr(List<Centroid> result)
        {
            float d_sr = 0.0F;
            float[,] intracluster_distance_matrix;
            float[] distances_in_one_cluster;
            float[] median_distances = new float[result.Count];
            float sum = 0.0F;

            for (int k = 0; k < result.Count; k++)
            {
                int document_count = result[k].GroupedDocument.Count;
                intracluster_distance_matrix = new float[document_count, document_count];
                distances_in_one_cluster = new float[document_count];
                for (int n = 0; n < document_count; n++)
                {
                    for (int j = 0; j < document_count; j++)
                    {
                        intracluster_distance_matrix[n, j] = SimilarityMatrixCalculations.FindEuclideanDistance(result[k].GroupedDocument[n].VectorSpace, result[k].GroupedDocument[j].VectorSpace);
                        distances_in_one_cluster[n] += intracluster_distance_matrix[n, j];
                    }
                    sum += distances_in_one_cluster[n];
                }
                d_sr = sum / document_count;
                                
                median_distances[k] = d_sr;
                sum = 0;
            }
            return median_distances;
        }

        public static float Find_Min_Value_in_array(float[,] matrix)
        {
            float result = 1.0F; 
            int x_length = matrix.GetLength(0); //with condition that we has symethrical matrix - 2x2, 3x3, 4x4 - none 3x4 or 2x5.
            for (int x = 0; x < x_length; x++)
            {
                for (int y = 0; y < x_length; y++)
                {
                    if ((result > matrix[x, y]) & (result != 0) & (matrix[x, y] != 0))
                    {
                        result = matrix[x, y];
                    }
                }
            }
            //always are chosing the same points
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
