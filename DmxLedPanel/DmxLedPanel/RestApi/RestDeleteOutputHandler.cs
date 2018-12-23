using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestDeleteOutputHandler : HttpRequestHandler
    {
        
        public override void HandleRequest(HttpListenerContext context)
        {

            var q = context.Request.QueryString;
            try
            {
                var ids = getIntArgArray(q.Get(KEY_ID));
                var state = StateManager.Instance.State;

                //check if all outputs exist
                foreach (int id in ids) {
                    if (!state.ContainOutput(id)) { throw new ArgumentNullException("Remove faild.. output with id: " + id + " not found."); }
                }

                //remove outputs and move contained fixtures to fixture pool
                var rmOutputs = new List<Output>();
                foreach (int id in ids) {
                    var o = state.RemoveOutput(id);
                    var fixIds = new List<int>();

                    foreach (Fixture f in o.GetFixtures()) {
                        fixIds.Add(f.ID);
                    }
                    foreach (int fixId in fixIds) {
                        state.FixturePool.Add(o.UnpatchFixture(fixId));
                    }
                    rmOutputs.Add(o);
                }

                SetInfoMessage( 
                    "Output with name: "
                    + rmOutputs.Aggregate("", (s, o) => s + o.Name + ",")
                    + "removed."
                    , IS_PART_OF_STATE,
                    Talker.Talker.GetSource());


                // send back state
                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);

            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
