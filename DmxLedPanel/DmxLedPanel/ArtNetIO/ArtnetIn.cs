
using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Threading;
using DmxLedPanel.Util;
using System.Threading.Tasks;
using DmxLedPanel.State;
using Haukcode.ArtNet.Sockets;
using Haukcode.Sockets;
using Haukcode.ArtNet.Packets;
using Haukcode.ArtNet;
using System.Linq;

namespace DmxLedPanel.ArtNetIO
{
    public class ArtnetIn : IDmxSignalListener
    {
        private static ArtnetIn instance;
        private ArtNetSocket socket;
        private List<ArtDmxListener> dmxListeners;
        private List<ArtPollListener> pollListeners;
        private List<IDmxPacketHandler>[,] dmxPacketHandlers; // subnet, universe
        public delegate void DmxSignalDeleage(bool hasSingal);
        public event DmxSignalDeleage DmxSignalChanged;
        public delegate void PortDmxSignalDelegate(HashSet<Port> activeProts);
        public event PortDmxSignalDelegate PortDmxSignalChanged;
        private object lockref = new object();
        private static readonly object padlock = new object();

        private static readonly string TAG = Talker.Talk.GetSource();
        private static readonly int PORT_SIZE = 16;

        private ArtnetIn()
        {
            dmxPacketHandlers = new List<IDmxPacketHandler>[PORT_SIZE, PORT_SIZE];

            InitSocket();
            InitPacketListeners();
            InitSignalTracker();
            
            var maxThreads = System.Environment.ProcessorCount / 2;
            maxThreads = maxThreads == 0 ? 1 : maxThreads;
            ThreadPool.SetMaxThreads(maxThreads, maxThreads);
        }

        public static ArtnetIn Instance {
            get {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ArtnetIn();
                        }
                    }
                }
                return instance;
            }
        }

        public ArtNetSocket Sotcket {
            get {
                return socket;
            }
        }

        private void InitSocket() {
            socket = new ArtNetSocket() { EnableBroadcast = true };
            socket.NewPacket += NewPacketHandler;
        }

        public void Start()
        {
            if (socket == null) {
                InitSocket();
            }
            socket.Open(
                IPAddress.Parse(SettingManager.Instance.Settings.ArtNetBindIp),
                IPAddress.Parse("255.255.255.0"));
        }

        public void Stop()
        {
            socket.Close();
            socket.Dispose();
            socket = null;
        }

       

        public void AddDmxPacketListener(IDmxPacketHandler handler)
        {
            lock (lockref)
            {
                var ports = handler.GetPortsRequired();
                // now add those prots to dmx handler array..

                foreach (var p in ports) {
                    // get handler list selected by port
                    var handlers = dmxPacketHandlers[p.SubNet, p.Universe];

                    // add handler to list
                    if(handlers == null)
                    {
                        handlers = new List<IDmxPacketHandler>();
                    }
                    var c = handlers.Select(h => h).ToList();
                    c.Add(handler);
                    dmxPacketHandlers[p.SubNet, p.Universe] = c;
                }
            }
        }

        public void AddDmxPacketListeners(List<IDmxPacketHandler> handlers)
        {
            foreach (IDmxPacketHandler h in handlers)
            {
                AddDmxPacketListener(h);
            }
        }

        public void ClearDmxPacketListeners()
        {
            lock (lockref)
            {
                foreach (var p in PortHelper.AllPorts) {
                    var handlers = dmxPacketHandlers[p.SubNet, p.Universe];
                    if (handlers != null) {
                        var c = handlers.Select(h => h).ToList();
                        c.Clear();
                        handlers = c;
                    }
                }
            }
        }

        public void ClearDmxSignalListeners()
        {
            DmxSignalChanged = null;
        }

        public void UpdateDmxPacketListeners(List<IDmxPacketHandler> handlers)
        {
            ClearDmxPacketListeners();
            AddDmxPacketListeners(handlers);
        }

        // packet dispatch workhorse.
        private void NewPacketHandler(object source, NewPacketEventArgs<ArtNetPacket> args)
        {
            ArtNetPacket packet = args.Packet;
            if (packet == null)
            {
                Talker.Talk.Log(new Talker.ActionMessage()
                {
                    Message = "Packet is null on receive.",
                    Source = TAG,
                    Level = Talker.LogLevel.WARNING
                });
                return;
            }
            switch (packet.OpCode)
            {
                case ArtNetOpCodes.Dmx:
                    {
                        dmxListeners.ForEach(l => l.Action(packet, args.Source.Address));
                        break;
                    }
                case ArtNetOpCodes.Poll:
                    {
                        pollListeners.ForEach(l => l.Action(packet, args.Source.Address));
                        break;
                    }

            }

        }


        private void InitPacketListeners()
        {
            dmxListeners = new List<ArtDmxListener>();
            dmxListeners.Add(new ArtDmxListener(this));

            pollListeners = new List<ArtPollListener>();
            pollListeners.Add(new ArtPollListener(this));
        }

        private void InitSignalTracker() {
            var signalTracker = new PortSignalTracker();
            signalTracker.OnDmxSignalChanged += OnSignalChange;
            signalTracker.OnPortSignalChanged += OnPortSignalChange;
            AddDmxPacketListener(signalTracker);
            signalTracker.Start();
        }


        public bool HasSignal { get; private set; } = false;

        public void OnSignalChange(bool detected)
        {
            HasSignal = detected;
            DmxSignalChanged?.Invoke(HasSignal);
        }

        public HashSet<Port> PortsWithDmxSignal { get; private set; } = new HashSet<Port>();

        public void OnPortSignalChange(HashSet<Port> activePorts)
        {
            PortsWithDmxSignal = activePorts;
            PortDmxSignalChanged?.Invoke(PortsWithDmxSignal);
        }

        // Art Net packet handlers down there
        public abstract class ArtnetListener
        {
            protected ArtnetIn artin;

            public ArtnetListener(ArtnetIn artin)
            {
                this.artin = artin;
            }

            public abstract void Action(ArtNetPacket p, IPAddress source);
        }

        private class ArtDmxListener : ArtnetListener
        {

            public ArtDmxListener(ArtnetIn artin) : base(artin)
            {
            }


            public override void Action(ArtNetPacket p, IPAddress source)
            {

                var packet = ((ArtNetDmxPacket)p);

                var packetPort = Port.From(packet);
                var dph = artin.dmxPacketHandlers[packetPort.SubNet, packetPort.Universe];
                dph?.ForEach(h => h.HandlePacket(packet));
            }
        }

        private class ArtPollListener : ArtnetListener
        {
            public ArtPollListener(ArtnetIn artin) : base(artin)
            {
            }

            public override void Action(ArtNetPacket p, IPAddress source)
            {

                ThreadPool.QueueUserWorkItem((i) =>
                {
                    var reply = new ArtPollReplyPacket()
                    {
                        ShortName = "mjau",
                        LongName = "vau vau vau"
                    };

                    artin.Sotcket.Send(reply);
                    
                });

            }
        }
    }
}
