using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations
{
    static class MyKmeansPPInterpritationcs
    {

        public static List<string> docCollection;
        public static HashSet<string> termCollection;
        public static Dictionary<string, int> wordIndex;
        public static List<DocumentVector> vSpace;
        public static List<DocumentVectorWrapper> wrappedVSpace;
        public static int number_of_Clusters=0;
        public static int document_Collection_length;
        public static int term_Collection_lenght;
        public static int iteration_Count;
        public static bool clusteringChanged;
        

        public static List<Centroid> Showresults(List<string>docCollection, int iteration_Count, List<DocumentVector> vSpace, int document_Collection_length, int numerOfClusters, Dictionary<string, int> wordIndex, HashSet<string> termCollection) 
        {
            //Centroid firstcentroid = chose_Random_Centroid(docCollection, vSpace, document_Collection_length);

            //here we chose the 5 the same cluster centroids - the points must be different!
            //List<Centroid> complete_Centroid_List = Calculate_Centroids(firstcentroid,docCollection,termCollection,vSpace, numerOfClusters);

            List<Centroid> complete_Centroid_List = Logic.ClusteringAlgorithms.Used_functions.CentroidCalculationClass.CentroidCalculationsForKMeansPP(vSpace, numerOfClusters);
            List<Centroid> result = NewKMeansClusterization(number_of_Clusters, docCollection, iteration_Count, vSpace, wordIndex, complete_Centroid_List);
            //List<Centroid> result = KMeansClusterization(number_of_Clusters, docCollection, iteration_Count, vSpace, wordIndex);
            return result;
        }

        #region WrapperDocumentCollectionCreation
        public static List<DocumentVectorWrapper> WrappedCollections(List<DocumentVector> vSpace)
        {
            List<DocumentVectorWrapper> wrappedCollection = new List<DocumentVectorWrapper>();
            for(int i=0; i<vSpace.Count; i++)
            {
                DocumentVectorWrapper WrappedDocument = new DocumentVectorWrapper(vSpace[i]);
                wrappedCollection.Add(WrappedDocument);
            }
            return wrappedCollection;
        }
        #endregion

        public static List<Centroid> NewKMeansClusterization(int number_of_Clusters, List<string> docCollection, int iteration_Count, List<DocumentVector> vSpace, Dictionary<string, int> wordIndex , List<Centroid> completeCentroidList)
        {
            clusteringChanged = true;
            List<Centroid> result = new List<Centroid>();
            //List<Centroid> centroidCollection = generate_random_Centroids(number_of_Clusters,vSpace);
            List<Centroid> centroidCollection = completeCentroidList;
            List<Centroid> oldCentroids = new List<Centroid>();
            List<Centroid> labels = new List<Centroid>();
            List<DocumentVector> newVSpace2 = new List<DocumentVector>();
            int iterations = 0;
            /*
            List<Centroid> oldRecomputedResult = new List<Centroid>();
            List<Centroid> recomputedCollection = new List<Centroid>();
            List<DocumentVector> newVSpace = new List<DocumentVector>(vSpace);
            
            List<Centroid> oldfilledCentroidCollection = new List<Centroid>();
            */

            for(;iterations<iteration_Count;)
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
                        centroidCollection = AverageMeansAssigned(labels, newVSpace2);
                        labels = AssignDocumentToCluster(centroidCollection, vSpace);
                        oldCentroids = labels;
                        newVSpace2 = vSpace;
                        //centroidCollection = AverageMeansAssigned(labels, newVSpace2);
                        result = labels;
                        clusteringChanged = ClusteringChanged(oldCentroids, labels);
                    }
                    
                    iterations ++;

                }
                while (clusteringChanged == true & iterations <= iteration_Count);
            }

            #region old_part_ofCode
            /*
            for (int i=0; i<iteration_Count; i++)
            {
                if (i <= 2)
                {
                    newVSpace = new List<DocumentVector>(newVSpace2);
                    List<Centroid> fillCentroidCollection = AssignDocumentToCluster(centroidCollection, newVSpace);
                    vSpace = new List<DocumentVector>(newVSpace2);
                    oldfilledCentroidCollection = new List<Centroid>(fillCentroidCollection);
                    oldRecomputedResult = AverageMeansAssigned(fillCentroidCollection, vSpace);
                }
                else
                {
                    if (clusteringChanged == false)
                    {
                        result = oldfilledCentroidCollection;
                        break;
                    }
                    else
                    {
                        List<Centroid> oldRecomputedResultCopy = new List<Centroid>(oldRecomputedResult);
                        List<Centroid> fillCentroidCollection = AssignDocumentToCluster(oldRecomputedResult, vSpace);
                        //List<Centroid> fillCentroidCollectionCopy = new List<Centroid>(fillCentroidCollection);
                        newVSpace2 = new List<DocumentVector>(vSpace);
                        recomputedCollection = AverageMeansAssigned(fillCentroidCollection, newVSpace2);
                        fillCentroidCollection = AssignDocumentToCluster(centroidCollection, vSpace);
                        oldRecomputedResult = recomputedCollection;
                        result = fillCentroidCollectionCopy;
                        clusteringChanged = ClusteringChanged(oldfilledCentroidCollection, fillCentroidCollection);
                        //recomputedCollection = new List<Centroid>();
                        //fillCentroidCollection = new List<Centroid>();
                        //oldfilledCentroidCollection = new List<Centroid>();

                    }
                    //tutaj wywala - newVspace = null
                }
            }   
            */
            #endregion
            return result;
        }

        /*
        private static  bool ClusteringChanged(List<Centroid> oldRecomputedResult, List<Centroid> recomputedCollection)
        {
            //bool flag = true;
            
            for(int i=0; i<oldRecomputedResult.Count; i++)
            {
                for(int j=0; j<oldRecomputedResult[j].GroupedDocument.Count; j++)
                {
                    for(int j1=0; j1<recomputedCollection[j].GroupedDocument.Count; j1++)
                    {
                    
                        //if(oldRecomputedResult==recomputedCollection)
                        //(oldRecomputedResult[j].GroupedDocument.Count == recomputedCollection[j].GroupedDocument.Count) & (oldRecomputedResult[j].GroupedDocument[j1].Content == recomputedCollection[j].GroupedDocument[j1].Content))
                        //flag = true;
                        //else
                        // flag = false;
                        /*
                    }
                }
            }
            
            //return flag;
            return oldRecomputedResult == recomputedCollection;
        }
    */

        /*
        private static bool ClusteringChanged(List<Centroid> oldRecomputedResult, List<Centroid> recomputedCollection)
        {
            bool result = true;
            IEnumerable<Centroid> differenceSecondFromFirst = recomputedCollection.Except(oldRecomputedResult);
            IEnumerable<Centroid> differenceFirstFromSecond = oldRecomputedResult.Except(recomputedCollection);
            List<Centroid> firstDifferenceList = differenceFirstFromSecond.ToList();
            List<Centroid> secondDifferenceList = differenceSecondFromFirst.ToList();
            if (firstDifferenceList.Count == 0 && secondDifferenceList.Count == 0)
                result = false;
            else
                result = true;
            return result;
        }
        */

        private static bool ClusteringChanged(List<Centroid> oldRecomputedResult, List<Centroid> recomputedCollection)
        {
            bool result = true;

            for(int i=0; i<oldRecomputedResult.Count; i++)
            {
                for(int j=0; j<oldRecomputedResult[i].GroupedDocument.Count; j++)
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

        public static List<Centroid> AverageMeansAssigned(List<Centroid> fillCentroidCollection, List<DocumentVector> vectorSpace)
        {
            List<Centroid>result;
            List<DocumentVector> newVectorSpace = vectorSpace;
            int length = vectorSpace[0].VectorSpace.Length;
            float[] newVectorSpaceArray = new float[length];
            float[] minDistancesToCluster = new float[0];

            for(int i=0; i<length; i++)
            {
                newVectorSpaceArray[i] = 0.0F;
            }

            for (int c=0; c<fillCentroidCollection.Count; c++)
            {
                for(int gd=0; gd<fillCentroidCollection[c].GroupedDocument.Count; gd++)
                {
                    for(int k=0; k<fillCentroidCollection[c].GroupedDocument[gd].VectorSpace.Length; k++)
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
                        newVectorSpaceArray[k1] = newVectorSpaceArray[k1]/fillCentroidCollection[c1].GroupedDocument.Count;
                    }
                }
            }

            float minDist = 0.1F;
            float currentValue = 0.1F;
            int index = 0;

            for (int i=0; i<fillCentroidCollection.Count; i++)
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
                DocumentVector newClusterCenter = fillCentroidCollection[i].GroupedDocument[index];
                index = 0;
                fillCentroidCollection[i].GroupedDocument.Clear();
                fillCentroidCollection[i].GroupedDocument.Add(newClusterCenter);
            }

            minDistancesToCluster = new float[0];
            result = new List<Centroid>(fillCentroidCollection);
            return result;
        }

        private static List<Centroid> AssignDocumentToCluster(List<Centroid> centroidCollection, List<DocumentVector> vectorSpace)
        {
            var result = new List<Centroid>();
            //List<DocumentVector> newVectorSpace = vectorSpace;
            List<DocumentVector> newVectorSpace = new List<DocumentVector>(vectorSpace);
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
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            newVectorSpace = new List<DocumentVector>(vectorSpace);
            return result;
        }

        #region Dont_use_in_KMans
        private static List<Centroid> generate_random_Centroids(int number_of_Clusters, List<DocumentVector> vSpace)
        {
            List<Centroid> result = new List<Centroid>();
            HashSet<int> clusterIndexes = new HashSet<int>();
            Random rand = new Random();

            while (clusterIndexes.Count != number_of_Clusters)
            {
                for (int i = 0; i < number_of_Clusters; i++)
                {
                    int index = rand.Next(0, vSpace.Count);
                    clusterIndexes.Add(index);
                }
            }

            for (int j=0; j<=clusterIndexes.Count-1; j++)
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
            List<DocumentVector> vSpaceCopy = new List<DocumentVector>(vSpace);
            for(int i=1; i<= number_of_Clusters; i++)
            {
                Centroid next_Centroid = Calculate_Next_Centroid(firstcentroid, vSpaceCopy);
                list_of_Centroid.Add(next_Centroid);
                vSpaceCopy.Remove(next_Centroid.GroupedDocument[0]);
                //firstcentroid = next_Centroid;
            }
            return list_of_Centroid;
        }

        private static Centroid Calculate_Next_Centroid(Centroid firstcentroid, List<DocumentVector> vSpace)
        {
            Centroid next_centroid = new Centroid();
            next_centroid.GroupedDocument = new List<DocumentVector>();
            List<DocumentVector> vSpaceCopy = new List<DocumentVector>(vSpace);
            float[] probabilitiesMatrixSimple = CalculateProbabilityArray(firstcentroid, vSpaceCopy);
            float[] probabilitiesMatrix = new float[probabilitiesMatrixSimple.Length];

            for (var i = 0; i<probabilitiesMatrix.Length; i++)
                  probabilitiesMatrix[i] = 0;

            for (var i = 0; i < probabilitiesMatrix.Length; i++)
                for (var j = 0; j < i; j++)
                    probabilitiesMatrix[i] += probabilitiesMatrixSimple[j];

            Random rand = new Random();
            
            float interval_Value = (float)rand.NextDouble();
            float sum_Of_Probabilies=0.0F;
            int index_of_min_distance_element = 0;
            for(int i=0; i<probabilitiesMatrix.Length; i++)
            {
                sum_Of_Probabilies += probabilitiesMatrix[i];
                //here are the problem! - trying to fix;
            }
            for(int j=0; j<probabilitiesMatrix.Length; j++)
            {
                if (sum_Of_Probabilies > interval_Value & sum_Of_Probabilies < probabilitiesMatrix[j])
                {
                    index_of_min_distance_element = j - 1;
                }
                else
                {
                    continue;
                }
            }

            next_centroid.GroupedDocument.Add(vSpaceCopy[index_of_min_distance_element]);
            vSpaceCopy.RemoveAt(index_of_min_distance_element);
             // but here we can find distance from oldCentroid tp old Centroid
            return next_centroid;
        }

        public static float[] CalculateProbabilityArray(Centroid oldCentroid, List<DocumentVector> vSpace)
        {
            List<DocumentVector> vSpaceCopy = new List<DocumentVector>(vSpace);
            float[] vector_A = oldCentroid.GroupedDocument[0].VectorSpace;
            float[] DistanceQuad = new float[vSpaceCopy.Count];

            for(int i=0; i<DistanceQuad.Length; i++)
                DistanceQuad[i] = 0;

            float SumDistanceQuad=0;
            int previous_index = vSpaceCopy.IndexOf(oldCentroid.GroupedDocument[0]);
            for(int j=0; j<= vSpaceCopy.Count-1; j++)
            {
                float[] vector_B = vSpaceCopy[j].VectorSpace;
                for(int k = 0; k<= vSpaceCopy[j].VectorSpace.Length-1; k++)
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
        #endregion
    }
}
