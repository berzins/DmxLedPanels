using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DmxLedPanel.Modes;
using DmxLedPanel.State;
using DmxLedPanel.Fixtures;
using System.Diagnostics;
using System.Collections.Concurrent;
using Haukcode.ArtNet.Packets;

namespace DmxLedPanel
{
    public class Fixture : IDmxPacketHandler
    {
        private static readonly string TAG = Talker.Talker.GetSource();

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
        /// Useful when reloading whole state of app. 
        /// </summary>
        public static void ResetIdCounter() { idCounter = 0; }

        public Fixture(List<IMode> modes, IPixelPatch pixelPatch) {
            dmxUtils = new List<FixtureDmxUtil>();

            this.modes = modes;
            Fields = new List<Field>();

            AddDmxUtil(new DmxModeSwitcher(0) { Values = new int[] { 256 } });
            this.pixelPatch = pixelPatch;
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
            dmxUtils.Add(dmxUtil);
        }

        public bool IsDmxUtilsEnabled { get; set; } = false;
        
        /// <summary>
        /// returns -1 if upatched otherwise value indicates output id
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

        public void SetModes(List<IMode> modes) {
            lock (lockref) {
                this.modes = modes;
                TrySwitchMode(CurrentModeIndex < this.modes.Count ? CurrentModeIndex : 0);
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
            } catch (IndexOutOfRangeException) {
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


        public bool TrySwitchMode(int index) {
            try
            {
                var mode = modes.ElementAt(index);
                SetMode(mode);
                currentModeIndex = index;
                return true;
            }
            catch (IndexOutOfRangeException e) {
                Talker.Talker.Log(new Talker.ActionMessage
                {
                    Level = Talker.LogLevel.WARNING,
                    Source = Talker.Talker.GetSource(),
                    Message = "Failed to swtich mode. Mode index is not in range of the fixture modes. " + e.Message
                });
                return false;
            }
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
        /// Change internal pixel order.
        /// Will fail if fixture is pattched to output.
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

        int IDmxPacketHandler.GetPortHash() {
            return Address.Port.GetHashCode();
        }

        void IDmxPacketHandler.HandlePacket(ArtNetDmxPacket packet) {
            Port packetPort = Port.From(packet);

            if (!Address.Port.Equals(packetPort)) return;

            // We are interested in this packet so process it
            if (IsDmxUtilsEnabled) {
                handleDmxUtil(packet);
            }
            handlePixelDmx(packet);
        }

        private void handlePixelDmx(ArtNetDmxPacket packet) {

            int offset = this.Address.DmxAddress - 1; // "-1" convert to 0 based index

            foreach (Field f in Fields)
            {
                int relOffset = f.AddressCount * f.Index;
                byte[] dmx = Utils.GetSubArray(packet.DmxData, offset + relOffset, f.AddressCount);
                f.SetDmxValues(dmx);
            }
            Update();
        }

        private void handleDmxUtil(ArtNetDmxPacket packet) {
            int offset = this.UtilAddress.DmxAddress;

            foreach (var du in dmxUtils) {
                du.HandleDmx(
                    this, 
                    Utils.GetSubArray(packet.DmxData, offset + du.Index -1, 1)
                    .Select(x => (int)x).ToArray());
            }
        }

        public void Update() {

            try
            {
                foreach (IFixtureUpdateHandler fuh in updateHandlers)
                {
                    fuh.OnUpdate(this);
                }
            }
            catch (InvalidOperationException e)
            {
                Talker.Talker.Log(new Talker.ActionMessage()
                {
                    Level = Talker.LogLevel.ERROR,
                    Source = Talker.Talker.GetSource(),
                    Message = "Fixture faild to update -> most likely on removal from the output: " + e.ToString()
                });
            }
        }

        public List<IFixtureUpdateHandler> GetOnUpdateHandlers() {
            return updateHandlers;
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

        private void updateAllValuesTo(byte inteisty) {
            foreach (Field f in Fields)
            {
                f.SetDmxValues(Enumerable.Repeat(inteisty, f.AddressCount).ToArray());
            }
        }

        public static string GetFixtureInfoStr(List<Fixture> fixtures) {
            StringBuilder sb = new StringBuilder();
            foreach (var f in fixtures) {
                sb.Append(f.Name + " ");
            }
            return sb.ToString().Trim();
        }

        public static string getFixtureInfoStr(Fixture f) {
            return f.Name;
        }

        public static string GetFixtureListNameString(List<Fixture> list) {
            if (list.Count > 0)
            {
                string str = list.Aggregate("", (s, f) => s + f.Name + ", ");
                return str.Substring(0, str.Length - 2);
            }
            return "";
        }


    }
}
