using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DmxLedPanel.Util;
using Haukcode.ArtNet.Packets;

namespace DmxLedPanel.ArtNetIO
{
    public class PortSignalTracker : IDmxPacketHandler, IDisposable
    {
        private static readonly int PORT_SIZE = 16;
        private static readonly int TIMEOUT = 500; //millisecons

        private bool[,] lastCycle;
        private bool[,] currentCycle;
        Thread trackingThread;
        private bool processing = false;

        public bool hasDmxSignal = false;

        public delegate void PortSignalChangeDelegate(HashSet<Port> activePorts);
        public event PortSignalChangeDelegate OnPortSignalChanged;

        public delegate void DmxSignalChangeDelegate(bool hasDmxSignal);
        public event DmxSignalChangeDelegate OnDmxSignalChanged;


        public PortSignalTracker() {
            lastCycle = GetNewSignalArray();
            currentCycle = GetNewSignalArray();
        }

        public void Start()
        {
            StartTracking();
        }

        public void Stop()
        {
            trackingThread.Abort();
            trackingThread = null;
        }

        public void StartTracking() {
            if (trackingThread != null) {
                trackingThread.Abort();
                trackingThread = null;
            }

            trackingThread = new Thread(() =>
            {
                while (true) {
                    processing = true;
                    if (isSignalChanged()) {
                        // get active prots
                        var activePorts = GetActivePorts();
                        // notify listeners
                        if (!hasDmxSignal && activePorts.Count > 0) {
                            hasDmxSignal = true;
                            OnDmxSignalChanged?.Invoke(hasDmxSignal);
                        }
                        OnPortSignalChanged?.Invoke(activePorts);

                        // set current as last
                        lastCycle = Utils.cloneArray(currentCycle);
                        
                    }
                    currentCycle = GetNewSignalArray();
                    processing = false;
                    Thread.Sleep(TIMEOUT);
                }
            });
            trackingThread.Start();

        }

        private bool isSignalChanged() {
            if (Object.ReferenceEquals(lastCycle, currentCycle)) {
                return false;
            }

                for (int subNet = 0; subNet < PORT_SIZE; subNet++)
                {
                    for (int uni = 0; uni < PORT_SIZE; uni++)
                    {
                        if (lastCycle[subNet, uni] != currentCycle[subNet, uni]) {
                            return true;
                        }
                    }
                }
            
            return false;
        }

        private HashSet<Port> GetActivePorts() {
            var ports = new HashSet<Port>();
                for (int subNet = 0; subNet < PORT_SIZE; subNet++)
                {
                    for (int uni = 0; uni < PORT_SIZE; uni++)
                    {
                        if (currentCycle[subNet, uni] == true) {
                            ports.Add(new Port(0, subNet, uni));
                        }
                    }
                }
            return ports;
        }

        public void HandlePacket(ArtNetDmxPacket packet)
        {
            if (processing)
            {
                return;
            }
            var port = Port.From(packet);
            currentCycle[port.SubNet, port.Universe] = true;
        }

        public List<Port> GetPortsRequired()
        {
            return PortHelper.AllPorts;
        }

        private bool[,] GetNewSignalArray() {
            return new bool[PORT_SIZE, PORT_SIZE];
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
