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
            PixelLength = Const.PIXEL_LENGTH;
        }


        // Copy constructor
        protected PixelPatch(string name, int cols, int rows, int addr, int pixLength, Pixel[,] patch) {
            Name = name;
            this.columns = cols;
            this.rows = rows;
            this.address = addr;
            this.pixelLength = pixLength;
            this.patch = new Pixel[patch.GetLength(0), patch.GetLength(1)];
            for (int i = 0; i < patch.GetLength(0); i++) {
                for (int e = 0; e < patch.GetLongLength(1); e++) {
                    this.patch[i, e] = patch[i, e].Clone();
                }
            }

        }

        public string Name { get; private set; }
        public int Columns { get { return columns;  } protected set { columns = value; } }
        public int Rows { get { return rows; } protected set { rows = value; } }
        public int Address { get { return address; } protected set { address = value; } }
        public int PixelLength { get { return pixelLength; } protected set { pixelLength = value; } }

        public abstract IPixelPatch Clone();

        public abstract Pixel[,] GetPixelPatch();
        public abstract int[] GetPixelValues();
    }

}
