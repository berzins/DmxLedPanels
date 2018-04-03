using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public interface IFixtureUpdateHandler
    {
        void OnUpdate(Fixture f);
    
    }
}
