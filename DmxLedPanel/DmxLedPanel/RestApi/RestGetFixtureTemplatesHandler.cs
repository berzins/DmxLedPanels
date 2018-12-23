using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    class RestGetFixtureTemplatesHandler : HttpRequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var templates = FixtureTemplateUtils.GetTemplates();
                var msg = new ResponseMessage(ResponseMessage.TYPE_FIXTURE_TEMPLATE, templates);

                SetInfoMessage(
                    "The templates are: " 
                    + StringUtil.RemoveLastChars(
                        templates.Aggregate("", (s, t) => s + t.Name + ", "), 2),
                    IS_NOT_PART_OF_STATE,
                    Talker.Talker.GetSource()
                    );

                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, StaticSerializer.Serialize(msg));
            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talker.GetSource());
                WriteErrorMessage(context, e);
            }



        }
    }
}
