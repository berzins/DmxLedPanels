using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Field
    {
        private byte[] dmxValues;

        public Field() {
            Pixels = new List<Pixel>();
            PixelChannelCount = 3;
            dmxValues = new byte[PixelChannelCount];
        }

        public int Index { get; set; }
        public List<Pixel> Pixels { get; set; }
        public int PixelChannelCount { get; set; }


        public void SetDmxValues(byte [] dmxValues) {
            Utils.checkDmxValueRange(dmxValues);
            foreach (Pixel p in Pixels)
            {
                p.DmxValues = dmxValues;
            }
        }
    }
}
