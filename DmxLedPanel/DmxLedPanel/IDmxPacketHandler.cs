using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtNet.ArtPacket;

namespace DmxLedPanel
{
    public interface IDmxPacketHandler
    {
        void HandlePacket(ArtDmxPacket packet);

        int GetPortHash();
    }
}
