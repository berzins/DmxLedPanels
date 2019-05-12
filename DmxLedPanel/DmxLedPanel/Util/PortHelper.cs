using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public static class PortHelper
    {

        static PortHelper()
        {
            AllPorts = new List<Port>();
            for (int subNet = 0; subNet < 16; subNet++)
            {
                for (int uni = 0; uni < 16; uni++)
                {
                    AllPorts.Add(new Port(0, subNet, uni));
                }
            }
        }

        public static List<Port> AllPorts { get; }
    }
}
