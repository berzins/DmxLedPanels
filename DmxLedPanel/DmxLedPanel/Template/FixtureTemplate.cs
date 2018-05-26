using DmxLedPanel.Modes;
using DmxLedPanel.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public class FixtureTemplate
    {

        public FixtureTemplate() {


        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public PixelPatchTemplate PixelPatch { get; set; }
        public int PatchedTo { get; set; } = -1;
        public List<ModeTemplate> Modes { get; set; }
        public int CurrentModeIndex { get; set; }
    }
}
