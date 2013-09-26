﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ComPortInfo
    {
        public string comPort;
        public string baudRate;
        public string flow;
        public string dataBits;
        public string parity;
        public string stopBits;
    }
}
