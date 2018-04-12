using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class NewTestKMeansClustering
    {
        public static List<TestCentroid> Cluster(List<DocumentVectorTest> rawData, int numClusters, int seed)
        {
            List<TestCentroid> clustering = new List<TestCentroid>();
            List<DocumentVectorTest> data = Normilized(rawData);
            List<TestCentroid> means = InitMeans(numClusters, data, seed);
            bool changed = true; // change in at least one cluster assignment?
            bool success = true; // all means computed? (no zero-count clusters)

            int maxCount = rawData.Count * 10; // sanity check
            int ct = 0;

            while (changed == true && success == true && ct < maxCount) // technically this is Lloyd's algorithm
            {
                changed = UpdateClustering(data, clustering, means); // (re)assign tuples to clusters. no effect if fail
                success = UpdateMeans(data, clustering, means); // compute new cluster means if possible. no effect if fail
                ++ct; // k-means typically converges very quickly
            }

            return clustering;
        }

        private static bool UpdateMeans(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        {
            int numClusters = means.Count;
            bool changed = false;

            List<TestCentroid> newClustering = new List<TestCentroid>();
            float[] distances = new float[numClusters];
            for(int i = 0; i < data.Count; i++)
            {
                for (int k = 0; k < numClusters; k++)
                    distances[k] = Logic.ClusteringAlgorithms.SimilarityMatrixCalculations.FindEuclideanDistance(data[i].VectorSpace, means[k].GroupedDocument[0].VectorSpace);

                int newClusterID = MinIndex(distances);

                if(newClustering[newClusterID].GroupedDocument[0].Content != newClustering[i].GroupedDocument[0].Content)
                {
                    changed = true;
                    newClustering[i] = newClustering[newClusterID];
                }
            }
            if (changed == false)
                return false;

            
            for(int i = 0; i < data.Count; i++)
            {
                //newClustering

            }

            return true;
        }

        private static bool UpdateClustering(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        {
            throw new NotImplementedException();
        }

        private static List<DocumentVectorTest> Normilized(List<DocumentVectorTest> rawData)
        {
            List<DocumentVectorTest> result = new List<DocumentVectorTest>();
            for(int j = 0; j < rawData.Count; j++)
            {
                float rowSum = 0.0F;
                for (int i = 0; i < rawData.Count; i++)
                    rowSum += rawData[i].VectorSpace[j];
                float mean = rowSum / rawData[j].VectorSpace.Length;
                float sum = 0.0F;
                for (int z = 0; z < rawData.Count; ++z)
                    for(int k = 0; k < rawData[z].VectorSpace.Length; k++)
                        sum += (result[z].VectorSpace[k] - mean) * (result[z].VectorSpace[k] - mean);
                float sd = sum / result[j].VectorSpace.Length;
                for (int i = 0; i < rawData.Count; i++)
                    for (int p = 0; p < rawData[i].VectorSpace.Length; p++)
                        result[i].VectorSpace[p] = (result[i].VectorSpace[p] - mean) / sd;
            }
            return result;
        }

        private static List<TestCentroid> InitMeans(int numberOfClusters, List<DocumentVectorTest> data, int seed)
        {
            List<TestCentroid> means = new List<TestCentroid>();
            List<int> usedIndex = new List<int>();
            Random rnd = new Random(seed);
            int idx = rnd.Next(0, data.Count);
            usedIndex.Add(idx);

            for (int k = 1; k < numberOfClusters; k++)
            {
                float[] dSquared = new float[data.Count];
                int newMean = -1;
                for (int i = 0; i < data.Count; i++)
                {
                    if (usedIndex.Contains(i) == true) continue;

                    float[] distances = new float[k];
                    for (int j = 0; j < k; j++)
                        distances[j] = Logic.ClusteringAlgorithms.SimilarityMatrixCalculations.FindEuclideanDistance(means[i].GroupedDocument.First().VectorSpace, means[i].GroupedDocument[j].VectorSpace);

                    int m = MinIndex(distances);

                    dSquared[i] = distances[m] * distances[m];
                }
            }

            return means;
        }

        private static int MinIndex(float[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
    }
}
