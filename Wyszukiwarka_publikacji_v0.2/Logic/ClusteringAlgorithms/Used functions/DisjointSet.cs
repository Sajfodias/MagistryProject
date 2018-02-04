//code and idea was taking form http://www.dotnetlovers.com/Article/184/disjoint-sets-data-structure?AspxAutoDetectCookieSupport=1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    public class DisjointSet
    {
        public static int[] parent;
        public static int[] rank;

        /*
        public static List<Centroid> MakeSet(List<DocumentVector> docCollection)
        {
            var result = new List<Centroid>();
            parent = new int[docCollection.Count];
            rank = new int[docCollection.Count];

            Centroid newCentroid;
            List<DocumentVector> docCollectionCopy = new List<DocumentVector>(docCollection);
            for (int i = 0; i < docCollection.Count; i++)
            {
                newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid.GroupedDocument.Add(docCollectionCopy[i]);
                parent[i] = i;
                rank[i] = 0;

                result.Add(newCentroid);
                docCollectionCopy.RemoveAt(i);
                
            }
            return result;
        }
        */

        public static Tuple<int[],int[], List<Centroid>> Set(List<DocumentVector> docCollection)
        {
            Tuple<int[], int[], List<Centroid>> result;
            parent = new int[docCollection.Count];
            rank = new int[docCollection.Count];
            var cntroidSet = new List<Centroid>();

            for (int i = 0; i < docCollection.Count; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }

            Centroid newCentroid;

            //here is a problem cntroidSet.Count must be 46 not 23!!!
            List<DocumentVector> docCollectionCopy = new List<DocumentVector>(docCollection);

            for(int j=0; j<docCollectionCopy.Count; j++)
            {
                newCentroid = new Centroid();
                newCentroid.GroupedDocument = new List<DocumentVector>();
                newCentroid.GroupedDocument.Add(docCollectionCopy[j]);
                cntroidSet.Add(newCentroid);
            }
            

            result = new Tuple<int[], int[], List<Centroid>>(parent, rank, cntroidSet);

            return result;
        }

        /*
        public static void MakeSet(int x)
        {
            parent[x] = x;
            rank[x] = 0;
        }
        */

        public static int Find(int[] parent, int i)
        {
            if (parent[i] == -1)
                return i;
            return Find(parent, parent[i]);
        }

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

        /*
        public void Union(int x, int y)
        {
            int elementX = FindSet(x);
            int elementY = FindSet(y);

            if (rank[elementX] == rank[elementY])
            {
                rank[elementY] = rank[elementY] + 1;
                parent[elementX] = elementY;
            }
            else if (rank[elementX] > rank[elementY])
                parent[elementY] = elementX;
            else
                parent[elementX] = elementY; 
        }
        */

        public static Tuple<int[],int[], List<Centroid>> Union(int x, int y, List<Centroid>list_of_Centroid)
        {
            Tuple<int[], int[], List<Centroid>> result;
            List<Centroid> list_of_Centroid_Copy = new List<Centroid>(list_of_Centroid);
            int elementX = FindSet(x);
            int elementY = FindSet(y);

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

            
            result = new Tuple<int[], int[], List<Centroid>>(parent, rank, list_of_Centroid_Copy);
            return result;
        }
    }
}
