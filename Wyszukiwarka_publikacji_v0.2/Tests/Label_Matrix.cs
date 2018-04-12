using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class Label_Matrix
    {
        internal static int[] Label_Matrix_Extractions(List<TestCentroid> resultSet, string path)
        {
            int number_of_elements = 0;
            for (int c = 0; c < resultSet.Count; c++)
                number_of_elements += resultSet[c].GroupedDocument.Count;

            int[] label_matrix = new int[number_of_elements-resultSet.Count];

            for(int r=0; r<resultSet.Count; r++)
            {
                for(int ri=0; ri<resultSet[r].GroupedDocument.Count; ri++)
                {
                    label_matrix[Convert.ToInt32(resultSet[r].GroupedDocument[ri].Content)-1] = r;
                }
            }

            string[] string_label_matrix = new string[number_of_elements - resultSet.Count];
            for (int k = 0; k < label_matrix.Length; k++)
                string_label_matrix[k] = label_matrix[k].ToString();

            System.IO.File.WriteAllLines(path, string_label_matrix);

            return label_matrix;
        }
    }
}
