using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms.Used_functions
{
    class ClusterWrapper
    {
        public List<DocumentVectorWrapper> documentVectorWrappersList { get; set; }

        public ClusterWrapper()
        {

        }

        public List<DocumentVectorWrapper> ReturnDocumentList()
        {
            return documentVectorWrappersList;
        }

    }
}
