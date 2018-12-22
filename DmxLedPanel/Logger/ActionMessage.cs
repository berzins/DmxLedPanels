using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.State
{
    public class ActionMessage
    {
        public static readonly string DEFAULT_MESSAGE = "";
        public static readonly string DEFAULT_SOURCE = "not defined";
        public static readonly string DEFAULT_LEVEL = ArtNet.Logger.GetLevelAsText(ArtNet.LogLevel.INFO);



        public string Message { get; set; } = DEFAULT_MESSAGE;
        public string Source { get; set; } = DEFAULT_SOURCE;
        public string Level { get; set; } = DEFAULT_LEVEL;

    }
}
