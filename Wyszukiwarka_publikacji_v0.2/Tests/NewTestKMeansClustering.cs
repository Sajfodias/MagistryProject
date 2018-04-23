using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    static class NewTestKMeansClustering
    {
        public static List<TestCentroid> Cluster(List<DocumentVectorTest> rawData, int numClusters, int iterationCount)
        {
            int iteration = 0;
            List<TestCentroid> clustering = new List<TestCentroid>(numClusters);
            List<DocumentVectorTest> data = TestDocVectorCreator.NormalizationDocumentVectors(rawData);
            //List<DocumentVectorTest> data = Normilized(rawData);
            //List<TestCentroid> means = InitMeans(numClusters, data, seed);
            List<TestCentroid> means = Test_KMeans.CentroidCalculationsForKMeans(data, numClusters);
            bool changed = true; // change in at least one cluster assignment?
            bool success = true; // all means computed? (no zero-count clusters)

            //int maxCount = rawData.Count * 10; // sanity check

            while (changed == true && success == true && iteration < iterationCount) // technically this is Lloyd's algorithm
            {
                var clusteringUpdate = UpdateClustering(data, clustering, means); // (re)assign tuples to clusters. no effect if fail
                changed = clusteringUpdate.Item1;
                clustering = clusteringUpdate.Item2;
                //changed = UpdateClustering(data, means);
              /*  var meansUpdate = UpdateMeans(data, clustering, means); // compute new cluster means if possible. no effect if fail
                success = meansUpdate.Item1;
                means = meansUpdate.Item2;
                */
                foreach (var center in clustering)
                    center.CalculateMeans();
                for (var  i = 0; i <clustering.Count; i++)
                {
                    means[i].means = clustering[i].means;
                }
                ++iteration; // k-means typically converges very quickly
            }

            return clustering;
        }

        #region OldVersion
        /*
        private static bool UpdateMeans(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        {
            int numClusters = means.Count;
            bool changed = false;

            List<TestCentroid> newClustering = new List<TestCentroid>(numClusters);
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


            List<TestCentroid> clusterCounts = new List<TestCentroid>(numClusters);
            for(int i = 0; i < data.Count; i++)
            {
                var cluster = newClustering[i];
                ++clusterCounts.IndexOf(cluster);
            }

            return true;
        }
        */
        #endregion

        #region OldUpdateMeans
        /*
        public static bool UpdateMeans(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        {
            List<DocumentVectorTest> newVectorSpace = new List<DocumentVectorTest>(data);
            int vectorSpaceLength = data[0].VectorSpace.Length;
            int numClusters = means.Count;
            int[] clusterCounts = new int[numClusters];

            #region AccordingToTheAuthorsWeCanOmitThis
            
            for(int c = 0; c < data.Count; c++)
            {
                var cluster = data[c];
                ++clusterCounts[data.IndexOf(cluster)];
            }
            
            #endregion

            for (int k = 0; k < numClusters; k++)
                if (clusterCounts[k] == 0)
                    return false;

            for (int cl = 0; cl < means.Count; cl++)
                clustering[cl].GroupedDocument.Clear();

            for(int i = 0; i < data.Count; i++)
            {
                var cluster = clustering.ElementAt(i);
                for (int j = 0; j < cluster.GroupedDocument.Count; j++)
                    for (int v = 0; v < cluster.GroupedDocument[j].VectorSpace.Length; v++)
                        clustering[clustering.IndexOf(cluster)].GroupedDocument[j].VectorSpace[v] += data[i].VectorSpace[v];
            }

            for (int k = 0; k < clustering.Count; ++k)
                for (int j = 0; j < clustering[k].GroupedDocument.Count; j++)
                    for (int z = 0; z < clustering[k].GroupedDocument[j].VectorSpace.Length; z++)
                        clustering[k].GroupedDocument[j].VectorSpace[z] /= clusterCounts[k];
            return true;
        }
        */
        #endregion

        public static Tuple<bool, List<TestCentroid>> UpdateMeans(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        {
            Tuple<bool, List<TestCentroid>> result;
            int numClusters = means.Count;
            bool changed = false;
            float minVal = float.MaxValue;
            int minIndex = 0;
            List<TestCentroid> centers = new List<TestCentroid>(numClusters);
            centers = TestCentroidInitializer(numClusters);
            List<TestCentroid> newMeans = new List<TestCentroid>(numClusters);
            newMeans = TestCentroidInitializer(numClusters);
            
            for(int i=0; i < means.Count; i++)
            {
                DocumentVectorTest newCenter = new DocumentVectorTest();
                newCenter.VectorSpace = new float[means[i].GroupedDocument[0].VectorSpace.Length];
                for (int j = 0; j < means[i].GroupedDocument.Count; j++)
                {
                    for (int v = 0; v < means[i].GroupedDocument[j].VectorSpace.Length; v++)
                        newCenter.VectorSpace[v] += means[i].GroupedDocument[j].VectorSpace[v];
                    for (int z = 0; z < means[i].GroupedDocument[j].VectorSpace.Length; z++)
                        newCenter.VectorSpace[z] = newCenter.VectorSpace[z] / means[i].GroupedDocument.Count;
                }
                newCenter.Content = "ideal center of " + i.ToString() + " cluster";
                centers[i].GroupedDocument.Add(newCenter);
            }
            
            for(int i=0; i<clustering.Count; i++)
            {
                float[] distances = new float[clustering[i].GroupedDocument.Count];
                for (int j = 0; j <clustering[i].GroupedDocument.Count; j++)
                {
                    distances[j] = Distance(clustering[i].GroupedDocument[j].VectorSpace, centers[i].GroupedDocument[0].VectorSpace);
                }
                for(int j = 0; j < distances.Length; j++)
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
            result = new Tuple<bool, List<TestCentroid>>(changed, means);
            return result;
        }

        private static List<TestCentroid> TestCentroidInitializer(int numClusters)
        {
            List<TestCentroid> initializedList = new List<TestCentroid>(numClusters);
            for(int i = 0; i< numClusters; i++)
            {
                TestCentroid newTestCentroid = new TestCentroid();
                newTestCentroid.GroupedDocument = new List<DocumentVectorTest>();
                initializedList.Add(newTestCentroid);
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

        //private static bool UpdateClustering(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        private static Tuple<bool,List<TestCentroid>> UpdateClustering(List<DocumentVectorTest> data, List<TestCentroid> clustering, List<TestCentroid> means)
        {
            int numClusters = means.Count;
            bool changed = false;
            clustering = new List<TestCentroid>(means);
            List<TestCentroid> newClustering = new List<TestCentroid>();
            newClustering = TestCentroidInitializer(numClusters);
            Tuple<bool, List<TestCentroid>> result;

       /*    for (int i = 0; i < numClusters; i++)
                newClustering[i].GroupedDocument.Add(means[i].GroupedDocument[0]);
       */
            float[,] distances = new float[data.Count, numClusters];
            for (int d = 0; d < data.Count; d++)
                for (int k = 0; k < numClusters; k++)
                    distances[d, k] = Distance(data[d].VectorSpace, means[k].means);//GroupedDocument[0].VectorSpace);

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
            #region oldCode
            /*
            //newClusterID = MinIndex(distances);
            for (int i = 0; i < data.Count * 2; i++)
            {
                #region comment1
                /*
                for(int j = 0; j < numClusters; j++)
                {
                
                #endregion
                newClusterID = MinIndexInRow(distances,i);
                if (!usedDocs.Contains(data[i]))
                {
                    newClustering[newClusterID].GroupedDocument.Add(data[i]);
                    usedDocs.Add(data[i]);
                    newClusterID = 0;
                }
                else
                {
                    newClusterID = 0;
                    continue;
                }

                //data.RemoveAt(i);
                
                #region comment2
                /*
                if (clustering[newClusterID].GroupedDocument[0] != clustering[j].GroupedDocument[0])
                {
                    changed = true;
                    clustering[]
                    //newClustering[j] = clustering[newClusterID];
                    //newClustering[k] = newClusterID;
                }

                if (changed == false)
                    return false;

            }   
            */
            #endregion
            #region comment3
            /*
            int[] clusterCounts = new int[numClusters];
            for(int i = 0; i < data.Count; ++i)
            {
                int cluster = newClustering[i];
                ++clusterCounts[cluster];
            }
            */
            #endregion
            for (int k = 0; k < numClusters; ++k)
                if (newClustering[k].GroupedDocument.Count == 0)
                    changed=false;

            clustering = newClustering;
            result = new Tuple<bool, List<TestCentroid>>(changed, clustering);
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

        private static List<DocumentVectorTest> Normilized(List<DocumentVectorTest> rawData)
        {
            List<DocumentVectorTest> result = new List<DocumentVectorTest>(rawData);
            for(int j = 0; j < rawData.Count; j++)
            {
                float rowSum = 0.0F;
                for (int i = 0; i < rawData[j].VectorSpace.Length; i++)
                    rowSum += result[j].VectorSpace[i];
                float mean = rowSum / result[j].VectorSpace.Length;
                float sum = 0.0F;
                for (int z = 0; z < rawData.Count; ++z)
                    for(int k = 0; k < rawData[z].VectorSpace.Length; k++)
                        sum += (result[z].VectorSpace[k] - mean) * (result[z].VectorSpace[k] - mean);
                float sd = sum / result[j].VectorSpace.Length;
                for (int i = 0; i < result.Count; i++)
                    for (int p = 0; p < result[i].VectorSpace.Length; p++)
                        result[i].VectorSpace[p] = (result[i].VectorSpace[p] - mean) / sd;
            }
            return result;
        }

        /*
        private static List<TestCentroid> InitMeans(int numberOfClusters, List<DocumentVectorTest> data, int seed)
        {
            List<TestCentroid> means = new List<TestCentroid>();
            List<int> usedIndex = new List<int>();
            Random rnd = new Random(seed);
            int idx = rnd.Next(0, data.Count);
            usedIndex.Add(idx);
            float[] dSquared = new float[data.Count];
            int newMean = -1;

            for (int k = 1; k < numberOfClusters; k++)
            {
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

            float p = (float)rnd.NextDouble();
            float sum = 0.0F;
            for (int i = 0; i < dSquared.Length; i++)
                sum += dSquared[i];
            float cumulative = 0.0F;

            int ii = 0;
            int sanity = 0;
            while(sanity < data.Count * 2)
            {
                cumulative += dSquared[ii] / sum;
                if(cumulative >= p && usedIndex.Contains(ii) == false)
                {
                    newMean = ii;
                    usedIndex.Add(newMean);
                    break;
                }
                ++ii;
                if (ii >= dSquared.Length)
                    ii = 0;
                ++sanity;
            }
            return means;
        }
        */

        private static int[] MinIndex(float[,] distances)
        {
            int[] indexOfMin = new int[distances.GetLength(0)];
            
            for (int k1 = 0; k1 < distances.GetLength(0); ++k1)
            {
                for(int k2 = 0; k2 < distances.GetLength(1); ++k2)
                {
                    float smallDist = distances[k1, 0];
                    if (distances[k1,k2] < smallDist)
                    {
                        smallDist = distances[k1,k2];
                        indexOfMin[k1] = k2;
                    }
                }
            }
            return indexOfMin;
        }

        public static int[] AssignDocsToClusters(float[,] clusteringResult, int clusterNumber, List<DocumentVectorTest> docCollection)
        {
            int[] result = new int[clusteringResult.GetLength(0)];
            List<DocumentVectorTest> newCopyDocCollection = new List<DocumentVectorTest>(docCollection);

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
                //newCopyDocCollection.RemoveAt(i);
            }
            return result;
        }
    }
}
