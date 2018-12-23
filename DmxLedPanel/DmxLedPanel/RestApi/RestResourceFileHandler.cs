using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestResourceFileHandler : HttpRequestHandler
    {
        private string file;

        private static readonly string EXT_CSS = ".css";
        private static readonly string EXT_JS = ".js";

        public RestResourceFileHandler(string file) {
            this.file = file;
        }

        public override void HandleRequest(HttpListenerContext context)
        {
            var ext = Path.GetExtension(file);
            string mime = RestConst.CONTENT_RESOURCE;
            if (ext.Equals(EXT_CSS)) {
                mime = RestConst.CONTENT_TEXT_CSS;
            }
            if (ext.Equals(EXT_JS)) {
                mime = RestConst.CONTENT_TEXT_JAVASCRIPT;
            }

            byte[] buf = File.ReadAllBytes(file);
            SetInfoMessage("'" + file + "' -> read ok.", IS_NOT_PART_OF_STATE,
                Talker.Talker.GetSource());
            WriteResponse(context, RestConst.RESPONSE_OK, mime, buf);
        }
    }
}
