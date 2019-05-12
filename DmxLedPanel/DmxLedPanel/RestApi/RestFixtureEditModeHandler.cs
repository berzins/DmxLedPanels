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
    public class RestEditFixtureModeHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_MODES = "modes";


        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var modesStr = (q.Get(KEY_MODES));

                if (fids == null || modesStr == null)
                {
                    throw new ArgumentNullException("Some of fixture edit properties was null");
                }

                var fixtures = StateManager.Instance.State.GetFixtures(fids);

                //validate pixel and mode values
                // The validation method will throw an Argument exception if mode values are not valid
                var modes =  RestCreateFixtureHandler.GetFixtureModes(modesStr);
                foreach (var fix in fixtures) {
                    RestCreateFixtureHandler.ValidateModeValues(modes, fix.PixelPatch);
                }

                // modes are valid .. 
                foreach (var fix in fixtures)
                {
                    fix.SetModes(modes);
                }

                SetInfoMessage(
                    "The modes for fixture: " + Fixture.GetFixtureListNameString(fixtures)
                    + "is set to: " + Mode.ModeListToString(modes),
                    IS_PART_OF_STATE,
                    Talker.Talk.GetSource()
                    );

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

