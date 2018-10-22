﻿using ArtNet;
using ArtNet.ArtPacket;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DmxLedPanel.ArtNetIO
{
    public class WhosInNetwork : ArtListener
    {

        private bool alive = true;
        private volatile Dictionary<int, ArtNetDevice> devices;
        private object lockref = new object();

        private volatile ArtPollReplyPacket lastRepy = null;

        public WhosInNetwork()
        {
            devices = new Dictionary<int, ArtNetDevice>();
            StartDeviceDisvorey();
        
            
        }

        public void Action(Packet p, IPAddress source)
        {
            var reply = (ArtPollReplyPacket)p;
            var dev = new ArtNetDevice(reply.LongName, reply.IP, DateTime.Now);
            if (devices.TryGetValue(dev.HashCode, out dev)) {
                dev.LastSeen = DateTime.Now;
                return;
            }

            dev = new ArtNetDevice(reply.LongName, reply.IP, DateTime.Now);
            devices.Add(dev.HashCode, dev);

            Logger.Log(dev.LastSeen.ToLocalTime() + ", Connected: " + dev.Name + " " + dev.Ip, LogLevel.INFO);
        }

        public void StopDeviceDiscovery()
        {
            alive = false;
        }

        public void StartDeviceDisvorey() {

            // polling thread
            new Thread(() =>
            {

                IPAddress binIp = IPAddress.Parse(
                    SettingManager.Instance.Settings.ArtNetPollReplyBindIp);
                var writter = new ArtNetWritter(
                    NetworkUtils.GetBroadcastAddress(binIp, NetworkUtils.GetSubnetMask(binIp)));

                while (alive)
                {
                    try
                    {
                        var poll = new ArtPollPacket();
                        writter.Write(poll);
                        Thread.Sleep(3000);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("Poll packt write failed. " + e.Message, LogLevel.ERROR);
                    }
                }

            }).Start();

            // Device monitoring thread
            new Thread(() =>
            {
                while (alive)
                {
                    lock (this.lockref)
                    {
                        for (int i = 0; i < devices.Count; i++) {
                            ArtNetDevice d = devices.ElementAt(i).Value;
                        if (DateTime.Now - d.LastSeen > TimeSpan.FromSeconds(3.0)) {
                            
                                devices.Remove(d.HashCode);
                            
                            Logger.Log(d.LastSeen.ToLocalTime() + ", Disconnected: " + d.Name + " " + d.Ip, LogLevel.INFO);
                        }
                        }
                    }  
                    Thread.Sleep(500);
                }
            }).Start();
        }

        public List<ArtNetDevice> Devices {
            get {
                List<ArtNetDevice> list =  new List<ArtNetDevice>();
                foreach(var d in devices) {
                    list.Add(d.Value);
                }
                return list;
            }
        }
    }
}
