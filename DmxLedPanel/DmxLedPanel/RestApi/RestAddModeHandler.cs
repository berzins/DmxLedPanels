using ArtNet;
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    class RestAddModeHandler : HttpRequestHandler
    {


        public static readonly string KEY_FIXTURE_ID= "fixture_id";
        public static readonly string KEY_MODE_NAME = "name";
        public static readonly string KEY_MODE_COLS = "cols";
        public static readonly string KEY_MODE_ROWS = "rows";
        public static readonly string KEY_SET_CURRENT = "setcurrent";


        // add mode for the fixture
        public override void HandleRequest(HttpListenerContext context)
        {

            var q = context.Request.QueryString;

            try
            {
                var fixIds = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var name = q.Get(KEY_MODE_NAME);
                var cols = int.Parse(q.Get(KEY_MODE_COLS));
                var rows = int.Parse(q.Get(KEY_MODE_ROWS));
                var setCurrent = bool.Parse(q.Get(KEY_SET_CURRENT));

                var fixtures = StateManager.Instance.State.GetFixtures(fixIds);

                foreach (var f in fixtures) {
                    f.AddMode(RestCreateFixtureHandler.GetFixtureMode(name));
                }

                SetInfoMessage(
                    "The mode '" + name + " cols:" + cols + ", rows:" + rows +"' added to fixtures: "
                    + fixtures.Aggregate("", (s, f) => s + f.Name + " "),
                    IS_NOT_PART_OF_STATE,
                    Talker.Talker.GetSource()
                    );
            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
