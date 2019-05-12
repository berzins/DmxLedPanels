using DmxLedPanel.State;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestLoadStateHandler : HttpRequestHandler
    {
        public static readonly string KEY_FILE_NAME = "file_name";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;

            try
            {
                var fileName = q.Get(KEY_FILE_NAME);
                StateManager.Instance.LoadStateFromFile(fileName);
                SettingManager.Instance.Settings.CurrentProject = fileName;
                SettingManager.Instance.Save();

                SetInfoMessage(
                        "State: '" + fileName + " 'is loaded.",
                        IS_PART_OF_STATE,
                        Talker.Talk.GetSource()
                        );

                var data = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, data);
                
            }
            catch (Exception e)
            {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }
    }
}
