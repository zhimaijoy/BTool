﻿using System;

public class SharedAppObjs
{
    public const string ProgramNameStr = "BTool";

    public enum MsgType
    {
        Incoming,
        Outgoing,
        Info,
        Warning,
        Error,
        RxDump,
        TxDump
    }

    public enum StringType
    {
        HEX,
        DEC,
        ASCII
    }
}
