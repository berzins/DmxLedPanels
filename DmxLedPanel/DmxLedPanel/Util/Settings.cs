using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public class Settings : ISerializable
    {

        private static readonly string REALTIVE_SETTINGS_PATH = "settings.json";

        private static Settings instance;

        private Settings() {
            
        }

        public static Settings Instance {
            get {
                if (instance == null)
                {
                    try
                    {
                        instance = new Settings().Deserialize<Settings>(
                        FileIO.ReadFile(REALTIVE_SETTINGS_PATH, true));
                    }
                    catch (Exception e) {
                        instance = new Settings();
                    }
                }
                return instance;
            }
        }

        public int RestApiPort { get; } = 8746;

        public T Deserialize<T>(string str)
        {
            Settings settings = JsonConvert.DeserializeObject<Settings>(str);
            return (T) Convert.ChangeType(settings, typeof(Settings));
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Save() {
            FileIO.WriteFile(REALTIVE_SETTINGS_PATH, true, Serialize());
        }
    }
}
