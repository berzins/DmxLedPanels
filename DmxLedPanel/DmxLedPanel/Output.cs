using ArtNet.ArtPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    class Output : IFixtureUpdateHandler
    {

        private List<Fixture> fixtures, updatePending;
        Dictionary<int, Fixture> fixturePatch;
        int curretnFixtureIndex = 0;

        
        public Output() {
            fixtures = new List<Fixture>();
            updatePending = new List<Fixture>();
            fixturePatch = new Dictionary<int, Fixture>();
        }

        public Port Port1 { get; set; }

        public Port Port2 { get; set; }

        public void PatchFixture(Fixture f) {
            if (!fixtures.Contains(f)) {
                fixtures.Add(f);
                f.AddUpdateHandler(this);
                ResetUpdatePending();
            }
        }

        public void OnUpdate(Fixture f) {
            updatePending.Remove(f);
            if (updatePending.Count == 0) {
                ResetUpdatePending();
                WriteOutput();
            }
            
        }

        private void WriteOutput() {

            int[] dmxValues = new int[1020];
            int copyIndex = 0;

            foreach (Fixture f in fixtures) {
                Array.Copy(f.DmxValues, 0, dmxValues, copyIndex, f.DmxValues.Length);
                copyIndex += f.DmxValues.Length;
            }

            var writer = ArtnetOut.Instance.Writer;

            List<ArtDmxPacket> packets = new List<ArtDmxPacket>();
            packets.Add(new ArtDmxPacket() { PhysicalPort = Port1.Net, SubNet = Port1.SubNet, Universe = Port1.Universe });
            packets.Add(new ArtDmxPacket() { PhysicalPort = Port2.Net, SubNet = Port2.SubNet, Universe = Port2.Universe });

            copyIndex = 0;
            foreach (ArtDmxPacket pack in packets) {
                pack.DmxData = Utils.GetSubArray(dmxValues, copyIndex, 510).Select(x => (byte)x).ToArray();
                copyIndex += 510;
                writer.Write(pack);
            }
        }

        private void ResetUpdatePending() {
            updatePending = new List<Fixture>();
            foreach (Fixture f in fixtures) {
                updatePending.Add(f);
            }
        }
    }
}
