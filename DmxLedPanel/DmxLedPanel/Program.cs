using ArtNet;
using ArtNet.ArtPacket;
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
            if (!Cmd.RestartAdmin) {
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
            ArtNetReader.GetAvailableBindAddresses();
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            var program = new Program();
            program.Run(args);
         
        }

        private void Run(string[] args) {

            RestApiServer restApi = new RestApiServer();

            // register objects who would want to init system 
            InitSystemManager.Instance.AddInitializer(restApi);


            if (shouldInit(args))
            {
                // init whatever necessary
                InitSystemManager.Instance.InitSystem();
            }
            else {
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
            ArtnetOut.Init();

            var whosInNetwork = new WhosInNetwork();


                //new Thread(() => {
                //    while (true) {
                //        ArtnetIn.ArtDmxListener.recieved = true;
                //        Thread.Sleep(1000);
                //    }
                //}).Start();
                
            

        }

        private bool shouldInit(string[] args) {
            foreach (var a in args)
            {
                if (a.Equals("init"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
