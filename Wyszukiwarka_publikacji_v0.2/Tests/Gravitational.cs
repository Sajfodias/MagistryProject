using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Gravitational
    {
        public static int[] GravitationalAlg(List<DocumentVectorTest> vSpace, float G, float deltaG, int M, float epsilon)
        {
            List<DocumentVectorTest> docVectorCopy = new List<DocumentVectorTest>(vSpace);
            int N = docVectorCopy.Count;
            int[] disjoint_set = new int[N];
            float[,] distance_element_table = new float[N, N];

            DisjointSetTest disjoint = new DisjointSetTest(N);

            for (int i = 0; i < N; i++)
                Tests.DisjointSetTest.MakeSet(i);

            for (int sp1 = 0; sp1 < distance_element_table.GetLength(0); sp1++)
                for (int sp2 = 0; sp2 < distance_element_table.GetLength(1); sp2++)
                    distance_element_table[sp1, sp2] = 0;

            int k = 0;

            for (int z = 0; z < M; z++)
            {
                for (int j = 0; j < N; j++)
                {
                    k = GenerateIndex(N, j);
                    var distance = Move(docVectorCopy[j], docVectorCopy[k], G);
                    distance_element_table[j, k] = distance;
                    if (Math.Pow(distance, 2) <= epsilon)
                        Tests.DisjointSetTest.Union(j, k);
                    G = (1 - deltaG) * G;

                    for (int i = 0; i < N; i++)
                        disjoint_set[i] = Tests.DisjointSetTest.Find(i);
                }
            }


            return disjoint_set;
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
                documentVector1.VectorSpace[i] = documentVector1.VectorSpace[i] + distance * (G / (float)Math.Pow(distance, 3.0));  //--last function
                //documentVector1.VectorSpace[i] = documentVector1.VectorSpace[i] + d[i] * (G / (float)Math.Pow(distance, 3.0));
            }
            return distance;
        }

        public static List<TestCentroid> GetClustersTest(int[] clusters, float alpha, List<DocumentVectorTest> data)
        {
            List<TestCentroid> centroidSet = new List<TestCentroid>();
            HashSet<int> clustersSet = new HashSet<int>();
            int N = data.Count;
            int number_of_clusters = clusters.Length;
            float MIN_POINTS = alpha * N;

            for (int i = 0; i < N; i++)
                clustersSet.Add(clusters[i]);

            for(int i=0; i<clustersSet.Count; i++)
            {
                TestCentroid centroid = new TestCentroid
                {
                    GroupedDocument = new List<DocumentVectorTest>()
                };
                var docIndex = clustersSet.ElementAt(i);
                centroid.GroupedDocument.Add(data[docIndex]);
                centroidSet.Add(centroid);
            }



          
                for(int j=0; j<clustersSet.Count; j++)
                {
                    for (int i = 0; i < N; i++)
                    {
                        if (clustersSet.ElementAt(j) == clusters[i])
                            centroidSet[j].GroupedDocument.Add(data[i]);
                    }
                }

            return centroidSet;
        }
    }
}