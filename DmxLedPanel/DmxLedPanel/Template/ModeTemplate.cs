using DmxLedPanel.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Template
{
    public class ModeTemplate
    {

        public ModeTemplate() {
            Params = new List<int>();
        }

        public ModeTemplate(IMode mode) :base() {
            Name = mode.Name;
            Params = mode.Params;
        }

        public string Name { get; set; }
        public List<int> Params {get; set;}
    }
}
