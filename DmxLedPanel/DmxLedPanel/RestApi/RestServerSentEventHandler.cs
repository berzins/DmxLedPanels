using DmxLedPanel.ArtNetIO;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestServerSentEventHandler : HttpRequestHandler
    {
        public static readonly string EVENT_DMX_SIGNAL = "dmx_sginal";
        public static readonly string EVENT_PORT_DMX_SIGNAL = "dmx_signal_port";

        public static readonly string KEY_HAS_DMX_SGINAL = "has_dmx_sginal";
        public static readonly string PORTS_WITH_DMX_SIGNAL = "dmx_active_port";


        private static RestServerSentEventHandler instance;
        private List<HttpListenerContext> remoteListeners;

        private RestServerSentEventHandler() {
            remoteListeners = new List<HttpListenerContext>();
            // dmx signal event handling. 
            ArtnetIn.Instance.DmxSignalChanged += hasSignal =>
            {
                remoteListeners.ForEach(context =>
                {
                    SendDmxSignalEvent(context, hasSignal);
                });
            };

            ArtnetIn.Instance.PortDmxSignalChanged += activeProts => {
                remoteListeners.ForEach(context => {
                    SendPortDmxSignalEvent(context, activeProts);
                });
            };
            
        }

        public static RestServerSentEventHandler Instance {
            get {
                if(instance == null)
                {
                    instance = new RestServerSentEventHandler();
                }
                return instance;
            }
        }
        
        public override void HandleRequest(HttpListenerContext context)
        {
            // send empty to keep connection open
            WriteServerSentEvent(context, 200, "none", null);
            SendDmxSignalEvent(context, ArtnetIn.Instance.HasSignal);
            SendPortDmxSignalEvent(context, ArtnetIn.Instance.PortsWithDmxSignal);
            remoteListeners.Add(context);           
        }

        public void OnEvent(string type, Dictionary<string, string> data)
        {
            remoteListeners.ForEach(l => WriteServerSentEvent(l, RestConst.RESPONSE_OK, type, data));
        }

        protected void WriteServerSentEvent(
            HttpListenerContext context,
            int status,
            string eventType,
            Dictionary<string, string> data)
        {
            var response = context.Response;
            // Check if connection isn't dead.


            if (!response.OutputStream.CanWrite) {
                LogError("The '" + context.Request.RemoteEndPoint 
                    + "' has died for some reason. Removing this client.");
                CloseConnection(context);
                return;
            }

            // All good.. write some response. 
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Cache-Control", "no-cache");
            response.KeepAlive = true;
            response.StatusCode = status;
            response.ContentType = "text/event-stream";
            if (data != null)
            {
                var eventData = Encoding.UTF8.GetBytes("event:" + eventType + "\n");
                var dataData = Encoding.UTF8.GetBytes("data:" + GetSSEDataString(data));
                var endData = Encoding.UTF8.GetBytes("\n\n");
                response.OutputStream.Write(eventData, 0, eventData.Length);
                response.OutputStream.Write(dataData, 0, dataData.Length);
                response.OutputStream.Write(endData, 0, endData.Length);
                LogInfo("Sending event (type: " + eventType 
                    + ", content: " + StringUtil.MapToString(data) 
                    + ") to: " + context.Request.RemoteEndPoint.Address);
            }
            else
            {
                response.OutputStream.Write(new byte[0], 0, 0);
                LogInfo("Sending 'keep-alive' event stream to: " + context.Request.RemoteEndPoint.Address);
            }
        }

        private string GetSSEDataString(Dictionary<string, string> data)
        {
            return StaticSerializer.Serialize(data)
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(" ", "");
        }

        public void ClearConnectons() {
            remoteListeners.ForEach(l => CloseConnection(l));
            remoteListeners.Clear();
        }

        private void CloseConnection(HttpListenerContext context) {
            context.Response.OutputStream.Close();
            remoteListeners.Remove(context);
            context = null;
        }

        private void Log(string msg, int level) {
            Talker.Talker.Log(new Talker.ActionMessage() {
                Message = msg,
                Level = level,
                Source = Talker.Talker.GetSource()
            });
        }
        
        private Dictionary<string, string> GetDmxSingalData(bool hasSignal) {
            return new Dictionary<string, string>
                {
                    { KEY_HAS_DMX_SGINAL, hasSignal + "" }
                };
        }

        private void SendDmxSignalEvent(HttpListenerContext context, bool hasSignal) {
            WriteServerSentEvent(
                        context,
                        RestConst.RESPONSE_OK,
                        EVENT_DMX_SIGNAL,
                        GetDmxSingalData(hasSignal));
        }

        private void SendPortDmxSignalEvent(HttpListenerContext context, List<Port> activePorts) {
            WriteServerSentEvent(
                context,
                RestConst.RESPONSE_OK,
                EVENT_PORT_DMX_SIGNAL,
                new Dictionary<string, string>
                {
                    { PORTS_WITH_DMX_SIGNAL, StaticSerializer.Serialize(activePorts) }
                });
        }


        private void LogInfo(string msg) {
            Log(msg, Talker.LogLevel.INFO);
        }

        private void LogError(string msg) {
            Log(msg, Talker.LogLevel.ERROR);
        }
    }
}
