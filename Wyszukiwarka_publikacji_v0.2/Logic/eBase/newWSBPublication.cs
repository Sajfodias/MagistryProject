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
    class newWSBPublication
    {
        public static String endText = ParserRTF.return_endText();
        public static String[] sep = { "\r" };
        public static string[] lines = endText.Split(sep,StringSplitOptions.RemoveEmptyEntries);
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

        public static void get_WSB_Document_content()
        {
            string[] WSB_newcontent = new string[hapDoc.DocumentNode.InnerText.Length];
            string[] WSB_separatedContent = new string[hapDoc.DocumentNode.InnerText.Length];

            WSB_articles_Count = 0;
            string[] WSB_articles_Matrix = { String.Empty };


        }
        }
}
