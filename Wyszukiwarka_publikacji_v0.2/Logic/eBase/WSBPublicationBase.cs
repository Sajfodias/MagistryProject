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
    class WSBPublicationBase
    {
        public static string endText = ParserRTF.return_endText();
        public static HtmlDocument hapDoc = ParserRTF.return_hapDoc();

        #region separators
        public static char[] separators = { ';', ',', ' ' };
        public static char[] autor_separators = { ',', ';', ' ' };
        public static char[] line_separator = { ':' };
        #endregion

        public static int WSB_articles_Count;
        public static string[] WSB_autors;
        public static string WSB_author_line;
        public static string WSB_Tytul_pracy;
        public static string WSB_Adres_wydawniczy;
        public static string WSB_Tytul_calosci;
        public static string[] WSB_Slowa_kluczowe_j_ang;
        public static string WSB_Slowa_kluczowe_j_ang_line;
        public static string[] WSB_Slowa_kluczowe_j_pl;
        public static string WSB_Slowa_kluczowe_j_pl_line;
        public static string WSB_Tytul_pracy_w_innym_j;
        public static string WSB_DOI;
        public static string WSB_Szczegoly;
        public static string WSB_URL;

        //potrzebnie zaimplementowac divide and conquer dla duzych plikow


        public static void get_WSB_Document_content()
        {
            string[] WSB_newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            string[] WSB_separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];

            WSB_articles_Count = 0;
            string[] WSB_articles_Matrix = { String.Empty };

            using (StringReader sr = new StringReader(endText))
            {
                string WSB_line;
                for (int i = 0; i <= hapDoc.DocumentNode.InnerText.Length; i++)
                {
                    WSB_line = sr.ReadLine();
                    if (WSB_line != null)
                    {
                        WSB_newcontent[i] = WSB_line;
                        WSB_separatedContent = WSB_line.Split(line_separator, 2);


                        if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("autor") || WSB_separatedContent[0].Contains("Autor") || WSB_separatedContent[0] == "Autorzy"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_autors = WSB_separatedContent[1].Split(autor_separators, StringSplitOptions.RemoveEmptyEntries);
                            WSB_author_line = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].Contains("Liczba odnalezionych") || WSB_separatedContent[0] == "Liczba odnalezionych rekordow"))
                        {
                            WSB_articles_Count = Convert.ToInt32(WSB_separatedContent[1]);
                            WSB_articles_Matrix = new string[WSB_articles_Count];
                            for (int z = 0; z <= WSB_articles_Count - 1; z++)
                            {
                                WSB_articles_Matrix[z] = (z + 1) + ".";
                            }
                        }

                        else if (WSB_separatedContent.Length == 1 && WSB_articles_Matrix.Any(x => WSB_separatedContent[0].Contains(x)))
                        {
                            if (WSB_author_line != null && WSB_Tytul_pracy != null)
                            {
                                using(var dbContext = new ArticlesDataContainer())
                                {
                                    var document = new StringBuilder();
                                    var wsb_article = dbContext.WSB_ArticlesSet.Create();

                                    if (WSB_author_line == null)
                                    {
                                        WSB_author_line = "Not_defined";
                                    }
                                    wsb_article.article_authors = WSB_author_line;
                                    WSB_author_line = null;

                                    if (WSB_Tytul_pracy == null)
                                    {
                                        WSB_Tytul_pracy = "Not_defined";
                                    }
                                    wsb_article.article_title = WSB_Tytul_pracy;
                                    if (WSB_Tytul_pracy != String.Empty || WSB_Tytul_pracy != " " || WSB_Tytul_pracy != null)
                                    {
                                        var termTitle_WSB = TextPreparing.TermsPrepataions(WSB_Tytul_pracy);
                                        document.Append(termTitle_WSB);
                                    }
                                    WSB_Tytul_pracy = null;

                                    if (WSB_Adres_wydawniczy == null)
                                    {
                                        WSB_Adres_wydawniczy = "Not_defined";
                                    }
                                    wsb_article.article_publisher_adres = WSB_Adres_wydawniczy;
                                    WSB_Adres_wydawniczy = null;

                                    if (WSB_Tytul_calosci == null)
                                    {
                                        WSB_Tytul_calosci = "Not_defined";
                                    }
                                    wsb_article.article_common_title = WSB_Tytul_calosci;
                                    if (WSB_Tytul_calosci != String.Empty || WSB_Tytul_calosci != " " || WSB_Tytul_calosci != null)
                                    {
                                        var termFullTitle_WSB = TextPreparing.TermsPrepataions(WSB_Tytul_calosci);
                                        document.Append(termFullTitle_WSB);
                                    }
                                    WSB_Tytul_calosci = null;

                                    if (WSB_Slowa_kluczowe_j_pl_line == null)
                                    {
                                        WSB_Slowa_kluczowe_j_pl_line = "Not_defined";
                                    }
                                    wsb_article.article_pl_keywords = WSB_Slowa_kluczowe_j_pl_line;
                                    if (WSB_Slowa_kluczowe_j_pl_line != String.Empty || WSB_Slowa_kluczowe_j_pl_line != " " || WSB_Slowa_kluczowe_j_pl_line != null)
                                    {
                                        var term_PL_Keywords_WSB = TextPreparing.TermsPrepataions(WSB_Slowa_kluczowe_j_pl_line);
                                        document.Append(term_PL_Keywords_WSB);
                                    }
                                    WSB_Slowa_kluczowe_j_pl_line = null;

                                    if (WSB_Slowa_kluczowe_j_ang_line == null)
                                    {
                                        WSB_Slowa_kluczowe_j_ang_line = "Not_defined";
                                    }
                                    wsb_article.article_eng_keywords = WSB_Slowa_kluczowe_j_ang_line;
                                    if (WSB_Slowa_kluczowe_j_ang_line != String.Empty || WSB_Slowa_kluczowe_j_ang_line != " " || WSB_Slowa_kluczowe_j_ang_line != null)
                                    {
                                        var term_Eng_Keywords_WSB = TextPreparing.TermsPrepataions(WSB_Slowa_kluczowe_j_ang_line);
                                        document.Append(term_Eng_Keywords_WSB);
                                    }
                                    WSB_Slowa_kluczowe_j_ang_line = null;

                                    if (WSB_Tytul_pracy_w_innym_j == null)
                                    {
                                        WSB_Tytul_pracy_w_innym_j = "Not_defined";
                                    }
                                    wsb_article.article_title_other_lang = WSB_Tytul_pracy_w_innym_j;
                                    if (WSB_Tytul_pracy_w_innym_j != String.Empty || WSB_Tytul_pracy_w_innym_j != " " || WSB_Tytul_pracy_w_innym_j != null)
                                    {
                                        var term_Title_Other_Lang_WSB = TextPreparing.TermsPrepataions(WSB_Tytul_pracy_w_innym_j);
                                        document.Append(term_Title_Other_Lang_WSB);
                                    }
                                    WSB_Tytul_pracy_w_innym_j = null;

                                    if (WSB_Szczegoly == null)
                                    {
                                        WSB_Szczegoly = "Not_defined";
                                    }
                                    wsb_article.article_details = WSB_Szczegoly;
                                    WSB_Szczegoly = null;

                                    if (WSB_URL == null)
                                    {
                                        WSB_URL = "Not_defined";
                                    }
                                    wsb_article.article_URL = WSB_URL;
                                    WSB_URL = null;

                                    if (WSB_DOI == null)
                                    {
                                        WSB_DOI = "Not_defined";
                                    }
                                    wsb_article.article_DOI = WSB_DOI;
                                    WSB_DOI = null;


                                    for (int k = 0; k <= WSB_autors.Length - 2;)
                                    {
                                        var authors_of_the_article = dbContext.AuthorSet.Create();
                                        authors_of_the_article.author_name = WSB_autors[k];
                                        authors_of_the_article.author_surename = WSB_autors[k + 1];
                                        wsb_article.Author.Add(authors_of_the_article);
                                        k += 2;
                                    }

                                    dbContext.WSB_ArticlesSet.Add(wsb_article);

                                    var _document = document.ToString().Split(' ', ';', ':', ',');
                                    for (int k = 0; k <= _document.Length - 1; k++)
                                    {
                                        var terms = dbContext.Terms_Vocabulary.Create();
                                        //tutaj potrzebnie przepisac id dokumenta w ktorym wystepuje dane slowo
                                        if (_document[k] != String.Empty || _document[k] != " " || _document[k] != null || _document[k] != Char.IsDigit(' ').ToString())
                                        {
                                            //dbContext.Terms_Vocabulary.Where(u)
                                            var termVocabularyTable = dbContext.Terms_Vocabulary;
                                            terms.term_value = _document[k];

                                        }
                                        wsb_article.Terms_Vocabulary.Add(terms);
                                    }

                                    dbContext.SaveChanges();
                                }

                                ///<summary>
                                /// Entity Code-first object creation WSBArticle
                                /// </summary>
                                #region WSB_Article_Entity_Object_Creation
                                /*
                                using (var db = new PublicationsContext())
                                {
                                    var wsb_article = new WSBArticle();
                                    wsb_article.article_authors = WSB_author_line;
                                    WSB_author_line = null;
                                    wsb_article.article_title = WSB_Tytul_pracy;
                                    WSB_Tytul_pracy = null;
                                    wsb_article.article_publisher_adres = WSB_Adres_wydawniczy;
                                    WSB_Adres_wydawniczy = null;
                                    wsb_article.article_common_title = WSB_Tytul_calosci;
                                    WSB_Tytul_calosci = null;
                                    wsb_article.article_pl_keywords = WSB_Slowa_kluczowe_j_pl_line;
                                    WSB_Slowa_kluczowe_j_pl_line = null;
                                    wsb_article.article_eng_keywords = WSB_Slowa_kluczowe_j_ang_line;
                                    WSB_Slowa_kluczowe_j_ang_line = null;
                                    wsb_article.article_title_other_lang = WSB_Tytul_pracy_w_innym_j;
                                    WSB_Tytul_pracy_w_innym_j = null;
                                    wsb_article.article_details = WSB_Szczegoly;
                                    WSB_Szczegoly = null;
                                    wsb_article.article_URL = WSB_URL;
                                    WSB_URL = null;
                                    wsb_article.article_DOI = WSB_DOI;
                                    WSB_DOI = null;

                                    var authors_of_the_article = new Authors();
                                    for (int k = 0; k <= WSB_autors.Length - 2; k++)
                                    {
                                        authors_of_the_article.author_name = WSB_autors[k];
                                        authors_of_the_article.author_surename = WSB_autors[k + 1];
                                        db.Authors.Add(authors_of_the_article);
                                    }


                                    db.WSB_Articles.Add(wsb_article);
                                    db.SaveChanges();
                                }
                                */
                                #endregion
                            }

                            else
                            {
                                //return;
                                //System.Windows.MessageBox.Show("brak danych!");
                                File.WriteAllText(@"F:\\Magistry files\WSB_emptyLines.txt", "empty_line");
                            }
                            
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("tytul pracy") || WSB_separatedContent[0].Contains("Tytul pracy") || WSB_separatedContent[0] == "Tytul pracy"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Tytul_pracy = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("adres wydawniczy") || WSB_separatedContent[0].Contains("Adres wydawniczy") || WSB_separatedContent[0] == "Adres wydawniczy"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Adres_wydawniczy = WSB_separatedContent[1];
                        }


                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("polskie hasla") || WSB_separatedContent[0].Contains("Polskie hasla") || WSB_separatedContent[0] == "Polskie hasla przedmiotowe"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Slowa_kluczowe_j_pl = WSB_separatedContent[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            WSB_Slowa_kluczowe_j_pl_line = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("angielskie hasla") || WSB_separatedContent[0].Contains("Angielskie hasla") || WSB_separatedContent[0] == "Angielskie hasla przedmiotowe"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Slowa_kluczowe_j_ang = WSB_separatedContent[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            WSB_Slowa_kluczowe_j_ang_line = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("tytul calosci") || WSB_separatedContent[0].Contains("Tytul calosci") || WSB_separatedContent[0] == "Tytul calosci"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Tytul_calosci = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("doi") || WSB_separatedContent[0].Contains("DOI") || WSB_separatedContent[0] == "DOI"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_DOI = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("tytul pracy w innym") || WSB_separatedContent[0].Contains("Tytul pracy w innym") || WSB_separatedContent[0] == "Tytul pracy w innym jezyku"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Tytul_pracy_w_innym_j = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("szczegoly") || WSB_separatedContent[0].Contains("Szczegoly") || WSB_separatedContent[0] == "Szczegoly"))
                        {
                           //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_Szczegoly = WSB_separatedContent[1];
                        }

                        else if (WSB_separatedContent.Length == 2 && (WSB_separatedContent[0].ToLower().Contains("url") || WSB_separatedContent[0].Contains("Url") || WSB_separatedContent[0] == "Adres url"))
                        {
                            //System.Windows.MessageBox.Show(WSB_separatedContent[1]);
                            WSB_URL = WSB_separatedContent[1];
                        }

                        //else if (PP_separatedContent.Length == 1 && PP_separatedContent[0] == String.Empty) System.Windows.MessageBox.Show("The empty line detected", "Empty line", System.Windows.MessageBoxButton.OK);
                        //else System.Windows.MessageBox.Show("Error! Content not found!", "Error!", System.Windows.MessageBoxButton.OK);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Entity Code-first class WSBArticle
    /// </summary>
    /*
    public class WSBArticle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int article_Id { get; set; }

        public string article_authors { get; set; }
        public string article_title { get; set; }
        public string article_publisher_adres { get; set; }
        public string article_common_title { get; set; }
        public string article_eng_keywords { get; set; }
        public string article_pl_keywords { get; set; }
        public string article_title_other_lang { get; set; }
        public string article_DOI { get; set; }
        public string article_details { get; set; }
        public string article_URL { get; set; }

        public int author_Id { get; set; }
        public virtual Authors Authors { get; set; }
    }
    */
}
