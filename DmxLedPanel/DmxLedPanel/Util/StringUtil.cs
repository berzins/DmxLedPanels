using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public static class StringUtil
    {
        public static bool IsEmpty(string str) {
            return str == null || str.Length == 0 ? true : false;
        }

        public static string RemoveLastChars(string str, int count) {
            return str.Substring(0, str.Length - count);
        }

        public static string MapToString<T>(Dictionary<T, T> map) {
            var str = map.Aggregate("", (s, e) => s + "'" + e.Key + "':" + "'" + e.Value + "', ");
            return str.Substring(0, str.Length - 2);
        }


    }
}
