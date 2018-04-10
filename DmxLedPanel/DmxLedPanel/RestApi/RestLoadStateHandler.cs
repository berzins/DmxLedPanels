using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestLoadStateHandler : HttpRequestHandler
    {
        public static readonly string KEY_FILE_NAME = "file_name";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try {

                var fileName = q.Get(KEY_FILE_NAME);


            } catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
