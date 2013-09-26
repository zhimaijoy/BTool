﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    public class AttExecuteWriteRsp
    {
        public AttExecuteWriteRspDelegate AttExecuteWriteRspCallback;
        private const string moduleName = "AttExecuteWriteRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public bool GetATT_ExecuteWriteRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp attExecuteWriteRsp = hciLeExtEvent.attExecuteWriteRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if ((attExecuteWriteRsp != null) && (hciReplies.objTag != null))
                {
                    dataFound = true;
                    byte eventStatus = header.eventStatus;
                    switch (eventStatus)
                    {
                        case 0:
                        case 0x17:
                        {
                            ushort objTag = (ushort) hciReplies.objTag;
                            this.SendRspCallback(hciReplies, true);
                            goto Label_0080;
                        }
                    }
                    if (eventStatus == 0x1a)
                    {
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttExecuteWriteRsp");
                }
            }
        Label_0080:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttExecuteWriteRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_ExecuteWriteRsp = hciReplies.hciLeExtEvent.attExecuteWriteRsp;
                this.AttExecuteWriteRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp aTT_ExecuteWriteRsp;
        }
    }
}

