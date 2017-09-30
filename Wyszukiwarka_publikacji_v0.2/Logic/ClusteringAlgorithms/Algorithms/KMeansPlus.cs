// This code is the adaptation from the free code developed by  James McCaffrey (jamccaff@microsoft.com, https://jamesmccaffrey.wordpress.com/)
// Projects of James McCaffrey you can find here https://msdn.microsoft.com/en-us/magazine/mt185575.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic;

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

            InitializeClusterCentroid(out result, centroidCollection.Count);

            do
            {
                prevClusterCenter = centroidCollection;

                foreach (DocumentVector docVector in documentCollection)
                {
                    int index = KMeansClustering.FindClosestClusterCenter(centroidCollection, docVector);
                    result[index].GroupedDocument.Add(docVector);
                }

                InitializeClusterCentroid(out centroidCollection, centroidCollection.Count());
                centroidCollection = CalculateMeanPoints(result);
                //stoppingCriteria = CheckStoppingCriteria(prevClusterCenter, centroidCollection);

                /* if (!stoppingCriteria)
                 {
                     InitializeClusterCentroid(out result, centroidCollection.Count);
                 }
             }
             */
                //while (stoppingCriteria == false);
            }
            while (true);
            _counter2 = counter2;
          
            return result;
        }

        public static void InitializeClusterCentroid(out List<Centroid> result, int count)
        {
            List<DocumentVector> docCollection = new List<DocumentVector>();
            result = new List<Centroid>(); //here we can set the number of elements to count like result = new List<Centroid>(count);

            int firstIndex = GenerateRandomNumber(0, count);
            Centroid first_Centroid = new Centroid();
            first_Centroid.GroupedDocument.Add(docCollection.ElementAt(firstIndex));
            result.Add(first_Centroid); //here we have list with 1 document getting using random index

                if(result.Count >= 2)
                {
                    //PointDetails minpd = GetMinDPD(pds);

                    //here we must to calculate distance to other clusters centroids.
                }

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
