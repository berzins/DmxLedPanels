using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public interface IPixelPatch
    {
        Pixel[,] GetPixelPatch();

        int[] GetPixelValues();

    }
}
