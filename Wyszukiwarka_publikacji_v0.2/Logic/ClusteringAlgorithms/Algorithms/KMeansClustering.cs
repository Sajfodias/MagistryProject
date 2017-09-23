/* Based on solution
 * Dil Prasad Kunwar
 * Kathmandu, Nepal
 * Email:samir_k2002@yahoo.com
 * All expanation you can find at:
 * https://www.codeproject.com/Articles/439890/Text-Documents-Clustering-using-K-Means-Algorithm
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class KMeansClustering
    {
        private static int counter1 = 0;
        private static int counter2 = 0;
        private static ParallelOptions MaxDegree = new ParallelOptions { MaxDegreeOfParallelism = 10 };
        

        public static List<Centroid> DocumentClusterPreparation(int k, List<DocumentVector> documentCollection, ref int _counter2)
        {
            counter1 = 0;
            
            List<Centroid> centroidCollection = new List<Centroid>();
            List<Centroid> result = new List<Centroid>();
            List<Centroid> prevClusterCenter;
            Boolean stoppingCriteria;
            Centroid c;
            HashSet<int> uniqRand = new HashSet<int>();

            GenerateRandomNumber(ref uniqRand, k, documentCollection.Count);


            //Parallel.ForEach(uniqRand, MaxDegree,(position) =>
            foreach (var position in uniqRand)
            {
                c = new Centroid();
                c.GroupedDocument = new List<DocumentVector>();
                c.GroupedDocument.Add(documentCollection[position]);
                centroidCollection.Add(c);
            }
            //);
            

            InitializeClusterCentroid(out result, centroidCollection.Count);

            do
            {
                prevClusterCenter = centroidCollection;

                foreach(DocumentVector docVector in documentCollection)
                {
                    int index = FindClosestClusterCenter(centroidCollection, docVector);
                    result[index].GroupedDocument.Add(docVector);
                }

                InitializeClusterCentroid(out centroidCollection, centroidCollection.Count());
                centroidCollection = CalculateMeanPoints(result);
                stoppingCriteria = CheckStoppingCriteria(prevClusterCenter, centroidCollection);

                if (!stoppingCriteria)
                {
                    InitializeClusterCentroid(out result, centroidCollection.Count);
                }
            }
            while (stoppingCriteria == false);

            _counter2 = counter2;
            return result;
        }

        private static bool CheckStoppingCriteria(List<Centroid> prevClusterCenter, List<Centroid> newClusterCenter)
        {
            counter1++;
            counter2 = counter1;
            if (counter1 > 10000)
                return true;
            else
            {
                bool stoppingCriteria;
                int[] changeIndex = new int[newClusterCenter.Count()];
                int index = 0;
                do
                {
                    int count = 0;
                    if (newClusterCenter[index].GroupedDocument.Count == 0 && prevClusterCenter[index].GroupedDocument.Count != 0)
                        index++;
                    else if (newClusterCenter[index].GroupedDocument.Count != 0 && prevClusterCenter[index].GroupedDocument.Count != 0)
                    {
                        for (int j = 0; j < newClusterCenter[index].GroupedDocument[0].VectorSpace.Count(); j++)
                            if (newClusterCenter[index].GroupedDocument[0].VectorSpace[j] == prevClusterCenter[index].GroupedDocument[0].VectorSpace[j])
                                count++;
                        if (count == newClusterCenter[index].GroupedDocument[0].VectorSpace.Count())
                            changeIndex[index] = 0;
                        else changeIndex[index] = 1;
                        index++;
                    }
                    else
                    {
                        index++;
                        continue;
                    }
                }
                while (index < newClusterCenter.Count());

                if (changeIndex.Where(k => (k != 0)).Select(r => r).Any())
                    stoppingCriteria = false;
                else
                    stoppingCriteria = true;
                return stoppingCriteria;
            }
        }

        private static int FindClosestClusterCenter(List<Centroid> clusterCenter, DocumentVector docVector)
        {
            float[] similarityMeasure = new float[clusterCenter.Count()];

            //error here! OutOfRangeException
            for (int i = 0; i <= clusterCenter.Count - 1; i++)
                similarityMeasure[i] = SimilarityMatrixCalculations.CalculateCosineSimilarity(clusterCenter[i].GroupedDocument[0].VectorSpace, docVector.VectorSpace);

            int index = 0;
            float maxValue = similarityMeasure[0];

            for(int j=0; j< similarityMeasure.Count(); j++)
            {
                if (similarityMeasure[j] > maxValue)
                {
                    maxValue = similarityMeasure[j];
                    index = j;
                }
            }
            return index;
        }

        private static List<Centroid> CalculateMeanPoints(List<Centroid> clusterCenter)
        {
            for(int i =0; i<clusterCenter.Count(); i++)
            {
                if (clusterCenter[i].GroupedDocument.Count() > 0)
                {
                    for(int j =0; j< clusterCenter[i].GroupedDocument[0].VectorSpace.Count(); j++)
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

        private static void InitializeClusterCentroid(out List<Centroid> result, int count)
        {
            Centroid c;
            result = new List<Centroid>();
            for (int i = 0; i < count; i++)
            {
                c = new Centroid();
                c.GroupedDocument = new List<DocumentVector>();
                result.Add(c);
            }
        }

        private static void GenerateRandomNumber(ref HashSet<int> uniqRand, int k, int count)
        {
            Random r = new Random();

            if (k > count)
            {
                do
                {
                    int pos = r.Next(0, count);
                    uniqRand.Add(pos);

                } while (uniqRand.Count != count);
            }
            else
            {
                do
                {
                    int pos = r.Next(0, count);
                    uniqRand.Add(pos);

                } while (uniqRand.Count != k);
            }
        }
    }
}
