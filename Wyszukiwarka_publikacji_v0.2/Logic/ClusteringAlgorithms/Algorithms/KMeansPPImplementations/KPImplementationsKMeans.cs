using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations
{
    public class KPImplementationsKMeans
    {
        public List<CentroidsKMeansPPKP> clusters;
        public List<DocumentVector> DocCollection;
        public bool documentMoved = true;
        public int dimensions;

        public void SetDocumentData(List<DocumentVector> documents)
        {
            DocCollection = documents;
        }

        public List<DocumentVector> GetDocumentData()
        {
            return DocCollection;
        }

        public KPImplementationsKMeans(int noClusters, int maxIterations, int dimensions)
        {
            clusters = new List<CentroidsKMeansPPKP>();
            DocCollection = new List<DocumentVector>();
            for (var i = 0; i < noClusters; i++)
                clusters.Add(FillRandomCluster(dimensions));
        }

        protected CentroidsKMeansPPKP FillRandomCluster(int dimensions)
        {
            this.dimensions = dimensions;
            CentroidsKMeansPPKP cl = new CentroidsKMeansPPKP(dimensions);
            Random random = new Random();
            for (var i = 0; i < dimensions; i++)
            {
                cl.TFIDF[i] = (float)random.Next(0, Int32.MaxValue) / (float)Int32.MaxValue;
            }
            return cl;
        }

        protected CentroidsKMeansPPKP FindNearestClusterCenter(DocumentVector doc)
        {
            var minDistance = (double)dimensions;
            CentroidsKMeansPPKP bestClusterCenter = clusters.First();
            foreach (var cluster in clusters)
            {
                var distance = cluster.ComputeTFIDFDistance(doc);
                if (distance < minDistance)
                {
                    bestClusterCenter = cluster;
                    minDistance = distance;
                }
            }
            return bestClusterCenter;
        }


        protected void Iteration(int current, int max)
        {
            documentMoved = false;
            foreach (var doc in DocCollection)
            {
                CentroidsKMeansPPKP cluster = FindNearestClusterCenter(doc);
                cluster.AssignedDocuments.Add(doc);
            }
            if (current == max - 1)
                foreach (var cluster in clusters)
                {
                    cluster.Update(true);
                }
        }

        public void RunAlgorithm(int maxIterations)
        {
            for (var i = 0; i < maxIterations; i++)
                Iteration(i, maxIterations);
        }
    }
}
