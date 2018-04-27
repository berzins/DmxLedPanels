using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestGetCurrentProject : HttpRequestHandler
    {
        
        public override void HandleRequest(HttpListenerContext context)
        {

            try
            {
                var cp = SettingManager.Instance.Settings.CurrentProject;
                var msg = new ResponseMessage(ResponseMessage.TYPE_CURRENT_PROJECT, cp);
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                    StaticSerializer.Serialize(msg)
                    );
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
