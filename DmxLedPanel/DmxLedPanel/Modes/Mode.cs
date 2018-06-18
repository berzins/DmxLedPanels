﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Modes
{
    public abstract class Mode : IMode
    {

        private static int id = 0;

        public static readonly string MODE_GRID_TOP_LEFT = "GridTopLeft";
        public static readonly string MODE_RECT_BORDER_OUT_IN = "RectBorderOutIn";

        protected Mode(string name) {
            Id = id++;
            Params = new List<int>();
            Name = name;
        }

        public static void ResetIdCounter() {
            id = 0;
        }

        public int Id { get; private set; }
        
        public List<int> Params { get; }

        public string Name { get; protected set; }

        public abstract List<Field> GetFields(Pixel[,] patch);


        public static IMode InstantiateModeByName(string name, int[] dim) {
            if (name.Equals(Mode.MODE_GRID_TOP_LEFT))
            {
                return new ModeGridTopLeft(dim[0], dim[1]);
            }
            if (name.Equals(Mode.MODE_RECT_BORDER_OUT_IN)) {
                return new ModeRectBorderOutIn(dim[0]);
            }
            return null;
        }
    }
}
