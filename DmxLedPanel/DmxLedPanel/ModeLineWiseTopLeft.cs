using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class ModeLineWiseTopLeft : IMode
    {
        private int width, height;

        public ModeLineWiseTopLeft(int width, int height) {
            this.width = width;
            this.height = height;
        }

        /*
         *  Return list of fields indexed from 0.
         *  Fields grab containg Pixels from patch. 
        */
        public List<Field> GetFields(Pixel[,] patch)
        {
            int pixWidth = patch.GetLength(0) / width;
            int pixHeight = patch.GetLength(0) / height;

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
