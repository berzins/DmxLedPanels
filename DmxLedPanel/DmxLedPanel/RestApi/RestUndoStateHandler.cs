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
                SetInfoMessage(
                        "'Undo' action successful.",
                        IS_NOT_PART_OF_STATE,
                        Talker.Talker.GetSource()
                        );
                WriteResponse(
                    context, RestConst.RESPONSE_OK, 
                    RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
