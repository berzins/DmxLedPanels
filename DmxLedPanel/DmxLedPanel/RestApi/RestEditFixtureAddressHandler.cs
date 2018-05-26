using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditFixtureAddressHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_PORT = "port";
        public static readonly string KEY_ADDRESS = "address";
        public static readonly string KEY_INCREMENT = "increment";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var port = getIntArgArray(q.Get(KEY_PORT));
                var addr = q.Get(KEY_ADDRESS);
                var increment = bool.Parse(q.Get(KEY_INCREMENT));

                if (fids == null || port == null || addr == null)
                {
                    throw new ArgumentNullException("Some of fixture edit properties was null");
                }

                Address address = new Address()
                {
                    DmxAddress = int.Parse(addr),
                    Port = new Port()
                    {
                        Net = port[0],
                        SubNet = port[1],
                        Universe = port[2]
                    }
                };

                var fixtures = StateManager.Instance.State.GetFixtures(fids);
    
                var i = 0;
                var offset = 0;
                foreach (var f in fixtures)
                {
                    if (increment)
                    {
                        if (address.DmxAddress + f.Address.DmxAddress > 512) {
                            address += f.InputAddressCount;
                            f.Address = address.Clone();
                            address += f.InputAddressCount;
                        } else {
                            f.Address = address.Clone();
                            address += f.InputAddressCount;
                        }
                    }
                    else
                    {
                        f.Address = address.Clone();
                    }
                    i++;
                }

                string state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);

            }
            catch (Exception e)
            {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }

        }
    }
}
