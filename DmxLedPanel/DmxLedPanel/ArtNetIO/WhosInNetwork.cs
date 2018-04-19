using ArtNet;
using ArtNet.ArtPacket;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DmxLedPanel.ArtNetIO.ArtnetIn;

namespace DmxLedPanel.ArtNetIO
{
    public class WhosInNetwork : ArtnetListener
    {

        private bool alive = true;
        private volatile List<ArtNetDevice> devices;
        private object lockref = new object();

        public WhosInNetwork(ArtnetIn artin) : base(artin)
        {
            devices = new List<ArtNetDevice>();

            new Thread(() => {

                var writter = new ArtNetWritter(
                    IPAddress.Parse(
                        SettingManager.Instance.Settings.ArtNetBroadcastIp
                        ));

                while (alive)
                {
                    try
                    {
                        var poll = new ArtPollPacket();
                        writter.Write(poll);
                        Thread.Sleep(2000);
                    }
                    catch (Exception e) {
                        Logger.Log("Poll packt write failed. " + e.Message, LogLevel.ERROR);
                    }
                }

            }).Start();
        }

        public override void Action(Packet p)
        {
            var reply = (ArtPollReplyPacket)p;
            ArtNetDevice device;

            if (tryGetDevice(reply.LongName, out device))
            {
                if (DateTime.Now - device.LastSeen > TimeSpan.FromSeconds(3))
                {
                    lock (this.lockref)
                    {
                        var d = new List<ArtNetDevice>(devices);
                        d.Remove(device);
                        devices = d;
                    }
                    Logger.Log(device.LastSeen.ToLocalTime() + ", Disconnected: " + device.Name + " " + device.Ip, LogLevel.INFO);
                    return;
                }
                device.LastSeen = DateTime.Now;
                return;
            }
            var dev = new ArtNetDevice(reply.LongName, reply.IP, DateTime.Now);
            lock (this.lockref)
            {
                var d = new List<ArtNetDevice>(devices);
                d.Add(dev);
                devices = d;
            }
            Logger.Log(dev.LastSeen.ToLocalTime() + ", Connected: " + dev.Name + " " + dev.Ip, LogLevel.INFO);
        }

        public List<ArtNetDevice> Devices {
            get {
                return devices;
            }
        }

        private bool tryGetDevice(string name, out ArtNetDevice device)
        {
            foreach (var d in devices)
            {
                if (d.Name.Equals(name))
                {
                    device = d;
                    return true;
                }
            }
            device = null;
            return false;
        }
    }
}
