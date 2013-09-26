﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    public class ExtCmdStatus
    {
        public ExtCmdStatusDelegate ExtCmdStatusCallback;
        private const string moduleName = "ExtCmdStatus";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public bool GetExtensionCommandStatus(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus gapHciCmdStat = hciLeExtEvent.gapHciCmdStat;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (gapHciCmdStat != null)
                {
                    dataFound = true;
                    if (header.eventStatus == 0)
                    {
                        dataFound = true;
                        flag = true;
                    }
                    else
                    {
                        switch (gapHciCmdStat.cmdOpCode)
                        {
                            case 0xfd90:
                            case 0xfd92:
                            case 0xfd96:
                            case 0xfdb2:
                            case 0xfd01:
                            case 0xfd04:
                            case 0xfd05:
                            case 0xfd06:
                            case 0xfd07:
                            case 0xfd08:
                            case 0xfd09:
                            case 0xfd0a:
                            case 0xfd0b:
                            case 0xfd0c:
                            case 0xfd0d:
                            case 0xfd10:
                            case 0xfd11:
                            case 0xfd12:
                            case 0xfd13:
                            case 0xfd16:
                            case 0xfd17:
                            case 0xfd18:
                            case 0xfd19:
                            case 0xfd84:
                            case 0xfd86:
                            case 0xfd88:
                            case 0xfd8a:
                            case 0xfd8c:
                                this.SendRspCallback(hciReplies, true);
                                goto Label_0148;
                        }
                        flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "ExtCmdStatus");
                    }
                }
            }
        Label_0148:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.ExtCmdStatusCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.gapHciCmdStat = hciReplies.hciLeExtEvent.gapHciCmdStat;
                this.ExtCmdStatusCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus gapHciCmdStat;
        }
    }
}

