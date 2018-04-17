using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Pixel
    {

        public Pixel() { }

        // copy constructor
        protected Pixel(int index, int[] dmxVals, int[] dmxAddr, int addrCount) {
            this.AddressCount = addrCount;
            this.Index = index;
            for (int i = 0; i < dmxVals.Length; i++) {
                DmxValues[i] = dmxVals[i];
            }
            for (int i = 0; i < dmxAddr.Length; i++) {
                DmxAddresses[i] = dmxAddr[i];
            }
        }

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

        public Pixel Clone() {
            return new Pixel(Index, DmxValues, DmxAddresses, AddressCount);
        }
    }
}
