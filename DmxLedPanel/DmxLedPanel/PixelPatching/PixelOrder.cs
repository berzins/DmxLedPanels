using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.PixelPatching
{
    /// <summary>
    /// Iterator class to provide ordered access to pixels
    /// </summary>
    public abstract class PixelOrder
    {
        protected int columns, rows;
        
        public PixelOrder(int columns, int rows) {
            this.columns = columns;
            this.rows = rows;
        }

        /// <summary>
        /// Move cursor to start;
        /// </summary>
        public abstract void Reset();


        public abstract PixelPosition Next();
        
    }

    public class PixelOrderRowWiseTopLeft : PixelOrder
    {

        private List<PixelPosition> order;
        private int current = 0;        

        public PixelOrderRowWiseTopLeft(int columns, int rows) : base(columns, rows)
        {
            order = new List<PixelPosition>();
            init();
        }

        private void init() {
            // loop through rows
            for (int row = 0; row < rows; row++)
            {

                bool inverted = row % 2 > 0;

                // loop cols forward
                if (!inverted)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        order.Add(new PixelPosition(col, row));
                    }
                }

                // loop columns backward
                else
                {
                    for (int col = columns - 1; col >= 0; col--)
                    {
                        order.Add(new PixelPosition(col, row));
                    }
                }
            }
        }

        public override PixelPosition Next()
        {
            if (current >= order.Count) {
                return null;
            }
            return order.ElementAt(current++);
        }

        public override void Reset()
        {
            current = 0;
        }
    }
}
