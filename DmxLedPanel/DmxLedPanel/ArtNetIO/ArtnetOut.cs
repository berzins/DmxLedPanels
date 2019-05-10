using DmxLedPanel.Util;
using Haukcode.ArtNet.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.ArtNetIO
{
    public class ArtnetOut : IDisposable
    {
        ArtNetSocket socket;
        static ArtnetOut instance;
        static bool socketClosed = true;
        private static readonly object padlock = new object();

        private ArtnetOut() {
            InitSocket();
        }

        private void InitSocket() {
            socket = new ArtNetSocket { EnableBroadcast = true };
            socket.Open(
                IPAddress.Parse("192.168.0.104"),
                IPAddress.Parse("255.255.255.0"));
            socketClosed = false;
        }

        public static ArtnetOut Instance {
            get {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ArtnetOut();
                        }
                    }
                }
                return instance;
            }
        } 

        public ArtNetSocket Socket {
            get {
                if (socketClosed) {
                    InitSocket();
                }
                return socket;
            }
        }

        public void CloseSocket() {
            socket.Close();
            socket.Dispose();
            socketClosed = true;
        }
        

        public void Dispose()
        {
            CloseSocket();
        }
    }
}
