using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditOutputNameHandler : HttpRequestHandler
    {

        public static readonly string KEY_NAME = "name";
        public static readonly string KEY_OUTPUT_ID = "output_id";

        public override void HandleRequest(HttpListenerContext context)
        {

            var q = context.Request.QueryString;

            try
            {
                var name = q.Get(KEY_NAME);
                var ids = getIntArgArray(q.Get(KEY_OUTPUT_ID));
               
                if (name == null) {
                    throw new ArgumentNullException("Some of properties was null in Eidit Output handler.");
                }

                var outputs = StateManager.Instance.State.GetOutputs(ids);

                var i = 0;
                foreach (var o in outputs) {
                    o.Name = name + " " + i++;
                }

                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                    StateManager.Instance.GetStateSerialized()
                    );
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
