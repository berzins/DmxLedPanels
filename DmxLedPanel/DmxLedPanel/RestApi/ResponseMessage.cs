using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.RestApi
{
    public class ResponseMessage
    {
        public static readonly string TYPE_INFO = "info";
        public static readonly string TYPE_ERROR = "error";
        public static readonly string TYPE_SAVED_STATES = "saved_states";
        public static readonly string TYPE_DMX_IN_STATE = "dmx_in_state";
        public static readonly string TYPE_CURRENT_PROJECT = "current_project";
        public static readonly string TYPE_HWOS_IN_NETWORK = "whos_in_network";
        public static readonly string TYPE_SESSION = "session";
        internal static readonly string TYPE_FIXTURE_TEMPLATE = "fixture_template";

        public ResponseMessage(string type, object content) {
            Type = type;
            Content = content;
        }

        public string Type { get; set; }
        public object Content { get; set; }
    }
}
