﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttReadByTypeRsp
    {
        private AttrDataUtils attrDataUtils;
        public AttReadByTypeRspDelegate AttReadByTypeRspCallback;
        private AttrUuidUtils attrUuidUtils = new AttrUuidUtils();
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        private const string moduleName = "AttReadByTypeRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public AttReadByTypeRsp(DeviceForm deviceForm)
        {
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_ReadByTypeRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp attReadByTypeRsp = hciLeExtEvent.attReadByTypeRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attReadByTypeRsp != null)
                {
                    dataFound = true;
                    byte eventStatus = header.eventStatus;
                    if (eventStatus == 0)
                    {
                        if (attReadByTypeRsp.handleData != null)
                        {
                            Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                            foreach (HCIReplies.HandleData data in attReadByTypeRsp.handleData)
                            {
                                string attrKey = this.attrUuidUtils.GetAttrKey(attReadByTypeRsp.attMsgHdr.connHandle, data.handle);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadByTypeRsp"))
                                {
                                    flag = false;
                                    break;
                                }
                                dataAttr.key = attrKey;
                                dataAttr.connHandle = attReadByTypeRsp.attMsgHdr.connHandle;
                                dataAttr.handle = data.handle;
                                dataAttr.value = this.devUtils.UnloadColonData(data.data, false);
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
                        flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadByTypeRsp");
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
            if (this.AttReadByTypeRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_ReadByTypeRsp = hciReplies.hciLeExtEvent.attReadByTypeRsp;
                this.AttReadByTypeRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp aTT_ReadByTypeRsp;
        }
    }
}
