using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching.PixelPatchLinear
{
    public class PixelPatchLinearColumnWiseTopLeft : RectaglePixelPatch
    {
        public PixelPatchLinearColumnWiseTopLeft( 
            int cols, 
            int rows, 
            int address, 
            int pixelLength) 
            
            : base(
                  PIXEL_PATCH_LINEAR_COLUMN_WISE_TOP_LEFT, 
                  cols, 
                  rows, 
                  address, 
                  pixelLength, 
                  new PixelOrderLinearColumnWiseTopLeft(cols, rows))
        {
        }
    }
}
