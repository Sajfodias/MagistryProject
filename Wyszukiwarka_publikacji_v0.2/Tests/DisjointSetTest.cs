using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class DisjointSetTest
    {
        public static int[] parent;
        public static int[] rank;



        public static Tuple<int[], int[], List<TestCentroid>> Set(List<DocumentVectorTest> docCollection)
        {
            Tuple<int[], int[], List<TestCentroid>> result;
            parent = new int[docCollection.Count];
            rank = new int[docCollection.Count];
            var cntroidSet = new List<TestCentroid>();

            for (int i = 0; i < docCollection.Count; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }

            TestCentroid newCentroid;

            //here is a problem cntroidSet.Count must be 46 not 23!!!
            List<DocumentVectorTest> docCollectionCopy = new List<DocumentVectorTest>(docCollection);

            for (int j = 0; j < docCollectionCopy.Count; j++)
            {
                newCentroid = new TestCentroid();
                newCentroid.GroupedDocument = new List<DocumentVectorTest>();
                newCentroid.GroupedDocument.Add(docCollectionCopy[j]);
                cntroidSet.Add(newCentroid);
            }


            result = new Tuple<int[], int[], List<TestCentroid>>(parent, rank, cntroidSet);

            return result;
        }

        /* MakeSet(x) - not used
        public static void MakeSet(int x)
        {
            parent[x] = x;
            rank[x] = 0;
        }
        */

        /* Old Find(int[] parent, int i) function
        public static int Find(int[] parent, int i)
        {
            if (parent[i] == -1)
                return i;
            return Find(parent, parent[i]);
        }
        */


        public static int Find(int i)
        {
            while (i != parent[i]) // If i is not root of tree we set i to his parent until we reach root (parent of all parents)
            {
                i = parent[i];
            }
            return i;
        }

        /* FindPath(i) - don't used now
        public static int FindPath(int i)
        {
            if (i != parent[i])
            {
                parent[i] = FindPath(parent[i]);
            }
            return parent[i];
        }
        */

        /* Old FindSet function
        /// <summary>
        /// Finds representative of a set
        /// </summary>
        /// <param name="x">element of a set</param>
        /// <returns></returns>
        public static int FindSet(int x)
        {
            if (parent[x] != x)
                parent[x] = FindSet(parent[x]); //path compression
            return x;
        }
        */

        public static List<TestCentroid> Union(int x, int y, List<TestCentroid> list_of_Centroid)
        {
            //Tuple<int[], int[], List<TestCentroid>> result;
            List<TestCentroid> result;
            List<TestCentroid> list_of_Centroid_Copy = new List<TestCentroid>(list_of_Centroid);
            List<int> used_index = new List<int>();

            int elementX = Find(x);
            int elementY = Find(y);

            /*
            if (elementX > list_of_Centroid.Count && !used_index.Contains(x))
            {
                used_index.Add(x);
                elementX = Find(x);
            }

            if (elementY > list_of_Centroid.Count && !used_index.Contains(y))
            {
                used_index.Add(y);
                elementY = Find(y);
            }
            */

            //int elementX = FindSet(x);
            //int elementY = FindSet(y);
            if (elementX != elementY)
            {
                if (rank[elementX] == rank[elementY])
                {
                    rank[elementY] = rank[elementY] + 1;
                    parent[elementX] = elementY;
                    if(elementY < list_of_Centroid_Copy.Count && elementX < list_of_Centroid_Copy.Count)
                    {
                        list_of_Centroid_Copy[elementX].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY].GroupedDocument);
                        list_of_Centroid_Copy.RemoveAt(elementY);
                    }
                    else if(elementY == list_of_Centroid_Copy.Count && elementX == list_of_Centroid_Copy.Count)
                    {
                        list_of_Centroid_Copy[elementX-1].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY-1].GroupedDocument);
                        list_of_Centroid_Copy.RemoveAt(elementY-1);
                    }
                }
                else if (rank[elementX] > rank[elementY])
                {
                    parent[elementY] = elementX;
                    if (elementX < list_of_Centroid_Copy.Count && elementY < list_of_Centroid_Copy.Count)
                    {
                        list_of_Centroid_Copy[elementY].GroupedDocument.AddRange(list_of_Centroid_Copy[elementX].GroupedDocument);
                        list_of_Centroid_Copy.RemoveAt(elementX);
                    }
                    else if(elementX == list_of_Centroid_Copy.Count && elementY == list_of_Centroid_Copy.Count)
                    {
                        list_of_Centroid_Copy[elementY-1].GroupedDocument.AddRange(list_of_Centroid_Copy[elementX-1].GroupedDocument);
                        list_of_Centroid_Copy.RemoveAt(elementX-1);
                    }
                }
                else if (rank[elementX] < rank[elementY])
                {
                    parent[elementX] = elementY;
                    if (elementY < list_of_Centroid_Copy.Count && elementX < list_of_Centroid_Copy.Count)
                    {
                        list_of_Centroid_Copy[elementX].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY].GroupedDocument);
                        list_of_Centroid_Copy.RemoveAt(elementY);
                    }
                    else if(elementY == list_of_Centroid_Copy.Count && elementX == list_of_Centroid_Copy.Count)
                    {
                        list_of_Centroid_Copy[elementX-1].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY-1].GroupedDocument);
                        list_of_Centroid_Copy.RemoveAt(elementY-1);
                    }
                }
            }

            //result = new Tuple<int[], int[], List<TestCentroid>>(parent, rank, list_of_Centroid_Copy);
            result = list_of_Centroid_Copy;
            return result;
        }

        public static List<TestCentroid> Union1(int x, int y, List<TestCentroid> list_of_Centroid)
        {
            List<TestCentroid> result;
            List<TestCentroid> list_of_Centroid_Copy = new List<TestCentroid>(list_of_Centroid);
            List<int> used_index = new List<int>();
            int elementX = 0;
            int elementY = 0;

            int element_X = Find(x);
            if (element_X >= list_of_Centroid_Copy.Count)
                element_X = Find(x);
            else
                elementX = element_X;
            
            int element_Y = Find(y);
            if (element_Y >= list_of_Centroid_Copy.Count)
                element_Y = Find(y);
            else
                elementY = element_Y;

            if (elementX != elementY)
            {
                if (rank[elementX] == rank[elementY])
                {
                    rank[elementY] = rank[elementY] + 1;
                    parent[elementX] = elementY;
                    list_of_Centroid_Copy[elementX].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY].GroupedDocument);
                    list_of_Centroid_Copy.RemoveAt(elementY);

                }
                else if (rank[elementX] > rank[elementY])
                {
                    parent[elementY] = elementX;
                    list_of_Centroid_Copy[elementY].GroupedDocument.AddRange(list_of_Centroid_Copy[elementX].GroupedDocument);
                    list_of_Centroid_Copy.RemoveAt(elementX);
                }
                else
                {
                    parent[elementX] = elementY;
                    list_of_Centroid_Copy[elementX].GroupedDocument.AddRange(list_of_Centroid_Copy[elementY].GroupedDocument);
                    list_of_Centroid_Copy.RemoveAt(elementY);
                }
            }

            result = list_of_Centroid_Copy;
            return result;
        }
    }
}
