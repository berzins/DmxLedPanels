using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Modes
{
    public class ModeRectBorderOutIn : Mode
    {
        private int fieldCount;

        public ModeRectBorderOutIn(int fieldCount) : base(Mode.MODE_RECT_BORDER_OUT_IN)
        {
            this.fieldCount = fieldCount;
            this.Params.Add(fieldCount);
            this.Params.Add(fieldCount);
        }

        public override List<Field> GetFields(Pixel[,] patch)
        {
            var fields = new List<Field>();

            int cols = patch.GetLength(0);
            int rows = patch.GetLength(1);
            

            var colPixelZones = cols % 2 == 0 ? cols / 2 : (cols / 2) + 1;
            var rowPixelZones = rows % 2 == 0 ? rows / 2 : (rows / 2) + 1;

            var zones = colPixelZones < rowPixelZones ? colPixelZones : rowPixelZones;

            // slect zones
            for (var zone = 0; zone < zones; zone ++) {

                Field field = new Field();
                field.Index = zone;
                
                for (int col = zone; col < cols - zone; col++) {
                    for (int row = zone; row < rows - zone; row++) {
                        if (col == zone || row == zone) {
                            field.Pixels.Add(patch[col, row]);
                            field.Pixels.Add(patch[cols - col - 1, rows - row - 1]); // -1 : convert collon count to array index
                        }
                    }
                }
                fields.Add(field);
            }

            return fields;
        }
    }
}
