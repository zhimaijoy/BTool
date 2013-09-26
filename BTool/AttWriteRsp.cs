﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    public class AttWriteRsp
    {
        public AttWriteRspDelegate AttWriteRspCallback;
        private const string moduleName = "AttWriteRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public bool GetATT_WriteRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp attWriteRsp = hciLeExtEvent.attWriteRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if ((attWriteRsp != null) && (hciReplies.objTag != null))
                {
                    dataFound = true;
                    switch (header.eventStatus)
                    {
                        case 0:
                        {
                            ushort objTag = (ushort) hciReplies.objTag;
                            this.SendRspCallback(hciReplies, true);
                            goto Label_008A;
                        }
                        case 0x17:
                            this.SendRspCallback(hciReplies, true);
                            goto Label_008A;
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttWriteRsp");
                }
            }
        Label_008A:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttWriteRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_WriteRsp = hciReplies.hciLeExtEvent.attWriteRsp;
                this.AttWriteRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp aTT_WriteRsp;
        }
    }
}
