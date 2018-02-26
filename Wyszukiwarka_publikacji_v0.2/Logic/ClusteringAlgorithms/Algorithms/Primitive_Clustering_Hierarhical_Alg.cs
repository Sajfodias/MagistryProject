using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Wyszukiwarka_publikacji_v0._2.Logic.eBase;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms
{
    class Primitive_Clustering_Hierarhical_Alg
    {
        public static float[,] proximity_Matrix;

        /// <summary>
        /// This function return the proximity matrix with proximity between all elements.
        /// </summary>
        /// <param name="docCollection"></param>
        /// <returns></returns>
        public static float[,] Compute_Proximity_Matrix(List<DocumentVector> docCollection)
        {
            proximity_Matrix = new float[docCollection.Count, docCollection.Count];
            for(int i=0; i<docCollection.Count; i++)
            {
                for(int j=0; j<docCollection.Count; j++)
                {
                    if (i == j)
                        proximity_Matrix[i, j] = 0.0F;
                    else
                        proximity_Matrix[i, j] = SimilarityMatrixCalculations.FindEuclideanDistance(docCollection[i].VectorSpace, docCollection[j].VectorSpace);
                }
            }
            return proximity_Matrix;
        }
        // proximity_matrix = MyUpdate_Proximity_Matrix(proximity_matrix, RowsRemoved, out RowsRemoved, ToJoin);
        public static float[,] MyUpdate_Proximity_Matrix(float[,] proximity_Matrix, List<int>RowsRemoved, out List<int>NewRowsRemoved, List<int> ToJoin, List<DocumentVector> docCollection)//       ist<DocumentVector> docCollection)
        {
            NewRowsRemoved = RowsRemoved;
            for (int i = 0; i < docCollection.Count; i++)
            {
                if (!RowsRemoved.Contains(i) && !ToJoin.Contains(i))
                {
                    var min = (float)Double.MaxValue;
                    foreach (var x in ToJoin)
                    {
                        if (proximity_Matrix[i, x] < min)
                        {
                            min = proximity_Matrix[i, x];
                        }
                    }
                    foreach (var j in ToJoin)
                    {
                        proximity_Matrix[i, j] = min;
                        proximity_Matrix[j, i] = min;
                    }

                }
            }
            RowsRemoved.Add(ToJoin.First());
            return proximity_Matrix;
        }

        /// <summary>
        /// The function must return list of Centroids with count of elements equals to docCollection.Length (1 element in each Centroid).
        /// </summary>
        /// <param name="docCollection"></param>
        /// <returns></returns>
        public static List<Centroid> Create_List_of_Centroids(List<DocumentVector> docCollection)
        {
            var result = new List<Centroid>();
            Centroid newCentroid;
            List<DocumentVector> docCollectionCopy = new List<DocumentVector>(docCollection);
            for(int i=0; i<docCollection.Count; i++)
            {
                newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid.GroupedDocument.Add(docCollectionCopy[i]);
                result.Add(newCentroid);
                //docCollectionCopy.RemoveAt(i);
            }
            return result;
        }

        public static Tuple<float, int[]> find_Min_Value_in_array(float[,] proximity_Matrix)
        {
            Tuple<float, int[]> result;
            float min_element = 1.0F; // we can't use 0 or proximity_Matrix[0,0] = 0
            int[] min_ElementCoordinate = new int[2];
            int x_length = proximity_Matrix.GetLength(0); //with condition that we has symethrical matrix - 2x2, 3x3, 4x4 - none 3x4 or 2x5.
            for (int x = 0; x < x_length; x++)
            {
                for (int y = 0; y < x_length; y++)
                {
                    if ((min_element > proximity_Matrix[x,y]) & (min_element!=0) & (proximity_Matrix[x,y] != 0))
                    {
                        min_element = proximity_Matrix[x, y];
                        min_ElementCoordinate[0] = x;
                        min_ElementCoordinate[1] = y;
                    }
                }
            }
            result = new Tuple<float, int[]>(min_element, min_ElementCoordinate);
            //always are chosing the same points
            return result;
        }

        public static List<Centroid> Merge_Closest_Clusters (List<Centroid> list_of_Centroid, Tuple<float, int, int> closestClusters, out List<int> ToJoin)
        {
            List<Centroid> result = new List<Centroid>();
            int index_of_first_element = closestClusters.Item2;
            int index_of_second_element = closestClusters.Item3;
            ToJoin = new List<int>();
            ToJoin.Add(index_of_first_element);
            ToJoin.Add(index_of_second_element);
            //here we have the problem
            foreach (var item in list_of_Centroid[index_of_first_element].GroupedDocument)
            {
                list_of_Centroid[index_of_second_element].GroupedDocument.Add(item);
            }

            list_of_Centroid.RemoveAt(index_of_first_element);
            //list_of_Centroid.RemoveAt(index_of_second_element);
            result = list_of_Centroid;
            return result;
        }

        public static float[,] Update_proximity_matrix(Tuple<float, int, int> mergedCentroids, float[,] proximityMatrix, int length)
        {
            float[,] newProximityMatrix = new float[length-1, length-1];
            int index_to_delete1 = mergedCentroids.Item2;
            int index_to_delete2 = mergedCentroids.Item3;


            int r = newProximityMatrix.GetLength(0);
            //while(r > 2)
            //{
               for(int i=0; i<r; i++)
               {
                    for(int j=0; j<r; j++)
                    {
                        newProximityMatrix[i, j] = 0.5F * proximityMatrix[i, index_to_delete1] + 0.5F * proximityMatrix[index_to_delete2, j] - 0.5F * proximityMatrix[index_to_delete1, index_to_delete2];
                    }
               }
            //}
            return newProximityMatrix;
        }

        public static List<Centroid> Hierarchical_Clusterization(List<DocumentVector> docCollection, int iterationCount)
        {
            List<Centroid> result = new List<Centroid>();
            float[,] proximity_Matrix = Compute_Proximity_Matrix(docCollection);
            List<Centroid> list_of_Centroid = Create_List_of_Centroids(docCollection);
            int length = proximity_Matrix.GetLength(0);
            List<int> RowsRemoved = new List<int>();
            List<int> ToJoin;
            for (int i=0; i<iterationCount; i++)
            {
                var length_of_list = list_of_Centroid.Count; ;
                //while (length_of_list - RowsRemoved.Count > 5)
                //{
                    while (length >= 2)
                    {
                        //var minimal_Distance = find_Min_Value_in_array(proximity_Matrix);
                        var minimal_Distance = Single_linkage(proximity_Matrix);
                        list_of_Centroid = Merge_Closest_Clusters(list_of_Centroid, minimal_Distance, out ToJoin); // don't return the new list_of_Centroid
                        proximity_Matrix = Update_proximity_matrix(minimal_Distance, proximity_Matrix, length);
                        //proximity_Matrix = MyUpdate_Proximity_Matrix(proximity_Matrix, RowsRemoved, out RowsRemoved, ToJoin, list_of_Centroid[0].GroupedDocument);
                        //proximity_Matrix = Update_proximity_matrix(minimal_Distance, proximity_Matrix, length);
                        length = length - 1;
                    }
                //}
            }
            result = list_of_Centroid;
            return result;
        }

        private static float Euclidean_Distance(Centroid a, Centroid b)
        {
            return SimilarityMatrixCalculations.FindEuclideanDistance(a.GroupedDocument[0].VectorSpace, b.GroupedDocument[0].VectorSpace);
        }

        private static Tuple<float, int, int> Single_linkage(float[,]distances)
        {
            float min = float.MaxValue;
            float d = 0.0F;
            int index_a = 0;
            int index_b = 0;
            Tuple<float, int, int> result;

            int matrix_length = distances.GetLength(0);
            for (int i = 0; i < matrix_length; i++)
                for (int j = 0; j < matrix_length; j++)
                {
                    d = distances[i, j];
                    if (d < min)
                    {
                        min = d;
                        index_a = i;
                        index_b = j;
                    }
                        
                }
            result = new Tuple<float, int, int>(min, index_a, index_b);
            return result;
        }

        private static Tuple<float, int, int> Complete_linkage(float[,] distances)
        {
            float max = 0.0F;
            float d = 0.0F;
            int index_a = 0;
            int index_b = 0;
            Tuple<float, int, int> result;

            int matrix_length = distances.GetLength(0);
            for (int i = 0; i < matrix_length; i++)
                for (int j = 0; j < matrix_length; j++)
                {
                    d = distances[i, j];
                    if (d > max)
                    {
                        max = d;
                        index_a = i;
                        index_b = j;
                    }

                }
            result = new Tuple<float, int, int>(max, index_a, index_b);
            index_a = 0;
            index_b = 0;
            return result;
        }

        private static Tuple<float, int, int> Avarage_linkage(float[,] distances)
        {
            float[] totalDistanceMatrix = new float[distances.GetLength(0)];
            float min = float.MaxValue;
            int index_a = 0;
            int index_b = 0;
            Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>();
            int[] indexMatrix = new int[2];
            int label = 0;
            Tuple<float, int, int> result;

            int matrix_length = distances.GetLength(0);
            for (int i = 0; i < matrix_length; i++)
            {
                for (int j = 0; j < matrix_length; j++)
                {
                    totalDistanceMatrix[i] += distances[i, j];
                    indexMatrix[0] = i;
                    indexMatrix[1] = j;
                    label = i;
                    dictionary.Add(label, indexMatrix);
                }
                totalDistanceMatrix[i] = totalDistanceMatrix[i] / matrix_length;
            }
            for(int j=0; j<totalDistanceMatrix.Length; j++)
            {
                if (totalDistanceMatrix[j] < min)
                {
                    min = totalDistanceMatrix[j];
                    index_a = dictionary[j][0];
                    index_b = dictionary[j][1];
                }
                    
            }
            result = new Tuple<float, int, int>(min, index_a, index_b);
            return result;
        }

        /*
         * int[,] array = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
         * var trim = TrimArray(0, 2, array);
        */

        public static float[,] TrimArray(int rowToRemove, int columnToRemove, float[,] originalArray)
        {
            float[,] result = new float[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }
    }
}
