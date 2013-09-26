﻿namespace BTool
{
    using System;

    public class RspHandlersUtils
    {
        private const string moduleName = "RspHandlersUtils";
        private MsgBox msgBox = new MsgBox();

        public bool CheckValidResponse(HCIReplies hciReplies)
        {
            bool flag = true;
            return (((hciReplies != null) && (hciReplies.hciLeExtEvent != null)) && flag);
        }

        public bool UnexpectedRspEventStatus(HCIReplies hciReplies, string moduleName)
        {
            return false;
        }
    }
}
