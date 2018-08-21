using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    /// <summary>
    /// https://www.andrew.cmu.edu/course/15-121/labs/HW-4%20Document%20Distance/lab.html
    /// </summary>
   public class SimilarityMatrixCalculations
    {
        //https://en.wikipedia.org/wiki/Cosine_similarity calculate cosine similarity for tf-idf
        public static float CalculateCosineSimilarity(float[] first_vector, float[] second_vector)
        {
            var Result = Common_Part_Vec_Calculations(first_vector, second_vector);
            var First_vector_magnitude = Magnitude(first_vector);
            var Second_vector_magnitude = Magnitude(second_vector);
            float result = Result / (First_vector_magnitude * Second_vector_magnitude);

            if (float.IsNaN(result))
                return 0;
            else
                return (float)result;
        }

        private static float Magnitude(float[] vec)
        {
            var dot_product_calculations = (float)Math.Sqrt(Common_Part_Vec_Calculations(vec, vec));
            if(float.IsNaN(dot_product_calculations) || float.IsInfinity(dot_product_calculations))
            {
                return 0;
            }
            else
            {
                return dot_product_calculations;
            }
        }

        //calculate common part (union) of two vecors
        private static float Common_Part_Vec_Calculations(float[] first_vector, float[] second_vector)
        {
            float common_part = 0;

            Parallel.For(0, first_vector.Length, i => {
                common_part += (first_vector[i] * second_vector[i]);
            });

            #region sequentional_dot_product_calculations
            /*
            for(int i=0; i<=first_vector.Length-1; i++)
            {
                common_part += (first_vector[i] * second_vector[i]);
            }
            */
            #endregion

            if (float.IsNaN(common_part))
            {
                return 0;
            }
            else
            {
                return common_part;
            }
        }

        //Computes the similarity between two documents as the distance between their point representations. Is translation invariant.
        public static float FindEuclideanDistance(float[] vector_A, float[] vector_B)
        {
            float euclideanDistance = 0;

          /*  Parallel.For(0, vector_A.Length, i => {
                euclideanDistance += (float)Math.Pow((vector_A[i] - vector_B[i]), 2);
            });*/

            #region sequential_Euclidean_distance
            
            for (var i = 0; i <= vector_A.Length-1; i++)
            {
                euclideanDistance += (float)Math.Pow((vector_A[i] - vector_B[i]), 2);
            }
            
            #endregion

            var end_result = (float)Math.Sqrt(euclideanDistance);

            if(float.IsNaN(end_result))
            {
                return 0;
            }
            else
            {
                return end_result;
            }

        }

        //https://en.wikipedia.org/wiki/Jaccard_index - 1st formula
        public static float FindJaccardIndex(float[] vector_A, float[] vector_B)
        {
            var product = Common_Part_Vec_Calculations(vector_A, vector_B);
            if(float.IsNaN(product))
            {
                product = 0;
            }
            var magnitudeOfA = Magnitude(vector_A);
            if (float.IsNaN(magnitudeOfA))
            {
                magnitudeOfA = 0;
            }
            var magnitudeOfB = Magnitude(vector_B);
            if (float.IsNaN(magnitudeOfB))
            {
                magnitudeOfB = 0;
            }
            var magnitude_result = magnitudeOfA + magnitudeOfB - product;
            if(float.IsNaN(magnitude_result) && magnitude_result == 0)
            {
                return 0;
            }
            var result = product / (magnitude_result);
            if (float.IsNaN(result))
            {
                return 0;
            }
            else
            {
                return result;
            }
            

        }
    }
}
