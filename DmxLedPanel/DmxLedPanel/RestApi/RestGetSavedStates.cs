using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestGetSavedStates : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try {

                var fileNames = StateManager.Instance.GetAllStateFiles();
                var msg = new ResponseMessage(ResponseMessage.TYPE_SAVED_STATES, fileNames);
                var data = Util.StaticSerializer.Serialize(msg);
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
                
            } catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
