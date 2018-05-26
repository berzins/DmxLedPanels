using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public abstract class HttpRequestHandler : IHttpRequestHandler
    {

        public static readonly char[] PARAM_SPLITTER = {  '|' };
        public static readonly char[] VALUE_SPLITTER = { '_' };
        public static readonly char[] ITEM_SPLITTER = { '^' };

        public static readonly string KEY_ID = "id";

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
            var error = Util.StaticSerializer.Serialize(
                    new ResponseMessage(ResponseMessage.TYPE_ERROR, e.Message + " || " + e.StackTrace));
            WriteResponse(context, RestConst.RESPONSE_INTERNAL_ERROR, RestConst.CONTENT_TEXT_JSON, error);
        }

        protected int[] getIntArgArray(string args) {
            return args.Split(PARAM_SPLITTER).Select(x => int.Parse(x)).ToArray();
        }
    }
}
