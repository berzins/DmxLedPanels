
using DmxLedPanel.Util;
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

        public static void checkDmxValueRange(byte value) {
            if (value > DMX_VALUE_MAX || value < DMX_VALUE_MIN) {
                throw new ArgumentOutOfRangeException(value + " is out of dmx value range (" + DMX_VALUE_MIN + " to " + DMX_VALUE_MAX + ")");
            }
        }

        public static void checkDmxValueRange(byte[] values) {
            foreach (byte val in values) {
                checkDmxValueRange(val);
            }
        }

        public static T[] GetSubArray<T>(T[] data, int index, int length) {
            return GetSubArray(data, index, length, length);
        }

        public static T[] GetSubArray<T>(T[] data, int index, int copyLength, int resultLength) {
            if (resultLength < copyLength) {
                throw new ArgumentOutOfRangeException("Reuslt array size value cant be less than coppy length.");
            }
            T[] result = new T[resultLength];
            Array.Copy(data, index, result, 0, copyLength);
            return result;
        }



        public static T [] cloneArray<T>(T[] source) {
            T [] copy = new T [source.Length];
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = source[i];
            }
            return copy;
        }

        public static T[,] cloneArray<T>(T[,] source) {
            T[,] copy = new T[source.GetLength(0),source.GetLength(1)];
            for (int i = 0; i < copy.GetLength(0); i++) {
                for (int e = 0; e < copy.GetLength(1); e++) {
                    copy[i, e] = source[i, e];
                }
            }
            return copy;
        }

        public static T[,] InitArray<T>(T[,] source, T value)
        {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int e = 0; e < source.GetLength(1); e++)
                {
                    source[i, e] = value;
                }
            }
            return source;
        }
        
        public static void ThrowArgumetExceptionIfEmpty(String arg, String errorMsg) {
            if (StringUtil.IsEmpty(arg)) {
                throw new ArgumentException(errorMsg);
            }
        }
    
    }
}
