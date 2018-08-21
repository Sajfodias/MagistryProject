using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Wyszukiwarka_publikacji_v0._2.Logic;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms;
using System.ComponentModel;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions;
using Wyszukiwarka_publikacji_v0._2.Tests;
using System.Globalization;

namespace Wyszukiwarka_publikacji_v0._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Downloader.AllocConsole();
            //var stopwatch = Stopwatch.StartNew();
            Task.Factory.StartNew(()=>BibtexParser.LoadBibtexFile());
            //stopwatch.Stop();
            //Console.WriteLine("Processing time " + stopwatch.Elapsed.Minutes + ":" + stopwatch.Elapsed.Milliseconds);
            //Console.ReadKey();
        }

        private void rtfReader(object sender, RoutedEventArgs e)
        {   
            RTFReader form = new RTFReader();
            form.Show();
        }

        private void DictionaryPreparation(object sender, RoutedEventArgs e)
        {
            string path = @"F:\Magistry files\csv_files\Allowed_term_dictionary.csv";
            string dictionary_text = File.ReadAllText(path);
            string[] allowed_dictionary = dictionary_text.Trim(' ').Split(',', '\n', '\r');
            List<string> result = new List<string>();

            for (int i = 0; i <= allowed_dictionary.Length - 1; i++)
                if (allowed_dictionary[i] != "")
                    result.Add(allowed_dictionary[i].TrimStart());
            string result_text = String.Join(",", result);
            File.WriteAllText(path, result_text);
        }

        private void KMeansPP(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string algorithm = " k-means++;";
            string PPKMeans_label_resul_path = @"F:\Magistry files\data\PPKMeans_label_result1.txt";
            string PPK_means_report_path = @"F:\Magistry files\reports\PPKMeans_report1.txt";
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            Dictionary<int, string> docCollectionDictionary = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoadingToDictionary();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex1 = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTermToDictionary(docCollectionDictionary, termCollection);
            List<DocumentVector> vSpace1 = VectorSpaceModel.DocumentCollectionProcessingDictionary(docCollectionDictionary);
            int totalIteration = 500;
            int clusterNumber = 5;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<Centroid> firstCentroidList = new List<Centroid>();
            firstCentroidList = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.InitialCentroidCalculation.CentroidCalculationsForTestKMeansPP(vSpace1, clusterNumber);
            List<Centroid> resultSet = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.KMeans.KMeansClustering(vSpace1, clusterNumber, totalIteration, firstCentroidList);
            clusterization_stopwatch.Stop();
            int[] PPKMeans_label_matrix = new int[vSpace1.Count];
            PPKMeans_label_matrix = Tests.Label_Matrix.ReleaseVersion_Label_Matrix_Extractions(resultSet, PPKMeans_label_resul_path);
            message = RaportGeneration.ReleaseRaportGenerationFunction(resultSet, clusterNumber, totalIteration, clusterization_stopwatch, PPK_means_report_path,algorithm);
            clustResultTxtBox.AppendText(message);
            invokeFilesToVisualizationGenerator(resultSet);
        }

        private void KMeans(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string algorithm = " k-means;";
            string PKMeans_label_resul_path = @"F:\Magistry files\data\PKMeans_label_result1.txt";
            string K_means_report_path = @"F:\Magistry files\reports\PKMeans_report1.txt";
            #region OldDataGeneration
            /*
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            */
            #endregion
            Dictionary<int, string> docCollectionDictionary = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoadingToDictionary();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTermToDictionary(docCollectionDictionary, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessingDictionary(docCollectionDictionary);
            int totalIteration = 500;
            int clusterNumber = 5;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<Centroid> firstCentroidList = new List<Centroid>();
            #region OldClusteringAlgorithm
            //firstCentroidList = CentroidCalculationClass.CentroidCalculationsForKMeans(vSpace, clusterNumber);
            //List<Centroid> resultSet = Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations.MyKmeansPPInterpritationcs.NewKMeansClusterization(clusterNumber, docCollection, totalIteration, vSpace, wordIndex, firstCentroidList);
            #endregion
            firstCentroidList = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.KMeans.CentroidCalculationsForKMeans(vSpace, clusterNumber);
            List<Centroid> resultSet = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.KMeans.KMeansClustering(vSpace, clusterNumber, totalIteration, firstCentroidList);
            clusterization_stopwatch.Stop();
            int[] PKMeans_label_matrix = new int[vSpace.Count];
            PKMeans_label_matrix = Tests.Label_Matrix.ReleaseVersion_Label_Matrix_Extractions(resultSet, PKMeans_label_resul_path);
            #region tests_metrics
            /*
            List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            List<List<string>> ClassCollection = Tests.DocClasses.ListOfClasses();
            var distance = Tests.InterclusterDistances.d_centroids(resultSet);
            var min_centroid_distances = Tests.InterclusterDistances.d_min_centroids(resultSet);
            var max_intracluster_d = Tests.IntraclusterDistances.d_max(resultSet);
            var min_intracluster_d = Tests.IntraclusterDistances.d_min(resultSet);
            var median_intracluster_d = Tests.IntraclusterDistances.d_sr(resultSet);
            var Recall_result = Tests.Recall.Recall_Calculating(resultSet, docs);
            var Precision_result = Tests.Precision.Precision_Calculating(resultSet, docs);
            var Purity = Tests.Purity.Purity_Calculating(resultSet, ClassCollection, vSpace);
            var Fmeasure = Tests.F1Measure.F1_Measure_Calculating(resultSet, ClassCollection);
            var GMeasure = Tests.F1Measure.G_Measure_Calculating(resultSet, ClassCollection);
            var NMI = Tests.NormilizedMutualInformation.NMI_Calculating(resultSet, ClassCollection, vSpace);
            var Entropy = Tests.Entropy.Enthropy_Calculating(resultSet, ClassCollection);
            */
            #endregion
            message = RaportGeneration.ReleaseRaportGenerationFunction(resultSet, clusterNumber, totalIteration, clusterization_stopwatch, K_means_report_path,algorithm);
            clustResultTxtBox.AppendText(message);
            invokeFilesToVisualizationGenerator(resultSet);
        }

        private void FuzzyKMeans_Click(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string algorithm = " Fuzzy c-Means;";
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            Dictionary<int, string> docCollectionDictionary = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoadingToDictionary();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTermToDictionary(docCollectionDictionary, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessingDictionary(docCollectionDictionary);
            string Fuzzy_K_means_clusterization_result = @"F:\Magistry files\Fuzzy_KMeans_result.txt";
            string Fuzzy_K_means_label_result = @"F:\Magistry files\FCM_label_result.txt";
            string Fuzzy_K_means_report_path = @"F:\Magistry files\reports\FCM_report.txt";
            float fuzziness = 0.5f;
            float epsilon = 0.003f;
            int clusterNumber = 5;
            int totalIteration = 0;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<Centroid> resultSet = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.FuzzyCMeans.CreateClusterSet(clusterNumber);
            float[,] Result_fcm;
            /*
            Result_fcm = FuzzyKMeans.Fcm(vSpace, clusterNumber, epsilon, fuzziness, termCollection);
            FuzzyKMeans.WriteSimilarityArrayToFile(Result_fcm, Fuzzy_K_means_clusterization_result);
            resultSet = FuzzyKMeans.AssignDocsToClusters(Result_fcm, clusterNumber,vSpace);
            FuzzyKMeans.Show_clusters(vSpace, Result_fcm, clusterNumber);
            */
            var result = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.FuzzyCMeans.Fcm(vSpace, clusterNumber, epsilon, fuzziness);
            Result_fcm = result.Item1;
            totalIteration = result.Item2;
            Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.FuzzyCMeans.WriteSimilarityArrayToFile(Result_fcm, Fuzzy_K_means_clusterization_result);
            var assignedResult = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.FuzzyCMeans.AssignDocsToClusters(Result_fcm, clusterNumber, vSpace, resultSet);
            clusterization_stopwatch.Stop();
            int[] FuzzyKMeans_label_matrix = new int[vSpace.Count];
            int[] FuzzyKMeans_label_matrix1 = assignedResult.Item1;
            resultSet = assignedResult.Item2;
            FuzzyKMeans_label_matrix = Tests.Label_Matrix.ReleaseVersion_Label_Matrix_Extractions(resultSet, Fuzzy_K_means_label_result);
            message = RaportGeneration.ReleaseRaportGenerationFunction(resultSet, clusterNumber, totalIteration, clusterization_stopwatch, Fuzzy_K_means_report_path,algorithm);
            clustResultTxtBox.AppendText(message);
            invokeFilesToVisualizationGenerator(resultSet);
        }

        private void Gravitational_Click(object sender, RoutedEventArgs e)
        {
            #region gravitational_old_working_code
            /*
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int M = 1000000;
            float G = -1.28171817154F; //G=1*e-4 according to 3.2.2  in article
            float deltaG = 0.01F;
            float epsilon = -3.28171817154F;//epsilon=1*e-6 according to 3.2.2 in article or 10^(-4)= 0.0001F;
            //float epsilon = 0.6F;
            float alpha = 0.06F; 
            var result1 = Logic.ClusteringAlgorithms.Algorithms.GravitationalClusteringAlgorithm.Gravitational(vSpace, G, deltaG, M, epsilon);
            //var result2 = GravitationalClusteringAlgorithm.GetClusters(result1, alpha, vSpace);
            List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            List<List<string>> ClassCollection = Tests.DocClasses.ListOfClasses();
            var distance = Tests.InterclusterDistances.d_centroids(result1);
            var min_centroid_distances = Tests.InterclusterDistances.d_min_centroids(result1);
            var max_intracluster_d = Tests.IntraclusterDistances.d_max(result1);
            var min_intracluster_d = Tests.IntraclusterDistances.d_min(result1);
            var median_intracluster_d = Tests.IntraclusterDistances.d_sr(result1);
            var Recall_result = Tests.Recall.Recall_Calculating(result1, docs);
            var Precision_result = Tests.Precision.Precision_Calculating(result1, docs);
            var Purity = Tests.Purity.Purity_Calculating(result1, ClassCollection, vSpace);
            var Fmeasure = Tests.F1Measure.F1_Measure_Calculating(result1, ClassCollection);
            var GMeasure = Tests.F1Measure.G_Measure_Calculating(result1, ClassCollection);
            var NMI = Tests.NormilizedMutualInformation.NMI_Calculating(result1, ClassCollection, vSpace);
            */
            #endregion

            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string algorithm = " Gravitational clustering algorithm;";
            Dictionary<int, string> docCollectionDictionary = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoadingToDictionary();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTermToDictionary(docCollectionDictionary, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessingDictionary(docCollectionDictionary);
            int M = 500;
            //float G = 7 * (float)Math.Pow(10, (-6));
            //float G = -1.28171817154F; //G=1*e-4 according to 3.2.2  in article
            float G = 6.67408313131313131F * (float)Math.Pow(10, (-6));
            float deltaG = 0.001F;
            //float epsilon = -3.28171817154F;//epsilon=1*e-6 according to 3.2.2 in article or 10^(-4)= 0.0001F;        
            float epsilon = 0.1F;
            float alpha = 0.06F;
            int clusterNumber = 6;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            M = Convert.ToInt32(txtboxIterationCount.Text);
            string gravitational_label_resul_path = @"F:\Magistry files\data\Gravitational_label_result5.txt";
            string Gravitational_report_path = @"F:\Magistry files\reports\Gravitational_report5.txt";
            List<Centroid> result = new List<Centroid>(vSpace.Count);
            var results = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.GravitationalClusteringAlgorithm.GravitationalAlgorithm(vSpace, G, deltaG, M, epsilon);
            var get_Clusters = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.GravitationalClusteringAlgorithm.GetClusters(results, alpha, vSpace);
            List<Centroid> resultSet = Logic.ClusteringAlgorithms.WorkedAlgorithmsFromTest.GravitationalClusteringAlgorithm.RemoveSameElementsFromClusters(get_Clusters);
            int[] label_matrix = Tests.Label_Matrix.ReleaseVersion_Label_Matrix_Extractions(get_Clusters, gravitational_label_resul_path);
            clusterization_stopwatch.Stop();
            message = RaportGeneration.ReleaseRaportGenerationFunction(get_Clusters, get_Clusters.Count, M, clusterization_stopwatch, Gravitational_report_path, algorithm);
            clustResultTxtBox.AppendText(message);
            invokeFilesToVisualizationGenerator(resultSet);
        }

        #region AHC
        /*
        private void AHC_Click(object sender, RoutedEventArgs e)
        {
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int iterationCount = 100;
            var result = Primitive_Clustering_Hierarhical_Alg.Hierarchical_Clusterization(vSpace, iterationCount);
            List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            List<List<string>> ClassCollection = Tests.DocClasses.ListOfClasses();
            var distance = Tests.InterclusterDistances.d_centroids(result);
            var min_centroid_distances = Tests.InterclusterDistances.d_min_centroids(result);
            var max_intracluster_d = Tests.IntraclusterDistances.d_max(result);
            var min_intracluster_d = Tests.IntraclusterDistances.d_min(result);
            var median_intracluster_d = Tests.IntraclusterDistances.d_sr(result);
            var Recall_result = Tests.Recall.Recall_Calculating(result, docs);
            var Precision_result = Tests.Precision.Precision_Calculating(result, docs);
            var Purity = Tests.Purity.Purity_Calculating(result, ClassCollection, vSpace);
            var Fmeasure = Tests.F1Measure.F1_Measure_Calculating(result, ClassCollection);
            var GMeasure = Tests.F1Measure.G_Measure_Calculating(result, ClassCollection);
            var NMI = Tests.NormilizedMutualInformation.NMI_Calculating(result, ClassCollection, vSpace);
        }
        */
        #endregion

        private void Kmeans_test(object sender, RoutedEventArgs e)
        {

            #region usual_KMeans_alg_invoke
            
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            string KMeansPP_label_resul_path = @"F:\Magistry files\data\test\testData\20Clusters\KMeans_label_result20clust5.txt";
            string K_means_report_path = @"F:\Magistry files\reports\KMeans_report20clust5.txt";
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            int iterationCount = 500;
            iterationCount = Convert.ToInt32(txtboxIterationCount.Text);
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            List<TestCentroid> firstCentroidList = new List<TestCentroid>();
            firstCentroidList = Test_KMeans.CentroidCalculationsForKMeans(normilized_vSpace, clusterNumber);
            List<TestCentroid> resultSet = Tests.NewTestKMeansClustering.Cluster(vSpace, clusterNumber, iterationCount);
            int[] KMeans_label_matrix = new int[vSpace.Count];
            KMeans_label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(resultSet, KMeansPP_label_resul_path);
            clusterization_stopwatch.Stop();
            message = RaportGeneration.RaportGenerationFunction(resultSet, clusterNumber, iterationCount, clusterization_stopwatch, K_means_report_path);
            clustResultTxtBox.AppendText(message);
            
            #endregion

            #region Iterational_test_Kmeans
            /*
            int iteration = 5;
            string algorithmFlag = "KMeans";
            int clusterNumbers = Convert.ToInt32(txtboxClusterNumber.Text);
            int algiterationCount = 500;
            algiterationCount = Convert.ToInt32(txtboxIterationCount.Text);
            string test_path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest>vSpaces = TestDocVectorCreator.CreatingTheDocVectorCollection(test_path_data);
            List<DocumentVectorTest> normilized_vSpaces = TestDocVectorCreator.NormalizationDocumentVectors(vSpaces);
            List<TestCentroid> firstCentroidLists = new List<TestCentroid>();

            for (int j = 5; j <= clusterNumbers;)
            {
                for (int i = 1; i <= iteration; i++)
                {
                    clustResultTxtBox.Document.Blocks.Clear();
                    var clusterization_stopwatchs = Stopwatch.StartNew();
                    firstCentroidLists = Test_KMeans.CentroidCalculationsForKMeans(normilized_vSpaces, j);
                    List<TestCentroid> IterationresultSets = Tests.NewTestKMeansClustering.Cluster(vSpaces, j, algiterationCount);
                    clusterization_stopwatchs.Stop();
                    Logic.IterationalReport.IterationalReportGenerationFunction(j, i, algorithmFlag, IterationresultSets, vSpaces.Count, algiterationCount, clusterization_stopwatchs);
                }
                j +=5;
            }
            */
            #endregion
        }

        private void KmeansPP_test(object sender, RoutedEventArgs e)
        {

            #region usual_KMeansPP_alg_invoke
            /*
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            int iterationCount = 500;
            iterationCount = Convert.ToInt32(txtboxIterationCount.Text);
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            List<TestCentroid> firstCentroidListPP = new List<TestCentroid>();
            firstCentroidListPP = Tests.KMeansPPTest.CentroidCalculationsForTestKMeansPP(normilized_vSpace, clusterNumber);
            string KMeansPP_label_resul_path = @"F:\Magistry files\data\testData\10Clusters\KMeansPP_label_result10clust1.txt";
            string K_meansPP_report_path = @"F:\Magistry files\reports\KMeansPP_report10clust1.txt";
            List<TestCentroid> resultSet = Tests.NewTestKMeansClustering.Cluster(vSpace, clusterNumber, iterationCount);
            int[] KMeansPP_label_matrix = new int[vSpace.Count];
            KMeansPP_label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(resultSet, KMeansPP_label_resul_path);
            clusterization_stopwatch.Stop();
            message = RaportGeneration.RaportGenerationFunction(resultSet, clusterNumber, iterationCount, clusterization_stopwatch, K_meansPP_report_path);
            clustResultTxtBox.AppendText(message);
            */
            #endregion


            #region Iterational_test_KmeansPP
            int iteration = 5;
            string algorithmFlag = "KmeansPP";
            int clusterNumbers = Convert.ToInt32(txtboxClusterNumber.Text);
            int algiterationCount = 500;
            algiterationCount = Convert.ToInt32(txtboxIterationCount.Text);
            string test_path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpaces = TestDocVectorCreator.CreatingTheDocVectorCollection(test_path_data);
            List<DocumentVectorTest> normilized_vSpaces = TestDocVectorCreator.NormalizationDocumentVectors(vSpaces);
            List<TestCentroid> firstCentroidLists = new List<TestCentroid>();

            for (int j = 5; j <= clusterNumbers;)
            {
                for (int i = 1; i <= iteration; i++)
                {
                    clustResultTxtBox.Document.Blocks.Clear();
                    var clusterizations_stopwatchs = Stopwatch.StartNew();
                    firstCentroidLists = Tests.KMeansPPTest.CentroidCalculationsForTestKMeansPP(normilized_vSpaces, j);
                    List<TestCentroid> IterationresultSets = Tests.NewTestKMeansClustering.Cluster(vSpaces, j, algiterationCount);
                    clusterizations_stopwatchs.Stop();
                    Logic.IterationalReport.IterationalReportGenerationFunction(j, i, algorithmFlag, IterationresultSets, vSpaces.Count, algiterationCount, clusterizations_stopwatchs);
                }
                j += 5;
            }
            #endregion
        }

        private void FuzzyKMeansTest_Click(object sender, RoutedEventArgs e)
        {
            #region usual_KMeansPP_alg_invoke
            
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            float fuzziness = 2f;
            float epsilon = 0.1f;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<TestCentroid> resultSet = new List<TestCentroid>(clusterNumber);
            List<TestCentroid> resultSetRTest = new List<TestCentroid>();
            //List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            float[,] Result_fcm;
            //List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            var result = Tests.FuzzyKmeansTest.Fcm(vSpace, clusterNumber, epsilon, fuzziness);
            Result_fcm = result.Item1;
            int iterationCount = result.Item2;
            resultSet = Tests.FuzzyKmeansTest.CreateClusterSet(clusterNumber);
            var resultSetArray = Tests.FuzzyKmeansTest.AssignDocsToClusters(Result_fcm, clusterNumber, vSpace,resultSet);
            //resultSetRTest = Tests.FuzzyKmeansTest.AssignDocsToClusters(Result_fcm, clusterNumber, vSpace);
            string FuzzyKMeans_label_resul_path = @"F:\Magistry files\data\testData\FuzzyKMeans_label_result.txt";
            string Fuzzy_K_means_report_path = @"F:\Magistry files\reports\Fuzzy_KMeans_report.txt";
            //string RTest_FuzzyKMeans_label_resul_path = @"F:\Magistry files\data\testData\5Clusters\RTest_FuzzyKMeans_label_result5clust1.txt";
            string[] string_label_matrix = new string[resultSetArray.Item1.Length];
            for (int k = 0; k < resultSetArray.Item1.Length; k++)
                string_label_matrix[k] = resultSetArray.Item1[k].ToString();
            var result_Centroid_Set = resultSetArray.Item2;
            System.IO.File.WriteAllLines(FuzzyKMeans_label_resul_path, string_label_matrix);
            //string membership_matrix_file_path = @"F:\Magistry files\data\R_membership_matrix.txt";
            //var memberShipMatrix = Tests.MemberShipMatrixReader.ReadDataFromFile(membership_matrix_file_path, vSpace.Count, clusterNumber);
            //resultSet = Tests.FuzzyKmeansTest.CreateClusterSet(clusterNumber);
            //var resultSetArray1 = Tests.FuzzyKmeansTest.AssignDocsToClusters(memberShipMatrix, clusterNumber, vSpace, resultSet);
            //for (int k = 0; k < resultSetArray.Item1.Length; k++)
                //string_label_matrix[k] = resultSetArray1.Item1[k].ToString();
            //System.IO.File.WriteAllLines(RTest_FuzzyKMeans_label_resul_path, string_label_matrix);
            clusterization_stopwatch.Stop();
            message = RaportGeneration.RaportGenerationFunction(resultSet, clusterNumber, iterationCount, clusterization_stopwatch, Fuzzy_K_means_report_path);
            clustResultTxtBox.AppendText(message);
            #endregion


            #region Iterational_test_FuzzyKmeans
            /*
            int iteration = 5;
            float fuzzinesss = 2f;
            float epsilons = 0.1f;
            int clusterNumbers = Convert.ToInt32(txtboxClusterNumber.Text);
            int algiterationCount = 500;
            string algorithm = "FuzzyKMeans";
            algiterationCount = Convert.ToInt32(txtboxIterationCount.Text);
            string test_path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<TestCentroid> resultSets = new List<TestCentroid>(clusterNumbers);
            List<DocumentVectorTest> vSpaces = TestDocVectorCreator.CreatingTheDocVectorCollection(test_path_data);
            List<DocumentVectorTest> normilized_vSpaces = TestDocVectorCreator.NormalizationDocumentVectors(vSpaces);

            for (int j = 5; j <= clusterNumbers;)
            {
                for (int i = 1; i <= iteration; i++)
                {

                    clustResultTxtBox.Document.Blocks.Clear();
                    var clusterizations_stopwatchs = Stopwatch.StartNew();
                    float[,] Result_fcms;
                    var results = Tests.FuzzyKmeansTest.Fcm(vSpaces, j, epsilons, fuzzinesss);
                    Result_fcms = results.Item1;
                    int AlgiterationCounts = results.Item2;
                    resultSets = Tests.FuzzyKmeansTest.CreateClusterSet(j);
                    var resultSetArrays = Tests.FuzzyKmeansTest.AssignDocsToClusters(Result_fcms, j, vSpaces, resultSets);
                    string[] string_label_matrixs = new string[resultSetArrays.Item1.Length];
                    for (int k = 0; k < resultSetArrays.Item1.Length; k++)
                        string_label_matrixs[k] = resultSetArrays.Item1[k].ToString();
                    string FuzzyKMeans_label_resul_paths = @"F:\Magistry files\data\test\testData\" + j.ToString() + "Clusters\\FuzzyKMeans_label_result" + j.ToString() + "clust" + i.ToString() + ".txt";
                    string Fuzzy_K_means_report_paths = @"F:\Magistry files\reports\test\Fuzzy_KMeans_report" + j.ToString() + "clust" + i.ToString() + ".txt";
                    clusterizations_stopwatchs.Stop();
                    using (StreamWriter w = File.AppendText(FuzzyKMeans_label_resul_paths))
                    {
                        for (int k = 0; k < string_label_matrixs.Length; k++)
                            w.WriteLine(string_label_matrixs[k]);
                    }
                    RaportGeneration.VoidRaportGenerationFunction(algorithm, resultSetArrays.Item2, j, AlgiterationCounts, clusterizations_stopwatchs, Fuzzy_K_means_report_paths);
                }
                j += 5;
            }
            */
            #endregion
        }

        private void Gravitational_clustering_test_Click(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string message = null;
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            int M = 500;
            float G = 6.67408313131313131F * (float)Math.Pow(10, (-6));
            float deltaG = 0.001F;      
            float epsilon = 0.1F;
            float alpha = 0.06F;
            int clusterNumber = 6;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            M = Convert.ToInt32(txtboxIterationCount.Text);
            string gravitational_label_resul_path = @"F:\Magistry files\data\Gravitational_label_results5.txt";
            string Gravitational_report_path = @"F:\Magistry files\reports\Gravitational_reports5.txt";
            List<TestCentroid> result = new List<TestCentroid>(normilized_vSpace.Count);
            var result1 = Tests.Gravitational.GravitationalAlg(normilized_vSpace, G, deltaG, M, epsilon);
            var get_Clusters = Tests.Gravitational.GetClustersTest(result1, alpha, normilized_vSpace);
            int[] label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(get_Clusters, gravitational_label_resul_path);
            clusterization_stopwatch.Stop();
            message = RaportGeneration.RaportGenerationFunction(get_Clusters, get_Clusters.Count, M, clusterization_stopwatch, Gravitational_report_path);
            clustResultTxtBox.AppendText(message);
        }

        #region last_code_version
        /*
        private void newTestKmeans_test_Click(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            int iterationCount = 300;
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            List<TestCentroid> firstCentroidList = new List<TestCentroid>();
            //firstCentroidList = Tests.KMeansPPTest.CentroidCalculationsForTestKMeansPP(normilized_vSpace, clusterNumber);
            firstCentroidList = Test_KMeans.CentroidCalculationsForKMeans(normilized_vSpace, clusterNumber);
            string KMeansPP_label_resul_path = @"F:\Magistry files\data\KMeans_new1_label_result.txt";
            List<TestCentroid> resultSet = Tests.NewTestKMeansClustering.Cluster(vSpace, clusterNumber, iterationCount);
            int[] label_matrix = new int[vSpace.Count];
            label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(resultSet, KMeansPP_label_resul_path);
        }

        private void ClusterizationProcessingBtn(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            //List<string> docCollection = CreateDocumentCollection.GenerateCollection();
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            //the line above is add to verify the speed of calculations.
            //List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int totalIteration = 0;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);// here change the number of clusters;
            //var resultSet =  await Task<List<Centroid>>.Run(()=>KMeansClustering.DocumentClusterPreparation(clusterNumber, vSpace, ref totalIteration));
            List<Centroid> resultSet = KMeansClustering.DocumentClusterPreparation(clusterNumber, vSpace, ref totalIteration);
            
            clusterization_stopwatch.Stop();

            List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            List<List<string>> ClassCollection = Tests.DocClasses.ListOfClasses();
            var distance = Tests.InterclusterDistances.d_centroids(resultSet);
            var min_centroid_distances = Tests.InterclusterDistances.d_min_centroids(resultSet);
            var max_intracluster_d = Tests.IntraclusterDistances.d_max(resultSet);
            var min_intracluster_d = Tests.IntraclusterDistances.d_min(resultSet);
            var median_intracluster_d = Tests.IntraclusterDistances.d_sr(resultSet);
            var Recall_result = Tests.Recall.Recall_Calculating(resultSet, docs);
            var Precision_result = Tests.Precision.Precision_Calculating(resultSet, docs);
            var Purity = Tests.Purity.Purity_Calculating(resultSet, ClassCollection, vSpace);
            var Fmeasure = Tests.F1Measure.F1_Measure_Calculating(resultSet, ClassCollection);
            var GMeasure = Tests.F1Measure.G_Measure_Calculating(resultSet, ClassCollection);
            var NMI = Tests.NormilizedMutualInformation.NMI_Calculating(resultSet, ClassCollection, vSpace);
        }

        private void KPKMeansClustering(object sender, RoutedEventArgs e)
        {
            //where list of document enters to k-means algorytm?????
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            var lastElementInList = vSpace[vSpace.Count - 1];
            int dimensions = lastElementInList.VectorSpace.Length;
            int totalIteration = 100;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            //kmeanskpbtn.IsEnabled = false;
            KPImplementationsKMeans kmm = new KPImplementationsKMeans(clusterNumber, totalIteration, dimensions);
            kmm.SetDocumentData(vSpace);
            //AddRandomDocs(Int32.Parse(pTB.Text), kmm, Int32.Parse(dTB.Text));
            kmm.RunAlgorithm(totalIteration);
            var result = kmm.clusters;
            //kmeanskpbtn.IsEnabled = true;
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show(elapsedMs.ToString());
        }
        */
        #endregion

        private void invokeFilesToVisualizationGenerator(List<Centroid> resultSet)
        {
            string csvFilesPath = @"F:\Magistry files\csv_files\exported_csv\";
            string jsonFilesPath = @"F:\Magistry files\csv_files\exported_json\";
            DateTime dateTime = DateTime.Now;
            Random random = new Random();
            int index = random.Next(1, 500000);
            string articlesCSV = csvFilesPath + index.ToString() + "_articles.csv";
            string articlesJson = jsonFilesPath + index.ToString() + "_articles.json";
            string authorsCSV = csvFilesPath + index.ToString() + "_authors.csv";
            string authorsJson = jsonFilesPath + index.ToString() + "_authors.json";
            string clusteringCSV = csvFilesPath + index.ToString() + "_clustering.csv";
            string clusteringJson = jsonFilesPath + index.ToString() + "_clustering.json";
            GraphData_and_Visualizations.CreateGraphDatabaseNeo4j.GenerateArticlesToCSVandJsonFromDB(articlesCSV, articlesJson);
            GraphData_and_Visualizations.CreateGraphDatabaseNeo4j.GenerateAuthorsToCSVandJsonFromDB(authorsCSV, authorsJson);
            GraphData_and_Visualizations.CreateGraphDatabaseNeo4j.GenerateClusterizationResultToCSVandJsonFromDB(clusteringCSV, resultSet, clusteringJson);
        }

        private void btn_BruteForceDownload_Click(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(() => ParserRTF.parseRTF());
            Task.Factory.StartNew(() => Downloader.bruteForce());
        }
    }
}
