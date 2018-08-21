using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    public class TFIDF2ndrealization
    {
        private string document;
        private string term;
        public static HashSet<string> termCollection;
        private static Regex r = new Regex("([ \\t{}()\",:;. \n])");
        public static Dictionary<string, int> wordIndex;

        public TFIDF2ndrealization(string _document, string _term)
        {
            document = _document;
            term = _term;
        }

        public static HashSet<string> getTermCollection()
        {
            termCollection = new HashSet<string>();

            using (var dbContext = new ArticleDBDataModelContainer())
            {
                dbContext.Terms_Vocabulary.Load();

                foreach (var terms in dbContext.Terms_Vocabulary.Local)
                {
                    termCollection.Add(terms.term_value.ToLower());
                }
            }
            return termCollection;
        }

        public static Dictionary<string, int> DocumentsContainsTerm(List<string> docCollection, HashSet<string> termCollection)
        {
            wordIndex = new Dictionary<string, int>();
            foreach (var term in termCollection)
                wordIndex.Add(term, docCollection.ToArray().Where(s => r.Split(s.ToLower()).ToArray().Contains(term.ToLower())).Count());
            foreach (var item in wordIndex.Where(kvp => kvp.Value == 0).ToList())
            {
                wordIndex.Remove(item.Key);
            }
            return wordIndex;
        }

        private static float CalculateInverseDocumentFrequency(List<string> documents, string term)
        {
            int count=0;
            var enumerable = wordIndex.Where(g => g.Key == term).Select(q => q.Value);
            if (enumerable.Count() >= 1)
            {
                foreach(var item in enumerable)
                    count =+ item;
            }
            else
            {
                count = 1;
            }
            float idf_result = (float)Math.Log((float)documents.Count() / (float)count);
            if (float.IsNaN(idf_result) || count == 0)
            {
                return 0;
            }
            else
            {
                return idf_result;
            }
        }

        public static float FindTFIDF(List<string> documents, string doc, string term)
        {
            float tf = FindTermFrequency(doc, term);
            if (float.IsNaN(tf))
            {
                tf = 0;
            }
            float idf = CalculateInverseDocumentFrequency(documents, term);
            if (float.IsNaN(idf))
            {
                idf = 0;
            }
            var tfidf = tf * idf;
            if (float.IsNaN(tfidf))
            {
                return 0;
            }
            else
            {
                return tfidf;
            }
        }

        private static float FindTermFrequency(string doc, string term)
        {
            int count = r.Split(doc).Where(s => s.ToLower() == term.ToLower()).Count();
            float tf_result = (float)((float)count / (float)(r.Split(doc).Count()));
            if (float.IsNaN(tf_result) || doc.Count() == 0)
            {
                return 0;
            }
            else
            {
                return tf_result;
            }
        }

        internal static Dictionary<string, int> DocumentsContainsTermToDictionary(Dictionary<int, string> docCollectionDictionary, HashSet<string> termCollection)
        {
            wordIndex = new Dictionary<string, int>();
            foreach (var term in termCollection)
                wordIndex.Add(term, docCollectionDictionary.Values.ToArray().Where(s => r.Split(s.ToLower()).ToArray().Contains(term.ToLower())).Count());
            foreach (var item in wordIndex.Where(kvp => kvp.Value == 0).ToList())
            {
                wordIndex.Remove(item.Key);
            }
            return wordIndex;
        }
    }
}
