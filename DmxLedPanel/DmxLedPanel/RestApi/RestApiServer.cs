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
    public class RestApiServer : ISystemInitializer
    {
        int port = 8746;
        private HttpListener server;
        private bool isRunning = false;
        Dictionary<string, IHttpRequestHandler> requestHandlers;

        public RestApiServer()
        {
            requestHandlers = new Dictionary<string, IHttpRequestHandler>(); 
            port = SettingManager.Instance.Settings.RestApiPort;
            initServer();
        }

        void initServer() {
            server = new HttpListener();
            server.IgnoreWriteExceptions = true;
            addRequestHandler("/", new RestHomeHandler());
            addRequestHandler("/getState/", new RestGetStateHandler());
            addRequestHandler("/createFixture/", new RestCreateFixtureHandler());
            addRequestHandler("/editFixture/", new RestEditFixtureHandler());
            addRequestHandler("/editFixtureName/", new RestEditFixtureNameHandler());
            addRequestHandler("/editFixtureAddress/", new RestEditFixtureAddressHandler());
            addRequestHandler("/editFixtureMode/", new RestEditFixtureModeHandler());
            addRequestHandler("/editFixturePixelPatch/", new RestEditFixturePixelPatchHandler());
            addRequestHandler("/deleteFixture/", new RestDeleteFixtureHandler());
            addRequestHandler("/createOutput/", new RestCreateOutputHandler());
            addRequestHandler("/editOutput/", new RestEditOutputHandler());
            addRequestHandler("/editOutputName/", new RestEditOutputNameHandler());
            addRequestHandler("/editOutputPort/", new RestEditOutputPortHandler());
            addRequestHandler("/editOuotputIP/", new RestEditOutputIPHandler());
            addRequestHandler("/deleteOutput/", new RestDeleteOutputHandler());
            addRequestHandler("/moveFixtureToOutput/", new RestMoveFixtureToOutputHandler());
            addRequestHandler("/moveFixtureToFixturePool/", new RestMoveFixtureToFixturePoolHandler());
            addRequestHandler("/saveState/", new RestSaveStateHandler());
            addRequestHandler("/loadState/", new RestLoadStateHandler());
            addRequestHandler("/getSavedStates/", new RestGetSavedStates());
            addRequestHandler("/enableHighlight/", new RestEnableHighlightHandler());
            addRequestHandler("/highlight/", new RestHighlightHandler());
            addRequestHandler("/getHighlightState/", new GetHighlightStateHandler());
            addRequestHandler("/undoState/", new RestUndoStateHandler());
            addRequestHandler("/redoState/", new RestRedoStateHandler());
            addRequestHandler("/dmxSignal/", new RestDmxSignalHandler());
            addRequestHandler("/currentProject/", new RestGetCurrentProject());
            addRequestHandler("/session/", new RestSessionHandler());
            addRequestHandler("/getTemplates/", new RestGetFixtureTemplatesHandler());
            addRequestHandler("/storeFixtureTemplate/", new RestStoreFixtureTemplateHandler());
        }

        public void InitSystem() {
            Cmd.Execute("netsh http add urlacl url=http://*:" + port + "/ user=Everyone");
        }
        
        public void Start() {
            if (server != null)
            {
                try
                {
                    server.Start();
                    ArtNet.Logger.Log("Rest API server started on port " + port, ArtNet.LogLevel.INFO);
                }
                catch (Exception e) {
                    ArtNet.Logger.Log("API server failed to start on port " + port + ". " + e.Message, ArtNet.LogLevel.ERROR);
                    return;
                }

                isRunning = true;
                new Thread(() =>
                {
                    while (isRunning) {
                        var context = server.GetContext();
                        IHttpRequestHandler handler = null;

                        ArtNet.Logger.Log("Request => '" + context.Request.Url, ArtNet.LogLevel.INFO);

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
                ArtNet.Logger.Log("Rest Api Server not initialized. Server not started.", ArtNet.LogLevel.WARNING);
            }
        }

        public void Stop() {
            isRunning = false;
            server.Stop();
        }

        public void Dispose() {
            server.Close();
            server = null;
            requestHandlers.Clear();
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
