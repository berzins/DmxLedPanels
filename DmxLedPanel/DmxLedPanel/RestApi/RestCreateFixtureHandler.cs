using DmxLedPanel.Fixtures;
using DmxLedPanel.Modes;
using DmxLedPanel.PixelPatching;
using DmxLedPanel.State;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Talker;

namespace DmxLedPanel.RestApi
{
    public class RestCreateFixtureHandler : HttpRequestHandler
    {

        public static readonly string KEY_FIX_NAME = "name";
        public static readonly string KEY_COUNT = "count";
        public static readonly string KEY_PORT = "port";
        public static readonly string KEY_ADDRESS = "address";
        public static readonly string KEY_PATCH_TYPE = "patch_type";
        public static readonly string KEY_MODES = "modes";
        public static readonly string KEY_CURRENT_MODE = "current_mode";
        public static readonly string KEY_UTILS_ENABLED = "utils_enabled";
        public static readonly string KEY_UTIL_ADDRESS = "utils_address";
        public static readonly string KEY_INCREMENT = "increment";

        public override void HandleRequest(HttpListenerContext context)
        {
            var q = context.Request.QueryString;
            List<Fixture> fixtures = new List<Fixture>();
            try
            {
                string name = q.Get(KEY_FIX_NAME);

                int dmxAddress = int.Parse(q.Get(KEY_ADDRESS));
                bool dmxUtilsEnabled = bool.Parse(q.Get(KEY_UTILS_ENABLED));
                
                var port = getPortFromArgs(q.Get(KEY_PORT));

                Address address = new Address()
                {
                    DmxAddress = int.Parse(q.Get(KEY_ADDRESS)),
                    Port = port.Clone()
                };

                var utilsEnabled = bool.Parse(q.Get(KEY_UTILS_ENABLED));
                var utilAddress = new Address() {
                    DmxAddress = int.Parse(q.Get(KEY_UTIL_ADDRESS)),
                    Port = port.Clone()
                };

                bool increment = bool.Parse(q.Get(KEY_INCREMENT));

                //validate pixel and mode values
                var modes = GetFixtureModes(q.Get(KEY_MODES));
                var pp = q.Get(KEY_PATCH_TYPE);
                ValidateModeValues(modes, getPixelPatch(pp));

                // create fixtures
                for (int i = 0; i < int.Parse(q.Get(KEY_COUNT)); i++)
                {
                    var fix = new Fixture(modes, getPixelPatch(pp));
                    fix.Name = name + " " + i;  
                    fix.UtilAddress = utilAddress.Clone();
                    fix.IsDmxUtilsEnabled = dmxUtilsEnabled;
                    fixtures.Add(fix);
                }
                FixtureAddressSetter.SetDmxAddressFor(fixtures, address, increment);

                var dmxUtils = fixtures[0].DmxUtils;
                StateManager.Instance.State.FixturePool.AddRange(fixtures);

                SetMessage(
                    "Fixtures created: " + Fixture.GetFixtureInfoStr(fixtures) + ".",
                    LogLevel.INFO,
                    IS_PART_OF_STATE,
                    Talker.Talk.GetSource()
                    );

                string state = StateManager.Instance.GetStateSerialized();
                WriteResponse(context, RestConst.RESPONSE_OK, RestConst.CONTENT_TEXT_JSON, state);
            }
            catch (ArgumentException e) {
                SetErrorMessage(e.ToString(), IS_NOT_PART_OF_STATE, Talker.Talk.GetSource());
                WriteErrorMessage(context, e);
            }
        }

        public static Port getPortFromArgs(string portStr) {
            int[] portVals = getIntArgArray(portStr);
            return new Port()
            {
                Net = portVals[0],
                SubNet = portVals[1],
                Universe = portVals[2]
            };
        }


        /// <summary>
        /// Figure out 'KEY_PATCH_TYPE' parmeter
        /// Expected syntax is [patch type name] VALUE_SPILTTER [columns] PARAM_SPILTTER [rows]
        /// </summary>
        public static IPixelPatch getPixelPatch(string param) {
            string[] args = param.Split(VALUE_SPLITTER);
            int[] dim = args[1].Split(PARAM_SPLITTER).Select(x => int.Parse(x)).ToArray();
            return RectaglePixelPatch.InstantiatePixelPatchByName(
                args[0], // name
                dim[0], //columns
                dim[1], //rows
                0,
                Const.PIXEL_LENGTH
                );
        }


        public static List<IMode> GetFixtureModes(string param) {
            Utils.ThrowArgumetExceptionIfEmpty(param, "Modes not set. Please set the modes and try again.");
            List<IMode> modes = new List<IMode>();
            string[] args = param.Split(ITEM_SPLITTER);

            foreach (var m in args) {
                modes.Add(GetFixtureMode(m));
            }

            return modes;
        }

        /// <summary>
        /// Figure out 'KEY_MODE' parmeter
        /// Expected syntax is [mode name] VALUE_SPILTTER [columns] PARAM_SPILTTER [rows]
        /// </summary>
        public static IMode GetFixtureMode(string param) {
            Utils.ThrowArgumetExceptionIfEmpty(param, "The mode parameter is null or empty.. hint: check suspicious values of modes. ");
            string[] args = param.Split(VALUE_SPLITTER);
            return Mode.InstantiateModeByName(
                args[0],
                args[1].Split(PARAM_SPLITTER).Select(x => int.Parse(x)).ToArray()
                );
        }

        private List<Address> createAddresses(Address addr, bool increment, int spacing, int count) {
            var addresses = new List<Address>();
            for (int i = 0; i < count; i++)
            {
                var a = addr.Clone();
                addresses.Add(a);
                if(increment)
                {
                    addr.DmxAddress += spacing;
                }
            }
            return addresses;
        }

        public static void ValidateModeValues(List<IMode> modes, IPixelPatch pp) {

            bool colsValid = true;
            bool rowsValid = true;

            foreach (var m in modes)
            {
                if (pp.Columns < m.Params[0]) colsValid = false;
                if (pp.Rows < m.Params[1]) rowsValid = false;
            }

            if (!colsValid && !rowsValid) throw new ArgumentException("Row and column values not valid");
            if (!colsValid) throw new ArgumentException("Column values not valid");
            if (!rowsValid) throw new ArgumentException("Row values not valid");

        }
    }

}
