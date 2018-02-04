using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;

namespace Wyszukiwarka_publikacji_v0._2.Logic.eBase
{
    class UMKPublicationBase
    {
        public static string endText = ParserRTF.return_endText();
        public static HtmlDocument hapDoc = ParserRTF.return_hapDoc();

        #region separators
        public static char[] separators = { ';', ',', ' ' };
        public static char[] autor_separators = { ',', ';', ' '};
        public static char[] line_separator = { ':' };
        #endregion

        public static string[] UMK_number_of_articles = new string[hapDoc.DocumentNode.InnerText.Length];
        public static int UMK_articles_Count;
        public static string[] UMK_autors;
        public static string UMK_author_line;
        public static string UMK_Tytul;
        public static string UMK_Jezyk_Publikacji;
        public static string UMK_Pelny_tytul_czasop;
        public static string[] UMK_Slowa_kluczowe_j_ang;
        public static string UMK_pl_keywords_line;
        public static string[] UMK_Slowa_kluczowe_j_pl;
        public static string UMK_en_keywords_line;
        public static string UMK_Tytul_rownolegly;
        public static string UMK_Adres_URL;
        public static string UMK_Opis_wydawn;
        public static string UMK_Tytul_Wydawn_Zbior;

        public static void get_UMK_Document_content()
        {
            string[] UMK_newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            string[] UMK_separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];

            UMK_articles_Count = 0;
            string[] PP_articles_Matrix = { String.Empty };

            using (StringReader sr = new StringReader(endText))
            {
                string UMK_line;
                for (int i = 0; i <= hapDoc.DocumentNode.InnerText.Length; i++)
                {
                    UMK_line = sr.ReadLine();
                    if (UMK_line != null)
                    {
                        UMK_newcontent[i] = UMK_line;
                        UMK_separatedContent = UMK_line.Split(line_separator,2);
                        //tutaj idzie funkcjonalnosc

                        if (UMK_separatedContent.Length == 1 && (UMK_separatedContent[0].ToLower().Contains("http://") || UMK_separatedContent[0].ToLower().Contains("https://") || UMK_separatedContent[0].Contains("http://") || UMK_separatedContent[0].Contains("https://")))
                        {
                            UMK_Adres_URL = UMK_separatedContent[0];
                            //System.Windows.MessageBox.Show(UMK_Adres_URL);
                        }
                        else if (UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("aut.") || UMK_separatedContent[0].Contains("Aut.") || UMK_separatedContent[0] == "Aut."))
                        {
                            //System.Windows.MessageBox.Show(UMK_separatedContent[1]);
                            UMK_autors = UMK_separatedContent[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            UMK_author_line = UMK_separatedContent[1];
                        }
                        else if (UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].Contains("Liczba odnalezionych") || UMK_separatedContent[0] == "Liczba odnalezionych rekordow"))
                        {
                            UMK_articles_Count = Convert.ToInt32(UMK_separatedContent[1]);
                            PP_articles_Matrix = new string[UMK_articles_Count];
                            for (int z = 0; z <= UMK_articles_Count - 1; z++)
                            {
                                PP_articles_Matrix[z] = (z + 1) + ".";
                            }
                        }
                        else if (UMK_separatedContent.Length == 1 && PP_articles_Matrix.Any(x => UMK_separatedContent[0].Contains(x)))
                        {
                            if (UMK_author_line != null && UMK_Tytul != null)
                            {
                                using(var dbContext = new ArticleDBDataModelContainer())
                                {
                                    var document = new StringBuilder();
                                    var umk_article = dbContext.UMK_ArticlesSet.Create();

                                    if (UMK_author_line == null)
                                    {
                                        UMK_author_line = "Not_defined";
                                    }
                                    umk_article.article_authors_line = UMK_author_line;
                                    UMK_author_line = null;

                                    if (UMK_Tytul == null)
                                    {
                                        UMK_Tytul = "Not_defined";
                                    }
                                    umk_article.article_title = UMK_Tytul;
                                    if (UMK_Tytul != String.Empty || UMK_Tytul != " " || UMK_Tytul != null)
                                    {
                                        var termTitle_UMK = TextPreparing.TermsPrepataions(UMK_Tytul);
                                        document.Append(termTitle_UMK);
                                    }
                                    UMK_Tytul = null;

                                    if (UMK_Pelny_tytul_czasop == null)
                                    {
                                        UMK_Pelny_tytul_czasop = "Not_defined";
                                    }
                                    umk_article.article_Full_title = UMK_Pelny_tytul_czasop;
                                    if (UMK_Pelny_tytul_czasop != String.Empty || UMK_Pelny_tytul_czasop != " " || UMK_Pelny_tytul_czasop != null)
                                    {
                                        var termFullTitle_UMK = TextPreparing.TermsPrepataions(UMK_Pelny_tytul_czasop);
                                        document.Append(termFullTitle_UMK);
                                    }
                                    UMK_Pelny_tytul_czasop = null;

                                    if (UMK_Jezyk_Publikacji == null)
                                    {
                                        UMK_Jezyk_Publikacji = "Not_defined";
                                    }
                                    umk_article.article_language = UMK_Jezyk_Publikacji;
                                    UMK_Jezyk_Publikacji = null;

                                    if (UMK_Tytul_rownolegly == null)
                                    {
                                        UMK_Tytul_rownolegly = "Not_defined";
                                    }
                                    umk_article.article_translated_title = UMK_Tytul_rownolegly;
                                    if (UMK_Tytul_rownolegly != String.Empty || UMK_Tytul_rownolegly != " " || UMK_Tytul_rownolegly != null)
                                    {
                                        var termParallelTitle_UMK = TextPreparing.TermsPrepataions(UMK_Tytul_rownolegly);
                                        document.Append(termParallelTitle_UMK);
                                    }
                                    UMK_Tytul_rownolegly = null;

                                    if (UMK_en_keywords_line == null)
                                    {
                                        UMK_en_keywords_line = "Not_defined";
                                    }
                                    umk_article.article_eng_keywords = UMK_en_keywords_line;
                                    if (UMK_en_keywords_line != String.Empty || UMK_en_keywords_line != " " || UMK_en_keywords_line != null)
                                    {
                                        var term_Eng_Keywords_UMK = TextPreparing.TermsPrepataions(UMK_en_keywords_line);
                                        document.Append(term_Eng_Keywords_UMK);
                                    }
                                    UMK_en_keywords_line = null;

                                    if (UMK_pl_keywords_line == null)
                                    {
                                        UMK_pl_keywords_line = "Not_defined";
                                    }
                                    umk_article.article_pl_keywords = UMK_pl_keywords_line;
                                    if (UMK_pl_keywords_line != String.Empty || UMK_pl_keywords_line != " " || UMK_pl_keywords_line != null)
                                    {
                                        var term_PL_Keywords_UMK = TextPreparing.TermsPrepataions(UMK_pl_keywords_line);
                                        document.Append(term_PL_Keywords_UMK);
                                    }
                                    UMK_pl_keywords_line = null;

                                    if (UMK_Adres_URL == null)
                                    {
                                        UMK_Adres_URL = "Not_defined";
                                    }
                                    umk_article.article_url = UMK_Adres_URL;
                                    UMK_Adres_URL = null;

                                    if (UMK_Tytul_Wydawn_Zbior == null)
                                    {
                                        UMK_Tytul_Wydawn_Zbior = "Not_defined";
                                    }
                                    umk_article.article_publisher_title = UMK_Tytul_Wydawn_Zbior;
                                    UMK_Tytul_Wydawn_Zbior = null;

                                    if (UMK_Opis_wydawn == null)
                                    {
                                        UMK_Opis_wydawn = "Not_defined";
                                    }
                                    umk_article.article_publisher_desc = UMK_Opis_wydawn;
                                    UMK_Opis_wydawn = null;


                                    for (int k = 0; k <= UMK_autors.Length - 2;)
                                    {
                                        var authors_of_the_article = dbContext.AuthorSet.Create();
                                        authors_of_the_article.author_name = UMK_autors[k];
                                        authors_of_the_article.author_surename = UMK_autors[k + 1];
                                        umk_article.Author.Add(authors_of_the_article);
                                        k += 2;
                                    }

                                    dbContext.UMK_ArticlesSet.Add(umk_article);
                                    //dbContext.Configuration.ValidateOnSaveEnabled = false;

                                    var _document = document.ToString().Split(' ', ';', ':', ',');
                                    for (int k = 0; k <= _document.Length - 1; k++)
                                    {
                                        var terms = dbContext.Terms_Vocabulary.Create();
                                        //
                                        string dictionary_text = File.ReadAllText(@"F:\Magistry files\csv_files\Allowed_term_dictionary.csv");
                                        string[] allowed_dictionary = dictionary_text.Split(',', '\n');

                                        for (int p = 0; p <= _document.Length - 1; p++)
                                        {
                                            for (int j = 0; j <= allowed_dictionary.Length - 1; j++)
                                            {
                                                if (_document[p].Length > 3 && _document[p].Contains(allowed_dictionary[j]))
                                                {
                                                    continue;
                                                }
                                                else if (_document[p].Length <= 3 && !(_document[p].Contains(allowed_dictionary[j])))
                                                {
                                                    _document.ToList().RemoveAt(p);
                                                }

                                            }
                                        }

                                        //tutaj potrzebnie przepisac id dokumenta w ktorym wystepuje dane slowo
                                        if (_document[k] != String.Empty || _document[k] != " " || _document[k] != null || _document[k] != Char.IsDigit(' ').ToString())
                                        {
                                            //dbContext.Terms_Vocabulary.Where(u)
                                            var termVocabularyTable = dbContext.Terms_Vocabulary;
                                            terms.term_value = _document[k];

                                        }
                                        umk_article.Terms_Vocabulary.Add(terms);
                                    }

                                    try
                                    {
                                        dbContext.SaveChanges();
                                    }
                                    catch(Exception ex)
                                    {
                                        File.WriteAllText(@"F:\\Magistry files\UMK_crawler_Log.txt", ex.ToString());
                                    }
                                }
                            }
                            else
                            {
                                //System.Windows.MessageBox.Show("brak danych!");
                                continue;
                            }

                        }
                        else if (UMK_separatedContent.Length >= 2 && (UMK_separatedContent[0].ToLower().Contains("tytu") || UMK_separatedContent[0].ToLower().Contains("tytuł") || UMK_separatedContent[0].ToLower().Contains("tytul") || UMK_separatedContent[0].Contains("TYTUŁ") || UMK_separatedContent[0] == "Tytuł" || UMK_separatedContent[0] == "Tytul"))
                        {
                            UMK_Tytul = UMK_separatedContent[1];
                            //System.Windows.MessageBox.Show(UMK_Tytul);
                        }
                        else if (UMK_separatedContent.Length >= 2 && (UMK_separatedContent[0].ToLower().Contains("opis wydawn.") || UMK_separatedContent[0].ToLower().Contains("opis wydawn") || UMK_separatedContent[0].Contains("Opis wydawn.") || UMK_separatedContent[0].Contains("Opis wydawn") || UMK_separatedContent[0] == "Opis wydawn."))
                        {
                            UMK_Opis_wydawn = UMK_separatedContent[1];
                            //System.Windows.MessageBox.Show(UMK_Opis_wydawn);
                        }

                        else if (UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("język") || UMK_separatedContent[0].ToLower().Contains("jezyk") || UMK_separatedContent[0].Contains("Język") || UMK_separatedContent[0].Contains("Jezyk") || UMK_separatedContent[0] == "Język" || UMK_separatedContent[0] == "Jezyk"))
                        {
                            UMK_Jezyk_Publikacji = UMK_separatedContent[1];
                            //System.Windows.MessageBox.Show(UMK_Jezyk_Publikacji);
                        }

                        else if (UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("polskie słowa kluczowe") || UMK_separatedContent[0].ToLower().Contains("polskie slowa kluczowe") || UMK_separatedContent[0].Contains("Polskie słowa kluczowe") || UMK_separatedContent[0].Contains("Polskie slowa kluczowe") || UMK_separatedContent[0] == "Polskie słowa kluczowe" || UMK_separatedContent[0] == "Polskie slowa kluczowe"))
                        {
                            //System.Windows.MessageBox.Show(UMK_separatedContent[1]);
                            UMK_Slowa_kluczowe_j_pl = UMK_separatedContent[1].Split(separators);
                            UMK_pl_keywords_line = UMK_separatedContent[1];
                        }


                        else if (UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("tytuł wydawn. zbior.") || UMK_separatedContent[0].ToLower().Contains("tytul wydawn. zbior.") || UMK_separatedContent[0].Contains("Tytuł wydawn. zbior.") || UMK_separatedContent[0].Contains("Tytul wydawn. zbior.") || UMK_separatedContent[0] == "Tytuł wydawn. zbior." || UMK_separatedContent[0] == "Tytul wydawn. zbior."))
                        {
                            UMK_Tytul_Wydawn_Zbior = UMK_separatedContent[1];
                            //System.Windows.MessageBox.Show(UMK_Tytul_Wydawn_Zbior);
                        }

                        else if ((UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("pełny tytuł czasop.") || UMK_separatedContent[0].ToLower().Contains("pelny tytul czasop.") || UMK_separatedContent[0].Contains("Pełny tytuł czasop.") || UMK_separatedContent[0].Contains("Pelny tytul czasop.") || UMK_separatedContent[0] == "Pełny tytuł czasop." || UMK_separatedContent[0] == "Pelny tytul czasop.")))
                        {
                            UMK_Pelny_tytul_czasop = UMK_separatedContent[1];
                            //System.Windows.MessageBox.Show(UMK_Pelny_tytul_czasop);
                        }


                        else if ((UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("tytuł równoległy") || UMK_separatedContent[0].ToLower().Contains("Tytul rownolegly")  || UMK_separatedContent[0] == "Tytuł równoległy" || UMK_separatedContent[0] == "Tytul rownolegly")))
                        {
                            UMK_Tytul_rownolegly = UMK_separatedContent[1];
                            //System.Windows.MessageBox.Show(UMK_Tytul_rownolegly);
                        }


                        else if (UMK_separatedContent.Length == 2 && (UMK_separatedContent[0].ToLower().Contains("angielskie słowa kluczowe") || UMK_separatedContent[0].ToLower().Contains("angielskie slowa kluczowe") || UMK_separatedContent[0].Contains("Angielskie słowa kluczowe") || UMK_separatedContent[0].Contains("angielskie słowa kluczowe ") || UMK_separatedContent[0] == "Angielskie słowa kluczowe" || UMK_separatedContent[0] == "angielskie słowa kluczowe"))
                        {
                            //System.Windows.MessageBox.Show(UMK_separatedContent[1]);
                            UMK_Slowa_kluczowe_j_ang = UMK_separatedContent[1].Split(separators);
                            UMK_en_keywords_line = UMK_separatedContent[1];
                        }
                        //else if (UMK_separatedContent.Length == 1 && UMK_separatedContent[0] == String.Empty) System.Windows.MessageBox.Show("The empty line detected", "Empty line", System.Windows.MessageBoxButton.OK);
                        //else System.Windows.MessageBox.Show("Error! Content not found!", "Error!", System.Windows.MessageBoxButton.OK);
                    }
                }
            }
        }
    }
}
