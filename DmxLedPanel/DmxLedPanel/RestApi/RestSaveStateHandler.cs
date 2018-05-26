using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestSaveStateHandler : HttpRequestHandler
    {
        public static readonly string KEY_FILE_NAME = "file_name";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            var fileName = q.Get(KEY_FILE_NAME);

            try
            {
                StateManager.Instance.SaveState(fileName);
                ResponseMessage msg = new ResponseMessage(ResponseMessage.TYPE_INFO, "File saved");
                var data = Util.StaticSerializer.Serialize(msg);
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
            }
            catch (Exception e) {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }
          

        }
    }
}
