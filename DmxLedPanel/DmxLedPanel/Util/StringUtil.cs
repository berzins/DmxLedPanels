﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public static class StringUtil
    {
        public static bool IsEmpty(string str) {
            return str == null || str.Length == 0 ? true : false;
        }


    }
}
