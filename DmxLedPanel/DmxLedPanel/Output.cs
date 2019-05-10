
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.Containers;
using System.Diagnostics;
using System.Threading;
using Haukcode.ArtNet.Sockets;
using Haukcode.ArtNet.Packets;
using DmxLedPanel.ArtNetIO;
using System.Net.Sockets;

namespace DmxLedPanel
{
    public class Output : IFixtureUpdateHandler, IDisposable
    {

        public static readonly int DMX_PACKET_POOL_SIZE = 128;

        // dmx lag debug stuff
        private string talkerSource = Talker.Talker.GetSource();
        private Stopwatch frameTimer = Stopwatch.StartNew();
        // END

        public static readonly IPAddress DEFAULT_IP = IPAddress.Parse("255.255.255.255");

        public static readonly int FIXTURE_UNPATCH = -1;
        private static readonly int PORT_ADDRESS_COUNT = 510;
        private static readonly int PORT_COUNT = 2;
        private static int idCounter = 0;


        private List<Fixture> fixtures;
        private List<Fixture> updatePendingFixtures;
        private int updatePending;

        private List<Port> ports;
        private int addressCount;
        private int patchedAdresses = 0;
        private IPAddress ipAddress;
        private Socket socket;
  
        private static readonly string TAG = Talker.Talker.GetSource();
        public static long DMX_PACKET_COUNTER = 0;

        private object synclock = new object();

        private static int universeCounter = 0;

        private static readonly int THREAD_ACCESS_BUUFER_SIZE = 256;
        //int[] dmxValues = new int[1020];
        int[] dmxValuesArray;
        List<ArtNetDmxPacket> packets = new List<ArtNetDmxPacket>();




        //static Output() {
        //    idCounterr = StateManager.Instance.State.GetLastOutputId() + 1;
        //}

        public static void ResetIdCounter() { idCounter = 0; }

        public Output()
        {
            addressCount = PORT_COUNT * PORT_ADDRESS_COUNT;
            fixtures = new List<Fixture>();
            updatePendingFixtures = new List<Fixture>();
            updatePending = 0;
            Ports = new List<Port>(PORT_COUNT);
            ID = idCounter++;
            Name = "Output " + ID;
            socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp
                );
            var ip = Util.SettingManager.Instance.Settings.ArtNetBroadcastIp;
            ipAddress = IPAddress.Parse(ip);
            dmxValuesArray = new int[1020];
            
        }

        public int ID { get; private set; }

        public string Name { get; set; }

        public List<Port> Ports {
            get { return ports; }
            set {
                if (value.Count > 2)
                {
                    throw new ArgumentOutOfRangeException("Port count should not be larger than 2");
                }
                ports = new List<Port>();
                ports.AddRange(value);
            }
        }

        public string IP {
            get {
                return ipAddress.ToString();
            }
            set {
                bool valid = IPAddress.TryParse(value, out ipAddress);
                if (!valid)
                {
                    Console.WriteLine("not valid ip address - outpt address set to " + DEFAULT_IP);
                    ipAddress = DEFAULT_IP;
                }
            }
        }

        public bool TryPatchFixture(Fixture f)
        {
            if (!fixtures.Contains(f) && (patchedAdresses + f.OutputAddressCount) < addressCount)
            {
                f.PatchedTo = this.ID;
                fixtures.Add(f);
                f.AddUpdateHandler(this);
                patchedAdresses += f.OutputAddressCount;
                ResetUpdatePending();
                return true;
            }
            return false;
        }

        public bool TryPatchFixtures(List<Fixture> fixtures)
        {
            int pa = patchedAdresses;
            foreach (Fixture f in fixtures)
            {
                pa += f.OutputAddressCount;
                if (pa > addressCount) return false;
            }
            foreach (Fixture f in fixtures)
            {
                TryPatchFixture(f);
            }
            return true;
        }


        public void OnUpdate(Fixture f)
        {
            updatePending--;
            updatePendingFixtures.Remove(f);
            if (updatePendingFixtures.Count <= 0)
            {

                //ThreadPool.QueueUserWorkItem(i => WriteOutput());
                WriteOutput();
                ResetUpdatePending();
            }
        }

        public List<Fixture> GetFixtures()
        {
            return this.fixtures;
        }

        public bool ContainFixture(int id)
        {
            foreach (Fixture f in fixtures)
            {
                if (f.ID == id) return true;
            }
            return false;
        }

        public Fixture GetFixture(int id)
        {
            foreach (Fixture f in fixtures)
            {
                if (f.ID == id) return f;
            }
            return null;
        }

        public Fixture UnpatchFixture(int id)
        {
            Fixture rmFix = null;
            foreach (Fixture f in fixtures)
            {
                if (f.ID == id) rmFix = f;
            }
            if (rmFix != null)
            {
                // Remove and repatch fixtures
                rmFix.PatchedTo = FIXTURE_UNPATCH;
                fixtures.Remove(rmFix);
                rmFix.RemoveUpdateHandler(this);
                var tmpFix = this.fixtures;
                fixtures = new List<Fixture>();
                //rest to 0 because we are repatching all remaining fixtures
                patchedAdresses = 0;
                foreach (Fixture f in tmpFix)
                {
                    TryPatchFixture(f);
                }
                return rmFix;
            }
            return null;
        }

        public List<Fixture> UnpatchAll()
        {
            var fix = new List<Fixture>();
            foreach (var f in fixtures)
            {
                f.PatchedTo = FIXTURE_UNPATCH;
                f.RemoveUpdateHandler(this);
                fix.Add(f);
                fixtures.Remove(f);
            }
            patchedAdresses = 0;
            return fix;
        }


        private void WriteOutput()
        {

            var copyIndex = 0;
            foreach (Fixture f in fixtures)
            {
                Array.Copy(f.DmxValues, 0, dmxValuesArray, copyIndex, f.DmxValues.Length);
                copyIndex += f.DmxValues.Length;
            }

            packets.Clear();
            foreach (Port p in Ports)
            {

                var packet = new ArtNetDmxPacket();
                packet.Physical = (byte)p.Net;
                packet.Universe = (byte)((p.SubNet * 16) + p.Universe);
                packets.Add(packet);
            }

            // copy data to packets
            copyIndex = 0;
            foreach (ArtNetDmxPacket pack in packets)
            {
                pack.DmxData = Utils.GetSubArray(dmxValuesArray, copyIndex, 510, 512).Select(x => (byte)x).ToArray();
                copyIndex += 510;
                socket = ArtnetOut.Instance.Socket;
                socket.SendTo(pack.ToArray(), new IPEndPoint(ipAddress, ArtNetSocket.Port));
                universeCounter++;
            }


        }

        private void ResetUpdatePending()
        {
            updatePendingFixtures.Clear();
            updatePendingFixtures.AddRange(fixtures);
        }

        public static List<FixtureOutputMap> GetPatchedFixtureOutputMap(List<Fixture> fixtures)
        {
            var state = StateManager.Instance.State;
            return fixtures.Aggregate(new List<FixtureOutputMap>(), (fom, f) =>
            {
                if (f.PatchedTo >= 0)
                {
                    fom.Add(new FixtureOutputMap(f.ID, state.GetOutputByFixture(f.ID), f));
                }
                return fom;
            });
        }

        public static string GetOutputInfoStr(List<Output> outputs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var o in outputs)
            {
                sb.Append(o.Name + " ");
            }
            return sb.ToString().Trim();
        }

        public static string getOutputInfoStr(Fixture f)
        {
            return f.Name;
        }

        public static string GetOutputListNameString(List<Output> list)
        {
            string str = list.Aggregate("", (s, o) => s + o.Name + ", ");
            return str.Substring(0, str.Length - 2);
        }

        public static void CalculateUniversesPerFrameSent()
        {
            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith((t) =>
                {
                    Console.WriteLine("Universes sent in a 1 frame" + universeCounter / 30);
                    universeCounter = 0;
                    CalculateUniversesPerFrameSent();
                });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
