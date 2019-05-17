using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Talker;

namespace DmxLedPanel
{
    
    public static class FileIO
    {
        public static string ReadFile(string path, bool relative) {
            return relative ? File.ReadAllText(GetRelativePath() + path) : File.ReadAllText(@path);
        }

        public static void WriteFile(string path, bool relative, string data) {
            var dir = Path.GetDirectoryName(GetRelativePath() + path);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            if (relative) {
                path = GetRelativePath() + path;
            }
            try
            {
                File.WriteAllText(path, data);
                Talk.Info(path + " written successfully.");
            }
            catch (Exception e) {
                Talk.Error(path + "write error => " + e.ToString());
            }
        }

        public static string GetRelativePath() {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\";
        }

        public static string[] GetFiles(string path, bool relative, bool fullPath, string filter) {
            return GetFiles(path, relative, fullPath, filter, true);
        }

        public static string[] GetFiles(string path, bool relative, bool fullPath, string filter, bool createDir) {
            var p = relative ? GetRelativePath() + path : path;

            if (!Directory.Exists(p) && createDir) {
                CreateDir(p);
                Talk.Warning("Direcotry '" + p + "' didn't existed -> created successfully.");
            }

            var files = Directory.GetFiles(p, "*.json");

            if (!fullPath)
            {
                files = files.Select(Path.GetFileName).ToArray();
            }
            return files;
        }

        public static void CreateDir(string dir) {
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
        }
        
    }
}
