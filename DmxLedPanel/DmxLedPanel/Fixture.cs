using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;
using DmxLedPanel.Modes;
using DmxLedPanel.State;

namespace DmxLedPanel
{
    public class Fixture : IDmxPacketHandler
    {
        private static int idCounter = 0;

        private List<IFixtureUpdateHandler> updateHandlers;
        private int addressCount;
        private IPixelPatch pixelPatch;
        private IMode mode;

        /// <summary>
        /// Use this with caution.. 
        /// Usful when reloading whole state of app. 
        /// </summary>
        public static void ResetIdCounter() { idCounter = 0; }

        public Fixture(IMode m, IPixelPatch pixelPatch) {
            this.pixelPatch = pixelPatch;
            this.mode = m;
            SetMode(m);
            Address = new Address();
            updateHandlers = new List<IFixtureUpdateHandler>();
            ID = idCounter++;
            Name = "Fixture " + ID;
        }

        // FOR TESTS

            public IPixelPatch PixelPatch { get { return pixelPatch; } }

       // END OF FOR TESTS

        public int ID {
            get; private set;
        }

        public string Name { get; set; }

        public Address Address { get; set; }

        /// <summary>
        /// -1 if upatched otherwise value indicates output id
        /// </summary>
        public int PatchedTo { get; set; } = Output.FIXTURE_UNPATCH;

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

        public int OutputAddressCount {
            get {
                return PixelPatch.Columns * PixelPatch.Rows * Const.PIXEL_LENGTH;
            }
        }

        public int[] DmxValues {
            get {
                return pixelPatch.GetPixelValues();
            }
        }

        public void SetMode(IMode m) {
            this.mode = m;
            Fields = this.mode.GetFields(pixelPatch.GetPixelPatch());
            addressCount = 0;
            foreach (Field  f in Fields) {
                addressCount += f.Pixels.Count * f.Pixels[0].AddressCount;     
            }
        }

        /// <summary>
        /// Will fail if fixture is pattched to output
        /// </summary>
        /// <param name="patch"></param>
        /// <returns>Return true if patch was successful / false if not</returns>
        public bool TrySetPatch(IPixelPatch patch) {
            if (PatchedTo == Output.FIXTURE_UNPATCH) {
                this.pixelPatch = patch;
                SetMode(this.mode);
                return true;
            }
            return false;
        }

        public IMode getMode() {
            return this.mode;
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
            Update();
        }

        public void Update() {
            foreach (IFixtureUpdateHandler fuh in updateHandlers)
            {
                fuh.OnUpdate(this);
            }
        }

        public void SetHighlight(bool on) {
            if (on)
            {
                updateAllValuesTo(255);
            }
            else {
                updateAllValuesTo(0);
            }
        }

        private void updateAllValuesTo(int inteisty) {
            foreach (Field f in Fields)
            {
                f.SetDmxValues(Enumerable.Repeat(inteisty, f.AddressCount).ToArray());
            }
        }
    }
}
