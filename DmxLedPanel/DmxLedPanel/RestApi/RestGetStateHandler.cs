using DmxLedPanel.State;
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
            var state = StateManager.Instance.State.Serialize();
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);
        }
    }
}
