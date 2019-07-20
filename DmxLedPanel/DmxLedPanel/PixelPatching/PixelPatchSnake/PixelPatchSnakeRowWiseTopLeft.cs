using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    public class PixelPatchSnakeRowWiseTopLeft : RectaglePixelPatch
    {
       
        public PixelPatchSnakeRowWiseTopLeft(
            int cols, 
            int rows, 
            int address, 
            int pixelLength
            ) : base(
                PixelPatch.PIXEL_PATCH_SNAKE_ROWWISE_TOP_LEFT,
                cols,
                rows,
                address,
                pixelLength,
                new PixelOrderSnakeRowWiseTopLeft(cols, rows)
                ) {

            this.columns = cols;
            this.rows = rows;
            this.address = address;
            this.pixelLength = pixelLength;
            patch = patchPixels();

        }
    }
}
