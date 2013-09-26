﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RxTxMgrData
    {
        public RxDataIn rxDataIn;
        public TxDataOut txDataOut;
    }
}
