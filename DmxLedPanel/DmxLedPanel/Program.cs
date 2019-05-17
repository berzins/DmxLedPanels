using DmxLedPanel.Modes;
using DmxLedPanel.RestApi;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using DmxLedPanel.State;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DmxLedPanel.ArtNetIO;
using Haukcode.ArtNet.Sockets;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Talker;

namespace DmxLedPanel
{
    class Program
    {
        #region Trap application termination
        [DllImport("Kernel32")]

        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        // handle close events
        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Shutting down");

            // do some work on close
            string closeHash = SettingsHash.GetHash();
            SettingManager.Instance.Settings.CloseHash = closeHash;
            SettingManager.Instance.Save();
            if (!Cmd.RestartAdmin)
            {
                Thread.Sleep(200);
                Console.WriteLine("Cleanup complete");
                Thread.Sleep(150);
            }
            //shutdown right away
            Environment.Exit(-1);
            return true;
        }
        #endregion

        static void Main(string[] args)
        {
            Talker.Talk.LogToFile = false;
            PrintAvailableInterfaces();
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            var program = new Program();
            program.Run(args);

        }

        private void Run(string[] args)
        {
            try
            {
                RestApiServer restApi = new RestApiServer();

                // register objects who would want to init system 
                InitSystemManager.Instance.AddInitializer(restApi);


                if (shouldInit(args))
                {
                    // init whatever necessary
                    InitSystemManager.Instance.InitSystem();
                }
                else
                {
                    // check if we do not have to reinit system (reboot necessary)
                    string openHash = SettingsHash.GetHash();
                    string closeHash = SettingManager.Instance.Settings.CloseHash;
                    if (!openHash.Equals(closeHash))
                    {
                        Cmd.RestartProcess(true);
                    }
                }

                restApi.Start();

                StateManager.Instance.LoadStateFromFile(SettingManager.Instance.Settings.CurrentProject);
                ArtnetIn.Instance.Start();
                new AutoSave().RunIfEnabled();
            }
            catch (Exception e) {
                HandleFatalException(e);
            }

        }

        public static void HandleFatalException(Exception e) {
            Talk.Fatal("No joke. The app crashed successfully!");
            Talk.Fatal("If you want to know why -> pelase press 'y' -> othervise press anything you like.");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Y:
                    {
                        Talk.Fatal("Error message: " + e.Message);
                        Talk.Fatal("Stacktrace:" + e.StackTrace);
                        break;
                    }
            }
            Talk.Info("If you think this is no big deal -> please press 'y' to continue.. or any other key to restart the app.");
            if (Console.ReadKey().Key == ConsoleKey.Y) {
                return;
            }
            Cmd.RestartProcess(false);
        }

        private bool shouldInit(string[] args)
        {
            foreach (var a in args)
            {
                if (a.Equals("init"))
                {
                    return true;
                }
            }
            return false;
        }

        private static void PrintAvailableInterfaces()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    Talk.Info("Network interface found: \n{0}", new NetworkInterfaceData(ni).ToString());
                }
            }
        }

        private class NetworkInterfaceData {

            public NetworkInterfaceData(NetworkInterface ni) {
                Ni = ni;
                IPs = new List<UnicastIPAddressInformation>();
                OperationalStatus = Ni.OperationalStatus;
                foreach (UnicastIPAddressInformation ip in Ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IPs.Add(ip);
                    }
                }
            }
            
            NetworkInterface Ni { get; }
            List<UnicastIPAddressInformation> IPs { get; }
            OperationalStatus OperationalStatus { get; }

            public override string ToString()
            {
                return String.Format(
                    "Name: '{0}'\n" +
                    "Status: '{1}'\n" +
                    "IPs/Subnets: {2}", 
                    Ni.Name, OperationalStatus, getIpSubnetString(IPs));
            }

            private string getIpSubnetString(List<UnicastIPAddressInformation> ips) {
                StringBuilder sb = new StringBuilder();
                foreach (UnicastIPAddressInformation ip in ips) {
                    sb.Append(String.Format("\n\t{0} / {1}", ip.Address, ip.IPv4Mask));
                }
                return sb.ToString();
            }


        }
    }
}
