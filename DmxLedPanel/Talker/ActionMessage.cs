using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talker
{
    public class ActionMessage
    {
        public static readonly string DEFAULT_MESSAGE = "";
        public static readonly string DEFAULT_SOURCE = "not defined";
        public static readonly int DEFAULT_LEVEL = LogLevel.INFO;



        public string Message { get; set; } = DEFAULT_MESSAGE;
        public string Source { get; set; } = DEFAULT_SOURCE;
        public int Level { get; set; } = DEFAULT_LEVEL;

        public ActionMessage() { }

        protected ActionMessage(string msg, string srouce, int level) {
            Message = msg;
            Source = srouce;
            Level = level;
        }

        public ActionMessage Copy() {
            return new ActionMessage(Message, Source, Level);
        }

    }
}
