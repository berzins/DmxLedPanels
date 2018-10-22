using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.ArtNetIO
{
    public class ArtNetDevice
    {
        public ArtNetDevice(string name, string ip, DateTime lastSeen)
        {
            Name = name;
            Ip = ip;
            LastSeen = lastSeen;
        }

        public string Name { get; set; }
        public string Ip { get; set; }
        public DateTime LastSeen { get; set; }

        public int HashCode {
            get {
                return getHashCode(Name, Ip);
            }
        }

        public static int getHashCode(string name, string ip) {
            return (name + ip).GetHashCode(); 
        }
    }
}
