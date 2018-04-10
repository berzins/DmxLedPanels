using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public static class RestConst
    {
        public const string CONTENT_TEXT_HTML = "text/html";

        public const string CONTENT_TEXT_JSON = "application/json";

        public const string CONTENT_RESOURCE = "application/octet-stream";

        public const string CONTENT_TEXT_CSS = "text/css";

        public const string CONTENT_TEXT_JAVASCRIPT = "application/javascript";

        public const int RESPONSE_OK = 200;

        public const int RESPONSE_NOT_FOUND = 404;

        public const int RESPONSE_INTERNAL_ERROR = 500;

    }
}
