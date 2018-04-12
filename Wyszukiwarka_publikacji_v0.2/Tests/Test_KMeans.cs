
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Test_KMeans
    {
        public static bool clusteringChanged;

        public static List<TestCentroid> CentroidCalculationsForKMeans(List<DocumentVectorTest> data, int ClusterNumber)
        {
            List<TestCentroid> centroidList = new List<TestCentroid>();
            Random randomizer = new Random();
            HashSet<int> indexSet = new HashSet<int>();
            int index = 0;

            while (centroidList.Count != ClusterNumber)
            {
                index = randomizer.Next(0, data.Count + 1);
                if (!indexSet.Contains(index))
                {
                    indexSet.Add(index);
                    TestCentroid newCentroid = new TestCentroid();
                    newCentroid.GroupedDocument = new List<DocumentVectorTest>();
                    newCentroid.GroupedDocument.Add(data[index]);
                    centroidList.Add(newCentroid);
                }
                else if (indexSet.Contains(index))
                {
                    continue;
                }
            }
            return centroidList;
        }

        public static List<TestCentroid> NewKMeansClusterization(int number_of_Clusters, int iteration_Count, List<DocumentVectorTest> vSpace, List<TestCentroid> completeCentroidList)
        {
            clusteringChanged = true;
            List<TestCentroid> result = new List<TestCentroid>();
            //List<Centroid> centroidCollection = generate_random_Centroids(number_of_Clusters,vSpace);
            List<TestCentroid> centroidCollection = completeCentroidList;
            List<TestCentroid> oldCentroids = new List<TestCentroid>();
            List<TestCentroid> labels = new List<TestCentroid>();
            List<DocumentVectorTest> newVSpace2 = new List<DocumentVectorTest>();
            int iterations = 0;
            /*
            List<Centroid> oldRecomputedResult = new List<Centroid>();
            List<Centroid> recomputedCollection = new List<Centroid>();
            List<DocumentVector> newVSpace = new List<DocumentVector>(vSpace);
            
            List<Centroid> oldfilledCentroidCollection = new List<Centroid>();
            */

            for (; iterations < iteration_Count;)
            {
                do
                {
                    if (iterations < 1)
                    {
                        labels = AssignDocumentToCluster(centroidCollection, vSpace);
                        newVSpace2 = vSpace;
                    }
                    //oldCentroids = centroidCollection;
                    else
                    {
                        centroidCollection = UpdateMeans(labels, newVSpace2);
                        labels = AssignDocumentToCluster(centroidCollection, vSpace);
                        oldCentroids = labels;
                        newVSpace2 = vSpace;
                        //centroidCollection = UpdateMeans(labels, newVSpace2);
                        result = labels;
                        clusteringChanged = ClusteringChanged(oldCentroids, labels);
                    }

                    iterations++;

                }
                while (clusteringChanged == true & iterations <= iteration_Count);
            }


            return result;
        }

        private static bool ClusteringChanged(List<TestCentroid> oldRecomputedResult, List<TestCentroid> recomputedCollection)
        {
            bool result = true;

            for (int i = 0; i < oldRecomputedResult.Count; i++)
            {
                for (int j = 0; j < oldRecomputedResult[i].GroupedDocument.Count; j++)
                {
                    foreach (var oldDoc in oldRecomputedResult[i].GroupedDocument[j].Content)
                    {
                        if (recomputedCollection[i].GroupedDocument[j].Content.Equals(oldDoc))
                            result = false;
                        else
                            result = true;
                    }
                }

            }
            return result;
        }

        public static List<TestCentroid> UpdateMeans(List<TestCentroid> fillCentroidCollection, List<DocumentVectorTest> vectorSpace)
        {
            List<TestCentroid> result = new List<TestCentroid>();
            List<DocumentVectorTest> newVectorSpace = new List<DocumentVectorTest>(vectorSpace);
            int length = vectorSpace[0].VectorSpace.Length;
            float[] newVectorSpaceArray = new float[length];
            float[] minDistancesToCluster = new float[0];

            for (int i = 0; i < length; i++)
            {
                newVectorSpaceArray[i] = 0.0F;
            }

            for (int c = 0; c < fillCentroidCollection.Count; c++)
            {
                for (int gd = 0; gd < fillCentroidCollection[c].GroupedDocument.Count; gd++)
                {
                    for (int k = 0; k < fillCentroidCollection[c].GroupedDocument[gd].VectorSpace.Length; k++)
                    {
                        newVectorSpaceArray[k] += fillCentroidCollection[c].GroupedDocument[gd].VectorSpace[k];
                    }
                }
            }

            for (int c1 = 0; c1 < fillCentroidCollection.Count; c1++)
            {
                for (int gd1 = 0; gd1 < fillCentroidCollection[c1].GroupedDocument.Count; gd1++)
                {
                    for (int k1 = 0; k1 < fillCentroidCollection[c1].GroupedDocument[gd1].VectorSpace.Length; k1++)
                    {
                        newVectorSpaceArray[k1] = newVectorSpaceArray[k1] / fillCentroidCollection[c1].GroupedDocument.Count;
                    }
                }
            }

            float minDist = 0.1F;
            float currentValue = 0.1F;
            int index = 0;

            for (int i = 0; i < fillCentroidCollection.Count; i++)
            {
                minDistancesToCluster = new float[fillCentroidCollection[i].GroupedDocument.Count];
                for (int j = 0; j < fillCentroidCollection[i].GroupedDocument.Count; j++)
                {
                    //minDistancesToCluster = new float[fillCentroidCollection[i].GroupedDocument.Count];
                    minDistancesToCluster[j] = SimilarityMatrixCalculations.FindEuclideanDistance(fillCentroidCollection[i].GroupedDocument.First().VectorSpace, fillCentroidCollection[i].GroupedDocument[j].VectorSpace);
                    //}

                    for (int z = 0; z < minDistancesToCluster.Length; z++)
                    {
                        currentValue = minDistancesToCluster[z];
                        if (currentValue <= minDist && currentValue != 0)
                        {
                            minDist = currentValue;
                            index = z;
                        }
                        //here we must to find the closest document to new vectorSpace;
                        //for all docs in cluster create the vectorSpace 
                    }
                    /*
                    DocumentVector newClusterCenter = fillCentroidCollection[i].GroupedDocument[index];
                    fillCentroidCollection[i].GroupedDocument.Clear();
                    fillCentroidCollection[i].GroupedDocument.Add(newClusterCenter);
                    */
                }
                DocumentVectorTest newClusterCenter = fillCentroidCollection[i].GroupedDocument[index];
                index = 0;
                fillCentroidCollection[i].GroupedDocument.Clear();
                fillCentroidCollection[i].GroupedDocument.Add(newClusterCenter);
            }

            minDistancesToCluster = new float[0];
            result = new List<TestCentroid>(fillCentroidCollection);
            return result;
        }

        private static List<TestCentroid> AssignDocumentToCluster(List<TestCentroid> centroidCollection, List<DocumentVectorTest> vectorSpace)
        {
            var result = new List<TestCentroid>();
            //List<DocumentVector> newVectorSpace = vectorSpace;
            List<DocumentVectorTest> newVectorSpace = new List<DocumentVectorTest>(vectorSpace);
            result = centroidCollection;
            //float[,] distancematrix = new float[centroidCollection.Count, vectorSpace.Count];
            float[,] distancematrix = new float[centroidCollection.Count, newVectorSpace.Count];

            float minDistance = 0.1F;
            float currentValue = 0.1F;
            int DocIndex = 0;
            int ClusterIndex = 0;

            foreach (var center in centroidCollection)
            {
                //foreach (var doc in vectorSpace)
                foreach (var doc in newVectorSpace)
                {
                    distancematrix[centroidCollection.IndexOf(center), newVectorSpace.IndexOf(doc)] = SimilarityMatrixCalculations.FindEuclideanDistance(center.GroupedDocument[0].VectorSpace, doc.VectorSpace);
                }
            }

            //while(vectorSpace.Count!=0)
            while (newVectorSpace.Count != 0)
            {
                for (int i = 0; i < centroidCollection.Count; i++)
                {
                    for (int j = 0; j < newVectorSpace.Count; j++) //vectorSpace.Count
                    {
                        currentValue = distancematrix[i, j];
                        if (currentValue <= minDistance || currentValue != 0)
                        {
                            minDistance = currentValue;
                            DocIndex = j;
                            ClusterIndex = i;
                            result[i].GroupedDocument.Add(newVectorSpace[j]);
                            newVectorSpace.RemoveAt(j);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            newVectorSpace = new List<DocumentVectorTest>(vectorSpace);
            return result;
        }

        
    }
}
