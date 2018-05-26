using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Modes

{
    public interface IMode
    {
        string Name { get; }

        List<int> Params { get; }

        List<Field> GetFields(Pixel[,] patch);

        int Id { get; }

    }
}
