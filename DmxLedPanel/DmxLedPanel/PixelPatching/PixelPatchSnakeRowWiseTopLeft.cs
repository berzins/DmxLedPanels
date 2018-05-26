using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    public class PixelPatchSnakeRowWiseTopLeft : PixelPatch
    {
       
        public PixelPatchSnakeRowWiseTopLeft(
            int cols, 
            int rows, 
            int address, 
            int pixelLength
            ) : base(PixelPatch.PIXEL_PATCH_SNAKE_ROWWISE_TOP_LEFT) {

            this.columns = cols;
            this.rows = rows;
            this.address = address;
            this.pixelLength = pixelLength;
            this.sortedPixelPositions = new PixelOrderRowWiseTopLeft(columns, rows);
            patch = patchPixels();

        }

        public PixelPatchSnakeRowWiseTopLeft(PixelPatch pp, Pixel[,] patch) :
             base(pp.Name, pp.Columns, pp.Rows, pp.Address, pp.PixelLength, patch) { }


        public override IPixelPatch Clone()
        {
            return (IPixelPatch)(new PixelPatchSnakeRowWiseTopLeft(this, this.patch));
        }

        public override Pixel[,] GetPixelPatch()
        {
            return patch;
        }

        public override int[] GetPixelValues()
        {
            return getPixelValues();    
        }

        private Pixel[,] patchPixels() {
            Pixel[,] order = new Pixel[columns, rows];
            int pixIndex = 0;

            sortedPixelPositions.Reset();
            PixelPosition pixPos = null;
            while ((pixPos = sortedPixelPositions.Next()) != null) {
                initOrderPixel(ref order[pixPos.Column, pixPos.Row], ref address, pixelLength, ref pixIndex);
            }
            
            return order;
        }
    }
}
