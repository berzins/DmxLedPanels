using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Fixtures
{
    public class DmxModeSwitcher : FixtureDmxUtil
    {
        public static readonly int ADDRESS_COUNT = 1;
        public static readonly string UTIL_NAME = "Pixel Mode Switcher";

        public DmxModeSwitcher(int index) : base(index, ADDRESS_COUNT) {
            Name = UTIL_NAME;
        }

        public DmxModeSwitcher(int index, string name, int[] values) : base(index, name, values) { }


        public override FixtureDmxUtil Clone()
        {
            return new DmxModeSwitcher(Index, Name, values);
        }

        public override void HandleDmx(Fixture f, int value)
        {
            int switchInterval = 255 / f.ModeCount;
            int index = value / switchInterval;
            f.SwitchMode(index);
        }
    }
}
