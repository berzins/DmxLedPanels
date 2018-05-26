using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Fixtures
{
    public interface IDmxUtlHandler
    {
        void HandleDmx(Fixture f, int Value);
    }
}
