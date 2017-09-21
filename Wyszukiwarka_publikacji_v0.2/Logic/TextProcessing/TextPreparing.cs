using System;
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

namespace Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing
{
    class TextPreparing
    {
        public static string TermsPrepataions(string _text)
        {
            string text = _text;
            char[] splitChars = { ' ', ',', '.', ';', '-', ':' };
            string[] removableWords = { "and", "or", "it", "at", "all", "in", "on", "under", "between", "a", "an", "the", "to", "pod", "nad", "tam", "tutaj", "między", "pomiędzy", "w", "przed", "się", "z", "na", "od", "jest", "iż", "co", "we", "ich", "ciebie", "ja", "ty", "ona", "ono", "oni", "owych", "of", "cz", "do", "s", "n", "r", "nr", "rys", "i", "by", "from", "o", "//", "**", "po", "jej", "przy", "rzecz", "jak", "tymi", "są", "czy", "oraz", "ze", "m", "p", "off", "for", "/", "is", "as", "be", "will", "go", "za", "też", "lub", "t", "poz", "wiad", "set", "use", "etc", "also", "are", "tzw", "out", "other", "its", "has", "<", ">", "pre", "its", "has", "are", "with", "[et", "]", "vol", "leszek", "j", "al"};
            //Regex reqular_expression = new Regex(@"(\d)\)+");
            Regex regular_expression = new Regex("[^0-9A-Za-z]+");
            string[] splittedTitle = text.ToLower().Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            var stemmer = new EnglishStemmer();
            
            var stemmingList = splittedTitle.ToList().Where(w => w.Any(c => !Char.IsDigit(c))).ToList();
            
            splittedTitle = stemmingList.ToArray();

            //added 12.08 18:35
            /*
            string[] numericalValues = new string[9999999];
            for(int i=0; i<=numericalValues.Length-1; i++)
            {
                numericalValues[i] = i.ToString() + ")";
            }
            for(int z=0; z<=numericalValues.Length-1; z++)
            {
                if (splittedTitle.Contains(numericalValues[z]))
                    splittedTitle[z].Replace(numericalValues[z], String.Empty);
            }
            */

            var stemmingString = string.Join(" ", splittedTitle.Except(removableWords).Distinct());
            var stemmingString1 = regular_expression.Replace(stemmingString, String.Empty);
            
            //System.Windows.MessageBox.Show(stemmingString, "Splitted String", System.Windows.MessageBoxButton.OK);
            return stemmingString;
        }
    }
}
