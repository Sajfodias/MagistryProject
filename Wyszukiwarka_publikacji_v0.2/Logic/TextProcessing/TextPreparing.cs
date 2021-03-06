﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibTeXLibrary;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Wyszukiwarka_publikacji_v0._2.Logic.eBase;
using SF.Snowball.Ext;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing
{
    class TextPreparing
    {
        public static string TermsPrepataions(string _text)
        {
            ArticleDBDataModelContainer dbContainer = new ArticleDBDataModelContainer();
            List<string> resultDBList = new List<string>();
            var TermsDBList = dbContainer.Set<Terms_Vocabulary>();
            foreach(var element in TermsDBList)
            {
                resultDBList.Add(element.term_value);
            }
            //here we will have the list of terms



            var text_preparation = Stopwatch.StartNew();
            string text = _text;
            List<string> Words = new List<string>();
            char[] splitChars = { ' ', ',', '.', ';', '-', ':' };
            string[] removableWords = { "and", "or", "it", "at", "all", "in", "on", "under", "between", "a", "an", "the", "to", "pod", "nad", "tam", "tutaj", "między", "pomiędzy", "w", "przed", "się", "z", "na", "od", "jest", "iż", "co", "we", "ich", "ciebie", "ja", "ty", "ona", "ono", "oni", "owych", "of", "cz", "do", "s", "n", "r", "nr", "rys", "i", "by", "from", "o", "//", "**", "po", "jej", "przy", "rzecz", "jak", "tymi", "są", "czy", "oraz", "ze", "m", "p", "off", "for", "/", "is", "as", "be", "will", "go", "za", "też", "lub", "t", "poz", "wiad", "set", "use", "etc", "also", "are", "tzw", "out", "other", "its", "has", "<", ">", "pre", "its", "has", "are", "with", "[et", "]", "vol", "leszek", "j", "al", "tych", "tym"};
            //Regex reqular_expression = new Regex(@"(\d)\)+");
            Regex regular_expression = new Regex("[^0-9A-Za-z]+");
            if (!string.IsNullOrEmpty(text))
            {
                string[] splittedTitle = text.ToLower().Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                Words = splittedTitle.ToList();
            }
            else
                return "NULL";
            char[] not_allowed_chars = {'1','2','3','4','5','6','7','8','9','0','!','@','#','$','%','^','&','\'','"','[',']','{','}','(',')'};
            for(int k1=0;  k1<resultDBList.Count; k1++)
            {
                foreach (var word in Words.ToList())
                {
                    foreach (var not_allow_ch in not_allowed_chars)
                        if (word.Length < 3 | word.Contains(not_allow_ch) | resultDBList.Contains(word) | resultDBList[k1]==word)
                            Words.Remove(word);
                }
            }

            var stemmer = new EnglishStemmer();
            
            var stemmingList = Words.ToList().Where(w => w.Any(c => !Char.IsDigit(c))).ToList();

            HashSet<string> stemmingHashSet = new HashSet<string>();
            foreach (var term in Words)
                stemmingHashSet.Add(term);

            Words = stemmingHashSet.ToList();
            
            var splittedTitle1 = Words.ToArray();

            string dictionary_text = File.ReadAllText(@"F:\Magistry files\csv_files\Allowed_term_dictionary.csv");
            string[] allowed_dictionary = dictionary_text.Split(',', '\n');

            for(int i=0; i<=splittedTitle1.Length-1; i++)
            {
                for(int j=0; j<=allowed_dictionary.Length-1; j++)
                {
                    if (splittedTitle1[i].Length > 3 && splittedTitle1[i].Contains(allowed_dictionary[j]))
                    {
                        continue;
                    }
                    else if(splittedTitle1[i].Length <= 3 && !(splittedTitle1[i].Contains(allowed_dictionary[j])))
                    {
                        splittedTitle1.ToList().RemoveAt(i);
                    }
                        
                }
            }

            var stemmingString = string.Join(" ", splittedTitle1.Except(removableWords).Distinct());
            var stemmingString1 = regular_expression.Replace(stemmingString, String.Empty);

            text_preparation.Stop();

            //System.Windows.MessageBox.Show("The text processing time is: "+ text_preparation.Elapsed.Minutes.ToString() + ":" + text_preparation.Elapsed.TotalMilliseconds, "Text processing time" ,System.Windows.MessageBoxButton.OK);
           
            string processing_log = @"F:\Magistry files\Processing_log.txt";
            using(StreamWriter sw = File.AppendText(processing_log))
            {
                sw.WriteLine(DateTime.Now.ToString() + "The text processing time is: " + text_preparation.Elapsed.Minutes.ToString() + ":" + text_preparation.Elapsed.TotalMilliseconds.ToString());
            }

            Debug.WriteLine("The text processing time is: " + text_preparation.Elapsed.Minutes.ToString() + ":" + text_preparation.Elapsed.TotalMilliseconds, "Text processing time", System.Windows.MessageBoxButton.OK);
           
            return stemmingString;
        }
    }
}