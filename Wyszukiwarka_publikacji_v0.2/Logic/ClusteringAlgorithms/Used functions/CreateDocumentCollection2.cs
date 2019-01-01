using System;
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

            int counter1 = 0;
            int counter2 = 0;
            int counter3 = 0;

            var database_processing = Stopwatch.StartNew();
            using (var dbContext = new ArticleDBDataModelContainer())
            {
                dbContext.PG_ArticlesSet.Load();

                foreach(var PG_articles in dbContext.PG_ArticlesSet.Local)
                {
                    string PG_record = PG_articles.title + PG_articles.abstractText + PG_articles.keywords;
                    DocumentCollection.Add(PG_record);
                    counter1++;
                }

                dbContext.PP_ArticlesSet.Load();

                foreach(var PP_articles in dbContext.PP_ArticlesSet.Local)
                {
                    string PP_record = PP_articles.article_title + PP_articles.article_source;
                    DocumentCollection.Add(PP_record);
                    counter1++;
                }

                dbContext.UG_ArticlesSet.Load();

                foreach (var UG_articles in dbContext.UG_ArticlesSet.Local)
                {
                    string UG_record = UG_articles.article_title + UG_articles.article_source + UG_articles.article_keywords;
                    DocumentCollection.Add(UG_record);
                    counter1++;
                }

                dbContext.UMK_ArticlesSet.Load();

                foreach (var UMK_articles in dbContext.UMK_ArticlesSet.Local)
                {
                    string UMK_record = UMK_articles.article_title + UMK_articles.article_Full_title + UMK_articles.article_eng_keywords + UMK_articles.article_pl_keywords+ UMK_articles.article_translated_title;
                    DocumentCollection.Add(UMK_record);
                    counter1++;
                }

                dbContext.WSB_ArticlesSet.Load();

                foreach (var WSB_articles in dbContext.WSB_ArticlesSet.Local)
                {
                    string WSB_record = WSB_articles.article_title + WSB_articles.article_common_title + WSB_articles.article_title_other_lang + WSB_articles.article_eng_keywords + WSB_articles.article_pl_keywords + WSB_articles.article_details;
                    DocumentCollection.Add(WSB_record);
                    counter1++;
                }

                counter2++;
            }

            counter3++;
            database_processing.Stop();


            //System.Windows.MessageBox.Show("The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds, "Database processing time" ,System.Windows.MessageBoxButton.OK);
            string processing_log = @"F:\Magistry files\Processing_log.txt";

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds.ToString() + ", database context counter: " + counter2.ToString() + ", selection counter in one dbContext: " + counter1.ToString() + ", method executing counter: " + counter3.ToString());
            }

            return DocumentCollection;
        }

        public static Dictionary<int, string> GenerateDocumentCollection_withoutLazyLoadingToDictionary()
        {
            Dictionary<int, string> DocumentCollection = new Dictionary<int, string>();

            int counter1 = 0;
            int counter2 = 0;
            int counter3 = 0;

            var database_processing = Stopwatch.StartNew();
            using (var dbContext = new ArticleDBDataModelContainer())
            {
                dbContext.PG_ArticlesSet.Load();

                foreach (var PG_articles in dbContext.PG_ArticlesSet.Local)
                {
                    string PG_record = PG_articles.title + PG_articles.abstractText + PG_articles.keywords;
                    if (!(DocumentCollection.ContainsKey(PG_articles.article_Id)) || !(DocumentCollection.ContainsValue(PG_record)))
                        DocumentCollection.Add(Convert.ToInt32(PG_articles.article_Id), PG_record);
                    else
                        continue;
                    counter1++;
                }

                dbContext.PP_ArticlesSet.Load();

                foreach (var PP_articles in dbContext.PP_ArticlesSet.Local)
                {
                    string PP_record = PP_articles.article_title + PP_articles.article_source;
                    if (!(DocumentCollection.ContainsKey(PP_articles.article_Id)) || !(DocumentCollection.ContainsValue(PP_record)))
                        DocumentCollection.Add(Convert.ToInt32(PP_articles.article_Id), PP_record);
                    else
                        continue;
                    counter1++;
                }

                dbContext.UG_ArticlesSet.Load();

                foreach (var UG_articles in dbContext.UG_ArticlesSet.Local)
                {
                    string UG_record = UG_articles.article_title + UG_articles.article_source + UG_articles.article_keywords;
                    if (!(DocumentCollection.ContainsKey(UG_articles.article_Id)) || !(DocumentCollection.ContainsValue(UG_record)))
                        DocumentCollection.Add(Convert.ToInt32(UG_articles.article_Id), UG_record);
                    else
                        continue;
                    counter1++;
                }

                dbContext.UMK_ArticlesSet.Load();

                foreach (var UMK_articles in dbContext.UMK_ArticlesSet.Local)
                {
                    string UMK_record = UMK_articles.article_title + UMK_articles.article_Full_title + UMK_articles.article_eng_keywords + UMK_articles.article_pl_keywords + UMK_articles.article_translated_title;
                    if (!(DocumentCollection.ContainsKey(UMK_articles.article_Id)) || !(DocumentCollection.ContainsValue(UMK_record)))
                        DocumentCollection.Add(Convert.ToInt32(UMK_articles.article_Id), UMK_record);
                    else
                        continue;
                    counter1++;
                }

                dbContext.WSB_ArticlesSet.Load();

                foreach (var WSB_articles in dbContext.WSB_ArticlesSet.Local)
                {
                    string WSB_record = WSB_articles.article_title + WSB_articles.article_common_title + WSB_articles.article_title_other_lang + WSB_articles.article_eng_keywords + WSB_articles.article_pl_keywords + WSB_articles.article_details;
                    if (DocumentCollection.ContainsKey(WSB_articles.article_Id))
                        continue;
                    else
                        DocumentCollection.Add(Convert.ToInt32(WSB_articles.article_Id), WSB_record);
                    counter1++;
                }

                counter2++;
            }

            counter3++;
            database_processing.Stop();


            //System.Windows.MessageBox.Show("The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds, "Database processing time" ,System.Windows.MessageBoxButton.OK);
            string processing_log = @"F:\Magistry files\Processing_log.txt";

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds.ToString() + ", database context counter: " + counter2.ToString() + ", selection counter in one dbContext: " + counter1.ToString() + ", method executing counter: " + counter3.ToString());
            }

            return DocumentCollection;
        }
    }
}
