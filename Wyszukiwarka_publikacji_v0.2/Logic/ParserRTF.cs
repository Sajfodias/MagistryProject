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

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    
    class ParserRTF
    {
        private static string filePathRTF = @"F:\\Magistry files\Rtf_files\";
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
            string stringFilter1 = ContentDocument.Replace("<span class=\"querylabel\" id=\"querylabel\">", "\r");
            string stringFilter2 = stringFilter1.Replace("<span class=\"cntfoundtxt\" id=\"cntfoundtxt\">", " ");
            string stringFilter3 = stringFilter2.Replace("<br>", "\r");
            string stringFilter4 = stringFilter3.Replace("ź", "z");
            string stringFilter5 = stringFilter4.Replace("ż", "z");
            string stringFilter6 = stringFilter5.Replace("ń", "n");
            string stringFilter7 = stringFilter6.Replace("ł", "l");
            string stringFilter8 = stringFilter7.Replace("ó", "o");
            string stringFilter9 = stringFilter8.Replace("Ż", "Z");
            string stringFilter10 = stringFilter9.Replace("Ź", "Z");
            string stringFilter11 = stringFilter10.Replace("Ń", "N");
            string stringFilter12 = stringFilter11.Replace("Ł", "L");
            string stringFilter13 = stringFilter12.Replace("Ó", "O");
            string stringFilter14 = stringFilter13.Replace("ś", "s");
            string stringFilter15 = stringFilter14.Replace("Ś", "S");
            string stringFilter16 = stringFilter15.Replace("ę", "e");
            string stringFilter17 = stringFilter16.Replace("ą", "a");
            string stringFilter18 = stringFilter17.Replace("Ą", "A");
            string stringFilter19 = stringFilter18.Replace("Ę", "E");
            
            endText = Regex.Replace(stringFilter19, "<.*?>", string.Empty);

            var file = File.CreateText(filePathRTF + @"formated_" + fileName.Substring(28) +".txt");
            file.Write(endText);
            file.Flush();
            file.Close();

            #region old_part_code
            //string[] newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            //string[] separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];
            /*
            using (StringReader sr = new StringReader(endText))
            {
                string line;
                for (int i = 0; i <= hapDoc.DocumentNode.InnerText.Length; i++)
                {
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        newcontent[i] = line;
                        separatedContent = line.Split(':');

                        if (separatedContent.Length == 2 && separatedContent[0].ToLower().Contains("aut"))
                        {
                            string[] UG_autors = separatedContent[1].Split(separators);

                        }
                        else if (separatedContent.Length == 2 && separatedContent[0].ToLower().Contains("tytu")) System.Windows.MessageBox.Show(separatedContent[1]);
                        else if(separatedContent.Length == 2 && (separatedContent[0].ToLower().Contains("zrod") || separatedContent[0].ToLower().Contains("czasop") || separatedContent[0].ToLower().Contains("opis wyd"))) System.Windows.MessageBox.Show(separatedContent[1]);

                        else System.Windows.MessageBox.Show("Error! Content not found!", "Error!", System.Windows.MessageBoxButton.OK);
                        
                    }
                    else
                    {
                        return endText;
                    }
                }
            }
            */
            #endregion

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
 