using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;
using Neo4jClient.Cypher;
using System.Data.Entity;
using System.IO;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Newtonsoft.Json;

namespace Wyszukiwarka_publikacji_v0._2.GraphData_and_Visualizations
{
    class CreateGraphDatabaseNeo4j
    {
        public static string articleHeaderCSV = "Article_Id,Article_Title,Article_Abstract_Text,Article_Keywords,Article_Year,Article_Authors,Article_Url" + '\n';
        public static string authorHeaderCSV = "Author_Id,Author_Name,Author_Surename" + '\n';
        public static string clusteringCSV = "Cluster_Id,Article_Id,Article_Content" + '\n';
        public static string edgesCSV = "Article_Id,Author_Id" + '\n';


        public  static void GenerateArticlesToCSVandJsonFromDB(string articlesCSV, string articlesJson)
        {
            string csvContent = string.Empty;
            string jsonContent = "var articles = [";
            csvContent += articleHeaderCSV;
            using (var PG_dbcontext = new ArticleProjDBEntities())
            {
                var resul_PG = PG_dbcontext.PG_ArticlesSet.SqlQuery("SELECT * FROM dbo.PG_ArticlesSet").ToList();
                if (resul_PG != null)
                {
                    foreach (var item in resul_PG)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id,item.title,item.abstractText,item.keywords,item.year.ToString(),item.authors,item.url);
                        csvContent+=("\""+item.article_Id+"\",")+
                            ("\"" + item.title + "\",")+
                            ("\"" + item.abstractText + "\",")+
                            ("\"" + item.keywords + "\",")+
                            ("\"" + item.year + "\",")+
                            ("\"" + item.authors + "\",")+
                            ("\"" + item.url + "\"")+'\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var PP_dbcontext = new ArticleProjDBEntities())
            {
                var resul_PP = PP_dbcontext.PP_ArticlesSet.SqlQuery("SELECT * FROM dbo.PP_ArticlesSet").ToList();
                if (resul_PP != null)
                {
                    foreach (var item in resul_PP)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title, string.Empty, string.Empty, item.article_year.ToString(), item.article_author_line, item.article_DOI);
                        csvContent += ("\""+item.article_Id+"\",")+ 
                            ("\"" + item.article_title+ "\",")+ 
                            ("\"" + "" + "\",")+
                            ("\"" + "" + "\",")+
                            ("\"" + item.article_year + "\",")+
                            ("\"" + item.article_author_line + "\",")+
                            ("\"" + item.article_DOI + "\"")+'\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var UG_dbcontext = new ArticleProjDBEntities())
            {
                var resul_UG = UG_dbcontext.UG_ArticlesSet.SqlQuery("SELECT * FROM dbo.UG_ArticlesSet").ToList();
                if (resul_UG != null)
                {
                    foreach (var item in resul_UG)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title, string.Empty, item.article_keywords, string.Empty, item.article_author_line, item.article_DOI);
                        csvContent += ("\""+item.article_Id+"\",")+
                            ("\"" + item.article_title + "\",")+
                            ("\"" + "" + "\",")+
                            ("\"" + item.article_keywords + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_author_line + "\",") +
                            ("\"" + item.article_DOI + "\"") +'\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var UMK_dbcontext = new ArticleProjDBEntities())
            {
                var resul_UMK = UMK_dbcontext.UMK_ArticlesSet.SqlQuery("SELECT * FROM dbo.UMK_ArticlesSet").ToList();
                if (resul_UMK != null)
                {
                    foreach (var item in resul_UMK)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title + " " + item.article_Full_title + " " + item.article_translated_title, string.Empty, item.article_pl_keywords + " " + item.article_eng_keywords, string.Empty, item.article_author_line, item.article_url);
                        csvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.article_title+" "+item.article_Full_title +" "+ item.article_translated_title + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_pl_keywords + " " + item.article_eng_keywords + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_author_line + "\",") +
                            ("\"" + item.article_url + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var WSB_dbcontext = new ArticleProjDBEntities())
            {
                var resul_WSB = WSB_dbcontext.WSB_ArticlesSet.SqlQuery("SELECT * FROM dbo.WSB_ArticlesSet").ToList();
                if (resul_WSB != null)
                {
                    foreach (var item in resul_WSB)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title + " " + item.article_common_title + " " + item.article_title_other_lang, string.Empty, item.article_pl_keywords + " " + item.article_eng_keywords, string.Empty, item.article_authors,item.article_URL);
                        csvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.article_title + " "+ item.article_common_title + " "+item.article_title_other_lang+"\",") +
                            ("\"" + " " + "\",") +
                            ("\"" + item.article_pl_keywords + " "+ item.article_eng_keywords + "\",") +
                            ("\"" + " " + "\",") +
                            ("\"" + item.article_authors + "\",") +
                            ("\"" + item.article_URL + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            jsonContent += "]";
            using (StreamWriter csv_SW = File.AppendText(articlesCSV))
            {
                csv_SW.Write(csvContent);
            }
            using (StreamWriter json_SW = File.AppendText(articlesJson))
            {
                json_SW.Write(jsonContent);
            }
        }

        public static void GenerateAuthorsToCSVandJsonFromDB(string authorsCSV, string authorsJson)
        {
            string authorsContentCSV = string.Empty;
            string jsonContent = "var authors = [";
            authorsContentCSV += authorHeaderCSV;
            using (var AuthorDBContext = new ArticleProjDBEntities())
            {
                var authors_Result = AuthorDBContext.AuthorSet.SqlQuery("SELECT * FROM dbo.AuthorSet").ToList();
                if (authors_Result != null)
                {
                    foreach (var item in authors_Result)
                    {
                        AuthorsJsonObj authorsJsonObj = new AuthorsJsonObj(item.author_Id, item.author_name, item.author_surename);
                        authorsContentCSV += ("\"" + item.author_Id + "\",") +
                            ("\"" + item.author_name + "\",") +
                            ("\"" + item.author_surename + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(authorsJsonObj) + '\n';
                    }
                }
            }
            jsonContent += "]";
            using (StreamWriter sw = File.AppendText(authorsCSV))
            {
                sw.Write(authorsContentCSV);
            }
            using (StreamWriter json_SW = File.AppendText(authorsJson))
            {
                json_SW.Write(jsonContent);
            }
        }

        public static void GenerateClusterizationResultToCSVandJsonFromDB(string clusteringCSV,List<Centroid> clustering, string clusteringJson)
        {
            string clusteringContentCSV = string.Empty;
            string jsonContent = "var clustering_result = [";
            clusteringContentCSV += clusteringCSV;
            for(int i=0; i<clustering.Count; i++)
            {
                for(int j=0; j<clustering[i].GroupedDocument.Count; j++)
                {
                    ClusteringJsonObj newJsonObj = new ClusteringJsonObj(i,clustering[i].GroupedDocument[j].ArticleID, clustering[i].GroupedDocument[j].Content);
                    clusteringContentCSV += ("\"" + i.ToString() + "\",") +
                            ("\"" + clustering[i].GroupedDocument[j].ArticleID + "\",") +
                            ("\"" + clustering[i].GroupedDocument[j].Content + "\"") + '\n';

                    jsonContent += JsonConvert.SerializeObject(newJsonObj) + '\n';
                }
            }
            jsonContent += "]";
            using (StreamWriter sw = File.AppendText(clusteringCSV))
            {
                sw.Write(clusteringContentCSV);
            }
            using (StreamWriter json_SW = File.AppendText(clusteringJson))
            {
                json_SW.Write(jsonContent);
            }
        }

        /*
        public static void GenerateEdgesToCSVandJsonFromDB(string edgesCSV, List<Centroid> clustering, string edgesJson)
        {
            string edgesCsvContent = string.Empty;
            string edgesJsonContent = "var edges = [";
            edgesCsvContent += edgesCSV;
            using (var PG_dbcontext = new ArticleDBDataModelContainer())
            {
                //var resul_PG = PG_dbcontext.PG_ArticlesSet.SqlQuery("SELECT * FROM dbo.PG_ArticlesSet").ToList();
                var result_PG = PG_dbcontext.ExecuteSqlCommand("SELECT * FROM dbo.PG_ArticlesAuthor");
                if (result_PG != null)
                {
                    foreach (var item in result_PG)
                    {
                        EdgesJsonObj articlesJsonObj = new EdgesJsonObj(item.article_Id, item.author_id);
                        edgesCsvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.author_id + "\"") + '\n';
                        edgesJsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var PP_dbcontext = new ArticleDBDataModelContainer())
            {
                var resul_PP = PP_dbcontext.PP_ArticlesSet.SqlQuery("SELECT * FROM dbo.PP_ArticlesSet").ToList();
                if (resul_PP != null)
                {
                    foreach (var item in resul_PP)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title, string.Empty, string.Empty, item.article_year.ToString(), item.article_author_line, item.article_DOI);
                        csvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.article_title + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_year + "\",") +
                            ("\"" + item.article_author_line + "\",") +
                            ("\"" + item.article_DOI + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var UG_dbcontext = new ArticleDBDataModelContainer())
            {
                var resul_UG = UG_dbcontext.UG_ArticlesSet.SqlQuery("SELECT * FROM dbo.UG_ArticlesSet").ToList();
                if (resul_UG != null)
                {
                    foreach (var item in resul_UG)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title, string.Empty, item.article_keywords, string.Empty, item.article_author_line, item.article_DOI);
                        csvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.article_title + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_keywords + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_author_line + "\",") +
                            ("\"" + item.article_DOI + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var UMK_dbcontext = new ArticleDBDataModelContainer())
            {
                var resul_UMK = UMK_dbcontext.UMK_ArticlesSet.SqlQuery("SELECT * FROM dbo.UMK_ArticlesSet").ToList();
                if (resul_UMK != null)
                {
                    foreach (var item in resul_UMK)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title + " " + item.article_Full_title + " " + item.article_translated_title, string.Empty, item.article_pl_keywords + " " + item.article_eng_keywords, string.Empty, item.article_author_line, item.article_url);
                        csvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.article_title + " " + item.article_Full_title + " " + item.article_translated_title + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_pl_keywords + " " + item.article_eng_keywords + "\",") +
                            ("\"" + "" + "\",") +
                            ("\"" + item.article_author_line + "\",") +
                            ("\"" + item.article_url + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            using (var WSB_dbcontext = new ArticleDBDataModelContainer())
            {
                var resul_WSB = WSB_dbcontext.WSB_ArticlesSet.SqlQuery("SELECT * FROM dbo.WSB_ArticlesSet").ToList();
                if (resul_WSB != null)
                {
                    foreach (var item in resul_WSB)
                    {
                        ArticlesJsonObj articlesJsonObj = new ArticlesJsonObj(item.article_Id, item.article_title + " " + item.article_common_title + " " + item.article_title_other_lang, string.Empty, item.article_pl_keywords + " " + item.article_eng_keywords, string.Empty, item.article_authors, item.article_URL);
                        csvContent += ("\"" + item.article_Id + "\",") +
                            ("\"" + item.article_title + " " + item.article_common_title + " " + item.article_title_other_lang + "\",") +
                            ("\"" + " " + "\",") +
                            ("\"" + item.article_pl_keywords + " " + item.article_eng_keywords + "\",") +
                            ("\"" + " " + "\",") +
                            ("\"" + item.article_authors + "\",") +
                            ("\"" + item.article_URL + "\"") + '\n';
                        jsonContent += JsonConvert.SerializeObject(articlesJsonObj) + '\n';
                    }
                }
            }
            jsonContent += "]";
            using (StreamWriter csv_SW = File.AppendText(articlesCSV))
            {
                csv_SW.Write(csvContent);
            }
            using (StreamWriter json_SW = File.AppendText(articlesJson))
            {
                json_SW.Write(jsonContent);
            }
        }
        */
    }
    public class ClusteringJsonObj
    {
        [JsonProperty(PropertyName = "Cluster_ID")]
        int Cluster_ID { get; set; }
        [JsonProperty(PropertyName = "Article_ID")]
        int Article_ID { get; set; }
        [JsonProperty(PropertyName = "Article_Content")]
        string Article_Content { get; set; }

        public ClusteringJsonObj(int cluster_ID, int article_ID, string article_Content)
        {
            Cluster_ID = cluster_ID;
            Article_ID = article_ID;
            Article_Content = article_Content;
        }
    }

    public class ArticlesJsonObj
    {
        [JsonProperty(PropertyName ="Article_Id")]
        int Article_Id { get; set; }
        [JsonProperty(PropertyName = "Article_Title")]
        string Article_Title { get; set; }
        [JsonProperty(PropertyName = "Article_Abstract_Text")]
        string Article_Abstract_Text { get; set; }
        [JsonProperty(PropertyName = "Article_Keywords")]
        string Article_Keywords { get; set; }
        [JsonProperty(PropertyName = "Article_Year")]
        string Article_Year { get; set; }
        [JsonProperty(PropertyName = "Article_Authors")]
        string Article_Authors { get; set; }
        [JsonProperty(PropertyName = "Article_Url")]
        string Article_Url { get; set; }

        public ArticlesJsonObj(int article_Id, string article_Title, string article_Abstract_Text, string article_Keywords, string article_Year, string article_Authors, string article_Url)
        {
            Article_Id = article_Id;
            Article_Title = article_Title;
            Article_Abstract_Text = article_Abstract_Text;
            Article_Keywords = article_Keywords;
            Article_Year = article_Year;
            Article_Authors = article_Authors;
            Article_Url = article_Url;
        }
    }

    public class AuthorsJsonObj
    {
        [JsonProperty(PropertyName = "Author_Id")]
        int Author_Id { get; set; }
        [JsonProperty(PropertyName = "Article_ID")]
        string Author_Name { get; set; }
        [JsonProperty(PropertyName = "Author_Surename")]
        string Author_Surename { get; set; }


        public AuthorsJsonObj(int author_Id, string author_Name, string author_Surename)
        {
            Author_Id = author_Id;
            Author_Name = author_Name;
            Author_Surename = author_Surename;
        }
    }

    public class EdgesJsonObj
    {
        [JsonProperty(PropertyName = "")]
        int Author_Id { get; set; }
        [JsonProperty(PropertyName = "Author_Id")]
        int Article_ID { get; set; }

        public EdgesJsonObj(int author_Id, int article_ID)
        {
            Author_Id = author_Id;
            Article_ID = article_ID;
        }
    }
}
