using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest
{
    class KMeans
    {
        public static List<Centroid> KMeansClustering(List<DocumentVector> rawData, int numClusters, int iterationCount, List<Centroid> initialMeans)
        {
            int iteration = 0;
            List<Centroid> clustering = new List<Centroid>(numClusters);
            //List<DocumentVector> data = Normilized(rawData);
            List<Centroid> means = initialMeans;
            bool changed = true; // change in at least one cluster assignment?
            bool success = true; // all means computed? (no zero-count clusters)



            while (changed == true && success == true && iteration < iterationCount) // technically this is Lloyd's algorithm
            {
                var clusteringUpdate = UpdateClustering(rawData, clustering, means); // (re)assign tuples to clusters. no effect if fail
                changed = clusteringUpdate.Item1;
                clustering = clusteringUpdate.Item2;

                foreach (var center in clustering)
                    center.CalculateMeans();
                for (var i = 0; i < clustering.Count; i++)
                {
                    means[i].means = clustering[i].means;
                }
                ++iteration; // k-means typically converges very quickly
            }

            return clustering;
        }

        public static Tuple<bool, List<Centroid>> UpdateMeans(List<DocumentVector> data, List<Centroid> clustering, List<Centroid> means)
        {
            Tuple<bool, List<Centroid>> result;
            int numClusters = means.Count;
            bool changed = false;
            float minVal = float.MaxValue;
            int minIndex = 0;
            List<Centroid> centers = new List<Centroid>(numClusters);
            centers = CentroidInitializer(numClusters);
            List<Centroid> newMeans = new List<Centroid>(numClusters);
            newMeans = CentroidInitializer(numClusters);

            for (int i = 0; i < means.Count; i++)
            {
                DocumentVector newCenter = new DocumentVector();
                newCenter.VectorSpace = new float[means[i].GroupedDocument[0].VectorSpace.Length];
                for (int j = 0; j < means[i].GroupedDocument.Count; j++)
                {
                    for (int v = 0; v < means[i].GroupedDocument[j].VectorSpace.Length; v++)
                        newCenter.VectorSpace[v] += means[i].GroupedDocument[j].VectorSpace[v];
                    for (int z = 0; z < means[i].GroupedDocument[j].VectorSpace.Length; z++)
                        newCenter.VectorSpace[z] = newCenter.VectorSpace[z] / means[i].GroupedDocument.Count;
                }
                //newCenter.Content = "ideal center of " + i.ToString() + " cluster";
                newCenter.Content = means[i].GroupedDocument[0].Content;
                newCenter.ArticleID = means[i].GroupedDocument[0].ArticleID;
                centers[i].GroupedDocument.Add(newCenter);
            }

            for (int i = 0; i < clustering.Count; i++)
            {
                float[] distances = new float[clustering[i].GroupedDocument.Count];
                for (int j = 0; j < clustering[i].GroupedDocument.Count; j++)
                {
                    distances[j] = Logic.ClusteringAlgorithms.SimilarityMatrixCalculations.FindEuclideanDistance(clustering[i].GroupedDocument[j].VectorSpace, centers[i].GroupedDocument[0].VectorSpace);
                    //distances[j] = Distance(clustering[i].GroupedDocument[j].VectorSpace, centers[i].GroupedDocument[0].VectorSpace);
                }
                for (int j = 0; j < distances.Length; j++)
                {
                    if (distances[j] < minVal)
                    {
                        minVal = distances[j];
                        minIndex = j;
                    }

                }
                newMeans[i].GroupedDocument.Add(clustering[i].GroupedDocument[minIndex]);
                minVal = float.MaxValue;
                minIndex = 0;
            }

            for (int i = 0; i < clustering.Count; i++)
                if (means[i].GroupedDocument[0].Content == newMeans[i].GroupedDocument[0].Content)
                    changed = true;
                else
                    changed = false;

            means = newMeans;
            result = new Tuple<bool, List<Centroid>>(changed, means);
            return result;
        }

        private static List<Centroid> CentroidInitializer(int numClusters)
        {
            List<Centroid> initializedList = new List<Centroid>(numClusters);
            for (int i = 0; i < numClusters; i++)
            {
                Centroid newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid.means = null;
                initializedList.Add(newCentroid);
            }
            return initializedList;
        }

        private static float Distance(float[] vectorA, float[] vectorB)
        {
            float sumSquaredDiffs = 0.0F;
            for (int j = 0; j < vectorA.Length; ++j)
                sumSquaredDiffs += (float)Math.Pow((vectorA[j] - vectorB[j]), 2);
            return (float)Math.Sqrt(sumSquaredDiffs);
        }

        private static Tuple<bool, List<Centroid>> UpdateClustering(List<DocumentVector> data, List<Centroid> clustering, List<Centroid> means)
        {
            int numClusters = means.Count;
            bool changed = false;
            clustering = new List<Centroid>(means);
            List<Centroid> newClustering = new List<Centroid>();
            newClustering = CentroidInitializer(numClusters);
            Tuple<bool, List<Centroid>> result;


            float[,] distances = new float[data.Count, numClusters];
            for (int d = 0; d < data.Count; d++)
                for (int k = 0; k < numClusters; k++)
                {
                    //means[k].means = means[k].GroupedDocument[0].VectorSpace;
                    //distances[d, k] = Distance(data[d].VectorSpace, means[k].GroupedDocument[0].VectorSpace);  //GroupedDocument[0].VectorSpace);    means[k].means
                    distances[d, k] = Logic.ClusteringAlgorithms.SimilarityMatrixCalculations.FindEuclideanDistance(data[d].VectorSpace, means[k].GroupedDocument[0].VectorSpace);
                }
                  

            int[] clustering_labels = AssignDocsToClusters(distances, numClusters, data);

            for (int i = 0; i < numClusters; i++)
            {
                for (int j = 0; j < clustering_labels.Length; j++)
                {
                    if (i == clustering_labels[j])
                    {
                        changed = true;
                        newClustering[i].GroupedDocument.Add(data[j]);
                    }
                }
            }

            for (int k = 0; k < numClusters; ++k)
                if (newClustering[k].GroupedDocument.Count == 0)
                    changed = false;

            clustering = newClustering;
            result = new Tuple<bool, List<Centroid>>(changed, clustering);
            return result;
        }

        private static int MinIndexInRow(float[,] distances, int i)
        {
            int indexOfMin = 0;

            for (int k = 0; k < distances.GetLength(1); k++)
            {
                float smallDist = distances[i, 0];
                if (distances[i, k] < smallDist)
                {
                    smallDist = distances[i, k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        private static List<DocumentVector> Normilized(List<DocumentVector> rawData)
        {
            List<DocumentVector> result = new List<DocumentVector>(rawData);
            for (int j = 0; j < rawData.Count; j++)
            {
                float rowSum = 0.0F;
                for (int i = 0; i < rawData[j].VectorSpace.Length; i++)
                    rowSum += result[j].VectorSpace[i];
                float mean = rowSum / result[j].VectorSpace.Length;
                float sum = 0.0F;
                for (int z = 0; z < rawData.Count; ++z)
                    for (int k = 0; k < rawData[z].VectorSpace.Length; k++)
                        sum += (result[z].VectorSpace[k] - mean) * (result[z].VectorSpace[k] - mean);
                float sd = sum / result[j].VectorSpace.Length;
                for (int i = 0; i < result.Count; i++)
                    for (int p = 0; p < result[i].VectorSpace.Length; p++)
                        result[i].VectorSpace[p] = (result[i].VectorSpace[p] - mean) / sd;
            }
            return result;
        }

        private static int[] MinIndex(float[,] distances)
        {
            int[] indexOfMin = new int[distances.GetLength(0)];

            for (int k1 = 0; k1 < distances.GetLength(0); ++k1)
            {
                for (int k2 = 0; k2 < distances.GetLength(1); ++k2)
                {
                    float smallDist = distances[k1, 0];
                    if (distances[k1, k2] < smallDist)
                    {
                        smallDist = distances[k1, k2];
                        indexOfMin[k1] = k2;
                    }
                }
            }
            return indexOfMin;
        }

        public static int[] AssignDocsToClusters(float[,] clusteringResult, int clusterNumber, List<DocumentVector> docCollection)
        {
            int[] result = new int[clusteringResult.GetLength(0)];
            List<DocumentVector> newCopyDocCollection = new List<DocumentVector>(docCollection);

            float highest = clusteringResult[0, 0];
            int IndexOfCluster = 0;
            var IterCount = docCollection.Count;
            int x_dimension = clusteringResult.GetLength(0);
            int y_dimension = clusteringResult.GetLength(1);

            for (int i = 0; i < x_dimension; i++)
            {
                highest = clusteringResult[0, 0];
                IndexOfCluster = 0;

                for (int j = 0; j < y_dimension; j++)
                {
                    if (clusteringResult[i, j] < highest)
                    {
                        highest = clusteringResult[i, j];
                        IndexOfCluster = j;
                    }

                }
                result[i] = IndexOfCluster;
            }
            return result;
        }

        public static List<Centroid> CentroidCalculationsForKMeans(List<DocumentVector> data, int ClusterNumber)
        {
            List<Centroid> centroidList = new List<Centroid>();
            Random randomizer = new Random();
            HashSet<int> indexSet = new HashSet<int>();
            int index = 0;

            while (centroidList.Count != ClusterNumber)
            {
                index = randomizer.Next(0, data.Count+1);
                if (!indexSet.Contains(index))
                {
                    indexSet.Add(index);
                    Centroid newCentroid = new Centroid();
                    newCentroid.GroupedDocument = new List<DocumentVector>();
                    newCentroid.GroupedDocument.Add(data[index]);
                    centroidList.Add(newCentroid);
                }
                else if (indexSet.Contains(index))
                {
                    continue;
                }
            }
            /*
            foreach (var doc in centroidList)
            {
                doc.CalculateMeans();
                doc.GroupedDocument.Clear();
            }
            */
            return centroidList;
        }
    }

}