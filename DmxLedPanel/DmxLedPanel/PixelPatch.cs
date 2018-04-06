using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public abstract class PixelPatch : IPixelPatch
    {

        public static readonly string PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_LEFT = "pixelPatchSnakeColumnWiseTopLeft";

        protected int columns, rows, address, pixelLength;
        
        protected Pixel[,] patch;

        public PixelPatch(string name) {
            Name = name;
        }

        public string Name { get; private set; }
        public int Columns { get { return columns;  } protected set { columns = value; } }
        public int Rows { get { return rows; } protected set { rows = value; } }
        public int Address { get { return address; } protected set { address = value; } }
        public int PixelLength { get { return pixelLength; } protected set { pixelLength = value; } }

        public abstract Pixel[,] GetPixelPatch();
        public abstract int[] GetPixelValues();
    }

}
