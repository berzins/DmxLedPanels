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
            createUiConfigFile(context);
            string html = FileIO.ReadFile(SettingManager.Instance.Settings.UIHomePath + UI_FILE_NAME, false);
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_HTML, html);
        }

        private void createUiConfigFile(HttpListenerContext context)
        {
            var settings = SettingManager.Instance.Settings;
            var file = settings.UIHomePath + settings.UIRelJavascriptPath + "config.js";
            var url = context.Request.Url.AbsoluteUri.Replace("?react_perf", "");
            //Console.WriteLine(url);

            UIConfig config = new UIConfig {
                host = url,
                defaultOutputIp = SettingManager.Instance.Settings.DefaultOutputIp
            };
            var js = "document.ledPanelUiConfig = " + StaticSerializer.Serialize(config);
            FileIO.WriteFile(file, false, js);
        }
    }
}