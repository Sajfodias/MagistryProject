﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;

namespace Wyszukiwarka_publikacji_v0._2.Logic.eBase
{
    public class UGPublicationBase
    {
        public static string endText = ParserRTF.return_endText();
        public static HtmlDocument hapDoc = ParserRTF.return_hapDoc();

        #region separators
        public static char[] separators = { ';', ',', ' ' };
        public static char[] autor_separators = { ',', ';', ' '};
        public static char[] line_separator = { ':' };
        #endregion

        public static string[] UG_autors;
        public static string UG_author_line;
        public static string UG_Tytul;
        public static string UG_Zrodlo;
        public static string[] UG_Slowa_kluczowe_j_ang;
        public static string UG_slowa_kluczowe_j_ang_line;
        public static string UG_DOI;
        public static int UG_articles_Count;

        public static void get_UG_Document_content()
        {
            string[] UG_newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            string[] UG_separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];

            UG_articles_Count = 0;
            string[] UG_articles_Matrix = { String.Empty };

            using (StringReader sr = new StringReader(endText))
            {
                int p = 0;
                string UG_line;

                while((UG_line = sr.ReadLine()) != null)
                {
                    UG_newcontent[p] = UG_line;
                    UG_separatedContent = UG_line.Split(line_separator, 2);

                    if (UG_separatedContent.Length == 1 & UG_separatedContent[0] == "")
                        continue;
                    else if (UG_separatedContent.Length == 1 && UG_articles_Matrix.Any(x => UG_separatedContent[0].Contains(x)))
                    {
                        if (UG_author_line != null && UG_Tytul != null)
                        {
                            using (var dbContext = new ArticleDBDataModelContainer())
                            {
                                var document = new StringBuilder();
                                var ug_article = dbContext.UG_ArticlesSet.Create();

                                ug_article.article_author_line = UG_author_line;
                                UG_author_line = null;

                                ug_article.article_keywords = UG_slowa_kluczowe_j_ang_line;
                                if (UG_slowa_kluczowe_j_ang_line != String.Empty || UG_slowa_kluczowe_j_ang_line != " " || UG_slowa_kluczowe_j_ang_line != null)
                                {
                                    var termEngKeywords = TextPreparing.TermsPrepataions(UG_slowa_kluczowe_j_ang_line);
                                    document.Append(termEngKeywords);
                                }
                                UG_slowa_kluczowe_j_ang_line = null;

                                ug_article.article_source = UG_Zrodlo;
                                UG_Zrodlo = null;

                                ug_article.article_title = UG_Tytul;
                                if (UG_Tytul != String.Empty || UG_Tytul != " " || UG_Tytul != null)
                                {
                                    var term_UG_Title = TextPreparing.TermsPrepataions(UG_Tytul);
                                    document.Append(term_UG_Title);
                                }
                                UG_Tytul = null;

                                ug_article.article_DOI = UG_DOI;
                                UG_DOI = null;

                                for (int k = 0; k <= UG_autors.Length - 2;)
                                {
                                    var authors_of_the_article = dbContext.AuthorSet.Create();
                                    authors_of_the_article.author_name = UG_autors[k];
                                    authors_of_the_article.author_surename = UG_autors[k + 1];
                                    ug_article.Author.Add(authors_of_the_article);
                                    k += 2;
                                }

                                dbContext.UG_ArticlesSet.Add(ug_article);

                                var _document = document.ToString().Split(' ', ';', ':', ',');
                                for (int k = 0; k <= _document.Length - 1; k++)
                                {
                                    var terms = dbContext.Terms_Vocabulary.Create();
                                    string dictionary_text = File.ReadAllText(@"F:\Magistry files\csv_files\Allowed_term_dictionary.csv");
                                    string[] allowed_dictionary = dictionary_text.Split(',', '\n');

                                    for (int d = 0; d <= _document.Length - 1; d++)
                                    {
                                        for (int j = 0; j <= allowed_dictionary.Length - 1; j++)
                                            if (_document[d].Length > 3 && _document[d].Contains(allowed_dictionary[j]))
                                                continue;
                                            else if (_document[d].Length <= 3 && !(_document[d].Contains(allowed_dictionary[j])))
                                                _document.ToList().RemoveAt(d);
                                    }

                                    //tutaj potrzebnie przepisac id dokumenta w ktorym wystepuje dane slowo
                                    if (_document[k] != String.Empty || _document[k] != " " || _document[k] != null || _document[k] != Char.IsDigit(' ').ToString() || dbContext.Terms_Vocabulary.Any(o=>o.term_value != _document[k]))
                                    {
                                        //dbContext.Terms_Vocabulary.Where(u)
                                        var termVocabularyTable = dbContext.Terms_Vocabulary;
                                        terms.term_value = _document[k];

                                    }
                                    try
                                    {
                                        ug_article.Terms_Vocabulary.Add(terms);
                                    }
                                    catch(Exception addingTermToDB)
                                    {
                                        File.WriteAllText(@"F:\\Magistry files\UG_crawler_Log.txt", DateTime.Now.ToString() + addingTermToDB.ToString());
                                    }
                                }
                                try
                                {
                                    dbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    File.WriteAllText(@"F:\\Magistry files\UG_crawler_Log.txt", DateTime.Now.ToString() + ex.ToString());
                                }
                            }
                        }
                        else
                            File.WriteAllText(@"F:\\Magistry files\UG_crawler_Log.txt", "Empty line detected." + '\n');
                    }
                    else if (UG_separatedContent.Length == 2 && (UG_separatedContent[0].Contains("Liczba odnalezionych") || UG_separatedContent[0] == "Liczba odnalezionych rekordow"))
                    {
                        UG_articles_Count = Convert.ToInt32(UG_separatedContent[1]);
                        UG_articles_Matrix = new string[UG_articles_Count];
                        for (int z = 0; z <= UG_articles_Count - 1; z++)
                            UG_articles_Matrix[z] = (z + 1) + ".";
                    }
                    else if (UG_separatedContent.Length == 2 && UG_separatedContent[0].ToLower().Contains("autorzy"))
                    {
                        UG_author_line = UG_separatedContent[1];
                        UG_autors = UG_separatedContent[1].Split(autor_separators, StringSplitOptions.RemoveEmptyEntries);

                    }
                    else if (UG_separatedContent.Length == 2 && (UG_separatedContent[0].ToLower().Contains("tytu") || UG_separatedContent[0].ToLower().Contains("tytul") || UG_separatedContent[0].Contains("TYTUL") || UG_separatedContent[0] == "TYTUL[ROZDZIALU, FRAGMENTU]" || UG_separatedContent[0].Contains("TYTUL[ROZDZIALU, FRAGMENTU]") || UG_separatedContent[0].ToLower().Contains("TYTUL[ROZDZIALU, FRAGMENTU]")))
                    {
                        UG_Tytul = UG_separatedContent[1];
                    }
                    else if (UG_separatedContent.Length == 2 && UG_separatedContent[0].ToLower().Contains("zrodlo"))
                    {
                        UG_Zrodlo = UG_separatedContent[1];
                    }
                    else if (UG_separatedContent.Length == 2 && UG_separatedContent[0].Contains("Slowa kluczowe w j. ang."))
                    {
                        UG_Slowa_kluczowe_j_ang = UG_separatedContent[1].Split(separators);
                        UG_slowa_kluczowe_j_ang_line = UG_separatedContent[1];
                    }
                    else if (UG_separatedContent.Length == 2 && (UG_separatedContent[0] == "DOI" || UG_separatedContent.Contains("DOI") || UG_separatedContent[0].ToLower().Contains("doi")))
                    {
                        UG_DOI = UG_separatedContent[1];
                    }
                    p++;
                }
                #region Old_reader_code
                // 21.08.2018 - Old version of code
                /*
                for (int i = 0; i <= hapDoc.DocumentNode.InnerText.Length; i++)
                {

                    UG_line = sr.ReadLine();
                    if (UG_line != null)
                    {
                        UG_newcontent[i] = UG_line;
                        UG_separatedContent = UG_line.Split(line_separator, 2);

                        if (UG_separatedContent.Length == 2 && UG_separatedContent[0].ToLower().Contains("autorzy"))
                        {
                            UG_author_line = UG_separatedContent[1];
                            UG_autors = UG_separatedContent[1].Split(autor_separators, StringSplitOptions.RemoveEmptyEntries);

                        }
                        else if (UG_separatedContent.Length == 2 && (UG_separatedContent[0].Contains("Liczba odnalezionych") || UG_separatedContent[0] == "Liczba odnalezionych rekordow"))
                        {
                            UG_articles_Count = Convert.ToInt32(UG_separatedContent[1]);
                            UG_articles_Matrix = new string[UG_articles_Count];
                            for (int z = 0; z <= UG_articles_Count - 1; z++)
                            {
                                UG_articles_Matrix[z] = (z + 1) + ".";
                            }
                        }
                        else if (UG_separatedContent.Length == 1 && UG_articles_Matrix.Any(x => UG_separatedContent[0].Contains(x)))
                        {
                            if (UG_author_line != null && UG_Tytul != null)
                            {
                                using(var dbContext = new ArticleDBDataModelContainer())
                                {
                                    var document = new StringBuilder();
                                    var ug_article = dbContext.UG_ArticlesSet.Create();

                                    ug_article.article_author_line = UG_author_line;
                                    UG_author_line = null;

                                    ug_article.article_keywords = UG_slowa_kluczowe_j_ang_line;
                                    if (UG_slowa_kluczowe_j_ang_line != String.Empty || UG_slowa_kluczowe_j_ang_line != " " || UG_slowa_kluczowe_j_ang_line != null)
                                    {
                                        var termEngKeywords = TextPreparing.TermsPrepataions(UG_slowa_kluczowe_j_ang_line);
                                        document.Append(termEngKeywords);
                                    }
                                    UG_slowa_kluczowe_j_ang_line = null;

                                    ug_article.article_source = UG_Zrodlo;
                                    UG_Zrodlo = null;

                                    ug_article.article_title = UG_Tytul;
                                    if (UG_Tytul != String.Empty || UG_Tytul != " " || UG_Tytul != null)
                                    {
                                        var term_UG_Title = TextPreparing.TermsPrepataions(UG_Tytul);
                                        document.Append(term_UG_Title);
                                    }
                                    UG_Tytul = null;

                                    ug_article.article_DOI = UG_DOI;
                                    UG_DOI = null;

                                    for (int k = 0; k <= UG_autors.Length - 2;)
                                    {
                                        var authors_of_the_article = dbContext.AuthorSet.Create();
                                        authors_of_the_article.author_name = UG_autors[k];
                                        authors_of_the_article.author_surename = UG_autors[k + 1];
                                        ug_article.Author.Add(authors_of_the_article);
                                        k += 2;
                                    }
                                    dbContext.UG_ArticlesSet.Add(ug_article);
                                    var _document = document.ToString().Split(' ', ';', ':', ',');
                                    for (int k = 0; k <= _document.Length - 1; k++)
                                    {
                                        var terms = dbContext.Terms_Vocabulary.Create();
                                        string dictionary_text = File.ReadAllText(@"F:\Magistry files\csv_files\Allowed_term_dictionary.csv");
                                        string[] allowed_dictionary = dictionary_text.Split(',', '\n');

                                        for (int d = 0; d <= _document.Length - 1; d++)
                                        {
                                            for (int j = 0; j <= allowed_dictionary.Length - 1; j++)
                                                if (_document[d].Length > 3 && _document[d].Contains(allowed_dictionary[j]))
                                                    continue;
                                                else if (_document[d].Length <= 3 && !(_document[d].Contains(allowed_dictionary[j])))
                                                    _document.ToList().RemoveAt(d);
                                        }

                                        //tutaj potrzebnie przepisac id dokumenta w ktorym wystepuje dane slowo
                                        if (_document[k] != String.Empty || _document[k] != " " || _document[k] != null || _document[k] != Char.IsDigit(' ').ToString())
                                        {
                                            //dbContext.Terms_Vocabulary.Where(u)
                                            var termVocabularyTable = dbContext.Terms_Vocabulary;
                                            terms.term_value = _document[k];

                                        }
                                        ug_article.Terms_Vocabulary.Add(terms);
                                    }
                                    try
                                    {
                                        dbContext.SaveChanges();
                                    }
                                    catch(Exception ex)
                                    {
                                        File.WriteAllText(@"F:\\Magistry files\UG_crawler_Log.txt", ex.ToString());
                                    }
                                }

                                ///<summary>
                                /// UGArticle_Entity_Object_Creation
                                /// </summary>
                                #region UGArticle_Entity_Object_Creation
                                using (var db = new PublicationsContext())
                                {
                                    var ug_article = new UGArticle();
                                    ug_article.article_author_line = UG_author_line;
                                    UG_author_line = null;
                                    ug_article.article_keywords = UG_slowa_kluczowe_j_ang_line;
                                    UG_slowa_kluczowe_j_ang_line = null;
                                    ug_article.article_source = UG_Zrodlo;
                                    UG_Zrodlo = null;
                                    ug_article.article_title = UG_Tytul;
                                    UG_Tytul = null;
                                    ug_article.article_DOI = UG_DOI;
                                    UG_DOI = null;

                                    var authors_of_the_article = new Authors();
                                    for (int k = 0; k <= UG_autors.Length - 2; k++)
                                    {
                                        authors_of_the_article.author_name = UG_autors[k];
                                        authors_of_the_article.author_surename = UG_autors[k + 1];
                                        authors_of_the_article.article_Id = ug_article.article_Id;
                                        
                                        db.Authors.Add(authors_of_the_article);
                                    }

                                    //authors_of_the_article.UG_Articles.Add(ug_article);
                                    db.UG_Articles.Add(ug_article);
                                    db.SaveChanges();
                                }
                            else
                            {
                                File.WriteAllText(@"F:\\Magistry files\UG_crawler_Log.txt", "Empty line detected."+'\n');
                            }
                        }
                        else if (UG_separatedContent.Length == 2 && (UG_separatedContent[0].ToLower().Contains("tytu") || UG_separatedContent[0].ToLower().Contains("tytul") || UG_separatedContent[0].Contains("TYTUL") || UG_separatedContent[0]=="TYTUL[ROZDZIALU, FRAGMENTU]" || UG_separatedContent[0].Contains("TYTUL[ROZDZIALU, FRAGMENTU]") || UG_separatedContent[0].ToLower().Contains("TYTUL[ROZDZIALU, FRAGMENTU]")))
                        {
                            UG_Tytul = UG_separatedContent[1];
                        }
                        else if (UG_separatedContent.Length == 2 && UG_separatedContent[0].ToLower().Contains("zrodlo")){
                            UG_Zrodlo = UG_separatedContent[1];
                        }
                        else if (UG_separatedContent.Length == 2 && UG_separatedContent[0].Contains("Slowa kluczowe w j. ang."))
                        {
                            UG_Slowa_kluczowe_j_ang = UG_separatedContent[1].Split(separators);
                            UG_slowa_kluczowe_j_ang_line = UG_separatedContent[1];
                        }
                        else if (UG_separatedContent.Length == 2 && (UG_separatedContent[0] == "DOI" || UG_separatedContent.Contains("DOI") || UG_separatedContent[0].ToLower().Contains("doi")))
                        {
                            UG_DOI = UG_separatedContent[1];
                        }
                    }
                }
                */
                #endregion
            }
        }
    }
}
