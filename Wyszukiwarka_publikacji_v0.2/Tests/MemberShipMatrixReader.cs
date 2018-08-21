using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class MemberShipMatrixReader
    {
        public static float[,] ReadDataFromFile(string filePath, int numberOfElements, int numberOfClusters)
        {
            float[,] membership_matrix = new float[numberOfElements, numberOfClusters];
            string[][] value_line_string_matrix = new string[numberOfElements][];

            using(StreamReader sr = new StreamReader(filePath))
            {
                String line;
                List<String[]> list = new List<string[]>();

                for (int i = 0; i < numberOfElements; i++)
                {
                    line = sr.ReadLine();
                    var data_line = line.Split('\n');
                    list.Add(data_line);
                }

                var data_table = list.ToArray();

                for(int i=0; i<numberOfElements; i++)
                    value_line_string_matrix[i] = data_table[i][0].Split(',');

                for (int i=0; i<value_line_string_matrix.Count(); i++)
                    for (int t = 0; t < value_line_string_matrix[i].Count(); t++)
                    {
                        var value_line_matrix = Convert.ToSingle(value_line_string_matrix[i][t]);
                        membership_matrix[i, t] = value_line_matrix;
                    }
                
            }
            return membership_matrix;
        }
    }
}
