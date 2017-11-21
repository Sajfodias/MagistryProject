using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class CalculateTFIDF
    {
        private string document;
        private string term;
        private static Regex r = new Regex("([ \\t{}()\",:;. \n])");

        public CalculateTFIDF(string _document, string _term)
        {
            document = _document;
            term = _term;
        }

        public static float FindTFIDF(List<string> documents,string doc, string term)
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

        private static float CalculateInverseDocumentFrequency(List<string> documents ,string term)
        {
            

            int count = documents.ToArray().Where(s => r.Split(s.ToLower()).ToArray().Contains(term.ToLower())).Count();
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

        private static float FindTermFrequency(string doc, string term)
        {
            int count = r.Split(doc).Where(s => s.ToLower() == term.ToLower()).Count();
            float tf_result = (float)((float)count / (float)(r.Split(doc).Count()));
            if(float.IsNaN(tf_result) || doc.Count() == 0)
            {
                return 0;
            }
            else
            {
                return tf_result;
            }
        }

    }
}
