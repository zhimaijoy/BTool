﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ConnectInfo
    {
        public ushort handle;
        public byte addrType;
        public string bDA;
    }
}

