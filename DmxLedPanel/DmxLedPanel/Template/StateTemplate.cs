using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Template
{
    public class StateTemplate
    {

        public StateTemplate() {
            Outputs = new List<OutputTemplate>();
            FixturePool = new List<FixtureTemplate>();
        }

        public List<OutputTemplate> Outputs { get; set; }
        public List<FixtureTemplate> FixturePool { get; set; }
    }
}
