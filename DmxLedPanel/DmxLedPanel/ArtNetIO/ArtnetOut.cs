using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.ArtNetIO
{
    public class ArtnetOut
    {
        private static ArtnetOut instance;
        private ArtNet.ArtNetWritter artWriter;

        private ArtnetOut() {
            artWriter = new ArtNet.ArtNetWritter(
                System.Net.IPAddress.Parse(
                    Util.SettingManager.Instance.Settings.ArtNetBroadcastIp
                    )
                );
        }

        public static ArtnetOut Instance {
            get {
                if (instance == null)
                {
                    instance = new ArtnetOut();
                }
                return instance;
            }
        }

        public ArtNet.ArtNetWritter Writer {
            get {
                return this.artWriter;
            }
        }
    }
}
