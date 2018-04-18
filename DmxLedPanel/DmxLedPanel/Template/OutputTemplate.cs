using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DmxLedPanel.Template
{
    public class OutputTemplate
    {

        public OutputTemplate() {
            Ports = new List<Port>();
            Fixtures = new List<FixtureTemplate>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<Port> Ports { get; set; }
        public List<FixtureTemplate> Fixtures { get; set; }
        public string IPAddress { get; set; }

    }
}
