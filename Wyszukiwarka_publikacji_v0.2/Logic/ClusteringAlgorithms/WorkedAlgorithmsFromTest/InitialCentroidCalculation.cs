using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest
{
    class InitialCentroidCalculation
    {
        public static List<Centroid> CentroidCalculationsForTestKMeansPP(List<DocumentVector> dataPP, int ClusterNumberPP)
        {
            List<Centroid> centroidListPP = new List<Centroid>();
            List<DocumentVector> dataPPCopy = new List<DocumentVector>(dataPP);
            List<DocumentVector> existingCentroids = new List<DocumentVector>();
            Random randomizerPP = new Random();
            float[] distances = new float[dataPP.Count];
            int indexOfFirstElement = randomizerPP.Next(0, dataPP.Count);// + 1);
            Centroid firstCentroid = new Centroid();
            firstCentroid.GroupedDocument = new List<DocumentVector>();
            firstCentroid.GroupedDocument.Add(dataPP[indexOfFirstElement]);
            centroidListPP.Add(firstCentroid);
            HashSet<Centroid> stringHashSet = new HashSet<Centroid>();

            while (centroidListPP.Count != ClusterNumberPP)
            {
                Centroid newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid = Calculate_Next_KMeansPP_Centroid(firstCentroid, dataPPCopy);
                if (!existingCentroids.Contains(newCentroid.GroupedDocument[0]))
                {
                    existingCentroids.Add(newCentroid.GroupedDocument[0]);
                    centroidListPP.Add(newCentroid);
                    //zmiana1
                    stringHashSet.Add(newCentroid);
                    firstCentroid = newCentroid;
                    dataPPCopy.Remove(newCentroid.GroupedDocument[0]);
                }
                //zmiana2
                else if (existingCentroids.Contains(newCentroid.GroupedDocument[0]) || stringHashSet.Contains(newCentroid))
                {
                    continue;
                }
                //zmiana 3
                centroidListPP = stringHashSet.ToList();
            }
            return centroidListPP;
        }

        public static List<Centroid> CentroidCalculationsForKMeans(List<DocumentVector> data, int ClusterNumber)
        {
            List<Centroid> centroidList = new List<Centroid>();
            Random randomizer = new Random();
            HashSet<int> indexSet = new HashSet<int>();
            int index = 0;

            while (centroidList.Count != ClusterNumber)
            {
                index = randomizer.Next(0, data.Count + 1);
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
            foreach (var doc in centroidList)
            {
                doc.CalculateMeans();
                doc.GroupedDocument.Clear();
            }
            return centroidList;
        }

        private static Centroid Calculate_Next_KMeansPP_Centroid(Centroid firstcentroid, List<DocumentVector> vSpace)
        {
            Centroid next_centroid = new Centroid();
            next_centroid.GroupedDocument = new List<DocumentVector>();
            List<DocumentVector> vSpaceCopy = new List<DocumentVector>(vSpace);
            float[] probabilitiesMatrixSimple = CalculateProbabilityArray(firstcentroid, vSpaceCopy);
            float[] probabilitiesMatrix = new float[probabilitiesMatrixSimple.Length];

            for (var i = 0; i < probabilitiesMatrix.Length; i++)
                probabilitiesMatrix[i] = 0;

            for (var i = 0; i < probabilitiesMatrix.Length; i++)
                for (var j = 0; j < i; j++)
                    probabilitiesMatrix[i] += probabilitiesMatrixSimple[j];

            Random rand = new Random();

            float interval_Value = (float)rand.NextDouble();
            float sum_Of_Probabilies = 0.0F;
            int index_of_min_distance_element = 0;
            for (int i = 0; i < probabilitiesMatrix.Length; i++)
            {
                sum_Of_Probabilies += probabilitiesMatrix[i];
                //here are the problem! - trying to fix;
            }
            for (int j = 0; j < probabilitiesMatrix.Length; j++)
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

            for (int i = 0; i < DistanceQuad.Length; i++)
                DistanceQuad[i] = 0;

            float SumDistanceQuad = 0;
            int previous_index = vSpaceCopy.IndexOf(oldCentroid.GroupedDocument[0]);
            for (int j = 0; j <= vSpaceCopy.Count - 1; j++)
            {
                float[] vector_B = vSpaceCopy[j].VectorSpace;
                for (int k = 0; k <= vSpaceCopy[j].VectorSpace.Length - 1; k++)
                {
                    DistanceQuad[j] += (float)Math.Pow((vector_A[k] - vector_B[k]), 2);
                }
                SumDistanceQuad += DistanceQuad[j];
            }
            for (int j = 0; j <= DistanceQuad.Length - 1; j++)
            {
                DistanceQuad[j] = DistanceQuad[j] / SumDistanceQuad;
            }
            return DistanceQuad;
        }
    }
}
