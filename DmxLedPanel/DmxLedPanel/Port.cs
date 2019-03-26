using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;

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
            hash = GetHashCode();
        }

        public int Net { get; set; }
        public int SubNet { get; set; }
        public int Universe { get; set; }
        private int hash;

        public override bool Equals(Object o)
        {
            if (o == null) {
                Talker.Talker.Log(new Talker.ActionMessage() {
                    Message = "On Port.Equals() the expected port object is NULL",
                    Level = Talker.LogLevel.WARNING,
                    Source = Talker.Talker.GetSource()
                });
                return false;
            }
            if (o.GetType() != typeof(Port)) return false;
            Port p = (Port)o;
            if (Net != p.Net) return false;
            if (SubNet != p.SubNet) return false;
            if (Universe != p.Universe) return false;


            return true;

        }

        public override int GetHashCode()
        {
            int hash = 0;
            hash += Net * 70000;
            hash += SubNet * 256;
            hash += Universe;
            return hash;
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

        public override string ToString()
        {
            return "net: " + Net
                + ", sub net: " + SubNet
                + ", universe: " + Universe;
        }

        public static List<Port> CopyList(List<Port> list) {
            return list.Aggregate(new List<Port>(), (l, p) => {
                l.Add(p.Clone());
                return l;
            });
        }

        public static HashSet<Port> CopyHashSet(HashSet<Port> set) {
            return set.Aggregate(new HashSet<Port>(), (s, p) =>
            {
                s.Add(p.Clone());
                return s;
            });
        }

        public static IEqualityComparer<Port> GetEqualityComparer() {
            return new EqualityComparer();
        }

        private class EqualityComparer : IEqualityComparer<Port>
        {
            public bool Equals(Port x, Port y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(Port p)
            {
                return p.GetHashCode();
            }
        }

        internal static Port From(ArtDmxPacket packet)
        {
            if (packet == null) {
                return null;
            }
            return new Port(packet.PhysicalPort, packet.SubnetUniverse.SubNet, packet.SubnetUniverse.Universe);
        }
    }
}

