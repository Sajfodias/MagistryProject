﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    public static class CreateDocumentCollection2
    {
        public static List<string> GenerateDocumentCollection_withoutLazyLoading()
        {
            List<string> DocumentCollection = new List<string>();

            var database_processing = Stopwatch.StartNew();
            using (var dbContext = new ArticlesDataContainer())
            {
                dbContext.PG_ArticlesSet.Load();

                foreach(var articles in dbContext.PG_ArticlesSet.Local)
                {
                    string record = articles.title + articles.abstractText + articles.keywords;
                    DocumentCollection.Add(record);
                }
            }
            database_processing.Stop();

            //System.Windows.MessageBox.Show("The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds, "Database processing time" ,System.Windows.MessageBoxButton.OK);
            string processing_log = @"F:\Magistry files\Processing_log.txt";

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds.ToString());
            }

            return DocumentCollection;
        }
    }
}
