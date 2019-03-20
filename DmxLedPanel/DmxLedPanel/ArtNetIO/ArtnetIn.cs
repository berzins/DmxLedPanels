﻿using ArtNet;
using System;
using System.Collections.Generic;
using ArtNet.ArtPacket;
using System.Net;
using System.Diagnostics;
using System.Threading;
using DmxLedPanel.Util;
using System.Threading.Tasks;

namespace DmxLedPanel.ArtNetIO
{
    public class ArtnetIn : IDmxSignalListener
    {
        private static ArtnetIn instance;
        private ArtNetReader reader;
        private readonly ArtDispatcher dispatcher;
        private volatile List<IDmxPacketHandler> dmxPacketHandlers;
        public delegate void DmxSignalDeleage(bool hasSingal);
        public event DmxSignalDeleage DmxSignalChanged;
        public delegate void PortDmxSignalDelegate(HashSet<Port> activeProts);
        public event PortDmxSignalDelegate PortDmxSignalChanged;
        private object lockref = new object();
        private static readonly object padlock = new object();

        private ArtnetIn() {
            dispatcher = InitDispatcher();
            reader = new ArtNetReader(dispatcher, IPAddress.Parse(
                SettingManager.Instance.Settings.ArtNetBindIp
                ));
            dmxPacketHandlers = new List<IDmxPacketHandler>();
        }

        public static ArtnetIn Instance {
            get {
                if (instance == null) {
                    lock (padlock) {
                        if (instance == null) {
                            instance = new ArtnetIn();
                        }
                    }
                }
                return instance;
            }
        }

        public void Start() {
            reader.Start();
        }

        public void Stop() {
            reader.Stop();
        }
        
        public void AddDmxPacketListener(IDmxPacketHandler handler) {
            lock (lockref) {
                var c = new List<IDmxPacketHandler>(dmxPacketHandlers);
                c.Add(handler);
                dmxPacketHandlers = c;
            }
        }

        public void AddDmxPacketListeners(List<IDmxPacketHandler> handlers) {
            foreach (IDmxPacketHandler h in handlers) {
                AddDmxPacketListener(h);
            }
        }

        public List<IDmxPacketHandler> ClearDmxPacketListeners() {
            var r = new List<IDmxPacketHandler>();
            foreach (IDmxPacketHandler h in dmxPacketHandlers) {
                r.Add(h);
            }
            lock (lockref)
            {
                var c = new List<IDmxPacketHandler>(dmxPacketHandlers);
                c.Clear();
                dmxPacketHandlers = c;
            }
            return r;
        }

        public void ClearDmxSignalListeners()
        {
            DmxSignalChanged = null;
        }

        public void UpdateDmxPacketListeners(List<IDmxPacketHandler> handlers) {
            ClearDmxPacketListeners();
            AddDmxPacketListeners(handlers);
        }


        private ArtDispatcher InitDispatcher() {
            ArtDispatcher d = new ArtDispatcher();
            d.AddArtDmxListener(new ArtDmxListener(this));
            d.AddArtPollListener(new ArtPollListener());
            return d;
        }


        public bool HasSignal { get; private set; } = false;

        public void OnSignalChange(bool detected)
        {
            HasSignal = detected;
            DmxSignalChanged?.Invoke(HasSignal);
        }

        public HashSet<Port> PortsWithDmxSignal { get; private set; } = new HashSet<Port>();

        public void OnPortSignalChange(HashSet<Port> activePorts) {
            PortsWithDmxSignal = activePorts;
            PortDmxSignalChanged?.Invoke(PortsWithDmxSignal);    
        } 
        
        // Art Net packet handlers down there
        public abstract class ArtnetListener : ArtListener{
            protected ArtnetIn artin;

            public ArtnetListener(ArtnetIn artin)
            {
                this.artin = artin;
            }

            public abstract void Action(Packet p, IPAddress source);
        }

        private class ArtDmxListener : ArtnetListener
        {
            private static readonly int DMX_INPUT_TIMEOUT = 1000;

            private object synclock = new object();
            private static bool recieved = false;
            private bool hasSignal = false;

            private volatile HashSet<Port> recievedUniverses;
            private volatile HashSet<Port> dmxDetectedUniverses;

            public ArtDmxListener(ArtnetIn artin) : base(artin){
                recievedUniverses = new HashSet<Port>(Port.GetEqualityComparer());
                dmxDetectedUniverses = new HashSet<Port>(Port.GetEqualityComparer());
                WatchDmxInput();
                WatchPortDmxInput();
            }
          

            private void WatchDmxInput() {
                new Thread(() => {
                    // log 
                    Talker.Talker.Log(new Talker.ActionMessage()
                    {
                        Message = "DMX tracking thread started.",
                        Level = Talker.LogLevel.INFO,
                        Source = Talker.Talker.GetSource()
                    });
                    // logic
                    while (true) {
                        if (recieved)
                        {
                            if (!hasSignal) {
                                hasSignal = true;
                                artin.OnSignalChange(hasSignal);
                            }
                            recieved = false;
                        }
                        else {
                            if (hasSignal) {
                                hasSignal = false;
                                artin.OnSignalChange(hasSignal);
                            }
                        }
                        Thread.Sleep(DMX_INPUT_TIMEOUT);
                    }
                }).Start();
            }

            private bool IsDmxForPortsChanged() {
                if (!dmxDetectedUniverses.IsSupersetOf(recievedUniverses)) {
                    return true;
                }
                if (!recievedUniverses.IsSupersetOf(dmxDetectedUniverses))
                {
                    return true;
                }
                return false;
            }

            private void WatchPortDmxInput() {
                ThreadPool.QueueUserWorkItem((i) => {
                    // log 
                    Talker.Talker.Log(new Talker.ActionMessage()
                    {
                        Message = "Port DMX tracking thread started.",
                        Level = Talker.LogLevel.INFO,
                        Source = Talker.Talker.GetSource()
                    });

                    //logic
                    while (true)
                    {
                        var isChanged = IsDmxForPortsChanged();
                        if (isChanged)
                        {
                            artin.OnPortSignalChange(Port.CopyHashSet(recievedUniverses));

                            lock (synclock)
                            {
                                var l = dmxDetectedUniverses;
                                l = new HashSet<Port>();
                                l.UnionWith(recievedUniverses);
                                dmxDetectedUniverses = l;
                            }
                        }

                        lock (synclock)
                        {
                            var l = recievedUniverses;
                            l.Clear();
                            recievedUniverses = l;
                        }

                        Thread.Sleep(DMX_INPUT_TIMEOUT);
                    }
                });
            }

            private bool HasEntry(HashSet<Port> data, Port key) {
         
                bool b = data.Contains(key);
                return b;
            }

            public override void Action(Packet p, IPAddress source)
            {
                var packet = ((ArtDmxPacket)p);
                foreach (IDmxPacketHandler h in artin.dmxPacketHandlers)
                {
                    h.HandlePacket(packet);
                }

                recieved = true;
                ThreadPool.QueueUserWorkItem((i) =>
                {
                    var port = new Port(packet.PhysicalPort, packet.SubnetUniverse.SubNet, packet.SubnetUniverse.Universe);
                    if (!HasEntry(recievedUniverses, port))
                    {
                        lock (synclock)
                        {
                            var l = recievedUniverses;
                            l.Add(port);
                            recievedUniverses = l;
                        }
                    }
                });
                //new Thread(() =>
                //{
                //    var port = new Port(packet.PhysicalPort, packet.SubnetUniverse.SubNet, packet.SubnetUniverse.Universe);
                //    if (!HasEntry(recievedUniverses, port))
                //    {
                //        lock (synclock)
                //        {
                //            var l = recievedUniverses;
                //            l.Add(port);
                //            recievedUniverses = l;
                //        }
                //    }
                //}).Start();
            }
        }

        private class ArtPollListener : ArtListener
        {

            public void Action(Packet p, IPAddress source)
            {
                var reply = new ArtPollReplyPacket()
                {
                    ShortName = "a-sound led ctrl",
                    LongName = "Dievs sveti Latviju!"
                };
                
                IPAddress ip;
                if (NetworkUtils.TryGetBroadcastAddress(source, out ip)) {
                    new ArtNetWritter(ip).Write(reply);
                }
            }
        }
    }
}
