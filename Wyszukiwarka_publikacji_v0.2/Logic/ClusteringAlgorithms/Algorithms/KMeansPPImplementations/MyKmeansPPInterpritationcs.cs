using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations
{
    class MyKmeansPPInterpritationcs
    {

        public static List<string> docCollection;
        public static HashSet<string> termCollection;
        public static Dictionary<string, int> wordIndex;
        public static List<DocumentVector> vSpace;
        public static int number_of_Clusters=0;
        public static int document_Collection_length;
        public static int term_Collection_lenght;
        public static int iteration_Count;
        public static bool clusteringChanged;

        public static List<Centroid> Showresults()
        {
            Centroid firstcentroid = chose_Random_Centroid(docCollection, vSpace, document_Collection_length);
            List<Centroid> complete_Centroid_List = Calculate_Centroids(firstcentroid,docCollection,termCollection,vSpace, number_of_Clusters);
            List<Centroid> result = KMeansClusterization(number_of_Clusters, docCollection, iteration_Count, vSpace, wordIndex);
            return result;
        }

        public static List<Centroid> KMeansClusterization(int number_of_Clusters, List<string> docCollection, int iteration_Count, List<DocumentVector> vSpace, Dictionary<string, int> wordIndex)
        {

            clusteringChanged = true;
            List<Centroid> result = new List<Centroid>();
            List<Centroid> centroidCollection = generate_random_Centroids(number_of_Clusters,vSpace);
            List<Centroid> oldRecomputedResult = new List<Centroid>();

            for(int i=0; i< iteration_Count; i++)
            {
                if (i < 2)
                {
                    List<Centroid> fillCentroidCollection = AssignDocumentToCluster(centroidCollection, vSpace);
                    oldRecomputedResult = AverageMeansAssigned(fillCentroidCollection, vSpace);
                }
                else
                {
                    List<Centroid> fillCentroidCollection = AssignDocumentToCluster(centroidCollection, vSpace);
                    List<Centroid> recomputedCollection = AverageMeansAssigned(fillCentroidCollection, vSpace);
                    clusteringChanged = CheckClustering(oldRecomputedResult, recomputedCollection);
                    recomputedCollection = oldRecomputedResult;
                    if (clusteringChanged == false)
                    {
                        result = recomputedCollection;
                        break;
                    }
                }
            }
               
            return result;
        }

        private static  bool CheckClustering(List<Centroid> oldRecomputedResult, List<Centroid> recomputedCollection)
        {
            bool flag = true;
            for(int i=0; i<oldRecomputedResult.Count; i++)
            {
                for(int j=0; j<oldRecomputedResult[j].GroupedDocument.Count; j++)
                {
                    for(int j1=0; j1<recomputedCollection[j].GroupedDocument.Count; j1++)
                    {
                        if ((oldRecomputedResult[j].GroupedDocument.Count == recomputedCollection[j].GroupedDocument.Count) & (oldRecomputedResult[j].GroupedDocument[j1].Content == recomputedCollection[j].GroupedDocument[j1].Content))
                            flag = true;
                        else
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }

        private static List<Centroid> AverageMeansAssigned(List<Centroid> fillCentroidCollection, List<DocumentVector> vectorSpace)
        {
            var result = new List<Centroid>();
            foreach(var cluster in fillCentroidCollection)
            {
                //for i=0 to c.vectorSpace.length
                //for i=0 to  p1,p2,p3,p4,p5 length
                //c[i]=(p1[i]+p2[i]+p3[i]+p4[i]+p5[i])/5
                //return c[i]
            }
            //throw new NotImplementedException();
            return result;
        }

        private static List<Centroid> AssignDocumentToCluster(List<Centroid> centroidCollection, List<DocumentVector> vectorSpace)
        {
            var result = new List<Centroid>();
            List<DocumentVector> newVectorSpace = vectorSpace;
            result = centroidCollection;
            float[,] distancematrix = new float[centroidCollection.Count, vectorSpace.Count];

            float minDistance = 0.1F;
            float currentValue = 0.1F;
            int DocIndex = 0;
            int ClusterIndex = 0;

            foreach(var center in centroidCollection)
            {
                foreach(var doc in vectorSpace)
                {
                    distancematrix[centroidCollection.IndexOf(center), vectorSpace.IndexOf(doc)] = SimilarityMatrixCalculations.FindEuclideanDistance(center.GroupedDocument[0].VectorSpace, doc.VectorSpace);
                }
            }
            for(int i=0; i<centroidCollection.Count; i++)
            {
                for(int j=0; j<vectorSpace.Count; j++)
                {
                    currentValue = distancematrix[i, j];
                    if (currentValue <= minDistance || currentValue!=0)
                    {
                        minDistance = currentValue;
                        DocIndex = j;
                        ClusterIndex = i;
                        result[i].GroupedDocument.Add(newVectorSpace[j]);
                        newVectorSpace.RemoveAt(j);
                    }
                    else
                    {
                        continue;
                    }
                    
                }
            }
            return result;
        }

        private static List<Centroid> generate_random_Centroids(int number_of_Clusters, List<DocumentVector> vSpace)
        {
            List<Centroid> result = new List<Centroid>();
            HashSet<int> clusterIndexes = new HashSet<int>();
            Random rand = new Random();

            for(int i=0; i<number_of_Clusters; i++)
            {

                int index = rand.Next(0, vSpace.Count);
                clusterIndexes.Add(index);
            }
            for(int j=0; j<=clusterIndexes.Count-1; j++)
            {
                int index = clusterIndexes.ElementAt(j);
                Centroid newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid.GroupedDocument.Add(vSpace[index]);
                result.Add(newCentroid);
            }

            return result;
        }

        private static List<Centroid> Calculate_Centroids(Centroid firstcentroid, List<string> docCollection, HashSet<string> termCollection, List<DocumentVector> vSpace, int number_of_Clusters)
        {
            List<Centroid> list_of_Centroid = new List<Centroid>(number_of_Clusters);
            for(int i=1; i<= number_of_Clusters; i++)
            {
                Centroid next_Centroid = Calculate_Next_Centroid(firstcentroid, vSpace);
                list_of_Centroid.Add(next_Centroid);
            }
            return list_of_Centroid;
        }

        private static Centroid Calculate_Next_Centroid(Centroid firstcentroid, List<DocumentVector> vSpace)
        {
            Centroid next_centroid = new Centroid();
            float[] probabilitiesMatrix = CalculateProbabilityArray(firstcentroid, vSpace);
            Random rand = new Random();
            float interval_Value = (float)rand.NextDouble();
            float sum_Of_Probabilies=0;
            int index_of_min_distance_element = 0;
            for(int i=0; i<=probabilitiesMatrix.Length-1; i++)
            {
                sum_Of_Probabilies += probabilitiesMatrix[i];
                if(sum_Of_Probabilies>interval_Value & sum_Of_Probabilies < probabilitiesMatrix[i])
                {
                    index_of_min_distance_element = i - 1;
                }
                else
                {
                    continue;
                }
            }
            next_centroid.GroupedDocument.Add(vSpace[index_of_min_distance_element]);
             // but here we can find distance from oldCentroid tp old Centroid
            return next_centroid;
        }

        public static float[] CalculateProbabilityArray(Centroid oldCentroid, List<DocumentVector> vSpace)
        {
            float[] vector_A = oldCentroid.GroupedDocument[0].VectorSpace;
            float[] DistanceQuad = new float[vSpace.Count];
            float SumDistanceQuad=0;
            int previous_index = vSpace.IndexOf(oldCentroid.GroupedDocument[0]);
            for(int j=0; j<=vSpace.Count-1; j++)
            {
                float[] vector_B = vSpace[j].VectorSpace;
                for(int k = 0; k<=vSpace[j].VectorSpace.Length-1; k++)
                {
                    DistanceQuad[j] += (float)Math.Pow((vector_A[k] - vector_B[k]), 2);
                }
                SumDistanceQuad += DistanceQuad[j];
            }
            for(int j=0; j<=DistanceQuad.Length-1; j++)
            {
                DistanceQuad[j] = DistanceQuad[j] / SumDistanceQuad;
            }
            return DistanceQuad;
        }

        private static Centroid chose_Random_Centroid(List<string> docCollection, List<DocumentVector> vSpace, int document_Collection_length)
        {
            Centroid firstCentroid = new Centroid();
            firstCentroid.GroupedDocument = new List<DocumentVector>();
            Random rand = new Random();
            int index = rand.Next(0, document_Collection_length);
            DocumentVector firstvector = vSpace[index];
            firstCentroid.GroupedDocument.Add(firstvector);
            return firstCentroid;
        }


    }
}
