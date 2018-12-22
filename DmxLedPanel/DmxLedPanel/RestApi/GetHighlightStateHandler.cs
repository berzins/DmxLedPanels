using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Talker;

namespace DmxLedPanel.RestApi
{
    public class GetHighlightStateHandler : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            ResponseMessage msg = new ResponseMessage(ResponseMessage.TYPE_INFO,
                State.HighlightState.Instance.Enabled
                );
            
            SetInfoMessage(
                "The highligh state is " + State.HighlightState.Instance.Enabled,
                IS_NOT_PART_OF_STATE
                );
            
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                Util.StaticSerializer.Serialize(msg)
                );
        }
    }
}
