using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibTeXLibrary;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    class BibtexParser
    {
        private static string filePathBibtex = @"F:\\Magistry files\new_Magistry_test_data\";
        public static DirectoryInfo rtfFilesDirectory = new DirectoryInfo(filePathBibtex);
        public static FileInfo[] files = rtfFilesDirectory.GetFiles();
        //public static string[] fileEntries = Directory.GetFiles(filePathBibtex);
        public object fileBibtex = filePathBibtex;
        public object nullobject = System.Reflection.Missing.Value;
        public static string[] context;
        public static string[] separatedContext;
        public static char[] separators = { '=' };
        public static char[] authorSeparator = { ' ', ',', ';' };
        


        public static string _title;
        public static string _abstract;
        public static string _keywords;
        public static int _year;
        public static string _country;
        public static string[] _authors;
        public static string _authorsLine;
        public static string _organization;
        public static string _url;

        public static void LoadBibtexFile()
        {
            string[] fileEntries = Directory.GetFiles(filePathBibtex);
            char[] not_allowedCharsforArticle = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '<', '>', 'x', '!', '#', '$', '%', '^', '&', '*', '(', ')', '/', '\'' };
            string[] new_document = new string[0];

            foreach (string file in fileEntries)
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(file)))
                {
                    if (reader.ToString() != null || !reader.ToString().Contains("title ="))
                    {
                        context = new string[14];
                        separatedContext = new string[2];


                        for (int i = 0; i <= context.Count() - 1; i++)
                        {
                            context[i] = reader.ReadLine();
                            if (context[i] != null || context[i] == "}")
                            {
                                try
                                {
                                    Console.WriteLine("Processing " + i.ToString() + " line.");
                                    context[i] = context[i].TrimStart(' ').Replace('\"', ' ').Replace('\\', ' ').TrimEnd(',');
                                    separatedContext = context[i].Split(separators, 2, StringSplitOptions.RemoveEmptyEntries);

                                    #region getVariables
                                    if (separatedContext[0].Contains("title"))
                                    {
                                        #region little_modification_for_title_clearing
                                        /*
                                        for (int a = 0; a < separatedContext[1].Length; a++)
                                        {
                                            for (int b = 0; b < not_allowedCharsforArticle.Length; b++)
                                            {
                                                if (separatedContext[1].ElementAt(a) == not_allowedCharsforArticle[b])
                                                    separatedContext[1].Remove(a, 1);
                                            }
                                        }
                                        */
                                        #endregion
                                        if (separatedContext[1].Length >= 2)
                                            _title = separatedContext[1];
                                    }
                                    else if (separatedContext[0].Contains("abstract"))
                                    {
                                        #region little_modification_for_abstract_clearing
                                        for (int a = 0; a < separatedContext[1].Length; a++)
                                        {
                                            for (int b = 0; b < not_allowedCharsforArticle.Length; b++)
                                            {
                                                if (separatedContext[1].ElementAt(a) == not_allowedCharsforArticle[b])
                                                    separatedContext[1].Remove(a, 1);
                                            }
                                        }
                                        #endregion
                                        if (separatedContext[1].Length >= 5)
                                            _abstract = separatedContext[1];
                                    }
                                    else if (separatedContext[0].Contains("keywords"))
                                    {
                                        if (separatedContext[1] != String.Empty || separatedContext[1] != " ")
                                            _keywords = separatedContext[1];
                                        else continue;
                                    }
                                    else if (separatedContext[0].Contains("year"))
                                    {
                                        if (Convert.ToInt32(separatedContext[1]) >= 1960)
                                            _year = Convert.ToInt32(separatedContext[1]);
                                        else continue;
                                    }
                                    else if (separatedContext[0].Contains("country"))
                                    {
                                        _country = separatedContext[1];
                                    }
                                    else if (separatedContext[0].Contains("author"))
                                    {
                                        _authorsLine = separatedContext[1];
                                        _authors = separatedContext[1].Split(authorSeparator, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    else if (separatedContext[0].Contains("organization"))
                                    {
                                        _organization = separatedContext[1];
                                    }
                                    else if (separatedContext[0].Contains("url"))
                                    {
                                        _url = separatedContext[1];
                                    }
                                    else continue;
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    if (ex.InnerException.GetType() == typeof(IndexOutOfRangeException))
                                    {
                                        File.WriteAllText(@"F:\\Magistry files\PG_crawler_Log.txt", ex.ToString());
                                        return;
                                    }
                                }

                            }
                        }

                    }
                }
                #region bibtexLibrary
                /*
                if(reader.ToString() != null)
                {
                    string fileEntry = reader.ReadToEnd();
                    string fileEntry_filter1 = fileEntry.Replace('*', ' ');
                    //string fileEntry_filter2 = fileEntry_filter1.Replace('{', ' ');
                   // string fileEntry_filter3 = fileEntry_filter2.Replace('}', ' ');
                    string fileEntry_filter2 = fileEntry_filter1.Replace('/', ' ');
                    if (fileEntry_filter2!=String.Empty && fileEntry_filter2.Contains("title = ") && fileEntry_filter2 != null)
                    {
                        BibTeXLibrary.BibParser parser = new BibParser(new StringReader(fileEntry));
                        var entry = parser.GetAllResult()[0];
                        if(!entry.ToString().Contains("publication100010"))
                        {
                            Console.WriteLine(entry["title"]);
                            Console.WriteLine(entry["abstract"]);
                            Console.WriteLine(entry["keywords"]);
                            Console.WriteLine(entry["year"]);
                            Console.WriteLine(entry["author"]);
                            Console.WriteLine(entry["organization"]);
                            Console.WriteLine(entry["url"]);
                        }
                        else
                        {
                            file.Skip(1);
                        }
                    }
                    else if (fileEntry_filter2 == String.Empty || !fileEntry_filter2.Contains("title = ") || fileEntry_filter2 == null)
                    {
                        file.Skip(1);
                    }
                    else{
                        Console.WriteLine("Error!");
                        return;
                    }
                    */
                #endregion
                try
                {
                    #region Bibtex_Entity_Object_Creation_Model_First
                    //
                    using (var dbContext = new ArticleDBDataModelContainer())
                    {
                        var document = new StringBuilder();

                        var bibtexArticle = dbContext.PG_ArticlesSet.Create();

                        bibtexArticle.title = _title;
                        if (_title != String.Empty || _title != " " || _title != null)
                        {
                            var termTitle = TextPreparing.TermsPrepataions(_title);
                            document.Append(termTitle);
                        }
                        _title = null;

                        bibtexArticle.abstractText = _abstract;
                        if (_abstract != String.Empty || _abstract != " " || _abstract != null)
                        {
                            var termAbstract = TextPreparing.TermsPrepataions(_abstract);
                            document.Append(termAbstract);
                        }
                        _abstract = null;

                        bibtexArticle.keywords = _keywords;
                        if (_keywords != String.Empty || _keywords != " " || _keywords != null)
                        {
                            var termKeywords = TextPreparing.TermsPrepataions(_keywords);
                            document.Append(termKeywords);
                        }
                        _keywords = null;
                        bibtexArticle.year = _year;
                        bibtexArticle.country = _country;
                        _country = null;
                        bibtexArticle.authors = _authorsLine;
                        _authorsLine = null;
                        //potrzebnie dorobic dodawanie autorow po 2 wartosci z tabeli authors[] do klasy Entity Authors
                        bibtexArticle.organizations = _organization;
                        _organization = null;
                        bibtexArticle.url = _url;
                        _url = null;


                        for (int i = 0; i <= _authors.Length - 2;)
                        {
                            var authors_of_the_article = dbContext.AuthorSet.Create();
                            authors_of_the_article.author_name = _authors[i];
                            authors_of_the_article.author_surename = _authors[i + 1];
                            bibtexArticle.Author.Add(authors_of_the_article);
                            i += 2;
                        }

                        dbContext.PG_ArticlesSet.Add(bibtexArticle);

                        var _document = document.ToString().Split(' ', ';', ':', ',');

                        
                        //dodano 11.02
                        for (int p = 0; p < _document.Length; p++)
                        {
                            for (int z = 0; z < not_allowedCharsforArticle.Length; z++)
                            {
                                if (_document[p].Contains(not_allowedCharsforArticle[z]))
                                    _document[p].Remove(z, 1);
                            }

                            //dodano 11.02
                            List<string> stringHashSet = new List<string>();
                            stringHashSet = _document.ToList();

                            foreach (var element in stringHashSet)
                            {
                                if (element == String.Empty || element == null || element == " ")
                                    stringHashSet.Remove(element);
                                else if (element.Length <= 3)
                                    stringHashSet.Remove(element);
                            }

                            new_document = stringHashSet.ToArray();
                        }

                        for (int k = 0; k <= new_document.Length - 1; k++)
                        {
                            var terms = dbContext.Terms_Vocabulary.Create();

                            string dictionary_text = File.ReadAllText(@"F:\Magistry files\csv_files\Allowed_term_dictionary.csv");
                            string[] allowed_dictionary = dictionary_text.Split(',', '\n');
                            #region old_cleaning_code_11.02.2018
                            //added 10.02.2018 - cleaninig the article list
                            /*
                            for (int i = 0; i <= new_document.Length - 1; i++)
                            {
                                for (int j = 0; j <= allowed_dictionary.Length - 1; j++)
                                {
                                    if (new_document[i].Length > 3 && new_document[i].Contains(allowed_dictionary[j]))
                                    {
                                        continue;
                                    }
                                    else if (new_document[i].Length < 3 && !(new_document[i].Contains(allowed_dictionary[j])))
                                    {
                                        new_document.ToList().RemoveAt(i);
                                    }
                                }
                            }
                            */
                            #endregion
                            #region old_version_11.02.2018
                            //tutaj potrzebnie przepisac id dokumenta w ktorym wystepuje dane slowo
                            //if (new_document[k] != String.Empty || new_document[k] != " " || new_document[k] != null || new_document[k] != Char.IsDigit(' ').ToString())
                            //{
                            //dbContext.Terms_Vocabulary.Where(u)
                            #endregion
                            var termVocabularyTable = dbContext.Terms_Vocabulary;
                                terms.term_value = new_document[k];
                            //}
                            bibtexArticle.Terms_Vocabulary.Add(terms);
                        }
                        dbContext.SaveChanges();
                    }
                    #endregion

                    ///<summary>
                    /// BibtexArticle_Entity_Object_Creation
                    /// </summary>
                    #region BibtexArticle_Entity_Object_Creation
                    /*
                    using (var db = new PublicationsContext())
                    {
                        var bibtexArticle = new BibtexArticle();
                        bibtexArticle.title = _title;
                        _title = null;
                        bibtexArticle.abstractText = _abstract;
                        _abstract = null;
                        bibtexArticle.keywords = _keywords;
                        _keywords = null;
                        bibtexArticle.year = _year;
                        bibtexArticle.country = _country;
                        _country = null;
                        bibtexArticle.authors = _authorsLine;
                        _authorsLine = null;
                        //potrzebnie dorobic dodawanie autorow po 2 wartosci z tabeli authors[] do klasy Entity Authors
                        bibtexArticle.organizations = _organization;
                        _organization = null;
                        bibtexArticle.url = _url;
                        _url = null;


                        var authors_of_the_article = new Authors();
                        for (int i = 0; i <= _authors.Length - 2; i++)
                        {
                            authors_of_the_article.author_name = _authors[i];
                            authors_of_the_article.author_surename = _authors[i + 1];
                            bibtexArticle.author_Id = authors_of_the_article.author_Id;
                            db.Authors.Add(authors_of_the_article);
                        }

                        db.PG_Articles.Add(bibtexArticle);
                        db.SaveChanges();
                    }
                    */
                    #endregion
                    Console.WriteLine("End of file! Go to the next ->");
                }
                catch (Exception ex)
                {
                    File.WriteAllText(@"F:\\Magistry files\PG_crawler_Log.txt", ex.ToString());
                }
            }
        }
    }

    /// <summary>
    /// BibtexArticle_Entity_Creation
    /// </summary>
    #region BibtexArticle_Entity_Creation
    /*
    public class BibtexArticle {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int article_Id { get; set; }

        public string title { get; set; }
        public string abstractText { get; set; }
        public string keywords { get; set; }
        public int year { get; set; }
        public string country { get; set; }
        public string authors { get; set; }
        public string organizations { get; set; }
        public string url { get; set; }

        public int author_Id { get; set; }
        public virtual Authors Authors { get; set; }
    }
    */
    #endregion
}


