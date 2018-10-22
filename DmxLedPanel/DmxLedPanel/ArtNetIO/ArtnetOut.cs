using ArtNet;
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
        private ArtNetWritter artWriter;

        //for polling device in network
        private ArtNetReader reader;
        private ArtDispatcher dispatcher;

        private ArtDispatcher InitDispatcher()
        {
            ArtDispatcher d = new ArtDispatcher();
            d.AddArtPollReplyListener(new WhosInNetwork());
            return d;
        }


        private ArtnetOut() {
            artWriter = new ArtNetWritter(
                System.Net.IPAddress.Parse(
                    Util.SettingManager.Instance.Settings.ArtNetBroadcastIp
                    )
                );
            reader = new ArtNetReader(InitDispatcher(),
                System.Net.IPAddress.Parse(Util.SettingManager.Instance.Settings.ArtNetPollReplyBindIp));
            reader.Start();
        }

        public static void Init() {
            if (instance == null) {
                instance = ArtnetOut.Instance;
            }
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
