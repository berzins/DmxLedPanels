﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class PixelPatchSnakeColumnWiseTopLeft : PixelPatch
    {

        public PixelPatchSnakeColumnWiseTopLeft(
            int columns, 
            int rows,
            int address, 
            int pixelLength) : base(PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_LEFT) {

            this.columns = columns;
            this.rows = rows;
            this.address = address;
            this.pixelLength = pixelLength;
           
            patch = patchPixels();
        }

        // terrible clone constructor
        public PixelPatchSnakeColumnWiseTopLeft(PixelPatch pp, Pixel[,] patch) :
             base(pp.Name, pp.Columns, pp.Rows, pp.Address, pp.PixelLength, patch) {}

        public override Pixel[,] GetPixelPatch() {
            return patch;
        }

        public override int[] GetPixelValues()
        {   
            List<int> values = new List<int>();

            //loop through columns
            for (int col = 0; col < columns; col++)
            {
                bool invert = col % 2 > 0;

                //loop rows forward
                if (!invert)
                {
                    for (int row = 0; row < rows; row++)
                    {
                        addPixelValuesToList(values, col, row);            
                    }
                }
                //loop rows backwards
                else
                {
                    for (int row = rows - 1; row >= 0; row--)
                    {
                        addPixelValuesToList(values, col, row);
                    }
                }
            }
            return values.ToArray();
        }

        private void addPixelValuesToList(List<int> values, int col, int row) {
            foreach (int val in patch[col, row].DmxValues)
            {
                values.Add(val);
            }
        }



        private Pixel[,] patchPixels() {
            Pixel[,] order = new Pixel[columns, rows];
            int pixIndex = 0;

            //loop through columns
            for (int col = 0; col < columns; col++)
            {
                bool invert = col % 2 > 0;

                //loop rows forward
                if (!invert)
                {
                    for (int row = 0; row < rows; row++)
                    {
                        order[col, row] = initPixel(pixIndex, address, pixelLength);
                        updateIndexes(ref address, pixelLength, ref pixIndex);
                    }
                }
                //loop rows backwards
                else
                {
                    for (int row = rows - 1; row >= 0; row--)
                    {
                        order[col, row] = initPixel(pixIndex, address, pixelLength);
                        updateIndexes(ref address, pixelLength, ref pixIndex);
                    }
                }
            }
            return order;
        }

        private static void updateIndexes(ref int address, int pixelLength, ref int pixIndex)
        {
            address += pixelLength;
            pixIndex++;
        }

        private static Pixel initPixel(int index, int address, int pixelLength)
        {
            var pix = new Pixel();
            pix.AddressCount = pixelLength;
            for (int i = 0; i < pix.DmxAddresses.Length; i++)
            {
                pix.Index = index;
                pix.DmxAddresses[i] = address++;
            }
            return pix;
        }

        public override IPixelPatch Clone()
        {
            return (IPixelPatch)(new PixelPatchSnakeColumnWiseTopLeft(this, this.patch));
        }
    }
}
