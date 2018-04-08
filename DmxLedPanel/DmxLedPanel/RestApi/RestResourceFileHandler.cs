using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestResourceFileHandler : HttpRequestHandler
    {
        private string file;

        public RestResourceFileHandler(string file) {
            this.file = file;
        }

        public override void HandleRequest(HttpListenerContext context)
        {
            byte[] buf = File.ReadAllBytes(file);   
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_RESOURCE, buf);
        }
    }
}
