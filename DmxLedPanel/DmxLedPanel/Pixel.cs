using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Pixel
    {
        
        private int addressCount = 0;

        public int Index { get; set; }

        public int[] DmxValues { get; set; }

        public int[] DmxAddresses { get; set; }

        public int AddressCount {
            get { return this.addressCount;  }
            set {
                this.DmxValues = new int[value];
                this.DmxAddresses = new int[value];
                this.addressCount = value;
            }
        }
    }
}
