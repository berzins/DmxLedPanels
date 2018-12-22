using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Talker
{
    public class Talker
    {
        public static void Log(ActionMessage msg) {
            printLevel(msg.Level);
            printCaller(msg.Source);
            printMessage(msg.Message);
        }
        

        private static void printMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(msg);
        }

        private static void printCaller(string from) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(from + " => ");
        }

        private static void printLevel(int lvl) {
            switch (lvl)
            {
                case LogLevel.ERROR:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    }
                case LogLevel.WARNING:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    }
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    }
            }
            Console.Write(GetLevelAsText(lvl) + ": ");
        }

        private static string INFO_STR = "INFO";
        private static string WARNING_STR = "WARINIG";
        private static string ERROR_STR = "ERROR";

        public static string GetSource()
        {
            return GetSource(1);
        }

        public static string GetSource(int frameIndex)
        {
            var mth = new StackTrace().GetFrame(frameIndex).GetMethod();
            return mth.ReflectedType.Name;
        }


        public static string GetLevelAsText(int level)
        {
            switch (level)
            {
                case LogLevel.INFO: {
                        return INFO_STR;
                    }
                case LogLevel.WARNING: {
                        return WARNING_STR;
                    }
                case LogLevel.ERROR: {
                        return ERROR_STR;
                    }
            }
            return "";
        }

    }

    public static class LogLevel {

        public const int INFO = 1;

        public const int WARNING = 0;

        public const int ERROR = -1;
    }
}
