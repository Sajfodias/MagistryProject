using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wyszukiwarka_publikacji_v0._2.Logic.Webcomunications
{
    public class HttpWebRequest_BeginGetResponse
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public const int BUFFER_SIZE = 1024;
        public const int DefaultTimeout = 5 * 60 * 1000; // 5 minutes timeout


        public static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
    }
}
