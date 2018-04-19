using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public class SettingsHash
    {

        public static string GetHash() {
            return SettingManager.Instance.Settings.RestApiPort.GetHashCode().ToString();
        }
    }
}
