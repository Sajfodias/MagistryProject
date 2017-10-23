using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms
{
    public struct DocDetails
    {
        private DocumentVector _seedpoint;
        private float[] _Weights;
        private float _Sum;
        private float _minD;

        public DocumentVector SeedDocVect
        {
            get { return _seedpoint; }
            set { _seedpoint = value; }
        }

        public float[] Weights
        {
            get { return _Weights; }
            set { _Weights = value; }
        }

        public float Sum
        {
            get { return _Sum; }
            set { _Sum = value; }
        }

        public float MinD
        {
            get { return _minD; }
            set { _minD = value; }
        }
    }

    class AdditionalFunctionalityForSecondKMeansppimplementation
    {
        public static List<DocumentVector> GetSeedPoints2v(List<DocumentVector> docCollection, int k)
        {
            List<DocumentVector> seedPoints = new List<DocumentVector>(k);
            DocDetails docDetails;
            List<DocDetails> docDetailsList = new List<DocDetails>();
            int index = 0;

            int firstIndex = KMeansPlus.GenerateRandomNumber(0, docCollection.Count);
            DocumentVector FirstPoint = docCollection[firstIndex];
            seedPoints.Add(FirstPoint);

            for(int i = 0; i<k-1; i++)
            {
                if(seedPoints.Count >= 2)
                {
                    DocDetails minpd = GetMinimalPointDistance(docDetailsList);
                    index = GetWeightedProbDist(minpd.Weights, minpd.Sum);
                    DocumentVector SubsequentPoint = docCollection[index];

                    docDetails = new DocDetails();
                    docDetails = GetAllDetails(docCollection, SubsequentPoint, docDetails);
                    docDetailsList.Add(docDetails);
                }
                else
                {
                    docDetails = new DocDetails();
                    docDetails = GetAllDetails(docCollection, FirstPoint, docDetails);
                    docDetailsList.Add(docDetails);
                    index = GetWeightedProbDist(docDetails.Weights, docDetails.Sum);
                    DocumentVector SecondPoint = docCollection[index];
                    seedPoints.Add(SecondPoint);

                    docDetails = new DocDetails();
                    docDetails = GetAllDetails(docCollection, SecondPoint, docDetails);
                    docDetailsList.Add(docDetails);
                }
            }
            return seedPoints;
        }

        private static DocDetails GetAllDetails(List<DocumentVector> docCollection, DocumentVector seedPoint, DocDetails docDetails)
        {
            float[] Weights = new float[docCollection.Count];
            float minD = float.MaxValue;
            float Sum = 0;
            int i = 0;

            foreach (DocumentVector point in docCollection)
            {
                if (point == seedPoint) //Delta is 0
                    continue;

                Weights[i] = KMeansPlus.GetEucliedeanDistance(point, seedPoint);
                Sum += Weights[i];
                if (Weights[i] < minD)
                    minD = Weights[i];
                i++;
            }

            docDetails.SeedDocVect = seedPoint;
            docDetails.Weights = Weights;
            docDetails.Sum = Sum;
            docDetails.MinD = minD;

            return docDetails;
        }

        public static DocDetails GetMinimalPointDistance(List<DocDetails> pds)
        {
            float minValue = float.MinValue;
            List<DocDetails> sameDistValues = new List<DocDetails>();

            foreach(DocDetails pd in pds)
            {
                if (pd.MinD < minValue)
                {
                    sameDistValues.Clear();
                    minValue = pd.MinD;
                    sameDistValues.Add(pd);
                }
                if(pd.MinD == minValue)
                {
                    if (!sameDistValues.Contains(pd))
                        sameDistValues.Add(pd);
                }
            }
            if (sameDistValues.Count > 1)
                return sameDistValues[KMeansPlus.GetRandNumCrypto(0, sameDistValues.Count)];
            else
                return sameDistValues[0];
        }

        private static int GetWeightedProbDist(float[] w, float s)
        {
            float p = KMeansPlus.GetRandNumCrypto();
            float q = 0;
            int i = -1;

            while (q < p)
            {
                i++;
                q += (w[i] / s);
            }
            return i;
        }

    }
}
