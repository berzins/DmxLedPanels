using DmxLedPanel.Modes;
using DmxLedPanel.PixelPatching;
using DmxLedPanel.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    public static class FixtureFactory
    {
        public static Fixture createFixture(FixtureTemplate fixTemplate) {

            var modes = new List<IMode>();
            foreach (var mt in fixTemplate.Modes) {
                modes.Add(getMode(mt));
            }
            
            var fix = new Fixture(modes, getPatchType(fixTemplate)) {
                Name = fixTemplate.Name,
                Address = fixTemplate.Address,
            };

            fix.SwitchMode(fixTemplate.CurrentModeIndex);
            fix.UtilAddress = fixTemplate.UtilAddress;
            fix.IsDmxUtilsEnabled = fixTemplate.UtilsEnabled;

            return fix;
        }

        private static IMode getMode(ModeTemplate modeTemplate) {
            var mode =  Mode.InstantiateModeByName(modeTemplate.Name, modeTemplate.Params.ToArray());
            if (mode == null) {
                throw new KeyNotFoundException(
                    "Fixture Factory failed to load a mode with name '" + 
                    modeTemplate.Name + "'");
            }
            return mode;
        }

        private static IPixelPatch getPatchType(FixtureTemplate fixTemplate) {
            var pp =  RectaglePixelPatch.InstantiatePixelPatchByName(
                fixTemplate.PixelPatch.Name,
                fixTemplate.PixelPatch.Columns,
                fixTemplate.PixelPatch.Rows,
                0,
                fixTemplate.PixelPatch.PixelLength
                );
            if (pp == null) {
                throw new KeyNotFoundException(
                    "Fixture Factory failed to load a Pixel patch with name '" +
                    fixTemplate.PixelPatch.Name + "'");
            }
            return pp;
        }

        
    }
}
