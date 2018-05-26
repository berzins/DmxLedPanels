using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditFixturePixelPatchHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_PATCH_TYPE = "patch_type";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var patch = q.Get(KEY_PATCH_TYPE);

                if (fids == null || patch == null)
                {
                    throw new ArgumentNullException("Some of fixture edit properties was null");
                }
                
                var fixtures = StateManager.Instance.State.GetFixtures(fids);
                
                foreach (var f in fixtures)
                {
                    f.TrySetPatch(RestCreateFixtureHandler.getPixelPatch(patch));    
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
