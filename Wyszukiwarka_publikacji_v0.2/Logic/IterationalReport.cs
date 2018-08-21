using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Tests;

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    static class IterationalReport
    {
        public static void IterationalReportGenerationFunction(int ClusterNumber, int iteration, string algorithm, List<TestCentroid> result, int elementCount, int iterationCount, Stopwatch clusterization_stopwatch)
        {
            int j = iteration;
            switch (algorithm)
            {
                case "KMeans":
                    string KMeans_label_resul_path = @"F:\Magistry files\data\test\testData\" + ClusterNumber.ToString() + "Clusters\\KMeans_label_result" + ClusterNumber.ToString() + "clust" + j.ToString() + ".txt";
                    string K_means_report_path = @"F:\Magistry files\reports\test\KMeans_report" + ClusterNumber.ToString() + "clust" + j.ToString() + ".txt";
                    int[] KMeans_label_matrix = new int[elementCount];
                    KMeans_label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(result, KMeans_label_resul_path);
                    RaportGeneration.VoidRaportGenerationFunction(algorithm, result, ClusterNumber, iterationCount, clusterization_stopwatch, K_means_report_path);
                    break;
                case "KmeansPP":
                    string KMeansPP_label_resul_path = @"F:\Magistry files\data\test\testData\" + ClusterNumber.ToString() + "Clusters\\KMeansPP_label_result" + ClusterNumber.ToString() + "clust" + j.ToString() + ".txt";
                    string K_meansPP_report_path = @"F:\Magistry files\reports\test\KMeansPP_report" + ClusterNumber.ToString()+"clust"+j.ToString()+".txt";
                    int[] KMeansPP_label_matrix = new int[elementCount];
                    KMeansPP_label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(result, KMeansPP_label_resul_path);
                    RaportGeneration.VoidRaportGenerationFunction(algorithm, result, ClusterNumber, iterationCount, clusterization_stopwatch, K_meansPP_report_path);
                    break;
                /*
                case "FuzzyKMeans":
                    string FuzzyKMeans_label_resul_path = @"F:\Magistry files\data\test\testData\" + ClusterNumber.ToString()+"Clusters\\FuzzyKMeans_label_result"+ ClusterNumber.ToString()+"clust"+j.ToString()+".txt";
                    string Fuzzy_K_means_report_path = @"F:\Magistry files\reports\test\Fuzzy_KMeans_report" + ClusterNumber.ToString() +"clust"+j.ToString()+".txt";
                    int[] FuzzyKMeans_label_matrix = new int[elementCount];
                    FuzzyKMeans_label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(result, FuzzyKMeans_label_resul_path);
                    RaportGeneration.VoidRaportGenerationFunction(algorithm, result, ClusterNumber, iterationCount, clusterization_stopwatch, Fuzzy_K_means_report_path);
                    break;
                    */
            }
        }
    }
}
