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
            Downloader.AllocConsole();
            var stopwatch = Stopwatch.StartNew();
            Downloader.bruteForce();
            //ParserRTF.parseRTF();
            //BibtexParser.LoadBibtexFile();
            stopwatch.Stop();
            Console.WriteLine("Processing time " + stopwatch.Elapsed.Minutes + ":" + stopwatch.Elapsed.Milliseconds);
            //Console.ReadKey();
        }

        private void rtfReader(object sender, RoutedEventArgs e)
        {   
            RTFReader form = new RTFReader();
            form.Show();
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

            #region Raport_creation
            string Message = String.Empty;
            int count = 1;
            foreach(Centroid c in resultSet)
            {
                Message += String.Format("Documents in Cluster {0} {1}", count, System.Environment.NewLine);
                foreach(DocumentVector doc in c.GroupedDocument)
                {
                    Message += doc.Content + System.Environment.NewLine;
                    if(c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
                count++;
            }

            Random rnd = new Random();
            int index = rnd.Next(1, 5000);
            string K_means_clusterization_result = @"F:\Magistry files\KMeans_result.txt";
            using(StreamWriter sw = File.AppendText(K_means_clusterization_result))
            {
                sw.WriteLine(@"Clusterization report nr: " + "00"+ index.ToString() + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString()
                + System.Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------"
                );
            }
            /*File.WriteAllText(K_means_clusterization_result, 
                "Clusterization report: " + '\n' + 
                "Number of clusters: " + clusterNumber + '\n' + 
                "Iteration count: " + totalIteration + '\n' + 
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());
                */

            clustResultTxtBox.AppendText("Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());
            //System.Windows.MessageBox.Show(Message+'\n'+"Iterations: " + totalIteration, "Clusterization Effect", MessageBoxButton.OK, MessageBoxImage.Information);
            #endregion
        }

        private void KMeansPPClustering(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            //List<string> docCollection = CreateDocumentCollection.GenerateCollection();
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            //the line above is add to verify the speed of calculations.
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int totalIteration = 100;
            int clusterNumber = 5;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);// here change the number of clusters;
            //var resultSet =  await Task<List<Centroid>>.Run(()=>KMeansClustering.DocumentClusterPreparation(clusterNumber, vSpace, ref totalIteration));
            List<Centroid> firstCentroidList = new List<Centroid>();
            firstCentroidList = CentroidCalculationClass.CentroidCalculationsForKMeansPP(vSpace, clusterNumber);
            //List<DocumentVectorWrapper> wrappedList = Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations.MyKmeansPPInterpritationcs.WrappedCollections(vSpace);
            List<Centroid> resultSet = Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations.MyKmeansPPInterpritationcs.NewKMeansClusterization(clusterNumber, docCollection, totalIteration, vSpace, wordIndex, firstCentroidList);
            //List<Centroid> resultSet = KMeansPlus.KMeansPlusClusterization(clusterNumber, vSpace, ref totalIteration);
            clusterization_stopwatch.Stop();

            #region Report_generation
            string Message = String.Empty;
            int count = 1;
            foreach (Centroid c in resultSet)
            {
                Message += String.Format("Documents in Cluster {0} {1}", count, System.Environment.NewLine);
                foreach (DocumentVector doc in c.GroupedDocument)
                {
                    Message += doc.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
                count++;
            }

            string K_means_clusterization_result = @"F:\Magistry files\KMeans_pp_result.txt";
            File.WriteAllText(K_means_clusterization_result,
                "Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());

            clustResultTxtBox.AppendText("Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());
            //MessageBox.Show("This functionality is not awailable now - in development!", "Informational message", MessageBoxButton.OK);
            #endregion
        }

        private void KmeanslastVersion(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int totalIteration = 100;
            int clusterNumber = 5;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<Centroid> firstCentroidList = new List<Centroid>();
            firstCentroidList = CentroidCalculationClass.CentroidCalculationsForKMeans(vSpace, clusterNumber);
            List<Centroid> resultSet = Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations.MyKmeansPPInterpritationcs.NewKMeansClusterization(clusterNumber, docCollection, totalIteration, vSpace, wordIndex, firstCentroidList);
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
            var Entropy = Tests.Entropy.Enthropy_Calculating(resultSet, ClassCollection);

            #region Report_generation2
            string Message = String.Empty;
            int count = 1;
            foreach (Centroid c in resultSet)
            {
                Message += String.Format("Documents in Cluster {0} {1}", count, System.Environment.NewLine);
                foreach (DocumentVector doc in c.GroupedDocument)
                {
                    Message += doc.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
                count++;
            }

            string K_means_clusterization_result = @"F:\Magistry files\KMeans_pp_result.txt";
            File.WriteAllText(K_means_clusterization_result,
                "Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());

            clustResultTxtBox.AppendText("Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());
            //MessageBox.Show("This functionality is not awailable now - in development!", "Informational message", MessageBoxButton.OK);
            #endregion
        }

        private void DictionaryPreparation(object sender, RoutedEventArgs e)
        {
            string path = @"F:\Magistry files\csv_files\Allowed_term_dictionary.csv";
            string dictionary_text = File.ReadAllText(path);
            string[] allowed_dictionary = dictionary_text.Trim(' ').Split(',','\n','\r');
            List<string> result = new List<string>();

            for (int i = 0; i <= allowed_dictionary.Length - 1; i++)
                if (allowed_dictionary[i] != "")
                    result.Add(allowed_dictionary[i].TrimStart());
            string result_text = String.Join(",", result);
            File.WriteAllText(path, result_text);
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

        #region Dont used James McCaffrey example
        /*
        private void OldKMeansPPClustering(object sender, RoutedEventArgs e)
        {
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);// here change the number of clusters;
            int seed = 0;
            int[] result = KMeansPP2Implementation.Cluster(vSpace, clusterNumber, seed);
            string Message = String.Empty;

            Message = KMeansPP2Implementation.ShowClustered(vSpace, result, clusterNumber, 1);
            string result_K_means_pp = @"F:\Magistry files\KMeans_pp_result.txt";
            File.WriteAllText(result_K_means_pp, Message);
            MessageBox.Show(Message, "Clusterization result!", MessageBoxButton.OK);
        }
        */
        #endregion

        private void FuzzyKMeans_Click(object sender, RoutedEventArgs e)
        {
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection= Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string,int> wordIndex= Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            float fuzziness = 0.5f;
            float epsilon = 0.003f;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<Centroid> resultSet = new List<Centroid>(clusterNumber);
            float[,] Result_fcm;
            List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            Result_fcm = FuzzyKMeans.Fcm(vSpace, clusterNumber, epsilon, fuzziness, termCollection);

            #region FKM_distance_matrix_to_File
            var message_row = String.Empty;
            var message = String.Empty;
            for(int i=0; i<Result_fcm.GetLength(0); i++)
            {
                for(int j=0; j<Result_fcm.GetLength(1); j++)
                {
                    message_row += Result_fcm[i, j] + ' ' + '\t';
                }
                message += message_row + '\n';
                message_row = String.Empty;
            }
            //MessageBox.Show(message);
            
            string Fuzzy_K_means_clusterization_result = @"F:\Magistry files\Fuzzy_KMeans_result.txt";
            File.Delete(Fuzzy_K_means_clusterization_result);
            File.Create(Fuzzy_K_means_clusterization_result).Close();
            if(File.Exists(Fuzzy_K_means_clusterization_result))
                File.AppendAllText(Fuzzy_K_means_clusterization_result, message);
            #endregion

            resultSet = FuzzyKMeans.AssignDocsToClusters(Result_fcm, clusterNumber,vSpace);
            FuzzyKMeans.Show_clusters(vSpace, Result_fcm, clusterNumber);
        }

        #region OldCode
        /*
        public void MyKmeansPP_Click(object sender, RoutedEventArgs e)
        {
            List<string> docCollection = Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            HashSet<string> termCollection = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.getTermCollection();
            Dictionary<string, int> wordIndex = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.DocumentsContainsTerm(docCollection, termCollection);
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            int iterationCount = 100;
            int docCollectionLength = docCollection.Count;
            //var results = MyKmeansPPInterpritationcs.KMeansClusterization(clusterNumber, docCollection, iterationCount, vSpace, wordIndex);
            var results1 = MyKmeansPPInterpritationcs.Showresults(docCollection, iterationCount, vSpace, docCollectionLength, clusterNumber, wordIndex, termCollection);
        }
        */
        #endregion

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

        private void Gravitational_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void Kmeans_test(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpace_test = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            //work too slow with that parameters
            //int totalIteration = 1000;
            //int clusterNumber = 15;
            int totalIteration = 300;
            int clusterNumber = 6;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<TestCentroid> firstCentroidList = new List<TestCentroid>();
            firstCentroidList = Test_KMeans.CentroidCalculationsForKMeans(vSpace_test, clusterNumber);
            string KMeans_label_resul_path = @"F:\Magistry files\data\KMeans_label_result.txt";
            List<TestCentroid> resultSet = Test_KMeans.NewKMeansClusterization(clusterNumber, totalIteration, vSpace_test, firstCentroidList);
            int[] label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(resultSet, KMeans_label_resul_path);
            clusterization_stopwatch.Stop();

            #region Report_generation_Test_KMeans
            string Message = String.Empty;
            int count = 1;
            foreach (TestCentroid c in resultSet)
            {
                Message += String.Format("Documents in Cluster {0} {1}", count, System.Environment.NewLine);
                foreach (DocumentVectorTest doc in c.GroupedDocument)
                {
                    Message += doc.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
                count++;
            }

            string K_means_clusterization_result = @"F:\Magistry files\KMeans_pp_result.txt";
            File.WriteAllText(K_means_clusterization_result,
                "Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());

            clustResultTxtBox.AppendText("Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + totalIteration + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());
            //MessageBox.Show("This functionality is not awailable now - in development!", "Informational message", MessageBoxButton.OK);
            #endregion
        }

        private void KmeansPP_test(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            int clusterNumber = 6;
            int totalIteration = 300;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);// here change the number of clusters;
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpace_test = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            
            List<TestCentroid> firstCentroidList = new List<TestCentroid>();
            firstCentroidList = Tests.KMeansPPTest.CentroidCalculationsForTestKMeansPP(vSpace_test, clusterNumber);
            string KMeansPP_label_resul_path = @"F:\Magistry files\data\KMeansPP_label_result.txt";

            List<TestCentroid> resultSet = Test_KMeans.NewKMeansClusterization(clusterNumber, totalIteration, vSpace_test, firstCentroidList);
            int[] label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(resultSet, KMeansPP_label_resul_path);
            clusterization_stopwatch.Stop();
        }

        private void FuzzyKMeansTest_Click(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            int[] resultSetArray = new int[vSpace.Count];
            float fuzziness = 0.5f;
            float epsilon = 0.3f;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            List<TestCentroid> resultSet = new List<TestCentroid>(clusterNumber);
            //List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            float[,] Result_fcm;
            //List<string> docs = Tests.DocClasses.SurveyAndMeasurementsClassOfDocuments_ListCreations();
            Result_fcm = Tests.FuzzyKmeansTest.Fcm(vSpace, clusterNumber, epsilon, fuzziness);
            resultSetArray = Tests.FuzzyKmeansTest.AssignDocsToClusters(Result_fcm, clusterNumber, vSpace);
            //resultSet = Tests.FuzzyKmeansTest.AssignDocsToClusters(Result_fcm, clusterNumber, vSpace);
            string FuzzyKMeans_label_resul_path = @"F:\Magistry files\data\FuzzyKMeans_label_result.txt";
            string[] string_label_matrix = new string[resultSetArray.Length];
            for (int k = 0; k < resultSetArray.Length; k++)
                string_label_matrix[k] = resultSetArray[k].ToString();

            System.IO.File.WriteAllLines(FuzzyKMeans_label_resul_path, string_label_matrix);
            clusterization_stopwatch.Stop();
        }

        private void Gravitational_clustering_test_Click(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            string path_data = @"F:\Magistry files\test_data\s1_data.txt";
            List<DocumentVectorTest> vSpace = TestDocVectorCreator.CreatingTheDocVectorCollection(path_data);
            List<DocumentVectorTest> normilized_vSpace = TestDocVectorCreator.NormalizationDocumentVectors(vSpace);
            int M = 500;
            //float G = 7 * (float)Math.Pow(10, (-6));
            float G = -1.28171817154F; //G=1*e-4 according to 3.2.2  in article
            //float G = 6.67408313131313131F * (float)Math.Pow(10, (-11));
            float deltaG = 0.001F;
            //float epsilon = -3.28171817154F;//epsilon=1*e-6 according to 3.2.2 in article or 10^(-4)= 0.0001F;
            float epsilon = 0.1F;
            float alpha = 0.06F;
            int clusterNumber = 6;
            clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            string gravitational_label_resul_path = @"F:\Magistry files\data\Gravitational_label_result.txt";
            List<TestCentroid> result = new List<TestCentroid>(normilized_vSpace.Count);
            result = Tests.GravitationalClusteringAlgorithm.Gravitational(normilized_vSpace, G, deltaG, M, epsilon, clusterNumber);

            var get_Clusters = Tests.GravitationalClusteringAlgorithm.GetClustersTest(result, alpha, normilized_vSpace);
            int[] label_matrix = Tests.Label_Matrix.Label_Matrix_Extractions(result, gravitational_label_resul_path);
        }
    }
}
