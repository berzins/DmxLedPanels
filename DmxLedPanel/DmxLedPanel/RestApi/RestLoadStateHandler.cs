using DmxLedPanel.State;
using DmxLedPanel.Util;
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
                StateManager.Instance.LoadState(fileName);
                SettingManager.Instance.Settings.CurrentProject = fileName;
                SettingManager.Instance.Save();
                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
                
            } catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
