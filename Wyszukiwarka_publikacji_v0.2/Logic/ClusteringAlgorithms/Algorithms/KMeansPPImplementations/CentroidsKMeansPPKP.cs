using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations
{
    public class CentroidsKMeansPPKP: DetailedDocumentVector
    {
        protected List<DocumentVector> assignedDocuments;
        protected int dimensions;
        public CentroidsKMeansPPKP(int size)
        {
            dimensions = size;
            //tDF = new double[size];
            //iDF = new double[size];
            tfIDF = new float[size];
            AssignedDocuments = new List<DocumentVector>();
        }

        public List<DocumentVector> AssignedDocuments
        {
            get { return assignedDocuments; }
            set { assignedDocuments = value; }
        }

        internal void Update(bool shouldClear)
        {
            tfIDF = new float[dimensions];
            //tDF = new double[dimensions];
            //iDF = new double[dimensions];
            for (var i = 0; i < dimensions; i++)
            {
                tfIDF[i] = 0;
                //tDF[i] = 0.0;
                //iDF[i] = 0.0;

            }
            foreach (var doc in assignedDocuments)
            {
                for (var i = 0; i < dimensions; i++)
                {
                    tfIDF[i] += doc.VectorSpace[i];
                    //tDF[i] += doc.TDF[i];
                    //iDF[i] += doc.IDF[i];
                }
            }
            for (var i = 0; i < dimensions; i++)
            {
                //tDF[i] /= (double)assignedDocuments.Count;
                //iDF[i] /= (double)assignedDocuments.Count;
                tfIDF[i] /= (float)assignedDocuments.Count;
            }

            //zanegować lub odwrócić warunki.
            if (!shouldClear)
                AssignedDocuments.Clear();
        }
    }
}
