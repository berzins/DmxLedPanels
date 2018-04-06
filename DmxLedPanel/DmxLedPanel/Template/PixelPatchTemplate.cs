using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Template
{
    public class PixelPatchTemplate
    {
        public PixelPatchTemplate() { }

        public PixelPatchTemplate(IPixelPatch pp) {
            Name = pp.Name;
            Columns = pp.Columns;
            Rows = pp.Rows;
            PixelLength = PixelLength;
        }

        public string Name { get; set; }
        public int Columns{ get; set; }
        public int Rows { get; set; }
        public int PixelLength { get; set; }

    }
}
