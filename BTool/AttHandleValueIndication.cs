﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttHandleValueIndication
    {
        public AttHandleValueIndicationDelegate AttHandleValueIndicationCallback;
        private AttrDataUtils attrDataUtils;
        private AttrUuidUtils attrUuidUtils;
        private const string moduleName = "AttHandleValueIndication";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
        private SendCmds sendCmds;

        public AttHandleValueIndication(DeviceForm deviceForm)
        {
            this.sendCmds = new SendCmds(deviceForm);
            this.attrUuidUtils = new AttrUuidUtils();
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_HandleValueIndication(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication attHandleValueIndication = hciLeExtEvent.attHandleValueIndication;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attHandleValueIndication != null)
                {
                    dataFound = true;
                    switch (header.eventStatus)
                    {
                        case 0:
                            if (attHandleValueIndication.value != null)
                            {
                                HCICmds.ATTCmds.ATT_HandleValueConfirmation confirmation = new HCICmds.ATTCmds.ATT_HandleValueConfirmation();
                                confirmation.connHandle = attHandleValueIndication.attMsgHdr.connHandle;
                                this.sendCmds.SendATT(confirmation);
                                Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                                string attrKey = this.attrUuidUtils.GetAttrKey(attHandleValueIndication.attMsgHdr.connHandle, attHandleValueIndication.handle);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttHandleValueIndication"))
                                {
                                    flag = false;
                                }
                                else
                                {
                                    dataAttr.key = attrKey;
                                    dataAttr.connHandle = attHandleValueIndication.attMsgHdr.connHandle;
                                    dataAttr.handle = attHandleValueIndication.handle;
                                    dataAttr.value = attHandleValueIndication.value;
                                    if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
                                    {
                                        flag = false;
                                    }
                                    else if (!this.attrDataUtils.UpdateAttrDict(tmpAttrDict))
                                    {
                                        flag = false;
                                    }
                                    else
                                    {
                                        this.SendRspCallback(hciReplies, true);
                                    }
                                }
                            }
                            goto Label_0156;
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttHandleValueIndication");
                }
            }
        Label_0156:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttHandleValueIndicationCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_HandleValueIndication = hciReplies.hciLeExtEvent.attHandleValueIndication;
                this.AttHandleValueIndicationCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication aTT_HandleValueIndication;
        }
    }
}
