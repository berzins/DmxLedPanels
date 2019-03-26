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

        ////for polling device in network
        //private ArtNetReader reader6454;
        private ArtDispatcher dispatcher;

        private ArtDispatcher InitDispatcher()
        {
            ArtDispatcher d = new ArtDispatcher();
            ////WhoTheFuckAreYou = new WhosInNetwork();
            ////d.AddArtPollReplyListener(WhoTheFuckAreYou);
            return d;
        }
        
        private ArtnetOut() {
            artWriter = new ArtNetWritter(
                System.Net.IPAddress.Parse(
                    Util.SettingManager.Instance.Settings.ArtNetBroadcastIp
                    )
                );
            dispatcher = InitDispatcher();
            //reader6454 = new ArtNetReader(dispatcher,
            //    System.Net.IPAddress.Parse(Util.SettingManager.Instance.Settings.ArtNetPollReplyBindIp));
            //reader6454.Start();
        }

        //public WhosInNetwork WhoTheFuckAreYou { get; private set; }


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
