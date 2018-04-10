using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.State;

namespace DmxLedPanel.RestApi
{
    public class RestDeleteFixtureHandler : HttpRequestHandler
    {
        
        public override void HandleRequest(HttpListenerContext context)
        {

            var q = context.Request.QueryString;
            try
            {
                var ids = getIntArgArray(q.Get(KEY_ID));
                var state = StateManager.Instance.State;
                foreach (int id in ids) {
                    if (!state.ContainFixture(id)) {
                        throw new ArgumentNullException("Fixture remove failed.. fixture  by id '" + id + "'not found");
                    }          
                }

                foreach (int id in ids) {
                    state.RemoveFixture(id);
                }

                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);

            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
           
        }
    }
}
