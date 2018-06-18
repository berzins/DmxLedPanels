using DmxLedPanel.Modes;
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

            var modes = new List<ModeTemplate>();
            foreach (IMode m in f.Modes) {
                modes.Add(new ModeTemplate(m));
            }

            return new FixtureTemplate() {
                Id = f.ID,
                Name = f.Name,
                Address = f.Address,
                PixelPatch = new PixelPatchTemplate(f.PixelPatch),
                Modes = modes,
                CurrentModeIndex = f.CurrentModeIndex,
                PatchedTo = f.PatchedTo,
                UtilAddress = f.UtilAddress,
                UtilsEnabled = f.IsDmxUtilsEnabled
            };
        }
    }
}
