using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms
{
    class GravitationalClusteringAlgorithm
    {
        /// <summary>
        /// Here the description of Gravitational clustering algorithm.
        /// </summary>
        /// <param name="docCollection">List of entry elements.</param>
        /// <param name="G">Gravitational parameter value, for the test = 7*10^(-6).</param>
        /// <param name="deltaG">Gravitational forse loss = 0.01F.</param>
        /// <param name="M">Count of iteration, for test = 500.</param>
        /// <param name="epsilon">Minimum distance, for test = 10^(-4).</param>
        /// <returns>List<Centroid> result = sets stored in disjoint set union-find strukture.</returns>
        public static List<Centroid> Gravitational(List<DocumentVector> docCollection, float G, float deltaG, int M, float epsilon)
        {
            List<Centroid> result = new List<Centroid>();
            List<DocumentVector> docVectorCopy = new List<DocumentVector>(docCollection);
            int docVectorCopy_Count = docVectorCopy.Count;
            int index = 0;
            Random rand = new Random();
            var set_result = DisjointSet.Set(docVectorCopy);
            float[] documentVectorOriginalFirst = new float[docVectorCopy[0].VectorSpace.Length];
            float[] documentVectorOriginalSecond = new float[docVectorCopy[0].VectorSpace.Length];
            int[]parent = set_result.Item1;
            int[] rank = set_result.Item2;
            List<Centroid> centroidSet = set_result.Item3;
            List<Centroid> unionChanged = new List<Centroid>(centroidSet);

            for (int i=0; i<M; i++)
            {
                for(int j=0; j<unionChanged.Count; j++)
                {
                    if (j == 0)
                    {
                        index = rand.Next(0, docVectorCopy.Count-1);
                    }
                    else
                    {
                        index = rand.Next(0, unionChanged.Count-1);
                    }
                    
                    if (index != j)
                    {
                        DocumentVector document = new DocumentVector();
                        document.SaveOriginal(docVectorCopy[j]);
                        documentVectorOriginalFirst = document.OriginalVectorSpace;
                        //float[,] distanceMatrix = Move(docVectorCopy[j], docVectorCopy[index], docVectorCopy_Count);
                        var distance = Move(docVectorCopy[j], docVectorCopy[index], G);

                        //if(distanceMatrix[j, index]<= epsilon)
                        if (distance <= epsilon)
                        {
                            if (j == 0)
                            {
                                var unionChangedResultTuple = DisjointSet.Union(j, index, centroidSet);
                                unionChanged = unionChangedResultTuple.Item3;
                                parent = unionChangedResultTuple.Item1;
                            }
                            // how to make union between to clusters.
                            else{
                                var unionChangedResultTuple = DisjointSet.Union(j, index, unionChanged);
                                unionChanged = unionChangedResultTuple.Item3;
                                parent = unionChangedResultTuple.Item1;
                            }
                            //List<Centroid> unionChanged = DisjointSet.Union(docVectorCopy[j], docVectorCopy[index]);
                        }
                    }
                    
                    G = (1 - deltaG) * G;
                    for(int z=0; z < result.Count; z++)
                    {
                        for(int k=0; k<result[z].GroupedDocument.Count; k++)
                        {
                            DocumentVector element = docVectorCopy[DisjointSet.Find(parent,k)];
                        }
                    }
                }
            }
            result = unionChanged;
            return result;
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
                documentVector1.VectorSpace[i] = documentVector1.VectorSpace[i] + d[i] * (G / (float)Math.Pow(distance, 3.0));
            }
            return distance;
        }

        public static List<Centroid> GetClusters(List<Centroid> clusters, float alpha, List<DocumentVector> data)
        {
            List<Centroid> newclusters = new List<Centroid>();
            int N = data.Count;
            int number_of_clusters = clusters.Count;
            float MIN_POINTS = alpha * N;

            for(int i=0; i<number_of_clusters; i++)
            {
                Centroid centroid = new Centroid();
                centroid.GroupedDocument = new List<DocumentVector>();

                if (clusters[i].GroupedDocument.Count >= MIN_POINTS)
                {
                    foreach(var elements in clusters[i].GroupedDocument)
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
