using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditOutputPortHandler : HttpRequestHandler
    {
        public static readonly string KEY_OUTPUT_ID = "output_id";
        public static readonly string KEY_PORT = "port";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var po = getIntArgArray(q.Get(KEY_PORT));
                var oids = getIntArgArray(q.Get(KEY_OUTPUT_ID));

                if (po == null || oids == null)
                {
                    throw new ArgumentNullException("Some of output edit properties was null");
                }

                var outputs = StateManager.Instance.State.GetOutputs(oids);
                var port = new Port() { Net = po[0], SubNet = po[1], Universe = po[2] };
                
                foreach (var o in outputs)
                {
                    var ports = new List<Port>();
                    ports.Add(port++);
                    ports.Add(port++);
                    o.Ports = ports;
                }

                var state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e)
            {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }
        }
    }
}
