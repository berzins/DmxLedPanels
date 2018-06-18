using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    public class PixelPatchSnakeRowWiseBottomRight : RectaglePixelPatch
    {
        public PixelPatchSnakeRowWiseBottomRight(
            int cols, 
            int rows, 
            int address, 
            int pixelLength) : base(
                PixelPatch.PIXEL_PATCH_SNAKE_ROWWISE_BOTTOM_RIGHT,
                cols,
                rows,
                address,
                pixelLength,
                new PixelOrderRowWiseBottomRight(cols,rows))
        {
        }
    }
}
