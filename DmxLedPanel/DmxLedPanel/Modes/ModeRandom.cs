using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Modes
{
    public class ModeRandom : Mode
    {
        private int fieldCount;

        public ModeRandom(int fieldCount) : base(Mode.MODE_RANDOM)
        {
            this.fieldCount = fieldCount;
            this.Params.Add(fieldCount);
            this.Params.Add(fieldCount);
        }

        public override List<Field> GetFields(Pixel[,] patch)
        {
            int cols = patch.GetLength(0);
            int rows = patch.GetLength(1);
            int pixTotal = cols * rows;
            int pixsInField = pixTotal / this.fieldCount;
            List<Pixel> pixels = getPatchAsPixelList(patch, cols, rows);

            List<Field> fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++) {
                Field field = new Field();
                field.Index = i;
                List<Pixel> randPixels = SelectRandomPixels(pixels, pixsInField);
                field.Pixels.AddRange(randPixels);
                fields.Add(field);
            }
            return fields;
        }

        private List<Pixel> getPatchAsPixelList(Pixel[,] patch, int cols, int rows)
        {
            List<Pixel> pixels = new List<Pixel>();
            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    pixels.Add(patch[col, row]);
                }
            }
            return pixels;
        }

        private List<Pixel> SelectRandomPixels(List<Pixel> pixels, int count)
        {
            Random rand = new Random();
            List<Pixel> pixelsRand = new List<Pixel>();
            count = count * 2 > pixels.Count ? pixels.Count : count;
            for (int i = 0; i < count; i++) {
                int randIndex = rand.Next(0, pixels.Count - 1);
                Pixel pix = pixels.ElementAt(randIndex);
                pixelsRand.Add(pix);
                pixels.Remove(pix);
            }
            return pixelsRand;
        }
    }
}
