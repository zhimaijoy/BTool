﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct WriteLimits
    {
        public int maxPacketSize;
        public int maxNumPreparedWrites;
    }
}

