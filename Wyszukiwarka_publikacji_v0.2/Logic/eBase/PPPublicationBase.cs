using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Objects;
using System.Text.RegularExpressions;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;

namespace Wyszukiwarka_publikacji_v0._2.Logic.eBase
{
    public class PPPublicationBase
    {
        public static string endText = ParserRTF.return_endText();
        public static HtmlDocument hapDoc = ParserRTF.return_hapDoc();

        #region separators
        public static char[] separators = { ';', ',', ' ' };
        public static char[] autor_separators = { ',', ';',' '};
        public static char[] line_separator = { ':' };
        #endregion

        public static int PP_articles_Count;
        public static string[] PP_autors;
        public static string PP_author_line;
        public static string PP_Tytul;
        public static string PP_Zrodlo;
        public static int PP_Rok;
        public static string PP_Adres_URL;
        public static string PP_Jezyk_Publikacji;
        public static string PP_Uwagi;
        public static string PP_DOI;


        public static void get_PP_Document_content()
        {
            string[] PP_newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            string[] PP_separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];

            PP_articles_Count = 0;
            string[] PP_articles_Matrix= { String.Empty };

            using (StringReader sr = new StringReader(endText))
            {
                string PP_line;
                
                for (int i = 0; i <= hapDoc.DocumentNode.InnerText.Length; i++)
                {
                    PP_line = sr.ReadLine();
                    int counter = 0;
                    if (PP_line != null)
                    {
                        PP_newcontent[i] = PP_line;
                        PP_separatedContent = PP_line.Split(line_separator,2);


                        if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].ToLower().Contains("autor") || PP_separatedContent[0].Contains("Autor") || PP_separatedContent[0] == "Autor"))
                        {
                            //System.Windows.MessageBox.Show(PP_separatedContent[1]);
                            PP_author_line = PP_separatedContent[1];
                            var PP_author_line_modified = PP_author_line.Replace("(", String.Empty);

                            PP_autors = PP_separatedContent[1].Split(autor_separators, StringSplitOptions.RemoveEmptyEntries);

                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("Liczba odnalezionych") || PP_separatedContent[0] == "Liczba odnalezionych rekordow"))
                        {
                            PP_articles_Count = Convert.ToInt32(PP_separatedContent[1]);
                            PP_articles_Matrix = new string[PP_articles_Count];
                            for (int l = 0; l <= PP_articles_Count - 1; l++)
                            {
                                PP_articles_Matrix[l] = (l + 1) + ".";
                            }
                        }
                        else if (PP_separatedContent.Length == 1 && PP_articles_Matrix.Any(x => PP_separatedContent[0].Contains(x)))
                        {
                            if (PP_author_line != null && PP_Tytul != null)
                            {
                                ///<summary>
                                ///PPArticle_Entity_Object_creation_Model_first
                                /// </summary>
                                try
                                {
                                    #region PP_Article_Object_creation_Model_First
                                    using (var PPdbContext = new ArticlesDataContainer())
                                    {
                                        var document = new StringBuilder();
                                        var pp_article = PPdbContext.PP_ArticlesSet.Create();

                                        pp_article.article_author_line = PP_author_line;
                                        PP_author_line = null;

                                        pp_article.article_title = PP_Tytul;
                                        if (PP_Tytul != String.Empty || PP_Tytul != " " || PP_Tytul != null)
                                        {
                                            var termTitlePP = TextPreparing.TermsPrepataions(PP_Tytul);
                                            document.Append(termTitlePP);
                                        }
                                        PP_Tytul = null;

                                        pp_article.article_source = PP_Zrodlo;
                                        if (PP_Zrodlo != String.Empty || PP_Zrodlo != " " || PP_Zrodlo != null)
                                        {
                                            var termSourcePP = TextPreparing.TermsPrepataions(PP_Zrodlo);
                                            document.Append(termSourcePP);
                                        }
                                        PP_Zrodlo = null;

                                        pp_article.article_year = PP_Rok;
                                        PP_Rok = 0;
                                        pp_article.article_language = PP_Jezyk_Publikacji;
                                        PP_Jezyk_Publikacji = null;
                                        pp_article.article_DOI = PP_DOI;
                                        PP_DOI = null;
                                        /*
                                        pp_article.article_details = PP_Uwagi;
                                        PP_Uwagi = null;
                                        pp_article.article_URL = PP_Adres_URL;
                                        PP_Adres_URL = null;
                                        */

                                        for (int z = 0; z <= PP_autors.Length - 4;)
                                        {
                                            var authors_of_the_PP_article = PPdbContext.AuthorSet.Create();
                                            if (PP_autors[z] != "IC)")
                                            {
                                                authors_of_the_PP_article.author_name = PP_autors[z + 1];
                                                authors_of_the_PP_article.author_surename = PP_autors[z];
                                                pp_article.Author.Add(authors_of_the_PP_article);
                                            }
                                            z += 4;
                                        }
                                        PPdbContext.PP_ArticlesSet.Add(pp_article);

                                        var _document = document.ToString().Split(' ', ';', ':', ',');
                                        for (int k = 0; k <= _document.Length - 1; k++)
                                        {
                                            var terms = PPdbContext.Terms_Vocabulary.Create();

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
                                                var termVocabularyTable = PPdbContext.Terms_Vocabulary;
                                                terms.term_value = _document[k];

                                            }
                                            pp_article.Terms_Vocabulary.Add(terms);
                                        }

                                        PPdbContext.SaveChanges();
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    File.WriteAllText(@"F:\\Magistry files\PP_crawler_Log.txt", ex.ToString());
                                }
                                ///<summary>
                                /// PPArticle_Entity_Object_Creation
                                /// </summary>
                                #region PPArticle_Entity_Object_Creation
                                /*
                                using (var dbppcontext = new PublicationsContext())
                                {
                                    var pp_article = new PPArticle();
                                    pp_article.article_author_line = PP_author_line;
                                    PP_author_line = null;
                                    pp_article.article_title = PP_Tytul;
                                    PP_Tytul = null;
                                    pp_article.article_source = PP_Zrodlo;
                                    PP_Zrodlo = null;
                                    pp_article.article_year = PP_Rok;
                                    PP_Rok = 0;
                                    pp_article.article_language = PP_Jezyk_Publikacji;
                                    PP_Jezyk_Publikacji = null;
                                    pp_article.article_DOI = PP_DOI;
                                    PP_DOI = null;
                                    pp_article.article_details = PP_Uwagi;
                                    PP_Uwagi = null;
                                    pp_article.article_URL = PP_Adres_URL;
                                    PP_Adres_URL = null;



                                    var authors_of_the_article = new Authors();
                                    for (int k = 0; k <= PP_autors.Length - 2; k++)
                                    {
                                        authors_of_the_article.author_name = PP_autors[k];
                                        authors_of_the_article.author_surename = PP_autors[k + 1];
                                        dbppcontext.Authors.Add(authors_of_the_article);

                                    }
                                    //dbppcontext.PP_Articles.Add(pp_article);
                                    dbppcontext.PP_Articles.Attach(pp_article);
                                    dbppcontext.Entry(pp_article).State = System.Data.Entity.EntityState.Added;
                                    dbppcontext.SaveChanges();
                                    //dbppcontext.SaveChanges();
                                }
                                */
                                #endregion
                            }
                            else
                            {
                                //System.Windows.MessageBox.Show("Brak danych");
                            }

                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].ToLower().Contains("tytu") || PP_separatedContent[0].ToLower().Contains("tytul") || PP_separatedContent[0].Contains("Tytul")))
                        {
                            PP_Tytul = PP_separatedContent[1];
                            //System.Windows.MessageBox.Show(PP_Tytul);
                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("Zrodlo") || PP_separatedContent[0].ToLower().Contains("zrodlo")))
                        {
                            PP_Zrodlo = PP_separatedContent[1];
                            //System.Windows.MessageBox.Show(PP_Zrodlo);
                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("Rok") || PP_separatedContent[0].ToLower().Contains("rok")))
                        {
                            var rok = PP_separatedContent[1].Substring(0, 5);
                            PP_Rok = Convert.ToInt32(rok);
                            //System.Windows.MessageBox.Show(PP_Rok.ToString());
                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("Jezyk publikacji") || PP_separatedContent[0].ToLower().Contains("jezyk publikacji") || PP_separatedContent[0].Contains("Język publikacji") || PP_separatedContent[0].ToLower().Contains("język publikacji")))
                        {
                            PP_Jezyk_Publikacji = PP_separatedContent[1];
                            //System.Windows.MessageBox.Show(PP_Jezyk_Publikacji);
                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("DOI") || PP_separatedContent[0].ToLower().Contains("doi") || PP_separatedContent[0] == "DOI"))
                        {
                            PP_DOI = PP_separatedContent[1];
                            //System.Windows.MessageBox.Show(PP_DOI);
                        }
                        /*
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("Uwagi") || PP_separatedContent[0].ToLower().Contains("uwagi") || PP_separatedContent[0] == "Uwagi"))
                        {
                            PP_Uwagi = PP_separatedContent[1];
                            System.Windows.MessageBox.Show(PP_Uwagi);
                        }
                        else if (PP_separatedContent.Length == 2 && (PP_separatedContent[0].Contains("Adres url") || PP_separatedContent[0].ToLower().Contains("adres url") || PP_separatedContent[0] == "Adres url"))
                        {
                            PP_Adres_URL = PP_separatedContent[1];
                            System.Windows.MessageBox.Show(PP_Adres_URL = PP_separatedContent[1]);
                        }
                        */

                        //else if (PP_separatedContent.Length == 1 && PP_separatedContent[0] == String.Empty) System.Windows.MessageBox.Show("The empty line detected", "Empty line", System.Windows.MessageBoxButton.OK);
                        else
                        {
                            //System.Windows.MessageBox.Show("Error! Content not found!", "Error!", System.Windows.MessageBoxButton.OK);
                            
                        }
                        counter++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// PPArticle_Entity_Creation
    /// </summary>
    #region PPArticle_Entity_Creation
    /*
    public class PPArticle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int article_Id { get; set; }

        public string article_author_line { get; set; }
        public string article_title { get; set; }
        public string article_source { get; set; }
        public int article_year { get; set; }
        public string article_language { get; set; }
        public string article_DOI { get; set;}
        public string article_details { get; set; }
        public string article_URL { get; set;}

        public int author_Id { get; set; }
        public virtual Authors Authors { get; set; }
    }
    */
    #endregion
}
