﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    public static class TestDisjointSet
    {
        static int[] parent;
        static int[] rank;

        public static void MakeSet(int i)
        {
            parent[i] = i;
        }

        public static int Find(int i)
        {
            while (i != parent[i]) // If i is not root of tree we set i to his parent until we reach root (parent of all parents)
            {
                i = parent[i];
            }
            return i;
        }

        // Path compression, O(log*n). For practical values of n, log* n <= 5
        public static int FindPath(int i)
        {
            if (i != parent[i])
            {
                parent[i] = FindPath(parent[i]);
            }
            return parent[i];
        }

        public static void Union(int i, int j)
        {
            int i_id = Find(i); // Find the root of first tree (set) and store it in i_id
            int j_id = Find(j); // // Find the root of second tree (set) and store it in j_id

            if (i_id == j_id) // If roots are equal (they have same parents) than they are in same tree (set)
            {
                return;
            }

            if (rank[i_id] > rank[j_id]) // If height of first tree is larger than second tree
            {
                parent[j_id] = i_id; // We hang second tree under first, parent of second tree is same as first tree
            }
            else
            {
                parent[i_id] = j_id; // We hang first tree under second, parent of first tree is same as second tree
                if (rank[i_id] == rank[j_id]) // If heights are same
                {
                    rank[j_id]++; // We hang first tree under second, that means height of tree is incremented by one
                }
            }
        }
    }
}
