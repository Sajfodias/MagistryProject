using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class TestDocVectorCreator
    {
        public static List<DocumentVectorTest> CreatingTheDocVectorCollection(string fileName)
        {
            List<DocumentVectorTest> TestDocVectorList = new List<DocumentVectorTest>();
            int number_of_lines = 0;
            char[] separators = { ' ', ',', '/', '.', '-','\t' };

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    number_of_lines++;
                    DocumentVectorTest testDoc = new DocumentVectorTest();
                    testDoc.VectorSpace = new float[2];
                    var lineitem = line.TrimStart(' ');
                    var lineitem2 = lineitem.TrimEnd(' ');
                    var items = lineitem2.Split(separators);
                    testDoc.VectorSpace[0] = float.Parse(items[0]);
                    testDoc.VectorSpace[1] = float.Parse(items[1]);

                    /*
                    for (int i = 0; i < items.Count(); i++)
                    {
                        if (!items[i].Contains(" "))
                            testDoc.VectorSpace[i] = float.Parse(items[i]);
                        else
                            continue;
                    }   
                    */
                    //testDoc.Content = "testDataPoint" + number_of_lines;
                    testDoc.Content = number_of_lines.ToString();

                    TestDocVectorList.Add(testDoc);
                }
            }
            return TestDocVectorList;
        }

        public static List<DocumentVectorTest> NormalizationDocumentVectors(List<DocumentVectorTest> vSpace)
        {
            List<DocumentVectorTest> result = new List<DocumentVectorTest>();

            float MaxValueX = float.MinValue;
            float MaxValueY = float.MinValue;

            for(int i=0; i<vSpace.Count; i++)
            {
                if (vSpace[i].VectorSpace[0] > MaxValueX)
                    MaxValueX = vSpace[i].VectorSpace[0];
                if (vSpace[i].VectorSpace[1] > MaxValueY)
                    MaxValueY = vSpace[i].VectorSpace[1];
            }

            for(int k=0; k<vSpace.Count; k++)
            {
                vSpace[k].VectorSpace[0] = vSpace[k].VectorSpace[0] / MaxValueX;
                vSpace[k].VectorSpace[1] = vSpace[k].VectorSpace[1] / MaxValueY;
            }

            result = vSpace;

            return result;
        }
    }
}
