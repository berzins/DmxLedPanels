using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Fixtures
{
    class FixtureAddressSetter
    {
        public static void SetDmxAddressFor(List<Fixture> fixtures, Address startAddress, bool increment) {
            foreach (Fixture fix in fixtures) {
                if (increment)
                {
                    if (startAddress.DmxAddress + fix.InputAddressCount > 512)
                    {
                        startAddress.DmxAddress = 1;
                        startAddress.Port++;
                    }
                    fix.Address = startAddress.Clone();
                    //increment address
                    startAddress.DmxAddress += fix.InputAddressCount;
                }
                else
                {
                    fix.Address = startAddress.Clone();
                }
            }
        }
    }
}
