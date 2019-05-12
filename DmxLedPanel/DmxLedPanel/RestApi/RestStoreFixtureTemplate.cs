using DmxLedPanel.State;
using DmxLedPanel.Template;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    class RestStoreFixtureTemplateHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIXTURE_ID = "fixture_id";


        public override void HandleRequest(HttpListenerContext context)
        {

            try
            {
                var q = context.Request.QueryString;
                var fixtureId = q.Get(KEY_FIXTURE_ID);

                if (fixtureId == null)
                {
                    throw new ArgumentNullException("Some of fixture template properties was null");
                }

                if (!StateManager.Instance.State.TryGetFixture(int.Parse(fixtureId), out Fixture fixture))
                {
                    throw new ArgumentException("Fixture with id: " + fixtureId + " does not exist: sorry!");
                }

                var template = FixtureTemplateFactory.createFixtureTemplate(fixture);

                FixtureTemplateUtils.StoreTemplate(template);

                SetInfoMessage(
                        "Fixture template: '" + template.Name + "' successfully stored.",
                        IS_NOT_PART_OF_STATE,
                        Talker.Talk.GetSource()
                        );

                //get all templates and send back as a new template state
                List<FixtureTemplate> templates = FixtureTemplateUtils.GetTemplates();
                ResponseMessage msg = new ResponseMessage(ResponseMessage.TYPE_FIXTURE_TEMPLATE, templates);
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, StaticSerializer.Serialize(msg));

            }
            catch (Exception e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
