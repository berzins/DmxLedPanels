using ArtNet;
using System;
using System.Collections.Generic;
using ArtNet.ArtPacket;
using System.Net;
using System.Diagnostics;
using System.Threading;
using DmxLedPanel.Util;

namespace DmxLedPanel.ArtNetIO
{
    public class ArtnetIn : IDmxSignalListener
    {
        private static ArtnetIn instance;
        private ArtNetReader reader;
        private ArtDispatcher dispatcher;
        private volatile List<IDmxPacketHandler> dmxPacketHandlers;
        private volatile List<IDmxSignalListener> signalListeners;
        private object lockref = new object();
        private static readonly object padlock = new object();

        private ArtnetIn() {
            dispatcher = InitDispatcher();
            reader = new ArtNetReader(dispatcher, IPAddress.Parse(
                SettingManager.Instance.Settings.ArtNetBindIp
                ));
            dmxPacketHandlers = new List<IDmxPacketHandler>();
            signalListeners = new List<IDmxSignalListener>();
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

        
        public void AddDmxSignalListener(IDmxSignalListener l) {
            lock (lockref) {
                var c = new List<IDmxSignalListener>(signalListeners);
                c.Add(l);
                signalListeners = c;
            }
        }

        public void RemoveDmxSignalListener(IDmxSignalListener l)
        {
            lock (lockref)
            {
                var c = new List<IDmxSignalListener>(signalListeners);
                c.Remove(l);
                signalListeners = c;
            }
        }

        public void ClearDmxSignalListeners()
        {
            lock (lockref)
            {
                var c = new List<IDmxSignalListener>(signalListeners);
                c.Clear();
                signalListeners = c;
            }
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
            
            foreach (IDmxSignalListener l in signalListeners) {
                l.OnSignalChange(HasSignal);
            }
            
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
            private object synclock = new object();
            private static bool recieved = false;
            private bool hasSignal = false;


            public ArtDmxListener(ArtnetIn artin) : base(artin){
               watchDmxInput();
            }

            private void watchDmxInput() {

                new Thread(() => {
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
                        Thread.Sleep(500);
                    }
                }).Start();
            }

            public override void Action(Packet p, IPAddress source)
            {
                recieved = true;
               
                foreach (IDmxPacketHandler h in artin.dmxPacketHandlers)
                {
                    h.HandlePacket((ArtDmxPacket)p);
                }
                
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

                IPAddress ip = NetworkUtils.GetBroadcastAddress(source, NetworkUtils.GetSubnetMask(source));

                new ArtNetWritter(ip).Write(reply);
            }
        }
    }
}
