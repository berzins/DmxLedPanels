using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public interface ISerializable
    {

        string Serialize();

        T Deserialize<T>(string str);
    }
}
