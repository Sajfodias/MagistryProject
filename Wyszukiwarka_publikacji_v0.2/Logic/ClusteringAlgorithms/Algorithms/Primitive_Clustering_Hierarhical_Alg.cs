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
            return result;
        }

        public static List<Centroid> Merge_Closest_Clusters (List<Centroid> list_of_Centroid, Tuple<float, int[]> closestClusters, out List<int> ToJoin)
        {
            List<Centroid> result = new List<Centroid>();
            int index_of_first_element = closestClusters.Item2[0];
            int index_of_second_element = closestClusters.Item2[1];
            ToJoin = new List<int>();
            ToJoin.Add(index_of_first_element);
            ToJoin.Add(index_of_second_element);
            foreach (var item in list_of_Centroid[index_of_first_element].GroupedDocument)
            {
                list_of_Centroid[index_of_second_element].GroupedDocument.Add(item);
            }

            list_of_Centroid.RemoveAt(index_of_first_element);
            list_of_Centroid.RemoveAt(index_of_second_element);
            result = list_of_Centroid;
            return result;
        }

        /*
        public static float[,] Update_proximity_matrix_Single_Link(Tuple<float, int[]> mergedCentroids, float[,] proximityMatrix, int length)
        {
            float[,] newProximityMatrix = new float[length, length];
            int[] indexes_to_delete = mergedCentroids.Item2;
            while(newProximityMatrix.Length != 2)
            {
                for(int i=0; i<proximityMatrix.Length; i++)
                {
                    for(int j=0; j<proximityMatrix.Length; j++)
                    {

                    }
                }
            }
            return newProximityMatrix;
        }
        */

        public static List<Centroid> Hierarchical_Clusterization(List<DocumentVector> docCollection, int iterationCount)
        {
            List<Centroid> result = new List<Centroid>();
            float[,] proximity_Matrix = Compute_Proximity_Matrix(docCollection);
            List<Centroid> list_of_Centroid = Create_List_of_Centroids(docCollection);
            int length = proximity_Matrix.Length;
            List<int> RowsRemoved = new List<int>();
            List<int> ToJoin;
            for (int i=0; i<iterationCount; i++)
            {
                var length_of_list = list_of_Centroid.Count; ;
                while (length_of_list - RowsRemoved.Count > 2)
                {
                    var minimal_Distance = find_Min_Value_in_array(proximity_Matrix);
                    list_of_Centroid = Merge_Closest_Clusters(list_of_Centroid, minimal_Distance, out ToJoin); // don't return the new list_of_Centroid
                   // length = length - 1;
                   
                    proximity_Matrix = MyUpdate_Proximity_Matrix(proximity_Matrix, RowsRemoved, out RowsRemoved, ToJoin, list_of_Centroid[0].GroupedDocument); ;// proximity_Matrix = Update_proximity_matrix_Single_Link(minimal_Distance, proximity_Matrix, length);
                }
            }
            return result;
        }
    }
}
