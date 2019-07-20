using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    class PixelPatchSnakeColumnWiseBottomLeft : RectaglePixelPatch
    {

        public PixelPatchSnakeColumnWiseBottomLeft(
            int cols,
            int rows,
            int address,
            int pixelLength
            ) : base(
                PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_BOTTOM_LEFT,
                cols,
                rows,
                address,
                pixelLength,
                new PixelOrderSnakeColumnWiseBottomLeft(cols, rows))
        {

            this.columns = cols;
            this.rows = rows;
            this.address = address;
            this.pixelLength = pixelLength;
            patch = patchPixels();

        }
    }
}
