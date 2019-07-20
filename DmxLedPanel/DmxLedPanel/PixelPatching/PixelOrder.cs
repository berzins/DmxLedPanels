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

    public abstract class RectanglePixleOrder : PixelOrder {

        protected List<PixelPosition> order;
        protected int current = 0;

        public RectanglePixleOrder(int columns, int rows) : base(columns, rows)
        {
            order = new List<PixelPosition>();
            order = init();
        }

        protected abstract List<PixelPosition> init();
        
        public override PixelPosition Next()
        {
            if (current >= order.Count)
            {
                return null;
            }
            return order.ElementAt(current++);
        }

        public override void Reset()
        {
            current = 0;
        }
    }

    public class PixelOrderSnakeRowWiseTopLeft : RectanglePixleOrder
    {
        public PixelOrderSnakeRowWiseTopLeft(int columns, int rows) : base(columns, rows)
        {
        }

        protected override List<PixelPosition> init() {

            var order = new List<PixelPosition>();
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
            return order;
        }
    }

    public class PixelOrderSnakeColumnWiseBottomLeft : RectanglePixleOrder
    {
        public PixelOrderSnakeColumnWiseBottomLeft(int columns, int rows) : base(columns, rows)
        {
        }

        protected override List<PixelPosition> init()
        {
            var order = new List<PixelPosition>();

            for (int col = 0; col < columns; col++) {
                var inverted = col % 2 == 0;

                if (!inverted) {
                    for (int row = 0; row < rows; row++) {
                        order.Add(new PixelPosition(col, row));
                    }
                }
                else
                {
                    for (int row = rows - 1; row >= 0; row--) {
                        order.Add(new PixelPosition(col, row));
                    }
                }
            }

            return order;
        }
    }

    public class PixelOrderSnakeRowWiseBottomRight : RectanglePixleOrder
    {
        public PixelOrderSnakeRowWiseBottomRight(int columns, int rows) : base(columns, rows)
        {
        }

        protected override List<PixelPosition> init()
        {
            var order = new List<PixelPosition>();

            for (var row = rows - 1; row >= 0; row--) {
                var inverted = row % 2 == 0;
                if (!inverted)
                {
                    for (var col = 0; col < columns; col++)
                    {
                        order.Add(new PixelPosition(col, row));
                    }
                }
                else {
                    for (var col = columns - 1; col >= 0; col--) {
                        order.Add(new PixelPosition(col, row));
                    }
                }
            }

            return order;
        }
    }

    public class PixelOrderSnakeColumnWiseTopRight : RectanglePixleOrder
    {
        public PixelOrderSnakeColumnWiseTopRight(int columns, int rows) : base(columns, rows)
        {
        }

        protected override List<PixelPosition> init()
        {
            var order = new List<PixelPosition>();

            for (var col = columns - 1; col >= 0; col--) {
                var inverted = col % 2 != 0;
                if (!inverted)
                {
                    for (var row = 0; row < rows; row++)
                    {
                        order.Add(new PixelPosition(col, row));
                    }
                }
                else {
                    for (var row = rows - 1; row >= 0; row--) {
                        order.Add(new PixelPosition(col, row));
                    }
                }
            }

            return order;
        }
    }

    public class PixelOrderLinearRowWiseTopLeft : RectanglePixleOrder
    {
        public PixelOrderLinearRowWiseTopLeft(int columns, int rows) : base(columns, rows)
        {
        }

        protected override List<PixelPosition> init()
        {
            var order = new List<PixelPosition>();

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < columns; col++)
                {
                    order.Add(new PixelPosition(col, row));
                }
            }
            return order;
        }
    }

    public class PixelOrderLinearColumnWiseTopLeft : RectanglePixleOrder
    {

        public PixelOrderLinearColumnWiseTopLeft(int columns, int rows) : base(columns, rows)
        {
        }

        protected override List<PixelPosition> init()
        {
            var order = new List<PixelPosition>();
            for (var col = 0; col < columns; col++)
            {
                for (var row = 0; row < rows; row++)
                {
                    order.Add(new PixelPosition(col, row));
                }
            }
            return order;
        }
    }

}
