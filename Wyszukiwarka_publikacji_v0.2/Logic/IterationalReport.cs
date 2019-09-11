using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Wyszukiwarka_publikacji_v0._2.Tests;

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    static class IterationalReport
    {
        public static void IterationalReportGenerationFunction(int ClusterNumber, int iteration, string algorithm, List<TestCentroid> result, int elementCount, int iterationCount, Stopwatch clusterization_stopwatch)
        {
            var ReportsTestDataFileDirectoryKMeans = ConfigurationManager.AppSettings["ReportsTestDataFileDirectoryKMeans"].ToString();
            var ReportsTestDataFileDirectoryKMeansPP = ConfigurationManager.AppSettings["ReportsTestDataFileDirectoryKMeansPP"].ToString();

            int j = iteration;
            switch (algorithm)
            {
                case "KMeans":
                    string KMeans_label_resul_path = Path.Combine(ReportsTestDataFileDirectoryKMeans,ClusterNumber.ToString(),"Clusters\\KMeans_label_result",ClusterNumber.ToString(),"clust",j.ToString(),".txt");
                    if (Directory.Exists(Path.GetDirectoryName(KMeans_label_resul_path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(KMeans_label_resul_path));
                    }
                    string K_means_report_path = Path.Combine(ReportsTestDataFileDirectoryKMeans,ClusterNumber.ToString(),"clust",j.ToString(),".txt");
                    if (Directory.Exists(Path.GetDirectoryName(K_means_report_path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(K_means_report_path));
                    }
                    int[] KMeans_label_matrix = new int[elementCount];
                    KMeans_label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(result, KMeans_label_resul_path);
                    RaportGeneration.VoidRaportGenerationFunction(algorithm, result, ClusterNumber, iterationCount, clusterization_stopwatch, K_means_report_path);
                    break;
                case "KmeansPP":
                    string KMeansPP_label_resul_path = Path.Combine(ReportsTestDataFileDirectoryKMeansPP,ClusterNumber.ToString(),"Clusters\\KMeansPP_label_result",ClusterNumber.ToString(),"clust",j.ToString(),".txt");
                    if (Directory.Exists(Path.GetDirectoryName(KMeansPP_label_resul_path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(KMeansPP_label_resul_path));
                    }
                    string K_meansPP_report_path = Path.Combine(ReportsTestDataFileDirectoryKMeansPP,ClusterNumber.ToString(),"clust",j.ToString(),".txt");
                    if (Directory.Exists(Path.GetDirectoryName(K_meansPP_report_path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(K_meansPP_report_path));
                    }
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
