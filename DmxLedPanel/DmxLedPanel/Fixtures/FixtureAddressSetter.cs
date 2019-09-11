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

            Address nextAddress = startAddress.Clone();
            foreach (Fixture fix in fixtures) {
                if (increment)
                {
                    if (nextAddress.DmxAddress + (fix.MaxInputAddressCount - (fix.PortsRequired.Count - 1) * 512) > 512)
                    {
                        nextAddress.Port++;
                        nextAddress.DmxAddress = 1;
                    }
                    

                    fix.Address = nextAddress.Clone();
                    nextAddress = fix.GetAddressAfterThis().Clone();
                }
                else
                {
                    fix.Address = startAddress.Clone();
                }
            }
        }

        public static void SetUtilAddressFor(List<Fixture> fixtures, Address startAddress, bool increment)
        {
            // as you see this is duumy method.. just set same address for all
            foreach (Fixture fix in fixtures)
            {
                fix.UtilAddress = startAddress;
            }
        }
    }
}
