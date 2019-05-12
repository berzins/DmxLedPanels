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
    public class RestGetStateHandler : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var state = StateManager.Instance.State.Serialize();

                SetInfoMessage(
                        "State to be returned is: " + SettingManager.Instance.Settings.CurrentProject,
                        IS_NOT_PART_OF_STATE,
                        Talker.Talk.GetSource()
                        );

                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
