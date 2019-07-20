using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching.PixelPatchLinear
{
    public class PixelPatchLinearRowWiseTopLeft : RectaglePixelPatch
    {
        public PixelPatchLinearRowWiseTopLeft(
            int cols, 
            int rows, 
            int address, 
            int pixelLength) 
            
            : base(
                  PIXEL_PATCH_LINEAR_ROW_WISE_TOP_LEFT, 
                  cols, 
                  rows, 
                  address, 
                  pixelLength, 
                  new PixelOrderLinearRowWiseTopLeft(cols, rows))
        {
        }
    }
}
