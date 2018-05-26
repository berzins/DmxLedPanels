using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;
using DmxLedPanel.Modes;
using DmxLedPanel.State;
using DmxLedPanel.Fixtures;

namespace DmxLedPanel
{
    public class Fixture : IDmxPacketHandler
    {
        private static int idCounter = 0;

        private List<IFixtureUpdateHandler> updateHandlers;
        private int addressCount;
        private IPixelPatch pixelPatch;
        private IMode mode;
        private List<FixtureDmxUtil> dmxUtils;

        private volatile List<IMode> modes;
        private int currentModeIndex = 0;
        private object lockref = new object();

        /// <summary>
        /// Use this with caution.. 
        /// Usful when reloading whole state of app. 
        /// </summary>
        public static void ResetIdCounter() { idCounter = 0; }

        public Fixture(List<IMode> modes, IPixelPatch pixelPatch) {
            dmxUtils = new List<FixtureDmxUtil>();
            modes = new List<IMode>();
            Fields = new List<Field>();

            dmxUtils.Add(new DmxModeSwitcher(0));
            this.pixelPatch = pixelPatch;
            this.mode = modes[currentModeIndex];
            SetMode(modes[currentModeIndex]);
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
        /// 
        /// </summary>
        public Address UtilAddress { get; set; }

        public int UtilAddressCount { get; protected set; }

        public List<FixtureDmxUtil> DmxUtils {
            get {
                var fdus = new List<FixtureDmxUtil>();
                foreach (var fdu in dmxUtils) {
                    fdus.Add(fdu.Clone());
                }
                return fdus;
            }
        }

        protected void AddDmxUtil(FixtureDmxUtil dmxUtil) {
            DmxUtils.Add(dmxUtil);
        }

        public bool IsDmxUtilsEnabled { get; set; } = false;
        
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

        // be careful with this.. do not modify list. 
        // basically this is necessary for state serialization
        public List<IMode> Modes {
            get {
                return this.modes;
            }
        }

        public int CurrentModeIndex {
            get {
                return currentModeIndex;
            }
        }

        public void SetMode(IMode m) {

            lock (lockref) {
                this.mode = m;
                List<Field> f = new List<Field>(Fields);
                f = this.mode.GetFields(pixelPatch.GetPixelPatch());
                Fields = f;
            }
            addressCount = 0;
            foreach (Field  f in Fields) {
                addressCount += f.Pixels.Count * f.Pixels[0].AddressCount;     
            }
        }

        public void AddMode(IMode m) {
            modes.Add(m);
        }

        public void RemoveMode(IMode m) {
            modes.Remove(m);
        }

        public void RemoveMode(int index) {
            modes.RemoveAt(index);
        }

        /// <summary>
        /// On success returns removed mode, else reurn null
        /// </summary>
        public IMode TryRemoveMode(int index) {
            try {
                var mode = modes.ElementAt(index);
                modes.RemoveAt(index);
                return mode;
            } catch (IndexOutOfRangeException e) {
                return null;
            }
        }

        public int ModeCount {
            get {
                return modes.Count();
            }
        }

        public void SwitchMode(int index) {
            SetMode(modes[index]);
            currentModeIndex = index;
        }

        public void SwitchMode(IMode mode) {
            SetMode(mode);
        }

        public bool TryGetModeById(int id, out IMode mode) {
            foreach (var m in modes) {
                if (m.Id == id) {
                    mode = m;
                    return true;
                }
            }
            mode = null;
            return false;
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


            if (IsDmxUtilsEnabled) {
                handleDmxUtil(packet);
            }
            handlePixelDmx(packet);
        }

        private void handlePixelDmx(ArtDmxPacket packet) {
            int offset = this.Address.DmxAddress - 1; // "-1" convert to 0 based index

           foreach (Field f in Fields) {
                int relOffset = f.AddressCount * f.Index;
                int [] 
                    dmx = Utils.GetSubArray(packet.DmxData, offset + relOffset, f.AddressCount)
                    .Select(x => (int)x).ToArray();
                f.SetDmxValues(dmx);
            }
            Update();            
        }

        private void handleDmxUtil(ArtDmxPacket packet) {
            int offset = this.UtilAddress.DmxAddress;

            foreach (var du in dmxUtils) {
                du.HandleDmx(this, Utils.GetSubArray(packet.DmxData, offset + du.Index, 1)[0]);
            }
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
