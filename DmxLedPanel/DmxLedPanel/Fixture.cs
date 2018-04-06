using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;

namespace DmxLedPanel
{
    public class Fixture : IDmxPacketHandler
    {
        private List<IFixtureUpdateHandler> updateHandlers;
        private int addressCount;
        private IPixelPatch pixelPatch;

        public Fixture(IMode m, IPixelPatch pixelPatch) {
            this.pixelPatch = pixelPatch;
            SetMode(m);
            Address = new Address();
            updateHandlers = new List<IFixtureUpdateHandler>();
        }

        // FOR TESTS

            public IPixelPatch PixelPatch { get { return pixelPatch; } }

       // END OF FOR TESTS

        public int ID {
            get; set;
        }

        public Address Address { get; set; }

        public int PixelAddressCount {
            get {
                return 3; // TODO: this is really dumb .. rewrite this for another pixel width support
            }
        }
         
        public List<Field> Fields { get; private set; }

        public int InputAddressCount {
            get {
                return Fields.Count * PixelAddressCount;
            }
        }

        public int[] DmxValues {
            get {
                return pixelPatch.GetPixelValues();
            }
        }

        public void SetMode(IMode m) {
            Fields = m.GetFields(pixelPatch.GetPixelPatch());
            addressCount = 0;
            foreach (Field  f in Fields) {
                addressCount += f.Pixels.Count * f.Pixels[0].AddressCount;     
            }
        }

        public void AddUpdateHandler(IFixtureUpdateHandler handler) {
            if (!updateHandlers.Contains(handler)) {
                updateHandlers.Add(handler);
            }
        }

        public void RemoveUpdateHandler(IFixtureUpdateHandler handler) {
            updateHandlers.Remove(handler);
        }

        void IDmxPacketHandler.HandlePacket(ArtDmxPacket packet) {
            Port packetPort = new Port(packet.PhysicalPort, packet.SubNet, packet.Universe);
            if (!Address.Port.Equals(packetPort)) return;

            // We are interested in this packet so process it

            int offset = this.Address.DmxAddress;

            foreach (Field f in Fields) {
                int relOffset = f.AddressCount * f.Index;
                int [] dmx = Utils.GetSubArray(packet.DmxData, offset + relOffset, f.AddressCount)
                    .Select(x => (int)x).ToArray();
                f.SetDmxValues(dmx);
            }
            foreach (IFixtureUpdateHandler fuh in updateHandlers)
            {
                fuh.OnUpdate(this);
            }
        }
    }
}
