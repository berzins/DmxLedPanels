using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditFixtureModeHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_MODE = "mode";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var mode = q.Get(KEY_MODE);

                if (fids == null || mode == null)
                {
                    throw new ArgumentNullException("Some of fixture edit properties was null");
                }

                var fixtures = StateManager.Instance.State.GetFixtures(fids);
                
                foreach (var f in fixtures)
                {
                    f.SetMode(RestCreateFixtureHandler.getFixtureMode(mode));
                }

                string state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);

            }
            catch (Exception e)
            {
                var msg = new ResponseMessage(ResponseMessage.TYPE_ERROR, e.Message + " ..... " + e.StackTrace);
            }

        }
    }
}

