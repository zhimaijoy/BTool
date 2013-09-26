﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttReadBlobRsp
    {
        private AttrDataUtils attrDataUtils;
        public AttReadBlobRspDelegate AttReadBlobRspCallback;
        private AttrUuidUtils attrUuidUtils = new AttrUuidUtils();
        private DataUtils dataUtils = new DataUtils();
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        private const string moduleName = "AttReadBlobRsp";
        private byte[] readBlobData;
        private ushort readBlobHandle;
        private bool readBlobHandleValid;
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public AttReadBlobRsp(DeviceForm deviceForm)
        {
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_ReadBlobRsp(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp attReadBlobRsp = hciLeExtEvent.attReadBlobRsp;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attReadBlobRsp != null)
                {
                    dataFound = true;
                    switch (header.eventStatus)
                    {
                        case 0:
                            if (attReadBlobRsp.data != null)
                            {
                                int length = attReadBlobRsp.data.Length;
                                byte[] data = attReadBlobRsp.data;
                                if (length > 0)
                                {
                                    if (this.readBlobData == null)
                                    {
                                        this.readBlobData = new byte[length];
                                        this.readBlobData = data;
                                    }
                                    else
                                    {
                                        byte[] sourceArray = new byte[this.readBlobData.Length];
                                        sourceArray = this.readBlobData;
                                        this.readBlobData = new byte[sourceArray.Length + length];
                                        Array.Copy(sourceArray, 0, this.readBlobData, 0, sourceArray.Length);
                                        Array.Copy(data, 0, this.readBlobData, sourceArray.Length, data.Length);
                                    }
                                    if (hciReplies.objTag != null)
                                    {
                                        this.readBlobHandle = (ushort) hciReplies.objTag;
                                        this.readBlobHandleValid = true;
                                    }
                                    else
                                    {
                                        this.readBlobHandle = 0;
                                        this.readBlobHandleValid = false;
                                    }
                                }
                            }
                            goto Label_04E1;

                        case 0x17:
                            this.SendRspCallback(hciReplies, true);
                            goto Label_04E1;

                        case 0x1a:
                            if ((this.readBlobData != null) && this.readBlobHandleValid)
                            {
                                Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                                string attrKey = this.attrUuidUtils.GetAttrKey(attReadBlobRsp.attMsgHdr.connHandle, this.readBlobHandle);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadBlobRsp"))
                                {
                                    flag = false;
                                    goto Label_04E1;
                                }
                                dataAttr.key = attrKey;
                                dataAttr.connHandle = attReadBlobRsp.attMsgHdr.connHandle;
                                dataAttr.handle = this.readBlobHandle;
                                dataAttr.value = this.devUtils.UnloadColonData(this.readBlobData, false);
                                if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
                                {
                                    flag = false;
                                    goto Label_04E1;
                                }
                                string[] delimiterStrs = new string[] { " ", ":" };
                                byte[] hexBytes = this.dataUtils.GetHexBytes(dataAttr.uuid, delimiterStrs);
                                if ((((hexBytes != null) && (hexBytes.Length > 1)) && ((hexBytes[0] == 3) && (hexBytes[1] == 40))) && (dataAttr.value.Length > 0))
                                {
                                    byte[] buffer4 = this.dataUtils.GetHexBytes(dataAttr.value, delimiterStrs);
                                    if (buffer4.Length > 0)
                                    {
                                        int index = 0;
                                        bool dataErr = false;
                                        dataAttr.properties = this.dataUtils.Unload8Bits(buffer4, ref index, ref dataErr);
                                        if (dataAttr.properties == 0)
                                        {
                                            dataAttr.propertiesStr = string.Empty;
                                        }
                                        else
                                        {
                                            dataAttr.propertiesStr = this.devUtils.GetGattCharProperties(dataAttr.properties, true) + " 0x" + dataAttr.properties.ToString("X2");
                                            if (buffer4.Length >= 5)
                                            {
                                                ushort handle = this.dataUtils.Unload16Bits(buffer4, ref index, ref dataErr, false);
                                                ushort connHandle = attReadBlobRsp.attMsgHdr.connHandle;
                                                attrKey = this.attrUuidUtils.GetAttrKey(connHandle, handle);
                                                DataAttr attr2 = new DataAttr();
                                                dataChanged = false;
                                                if (!this.attrDataUtils.GetDataAttr(ref attr2, ref dataChanged, attrKey, "AttReadBlobRsp"))
                                                {
                                                    flag = false;
                                                    goto Label_04E1;
                                                }
                                                attr2.key = attrKey;
                                                attr2.connHandle = connHandle;
                                                attr2.handle = handle;
                                                int dataLength = buffer4.Length - index;
                                                byte[] destData = new byte[dataLength];
                                                this.dataUtils.UnloadDataBytes(buffer4, dataLength, ref index, ref destData, ref dataErr);
                                                attr2.uuid = this.devUtils.UnloadColonData(destData, false);
                                                attr2.uuidHex = this.dataUtils.GetStringFromBytes(destData, true);
                                                attr2.properties = dataAttr.properties;
                                                attr2.propertiesStr = dataAttr.propertiesStr;
                                                attr2.indentLevel = this.attrUuidUtils.GetIndentLevel(attr2.uuidHex);
                                                attr2.uuidDesc = this.attrUuidUtils.GetUuidDesc(attr2.uuidHex);
                                                attr2.valueDesc = this.attrUuidUtils.GetUuidValueDesc(attr2.uuidHex);
                                                attr2.foreColor = this.attrUuidUtils.GetForegroundColor(attr2.uuidHex);
                                                attr2.backColor = this.attrUuidUtils.GetBackgroundColor(attr2.uuidHex);
                                                attr2.valueDsp = this.attrUuidUtils.GetValueDsp(attr2.uuidHex);
                                                attr2.valueEdit = this.attrUuidUtils.GetValueEdit(attr2.uuidHex);
                                                if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, attr2, dataChanged, attrKey))
                                                {
                                                    flag = false;
                                                    goto Label_04E1;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!this.attrDataUtils.UpdateAttrDict(tmpAttrDict))
                                {
                                    flag = false;
                                    goto Label_04E1;
                                }
                            }
                            this.readBlobData = null;
                            this.readBlobHandle = 0;
                            this.SendRspCallback(hciReplies, true);
                            goto Label_04E1;
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadBlobRsp");
                }
            }
        Label_04E1:
            if (!flag && dataFound)
            {
                this.readBlobData = null;
                this.readBlobHandle = 0;
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttReadBlobRspCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_ReadBlobRsp = hciReplies.hciLeExtEvent.attReadBlobRsp;
                this.AttReadBlobRspCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp aTT_ReadBlobRsp;
        }
    }
}
