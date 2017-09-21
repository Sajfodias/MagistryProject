using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    class VectorSpaceModel
    {
        public static HashSet<string> dTerms;
        public static List<String> documentCollection;
        private static Regex r = new Regex("([ \\t{}()\",:;. \n])");
        public static string[] removableWords = { "and", "or", "it", "at", "all", "in", "on", "under", "between", "a", "an", "the", "to", "pod", "nad", "tam", "tutaj", "między", "pomiędzy", "w", "przed", "się", "z", "na", "od", "jest", "iż", "co", "we", "ich", "ciebie", "ja", "ty", "ona", "ono", "oni", "owych", "of", "cz", "do", "s", "n", "r", "nr", "rys", "i", "by", "from", "o", "//", "**", "po", "jej", "przy", "rzecz", "jak", "tymi", "są", "czy", "oraz", "ze", "m", "p", "off", "for", "/", "is", "as", "be", "will", "go", "za", "też", "lub", "t", "poz", "wiad", "set", "use", "etc", "also", "are", "tzw", "out", "other", "its", "has", "<", ">", "pre", "its", "has", "are", "with", "[et", "]", "vol", "leszek", "j", "al" };

        public static List<DocumentVector> DocumentCollectionProcessing(List<String> collection)
        {
            dTerms = new HashSet<string>();
            documentCollection = CreateDocumentCollection.GenerateCollection();

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

            

            using(var dbContext = new ArticlesDataContainer())
            {
                var termQuerty = dbContext.Terms_Vocabulary.SqlQuery(@"SELECT * FROM Terms_Vocabulary").ToList();


                foreach (var terms in termQuerty)
                {
                    if (terms.term_value != null || terms.term_value != String.Empty)
                        dTerms.Add(terms.term_value.ToLower());
                }
            }

            List<DocumentVector> documentVectorSpace = new List<DocumentVector>();
            DocumentVector _documentVector;
            float[] space;
            foreach (string document in documentCollection)
            {
                int count = 0;
                space = new float[dTerms.Count];
                foreach (string term in dTerms){
                    space[count] = CalculateTFIDF.FindTFIDF(document, term);
                    count++;
                }
               
                _documentVector = new DocumentVector();
                _documentVector.Content = document;
                _documentVector.VectorSpace = space;
                documentVectorSpace.Add(_documentVector);

            }

            return documentVectorSpace;
        }

    }
}
