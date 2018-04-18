using System;
using System.Net;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestUndoStateHandler : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var state = StateManager.Instance.Undo(1).Serialize();
                WriteResponse(
                    context, RestConst.RESPONSE_OK, 
                    RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
