
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestSwitchModeHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_MODE_ID = "mode_id";
        

        public override void HandleRequest(HttpListenerContext context)
        {

            var q = context.Request.QueryString;

            try
            {
                var modeId = int.Parse(q.Get(KEY_MODE_ID));
                var fixIds = getIntArgArray(KEY_FIXTURE_ID);

                var fixtures = StateManager.Instance.State.GetFixtures(fixIds);
                bool isModeSwitchedForAllFixtures = true;

                foreach (var fix in fixtures) {
                    if (!fix.TrySwitchMode(modeId)) {
                        isModeSwitchedForAllFixtures = false;
                    }
                }
                
                SetInfoMessage(
                        "The mode for fixture: " + Fixture.GetFixtureListNameString(fixtures)
                        + " is set to index: " + modeId,
                        IS_NOT_PART_OF_STATE,
                        Talker.Talker.GetSource()
                        );
                Log();

                if (!isModeSwitchedForAllFixtures)
                {
                    SetWarningMessage(
                        "Not all fixtures has switched mode.. probably not existing mode index",
                        IS_NOT_PART_OF_STATE,
                        Talker.Talker.GetSource()
                        );
                    Log();
                }
            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
