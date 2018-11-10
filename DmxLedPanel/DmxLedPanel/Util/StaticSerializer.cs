using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public static class StaticSerializer
    {
        public static T Deserialize<T>(string str)
        {
            T settings = JsonConvert.DeserializeObject<T>(str);
            return (T)Convert.ChangeType(settings, typeof(T));
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
