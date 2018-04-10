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

        public ResponseMessage(string type, string content) {
            Type = type;
            Content = content;
        }

        public string Type { get; set; }
        public string Content { get; set; }
    }
}
