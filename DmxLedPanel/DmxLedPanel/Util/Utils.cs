using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public static class Utils
    {
        public static readonly int DMX_VALUE_MAX = 255;
        public static readonly int DMX_VALUE_MIN = 0;

        public static void checkDmxValueRange(int value) {
            if (value > DMX_VALUE_MAX || value < DMX_VALUE_MIN) {
                throw new ArgumentOutOfRangeException(value + " is out of dmx value range (" + DMX_VALUE_MIN + " to " + DMX_VALUE_MAX + ")");
            }
        }

        public static void checkDmxValueRange(int[] values) {
            foreach (int val in values) {
                checkDmxValueRange(val);
            }
        }

        public static T[] GetSubArray<T>(T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    
    }
}
