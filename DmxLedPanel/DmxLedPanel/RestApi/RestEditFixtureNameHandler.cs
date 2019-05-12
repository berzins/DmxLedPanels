using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestEditFixtureNameHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_NAME = "name";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var name = q.Get(KEY_NAME);

                if (fids == null || name == null)
                {
                    throw new ArgumentNullException("Some of fixture edit properties was null");
                }


                var fixtures = StateManager.Instance.State.GetFixtures(fids);
                var fixNameList = Fixture.GetFixtureListNameString(fixtures);

                var i = 0;
                foreach (var f in fixtures)
                {
                    f.Name = name + " " + i++;
                }

                SetInfoMessage(
                    "The name for fixtures: " + fixNameList
                    + " is set to: " + name,
                    IS_PART_OF_STATE,
                    Talker.Talk.GetSource());

                string state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);

            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }

        }
    }
}
