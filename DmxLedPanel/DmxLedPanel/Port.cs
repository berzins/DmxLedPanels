using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Port : object
    {
        protected int net;

        public Port()
        {
            Net = 0;
            SubNet = 0;
            Universe = 0;
        }

        public Port(int net, int subNet, int universe) {
            Net = net;
            SubNet = subNet;
            Universe = universe;           
        }

        public int Net { get; set; }
        public int SubNet { get; set; }
        public int Universe { get; set; }

        public override bool Equals(Object o)
        {
            if (o.GetType() != typeof(Port)) return false;
            Port p = (Port)o;
            if (Net != p.Net) return false;
            if (SubNet != p.SubNet) return false;
            if (Universe != p.Universe) return false;


            return true;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

