using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Template
{
    public static class FixtureTemplateFactory
    {
        public static FixtureTemplate createFixtureTemplate(Fixture f) {
            return new FixtureTemplate() {
                Id = f.ID,
                Name = f.Name,
                Address = f.Address,
                PixelPatch = new PixelPatchTemplate(f.PixelPatch),
                Mode = new ModeTemplate(f.getMode())
            };
        }
    }
}
