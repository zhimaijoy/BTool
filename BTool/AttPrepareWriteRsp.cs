﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    public class AttPrepareWriteRsp
    {
        public AttPrepareWriteRspDelegate AttPrepareWriteRspCallback;
        private const string moduleName = "AttPrepareWriteRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public bool GetATT_PrepareWriteRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp attPrepareWriteRsp = hciLeExtEvent.attPrepareWriteRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attPrepareWriteRsp != null)
                {
                    dataFound = true;
                    switch (header.eventStatus)
                    {
                        case 0:
                        case 0x1a:
                            goto Label_0068;

                        case 0x17:
                            this.SendRspCallback(hciReplies, true);
                            goto Label_0068;
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttPrepareWriteRsp");
                }
            }
        Label_0068:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttPrepareWriteRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_PrepareWriteRsp = hciReplies.hciLeExtEvent.attPrepareWriteRsp;
                this.AttPrepareWriteRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp aTT_PrepareWriteRsp;
        }
    }
}

