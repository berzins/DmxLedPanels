using ArtNet;
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

                foreach (var fix in fixtures) {
                    IMode mode = null;
                    if ((mode = fix.TryRemoveMode(modeId)) == null) {
                        isRemovedForAllFixtures = false;
                    }
                }

                // let to know that this mode index was not walid for all fixtures.
                if (!isRemovedForAllFixtures) {
                    Logger.Log("Mode index was not valid for all fixtures.", LogLevel.WARNING);
                } 

            }
            catch (Exception e) {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }
        }
    }
}
