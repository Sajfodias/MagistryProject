using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    public class Document
    {
        public int document_ID { get; set; }
        public string document_Content { get; set; }
        public float[] VectorSpace { get; set; }
        public int[] author_ID { get; set; }
        
    }
}
