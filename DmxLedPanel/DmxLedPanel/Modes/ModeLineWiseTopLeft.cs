using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Modes
{
    public class ModeGridTopLeft : Mode
    {

        private int width, height;

        public ModeGridTopLeft(int width, int height) : base(MODE_GRID_TOP_LEFT) {
            Params.Add(width);
            Params.Add(height);
            this.width = width;
            this.height = height;
        }
        
        public int Width { get; set; }
        public int Height { get; set; }
        

        /*
         *  Return list of fields indexed from 0.
         *  Fields grab containg Pixels from patch. 
        */
        public override List<Field> GetFields(Pixel[,] patch)
        {
            int pixWidth = patch.GetLength(0) / width;
            int pixHeight = patch.GetLength(1) / height;

            List<Field> fields = new List<Field>();
            int fieldIndex = 0;

            // select Field from matrix
            for (int fHeigth = 0; fHeigth < height; fHeigth++)
            {
                for (int fWidth = 0; fWidth < width; fWidth++)
                {

                    var field = new Field();
                    field.Index = fieldIndex++;

                    //Select Pixels from patch;
                    int i = pixHeight * fHeigth;
                    for (int pHeight = i; pHeight < i + pixHeight; pHeight++)
                    {
                        int e = pixWidth * fWidth;
                        for (int pWidth = e; pWidth < e + pixWidth; pWidth++)
                        {
                            field.Pixels.Add(patch[pWidth, pHeight]);
                        }
                    }

                    fields.Add(field);
                }
            }

            return fields;
        }
    }
}
