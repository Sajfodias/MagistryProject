using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms
{
    class KMeanspp
    {
        public class ClusterPoint
        {
            private Dictionary<DocumentVector, List<DocumentVector>> clusterPoints = new Dictionary<DocumentVector, List<DocumentVector>>();

            public Dictionary<DocumentVector, List<DocumentVector>> ClustersPoint
            {
                get { return clusterPoints; }
                set { clusterPoints = value; }
            }
        }
/*
        public static ClusterPoint GetKMeansPP(List<DocumentVector> allPoints, int k)
        {
            List<DocumentVector> seedPoints = AdditionalFunctionalityForSecondKMeansppimplementation.GetSeedPoints2v(allPoints, k);

            //ClusterPoint result = KMeansCalculation(allPoints, seedPoints, k);

            //return result;
        }
*/
        private static DocumentVector KMeansCalculation(List<DocumentVector> docList, List<DocumentVector> seedPoints, int k)
        {
            DocumentVector cluster = new DocumentVector();
            float[] Distances = new float[k];
            float minD = float.MaxValue;
            List<DocumentVector> sameDPoint = new List<DocumentVector>();
            bool exit = true;

            foreach(DocumentVector vectror in docList)
            {
                foreach(DocumentVector seedPoint in seedPoints)
                {
                    float dist = KMeansPlus.GetEucliedeanDistance(vectror, seedPoint); 
                    if (dist < minD)
                    {
                        sameDPoint.Clear();
                        minD = dist;
                        sameDPoint.Add(seedPoint);
                    }
                    if (dist == minD)
                    {
                        if (!sameDPoint.Contains(seedPoint))
                            sameDPoint.Add(seedPoint);
                    }
                }

                DocumentVector keyPoint;
                if (sameDPoint.Count > 1)
                {
                    int index = KMeansPlus.GetRandNumCrypto(0, sameDPoint.Count);
                    keyPoint = sameDPoint[index];
                }
                else
                    keyPoint = sameDPoint[0];
                /*
                //Assign ensemble point to correct central point cluster
                if (!cluster.ClustersPoint.ContainsKey(keyPoint))  //New
                {
                    List<Point> newCluster = new List<Point>();
                    newCluster.Add(p);
                    cluster.PC.Add(keyPoint, newCluster);
                }
                else
                {   //Existing cluster centre   
                    cluster.PC[keyPoint].Add(p);
                }
                */
                //Reset
                sameDPoint.Clear();
                minD = float.MaxValue;
            }

            if (exit)
                return cluster;
            else
                return KMeansCalculation(docList, seedPoints, k);
        }
    }
}
