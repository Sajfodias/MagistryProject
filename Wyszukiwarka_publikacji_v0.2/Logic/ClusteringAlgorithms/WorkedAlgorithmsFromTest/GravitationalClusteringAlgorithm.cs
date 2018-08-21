using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest
{
    class GravitationalClusteringAlgorithm
    {
        public static int[] GravitationalAlgorithm(List<DocumentVector> vSpace, float G, float deltaG, int M, float epsilon)
        {
            List<DocumentVector> docVectorCopy = new List<DocumentVector>(vSpace);
            int N = docVectorCopy.Count;
            int[] disjoint_set = new int[N];
            float[,] distance_element_table = new float[N, N];

            DisjointSet disjoint = new DisjointSet(N);

            for (int i = 0; i < N; i++)
                DisjointSet.MakeSet(i);

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
                        DisjointSet.Union(j, k);
                    G = (1 - deltaG) * G;

                    for (int i = 0; i < N; i++)
                        disjoint_set[i] = DisjointSet.Find(i);
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

        private static float GetDocumentDistance(DocumentVector doc1, DocumentVector doc2)
        {
            var dist = 0.0f;
            for (var i = 0; i < doc1.VectorSpace.Length; i++)
            {
                dist += (float)Math.Pow((double)(doc1.VectorSpace[i] - doc2.VectorSpace[i]), 2.0);

            }
            dist = (float)Math.Pow((double)dist, 0.5);
            return dist;
        }

        private static float Move(DocumentVector documentVector1, DocumentVector documentVector2, float G)
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
            }
            return distance;
        }

        public static List<Centroid> GetClusters(int[] clusters, float alpha, List<DocumentVector> data)
        {
            List<Centroid> centroidSet = new List<Centroid>();
            HashSet<int> clustersSet = new HashSet<int>();
            int N = data.Count;
            int number_of_clusters = clusters.Length;
            float MIN_POINTS = alpha * N;

            for (int i = 0; i < N; i++)
                clustersSet.Add(clusters[i]);

            for (int i = 0; i < clustersSet.Count; i++)
            {
                Centroid centroid = new Centroid
                {
                    GroupedDocument = new List<DocumentVector>()
                };
                var docIndex = clustersSet.ElementAt(i);
                centroid.GroupedDocument.Add(data[docIndex]);
                centroidSet.Add(centroid);
            }

            for (int j = 0; j < clustersSet.Count; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    if (clustersSet.ElementAt(j) == clusters[i])
                        centroidSet[j].GroupedDocument.Add(data[i]);
                }
            }
            return centroidSet;
        }

        public static List<Centroid> RemoveSameElementsFromClusters(List<Centroid> get_Clusters)
        {
            int times=0;
            List<Centroid> result = new List<Centroid>();

            for (int i = 0; i < get_Clusters.Count; i++)
            {
                result[i].GroupedDocument = new List<DocumentVector>();
                for (int j = 0; j < get_Clusters[i].GroupedDocument.Count; j++)
                    for (int z = 0; z < get_Clusters[i].GroupedDocument.Count; z++)
                    {
                        if ((get_Clusters[i].GroupedDocument[j].ArticleID != get_Clusters[i].GroupedDocument[z].ArticleID))
                        {
                            result[i].GroupedDocument.Add(get_Clusters[i].GroupedDocument[j]);
                        }
                        else if ((get_Clusters[i].GroupedDocument[j].ArticleID == get_Clusters[i].GroupedDocument[z].ArticleID) && times == 0)
                        {
                            times++;
                        }
                        else if ((get_Clusters[i].GroupedDocument[j].ArticleID == get_Clusters[i].GroupedDocument[z].ArticleID) && times == 1)
                            continue;     
                    }
                times = 0;
            }
            return result;
        }
    }

    class DisjointSet
    {
        public static int[] parent;

        public static int[] rank;

        public DisjointSet(int N)
        {
            parent = new int[N];
            rank = new int[N];
        }

        public static Tuple<int[], int[], List<Centroid>> Set(List<DocumentVector> docCollection)
        {
            Tuple<int[], int[], List<Centroid>> result;
            parent = new int[docCollection.Count];
            rank = new int[docCollection.Count];
            var centroidSet = new List<Centroid>();

            for (int i = 0; i < docCollection.Count; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }

            Centroid newCentroid;
            List<DocumentVector> docCollectionCopy = new List<DocumentVector>(docCollection);

            for (int j = 0; j < docCollectionCopy.Count; j++)
            {
                newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid.GroupedDocument.Add(docCollectionCopy[j]);
                centroidSet.Add(newCentroid);
            }
            result = new Tuple<int[], int[], List<Centroid>>(parent, rank, centroidSet);
            return result;
        }

        public static void MakeSet(int x)
        {
            parent[x] = x;
            rank[x] = 0;
        }

        public static int Find(int x)
        {
            int px = x;
            int i = 0;
            while (px != parent[x])
            {
                px = parent[px];
            }
            while (x != px)
            {
                i = parent[x];
                parent[x] = px;
                x = i;
            }
            return px;
        }

        public static void Union(int x, int y)
        {
            x = Find(x);
            y = Find(y);
            if (x == y) return;
            if (rank[x] > rank[y])
            {
                parent[y] = x;
            }
            else
            {
                if (rank[x] == rank[y])
                    rank[y] += 1;
                parent[x] = y;
            }
        }

        /*
        public static List<Centroid> Union(int x, int y, List<Centroid> list_of_Centroid)
        {
            List<Centroid> result;
            List<Centroid> list_of_Centroid_Copy = new List<Centroid>(list_of_Centroid);

            int elementX = Find(x);
            int elementY = Find(y);

            if (elementX != elementY)
            {
                if (rank[elementX] == rank[elementY])
                {
                    rank[elementY] = rank[elementY] + 1;
                    parent[elementX] = elementY;
                    list_of_Centroid_Copy[elementX].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY].GroupedDocument);
                    list_of_Centroid_Copy.RemoveAt(elementY);
                }
                else if (rank[elementX] > rank[elementY])
                {
                    parent[elementY] = elementX;
                    list_of_Centroid_Copy[elementY].GroupedDocument.AddRange(list_of_Centroid_Copy[elementX].GroupedDocument);
                    list_of_Centroid_Copy.RemoveAt(elementX);
                }
                else
                {
                    parent[elementX] = elementY;
                    list_of_Centroid_Copy[elementX].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY].GroupedDocument);
                    list_of_Centroid_Copy.RemoveAt(elementY);
                }
            }
            result = list_of_Centroid_Copy;
            return result;
        }
        */
    }
}