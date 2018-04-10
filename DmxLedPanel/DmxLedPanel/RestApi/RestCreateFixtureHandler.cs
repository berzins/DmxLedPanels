using DmxLedPanel.Modes;
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class RestCreateFixtureHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIX_NAME = "name";
        public static readonly string KEY_COUNT = "count";
        public static readonly string KEY_PORT = "port";
        public static readonly string KEY_ADDRESS = "address";
        public static readonly string KEY_PATCH_TYPE = "patch_type";
        public static readonly string KEY_MODE = "mode";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;
            List<Fixture> fixtures = new List<Fixture>();
            try
            {

                string name = q.Get(KEY_FIX_NAME);

                int dmxAddress = int.Parse(q.Get(KEY_ADDRESS));


                int[] portVals = getIntArgArray(q.Get(KEY_PORT));
                Address address = new Address()
                {
                    DmxAddress = int.Parse(q.Get(KEY_ADDRESS)),
                    Port = new Port()
                    {
                        Net = portVals[0],
                        SubNet = portVals[1],
                        Universe = portVals[2]
                    }
                };

                IMode mode = getFixtureMode(q.Get(KEY_MODE));
                IPixelPatch patch = getPixelPatch(q.Get(KEY_PATCH_TYPE));
                
                for (int i = 0; i < int.Parse(q.Get(KEY_COUNT)); i++)
                {
                    fixtures.Add(new Fixture(mode, patch)
                    {
                        Name = name + " " + i,
                        Address = address.Clone()

                    });
                }

                StateManager.Instance.State.FixturePool.AddRange(fixtures);
                string state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (Exception e) {
                WriteErrorMessage(context, e);
            }
        }


        /// <summary>
        /// Figure out 'KEY_PATCH_TYPE' parmeter
        /// Expected syntax is [patch type name] VALUE_SPILTTER [columns] PARAM_SPILTTER [rows]
        /// </summary>
        private IPixelPatch getPixelPatch(string param) {
            string[] args = param.Split(VALUE_SPLITTER);
            if (args[0].Equals(PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_LEFT)) {
                int[] dim = args[1].Split(PARAM_SPLITTER).Select(x => int.Parse(x)).ToArray();
                return new PixelPatchSnakeColumnWiseTopLeft(dim[0], dim[1], 0, Const.PIXEL_LENGTH);
            }
            return null;
        }


        /// <summary>
        /// Figure out 'KEY_MODE' parmeter
        /// Expected syntax is [mode name] VALUE_SPILTTER [columns] PARAM_SPILTTER [rows]
        /// </summary>
        private IMode getFixtureMode(string param) {
            string[] args = param.Split(VALUE_SPLITTER);
            if (args[0].Equals(Mode.MODE_GRID_TOP_LEFT)) {
                int[] dim = args[1].Split(PARAM_SPLITTER).Select(x => int.Parse(x)).ToArray();
                return new ModeGridTopLeft(dim[0], dim[1]);
            }
            return null;
        }
    }
}
