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
        private Dictionary<String, HttpListenerContext> remoteListeners;

        private RestServerSentEventHandler() {
            remoteListeners = new Dictionary<String, HttpListenerContext>();
            // dmx signal event handling. 
            ArtnetIn.Instance.DmxSignalChanged += hasSignal =>
            {
                foreach (KeyValuePair<String, HttpListenerContext> item in remoteListeners) {
                    SendDmxSignalEvent(item.Value, hasSignal);
                }
            };

            ArtnetIn.Instance.PortDmxSignalChanged += activeProts => {
                foreach (KeyValuePair<String, HttpListenerContext> item in remoteListeners)
                {
                    SendPortDmxSignalEvent(item.Value, activeProts);
                }
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
            string clientIp = context.Request.RemoteEndPoint.Address.ToString();
            if (remoteListeners.TryGetValue(clientIp, out HttpListenerContext c)) {
                CloseConnection(c);
            }
            remoteListeners.Add(clientIp, context);           
        }

        public void OnEvent(string type, Dictionary<string, string> data)
        {
            foreach (KeyValuePair<String, HttpListenerContext> item in remoteListeners)
            {
                WriteServerSentEvent(item.Value, RestConst.RESPONSE_OK, type, data);
            }
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
                var msg = StringUtil.MapToString(data);
                response.OutputStream.Write(eventData, 0, eventData.Length);
                response.OutputStream.Write(dataData, 0, dataData.Length);
                response.OutputStream.Write(endData, 0, endData.Length);
                LogInfo("Sending event (type: " + eventType 
                    + ", content: " + (msg.Length > 100 ? msg.Substring(0, 100) : msg) + "..."
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
            foreach (KeyValuePair<String, HttpListenerContext> item in remoteListeners)
            {
                CloseConnection(item.Value);
            }
            remoteListeners.Clear();
        }

        private void CloseConnection(HttpListenerContext context) {
            context.Response.OutputStream.Close();
            remoteListeners.Remove(context.Request.RemoteEndPoint.Address.ToString());
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

        private void SendPortDmxSignalEvent(HttpListenerContext context, HashSet<Port> activePorts) {
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
