using DmxLedPanel.Modes;
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

            return fix;
        }

        private static IMode getMode(ModeTemplate modeTemplate) {
            if (modeTemplate.Name.Equals(Mode.MODE_GRID_TOP_LEFT)) {
                return new ModeGridTopLeft(modeTemplate.Params[0], modeTemplate.Params[1]);
            }
            throw new KeyNotFoundException(
                "Fixture Factory failed to load a mode with key '" + 
                modeTemplate.Name + "'");
        }

        private static IPixelPatch getPatchType(FixtureTemplate fixTemplate) {
            if (fixTemplate.PixelPatch.Name.Equals(PixelPatch.PIXEL_PATCH_SNAKE_COLUMNWISE_TOP_LEFT)) {
                return new PixelPatchSnakeColumnWiseTopLeft(
                   fixTemplate.PixelPatch.Columns,
                   fixTemplate.PixelPatch.Rows,
                   0,
                   fixTemplate.PixelPatch.PixelLength
                    );
            }

            throw new KeyNotFoundException(
                "Fixture Factory failed to load a pixel patch with key '" + 
                fixTemplate.PixelPatch.Name + "'");
        }
    }
}
