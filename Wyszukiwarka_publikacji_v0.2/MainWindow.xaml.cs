using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Wyszukiwarka_publikacji_v0._2.Logic;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using System.Threading;

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

        private void ClusterizationProcessingBtn(object sender, RoutedEventArgs e)
        {
            var clusterization_stopwatch = Stopwatch.StartNew();
            List<string> docCollection = CreateDocumentCollection.GenerateCollection();
            List<DocumentVector> vSpace = VectorSpaceModel.DocumentCollectionProcessing(docCollection);
            int totalIteration = 0;
            int clusterNumber = 3; // here change the number of clusters;

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


            string K_means_clusterization_result = @"F:\Magistry files\KMeans_result.txt";
            File.WriteAllText(K_means_clusterization_result, 
                "Clusterization report: " + '\n' + 
                "Number of clusters: " + clusterNumber + '\n' + 
                "Iteration count: " + totalIteration + '\n' + 
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString());

            System.Windows.MessageBox.Show(Message+'\n'+"Iterations: " + totalIteration, "Clusterization Effect", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionality is not awailable now - in development!", "Informational message", MessageBoxButton.OK);

        }
    }
}
