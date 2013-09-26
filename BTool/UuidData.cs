﻿namespace BTool
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UuidData
    {
        public string uuid;
        public byte indentLevel;
        public string uuidDesc;
        public string valueDesc;
        public string dataSetName;
        public Color foreColor;
        public Color backColor;
        public ValueDisplay valueDsp;
        public ValueEdit valueEdit;
    }
}

