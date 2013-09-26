﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttReadByGrpTypeRsp
    {
        private AttrDataUtils attrDataUtils;
        public AttReadByGrpTypeRspDelegate AttReadByGrpTypeRspCallback;
        private AttrUuidUtils attrUuidUtils;
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        private const string moduleName = "AttReadByGrpTypeRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
        private SendCmds sendCmds;

        public AttReadByGrpTypeRsp(DeviceForm deviceForm)
        {
            this.devForm = deviceForm;
            this.attrUuidUtils = new AttrUuidUtils();
            this.attrDataUtils = new AttrDataUtils(deviceForm);
            this.sendCmds = new SendCmds(deviceForm);
        }

        public bool GetATT_ReadByGrpTypeRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp attReadByGrpTypeRsp = hciLeExtEvent.attReadByGrpTypeRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attReadByGrpTypeRsp != null)
                {
                    dataFound = true;
                    byte eventStatus = header.eventStatus;
                    if (eventStatus == 0)
                    {
                        if (attReadByGrpTypeRsp.handleHandleData != null)
                        {
                            Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                            foreach (HCIReplies.HandleHandleData data in attReadByGrpTypeRsp.handleHandleData)
                            {
                                string attrKey = this.attrUuidUtils.GetAttrKey(attReadByGrpTypeRsp.attMsgHdr.connHandle, data.handle1);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadByGrpTypeRsp"))
                                {
                                    flag = false;
                                    break;
                                }
                                dataAttr.key = attrKey;
                                dataAttr.connHandle = attReadByGrpTypeRsp.attMsgHdr.connHandle;
                                dataAttr.handle = data.handle1;
                                dataAttr.value = this.devUtils.UnloadColonData(data.data, false);
                                if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
                                {
                                    flag = false;
                                    break;
                                }
                                if (data.handle2 == 0xffff)
                                {
                                    break;
                                }
                                int num = data.handle2 - data.handle1;
                                if (num <= 0)
                                {
                                    flag = false;
                                    break;
                                }
                                for (int i = data.handle1 + 1; i <= data.handle2; i++)
                                {
                                    attrKey = this.attrUuidUtils.GetAttrKey(attReadByGrpTypeRsp.attMsgHdr.connHandle, (ushort) i);
                                    dataAttr = new DataAttr();
                                    dataChanged = false;
                                    if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadByGrpTypeRsp"))
                                    {
                                        flag = false;
                                        break;
                                    }
                                    dataAttr.key = attrKey;
                                    dataAttr.connHandle = attReadByGrpTypeRsp.attMsgHdr.connHandle;
                                    dataAttr.handle = (ushort) i;
                                    if (this.devForm.attrData.sendAutoCmds)
                                    {
                                        HCICmds.GATTCmds.GATT_ReadLongCharValue value2 = new HCICmds.GATTCmds.GATT_ReadLongCharValue();
                                        value2.connHandle = dataAttr.connHandle;
                                        value2.handle = dataAttr.handle;
                                        this.sendCmds.SendGATT(value2, TxDataOut.CmdType.DiscUuidAndValues, null);
                                    }
                                    if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
                                    {
                                        flag = false;
                                        break;
                                    }
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
                        flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadByGrpTypeRsp");
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
            if (this.AttReadByGrpTypeRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_ReadByGrpTypeRsp = hciReplies.hciLeExtEvent.attReadByGrpTypeRsp;
                this.AttReadByGrpTypeRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp aTT_ReadByGrpTypeRsp;
        }
    }
}

