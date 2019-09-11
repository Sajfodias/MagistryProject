using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Web;
using HtmlAgilityPack;
using BibTeXLibrary;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    
    class ParserRTF
    {
        private static string filePathRTF = ConfigurationManager.AppSettings["RtfDataFileDirectory"].ToString();
        //private static string filePathRTF = @"F:\\Magistry files\Rtf_files\";
        public static HtmlDocument hapDoc = new HtmlDocument();
        /// <summary>
        /// We must return the formatted text
        /// </summary>
        public static string endText = String.Empty;


        public static string parseRTF(string fileName)
        {

            HtmlWeb web = new HtmlWeb();
            var encoding = web.AutoDetectEncoding;
            web.OverrideEncoding = Encoding.GetEncoding("iso-8859-2");
            hapDoc = web.Load(fileName);
            //char[] separators = {';', ',', ' '};
            string ContentDocument = hapDoc.DocumentNode.InnerHtml.ToString();
            string filteredDocument = ContentDocument.Replace("<span class=\"querylabel\" id=\"querylabel\">", "\r");

            string[] replacementArray = {
                "<span class=\"cntfoundtxt\" id=\"cntfoundtxt\">",
                "<br>",
                "ź",
                "ż",
                "ń",
                "ł",
                "ó",
                "Ż",
                "Ź",
                "Ń",
                "Ł",
                "Ó",
                "ś",
                "Ś",
                "ę",
                "ą",
                "Ą",
                "Ę",
            };
            string[] replacedCharacters = {
                " ",
                "\r",
                "z",
                "z",
                "n",
                "l",
                "o",
                "Z",
                "Z",
                "N",
                "L",
                "O",
                "s",
                "S",
                "e",
                "a",
                "A",
                "E"
            };

            for (int i = 0; i < replacementArray.Length; i++)
                filteredDocument = filteredDocument.Replace(replacementArray[i], replacedCharacters[i]);

            endText = Regex.Replace(filteredDocument, "<.*?>", string.Empty);
         
            var CombinedPath = Path.Combine(filePathRTF, "Formated_"+Path.GetFileNameWithoutExtension(fileName)+".txt");
            File.AppendAllText(CombinedPath,endText);

            return endText;
        }
            

        public static HtmlDocument return_hapDoc()
        {
            return hapDoc;
        }

        public static string return_endText()
        {
            return endText;
        }
      }
}
 