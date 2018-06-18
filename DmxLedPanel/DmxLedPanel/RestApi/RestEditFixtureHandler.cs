using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditFixtureHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_NAME = "name";
        public static readonly string KEY_PORT = "port";
        public static readonly string KEY_ADDRESS = "address";
        public static readonly string KEY_PATCH_TYPE = "patch_type";
        public static readonly string KEY_MODE = "mode";
        public static readonly string KEY_INCREMENT = "increment";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var name = q.Get(KEY_NAME);
                var port = getIntArgArray(q.Get(KEY_PORT));
                var addr = q.Get(KEY_ADDRESS);
                var mode = q.Get(KEY_MODE);
                var patch = q.Get(KEY_PATCH_TYPE);
                var increment = bool.Parse(q.Get(KEY_INCREMENT));

                if (fids == null || name == null || port == null || 
                    addr == null || mode == null || patch == null) {
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
                foreach(var f in fixtures)
                {
                    f.TrySetPatch(RestCreateFixtureHandler.getPixelPatch(patch));
                    f.SetMode(RestCreateFixtureHandler.GetFixtureMode(mode));
          
                    f.Name = name + " " + i;
                    if (increment)
                    {
                        f.Address = address.Clone();
                        f.Address.DmxAddress += f.InputAddressCount * i;
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
            catch (Exception e) {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }
            
        }
    }
}
