using DmxLedPanel.PixelPatching.PixelPatchLinear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    public class RectaglePixelPatch : PixelPatch
    {
        public RectaglePixelPatch(
            string name,
            int cols,
            int rows,
            int address,
            int pixelLength,
            PixelOrder pixelOrder
            ) 
            : base(name)
        {

            this.columns = cols;
            this.rows = rows;
            this.address = address;
            this.pixelLength = pixelLength;
            this.sortedPixelPositions = pixelOrder;
            patch = patchPixels();

        }

        public override Pixel[,] GetPixelPatch()
        {
            return patch;
        }

        public override int[] GetPixelValues()
        {
            return getPixelValues();
        }

        protected Pixel[,] patchPixels()
        {
            Pixel[,] order = new Pixel[columns, rows];
            int pixIndex = 0;

            sortedPixelPositions.Reset();
            PixelPosition pixPos = null;
            while ((pixPos = sortedPixelPositions.Next()) != null)
            {
                initOrderPixel(ref order[pixPos.Column, pixPos.Row], ref address, pixelLength, ref pixIndex);
            }

            return order;
        }

        public static IPixelPatch InstantiatePixelPatchByName(string name, int cols, int rows, int addr, int pixLength)
        {
            if (name.Equals(PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_LEFT))
            {
                return new PixelPatchSnakeColumnWiseTopLeft(cols, rows, addr, pixLength);
            }
            if (name.Equals(PixelPatch.PIXEL_PATCH_SNAKE_ROWWISE_TOP_LEFT))
            {
                return new PixelPatchSnakeRowWiseTopLeft(cols, rows, addr, pixLength);
            }
            if (name.Equals(PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_BOTTOM_LEFT))
            {
                return new PixelPatchSnakeColumnWiseBottomLeft(cols, rows, addr, pixLength);
            }
            if (name.Equals(PixelPatch.PIXEL_PATCH_SNAKE_ROWWISE_BOTTOM_RIGHT))
            {
                return new PixelPatchSnakeRowWiseBottomRight(cols, rows, addr, pixLength);
            }
            if (name.Equals(PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_RIGHT))
            {
                return new PixelPatchSnakeColumnWiseTopRight(cols, rows, addr, pixLength);
            }
            if (name.Equals(PIXEL_PATCH_LINEAR_ROW_WISE_TOP_LEFT))
            {
                return new PixelPatchLinearRowWiseTopLeft(cols, rows, addr, pixLength);
            }
            if (name.Equals(PIXEL_PATCH_LINEAR_COLUMN_WISE_TOP_LEFT))
            {
                return new PixelPatchLinearColumnWiseTopLeft(cols, rows, addr, pixLength);
            }

            return null;
        }
    }
}
