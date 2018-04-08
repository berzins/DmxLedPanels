using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public interface IHttpRequestHandler
    {

        void HandleRequest(HttpListenerContext context);

    }
}
