using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Algorithms
{
    public class DetailedDocumentVector
    {
        //protected double[] tDF;
        //protected double[] iDF;
        public float[] tfIDF;
        
        protected DocumentVector document;

        public DetailedDocumentVector()
        {

        }

        public void SetDocument(DocumentVector doc)
        {
            document = doc;
        }

        #region OldGeters
        /*
        public int GetIDFDimensions()
        {
            return iDF.Length;
        }
        public int GetTDFDimensions()
        {
            return tDF.Length;
        }
        */
        #endregion

        public int GetTFIDFDimensions()
        {
            return tfIDF.Length;
        }
        public DocumentVector GetDocument()
        {
            return document;
        }
        public DetailedDocumentVector(int size)
        {
            //tDF = new double[size];
            //iDF = new double[size];
            tfIDF = new float[size];
        }

        #region OldCumputingDistanceFunctions
        /*
        public double ComputeTDFDistance(DetailedDocumentVector doc2)
        {
            double result = 0.0;
            if (this.GetTDFDimensions() != doc2.GetTDFDimensions())
                throw new ArgumentOutOfRangeException();
            for (var i = 0; i < doc2.GetTDFDimensions(); i++)
                result += Math.Pow(Math.Abs(tDF[i] - doc2.TDF[i]), 2.0);
            return result;

        }

        public double ComputeIDFDistance(DetailedDocumentVector doc2)
        {
            double result = 0.0;
            if (this.GetIDFDimensions() != doc2.GetIDFDimensions())
                throw new ArgumentOutOfRangeException();
            for (var i = 0; i < doc2.GetIDFDimensions(); i++)
                result += Math.Pow(Math.Abs(iDF[i] - doc2.IDF[i]), 2.0);
            return result;

        }
        */
        #endregion

        public float ComputeTFIDFDistance(DocumentVector doc2)
        {
            float result = 0;
            if (this.GetTFIDFDimensions() != doc2.VectorSpace.Length)
                throw new ArgumentOutOfRangeException();
            for (int i = 0; i < doc2.VectorSpace.Length; i++)
                result += (float)Math.Pow(Math.Abs(tfIDF[i] - doc2.VectorSpace[i]), 2);
            return result;
        }

        #region OldProperties
        /*
        public double[] TDF
        {
            get { return tDF; }
            set { tDF = value; }
        }

        public double[] IDF
        {
            get { return iDF; }
            set { iDF = value; }
        }
        */
        #endregion

        public float[] TFIDF
        {
            get { return tfIDF; }
            set { tfIDF = value; }
        }
    }
}
