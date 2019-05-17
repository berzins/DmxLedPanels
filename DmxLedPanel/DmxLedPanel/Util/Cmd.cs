using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace DmxLedPanel.Util
{
    public static class Cmd
    {

        public static bool RestartAdmin { get; private set; } = false;

        /// <summary>
        ///  Executes provided command and returns output 
        /// </summary>
        public static string Execute(string command)
        {
            var cmd = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            cmd.StartInfo = startInfo;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            string ret = cmd.StandardOutput.ReadLine();
            Console.WriteLine(ret);
            return ret;
        }

        public static void RestartProcess(bool admin)
        {
            RestartAdmin = admin;
            var path = Process.GetCurrentProcess().MainModule.FileName;
            var app = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = path;
            startInfo.UseShellExecute = true;
            if (admin)
            {
                startInfo.Arguments = "init";
                startInfo.Verb = "runas";
            }
            app.StartInfo = startInfo;
            app.Start();
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}
