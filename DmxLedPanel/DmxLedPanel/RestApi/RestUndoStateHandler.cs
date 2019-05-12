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
                        "'Undo' successful. State message: " + StateManager.Instance.State.ActionMessage.Message,
                        IS_NOT_PART_OF_STATE,
                        Talker.Talk.GetSource()
                        );
                WriteResponse(
                    context, RestConst.RESPONSE_OK, 
                    RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
