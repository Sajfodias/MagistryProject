using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class GravitationalClusteringAlgorithm
    {
        public static List<TestCentroid> Gravitational(List<DocumentVectorTest> vSpace, float G, float deltaG, int M, float epsilon, int cluster_count)
        {
            List<TestCentroid> result = new List<TestCentroid>();
            List<DocumentVectorTest> docVectorCopy = new List<DocumentVectorTest>(vSpace);
            int N = docVectorCopy.Count;

            var set_result = Tests.DisjointSetTest.Set(docVectorCopy); //Make(i) in article


            int[] parent = set_result.Item1;
            int[] rank = set_result.Item2;
            List<TestCentroid> centroidSet = set_result.Item3;
            List<TestCentroid> unionChanged = new List<TestCentroid>(centroidSet);
            float[,] distance_element_table = new float[unionChanged.Count, unionChanged.Count];

            for (int sp1 = 0; sp1 < distance_element_table.GetLength(0); sp1++)
                for (int sp2 = 0; sp2 < distance_element_table.GetLength(1); sp2++)
                    distance_element_table[sp1, sp2] = 0;


            int k = 0;

            for(int z=0; z<M; z++)
            {
                for (int j = 0; j < unionChanged.Count; j++)
                {
                    k = GenerateIndex(unionChanged.Count, j);

                    var distance = Move(docVectorCopy[j], docVectorCopy[k], G);
                    distance_element_table[j, k] = distance;

                    if (Math.Pow(distance, 2) <= epsilon)
                    {
                        #region OldPart
                        /*
                        if (j == 0)
                        {
                            var unionChangedResultTuple = Tests.DisjointSetTest.Union(j, index, centroidSet);
                            unionChanged = unionChangedResultTuple.Item3;
                            parent = unionChangedResultTuple.Item1;
                            //unionChanged.RemoveAt(j);
                            unionChanged.RemoveAt(index);
                        }
                        // how to make union between to clusters.
                        else
                        {
                            if (j <= unionChanged.Count && index <= unionChanged.Count)
                            {
                                var unionChangedResultTuple = Tests.DisjointSetTest.Union(j, index, unionChanged);
                                unionChanged = unionChangedResultTuple.Item3;
                                parent = unionChangedResultTuple.Item1;
                                //unionChanged.RemoveAt(index);
                            }
                            else
                                continue;
                        }
                        //List<Centroid> unionChanged = DisjointSet.Union(docVectorCopy[j], docVectorCopy[index]);
                        */
                        #endregion
                        unionChanged = Tests.DisjointSetTest.Union1(j, k, unionChanged);
                    }
                    G = (1 - deltaG) * G;

                    #region Find(i)- i think we don't need the views of nested cluster here
                    /*
                    for (int z1 = 0; z < result.Count; z++)
                    {
                        for (int k1 = 0; k < result[z1].GroupedDocument.Count; k1++)
                        {
                            TestCentroid element = unionChanged[Tests.DisjointSetTest.Find(k1)];
                        }
                    }
                    */
                    #endregion
                }
            }
            
            result = unionChanged;
            return result;
        }

        private static int GenerateIndex(int count, int j)
        {
            int index = 0;
            Random rand = new Random();
            index = rand.Next(0, count - 1);
            if (index == j)
                index = GenerateIndex(count, j);
            else
                return index;
            return index;
        }

        private static float GetDocumentDistance(DocumentVectorTest doc1, DocumentVectorTest doc2)
        {
            var dist = 0.0f;
            for (var i = 0; i < doc1.VectorSpace.Length; i++)
            {
                dist += (float)Math.Pow((double)(doc1.VectorSpace[i] - doc2.VectorSpace[i]), 2.0);

            }
            dist = (float)Math.Pow((double)dist, 0.5);
            return dist;
        }
        private static float Move(DocumentVectorTest documentVector1, DocumentVectorTest documentVector2, float G)
        {
            int length = documentVector1.VectorSpace.Count();
            float[] d = new float[length];
            var distance = GetDocumentDistance(documentVector1, documentVector2);
            for (int i = 0; i < length; i++)
            {
                d[i] = documentVector2.VectorSpace[i] - documentVector1.VectorSpace[i];
            }
            for (var i = 0; i < length; i++)
            {
                documentVector1.VectorSpace[i] = documentVector1.VectorSpace[i] + distance * (G / (float)Math.Pow(distance, 3.0));
                //documentVector1.VectorSpace[i] = documentVector1.VectorSpace[i] + d[i] * (G / (float)Math.Pow(distance, 3.0));
            }
            return distance;
        }

        public static List<TestCentroid> GetClustersTest(List<TestCentroid> clusters, float alpha, List<DocumentVectorTest> data)
        {
            List<TestCentroid> newclusters = new List<TestCentroid>();
            int N = data.Count;
            int number_of_clusters = clusters.Count;
            float MIN_POINTS = alpha * N;

            for (int i = 0; i < number_of_clusters; i++)
            {
                TestCentroid centroid = new TestCentroid
                {
                    GroupedDocument = new List<DocumentVectorTest>()
                };

                if (clusters[i].GroupedDocument.Count >= MIN_POINTS)
                {
                    foreach (var elements in clusters[i].GroupedDocument)
                    {
                        centroid.GroupedDocument.Add(elements);
                        newclusters.Add(centroid);
                    }
                }
            }
            return newclusters;
        }
    }
}
