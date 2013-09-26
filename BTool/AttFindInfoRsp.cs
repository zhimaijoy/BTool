﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttFindInfoRsp
    {
        public AttFindInfoRspDelegate AttFindInfoRspCallback;
        private AttrDataUtils attrDataUtils;
        private AttrUuidUtils attrUuidUtils;
        private DataUtils dataUtils = new DataUtils();
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        private const string moduleName = "AttFindInfoRsp";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
        private SendCmds sendCmds;

        public AttFindInfoRsp(DeviceForm deviceForm)
        {
            this.devForm = deviceForm;
            this.sendCmds = new SendCmds(deviceForm);
            this.attrUuidUtils = new AttrUuidUtils();
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_FindInfoRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp attFindInfoRsp = hciLeExtEvent.attFindInfoRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attFindInfoRsp != null)
                {
                    dataFound = true;
                    byte eventStatus = header.eventStatus;
                    if (eventStatus == 0)
                    {
                        if (attFindInfoRsp.handleData != null)
                        {
                            Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                            foreach (HCIReplies.HandleData data in attFindInfoRsp.handleData)
                            {
                                string attrKey = this.attrUuidUtils.GetAttrKey(attFindInfoRsp.attMsgHdr.connHandle, data.handle);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindInfoRsp"))
                                {
                                    flag = false;
                                    break;
                                }
                                dataAttr.key = attrKey;
                                dataAttr.connHandle = attFindInfoRsp.attMsgHdr.connHandle;
                                dataAttr.handle = data.handle;
                                dataAttr.uuid = this.devUtils.UnloadColonData(data.data, false);
                                dataAttr.uuidHex = this.dataUtils.GetStringFromBytes(data.data, true);
                                dataAttr.indentLevel = this.attrUuidUtils.GetIndentLevel(dataAttr.uuidHex);
                                dataAttr.uuidDesc = this.attrUuidUtils.GetUuidDesc(dataAttr.uuidHex);
                                dataAttr.valueDesc = this.attrUuidUtils.GetUuidValueDesc(dataAttr.uuidHex);
                                dataAttr.foreColor = this.attrUuidUtils.GetForegroundColor(dataAttr.uuidHex);
                                dataAttr.backColor = this.attrUuidUtils.GetBackgroundColor(dataAttr.uuidHex);
                                dataAttr.valueDsp = this.attrUuidUtils.GetValueDsp(dataAttr.uuidHex);
                                dataAttr.valueEdit = this.attrUuidUtils.GetValueEdit(dataAttr.uuidHex);
                                if (this.devForm.attrData.sendAutoCmds || (hciReplies.cmdType == TxDataOut.CmdType.DiscUuidAndValues))
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
                        flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttFindInfoRsp");
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
            if (this.AttFindInfoRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_FindInfoRsp = hciReplies.hciLeExtEvent.attFindInfoRsp;
                this.AttFindInfoRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp aTT_FindInfoRsp;
        }
    }
}
