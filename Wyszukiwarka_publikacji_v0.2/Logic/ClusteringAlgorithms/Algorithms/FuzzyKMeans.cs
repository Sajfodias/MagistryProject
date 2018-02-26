//projected by Gagarine Yaikhom in the work Implementing the Fuzzy c-Means Algorithm
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms
{
    public class FuzzyKMeans
    {
        public static int number_of_dataPoints;
        public static int number_of_clusters;
        public static int max_number_of_dimensions;
        //public static float[,] low_high;            // low_high = new float[max_number_of_dimensions,2];
        public static float[,] degree_of_member;    // degree_of_member = new float[number_of_dataPoints,number_of_clusters];
        //public static float epsilon;                //must be < 0.0 and >= 1.0;
        //public static float fuzziness;              //must be >= 1.0;
        public static float[,] data_point;          //data_point = new float[number_of_dataPoints,max_number_of_dimensions];
        //or we can use the DocumentVector structure
        public static float[,] cluster_center;      //cluster_center = new float[number_of_clusters,max_number_of_dimensions];
        //or we can use the Centroid structure

        public static void Initialization(List<DocumentVector> docCollection, int number_of_clusters)
        {
            number_of_dataPoints = docCollection.Count;
            var lastElementInList = docCollection[docCollection.Count - 1];
            max_number_of_dimensions = lastElementInList.VectorSpace.Length;
            Random rand = new Random();
            data_point = new float[number_of_dataPoints, max_number_of_dimensions];
            degree_of_member = new float[number_of_dataPoints, number_of_clusters];
            float[] row_sum = new float[number_of_dataPoints];

            for (int i=0; i<docCollection.Count; i++)
            {
                for(int j=0; j<docCollection[i].VectorSpace.Length; j++)
                {
                    data_point[i, j] = docCollection[i].VectorSpace[j];
                }
            }


            for (int k = 0; k <= number_of_dataPoints-1; k++)
            {
                float sum = 0;      //probability sum
                
                //int r = 100;    //remaining probability
                float r = 1.0f;

                //tutaj żle się liczy
                for(int j = 0; j <= number_of_clusters-1; j++)
                {
                    float rval = (float)rand.NextDouble();
                    r -= rval;
                    degree_of_member[k, j] = rval;
                    sum += degree_of_member[k, j];
                }
                
                row_sum[k] = sum;

                for (int i=0; i<number_of_clusters; i++)
                {
                    degree_of_member[k, i] = degree_of_member[k, i] / row_sum[k];
                }
               
                //degree_of_member[k, 0] = 1 - sum;
            }
        }

        public static float[,] calculate_Center_vectors(float fuzziness, int number_of_clusters, int max_number_of_dimensions)
        {
            cluster_center = new float[number_of_clusters, max_number_of_dimensions];
            float numerator, denominator;
            float[,] t = new float[number_of_dataPoints,number_of_clusters];
            
            for(int i=0; i<number_of_dataPoints; i++)
                for(int j=0; j<number_of_clusters; j++)
                    t[i, j] = (float)Math.Pow(degree_of_member[i,j],fuzziness);

            for(int j=0; j<number_of_clusters; j++)
            {
                for(int k=0; k<max_number_of_dimensions; k++)
                {
                    numerator = 0;
                    denominator = 0;
                    for(int i=0; i<number_of_dataPoints; i++)
                    {
                        numerator += t[i, j] * data_point[i, k];
                        denominator += t[i, j];
                    }
                    cluster_center[j, k] = numerator / denominator;
                }
            }
            return cluster_center;
        }

        public static float Get_norm(int i, int j, int max_number_of_dimensions)
        {
            float sum = 0;
            for (int k = 0; k < max_number_of_dimensions; k++)
                sum += (float)Math.Pow((data_point[i, k] - cluster_center[j, k]),2);
            var result = (float)Math.Sqrt(sum);
            return result;
        }

        public static float Get_new_value(int i, int j, float fuzziness, int number_of_clusters, int max_number_of_dimensions)
        {
            float p, sum = 0;
            p = 2 / (fuzziness - 1);
            for(int k=0; k<number_of_clusters; k++)
            {
                sum += (float)Math.Pow(
                    (Get_norm(i, j, max_number_of_dimensions))/
                    (Get_norm(j, k, max_number_of_dimensions)), p);
            }
            var result = 1 / sum;
            return result;
        }

        public static Tuple<float, float[,]> Update_degree_of_membership(float fuzziness, int number_of_clusters, int max_number_of_dimensions)
        {
            Tuple<float, float[,]> result;
            float new_uij, diff, max_diff = 0;
            for(int j=0; j<number_of_clusters; j++)
            {
                for(int i=0; i<number_of_dataPoints; i++)
                {
                    new_uij = Get_new_value(i, j, fuzziness, number_of_clusters, max_number_of_dimensions);
                    diff = new_uij - degree_of_member[i, j];
                    if (diff > max_diff)
                        max_diff = diff;
                    degree_of_member[i, j] = new_uij;
                }
            }
            result = new Tuple<float, float[,]>(max_diff, degree_of_member);
            return result;
        }

        public static float[,] Fcm(List<DocumentVector> docCollection, int number_of_clusters, float epsilon, float fuzziness, HashSet<string> termCollection)
        {
            max_number_of_dimensions = termCollection.Count;
            Tuple<float, float[,]> max_diff;
            float[,] clusters_centers;
            Initialization(docCollection, number_of_clusters);
            do
            {
                clusters_centers=calculate_Center_vectors(fuzziness, number_of_clusters, max_number_of_dimensions);
                max_diff = Update_degree_of_membership(fuzziness,number_of_clusters, max_number_of_dimensions);
            }
            while (max_diff.Item1 > epsilon);
            return max_diff.Item2;
        }

        public static String Show_clusters(List<DocumentVector> docCollection, float[,] Fcm_degree_of_member, int number_of_clusters)
        {
            List<Centroid> clusterization_result = new List<Centroid>();
            while (clusterization_result.Count != number_of_clusters)
            {
                Centroid newCentroid = new Centroid
                {
                    GroupedDocument = new List<DocumentVector>()
                };
                clusterization_result.Add(newCentroid);
            }
            String Message = String.Empty;
            for(int i=0; i<docCollection.Capacity; i++)
            {
                var cluster = 0;
                float highest = 0;
                for(int j=0; j<number_of_clusters; j++)
                {
                    if (Fcm_degree_of_member[i, j] > highest)
                    {
                        //highest = Fcm_degree_of_member[i, j];
                        highest = MaxValueOfArray(Fcm_degree_of_member, docCollection.Capacity, number_of_clusters);
                        cluster = j;
                        if (i < docCollection.Capacity)
                        {
                            clusterization_result[cluster].GroupedDocument.Add(docCollection[i]);
                            Fcm_degree_of_member[i, j] = 0;
                            docCollection.RemoveAt(i);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                string result_FuzzyK_means_ = @"F:\Magistry files\FuzzyKMeans_result.txt";
                using(StreamWriter sw = new StreamWriter(result_FuzzyK_means_))
                {
                    sw.WriteLine("The cluster " + cluster + " contains " + data_point[i, 0] + " " + data_point[i, 1]);
                }
                Message += "The cluster " + cluster + " contains " + data_point[i, 0] + " " + data_point[i, 1] + '\n';
            }
            return Message;
        }

        static float MaxValueOfArray(float[,] inputArray, int ix, int jy)
        {
            float result = 0;
            float maxVal = 0;
            for (int i = 1; i < ix-1; i++)
            {
                for(int j = 1; j<jy-1; j++)
                {
                    if (inputArray[i,j] > maxVal)
                    {
                        maxVal = inputArray[i, j];

                    }
                }       
            }
            result = maxVal;
            return result;
        }
    }
}
