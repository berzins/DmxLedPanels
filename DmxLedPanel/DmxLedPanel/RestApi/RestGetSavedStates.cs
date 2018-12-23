using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;
using DmxLedPanel.Util;

namespace DmxLedPanel.RestApi
{
    public class RestGetSavedStates : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try {
                var fileNames = StateManager.Instance.GetAllStateFiles().ToList();
                fileNames.Sort();
                var msg = new ResponseMessage(ResponseMessage.TYPE_SAVED_STATES, fileNames);

                SetInfoMessage(
                    "Saved projects are: "
                    + StringUtil.RemoveLastChars(
                        fileNames.Aggregate("", (s, f) => s + f + ", "), 2),
                    IS_NOT_PART_OF_STATE,
                    Talker.Talker.GetSource()
                    );

                var data = Util.StaticSerializer.Serialize(msg);
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
                
            } catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
