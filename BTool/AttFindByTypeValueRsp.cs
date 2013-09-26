﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttFindByTypeValueRsp
    {
        public AttFindByTypeValueRspDelegate AttFindByTypeValueRspCallback;
        private AttrDataUtils attrDataUtils;
        private AttrUuidUtils attrUuidUtils = new AttrUuidUtils();
        private const string moduleName = "AttFindByTypeValueRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public AttFindByTypeValueRsp(DeviceForm deviceForm)
        {
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_FindByTypeValueRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp attFindByTypeValueRsp = hciLeExtEvent.attFindByTypeValueRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attFindByTypeValueRsp != null)
                {
                    dataFound = true;
                    byte eventStatus = header.eventStatus;
                    if (eventStatus == 0)
                    {
                        if (attFindByTypeValueRsp.handle != null)
                        {
                            Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                            foreach (ushort num in attFindByTypeValueRsp.handle)
                            {
                                string attrKey = this.attrUuidUtils.GetAttrKey(attFindByTypeValueRsp.attMsgHdr.connHandle, num);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindByTypeValueRsp"))
                                {
                                    flag = false;
                                    break;
                                }
                                dataAttr.key = attrKey;
                                dataAttr.connHandle = attFindByTypeValueRsp.attMsgHdr.connHandle;
                                dataAttr.handle = num;
                                if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (!this.attrDataUtils.UpdateAttrDict(tmpAttrDict))
                            {
                                flag = false;
                            }
                        }
                    }
                    else if ((eventStatus == 0x17) || (eventStatus == 0x1a))
                    {
                        this.SendRspCallback(hciReplies, true);
                    }
                    else
                    {
                        flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttFindByTypeValueRsp");
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
            if (this.AttFindByTypeValueRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_FindByTypeValueRsp = hciReplies.hciLeExtEvent.attFindByTypeValueRsp;
                this.AttFindByTypeValueRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp aTT_FindByTypeValueRsp;
        }
    }
}

