using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wyszukiwarka_publikacji_v0._2;
using Wyszukiwarka_publikacji_v0._2.Logic;

namespace Wyszukiwarka_publikacji_v0._2
{
    /// <summary>
    /// Interaction logic for RTFReader.xaml
    /// </summary>
    public partial class RTFReader : Window
    {
        
        public RTFReader()
        {
            InitializeComponent();
        }

        private static void LoadRTF(string rtf, RichTextBox richTextBox)
        {
            richTextBox.AcceptsReturn = true;
            richTextBox.AcceptsTab = true;

            if (string.IsNullOrEmpty(rtf))
            {
                throw new ArgumentNullException();
            }

        
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);


            //Create a MemoryStream of the Rtf content

            using (MemoryStream rtfMemoryStream = new MemoryStream())
            {
                using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                {
                    
                    rtfStreamWriter.Write(rtf);
                    rtfStreamWriter.Flush();
                    rtfMemoryStream.Seek(0, SeekOrigin.Begin);


                    //Load the MemoryStream into TextRange ranging from start to end of RichTextBox.
                    textRange.Load(rtfMemoryStream, DataFormats.Rtf);
                }
            }
        }

        private void loadRTFBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".rtf";
            dlg.Filter = "rtf Files (*.rtf)|*.rtf|TXT Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                string content = ParserRTF.parseRTF(filename);
                RTFContent.AcceptsReturn = true;
                RTFContent.AcceptsTab = true;
                LoadRTF(content, RTFContent); // here we have the delay in calculations
                if (filename.Contains("UG"))
                {
                    //Task.Factory.StartNew(()=>Logic.eBase.UGPublicationBase.get_UG_Document_content());
                    Logic.eBase.UGPublicationBase.get_UG_Document_content();
                }
                else if (filename.Contains("PP"))
                {
                    //Task.Factory.StartNew(() => Logic.eBase.PPPublicationBase.get_PP_Document_content());
                    Logic.eBase.PPPublicationBase.get_PP_Document_content();
                }
                else if (filename.Contains("UMK"))
                {
                    //Task.Factory.StartNew(() => Logic.eBase.UMKPublicationBase.get_UMK_Document_content());
                    Logic.eBase.UMKPublicationBase.get_UMK_Document_content();
                }
                else if (filename.Contains("WSB"))
                {
                    //Task.Factory.StartNew(() => Logic.eBase.WSBPublicationBase.get_WSB_Document_content());
                    //Task.Factory.StartNew(() => Logic.eBase.WSBPublication.get_WSB_Document_content());
                    Logic.eBase.WSBPublicationBase.get_WSB_Document_content();
                }
            }
        }
    }
}
