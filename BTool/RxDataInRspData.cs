﻿namespace BTool
{
    using System;
    using System.Collections.Generic;

    public class RxDataInRspData
    {
        private DataUtils dataUtils = new DataUtils();
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        private const string moduleName = "RxDataInRspData";
        private MsgBox msgBox = new MsgBox();

        public RxDataInRspData(DeviceForm deviceForm)
        {
            this.devForm = deviceForm;
        }

        public void GetRspData(RxDataIn rxDataIn, HCIStopWait.StopWaitEvent stopWaitEvent)
        {
            int index = 0;
            bool dataErr = false;
            int totalLength = 0;
            try
            {
                int num7;
                int num8;
                int num9;
                int num10;
                ushort cmdOpCode;
                HCIReplies data = new HCIReplies();
                data.objTag = null;
                data.cmdType = TxDataOut.CmdType.General;
                if (stopWaitEvent != null)
                {
                    data.objTag = stopWaitEvent.tag;
                    data.cmdType = stopWaitEvent.cmdType;
                }
                ushort cmdOpcode = rxDataIn.cmdOpcode;
                if (((cmdOpcode != 14) && (cmdOpcode != 0x13)) && (cmdOpcode == 0xff))
                {
                    byte num3 = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                    if (!dataErr)
                    {
                        data.hciLeExtEvent = new HCIReplies.HCI_LE_ExtEvent();
                        data.hciLeExtEvent.header.eventCode = rxDataIn.eventOpcode;
                        data.hciLeExtEvent.header.eventStatus = num3;
                        ushort eventOpcode = rxDataIn.eventOpcode;
                        if (eventOpcode <= 0x493)
                        {
                            if (eventOpcode <= 0x481)
                            {
                            }
                            else if ((eventOpcode == 0x48b) || (eventOpcode == 0x493))
                            {
                            }
                        }
                        else
                        {
                            int uuidLength;
                            int num6;
                            switch (eventOpcode)
                            {
                                case 0x501:
                                    data.hciLeExtEvent.attErrorRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attErrorRsp.attMsgHdr)) != 0) && !dataErr)
                                    {
                                        byte num4 = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            data.hciLeExtEvent.attErrorRsp.reqOpCode = num4;
                                            data.hciLeExtEvent.attErrorRsp.handle = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                                            if (!dataErr)
                                            {
                                                data.hciLeExtEvent.attErrorRsp.errorCode = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 0x505:
                                    data.hciLeExtEvent.attFindInfoRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attFindInfoRsp.attMsgHdr)) != 0) && !dataErr)
                                    {
                                        data.hciLeExtEvent.attFindInfoRsp.format = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            uuidLength = this.devUtils.GetUuidLength(data.hciLeExtEvent.attFindInfoRsp.format, ref dataErr);
                                            if (!dataErr)
                                            {
                                                uuidLength += 2;
                                                num6 = rxDataIn.length - index;
                                                data.hciLeExtEvent.attFindInfoRsp.handleData = new List<HCIReplies.HandleData>();
                                                this.devUtils.UnloadHandleValueData(rxDataIn.data, ref index, num6, uuidLength, ref dataErr, "Uuid", ref data.hciLeExtEvent.attFindInfoRsp.handleData);
                                                if (!dataErr)
                                                {
                                                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 0x507:
                                    data.hciLeExtEvent.attFindByTypeValueRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attFindByTypeValueRsp.attMsgHdr)) == 0) || dataErr)
                                    {
                                        break;
                                    }
                                    if (totalLength < 2)
                                    {
                                        goto Label_04E0;
                                    }
                                    num7 = totalLength / 2;
                                    data.hciLeExtEvent.attFindByTypeValueRsp.handle = new ushort[num7];
                                    num8 = 0;
                                    goto Label_04D7;

                                case 0x509:
                                    data.hciLeExtEvent.attReadByTypeRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attReadByTypeRsp.attMsgHdr)) != 0) && !dataErr)
                                    {
                                        uuidLength = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            data.hciLeExtEvent.attReadByTypeRsp.length = (byte) uuidLength;
                                            totalLength--;
                                            if (uuidLength != 0)
                                            {
                                                string handleStr = string.Empty;
                                                string valueStr = string.Empty;
                                                data.hciLeExtEvent.attReadByTypeRsp.handleData = new List<HCIReplies.HandleData>();
                                                this.devUtils.UnloadHandleValueData(rxDataIn.data, ref index, totalLength, uuidLength, ref handleStr, ref valueStr, ref dataErr, "Data", ref data.hciLeExtEvent.attReadByTypeRsp.handleData);
                                                if (!dataErr)
                                                {
                                                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 0x50b:
                                    data.hciLeExtEvent.attReadRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attReadRsp.attMsgHdr)) == 0) || dataErr)
                                    {
                                        break;
                                    }
                                    data.hciLeExtEvent.attReadRsp.data = new byte[totalLength];
                                    num9 = 0;
                                    goto Label_0681;

                                case 0x50d:
                                    data.hciLeExtEvent.attReadBlobRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp();
                                    totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attReadBlobRsp.attMsgHdr);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    if (totalLength <= 0)
                                    {
                                        goto Label_073C;
                                    }
                                    data.hciLeExtEvent.attReadBlobRsp.data = new byte[totalLength];
                                    num10 = 0;
                                    goto Label_0734;

                                case 0x511:
                                    data.hciLeExtEvent.attReadByGrpTypeRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attReadByGrpTypeRsp.attMsgHdr)) != 0) && !dataErr)
                                    {
                                        byte num11 = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            data.hciLeExtEvent.attReadByGrpTypeRsp.length = num11;
                                            if (num11 != 0)
                                            {
                                                uuidLength = num11;
                                                num6 = ((rxDataIn.length - 3) - index) + 1;
                                                data.hciLeExtEvent.attReadByGrpTypeRsp.handleHandleData = new List<HCIReplies.HandleHandleData>();
                                                this.devUtils.UnloadHandleHandleValueData(rxDataIn.data, ref index, num6, uuidLength, ref dataErr, ref data.hciLeExtEvent.attReadByGrpTypeRsp.handleHandleData);
                                                if (!dataErr)
                                                {
                                                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 0x513:
                                    data.hciLeExtEvent.attWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp();
                                    if (((totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attWriteRsp.attMsgHdr)) != 0) && !dataErr)
                                    {
                                        this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                    }
                                    break;

                                case 0x517:
                                    data.hciLeExtEvent.attPrepareWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp();
                                    totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attPrepareWriteRsp.attMsgHdr);
                                    if (!dataErr)
                                    {
                                        data.hciLeExtEvent.attPrepareWriteRsp.handle = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            data.hciLeExtEvent.attPrepareWriteRsp.offset = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                                            if (!dataErr)
                                            {
                                                data.hciLeExtEvent.attPrepareWriteRsp.value = this.devUtils.UnloadColonData(rxDataIn.data, ref index, rxDataIn.data.Length - index, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 0x519:
                                    data.hciLeExtEvent.attExecuteWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp();
                                    totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attExecuteWriteRsp.attMsgHdr);
                                    if (!dataErr)
                                    {
                                        this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                    }
                                    break;

                                case 0x51b:
                                    data.hciLeExtEvent.attHandleValueNotification = new HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification();
                                    totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attHandleValueNotification.attMsgHdr);
                                    if (!dataErr)
                                    {
                                        data.hciLeExtEvent.attHandleValueNotification.handle = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            data.hciLeExtEvent.attHandleValueNotification.value = this.devUtils.UnloadColonData(rxDataIn.data, ref index, rxDataIn.data.Length - index, ref dataErr);
                                            if (!dataErr)
                                            {
                                                this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                            }
                                        }
                                    }
                                    break;

                                case 0x51d:
                                    data.hciLeExtEvent.attHandleValueIndication = new HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication();
                                    totalLength = this.UnloadAttMsgHeader(ref rxDataIn.data, ref index, ref dataErr, ref data.hciLeExtEvent.attHandleValueIndication.attMsgHdr);
                                    if (!dataErr)
                                    {
                                        data.hciLeExtEvent.attHandleValueIndication.handle = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            data.hciLeExtEvent.attHandleValueIndication.value = this.devUtils.UnloadColonData(rxDataIn.data, ref index, rxDataIn.data.Length - index, ref dataErr);
                                            if (!dataErr)
                                            {
                                                this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                            }
                                        }
                                    }
                                    break;

                                case 0x67f:
                                    data.hciLeExtEvent.gapHciCmdStat = new HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus();
                                    data.hciLeExtEvent.gapHciCmdStat.cmdOpCode = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        data.hciLeExtEvent.gapHciCmdStat.dataLength = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            cmdOpCode = data.hciLeExtEvent.gapHciCmdStat.cmdOpCode;
                                            if (cmdOpCode > 0xfd96)
                                            {
                                                goto Label_0CED;
                                            }
                                            if (cmdOpCode <= 0xfc92)
                                            {
                                                if ((cmdOpCode == 0xfc8a) || (cmdOpCode == 0xfc92))
                                                {
                                                }
                                            }
                                            else
                                            {
                                                switch (cmdOpCode)
                                                {
                                                    case 0xfd0a:
                                                    case 0xfd0b:
                                                    case 0xfd0c:
                                                    case 0xfd0d:
                                                        this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                        break;

                                                    case 0xfd12:
                                                    case 0xfd13:
                                                    case 0xfd16:
                                                    case 0xfd17:
                                                    case 0xfd18:
                                                    case 0xfd19:
                                                        this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                        break;

                                                    case 0xfd84:
                                                    case 0xfd88:
                                                    case 0xfd8a:
                                                        goto Label_0E7E;

                                                    case 0xfd86:
                                                    case 0xfd90:
                                                        this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                                                        break;

                                                    case 0xfd8c:
                                                        goto Label_0E9C;

                                                    case 0xfd92:
                                                    case 0xfd96:
                                                        goto Label_0EBA;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                return;
            Label_04A5:
                data.hciLeExtEvent.attFindByTypeValueRsp.handle[num8] = this.dataUtils.Unload16Bits(rxDataIn.data, ref index, ref dataErr, false);
                if (dataErr)
                {
                    goto Label_04E0;
                }
                num8++;
            Label_04D7:
                if ((num8 < num7) && !dataErr)
                {
                    goto Label_04A5;
                }
            Label_04E0:
                if (!dataErr)
                {
                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                }
                return;
            Label_0653:
                data.hciLeExtEvent.attReadRsp.data[num9] = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                num9++;
            Label_0681:
                if ((num9 < totalLength) && !dataErr)
                {
                    goto Label_0653;
                }
                if (!dataErr)
                {
                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                }
                return;
            Label_0706:
                data.hciLeExtEvent.attReadBlobRsp.data[num10] = this.dataUtils.Unload8Bits(rxDataIn.data, ref index, ref dataErr);
                num10++;
            Label_0734:
                if ((num10 < totalLength) && !dataErr)
                {
                    goto Label_0706;
                }
            Label_073C:
                if (!dataErr)
                {
                    this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                }
                return;
            Label_0CED:
                if (cmdOpCode <= 0xfdc2)
                {
                    switch (cmdOpCode)
                    {
                        case 0xfdb2:
                            goto Label_0E7E;
                    }
                }
                return;
            Label_0E7E:
                this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                return;
            Label_0E9C:
                this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
                return;
            Label_0EBA:
                this.devForm.threadMgr.rspDataIn.dataQ.AddQTail(data);
            }
            catch (Exception exception)
            {
                string msg = "Get Response Data Problem.\n" + exception.Message + "\nRxDataInRspData\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        }

        public byte UnloadAttMsgHeader(ref byte[] data, ref int index, ref bool dataErr, ref HCIReplies.ATT_MsgHeader attMsgHdr)
        {
            attMsgHdr.connHandle = 0;
            attMsgHdr.pduLength = 0;
            try
            {
                attMsgHdr.connHandle = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                attMsgHdr.pduLength = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
            }
            catch (Exception exception)
            {
                string msg = string.Format("UnloadAttMsgHeader Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                dataErr = true;
            }
            return attMsgHdr.pduLength;
        }
    }
}

