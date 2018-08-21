using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms
{
    public class Centroid
    {
        public List<DocumentVector> GroupedDocument { get; set; }

        #region SetCenter
        public float[] Center { get; set; }
        public void SetCenter(float[] center)
        {
            Center = center;
        }
        #endregion

        public float[] means { get; set; }

        public void CalculateMeans()
        {
            if (GroupedDocument == null)
                return;
            if (GroupedDocument.Count < 1)
                return;
            means = new float[GroupedDocument.First().VectorSpace.Length];
            for (var i = 0; i < GroupedDocument.First().VectorSpace.Length; i++)
            {
                means[i] = 0;
            }
            foreach (var doc in GroupedDocument)
            {
                for (var i = 0; i < doc.VectorSpace.Length; i++)
                {
                    means[i] += doc.VectorSpace[i];
                }
            }
            for (var i = 0; i < GroupedDocument.First().VectorSpace.Length; i++)
            {
                means[i] /= GroupedDocument.Count;
            }
        }
    }
}
