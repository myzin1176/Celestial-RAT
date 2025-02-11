﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialDES.Helper.compression
{
    [Flags]
    public enum FrameFlags
    {
        UpdatedRegion = 1 << 0,
        MovedRegion = 1 << 1
    }
}

