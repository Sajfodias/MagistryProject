using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class KMeansPPTest
    {
        public static List<TestCentroid> CentroidCalculationsForTestKMeansPP(List<DocumentVectorTest> dataPP, int ClusterNumberPP)
        {
            List<TestCentroid> centroidListPP = new List<TestCentroid>();
            List<DocumentVectorTest> dataPPCopy = new List<DocumentVectorTest>(dataPP);
            List<DocumentVectorTest> existingCentroids = new List<DocumentVectorTest>();
            Random randomizerPP = new Random();
            float[] distances = new float[dataPP.Count];
            int indexOfFirstElement = randomizerPP.Next(0, dataPP.Count);// + 1);
            TestCentroid firstCentroid = new TestCentroid();
            firstCentroid.GroupedDocument = new List<DocumentVectorTest>();
            firstCentroid.GroupedDocument.Add(dataPP[indexOfFirstElement]);
            centroidListPP.Add(firstCentroid);
            HashSet<TestCentroid> stringHashSet = new HashSet<TestCentroid>();

            while (centroidListPP.Count != ClusterNumberPP)
            {
                TestCentroid newCentroid = new TestCentroid();
                newCentroid.GroupedDocument = new List<DocumentVectorTest>();
                newCentroid = Calculate_Next_Centroid_Test(firstCentroid, dataPPCopy);
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

        private static TestCentroid Calculate_Next_Centroid_Test(TestCentroid firstcentroid, List<DocumentVectorTest> vSpace)
        {
            TestCentroid next_centroid = new TestCentroid();
            next_centroid.GroupedDocument = new List<DocumentVectorTest>();
            List<DocumentVectorTest> vSpaceCopy = new List<DocumentVectorTest>(vSpace);
            float[] probabilitiesMatrixSimple = CalculateProbabilityArray_Test(firstcentroid, vSpaceCopy);
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

        public static float[] CalculateProbabilityArray_Test(TestCentroid oldCentroid, List<DocumentVectorTest> vSpace)
        {
            List<DocumentVectorTest> vSpaceCopy = new List<DocumentVectorTest>(vSpace);
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
