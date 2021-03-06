﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.ArtNetIO;

namespace DmxLedPanel.RestApi
{
    public class RestDmxSignalHandler : HttpRequestHandler, IDmxSignalListener
    {

        public static readonly string KEY_HAS_SIGNAL = "has_signal";

        HttpListenerContext context = null;

        public override void HandleRequest(HttpListenerContext cntx)
        {
            this.context = cntx;
            try
            {

                // in case UI and server has different states, just sync them. 
                var hasSignal = bool.Parse(context.Request.QueryString.Get(KEY_HAS_SIGNAL));
                if (hasSignal != ArtnetIn.Instance.HasSignal)
                {
                    Respond(context, ArtnetIn.Instance.HasSignal);
                    return;
                }

                // wait for signal state change 
                ArtnetIn.Instance.DmxSignalChanged += OnSignalChange;

            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }

        // this is triggered when signal state changes
        public void OnSignalChange(bool detected)
        {
            try
            {
                SetInfoMessage(
                    "Dmx signal " + (detected ? "detected" : "lost")
                    , IS_PART_OF_STATE,
                    Talker.Talk.GetSource());
                context.Request.GetClientCertificate();
                Respond(this.context, detected);
            }
            catch (Exception e)
            {
                LogException(
                    "Dmx state response failed. Refresh UI to sync dmx state" + "\n\r"
                    + e.Message,
                    Talker.Talk.GetSource());
            }

            // our job is done.. wait for next ui call
            try
            {
                ArtnetIn.Instance.DmxSignalChanged -= OnSignalChange;
            }
            catch (Exception ee)
            {
                Console.WriteLine("Dmx signal litener remove failed." + ee.Message);
            }
        }

        private void Respond(HttpListenerContext context, bool hasSignal)
        {
            var msg = new ResponseMessage(
                        ResponseMessage.TYPE_DMX_IN_STATE,
                        hasSignal
                        );
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                Util.StaticSerializer.Serialize(msg));
        }
    }
}
