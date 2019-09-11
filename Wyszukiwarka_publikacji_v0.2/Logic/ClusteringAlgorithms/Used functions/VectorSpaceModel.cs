using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Logic;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class VectorSpaceModel
    {
        //public static HashSet<string> dTerms;
        //public static List<String> documentCollection;
        public static HashSet<string> termHashset;
        private static Regex r = new Regex("([ \\t{}()\",:;. \n])");
        public static string[] removableWords = { "and", "or", "it", "at", "all", "in", "on", "under", "between", "a", "an", "the", "to", "pod", "nad", "tam", "tutaj", "między", "pomiędzy", "w", "przed", "się", "z", "na", "od", "jest", "iż", "co", "we", "ich", "ciebie", "ja", "ty", "ona", "ono", "oni", "owych", "of", "cz", "do", "s", "n", "r", "nr", "rys", "i", "by", "from", "o", "//", "**", "po", "jej", "przy", "rzecz", "jak", "tymi", "są", "czy", "oraz", "ze", "m", "p", "off", "for", "/", "is", "as", "be", "will", "go", "za", "też", "lub", "t", "poz", "wiad", "set", "use", "etc", "also", "are", "tzw", "out", "other", "its", "has", "<", ">", "pre", "its", "has", "are", "with", "[et", "]", "vol", "leszek", "j", "al", "może", "być", "wy", "apis", "zb" };
        public static ParallelOptions parallelOption = new ParallelOptions();
        

        public static List<DocumentVector> DocumentCollectionProcessing(List<String> collection)
        {
            parallelOption.MaxDegreeOfParallelism = 20;
            var vector_space_model_calculation = Stopwatch.StartNew();
            //dTerms = new HashSet<string>();
            //documentCollection = CreateDocumentCollection.GenerateCollection();

            #region old_parts_of_code
            /*foreach (string documentContent in documentCollection)
            {
                foreach (string term in r.Split(documentContent))
                {
                    if (!StopWordsHandler.IsStotpWord(term))
                        dTerms.Add(term);
                    else
                        continue;
                }
            }
            List<string> removeList = new List<string>() { "\"", "\r", "\n", "(", ")", "[", "]", "{", "}", "", ".", " ", "," };
            foreach (string s in removeList)
            {
                dTerms.Remove(s);
            }*/
            #endregion

            termHashset = new HashSet<string>();

            using (var dbContext = new ArticleProjDBEntities())
            {
                dbContext.Terms_Vocabulary.Load();

                foreach(var terms in dbContext.Terms_Vocabulary.Local)
                {
                    termHashset.Add(terms.term_value.ToLower());
                }
            }

            /*
            foreach(var items in termHashset)
            {
                dTerms.Add(items.ToLower());
            }
            */

            List<DocumentVector> documentVectorSpace = new List<DocumentVector>();
            DocumentVector _documentVector;
            float[] space;

            // trying to optimize execution time 04.10.2017
            //foreach (string document in documentCollection)
            Parallel.ForEach(collection, parallelOption,document => {
                int count = 0;
                space = new float[termHashset.Count];
                //space = new float[dTerms.Count];
                //foreach (string term in dTerms)
                foreach(string term in termHashset)
                {
                    //space[count] = CalculateTFIDF.FindTFIDF(collection, document, term);
                    space[count] = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.FindTFIDF(collection,document,term);
                    count++;
                }

                _documentVector = new DocumentVector();

                #region dont_usable_now
                //last changes 21.05.2018
                /*
                using (var PGDbContext = new ArticleDBDataModelContainer())
                {
                    foreach (var article in PGDbContext.PG_ArticlesSet)
                    {
                        string PG_article = article.title + article.abstractText + article.keywords;
                        if (PG_article.Contains(document))
                            _documentVector.ArticleID = article.article_Id;
                    } 
                }
                using (var PPDbContext = new ArticleDBDataModelContainer())
                {
                    foreach (var article in PPDbContext.PP_ArticlesSet)
                    {
                        string PP_record = article.article_title + article.article_source;
                        if (PP_record.Contains(document))
                            _documentVector.ArticleID = article.article_Id;
                    }
                }
                using (var UMKDbContext = new ArticleDBDataModelContainer())
                {
                    foreach (var article in UMKDbContext.UMK_ArticlesSet)
                    {
                        string UMK_record = article.article_title + article.article_Full_title + article.article_eng_keywords + article.article_pl_keywords + article.article_translated_title;
                        if (UMK_record.Contains(document))
                            _documentVector.ArticleID = article.article_Id;
                    }  
                }
                using (var UGDbContext = new ArticleDBDataModelContainer())
                {
                    foreach (var article in UGDbContext.UG_ArticlesSet)
                    {
                        string UG_record = article.article_title + article.article_source + article.article_keywords;
                        if (UG_record.Contains(document))
                            _documentVector.ArticleID = article.article_Id;
                    }
                        
                }
                using (var WSBDbContext = new ArticleDBDataModelContainer())
                {
                    foreach (var article in WSBDbContext.WSB_ArticlesSet)
                    {
                        string WSB_record = article.article_title + article.article_common_title + article.article_title_other_lang + article.article_eng_keywords + article.article_pl_keywords + article.article_details;
                        if (WSB_record.Contains(document))
                            _documentVector.ArticleID = article.article_Id;
                    }
                }
                */
                #endregion

                _documentVector.Content = document;
                _documentVector.VectorSpace = space;
                _documentVector.index_Of_Doc_for_labeling = collection.IndexOf(document);
                documentVectorSpace.Add(_documentVector);
            });

            /*
            foreach(string document in collection)
            {
                int count = 0;
                space = new float[dTerms.Count];
                foreach (string term in dTerms){
                    space[count] = CalculateTFIDF.FindTFIDF(collection,document, term);
                    count++;
                }
               
                _documentVector = new DocumentVector();
                _documentVector.Content = document;
                _documentVector.VectorSpace = space;
                documentVectorSpace.Add(_documentVector);
                //tu mamy 2296 termow
                //ClusteringAlgorithms.Used_functions.Normalization.Normilize_Term_Frequency(documentVectorSpace); // are that the correct place to perform normalization?

            }
            */
            vector_space_model_calculation.Stop();

            //string processing_log = @"F:\Magistry files\Processing_log.txt";
            string logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"].ToString();
            string processingLogFileName = ConfigurationManager.AppSettings["ProcessingLogFile"].ToString();
            string processing_log = Path.Combine(logFileDirectory, processingLogFileName);

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The vector space model calculation time is: " + vector_space_model_calculation.Elapsed.Minutes.ToString() + ":" + vector_space_model_calculation.Elapsed.TotalMilliseconds.ToString());
            }

            return documentVectorSpace;
        }

        internal static List<DocumentVector> DocumentCollectionProcessingDictionary(Dictionary<int, string> docCollectionDictionary)
        {
            parallelOption.MaxDegreeOfParallelism = 20;
            var vector_space_model_calculation = Stopwatch.StartNew();

            termHashset = new HashSet<string>();

            using (var dbContext = new ArticleProjDBEntities())
            {
                dbContext.Terms_Vocabulary.Load();

                foreach (var terms in dbContext.Terms_Vocabulary.Local)
                {
                    termHashset.Add(terms.term_value.ToLower());
                }
            }

            List<DocumentVector> documentVectorSpace = new List<DocumentVector>();
            DocumentVector _documentVector;
            float[] space;
            int index=0;
            var arrayOfDocs = docCollectionDictionary.Keys.ToArray();

            Parallel.ForEach(docCollectionDictionary, parallelOption, document => {
                int count = 0;
                space = new float[termHashset.Count];
                var collectionValue = docCollectionDictionary.Values.ToList();

                foreach (string term in termHashset)
                {
                    space[count] = Logic.ClusteringAlgorithms.Used_functions.TFIDF2ndrealization.FindTFIDF(collectionValue, document.Value, term);
                    count++;
                }
                for (int i = 0; i < arrayOfDocs.Length; i++)
                    if (arrayOfDocs[i] == document.Key)
                        index = i;

                _documentVector = new DocumentVector();

                _documentVector.ArticleID = document.Key;
                _documentVector.index_Of_Doc_for_labeling = index;
                _documentVector.Content = document.Value;
                _documentVector.VectorSpace = space;
                documentVectorSpace.Add(_documentVector);
            });
            vector_space_model_calculation.Stop();

            string logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"].ToString();
            string processingLogFile = ConfigurationManager.AppSettings["ProcessingLogFile"].ToString();
            string processing_log = Path.Combine(logFileDirectory, processingLogFile);

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The vector space model calculation time is: " + vector_space_model_calculation.Elapsed.Minutes.ToString() + ":" + vector_space_model_calculation.Elapsed.TotalMilliseconds.ToString());
            }

            return documentVectorSpace;
        }
    }
}
