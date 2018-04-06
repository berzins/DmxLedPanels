using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;

namespace DmxLedPanel
{
    public class Converter
    {
        private readonly List<Fixture> fixtures;
        private List<Port> inputPorts;

        public Converter() {
            inputPorts = new List<Port>();
        }

        public List<Fixture> Fixtures {
            get {
                return fixtures;
            }
        }
        
        public void AddFixture(Fixture f) {

            // Add port to inputPorts if it does not exists already

            if (!HasPort(f.Address.Port, inputPorts)) {
                inputPorts.Add(f.Address.Port);
            }
        
            //TODO: check if fixture does not overlap with other fixtures
            fixtures.Add(f);
        }

        private bool HasPort(Port port, List<Port> ports) {
            foreach (Port p in ports)
            {
                if (p.Equals(port)) return true;
            }
            return false;
        }
    }
}
