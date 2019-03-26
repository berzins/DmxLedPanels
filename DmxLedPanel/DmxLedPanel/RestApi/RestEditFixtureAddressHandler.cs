using DmxLedPanel.Fixtures;
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
        public static readonly string KEY_UTIL_ADDRESS = "util_address";
        public static readonly string KEY_UTIL_ENABLED = "util_enabled";
        public static readonly string KEY_INCREMENT = "increment";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var portStr = getIntArgArray(q.Get(KEY_PORT));
                var addr = q.Get(KEY_ADDRESS);
                var increment = bool.Parse(q.Get(KEY_INCREMENT));
                var utilEnabled = bool.Parse(q.Get(KEY_UTIL_ENABLED));

                if (fids == null || portStr == null || addr == null)
                {
                    throw new ArgumentNullException("Some of fixture edit properties was null");
                }

                var port = new Port()
                {
                    Net = portStr[0],
                    SubNet = portStr[1],
                    Universe = portStr[2]
                };

                var address = new Address()
                {
                    DmxAddress = int.Parse(addr),
                    Port = port.Clone()
                };

                var utilAddress = new Address()
                {
                    Port = port.Clone(),
                    DmxAddress = int.Parse(q.Get(KEY_UTIL_ADDRESS))
                };

                var fixtures = StateManager.Instance.State.GetFixtures(fids);
                FixtureAddressSetter.SetDmxAddressFor(fixtures, address, increment);

                SetInfoMessage(
                    "Address for fixtures: " + Fixture.GetFixtureListNameString(fixtures)
                    + "is set to => "
                    + "address: " + address.ToString()
                    + "util address: " + utilAddress.ToString(),
                    IS_PART_OF_STATE,
                    Talker.Talker.GetSource());

                string state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);

            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }

        }

        private void setAddresses(Fixture f, Address address, Address utilAddress, bool utilEnabled) {
            f.Address = address.Clone();
            f.UtilAddress = utilAddress.Clone();
            f.IsDmxUtilsEnabled = utilEnabled;
        }
    }
}
