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
        public abstract void HandleRequest(HttpListenerContext context);


        protected void WriteResponse(
            HttpListenerContext context,
            int status,
            string contType,
            byte[] data)
        {
            var response = context.Response;
            response.StatusCode = status;
            response.ContentType = contType;
            response.ContentLength64 = data.Length;
            response.OutputStream.Write(data, 0, data.Length);
            response.OutputStream.Close();          
        }

        protected void WriteResponse(
            HttpListenerContext context, 
            int status, 
            string contType, 
            string data)
        {
            WriteResponse(context, status, contType, Encoding.UTF8.GetBytes(data));
        }
    }
}
