using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    
    public static class FileIO
    {
        public static string ReadFile(string path, bool relative) {
            return relative ? File.ReadAllText(GetRelativePath() + path) : File.ReadAllText(@path);
        }

        public static void WriteFile(string path, bool relative, string data) {
            if (relative) {
                File.WriteAllText(GetRelativePath() + path, data);
            }
            else {
                File.WriteAllText(path, data);
            }
        }

        public static string GetRelativePath() {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\";
        }

        public static string[] GetFiles(string path, bool relative, bool fullPath, string filter) {
            var p = relative ? GetRelativePath() + path : path;
            var files = Directory.GetFiles(p, "*.json");
            if (!fullPath) {
                files = files.Select(Path.GetFileName).ToArray();
            }
            return files;
        }
        
    }
}
