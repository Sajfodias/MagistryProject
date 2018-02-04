using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    class DocumentVectorWrapper
    {
        public float[] DocVect { get; set; }


        public DocumentVectorWrapper(DocumentVector vector)
        {
            this.DocVect = vector.VectorSpace;
        }

    }
}
