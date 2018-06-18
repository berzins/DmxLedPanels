using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    public class PixelPatchSnakeColumnWiseTopRight : RectaglePixelPatch
    {
        public PixelPatchSnakeColumnWiseTopRight(
            int cols, 
            int rows, 
            int address, 
            int pixelLength
            ) : base(
                PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_RIGHT,
                cols,
                rows,
                address,
                pixelLength,
                new PixelOrderColumnWiseTopRight(cols, rows))
        {
        }
    }
}
