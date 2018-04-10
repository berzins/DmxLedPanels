using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestPrefixNotRegistredHandler : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            var relUrl = RestApiServer.GetRealtiveUrl(context.Request.Url.ToString());
            string file = SettingManager.Instance.Settings.UIHomePath + relUrl;
            file = file.Replace("/", @"\");
            if (File.Exists(file))
            {
                new RestResourceFileHandler(file).HandleRequest(context);
            }
            else {
                new RestPageNotFoundHandler().HandleRequest(context);
            }

        }
    }
}
