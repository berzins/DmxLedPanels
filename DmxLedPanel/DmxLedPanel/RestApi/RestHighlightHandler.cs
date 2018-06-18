using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestHighlightHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";
        public static readonly string KEY_OUTPUT_ID = "output_id";
        

        public override void HandleRequest(HttpListenerContext context)
        {

            if (!HighlightState.Instance.Enabled) {
                var msg = Util.StaticSerializer.Serialize(new ResponseMessage(ResponseMessage.TYPE_INFO, "Highlight is disabled"));
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTETN_TEST_PLAIN, msg);
            }

            var q = context.Request.QueryString;
            var state = StateManager.Instance.State;


            int[] fids, oids;
            
            try
            {
                try {
                    fids = getIntArgArray(q.Get(KEY_FIXTURE_ID));
                } catch ( FormatException e) {
                    fids = new int[0];
                }

                try
                {
                    oids = getIntArgArray(q.Get(KEY_OUTPUT_ID));
                }
                catch (FormatException e)
                {
                    oids = new int[0];
                }

                HashSet<Fixture> highlightf = new HashSet<Fixture>();
                var allFixtures = state.GetAllFixtures();

                // get fixtures to highlight
                foreach (int id in fids) {
                    var f = state.GetFixtureFromFixturePool(id);
                    if (f != null) {
                        highlightf.Add(f);
                    }
                    
                }
                var outs = new List<Output>();
                foreach (var id in oids) {
                    var output = state.GetOutput(id);
                    if (output != null) {
                        foreach (var f in output.GetFixtures())
                        {
                            highlightf.Add(f);
                        }
                    }
                }

                // clear all highlights
                foreach(var f in allFixtures)
                {
                    f.SetHighlight(false);
                }

                // highligh selected fixtures
                foreach (var f in highlightf) {
                    f.SetHighlight(true);
                }

                // Trigger ouptus to send out data
                var patchedFix = state.GetPatchedFixtures();
                foreach(var f in patchedFix)
                {
                    f.Update();
                }

                var hids = new List<int>();
                foreach (var f in highlightf) {
                    hids.Add(f.ID);
                }

                var msg = Util.StaticSerializer.Serialize(new ResponseMessage(ResponseMessage.TYPE_INFO,
                    Util.StaticSerializer.Serialize(hids)
                    ));
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, msg);
                
            }
            catch (Exception e)
            {
                Utils.LogException(e);
                WriteErrorMessage(context, e);
            }
        }
    }
}
