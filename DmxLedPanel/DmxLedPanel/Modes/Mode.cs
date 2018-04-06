using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Modes
{
    public abstract class Mode : IMode
    {

        public static readonly string MODE_GRID_TOP_LEFT = "GridTopLeft";

        protected Mode(string name) {
            Params = new List<int>();
            Name = name;
        }
        
        public List<int> Params { get; }

        public string Name { get; protected set; }

        public abstract List<Field> GetFields(Pixel[,] patch);
    }
}
