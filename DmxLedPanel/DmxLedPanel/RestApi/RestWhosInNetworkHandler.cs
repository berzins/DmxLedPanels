using DmxLedPanel.ArtNetIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    class RestWhosInNetworkHandler : HttpRequestHandler
    {

        HttpListenerContext context = null;
        WhosInNetwork.DevicesChangedDelegate handler;

        public override void HandleRequest(HttpListenerContext cntx)
        {
            this.context = cntx;
            try
            {

                handler = devices =>
                {
                    Respond(this.context, devices);
                    RemoveHandler();
                };

                ArtnetOut.Instance.WhoTheFuckAreYou.DevicesChanged += handler;

            }
            catch (Exception e) {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }
        }

        private void Respond(HttpListenerContext context, List<ArtNetDevice> devices) {
            var msg = new ResponseMessage(
                ResponseMessage.TYPE_HWOS_IN_NETWORK,
                devices
                );
            WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                Util.StaticSerializer.Serialize(msg));
        }

        private void RemoveHandler() {
            ArtnetOut.Instance.WhoTheFuckAreYou.DevicesChanged -= handler;
            handler = null;
        }
    }
}
