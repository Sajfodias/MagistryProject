// This code is the adaptation from the free code developed by  James McCaffrey (jamccaff@microsoft.com, https://jamesmccaffrey.wordpress.com/)
// Projects of James McCaffrey you can find here https://msdn.microsoft.com/en-us/magazine/mt185575.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions;


namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class KMeansPlus
    {
        private static int counter1 = 0;
        private static int counter2 = 0;
        private static ParallelOptions MaxDegree = new ParallelOptions { MaxDegreeOfParallelism = 10 };

        public static List<Centroid> KMeansPlusClusterization(int k, List<DocumentVector> documentCollection, ref int _counter2)
        {
            counter1 = 0;

            List<Centroid> centroidCollection = new List<Centroid>();
            List<Centroid> result = new List<Centroid>();
            List<Centroid> prevClusterCenter;
            Boolean stoppingCriteria;
            Centroid c;
            HashSet<int> uniqRand = new HashSet<int>();

            KMeansClustering.GenerateRandomNumber(ref uniqRand, k, documentCollection.Count);

            foreach (var position in uniqRand)
            {
                c = new Centroid();
                c.GroupedDocument = new List<DocumentVector>();
                c.GroupedDocument.Add(documentCollection[position]); 
                centroidCollection.Add(c);
            }

            KMeansClustering.InitializeClusterCentroid(out result, centroidCollection.Count);

            do
            {
                prevClusterCenter = centroidCollection;

                foreach (DocumentVector docVector in documentCollection)
                {
                    int index = KMeansClustering.FindClosestClusterCenter(centroidCollection, docVector);
                    result[index].GroupedDocument.Add(docVector);
                }

                KMeansClustering.InitializeClusterCentroid(out centroidCollection, centroidCollection.Count());
                centroidCollection = GetSeedPoints(documentCollection, k);
                stoppingCriteria = KMeansClustering.CheckStoppingCriteria(prevClusterCenter, centroidCollection);

                if (!stoppingCriteria)
                {
                    KMeansClustering.InitializeClusterCentroid(out result, centroidCollection.Count);
                }
            }
            while (stoppingCriteria == false);
            
            _counter2 = counter2;
          
            return result;
        }

        public static List<Centroid> GetSeedPoints(List<DocumentVector>docCollection, int count)
        {
            List<DocumentVector> documentCollection = new List<DocumentVector>(docCollection.Count);
            documentCollection = docCollection;
            List<Centroid> seedPoints = new List<Centroid>(count);
            Document documentDetails;
            List<Document> detailedDocumentCollection = new List<Document>();
            int index = 0;

            int firstIndex = GenerateRandomNumber(0, count);
            Centroid first_Centroid = new Centroid();
            first_Centroid.GroupedDocument.Add(documentCollection[firstIndex]);
            seedPoints.Add(first_Centroid); //here we have list with 1 document getting using random index

            for(int i=0; i<=count; i++)
            {
                if(seedPoints.Count >= 2)
                {
                    Document minDocumentDetails = GetMinDocumenDetailsDistance(detailedDocumentCollection);
                    index = GetWeightedProbDist(minDocumentDetails.Weights, minDocumentDetails.Sum);
                    DocumentVector SubsequentDocument = documentCollection[index];
                    Centroid subsequentCentroid = new Centroid();
                    subsequentCentroid.GroupedDocument.Add(SubsequentDocument);
                    seedPoints.Add(subsequentCentroid);

                    documentDetails = new Document();
                    documentDetails = GetAllDetails(documentCollection, subsequentCentroid, documentDetails);
                    detailedDocumentCollection.Add(documentDetails);
                }
                else
                {
                    documentDetails = new Document();
                    documentDetails = GetAllDetails(documentCollection, first_Centroid, documentDetails);
                    detailedDocumentCollection.Add(documentDetails);
                    index = GetWeightedProbDist(documentDetails.Weights, documentDetails.Sum);
                    DocumentVector SecondDocumentVector = documentCollection[index];
                    Centroid second_Centroid = new Centroid();
                    second_Centroid.GroupedDocument.Add(SecondDocumentVector);
                    seedPoints.Add(second_Centroid);

                    documentDetails = new Document();
                    documentDetails = GetAllDetails(documentCollection, second_Centroid, documentDetails);
                    detailedDocumentCollection.Add(documentDetails);
                }
            }
            //PointDetails minpd = GetMinDPD(pds);    
            //here we must to calculate distance to other clusters centroids.
            return seedPoints;
        }

        private static Document GetAllDetails(List<DocumentVector> docCollection, Centroid subsequentCentroid, Document documentDetails)
        {
            float[] Weights = new float[docCollection.Count];
            float minD = float.MaxValue;
            float Sum = 0;
            int i = 0;

            foreach(DocumentVector docVector in docCollection)
            {
                if (subsequentCentroid.GroupedDocument.Contains(docVector))
                    continue;

                Weights[i] = GetEucliedeanDistance(docVector, subsequentCentroid.GroupedDocument[0]);
                Sum += Weights[i];
                if (Weights[i] < minD)
                    minD = Weights[i];
                i++;
                //here we must to check is the docVector(type DocumentVector) a seedPoint(type Centroid - list of DocumentVector)
                /*foreach (Point p in allPoints)
                {
                    if (p == seedPoint) //Delta is 0
                        continue;

                    Weights[i] = GetEuclideanD(p, seedPoint);
                    Sum += Weights[i];
                    if (Weights[i] < minD)
                        minD = Weights[i];
                    i++;
                }
                */
                
            }
            documentDetails.seed_Document = subsequentCentroid.GroupedDocument[0];
            documentDetails.Weights = Weights;
            documentDetails.Sum = Sum;
            documentDetails.MinDistance = minD;

            return documentDetails;
        }

        private static float GetEucliedeanDistance(DocumentVector docVector1, DocumentVector docVector2)
        {
            float euclideanDistance = 0;

            Parallel.For(0, docVector1.VectorSpace.Length, i => {
                euclideanDistance += (float)Math.Pow((docVector1.VectorSpace[i] - docVector2.VectorSpace[i]), 2);
            });

            #region sequential_Euclidean_distance
            /*
            for (var i = 0; i <= vector_A.Length-1; i++)
            {
                euclideanDistance += (float)Math.Pow((vector_A[i] - vector_B[i]), 2);
            }
            */
            #endregion

            var end_result = (float)Math.Sqrt(euclideanDistance);

            if (float.IsNaN(end_result))
            {
                return 0;
            }
            else
            {
                return end_result;
            }
        }

        private static int GetWeightedProbDist(float[] weights, float sum)
        {
            float p = GetRandNumCrypto();
            float q = 0;
            int i = -1;
            while (q < p)
            {
                i++;
                q += (weights[i] / sum);
            }
            return i;
        }

        private static float GetRandNumCrypto()
        {
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            var result = (float)BitConverter.ToUInt64(salt, 0) / UInt64.MaxValue;
            return result;
        }

        private static int GetRandNumCrypto(int min, int max)
        {
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            var result = (int)((float)BitConverter.ToUInt64(salt, 0) / UInt64.MaxValue * (max - min)) + min;
            return result;
        }

        private static Document GetMinDocumenDetailsDistance(List<Document> detailedDocumentCollection)
        {
            float minValue = float.MaxValue;
            List<Document> sameDistValues = new List<Document>();
            
            foreach(Document ddoc in detailedDocumentCollection)
            {
                if(ddoc.MinDistance < minValue)
                {
                    sameDistValues.Clear();
                    minValue = ddoc.MinDistance;
                    sameDistValues.Add(ddoc);
                }
                if (ddoc.MinDistance == minValue)
                    if (!sameDistValues.Contains(ddoc))
                        sameDistValues.Add(ddoc);
            }

            if (sameDistValues.Count > 1)
                return sameDistValues[GetRandNumCrypto(0, sameDistValues.Count)];
            else
                return sameDistValues[0];
        }

        public static List<Centroid> CalculateMeanPoints(List<Centroid> clusterCenter)
        {
            for (int i = 0; i < clusterCenter.Count(); i++)
            {
                if (clusterCenter[i].GroupedDocument.Count() > 0)
                {
                    for (int j = 0; j < clusterCenter[i].GroupedDocument[0].VectorSpace.Count(); j++)
                    {
                        float total = 0;

                        foreach (DocumentVector vSpace in clusterCenter[i].GroupedDocument)
                            total += vSpace.VectorSpace[j];

                        clusterCenter[i].GroupedDocument[0].VectorSpace[j] = total / clusterCenter[i].GroupedDocument.Count();

                    }
                }
            }
            return clusterCenter;
        }


        public static int GenerateRandomNumber(int min, int max)
        {
            Random r = new Random();
            int pos = r.Next(min, max);
            return pos;
        }

    }
}
