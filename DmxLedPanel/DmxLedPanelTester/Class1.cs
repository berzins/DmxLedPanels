using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanelTester
{
    public class Tester
    {
        public void TestModeSwitch(StateManager sm)  
        {
            var fixtures = sm.State.GetPatchedFixtures();
            foreach (var f in fixtures) {
             
            }

        }


    }
}
