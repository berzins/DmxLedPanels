using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestMoveFixtureToOutputHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_OUTPUT_ID = "output_id";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fixIds = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var outId = int.Parse(q.Get(KEY_OUTPUT_ID));
                var state = StateManager.Instance.State;

                foreach(int fixId in fixIds) {
                    if (!state.ContainFixture(fixId)) {
                        throw new ArgumentException("State fixture pool does not contain fixture with id '" + fixId + "'");
                    }
                }
                if (!state.ContainOutput(outId)) {
                    throw new ArgumentException("State outputs does not contain output with id '" + outId + "'");
                }

                var fixtures = state.GetFixtures(fixIds);
                var success = state.GetOutput(outId).
                    TryPatchFixtures(fixtures);
                if (!success) {
                    throw new ArgumentOutOfRangeException("Faild to patch fixtures.. not enough address space");
                }

                state.RemoveFixtures(fixtures);

                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
