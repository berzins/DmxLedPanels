using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class ArtnetOut
    {
        private static ArtnetOut instance;
        private ArtNet.ArtNetWritter artWriter;

        private ArtnetOut() {
            artWriter = new ArtNet.ArtNetWritter();
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
