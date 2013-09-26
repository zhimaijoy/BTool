﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Columns
    {
        public int keyWidth;
        public int connHandleWidth;
        public int handleWidth;
        public int uuidWidth;
        public int uuidDescWidth;
        public int valueWidth;
        public int valueDescWidth;
        public int propertiesWidth;
    }
}

