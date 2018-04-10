using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestApiServer
    {
        int port = 8746;
        private HttpListener server;
        private bool isRunning = false;
        Dictionary<string, IHttpRequestHandler> requestHandlers;

        public RestApiServer()
        {
            requestHandlers = new Dictionary<string, IHttpRequestHandler>();
            port = SettingManager.Instance.Settings.RestApiPort;
            server = new HttpListener();
            addRequestHandler("/", new RestHomeHandler());
            addRequestHandler("/getState/", new RestGetStateHandler());
            addRequestHandler("/createFixture/", new RestCreateFixtureHandler());
            addRequestHandler("/createOutput/", new RestCreateOutputHandler());
            addRequestHandler("/deleteOutput/", new RestDeleteOutputHandler());
            addRequestHandler("/deleteFixture/", new RestDeleteFixtureHandler());
            addRequestHandler("/moveFixtureToOutput/", new RestMoveFixtureToOutputHandler());
            addRequestHandler("/moveFixtureToFixturePool/", new RestMoveFixtureToFixturePoolHandler());
            addRequestHandler("/saveState/", new RestSaveStateHandler());
            addRequestHandler("/loadState/", new RestLoadStateHandler());
            addRequestHandler("/getSavedStates/", new RestGetSavedStates());
        }
        
        public void Start() {
            if (server != null)
            {
                server.Start();
                isRunning = true;
                new Thread(() =>
                {
                    while (isRunning) {
                        var context = server.GetContext();
                        IHttpRequestHandler handler = null;
                        // get relative url cause handlers are sotred by relative keys
                        var url = GetRealtiveUrl(context.Request.Url.ToString());
                        
                        if (requestHandlers.TryGetValue(url, out handler))
                        {
                            handler.HandleRequest(context);
                        }
                        else
                        {
                            new RestPrefixNotRegistredHandler().HandleRequest(context);
                        }
                    }
                }).Start(); 
            }
            else {
                Console.WriteLine("Rest Api Server not initialized. Server not started.");
            }
        }

        public void Stop() {
            isRunning = false;
            server.Stop();
        }

        protected void addRequestHandler(string relPrefix, IHttpRequestHandler handler) {
            server.Prefixes.Add(createPrefix(relPrefix));
            // as handler key use relative path
            this.requestHandlers.Add(relPrefix, handler);
        }

        /// <summary>
        /// Extract relative server url without params
        /// </summary>
        public static string GetRealtiveUrl(string url) {
            var rgx = @"\w{4}://.*:\d*";
            return Regex.Replace(url, rgx, "").Split('?')[0]; 
        }

        protected string createPrefix(string pf) {
            return "http://*:" + port + pf;
        }
    }
}
