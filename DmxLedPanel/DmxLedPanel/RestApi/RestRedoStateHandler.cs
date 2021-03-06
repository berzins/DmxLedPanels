﻿using System;
using System.Net;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestRedoStateHandler : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var state = StateManager.Instance.Redo(1).Serialize();
                SetInfoMessage(
                        "'Redo' action successful.",
                        IS_NOT_PART_OF_STATE,
                        Talker.Talk.GetSource()
                        );
                WriteResponse(
                    context, RestConst.RESPONSE_OK,
                    RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
