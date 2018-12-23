using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Address
    {

        public Address() {

            Port = new Port();
            DmxAddress = 1;

        }

        public Port Port {get; set;} 
        public int DmxAddress { get; set; }

        public Address Clone() {
            return new Address()
            {
                Port = this.Port.Clone(),
                DmxAddress = this.DmxAddress
            };
        }

        public static Address operator +(Address a, int lenght) {
            if ((a.DmxAddress + lenght) > 512) {
                a.Port++;
                a.DmxAddress = 1;
                return a.Clone();
            }
            a.DmxAddress += lenght;
            return a.Clone();
        }

        public override string ToString()
        {
            return Port.ToString()
                + ", address: " + DmxAddress;
        }
    }
}
