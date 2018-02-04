using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    public class DocumentVector
    {
        public string Content { get; set; }
        public float[] VectorSpace { get; set; }
        public float[] OriginalVectorSpace { get; set; }

        public void SaveOriginal(DocumentVector docVect)
        {
            OriginalVectorSpace = new float[docVect.VectorSpace.Length];
            for (var i = 0; i < docVect.VectorSpace.Count(); i++)
            {
                OriginalVectorSpace[i] = docVect.VectorSpace[i];
            }
        }


        public void RestoreOriginal(DocumentVector docVect)
        {
            if (OriginalVectorSpace == null)
                return;
            if (OriginalVectorSpace.Length == 0)
                return;

            VectorSpace = new float[docVect.VectorSpace.Length];
            for (var i = 0; i < docVect.VectorSpace.Length; i++)
                VectorSpace[i] = OriginalVectorSpace[i];
        }

    }
}
