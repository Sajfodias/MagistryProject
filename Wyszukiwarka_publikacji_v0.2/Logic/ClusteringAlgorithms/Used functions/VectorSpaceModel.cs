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

            using (var dbContext = new ArticleDBDataModelContainer())
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
                _documentVector.Content = document;
                _documentVector.VectorSpace = space;
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

            string processing_log = @"F:\Magistry files\Processing_log.txt";

            using (StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + " The vector space model calculation time is: " + vector_space_model_calculation.Elapsed.Minutes.ToString() + ":" + vector_space_model_calculation.Elapsed.TotalMilliseconds.ToString());
            }

            return documentVectorSpace;
        }

    }
}
