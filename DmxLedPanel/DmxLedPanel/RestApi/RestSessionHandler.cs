using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestSessionHandler : HttpRequestHandler
    {

        public static readonly string PASSOWRD = "pass";
        public override void HandleRequest(HttpListenerContext context)
        {
            try
            {
                var q = context.Request.QueryString;
                var pswd = q.Get(PASSOWRD);
                var pswd2 = SettingManager.Instance.Settings.Password;
                var correct = pswd.Equals(pswd2);
                var body = new ResponseBody()
                {
                    Logged = correct ? true : false,
                    Message = correct ? "logged" : "wrong password"
                };

                var msg = new ResponseMessage(ResponseMessage.TYPE_SESSION, body);
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON,
                    StaticSerializer.Serialize(msg)
                    );
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }

        class ResponseBody
        {
            public bool Logged { get; set; }
            public String Message { get; set; }
        }
    }
}
