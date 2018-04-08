using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestPageNotFoundHandler : HttpRequestHandler
    {

        string html = "<!DOCTYPE html><html><head></head><body><H1>Page not found</H1></body></html>";

        public override void HandleRequest(HttpListenerContext context)
        {
            WriteResponse(context, RestConst.RESPONSE_NOT_FOUND, RestConst.CONTENT_TEXT_HTML, html);
        }
    }
}
