using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haukcode.ArtNet.Packets;

namespace DmxLedPanel
{
    public interface IDmxPacketHandler
    {
        void HandlePacket(ArtNetDmxPacket packet);

        List<int> GetPortHash();
    }
}
