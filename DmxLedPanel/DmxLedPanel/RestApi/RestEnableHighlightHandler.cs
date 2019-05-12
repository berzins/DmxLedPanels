using DmxLedPanel.ArtNetIO;
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEnableHighlightHandler : HttpRequestHandler
    {

        public static readonly string KEY_ENABLED = "enabled";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var enabled = bool.Parse(q.Get(KEY_ENABLED));
                ResponseMessage msg = null;
                if (enabled)
                {
                    ArtnetIn.Instance.Stop();
                    State.HighlightState.Instance.Enabled = true;
                    msg = new ResponseMessage(ResponseMessage.TYPE_INFO, true);
                }
                else {
                    ArtnetIn.Instance.Start();
                    HighlightState.Instance.Enabled = false;
                    msg = new ResponseMessage(ResponseMessage.TYPE_INFO, false);
                }

                SetInfoMessage(
                    "The highlight state is set to: " + enabled,
                    IS_PART_OF_STATE,
                    Talker.Talk.GetSource()
                    );

                WriteResponse(
                    context, 
                    RestConst.RESPONSE_OK, 
                    RestConst.CONTETN_TEST_PLAIN,
                    Util.StaticSerializer.Serialize(msg));
            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
