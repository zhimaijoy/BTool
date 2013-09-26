﻿namespace BTool
{
    using System;

    public class TxDataOut
    {
        public SendCmdResult callback;
        public string cmdName;
        public ushort cmdOpcode;
        public CmdType cmdType;
        public byte[] data;
        public object tag;
        public string time;

        public enum CmdType
        {
            General,
            DiscUuidOnly,
            DiscUuidAndValues
        }
    }
}

