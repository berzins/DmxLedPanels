using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Talker
{
    public class Talker
    {
        private static readonly string RELATIVE_LOG_PATH = "log\\";
        private static string currentFileName = string.Empty;

        private static object syncLock = new object();
        private static volatile Queue<ActionMessage> messages = new Queue<ActionMessage>();

        public static bool LogToFile { get; set; }

        public static void Log(ActionMessage msg) {
            new Thread(() =>
            {
                lock (syncLock)
                {
                    printLevel(msg.Level);
                    PrintCurrentTime();
                    PrintCaller(msg);
                    PrintMessage(msg.Message);
                    if (LogToFile)
                    {
                        if (currentFileName.Equals(string.Empty))
                        {
                            currentFileName = RELATIVE_LOG_PATH + GetCurrentDateTimeFormated() + ".log";
                        }

                        WriteFile(currentFileName, true, GetLogString(msg) + Environment.NewLine);
                    }
                }
            }).Start();
        }

        public static string GetLogString(ActionMessage msg) {
            return GetLevelAsText(msg.Level) + " "
                + GetCurrentTimeFormated() + " "
                + GetSourceString(msg) + " "
                + msg.Message;
        }

        private static void PrintMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(msg);
        }

        private static void PrintCaller(ActionMessage msg) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(GetSourceString(msg));
        }

        private static string GetSourceString(ActionMessage msg) {
            return msg.Source + " => ";
        }

        private static void PrintCurrentTime() {
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(GetCurrentTimeFormated() + " ");
        }

        private static string GetCurrentTimeFormated() {
            return GetTimeFormated(DateTime.Now, "H:mm:ss:fff");
        }

        private static string GetCurrentDateTimeFormated() {
            return GetTimeFormated(DateTime.Now, "yy_MM_dd_H_mm_ss_fff");
        }

        private static string GetTimeFormated(DateTime time, string format) {
            return time.ToString(format);
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

        private static void WriteFile(string path, bool relative, string data)
        {
            var dir = Path.GetDirectoryName(GetRelativePath() + path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (relative)
            {
                path = GetRelativePath() + path;
            }
            try
            {
                string content = String.Empty;
                if (File.Exists(path))
                {
                    content = File.ReadAllText(path);
                }

                File.WriteAllText(path, content + data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Talker while writing the log file: " + e.ToString());
            }
        }

        public static string GetRelativePath()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\";
        }

        private static readonly string INFO_STR = "INFO";
        private static readonly string WARNING_STR = "WARINIG";
        private static readonly string ERROR_STR = "ERROR";

        public static string GetSource()
        {
            return GetSource(2);
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

    internal interface ILogPrinter {
        void OnLogRecieved(ref List<ActionMessage> logs);
    }
}
