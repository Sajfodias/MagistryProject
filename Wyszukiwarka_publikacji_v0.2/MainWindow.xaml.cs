using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Wyszukiwarka_publikacji_v0._2.Logic;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using System.ComponentModel;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms.KMeansPPImplementations;

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
            //Downloader.bruteForce();
            //ParserRTF.parseRTF();
            BibtexParser.loadBibtexFile();
            stopwatch.Stop();
            Console.WriteLine("Processing time " + stopwatch.Elapsed.Minutes + ":" + stopwatch.Elapsed.Milliseconds);
            Console.ReadKey();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {   
            RTFReader form = new RTFReader();
            form.Show();
        }

        /*
        public string NumberOfClusters
        {
            get { return txtboxClusterNumber.Text; }
            set { txtboxClusterNumber.Text = value; }
        }
        */

        private void ClusterizationProcessingBtn(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            //List<string> docCollection = CreateDocumentCollection.GenerateCollection();
            List<string> docCollection = Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            //the line above is add to verify the speed of calculations.
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int totalIteration = 0;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);// here change the number of clusters;

            //var resultSet =  await Task<List<Centroid>>.Run(()=>KMeansClustering.DocumentClusterPreparation(clusterNumber, vSpace, ref totalIteration));

            List<Centroid> resultSet = KMeansClustering.DocumentClusterPreparation(clusterNumber, vSpace, ref totalIteration);


            clusterization_stopwatch.Stop();

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clustResultTxtBox.Document.Blocks.Clear();
            var clusterization_stopwatch = Stopwatch.StartNew();
            //List<string> docCollection = CreateDocumentCollection.GenerateCollection();
            List<string> docCollection = Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            //the line above is add to verify the speed of calculations.
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int totalIteration = 0;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);// here change the number of clusters;

            //var resultSet =  await Task<List<Centroid>>.Run(()=>KMeansClustering.DocumentClusterPreparation(clusterNumber, vSpace, ref totalIteration));

            List<Centroid> resultSet = KMeansPlus.KMeansPlusClusterization(clusterNumber, vSpace, ref totalIteration);


            clusterization_stopwatch.Stop();

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

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //where list of document enters to k-means algorytm?????
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<string> docCollection = Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions.CreateDocumentCollection2.GenerateDocumentCollection_withoutLazyLoading();
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            var lastElementInList = vSpace[vSpace.Count - 1];
            int dimensions = lastElementInList.VectorSpace.Length;
            int totalIteration = 100;
            int clusterNumber = Convert.ToInt32(txtboxClusterNumber.Text);
            kmeanskpbtn.IsEnabled = false;
            KPImplementationsKMeans kmm = new KPImplementationsKMeans(clusterNumber, totalIteration, dimensions);
            kmm.SetDocumentData(vSpace);
            //AddRandomDocs(Int32.Parse(pTB.Text), kmm, Int32.Parse(dTB.Text));
            kmm.RunAlgorithm(totalIteration);
            var result = kmm.clusters;
            kmeanskpbtn.IsEnabled = true;
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show(elapsedMs.ToString());
        }
    }
}
