using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestHomeHandler : HttpRequestHandler
    {
        public static readonly string UI_FILE_NAME = @"\index.html";

        public override void HandleRequest(HttpListenerContext context)
        {
            string html = FileIO.ReadFile(SettingManager.Instance.Settings.UIHomePath + UI_FILE_NAME, false);
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_HTML, html);
        }


    }
}