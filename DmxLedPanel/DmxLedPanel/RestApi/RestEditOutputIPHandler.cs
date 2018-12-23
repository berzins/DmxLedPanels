using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditOutputIPHandler : HttpRequestHandler
    {

        public static readonly string KEY_IP = "ip";
        public static readonly string KEY_OUTPUT_ID = "output_id";

        public override void HandleRequest(HttpListenerContext context)
        {

            var q = context.Request.QueryString;

            try
            {
                var ip = q.Get(KEY_IP).Replace('_', '.');
                var ids = getIntArgArray(q.Get(KEY_OUTPUT_ID));

                if (ip == null)
                {
                    throw new ArgumentNullException("Some of properties was null in edit output ip handler.");
                }

                var outputs = StateManager.Instance.State.GetOutputs(ids);

                var i = 0;
                foreach (var o in outputs)
                {
                    o.IP = ip;
                }


                SetInfoMessage(
                    "The IP for otputs: " + Output.GetOutputListNameString(outputs)
                    + " is set to: " + ip,
                    IS_PART_OF_STATE,
                    Talker.Talker.GetSource()
                    );

                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                    StateManager.Instance.GetStateSerialized()
                    );
            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
