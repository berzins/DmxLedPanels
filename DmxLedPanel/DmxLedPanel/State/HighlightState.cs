using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.State
{
    public class HighlightState
    {
        private static HighlightState instance;

        public static HighlightState Instance {
            get {
                if (instance == null) {
                    instance = new HighlightState();
                }
                return instance;
            }
        }

        public bool Enabled { get; set; } = false;


    }
}
