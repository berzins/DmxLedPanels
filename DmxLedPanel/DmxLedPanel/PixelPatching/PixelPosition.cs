using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    public class PixelPosition
    {
        public PixelPosition(int column, int row) {
            Column = column;
            Row = row;
        }

        public int Column { get; private set; }

        public int Row { get; private set; }

    }
}
