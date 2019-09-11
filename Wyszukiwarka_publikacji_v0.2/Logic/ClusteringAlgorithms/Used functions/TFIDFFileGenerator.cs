using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    public static class TFIDFFileGenerator
    {
        public static void GenerateTFIDFFile(List<DocumentVector> vSpace)
        {
            string testFileDirectory = ConfigurationManager.AppSettings["TestFilesDirectory"].ToString();
            string fileName = "tfidf_matrix_file.csv";
            string filePath = Path.Combine(testFileDirectory, fileName);
            string fileSeparator = ConfigurationManager.AppSettings["FileSeparator"].ToString();

            using (StreamWriter streamWriter = File.AppendText(filePath))
            {
                foreach (var vector in vSpace)
                {
                    foreach (var item in vector.VectorSpace)
                    {
                        streamWriter.Write(item + fileSeparator + " ");
                    }
                    streamWriter.Write(Environment.NewLine);
                }
            }
        }

        public static void DocumentListGeneration(List<DocumentVector> vSpace)
        {
            string testFileDirectory = ConfigurationManager.AppSettings["TestFilesDirectory"].ToString();
            string fileName = "article_list.txt";
            string filePath = Path.Combine(testFileDirectory, fileName);

            using (StreamWriter streamWriter = File.AppendText(filePath))
            {
                foreach (var vector in vSpace)
                {
                    streamWriter.Write(vector.Content);
                    streamWriter.Write(Environment.NewLine);
                }
            }
        }
    }
}
