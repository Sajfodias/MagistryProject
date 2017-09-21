// This code is the adaptation from the free code developed by  James McCaffrey (jamccaff@microsoft.com, https://jamesmccaffrey.wordpress.com/)
// Projects of James McCaffrey you can find here https://msdn.microsoft.com/en-us/magazine/mt185575.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class KMeansPlus
    {
        public static int[] Cluster(double[][] rawData, int numClusters, int seed)
        {
            double[][] data = Normalized(rawData);

            bool changed = true;
            bool success = true;

            double[][] means = InitMeans(numClusters, data, seed);

            int[] clustering = new int[data.Length];

            int maxCount = data.Length * 10;
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                changed = UpdateClustering(data, clustering, means);
                success = UpdateMeans(data, clustering, means);
                ++ct;
            }

            return clustering;
        }

        private static bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
        {
            int numClusters = means.Length;
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false;

            for (int z = 0; z < means.Length; z++)
                for (int j = 0; j < means[z].Length; ++j)
                    means[z][j] = 0.0;

            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                for (int j = 0; j < data[i].Length; ++j)
                    means[cluster][j] += data[i][j]; 
            }

            for (int k1 = 0; k1 < means.Length; k1++)
                for (int j1 = 0; j1 < means[k1].Length; j1++)
                    means[k1][j1] /= clusterCounts[k1]; 
            return true;
        }

        private static bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
        {
            int number_of_Clusters = means.Length;
            bool changed = false;

            int[] newClustering = new int[clustering.Length];
            Array.Copy(clustering, newClustering, clustering.Length);

            double[] distance = new double[number_of_Clusters];

            for(int i=0; i<data.Length; i++)
            {
                for (int k = 0; k < number_of_Clusters; k++)
                    distance[k] = EuclideanDistance(data[i], means[k]); //can change to ManhattanDistance()

                int newClusterID = MinIndex(distance);

                if (newClusterID != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterID;
                }
            }
            if (changed == false)
                return false;

            int[] clusterCounts = new int[number_of_Clusters];

            for (int j = 0; j < data.Length; j++)
            {
                int cluster = newClustering[j];
                clusterCounts[cluster]++;
            }

            for (int z = 0; z < number_of_Clusters; z++)
                if (clusterCounts[z] == 0)
                    return false;

            Array.Copy(newClustering, clustering, newClustering.Length);
            return true;
        }

        private static double[][] InitMeans(int numClusters, double[][] data, int seed)
        {
            double[][] means = CreateMatrix(numClusters, data[0].Length);

            List<int> used_means = new List<int>();
            Random generator = new Random(seed);
            int index = generator.Next(0, data.Length);
            Array.Copy(data[index], means[0], data[index].Length);
            used_means.Add(index);

            for(int i=1; i< numClusters; i++)
            {
                double[] squaredDistance = new double[data.Length];
                int newMean = -1;

                for(int y=0; y< data.Length; y++)
                {
                    if (used_means.Contains(y)) continue;

                    double[] dist = new double[i];
                    for (int j = 0; j < i; ++j)
                        dist[j] = EuclideanDistance(data[i], means[i]);

                    int minIndex = MinIndex(dist);
                    squaredDistance[i] = dist[minIndex] * dist[minIndex];
                }

                double punkt = generator.NextDouble();
                double distanceSquaredSum = 0.0;

                for (int k = 0; k < squaredDistance.Length; k++)
                    distanceSquaredSum += squaredDistance[k];

                double cumulativeProbability = 0.0;
                int pointInSquarDist = 0;
                int sanityDistanceCount = 0;

                while(sanityDistanceCount < data.Length * 2)
                {
                    cumulativeProbability += squaredDistance[pointInSquarDist] / distanceSquaredSum;
                    if(cumulativeProbability >= pointInSquarDist && used_means.Contains(pointInSquarDist) == false)
                    {
                        newMean = pointInSquarDist;
                        used_means.Add(newMean);
                        break;
                    }
                    pointInSquarDist++;
                    if (pointInSquarDist >= squaredDistance.Length) pointInSquarDist = 0;
                    sanityDistanceCount++;
                }
                Array.Copy(data[newMean], means[i], data[newMean].Length);
            }
            return means;
        }

        private static double EuclideanDistance(double[] xd, double[] yd)
        {
            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < xd.Length; j++)
                sumSquaredDiffs += Math.Pow((xd[j] - yd[j]), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private static double ManhattanDistance(double[] xd, double[] yd)
        {
            double sumSquareDiffs = 0.0;
            for (int k = 0; k < xd.Length; k++)
                sumSquareDiffs += Math.Abs(xd[k]) + Math.Abs(yd[k]);
            return sumSquareDiffs;
        }

        private static int MinIndex(double[] dist)
        {
            int minIndex = 0;
            double smallDist = dist[0];
            for (int k = 0; k < dist.Length; ++k)
            {
                if (dist[k] < smallDist)
                {
                    smallDist = dist[k];
                    minIndex = k;
                }
            }
            return minIndex;
        }

        private static double[][] CreateMatrix(int rows, int cols)
        {
            double[][] computedMatrix = new double[rows][];
            for (int i = 0; i < rows; ++i)
                computedMatrix[i] = new double[cols];
            return computedMatrix;
        }

        private static double[][] Normalized(double[][] rawData)
        {
            double[][] result = new double[rawData.Length][];
            for (int i = 0; i < rawData.Length; ++i)
            {
                result[i] = new double[rawData[i].Length];
                Array.Copy(rawData[i], result[i], rawData[i].Length);
            }

            for (int j = 0; j < result[0].Length; ++j)
            {
                double colSum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    colSum += result[i][j];
                double mean = colSum / result.Length;
                double sum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    sum += (result[i][j] - mean) * (result[i][j] - mean);
                double sd = sum / result.Length;
                for (int i = 0; i < result.Length; ++i)
                    result[i][j] = (result[i][j] - mean) / sd;
            }
            return result;
        }
    }
}
