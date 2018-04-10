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
                foreach (int id in ids) {
                    var o = state.RemoveOutput(id);
                    var fixIds = new List<int>();

                    foreach (Fixture f in o.GetFixtures()) {
                        fixIds.Add(f.ID);
                    }
                    foreach (int fixId in fixIds) {
                        state.FixturePool.Add(o.UnpatchFixture(fixId));
                    }
                }

                // send back state
                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);

            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }
    }
}
