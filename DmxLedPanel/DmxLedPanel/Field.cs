using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Field
    {

        public Field() {
            this.Pixels = new List<Pixel>();
            AddressCount = 3;
        }

        public int Index { get; set; }
        public List<Pixel> Pixels {
            get; set;
        }
        public int AddressCount { get; set; }


        public void SetDmxValues(int [] dmxValues) {
            Utils.checkDmxValueRange(dmxValues);
            foreach (Pixel p in Pixels) {
                p.DmxValues = Utils.GetSubArray(dmxValues, 0, dmxValues.Length);
            }
        }
    }
}
