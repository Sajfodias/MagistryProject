using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class RaportGeneration
    {
        public static string RaportGenerationFunction(List<TestCentroid> resultSet, int clusterNumber, int iterationCount, Stopwatch clusterization_stopwatch, string RaportfilePath)
        {
            string Message = String.Empty;
 
            foreach (TestCentroid c in resultSet)
            {
                foreach (DocumentVectorTest doc in c.GroupedDocument)
                {
                    Message += doc.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
            }

            string message = "Clusterization report: " + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + iterationCount + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString();

            using(StreamWriter sw = File.AppendText(RaportfilePath))
            {
                sw.Write(message);
            }
            //File.WriteAllText(RaportfilePath, message);
            return message;
        }

        public static string ReleaseRaportGenerationFunction(List<Centroid> resultSet, int clusterNumber, int iterationCount, Stopwatch clusterization_stopwatch, string RaportfilePath, string algorithm)
        {
            string Message = String.Empty;
            string Authors;
            string author_line = null;
            int count = 1;
            foreach (Centroid c in resultSet)
            {
                Message += String.Format("Documents in Cluster {0} {1}", count, System.Environment.NewLine)+
                    "----------------------------------------------------------"+'\n';

                foreach (DocumentVector doc in c.GroupedDocument)
                {
                    Authors = SelectAutorsFromDB(doc.ArticleID);
                    Message += "ArticleID: " + doc.ArticleID + System.Environment.NewLine
                        + "Authors of the article: " + Authors + System.Environment.NewLine
                        + "Content: " + System.Environment.NewLine + doc.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
                count++;
            }

            string message = "Clusterization report: " + '\n' +
                "Used algorithm/technic: " + algorithm + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + iterationCount + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + System.Environment.NewLine + Message.ToString();

            using (StreamWriter sw = File.AppendText(RaportfilePath))
            {
                sw.Write(message);
            }
            //File.WriteAllText(RaportfilePath, message);
            return message;
        }


        public static string SelectAutorsFromDB(int ArticleID)
        {
            List<string> Author_list = new List<string>();
            using(var article = new ArticleProjDBEntities())
            {

                Author_list = article.Database.SqlQuery<string>("SELECT authors FROM dbo.PG_ArticlesSet WHERE article_Id=" + ArticleID.ToString()).ToList();
                
                if (Author_list.Count <1)
                    Author_list = article.Database.SqlQuery<string>("SELECT article_author_line FROM dbo.PP_ArticlesSet WHERE article_Id=" + ArticleID.ToString()).ToList();
                else if (Author_list.Count < 1)
                    Author_list = article.Database.SqlQuery<string>("SELECT article_author_line FROM dbo.UG_ArticlesSet WHERE article_Id=" + ArticleID.ToString()).ToList();
                else if (Author_list.Count < 1)
                    Author_list = article.Database.SqlQuery<string>("SELECT article_authors_line FROM dbo.UMK_ArticlesSet WHERE article_Id=" + ArticleID.ToString()).ToList();
                else if (Author_list.Count < 1)
                    Author_list = article.Database.SqlQuery<string>("SELECT article_authors FROM dbo.WSB_ArticlesSet WHERE article_Id=" + ArticleID.ToString()).ToList();

            }
            string Authors = string.Join(", ", Author_list.ToArray());
            return Authors;
        }

        public static void VoidRaportGenerationFunction(string Alg, List<TestCentroid> resultSet, int clusterNumber, int iterationCount, Stopwatch clusterization_stopwatch, string RaportfilePath)
        {
            string Message = String.Empty;
            foreach (TestCentroid c in resultSet)
            {
                Message += String.Format("Documents in Cluster {0} {1}", c.GroupedDocument.Count, System.Environment.NewLine);
                foreach (DocumentVectorTest doc in c.GroupedDocument)
                {
                    Message += doc.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        Message += String.Format("{0}----------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                Message += "---------------------------------------------------" + System.Environment.NewLine;
            }

            string message = "Clusterization report: " + '\n' +
                "Clustering algorithm: " + Alg + '\n' +
                "Number of clusters: " + clusterNumber + '\n' +
                "Iteration count: " + iterationCount + '\n' +
                "Clusterization time: " + clusterization_stopwatch.Elapsed.TotalMinutes.ToString() + ":" + clusterization_stopwatch.ElapsedMilliseconds.ToString() + '\n' +
                "Clusterization result: " + '\n' + Message.ToString();

            using (StreamWriter sw = File.AppendText(RaportfilePath))
            {
                sw.Write(message);
            }
        }
    }
}
