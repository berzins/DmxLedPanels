using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Containers
{
    public class FixtureOutputMap
    {

        public FixtureOutputMap(int id, Output o, Fixture f = null)
        {
            FixtureId = id;
            Output = o;
            Fixture = f;
        }

        public int FixtureId { get; set; } // Keep this for backward compatability
        public Output Output { get; set; }
        public Fixture Fixture { get; set; }
    }
}
