using ArtNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;
using System.Net;
using DmxLedPanel.State;

namespace DmxLedPanel
{
    public class ArtnetIn
    {
        private static ArtnetIn instance;
        private ArtNetReader reader;
        private ArtDispatcher dispatcher;
        private List<IDmxPacketHandler> dmxPacketHandlers;
        private bool paused = false;

        private ArtnetIn() {
            dispatcher = initDispatcher();
            reader = new ArtNetReader(dispatcher, IPAddress.Parse("192.168.0.100"));
            dmxPacketHandlers = new List<IDmxPacketHandler>();
        }

        public static ArtnetIn Instance {
            get {
                if (instance == null) {
                    instance = new ArtnetIn();
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
            this.dmxPacketHandlers.Add(handler);
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
            dmxPacketHandlers.Clear();
            return r;
        }

        public void UpdateDmxPacketListeners(List<IDmxPacketHandler> handlers) {
            ClearDmxPacketListeners();
            AddDmxPacketListeners(handlers);
        }


        private ArtDispatcher initDispatcher() {
            ArtDispatcher d = new ArtDispatcher();
            d.AddArtDmxListener(new ArtDmxListener(this));
            d.AddArtPollListener(new ArtPollListener(this));
            return d;
        }


        // Art Net packet handlers down there

        private abstract class ArtnetListener : ArtListener{
            protected ArtnetIn artin;

            public ArtnetListener(ArtnetIn artin)
            {
                this.artin = artin;
            }

            public abstract void Action(Packet p);
        }

        private class ArtDmxListener : ArtnetListener
        {
            public ArtDmxListener(ArtnetIn artin) : base(artin)
            {
            }

            public override void Action(Packet p)
            {
                foreach(IDmxPacketHandler h in artin.dmxPacketHandlers)
                {
                    h.HandlePacket((ArtDmxPacket)p);
                }
            }
        }

        private class ArtPollListener : ArtnetListener
        {
            public ArtPollListener(ArtnetIn artin) : base(artin)
            {
            }

            public override void Action(Packet p)
            {
                var reply = new ArtPollReplyPacket();
                reply.ShortName = "LED PANELS";
                ArtnetOut.Instance.Writer.Write(reply);
                Console.WriteLine("Art Poll recieved");   
            }
        }
    }
}
