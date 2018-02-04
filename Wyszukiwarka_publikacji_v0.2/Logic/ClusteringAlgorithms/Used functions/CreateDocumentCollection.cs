using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using System.Diagnostics;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class CreateDocumentCollection
    {

        public static List<string> GenerateCollection()
        {
            List<string> DocumentCollection = new List<string>();

            Stopwatch database_processing = Stopwatch.StartNew();

            database_processing.Start();
            using (var dbContext = new ArticleDBDataModelContainer())
            {
                var resul_PG = dbContext.PG_ArticlesSet.SqlQuery("SELECT * FROM dbo.PG_ArticlesSet").ToList();
                if (resul_PG != null)
                {
                    foreach (var item in resul_PG)
                    {
                        if (item.title != null || item.title!= String.Empty || item.abstractText != null || item.abstractText != String.Empty || item.keywords != null || item.keywords != String.Empty)
                            DocumentCollection.Add(item.title.ToLower() + item.abstractText.ToLower() + item.keywords.ToLower());
                    }
                }

                var result_PP = dbContext.PP_ArticlesSet.SqlQuery("SELECT * FROM dbo.PP_ArticlesSet").ToList();
                if (result_PP != null)
                {
                    foreach (var PP_item in result_PP)
                    {
                        if (PP_item.article_title != null || PP_item.article_title != String.Empty || PP_item.article_source != null || PP_item.article_source != String.Empty)
                            DocumentCollection.Add(PP_item.article_title.ToLower() + PP_item.article_source.ToLower());
                    }
                        
                }

                var result_UG = dbContext.UG_ArticlesSet.SqlQuery("SELECT * FROM UG_ArticlesSet").ToList();
                if (result_UG != null)
                {
                    foreach (var UG_item in result_UG)
                    {
                        if (UG_item.article_title != null || UG_item.article_title != String.Empty || UG_item.article_keywords != null || UG_item.article_keywords != String.Empty)
                            DocumentCollection.Add(UG_item.article_title.ToLower() + UG_item.article_keywords.ToLower());
                    }
                        
                }

                var result_UMK = dbContext.UMK_ArticlesSet.SqlQuery("SELECT * FROM UMK_ArticlesSet").ToList();
                if (result_UMK != null)
                {
                    foreach (var UMK_item in result_UMK)
                    {
                        if (UMK_item.article_title != null || UMK_item.article_title != String.Empty || UMK_item.article_Full_title != null || UMK_item.article_Full_title != String.Empty || UMK_item.article_translated_title != null || UMK_item.article_translated_title != String.Empty || UMK_item.article_publisher_title != null 
                            || UMK_item.article_publisher_title != String.Empty || UMK_item.article_eng_keywords != null || UMK_item.article_eng_keywords != String.Empty || UMK_item.article_pl_keywords != null || UMK_item.article_pl_keywords != String.Empty)
                        {
                            DocumentCollection.Add(UMK_item.article_title.ToLower()
                                                + UMK_item.article_Full_title.ToLower()
                                                + UMK_item.article_translated_title.ToLower()
                                                + UMK_item.article_publisher_title.ToLower()
                                                + UMK_item.article_eng_keywords.ToLower()
                                                + UMK_item.article_pl_keywords.ToLower());
                        }

                    }  
                }

                var result_WSB = dbContext.WSB_ArticlesSet.SqlQuery("SELECT * FROM WSB_ArticlesSet").ToList();
                if (result_WSB != null)
                {
                    foreach (var WSB_item in result_WSB)
                    {
                        if (WSB_item.article_title != null || WSB_item.article_title != String.Empty || WSB_item.article_common_title != null || WSB_item.article_common_title != String.Empty || WSB_item.article_title_other_lang != null || WSB_item.article_title_other_lang != String.Empty 
                            || WSB_item.article_pl_keywords != null || WSB_item.article_pl_keywords != String.Empty || WSB_item.article_eng_keywords != null || WSB_item.article_eng_keywords != String.Empty)
                            DocumentCollection.Add(WSB_item.article_title.ToLower()
                                                    + WSB_item.article_common_title.ToLower()
                                                    + WSB_item.article_title_other_lang.ToLower()
                                                    + WSB_item.article_pl_keywords.ToLower()
                                                    + WSB_item.article_eng_keywords.ToLower());
                    }
                }
            }
            /*
            database_processing.Stop();
            string processing_log = @"F:\Magistry files\Processing_log.txt";

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The database processing time is: " + database_processing.Elapsed.Minutes.ToString() + ":" + database_processing.Elapsed.TotalMilliseconds.ToString() + ", database context counter: " + counter2.ToString() + ", selection counter in one dbContext: " + counter1.ToString() + ", method executing counter: " + counter3.ToString());
            }
            */
            return DocumentCollection;
        }

        //create term collection method here!
    }
}
