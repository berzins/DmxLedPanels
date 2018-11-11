﻿using ArtNet.ArtPacket;
using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DmxLedPanel.Containers;

namespace DmxLedPanel
{
    public class Output : IFixtureUpdateHandler
    {

        public static readonly IPAddress DEFAULT_IP = IPAddress.Parse("255.255.255.255");

        public static readonly int FIXTURE_UNPATCH = -1;
        private static readonly int PORT_ADDRESS_COUNT = 510;
        private static readonly int PORT_COUNT = 2;
        private static int idCounter = 0;


        private List<Fixture> fixtures, updatePending;

        private List<Port> ports;
        private int addressCount;
        private int patchedAdresses = 0;
        private IPAddress ipAddress;
        private ArtNet.ArtNetWritter writer;
        

        //static Output() {
        //    idCounter = StateManager.Instance.State.GetLastOutputId() + 1;
        //}

        public static void ResetIdCounter() { idCounter = 0; }

        public Output() {
            addressCount = PORT_COUNT * PORT_ADDRESS_COUNT;
            fixtures = new List<Fixture>();
            updatePending = new List<Fixture>();
            Ports = new List<Port>(PORT_COUNT);
            ID = idCounter++;
            Name = "Output " + ID;

            var ip = Util.SettingManager.Instance.Settings.ArtNetBroadcastIp;
            ipAddress = IPAddress.Parse(ip);
            writer = new ArtNet.ArtNetWritter(ipAddress);
        }

        public int ID { get; private set; }

        public string Name { get; set; }

        public List<Port> Ports {
            get { return ports; }
            set {
                if (value.Count > 2) {
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
                    if (valid)
                    {
                        writer = new ArtNet.ArtNetWritter(ipAddress);
                    }
                    else {
                        Console.WriteLine("not valid ip address - outpt address set to " + DEFAULT_IP);
                        ipAddress = DEFAULT_IP;
                        writer = new ArtNet.ArtNetWritter(ipAddress);
                    }
            }
        }
        
        public bool TryPatchFixture(Fixture f) {
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

        public bool TryPatchFixtures(List<Fixture> fixtures) {
            int pa = patchedAdresses;
            foreach (Fixture f in fixtures) {
                pa += f.OutputAddressCount;
                if (pa > addressCount) return false;
            }
            foreach (Fixture f in fixtures) {
                TryPatchFixture(f);
            }
            return true;
        }


        public void OnUpdate(Fixture f) {

            updatePending.Remove(f);
            if (updatePending.Count == 0)
            {
                WriteOutput();
                ResetUpdatePending();
            }
        }

        public List<Fixture> GetFixtures() {
            return this.fixtures;
        }

        public bool ContainFixture(int id) {
            foreach (Fixture f in fixtures) {
                if (f.ID == id) return true;
            }
            return false;
        }

        public Fixture GetFixture(int id) {
            foreach (Fixture f in fixtures)
            {
                if (f.ID == id) return f;
            }
            return null;
        }

        public Fixture UnpatchFixture(int id) {
            Fixture rmFix = null;
            foreach (Fixture f in fixtures) {
                if (f.ID == id) rmFix = f;
            }
            if (rmFix != null) {
                // Remove and repatch fixtures
                rmFix.PatchedTo = FIXTURE_UNPATCH;
                fixtures.Remove(rmFix);
                rmFix.RemoveUpdateHandler(this);
                var tmpFix = this.fixtures;
                fixtures = new List<Fixture>();
                //rest to 0 because we are repatching all remaining fixtures
                patchedAdresses = 0;
                foreach (Fixture f in tmpFix) {
                    TryPatchFixture(f);
                }
                return rmFix;
            }
            return null;
        }

        public List<Fixture> UnpatchAll() {
            var fix = new List<Fixture>();
            foreach (var f in fixtures) {
                f.PatchedTo = FIXTURE_UNPATCH;
                f.RemoveUpdateHandler(this);
                fix.Add(f);
                fixtures.Remove(f);
            }
            patchedAdresses = 0;
            return fix;
        }

        private void WriteOutput() {

            int[] dmxValues = new int[1020];
            int copyIndex = 0;

            foreach (Fixture f in fixtures) {
                Array.Copy(f.DmxValues, 0, dmxValues, copyIndex, f.DmxValues.Length);
                copyIndex += f.DmxValues.Length;
            }

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
            //ArtNet.Utils.ArtPacketStopwatch.Stop();
            //Console.WriteLine("packet took " + ArtNet.Utils.ArtPacketStopwatch.ElapsedMilliseconds + " mils to process");
        }

        private void ResetUpdatePending() {
            updatePending = new List<Fixture>();
            foreach (Fixture f in fixtures) {
                updatePending.Add(f);
            }
        }

        public static List<FixtureOutputMap> GetPatchedFixtureOutputMap(List<Fixture> fixtures) {
            var state = StateManager.Instance.State;
            return fixtures.Aggregate(new List<FixtureOutputMap>(), (fom, f) => {
                if (f.PatchedTo >= 0) {
                    fom.Add(new FixtureOutputMap(f.ID, state.GetOutputByFixture(f.ID), f));
                }
                return fom;
            });
        }
    }
}
