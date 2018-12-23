using System;
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
                if (hasSignal != ArtnetIn.Instance.HasSignal) {
                    Respond(context, ArtnetIn.Instance.HasSignal);
                    return;
                }

                // wait for signal state change 
                ArtnetIn.Instance.AddDmxSignalListener(this);

            }
            catch (Exception e) {
                Utils.LogException(e);
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
                    Talker.Talker.GetSource());
                context.Request.GetClientCertificate();
                Respond(this.context, detected);
            }
            catch (Exception e) {
                LogException(
                    "Dmx state response failed. Refresh UI to sync dmx state" + "\n\r" 
                    + e.Message,
                    Talker.Talker.GetSource());
            }

            // our job is done.. wait for next ui call
            try
            {
                ArtnetIn.Instance.RemoveDmxSignalListener(this);
            }
            catch (Exception ee)
            {
                Console.WriteLine("Dmx signal litener remove failed." + ee.Message);
                ArtnetIn.Instance.RemoveDmxSignalListener(this);
            } 
        }

        private void Respond(HttpListenerContext context, bool hasSignal) {
            var msg = new ResponseMessage(
                        ResponseMessage.TYPE_DMX_IN_STATE,
                        hasSignal
                        );
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                Util.StaticSerializer.Serialize(msg));
        }
    }
}
