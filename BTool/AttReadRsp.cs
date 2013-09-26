﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttReadRsp
    {
        private AttrDataUtils attrDataUtils;
        public AttReadRspDelegate AttReadRspCallback;
        private AttrUuidUtils attrUuidUtils = new AttrUuidUtils();
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        private const string moduleName = "AttReadRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public AttReadRsp(DeviceForm deviceForm)
        {
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_ReadRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp attReadRsp = hciLeExtEvent.attReadRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attReadRsp != null)
                {
                    dataFound = true;
                    switch (header.eventStatus)
                    {
                        case 0:
                            if ((attReadRsp.data != null) && (hciReplies.objTag != null))
                            {
                                ushort objTag = (ushort) hciReplies.objTag;
                                Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                                string attrKey = this.attrUuidUtils.GetAttrKey(attReadRsp.attMsgHdr.connHandle, objTag);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadRsp"))
                                {
                                    flag = false;
                                }
                                else
                                {
                                    dataAttr.key = attrKey;
                                    dataAttr.connHandle = attReadRsp.attMsgHdr.connHandle;
                                    dataAttr.handle = objTag;
                                    dataAttr.value = this.devUtils.UnloadColonData(attReadRsp.data, false);
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
                            goto Label_0158;

                        case 0x17:
                        case 0x1a:
                            this.SendRspCallback(hciReplies, true);
                            goto Label_0158;
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadRsp");
                }
            }
        Label_0158:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttReadRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_ReadRsp = hciReplies.hciLeExtEvent.attReadRsp;
                this.AttReadRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp aTT_ReadRsp;
        }
    }
}
