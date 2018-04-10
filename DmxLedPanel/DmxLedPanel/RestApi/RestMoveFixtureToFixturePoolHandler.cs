using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestMoveFixtureToFixturePoolHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fixIds = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                var state = StateManager.Instance.State; 
                var fixOutMap = new List<FixtureOutputMap>();
               
                
                foreach (int id in fixIds) {
                    var output = state.GetOutputByFixture(id);
                    if (output == null) {
                        throw new ArgumentException("No ouputs contain fixture with id '" + id + "', move failed.");
                    }
                    fixOutMap.Add(new FixtureOutputMap(id, output));        
                }

                foreach (FixtureOutputMap fom in fixOutMap) {
                    state.FixturePool.Add(
                        fom.Output.UnpatchFixture(
                            fom.FixtureId));
                }

                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);

                
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }

        private class FixtureOutputMap {

            public FixtureOutputMap(int id, Output o) {
                FixtureId = id;
                Output = o;
            }

            public int FixtureId { get; set; }
            public Output Output { get; set; }
        }

    }
}
