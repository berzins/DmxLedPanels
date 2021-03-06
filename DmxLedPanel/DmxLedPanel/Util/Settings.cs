﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public class SettingManager
    {
        private static readonly string REALTIVE_SETTINGS_PATH = "settings.json";
        public static readonly int DEFAULT_UI_REST_PORT = 8746;
        public static readonly string DEFAULT_UI_INDEX_PATH = "ui/index.html";

        private static SettingManager instance;

        private Settings settings;

        private SettingManager() {
            settings = LoadSettings();
        }

        public static SettingManager Instance {
            get {
                if (instance == null)
                {
                    instance = new SettingManager();
                }
                return instance;
            }
        }
        
        public Settings Settings { get { return settings; } }
        

        public void Save() {
            FileIO.WriteFile(REALTIVE_SETTINGS_PATH, true, StaticSerializer.Serialize(settings));
        }

        public void Reload() {
            settings = LoadSettings();
        }

        private Settings LoadSettings() {
            try
            {
                var json = FileIO.ReadFile(REALTIVE_SETTINGS_PATH, true)
                    .Replace("/", "\\");
                    //.Replace("\r\n", "");
                    //.Replace("\\\"", "\""); 
                Settings set = StaticSerializer.Deserialize<Settings>(json);
                return set; 
            }
            catch (Exception e)
            {
                Console.WriteLine("Loading " + REALTIVE_SETTINGS_PATH + " failed! Default settings has been loaded. >> " + e.StackTrace);
                settings = new Settings()
                {
                    RestApiPort = DEFAULT_UI_REST_PORT,
                    UIHomePath = DEFAULT_UI_INDEX_PATH.Replace("/", "\\"),
                    UIRelJavascriptPath = "ui/resource/app".Replace("/", "\\"),
                    DefaultOutputIp = "0.0.0.0",
                    ArtNetBindIp = "0.0.0.0",
                    ArtNetBroadcastIp = "255.255.255.255",
                    ArtNetPollReplyBindIp = "0.0.0.0",
                    CurrentProject = "default",
                    CloseHash = "-2342-",
                    Password = "p",
                    AutoSave = 0

                };
                Save();
                return settings;
            }
        }
    }

    public class Settings {

        public int RestApiPort { get; set; }

        public bool AddFireWallException { get; set; }

        public string UIHomePath { get; set; }

        public string UIRelJavascriptPath { get; set; }

        public string DefaultOutputIp { get; set; }

        public string ArtNetBindIp { get; set; }

        public string ArtNetBroadcastIp { get; set; }

        public string CurrentProject { get; set; }

        public string CloseHash { get; set; }

        public string ArtNetPollReplyBindIp { get; set; }

        public string Password { get; set; }

        public int AutoSave { get; set; }
        
    }
}
