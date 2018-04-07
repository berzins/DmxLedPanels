using ArtNet.ArtPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class Output : IFixtureUpdateHandler
    {

        private static int idCounter = 0;

        private List<Fixture> fixtures, updatePending;
        public Output() {
            fixtures = new List<Fixture>();
            updatePending = new List<Fixture>();
            Ports = new List<Port>(2);
            Name = "Output " + (idCounter++);
        }

        public string Name { get; set; }

        public List<Port> Ports { get; }
        
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
                WriteOutput();
                ResetUpdatePending();
            }
        }

        public List<Fixture> GetFixtures() {
            return this.fixtures;
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


            foreach (Port p in Ports) {
                packets.Add(new ArtDmxPacket() { PhysicalPort = p.Net, SubNet = p.SubNet, Universe = p.Universe });
            }
            
            // copy data to packets
            copyIndex = 0;
            foreach (ArtDmxPacket pack in packets) {
                pack.DmxData = Utils.GetSubArray(dmxValues, copyIndex, 510).Select(x => (byte)x).ToArray();
                copyIndex += 510;
                writer.Write(pack);
            }
            ArtNet.Utils.ArtPacketStopwatch.Stop();
            //Console.WriteLine("packet took " + ArtNet.Utils.ArtPacketStopwatch.ElapsedMilliseconds + " mils to process");
        }

        private void ResetUpdatePending() {
            updatePending = new List<Fixture>();
            foreach (Fixture f in fixtures) {
                updatePending.Add(f);
            }
        }
    }
}
