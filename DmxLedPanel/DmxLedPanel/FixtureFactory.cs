using DmxLedPanel.Modes;
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
            return new Fixture(getMode(fixTemplate), getPatchType(fixTemplate)) {
                Name = fixTemplate.Name,
                Address = fixTemplate.Address
            };
        }

        private static IMode getMode(FixtureTemplate fixTemplate) {
            if (fixTemplate.Mode.Name.Equals(Mode.MODE_GRID_TOP_LEFT)) {
                return new ModeGridTopLeft(fixTemplate.Mode.Params[0], fixTemplate.Mode.Params[1]);
            }
            throw new KeyNotFoundException(
                "Fixture Factory failed to load a mode with key '" + 
                fixTemplate.Mode.Name + "'");
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
