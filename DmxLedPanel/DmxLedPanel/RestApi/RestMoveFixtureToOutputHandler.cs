using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;
using DmxLedPanel.ArtNetIO;

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

                var fixtures = state.GetFixturesFromFixturePool(fixIds);

                // Lets get patched fixtures and their outputs
                var patchedFixtureOutputs = Output.GetPatchedFixtureOutputMap(fixtures);
                foreach (var fo in patchedFixtureOutputs)
                {
                    fo.Output.UnpatchFixture(fo.FixtureId);
                }

                var output = state.GetOutput(outId);
                var success = output.TryPatchFixtures(fixtures);
                if (!success) {
                    // patch unpatched fixtures back so we don't loose them forever
                    foreach (var fo in patchedFixtureOutputs) {
                        if (!state.GetOutput(fo.Output.ID).TryPatchFixture(fo.Fixture))
                        {
                            throw new InvalidOperationException("Faild to repatch fixtures on patch cancelation. I suggest you to do the 'undo' as this state is more than unexpected! :)");
                        }
                    }
                    throw new ArgumentOutOfRangeException("Faild to patch fixtures.. not enough address space");
                }

                state.RemoveFixturesFromFixturePool(fixtures);

                ArtnetIn.Instance.UpdateDmxPacketListeners(
                    StateManager.Instance.State.GetPatchedFixtures().
                    Select(x => (IDmxPacketHandler)x).ToList()
                    );

                SetInfoMessage(
                        "Fixtures: " + Fixture.GetFixtureListNameString(fixtures)
                        + " is patched to output: " + output.Name,
                        IS_PART_OF_STATE,
                        Talker.Talker.GetSource()
                        );

                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
