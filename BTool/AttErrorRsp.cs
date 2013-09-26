﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    public class AttErrorRsp
    {
        public AttErrorRspDelegate AttErrorRspCallback;
        private const string moduleName = "AttErrorRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public bool GetATT_ErrorRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp attErrorRsp = hciLeExtEvent.attErrorRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attErrorRsp != null)
                {
                    dataFound = true;
                    byte eventStatus = header.eventStatus;
                    if (eventStatus != 0)
                    {
                        if (eventStatus == 0x1a)
                        {
                        }
                        flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttErrorRsp");
                    }
                    else
                    {
                        this.SendRspCallback(hciReplies, true);
                    }
                }
            }
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttErrorRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_ErrorRsp = hciReplies.hciLeExtEvent.attErrorRsp;
                this.AttErrorRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp aTT_ErrorRsp;
        }
    }
}

