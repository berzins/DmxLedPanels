using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public interface IPixelPatch

    {

        string Name { get; }
        int Columns { get; }
        int Rows { get; }
        int Address { get; }
        int PixelLength { get; }

        Pixel[,] GetPixelPatch();

        int[] GetPixelValues();

    }
}
