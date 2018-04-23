using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    public interface IClonable<T>
    {
        T Clone();
    }


    class DocumentVectorTest: IClonable<DocumentVectorTest>
    {
        public string Content { get; set; }
        public float[] VectorSpace { get; set; }


        public DocumentVectorTest Clone()
        {
            return new DocumentVectorTest
            {
                Content = this.Content,
                VectorSpace = this.VectorSpace
            };
        }
    }
}
