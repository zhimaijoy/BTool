﻿namespace BTool
{
    using System;

    public class RxDataIn
    {
        public ushort cmdOpcode;
        public byte[] data;
        public ushort eventOpcode;
        public byte length;
        public string time;
        public byte type;
    }
}
