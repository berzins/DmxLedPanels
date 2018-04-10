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
    }
}
