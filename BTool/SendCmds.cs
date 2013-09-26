﻿namespace BTool
{
    using System;
    using System.Runtime.InteropServices;

    public class SendCmds
    {
        private DataUtils dataUtils = new DataUtils();
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public DisplayMsgDelegate DisplayMsgCallback;
        private const string moduleName = "SendCmds";
        private MsgBox msgBox = new MsgBox();
        private const string strSendDataErr = "Data Error Sending Message.\n{0}\n";
        private const string strSendExceptionErr = "Error Sending Message.\n{0}\n\n{1}\n";

        public SendCmds(DeviceForm deviceForm)
        {
            this.devForm = deviceForm;
        }

        private void DisplayInvalidAttributeValue(string value)
        {
            string msg = string.Format("Invalid Attribute Value '{0}'\nFormat: 11:22:33:44:55:66\n", value);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private void DisplayInvalidData(string data)
        {
            string msg = string.Format("Invalid Data Entry.\n '{0:D}'\nFormat Is  00:00....\n", data);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private void DisplayInvalidDataValue(string value)
        {
            string msg = string.Format("Invalid Data Value Entry.\n '{0:D}'\nFormat Is  00:00....\n", value);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private void DisplayInvalidUUIDEntry(string uuid)
        {
            string msg = string.Format("Invalid UUID Entry '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", uuid);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        public void DisplayInvalidValue(string value)
        {
            string msg = string.Format("Invalid Value Entry '{0}'\nFormat: 00:00....\n", value);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private bool HandleDataError(string cmdName)
        {
            string msg = string.Format("Data Error Sending Message.\n{0}\n", cmdName);
            this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return false;
        }

        private bool HandleException(string cmdName, string exceptionMsg)
        {
            string msg = string.Format("Error Sending Message.\n{0}\n\n{1}\n", cmdName, exceptionMsg);
            this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return false;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ErrorRsp obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.reqOpcode, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.errorCode, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ExchangeMTUReq obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.clientRxMTU, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ExchangeMTURsp obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.serverRxMTU, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ExecuteWriteRsp obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_FindByTypeValueReq obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.type, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Type Entry.\n '{0}'\nFormat Is 00:00\n", obj.type);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte[] buffer2 = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                if (buffer2 != null)
                {
                    dataLength = (byte) (dataLength + ((byte) buffer2.Length));
                }
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.dataUtils.LoadDataBytes(ref data, ref index, buffer2, ref dataErr);
                                    if (!dataErr)
                                    {
                                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                    }
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_FindByTypeValueRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.handlesInfo, 0x10);
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_FindInfoRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.info, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Info Entry.\n '{0}'\nFormat Is 00:00....\n", obj.info);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.format, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueConfirmation obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueIndication obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.authenticated, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueNotification obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.authenticated, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_PrepareWriteReq obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_PrepareWriteRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadBlobRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", obj.value);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadByGrpTypeRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.dataList, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Data List Entry.\n '{0}'\nFormat Is 00:00...\n", obj.dataList);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        obj.length = (byte) sourceData.Length;
                        this.dataUtils.Load8Bits(ref data, ref index, obj.length, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadByTypeReq obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.type, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Type UUID Entry.\n '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", obj.type);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadByTypeRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.dataList, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Data ListEntry.\n '{0}'\nFormat Is 00:00..........\n", obj.dataList);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        obj.length = (byte) sourceData.Length;
                        this.dataUtils.Load8Bits(ref data, ref index, obj.length, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadMultiReq obj)
        {
            bool flag = true;
            try
            {
                ushort[] numArray = this.devUtils.String2UInt16_LSBMSB(obj.handles, 0x10);
                bool dataErr = false;
                if ((numArray != null) && (numArray.Length > 1))
                {
                    byte dataLength = (byte) (obj.dataLength + (numArray.Length * 2));
                    byte[] data = new byte[dataLength + 4];
                    int index = 0;
                    if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            for (uint i = 0; i < numArray.Length; i++)
                            {
                                this.dataUtils.Load16Bits(ref data, ref index, numArray[i], ref dataErr, false);
                                if (dataErr)
                                {
                                    break;
                                }
                            }
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                else if (numArray == null)
                {
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n");
                }
                else if (numArray.Length < 2)
                {
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Need More Than One Characteristic Value Handle\nFormat: 0x0001;0x0002\n");
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadMultiRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.values, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", obj.values);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", obj.value);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_WriteRsp obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ExecuteWriteReq obj, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.flags, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, callback);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_FindInfoReq obj, TxDataOut.CmdType cmdType)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadByGrpTypeReq obj, TxDataOut.CmdType cmdType)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.groupType, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Group Type Entry.\n '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", obj.groupType);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_WriteReq obj, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.signature, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.command, ref dataErr);
                            if (!dataErr)
                            {
                                this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                                if (!dataErr)
                                {
                                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                    if (!dataErr)
                                    {
                                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General, obj.handle, callback);
                                    }
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadBlobReq obj, TxDataOut.CmdType cmdType, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendATT(HCICmds.ATTCmds.ATT_ReadReq obj, TxDataOut.CmdType cmdType, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_Authenticate obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_0217;
                }
                this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                if (dataErr)
                {
                    goto Label_0217;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.secReq_ioCaps, ref dataErr);
                if (dataErr)
                {
                    goto Label_0217;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.secReq_oobAvailable, ref dataErr);
                if (dataErr)
                {
                    goto Label_0217;
                }
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.secReq_oob, 0x10);
                if (sourceData.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_00FF;
                    }
                    goto Label_0217;
                }
                string msg = string.Format("Invalid secReq_OOB = {0:D} \nLength must be {1:D}", sourceData.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_00FF:
                this.dataUtils.Load8Bits(ref data, ref index, obj.secReq_authReq, ref dataErr);
                if (!dataErr)
                {
                    this.dataUtils.Load8Bits(ref data, ref index, obj.secReq_maxEncKeySize, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.secReq_keyDist, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.pairReq_Enable, ref dataErr);
                            if (!dataErr)
                            {
                                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.pairReq_ioCaps, ref dataErr);
                                if (!dataErr)
                                {
                                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.pairReq_oobDataFlag, ref dataErr);
                                    if (!dataErr)
                                    {
                                        this.dataUtils.Load8Bits(ref data, ref index, obj.pairReq_authReq, ref dataErr);
                                        if (!dataErr)
                                        {
                                            this.dataUtils.Load8Bits(ref data, ref index, obj.pairReq_maxEncKeySize, ref dataErr);
                                            if (!dataErr)
                                            {
                                                this.dataUtils.Load8Bits(ref data, ref index, obj.secReq_keyDist, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            Label_0217:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_Bond obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.secInfo_LTK, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid secInfo_LTK Entry.\n '{0}'\nFormat Is  00:00....", obj.secInfo_LTK);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte[] buffer2 = this.devUtils.String2Bytes_LSBMSB(obj.secInfo_RAND, 0x10);
                if (sourceData == null)
                {
                    string str2 = string.Format("Invalid secInfo_RRAND Value Entry.\n '{0}'\nFormat Is  00:00....", obj.secInfo_RAND);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                    return false;
                }
                byte dataLength = (byte) ((obj.dataLength + sourceData.Length) + buffer2.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_0203;
                }
                this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                if (dataErr)
                {
                    goto Label_0203;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.authenticated, ref dataErr);
                if (dataErr)
                {
                    goto Label_0203;
                }
                if (sourceData.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_0163;
                    }
                    goto Label_0203;
                }
                string msg = string.Format("Invalid secInfo_LTK Data Length = {0:D} \nLength must be {1:D}", sourceData.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_0163:
                this.dataUtils.Load16Bits(ref data, ref index, obj.secInfo_DIV, ref dataErr, false);
                if (dataErr)
                {
                    goto Label_0203;
                }
                if (buffer2.Length == 8)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, buffer2, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_01D4;
                    }
                    goto Label_0203;
                }
                string str4 = string.Format("Invalid secInfo_RAND Data Length = {0:D} \nLength must be {1:D}", sourceData.Length, (byte) 8);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                return false;
            Label_01D4:
                this.dataUtils.Load8Bits(ref data, ref index, obj.secInfo_LTKSize, ref dataErr);
                if (!dataErr)
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
            Label_0203:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_BondGetParam obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, (ushort) obj.paramId, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_BondSetParam obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, (ushort) obj.paramId, ref dataErr, false);
                    if (!dataErr)
                    {
                        obj.length = (byte) sourceData.Length;
                        this.dataUtils.Load8Bits(ref data, ref index, obj.length, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_ConfigDeviceAddr obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] sourceData = new byte[6];
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.addrType, ref dataErr);
                    if (!dataErr)
                    {
                        sourceData = this.devUtils.String2BDA_LSBMSB(obj.addr);
                        if (sourceData != null)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.mode, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.activeScan, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.whiteList, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceInit obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.irk, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid IRK Value Entry '{0}'\nFormat Is 00:00....\n", obj.irk);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte[] buffer2 = this.devUtils.String2Bytes_LSBMSB(obj.csrk, 0x10);
                if (buffer2 == null)
                {
                    string str2 = string.Format("Invalid CSRK Value Entry '{0}'\nFormat Is 00:00....\n", obj.csrk);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                    return false;
                }
                byte dataLength = (byte) ((obj.dataLength + sourceData.Length) + buffer2.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_01E6;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.profileRole, ref dataErr);
                if (dataErr)
                {
                    goto Label_01E6;
                }
                this.dataUtils.Load8Bits(ref data, ref index, obj.maxScanResponses, ref dataErr);
                if (dataErr)
                {
                    goto Label_01E6;
                }
                if (sourceData.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_0162;
                    }
                    goto Label_01E6;
                }
                string msg = string.Format("Invalid IRK Data Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_0162:
                if (buffer2.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, buffer2, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_01B6;
                    }
                    goto Label_01E6;
                }
                string str4 = string.Format("Invalid CSRK Data Length = {0:D} \nLength must be {1:D}\n", buffer2.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                return false;
            Label_01B6:
                this.dataUtils.Load32Bits(ref data, ref index, obj.signCounter, ref dataErr, false);
                if (!dataErr)
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
            Label_01E6:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_EndDiscoverable obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_EstablishLinkRequest obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] sourceData = new byte[6];
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.highDutyCycle, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.whiteList, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.addrTypePeer, ref dataErr);
                            if (!dataErr)
                            {
                                sourceData = this.devUtils.String2BDA_LSBMSB(obj.peerAddr);
                                if (sourceData != null)
                                {
                                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                    if (!dataErr)
                                    {
                                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_GetParam obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.paramId, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_MakeDiscoverable obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.initiatorAddr, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid Initiator Address Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.initiatorAddr);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_0159;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.eventType, ref dataErr);
                if (dataErr)
                {
                    goto Label_0159;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.initiatorAddrType, ref dataErr);
                if (dataErr)
                {
                    goto Label_0159;
                }
                if (sourceData.Length == 6)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_010E;
                    }
                    goto Label_0159;
                }
                string msg = string.Format("Invalid Initiator's Address Length = {0:D} \nLength must be {1:D}", sourceData.Length, (byte) 6);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_010E:
                this.dataUtils.Load8Bits(ref data, ref index, obj.channelMap, ref dataErr);
                if (!dataErr)
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.filterPolicy, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
            Label_0159:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_PasskeyUpdate obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.passKey, 0xff);
                        if (sourceData.Length == 6)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_RemoveAdvToken obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.adType, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_ResolvePrivateAddr obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.irk, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid IRK Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.irk);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte[] buffer2 = this.devUtils.String2BDA_LSBMSB(obj.addr);
                if (buffer2 == null)
                {
                    string str2 = string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.addr);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                    return false;
                }
                byte dataLength = (byte) ((obj.dataLength + sourceData.Length) + buffer2.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_0189;
                }
                if (sourceData.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_0123;
                    }
                    goto Label_0189;
                }
                string msg = string.Format("Invalid IRK Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_0123:
                if (buffer2.Length == 6)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, buffer2, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_0175;
                    }
                    goto Label_0189;
                }
                string str4 = string.Format("Invalid BDA Addr Address Length = {0:D} \nLength must be {1:D}\n", buffer2.Length, (byte) 6);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                return false;
            Label_0175:
                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
            Label_0189:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_SetAdvToken obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.advData, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid ADV Data Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.advData);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.adType, ref dataErr);
                    if (!dataErr)
                    {
                        obj.advDataLen = (byte) sourceData.Length;
                        this.dataUtils.Load8Bits(ref data, ref index, obj.advDataLen, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_SetParam obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.paramId, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.value, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_Signable obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.csrk, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid CSRK Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.csrk);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_0140;
                }
                this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                if (dataErr)
                {
                    goto Label_0140;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.authenticated, ref dataErr);
                if (dataErr)
                {
                    goto Label_0140;
                }
                if (sourceData.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_0110;
                    }
                    goto Label_0140;
                }
                string msg = string.Format("Invalid CSRK Data Length = {0:D} \nLength must be {1:D}", sourceData.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_0110:
                this.dataUtils.Load32Bits(ref data, ref index, obj.signCounter, ref dataErr, false);
                if (!dataErr)
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
            Label_0140:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_SlaveSecurityRequest obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.authReq, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_TerminateAuth obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.reason, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_TerminateLinkRequest obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateAdvertisingData obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.advertData, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid Advert Data Entry.\n '{0}' \nFormat Is  00:00....\n", obj.advertData);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.adType, ref dataErr);
                    if (!dataErr)
                    {
                        obj.dataLen = (byte) sourceData.Length;
                        this.dataUtils.Load8Bits(ref data, ref index, obj.dataLen, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateAdvTokens obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateLinkParamReq obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.intervalMin, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.intervalMax, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.Load16Bits(ref data, ref index, obj.connLatency, ref dataErr, false);
                                if (!dataErr)
                                {
                                    this.dataUtils.Load16Bits(ref data, ref index, obj.connTimeout, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                    }
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_AddAttribute obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.uuid, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidUUIDEntry(obj.uuid);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.permissions, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_AddService obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, (ushort) obj.uuid, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.numAttrs, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_DelService obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllChars obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_DiscCharsByUUID obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.type, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidUUIDEntry(obj.type);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_DiscPrimaryServiceByUUID obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ExchangeMTU obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.clientRxMTU, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_FindIncludedServices obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_Indication obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.authenticated, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_Notification obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.authenticated, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReadCharDesc obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, obj.handle);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReadLongCharDesc obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReadMultiCharValues obj)
        {
            bool flag = true;
            try
            {
                ushort[] numArray = this.devUtils.String2UInt16_LSBMSB(obj.handles, 0x10);
                bool dataErr = false;
                if ((numArray != null) && (numArray.Length > 1))
                {
                    byte dataLength = (byte) (obj.dataLength + (numArray.Length * 2));
                    byte[] data = new byte[dataLength + 4];
                    int index = 0;
                    if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            for (uint i = 0; i < numArray.Length; i++)
                            {
                                this.dataUtils.Load16Bits(ref data, ref index, numArray[i], ref dataErr, false);
                                if (dataErr)
                                {
                                    break;
                                }
                            }
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                else if (numArray == null)
                {
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n");
                }
                else if (numArray.Length < 2)
                {
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Need More Than One Characteristic Value Handle\nFormat: 0x0001;0x0002\n");
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReadUsingCharUUID obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.type, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidUUIDEntry(obj.type);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReliableWrites obj)
        {
            bool flag = true;
            try
            {
                if (obj.numRequests > 5)
                {
                    string msg = string.Format("Invalid Value Entry '{0}'\nValid Range Is 1 to {1}\n", obj.numRequests, 5);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                int num = 0;
                Element[] elementArray = new Element[5];
                if (obj.numRequests > 0)
                {
                    for (int i = 0; i < obj.numRequests; i++)
                    {
                        num++;
                        num += 2;
                        num += 2;
                        elementArray[i].temp = this.devUtils.String2Bytes_LSBMSB(obj.writeElement[i].value, 0x10);
                        if (elementArray[i].temp == null)
                        {
                            this.DisplayInvalidValue(obj.writeElement[i].value);
                            return false;
                        }
                        num += elementArray[i].temp.Length;
                    }
                }
                byte dataLength = (byte) (obj.dataLength + num);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.numRequests, ref dataErr);
                        if (!dataErr)
                        {
                            if (obj.numRequests > 0)
                            {
                                for (int j = 0; j < obj.numRequests; j++)
                                {
                                    obj.writeElement[j].valueLen = (byte) elementArray[j].temp.Length;
                                    this.dataUtils.Load8Bits(ref data, ref index, obj.writeElement[j].valueLen, ref dataErr);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    this.dataUtils.Load16Bits(ref data, ref index, obj.writeElement[j].handle, ref dataErr, false);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    this.dataUtils.Load16Bits(ref data, ref index, obj.writeElement[j].offset, ref dataErr, false);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    if (obj.writeElement[j].valueLen > 0)
                                    {
                                        this.dataUtils.LoadDataBytes(ref data, ref index, elementArray[j].temp, ref dataErr);
                                        if (dataErr)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_SignedWriteNoRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_WriteCharDesc obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_WriteLongCharDesc obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_WriteNoRsp obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllCharDescs obj, TxDataOut.CmdType cmdType)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllPrimaryServices obj, TxDataOut.CmdType cmdType)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_WriteCharValue obj, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidAttributeValue(obj.value);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General, obj.handle, callback);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReadCharValue obj, TxDataOut.CmdType cmdType, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_ReadLongCharValue obj, TxDataOut.CmdType cmdType, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendGATT(HCICmds.GATTCmds.GATT_WriteLongCharValue obj, byte[] valueData, SendCmdResult callback)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = null;
                if (valueData == null)
                {
                    sourceData = this.devUtils.String2Bytes_LSBMSB(obj.value, 0x10);
                    if (sourceData == null)
                    {
                        this.DisplayInvalidValue(obj.value);
                        return false;
                    }
                }
                else
                {
                    sourceData = valueData;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                                if (!dataErr)
                                {
                                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General, obj.handle, callback);
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ClkDivideOnHalt obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.control, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_DeclareNvUsage obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.mode, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_Decrypt obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.key, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid Key Entry.\n '{0}'\nFormat Is 00:00....\n", obj.key);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte[] buffer2 = this.devUtils.String2Bytes_LSBMSB(obj.data, 0x10);
                if (buffer2 == null)
                {
                    this.DisplayInvalidData(obj.data);
                    return false;
                }
                byte dataLength = (byte) ((obj.dataLength + sourceData.Length) + buffer2.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_0174;
                }
                if (sourceData.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_010C;
                    }
                    goto Label_0174;
                }
                string msg = string.Format("Invalid Key Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_010C:
                if (buffer2.Length == 0x10)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, buffer2, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_0160;
                    }
                    goto Label_0174;
                }
                string str3 = string.Format("Invalid Data Length = {0:D} \nLength must be {1:D}\n", buffer2.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                return false;
            Label_0160:
                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
            Label_0174:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_DisconnectImmed obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_EnablePTM obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_EndModemTest obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_MapPmIoPort obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.pmIoPort, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.pmIoPortPin, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemHopTestTx obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemTestRx obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, obj.rxRfChannel, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemTestTx obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.cwMode, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.txRfChannel, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_OnePktPerEvt obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.control, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_PER obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.perTestCommand, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SaveFreqTune obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetBDADDR obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2BDA_LSBMSB(obj.bleDevAddr);
                byte dataLength = obj.dataLength;
                if (sourceData != null)
                {
                    dataLength = (byte) (dataLength + ((byte) sourceData.Length));
                }
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetFastTxRespTime obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.control, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetFreqTune obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.setFreqTune, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetLocalSupportedFeatures obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.localFeatures, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid Local Features Entry.\n '{0}'\nFormat Is 00:00....\n", obj.localFeatures);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_00E1;
                }
                if (sourceData.Length == 8)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_00CD;
                    }
                    goto Label_00E1;
                }
                string msg = string.Format("Invalid Local Features Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, (byte) 8);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_00CD:
                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
            Label_00E1:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetMaxDtmTxPower obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.txPower, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetRxGain obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.rxGain, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetSCA obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.sca, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetTxPower obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.txPower, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEAddDeviceToWhiteList obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2BDA_LSBMSB(obj.devAddr);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.devAddr);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_00FE;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.addrType, ref dataErr);
                if (dataErr)
                {
                    goto Label_00FE;
                }
                if (sourceData.Length == 6)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_00EA;
                    }
                    goto Label_00FE;
                }
                string msg = string.Format("Invalid Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, (byte) 6);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_00EA:
                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
            Label_00FE:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEClearWhiteList obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEConnectionUpdate obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.connInterval, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.connIntervalMax, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.Load16Bits(ref data, ref index, obj.connLatency, ref dataErr, false);
                                if (!dataErr)
                                {
                                    this.dataUtils.Load16Bits(ref data, ref index, obj.connTimeout, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        this.dataUtils.Load16Bits(ref data, ref index, obj.minimumLength, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            this.dataUtils.Load16Bits(ref data, ref index, obj.maximumLength, ref dataErr, false);
                                            if (!dataErr)
                                            {
                                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LERemoveDeviceFromWhiteList obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2BDA_LSBMSB(obj.devAddr);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.devAddr);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (!this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    goto Label_00FE;
                }
                this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.addrType, ref dataErr);
                if (dataErr)
                {
                    goto Label_00FE;
                }
                if (sourceData.Length == 6)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_00EA;
                    }
                    goto Label_00FE;
                }
                string msg = string.Format("Invalid Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, (byte) 6);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_00EA:
                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
            Label_00FE:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_ReadRSSI obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendL2CAP(HCICmds.L2CAPCmds.L2CAP_ConnParamUpdateReq obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, obj.intervalMin, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.dataUtils.Load16Bits(ref data, ref index, obj.intervalMax, ref dataErr, false);
                            if (!dataErr)
                            {
                                this.dataUtils.Load16Bits(ref data, ref index, obj.slaveLatency, ref dataErr, false);
                                if (!dataErr)
                                {
                                    this.dataUtils.Load16Bits(ref data, ref index, obj.timeoutMultiplier, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                                    }
                                }
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendL2CAP(HCICmds.L2CAPCmds.L2CAP_InfoReq obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dataUtils.Load16Bits(ref data, ref index, (ushort) obj.infoType, ref dataErr, false);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendMISC(HCICmds.MISCCmds.MISC_GenericCommand obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.data, 0x10);
                if (sourceData == null)
                {
                    this.DisplayInvalidData(obj.data);
                    return false;
                }
                obj.dataLength = (byte) sourceData.Length;
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                ushort opCode = 0;
                try
                {
                    opCode = Convert.ToUInt16(obj.opCode.ToString(), 0x10);
                }
                catch (Exception exception)
                {
                    string msg = string.Format("Invalid OpCode Entry.\n '{0}'\nFormat Is 0x0000.\n\n{1}\n", obj.opCode.ToString(), exception.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, opCode, dataLength))
                {
                    if (sourceData.Length > 0)
                    {
                        this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                        if (dataErr)
                        {
                            goto Label_00E7;
                        }
                    }
                    this.TransmitCmd(obj.cmdName, opCode, data);
                }
            Label_00E7:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception2)
            {
                flag = this.HandleException(obj.cmdName, exception2.Message);
            }
            return flag;
        }

        public bool SendMISC(HCICmds.MISCCmds.MISC_RawTxMessage obj)
        {
            bool flag = true;
            try
            {
                ushort num3;
                bool dataErr = false;
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.message, 0x10);
                if (sourceData == null)
                {
                    string str = string.Format("Invalid Message Entry.\n '{0}'\nFormat Is  00:00....\n", obj.message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                    return false;
                }
                byte num = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[num];
                int index = 0;
                if (sourceData.Length >= 4)
                {
                    this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                    if (!dataErr)
                    {
                        goto Label_00B2;
                    }
                    goto Label_00E6;
                }
                string msg = string.Format("Raw Tx Message Length = {0:D} \nLength must be greater or equal to {1:D}\n", sourceData.Length, (byte) 4);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            Label_00B2:
                num3 = (ushort) (this.dataUtils.SetByte16(data[1], 0) + this.dataUtils.SetByte16(data[2], 1));
                this.TransmitCmd(obj.cmdName, num3, data);
            Label_00E6:
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendUTIL(HCICmds.UTILCmds.UTIL_ForceBoot obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool flag2 = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                }
                if (flag2)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendUTIL(HCICmds.UTILCmds.UTIL_NVRead obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, obj.nvId, ref dataErr);
                    if (!dataErr)
                    {
                        this.dataUtils.Load8Bits(ref data, ref index, obj.nvDataLen, ref dataErr);
                        if (!dataErr)
                        {
                            this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendUTIL(HCICmds.UTILCmds.UTIL_NVWrite obj)
        {
            bool flag = true;
            try
            {
                byte[] sourceData = this.devUtils.String2Bytes_LSBMSB(obj.nvData, 0x10);
                if (sourceData == null)
                {
                    string msg = string.Format("Invalid NV Data Entry.\n '{0}'\nFormat Is  00:00....\n", obj.nvData);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
                byte dataLength = (byte) (obj.dataLength + sourceData.Length);
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, obj.nvId, ref dataErr);
                    if (!dataErr)
                    {
                        obj.nvDataLen = (byte) sourceData.Length;
                        this.dataUtils.Load8Bits(ref data, ref index, obj.nvDataLen, ref dataErr);
                        if (!dataErr)
                        {
                            this.dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
                            if (!dataErr)
                            {
                                this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                            }
                        }
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        public bool SendUTIL(HCICmds.UTILCmds.UTIL_Reset obj)
        {
            bool flag = true;
            try
            {
                byte dataLength = obj.dataLength;
                byte[] data = new byte[dataLength + 4];
                int index = 0;
                bool dataErr = false;
                if (this.devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
                {
                    this.dataUtils.Load8Bits(ref data, ref index, (byte) obj.resetType, ref dataErr);
                    if (!dataErr)
                    {
                        this.TransmitCmd(obj.cmdName, obj.opCodeValue, data);
                    }
                }
                if (dataErr)
                {
                    flag = this.HandleDataError(obj.cmdName);
                }
            }
            catch (Exception exception)
            {
                flag = this.HandleException(obj.cmdName, exception.Message);
            }
            return flag;
        }

        private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data)
        {
            this.TransmitCmd(cmdName, cmdOpcode, data, null);
        }

        private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data, TxDataOut.CmdType cmdType)
        {
            this.TransmitCmd(cmdName, cmdOpcode, data, cmdType, null, null);
        }

        private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data, object tag)
        {
            this.TransmitCmd(cmdName, cmdOpcode, data, TxDataOut.CmdType.General, null, null);
        }

        private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data, TxDataOut.CmdType cmdType, object tag, SendCmdResult callback)
        {
            TxDataOut @out = new TxDataOut();
            @out.cmdName = cmdName;
            @out.cmdOpcode = cmdOpcode;
            @out.data = data;
            @out.cmdType = cmdType;
            @out.tag = tag;
            @out.callback = callback;
            this.devForm.threadMgr.txDataOut.dataQ.AddQTail(@out);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Element
        {
            public byte[] temp;
        }
    }
}

