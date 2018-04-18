using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestCreateOutputHandler : HttpRequestHandler
    {
        public static readonly string KEY_NAME = "name";
        public static readonly string KEY_PORT = "port";
        public static readonly string KEY_IP = "ip";
        public static readonly string KEY_COUNT = "count";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;
            List<Output> outputs = new List<Output>();
            
            try
            {

                var name = q.Get(KEY_NAME);
                var pv = getIntArgArray(q.Get(KEY_PORT));
                var port = new Port() { Net = pv[0], SubNet = pv[1], Universe = pv[2] };
                var ip = q.Get(KEY_IP).Replace('_', '.');

                for (int i = 0; i < int.Parse(q.Get(KEY_COUNT)); i++) {
                    var ports = new List<Port>();
                    ports.Add(port++);
                    ports.Add(port++);
                    outputs.Add(new Output() {
                        Name = name + " " + i,
                        Ports = ports,
                        IP = ip
                    });    
                }
                StateManager.Instance.State.Outputs.AddRange(outputs);
                var state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
