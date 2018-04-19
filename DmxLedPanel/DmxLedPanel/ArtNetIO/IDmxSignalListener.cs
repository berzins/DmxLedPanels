using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.ArtNetIO
{
    public interface IDmxSignalListener
    {
        void OnSignalChange(bool detected);

    }
}
