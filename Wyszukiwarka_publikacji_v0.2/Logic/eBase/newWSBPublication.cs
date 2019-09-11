using System;
using System.Linq;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;
using Wyszukiwarka_publikacji_v0._2.Models;
using System.Configuration;
using Wyszukiwarka_publikacji_v0._2.Context;
using System.Collections.Generic;

namespace Wyszukiwarka_publikacji_v0._2.Logic.eBase
{
    class newWSBPublication
    {
        public static String endText = ParserRTF.return_endText();
        public static String[] sep = { "\r" };
        public static string[] lines = endText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
        public static HtmlDocument hapDoc = ParserRTF.return_hapDoc();

        #region separators
        public static char[] separators = { ';', ',', ' ' };
        public static char[] autor_separators = { ',', ';', ' ' };
        public static char[] line_separator = { ':' };
        #endregion


        public static void get_WSB_Document_content()
        {
            var termDictionaryFilePath = ConfigurationManager.AppSettings["CsvFileDirectory"].ToString();
            var termDictionaryFile = ConfigurationManager.AppSettings["TermDictionaryFiles"].ToString();
            var termDictionaryFullFilePath = Path.Combine(termDictionaryFilePath, termDictionaryFile);

            string[] WSB_newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            string[] WSB_separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];

            int WSB_articles_Count = 0;
            string[] WSB_autors;
            string WSB_author_name = string.Empty;
            string WSB_author_surename = string.Empty;
            string WSB_author_line = string.Empty;
            string WSB_Tytul_pracy = string.Empty;
            string WSB_Adres_wydawniczy = string.Empty;
            string WSB_Tytul_calosci = string.Empty;
            string[] WSB_Slowa_kluczowe_j_ang;
            string WSB_Slowa_kluczowe_j_ang_line = string.Empty;
            string[] WSB_Slowa_kluczowe_j_pl;
            string WSB_Slowa_kluczowe_j_pl_line = string.Empty;
            string WSB_Tytul_pracy_w_innym_j = string.Empty;
            string WSB_DOI = string.Empty;
            string WSB_Szczegoly = string.Empty;
            string WSB_URL = string.Empty;

            var wsb_article = new WSBArticle();
            var terms = new TermVocabulary();
            var listOfAuthors = new List<Author>();
            var ListOfTerms = new HashSet<TermVocabulary>();

            WSB_articles_Count = 0;
            string[] WSB_articles_Matrix = { String.Empty };

            using (StringReader sr = new StringReader(endText))
            {
                int p = 0;
                string WSB_line;
                while ((WSB_line = sr.ReadLine()) != null)
                {
                    var document = new StringBuilder();

                    WSB_newcontent[p] = WSB_line;
                    WSB_separatedContent = WSB_line.Split(line_separator, 2);
                    if (WSB_separatedContent.Length == 1 & WSB_separatedContent[0] == "")
                    {
                        if(WSB_Tytul_pracy != null && WSB_Tytul_pracy != String.Empty && WSB_Tytul_pracy != "")
                        {
                            wsb_article.article_authors = WSB_author_line;
                            wsb_article.article_common_title = WSB_Tytul_calosci;
                            wsb_article.article_details = WSB_Szczegoly;
                            wsb_article.article_DOI = WSB_DOI;
                            wsb_article.article_eng_keywords = WSB_Slowa_kluczowe_j_ang_line;
                            wsb_article.article_pl_keywords = WSB_Slowa_kluczowe_j_pl_line;
                            wsb_article.article_publisher_adres = WSB_Adres_wydawniczy;
                            wsb_article.article_title = WSB_Tytul_pracy;
                            wsb_article.article_title_other_lang = WSB_Tytul_pracy_w_innym_j;
                            wsb_article.article_URL = WSB_URL;

                            using (var dbContext = new BaseDbContext())
                            {
                                var SaveChanges = false;
                                foreach(var authorItem in listOfAuthors)
                                {
                                    var authors = dbContext.Authors.Any(a => (a.author_name == authorItem.author_name) && (a.author_surename == authorItem.author_surename));
                                    if (!authors)
                                    {
                                        SaveChanges = true;
                                        dbContext.Authors.Add(authorItem);
                                    }
                                }
                                var article = dbContext.WSBArticles.Any(a => a.article_title == wsb_article.article_title);
                                if (!article)
                                {
                                    SaveChanges = true;
                                    dbContext.WSBArticles.Add(wsb_article);
                                }
                                foreach (var item in ListOfTerms)
                                {
                                    var term = dbContext.TermVocabularies.Any(t => t.term_value == item.term_value);
                                    if (!term)
                                    {
                                        SaveChanges = true;
                                        dbContext.TermVocabularies.Add(terms);
                                    }
                                }
                                if (SaveChanges)
                                {
                                    dbContext.SaveChanges();
                                }
                            }
                        }

                        wsb_article = new WSBArticle();
                        terms = new TermVocabulary();
                        listOfAuthors = new List<Author>();
                        ListOfTerms = new HashSet<TermVocabulary>();
                    }
                        //continue;
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("autor") | WSB_separatedContent[0].Contains("Autor") | WSB_separatedContent[0] == "Autorzy"))
                    {
                        
                        WSB_autors = WSB_separatedContent[1].Split(autor_separators, StringSplitOptions.RemoveEmptyEntries);
                        for (int k = 0; k <= WSB_autors.Length - 2;)
                        {
                            var authors_of_the_article = new Author();

                            if (WSB_autors[k] != null || WSB_autors[k] != String.Empty)
                                authors_of_the_article.author_name = WSB_autors[k];

                            if (WSB_autors[k+1] != null || WSB_autors[k+1] != String.Empty)
                                authors_of_the_article.author_surename = WSB_autors[k + 1];

                            listOfAuthors.Add(authors_of_the_article);
                            k += 2;
                        }
                        wsb_article.article_authors = WSB_separatedContent[1];
                    }
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("tytul pracy") | WSB_separatedContent[0].Contains("Tytul pracy") | WSB_separatedContent[0] == "Tytul pracy"))
                    {
                        if(WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Tytul_pracy = WSB_separatedContent[1];
                            document.Append(WSB_Tytul_pracy);
                        }
                    }
                        
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].Contains("Liczba odnalezionych") | WSB_separatedContent[0] == "Liczba odnalezionych rekordow"))
                    {
                        WSB_articles_Count = Convert.ToInt32(WSB_separatedContent[1]);
                        WSB_articles_Matrix = new string[WSB_articles_Count];
                        for (int z = 0; z <= WSB_articles_Count - 1; z++)
                            WSB_articles_Matrix[z] = (z + 1) + ".";
                    }
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("adres wydawniczy") | WSB_separatedContent[0].Contains("Adres wydawniczy") | WSB_separatedContent[0] == "Adres wydawniczy"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Adres_wydawniczy = WSB_separatedContent[1];
                        }
                    } 
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("polskie hasla") | WSB_separatedContent[0].Contains("Polskie hasla") | WSB_separatedContent[0] == "Polskie hasla przedmiotowe"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Slowa_kluczowe_j_pl = WSB_separatedContent[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            document.Append(WSB_Slowa_kluczowe_j_pl);
                        }
                    }
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("angielskie hasla") | WSB_separatedContent[0].Contains("Angielskie hasla") | WSB_separatedContent[0] == "Angielskie hasla przedmiotowe"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Slowa_kluczowe_j_ang = WSB_separatedContent[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            document.Append(WSB_Slowa_kluczowe_j_ang);
                        }
                    }
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("tytul calosci") | WSB_separatedContent[0].Contains("Tytul calosci") | WSB_separatedContent[0] == "Tytul calosci"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Tytul_calosci = WSB_separatedContent[1];
                            document.Append(WSB_Tytul_calosci);
                        }
                    }
                        
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("doi") | WSB_separatedContent[0].Contains("DOI") | WSB_separatedContent[0] == "DOI"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_DOI = WSB_separatedContent[1];
                        }
                    }
                        
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("tytul pracy w innym") | WSB_separatedContent[0].Contains("Tytul pracy w innym") | WSB_separatedContent[0] == "Tytul pracy w innym jezyku"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Tytul_pracy_w_innym_j = WSB_separatedContent[1];
                            document.Append(WSB_Tytul_pracy_w_innym_j);
                        }
                    }
                        
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("szczegoly") | WSB_separatedContent[0].Contains("Szczegoly") | WSB_separatedContent[0] == "Szczegoly"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_Szczegoly = WSB_separatedContent[1];
                            document.Append(WSB_Szczegoly);
                        }
                    }
                        
                    else if (WSB_separatedContent.Length == 2 & (WSB_separatedContent[0].ToLower().Contains("url") | WSB_separatedContent[0].Contains("Url") | WSB_separatedContent[0] == "Adres url"))
                    {
                        if (WSB_separatedContent[1] != null || WSB_separatedContent[1] != String.Empty)
                        {
                            WSB_URL = WSB_separatedContent[1];
                        }
                    }
                        
                    var termsArray = TextPreparing.TermsPrepataions(document.ToString()).Split(' ', ';', ':', ',');
                    if(termsArray.Count()==1 && termsArray[0] == null)
                    {
                        HashSet<string> documentHashList = new HashSet<string>();

                        for (int i = 0; i <= termsArray.Length - 1; i++)
                            documentHashList.Add(termsArray[i]);

                        var _document = documentHashList.ToArray();

                        for (int k = 0; k <= _document.Length - 1; k++)
                        {
                            string dictionary_text = File.ReadAllText(termDictionaryFullFilePath);
                            string[] allowed_dictionary = dictionary_text.Split(',', '\n');

                            for (int d = 0; d <= _document.Length - 1; d++)
                            {
                                for (int j = 0; j <= allowed_dictionary.Length - 1; j++)
                                {
                                    if (_document[d].Length > 3 & _document[d].Contains(allowed_dictionary[j]))
                                    {
                                        var term = new TermVocabulary();
                                        term.term_value = _document[d];
                                        ListOfTerms.Add(term);
                                    }
                                    else if (_document[d].Length <= 3 & !(_document[d].Contains(allowed_dictionary[j])))
                                    {
                                        _document.ToList().RemoveAt(d);
                                    }
                                }
                            }
                        }
                    }
                    p++;
                    }
                }
            }
        }
}