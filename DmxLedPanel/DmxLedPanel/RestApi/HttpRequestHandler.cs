using DmxLedPanel.RestApi.Log;
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Talker;

namespace DmxLedPanel.RestApi
{
    public abstract class HttpRequestHandler 
        : MessageLogger, 
        IHttpRequestHandler
    {

        public static readonly char[] PARAM_SPLITTER = {  '|' };
        public static readonly char[] VALUE_SPLITTER = { '_' };
        public static readonly char[] ITEM_SPLITTER = { '^' };

        public static readonly string KEY_ID = "id";

        protected string name = "";

        public string Name {
            get {
                return name;
            }
        }

        public abstract void HandleRequest(HttpListenerContext context);

        protected void WriteResponse(
            HttpListenerContext context,
            int status,
            string contType,
            byte[] data)
        {
            var response = context.Response;
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.StatusCode = status;
            response.ContentType = contType;
            response.ContentLength64 = data.Length;
            response.OutputStream.Write(data, 0, data.Length);
            response.OutputStream.Close();
            response = null;
            context = null;
            Log();
        }

        protected void WriteResponse(
            HttpListenerContext context, 
            int status, 
            string contType, 
            string data)
        {
            WriteResponse(context, status, contType, Encoding.UTF8.GetBytes(data));
        }

        public void WriteErrorMessage(HttpListenerContext context, Exception e) {
            Message.Message = e.ToString();
            var error = Util.StaticSerializer.Serialize(
                    new ResponseMessage(ResponseMessage.TYPE_ERROR, e.Message));
            WriteResponse(context, RestConst.RESPONSE_INTERNAL_ERROR, RestConst.CONTENT_TEXT_JSON, error);
        }

        protected static int[] getIntArgArray(string args) {
            return args.Split(PARAM_SPLITTER).Select(x => int.Parse(x)).ToArray();
        }
    }

}
