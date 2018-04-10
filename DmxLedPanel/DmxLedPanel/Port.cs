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

        public Port Clone() {
            return new Port()
            {
                Net = this.Net,
                SubNet = this.SubNet,
                Universe = this.Universe
            };
        }


        public static Port operator ++(Port p) {
            var next = p.GetNextPort();
            return new Port() { Net = next.Net, SubNet = next.SubNet, Universe = next.Universe };
        }

        public Port GetNextPort() {

           var p = this.Clone();
            // TODO: maybe rewrite this as a recursion...
            // or use bit banging. 
            if (p.Universe + 1 > 15)
            {
                p.Universe = 0;
                if (p.SubNet + 1 > 15)
                {
                    p.SubNet = 0;
                    if (p.Net + 1 > 15) {
                        return null;
                    }
                    p.Net += 1;
                }
                else {
                    p.SubNet += 1;
                }
            }
            else {
                p.Universe += 1;
            }
            return p;
        }
    }
}

