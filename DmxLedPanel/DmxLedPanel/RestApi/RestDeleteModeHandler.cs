
using DmxLedPanel.Modes;
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    class RestDeleteModeHandler : HttpRequestHandler
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

                var isRemovedForAllFixtures = true;

                var failedFix = new List<Fixture>();
                foreach (var fix in fixtures) {
                    IMode mode = null;
                    if ((mode = fix.TryRemoveMode(modeId)) == null) {
                        failedFix.Add(fix);
                        isRemovedForAllFixtures = false;
                    }
                }

                // let to know that this mode index was not walid for all fixtures.
                if (!isRemovedForAllFixtures) {
                    SetWarningMessage("Mode index was not valid for all fixtures.", IS_PART_OF_STATE,
                        Talker.Talk.GetSource());
                    return;
                }

                SetInfoMessage(
                    "Mode with the index: " + modeId + "wasn't present for fixture '"
                    + failedFix.Aggregate("", (s, f) => s + f.Name + ",") + "'",
                    IS_PART_OF_STATE,
                    Talker.Talk.GetSource());

            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
