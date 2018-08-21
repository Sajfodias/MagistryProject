using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Label_Matrix
    {
        internal static int[] Label_Matrix_Extractions(List<TestCentroid> resultSet, string path)
        {
            int number_of_elements = 0;
            for (int c = 0; c < resultSet.Count; c++)
                number_of_elements += resultSet[c].GroupedDocument.Count;

            //int[] label_matrix = new int[number_of_elements-resultSet.Count];
            int[] label_matrix = new int[number_of_elements];

            for(int i=0; i<number_of_elements; i++)
            {
                for (int r = 0; r < resultSet.Count; r++)
                {
                    for (int ri = 0; ri < resultSet[r].GroupedDocument.Count; ri++)
                    {
                        var index = Convert.ToInt32(resultSet[r].GroupedDocument[ri].Content)-1;
                        label_matrix[index] = r;
                    }
                }
            }

            //string[] string_label_matrix = new string[number_of_elements-resultSet.Count];
            string[] string_label_matrix = new string[number_of_elements];
            for (int k = 0; k < label_matrix.Length; k++)
                string_label_matrix[k] = label_matrix[k].ToString();

            using(StreamWriter w = File.AppendText(path))
            {
                for (int i = 0; i < string_label_matrix.Length; i++)
                    w.WriteLine(string_label_matrix[i]);
            }
            //System.IO.File.WriteAllLines(path, string_label_matrix);

            return label_matrix;
        }

        internal static int[] ReleaseVersion_Label_Matrix_Extractions(List<Centroid> resultSet, string path)
        {
            int number_of_elements = 0;
            for (int c = 0; c < resultSet.Count; c++)
                number_of_elements += resultSet[c].GroupedDocument.Count;

            int[] label_matrix = new int[number_of_elements];

            for (int i = 0; i < number_of_elements; i++)
            {
                for (int r = 0; r < resultSet.Count; r++)
                {
                    for (int ri = 0; ri < resultSet[r].GroupedDocument.Count; ri++)
                    {
                        // i must to think how i will generate the index here
                        var index = resultSet[r].GroupedDocument[ri].index_Of_Doc_for_labeling;
                        label_matrix[index] = r;
                    }
                }
            }

            string[] string_label_matrix = new string[number_of_elements];
            for (int k = 0; k < label_matrix.Length; k++)
                string_label_matrix[k] = label_matrix[k].ToString();

            using (StreamWriter w = File.AppendText(path))
            {
                for (int i = 0; i < string_label_matrix.Length; i++)
                    w.WriteLine(string_label_matrix[i]);
            }
            return label_matrix;
        }
    }
}
