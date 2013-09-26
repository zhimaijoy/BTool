﻿namespace BTool
{
    using System;

    public class DisplayTxCmds
    {
        private DataUtils dataUtils = new DataUtils();
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public DisplayMsgDelegate DisplayMsgCallback;
        public DisplayMsgTimeDelegate DisplayMsgTimeCallback;
        private DisplayCmdUtils dspCmdUtils = new DisplayCmdUtils();
        private const string moduleName = "DisplayTxCmds";

        public void DisplayTxCmd(TxDataOut txDataOut, bool displayBytes)
        {
            int num11;
            string str3;
            ushort cmdOpcode = txDataOut.cmdOpcode;
            byte[] data = txDataOut.data;
            byte packetType = data[0];
            byte length = data[3];
            string msg = string.Empty;
            string str2 = string.Empty;
            int num4 = 0;
            msg = msg + string.Format("-Type\t\t: 0x{0:X2} ({1:S})\n-Opcode\t\t: 0x{2:X4} ({3:S})\n-Data Length\t: 0x{4:X2} ({5:D}) byte(s)\n", new object[] { packetType, this.devUtils.GetPacketTypeStr(packetType), cmdOpcode, this.devUtils.GetOpCodeName(cmdOpcode), length, length });
            byte[] addr = new byte[6];
            str2 = string.Empty;
            int index = 4;
            byte num6 = 0;
            ushort num7 = 0;
            uint num8 = 0;
            bool dataErr = false;
            switch (cmdOpcode)
            {
                case 0xfd01:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num6 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ReqCode\t: 0x{0:X2} ({1:D})\n", num6, num6);
                            this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                            if (!dataErr)
                            {
                                num6 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" ErrorCode\t: 0x{0:X2} ({1:D})\n", num6, num6);
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd02:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ClientRxMTU\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        }
                    }
                    goto Label_2D9A;

                case 0xfd03:
                    num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ServerRxMTU\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        }
                    }
                    goto Label_2D9A;

                case 0xfd04:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfd05:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        byte num9 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Format\t\t: 0x{0:X2} ({1:S})\n", num9, this.devUtils.GetFindFormatStr(num9));
                            int uuidLength = this.devUtils.GetUuidLength(num9, ref dataErr);
                            if (!dataErr)
                            {
                                uuidLength += 2;
                                num11 = (length + 4) - index;
                                msg = msg + this.devUtils.UnloadHandleValueData(data, ref index, num11, uuidLength, ref dataErr);
                                if (!dataErr)
                                {
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd06:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        str2 = string.Empty;
                        msg = msg + string.Format(" Type\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 2, ref dataErr));
                        if (!dataErr)
                        {
                            str2 = string.Empty;
                            this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd07:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" HandlesInfo\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd08:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Type\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd09:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        byte num12 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Length\t\t: 0x{0:X2} ({1:D})\n", num12, num12);
                            if (num12 != 0)
                            {
                                num11 = (length + 4) - index;
                                msg = msg + this.devUtils.UnloadHandleValueData(data, ref index, num11, num12, ref dataErr);
                                if (!dataErr)
                                {
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd0a:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd0b:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd0c:
                    this.dspCmdUtils.AddConnectHandleOffset(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfd0d:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd0e:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        for (num4 = 0; num4 < ((length - 2) / 2); num4++)
                        {
                            num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                            if (dataErr)
                            {
                                break;
                            }
                            msg = msg + string.Format(" Handle #{0:D}\t: 0x{1:X4} ({1:D})\n", num4, num7, num7);
                        }
                    }
                    goto Label_2D9A;

                case 0xfd0f:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd10:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" GroupType\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd11:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num6 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Length\t\t: 0x{0:X2} ({1:D})\n", num6, num6) + string.Format(" DataList\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd12:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Signature\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapYesNoStr(num6));
                            this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" Command\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapYesNoStr(num6));
                                this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                                    if (!dataErr)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd13:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfd16:
                case 0xfd17:
                    this.dspCmdUtils.AddConnectHandleOffset(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd18:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Flags\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetAttExecuteWriteFlagsStr(num6));
                        }
                    }
                    goto Label_2D9A;

                case 0xfd19:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfd1b:
                case 0xfd1d:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Authenticated\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapYesNoStr(num6));
                            this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                            if (!dataErr)
                            {
                                this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                                if (!dataErr)
                                {
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd1e:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfc92:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" IntervalMin\t: 0x{0:X4} ({1:D})\n", num7, num7);
                            num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" IntervalMax\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" SlaveLatency\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" TimeoutMultiply\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfc8a:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" InfoType\t\t: 0x{0:X4} ({1:S})\n", num7, this.devUtils.GetL2CapInfoTypesStr(num7));
                        }
                    }
                    goto Label_2D9A;

                case 0x2010:
                case 0xfc09:
                case 0xfc0b:
                case 0xfc0e:
                case 0xfc10:
                case 0xfe05:
                case 0xfe08:
                case 0xfe35:
                case 0xfe83:
                    goto Label_2D9A;

                case 0x2011:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" AddressType\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetLEAddressTypeStr(num6)) + string.Format(" DeviceAddr\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0x2012:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" AddressType\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetLEAddressTypeStr(num6)) + string.Format(" DeviceAddr\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0x2013:
                    this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Handle\t\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ConnInterval\t: 0x{0:X4} ({1:D})\n", num7, num7);
                            this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" ConnIntervalMax\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" ConnLatency\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" ConnTimeout\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                        this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" MinimumLength\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                            this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" MaximumLength\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0x1405:
                    this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfc00:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Rx Gain\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtRxGainStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc01:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Tx Power\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtTxPowerStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc02:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Control\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtOnePktPerEvtCtrlStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc03:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Control\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtClkDivideOnHaltCtrlStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc04:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Mode\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtDeclareNvUsageModeStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc05:
                    str2 = string.Empty;
                    msg = msg + string.Format(" Key\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                    if (!dataErr)
                    {
                        str2 = string.Empty;
                        msg = msg + string.Format(" Data\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfc06:
                    str2 = string.Empty;
                    msg = msg + string.Format(" LocalFeatures\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfc07:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Control\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtSetFastTxRespTimeCtrlStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc08:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" CW Mode\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtCwModeStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Tx RF Channel\t: 0x{0:X2} ({1:D})\n", num6, num6);
                        }
                    }
                    goto Label_2D9A;

                case 0xfc0a:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Rx RF Channel\t: 0x{0:X2} ({1:D})\n", num6, num6);
                    }
                    goto Label_2D9A;

                case 0xfc0c:
                    msg = msg + string.Format(" BLEAddress\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, true, ref dataErr));
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfc0d:
                    num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" SCA\t\t: 0x{0:X4} ({1:D})\n", num7, num7);
                    }
                    goto Label_2D9A;

                case 0xfc0f:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Freq Tune\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtSetFreqTuneStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc11:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Max Tx Power\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtTxPowerStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfc12:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" PM IO Port\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtMapPmIoPortStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" PM IO Port Pin\t: 0x{0:X2} ({1:D})\n", num6, num6);
                        }
                    }
                    goto Label_2D9A;

                case 0xfc13:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfc14:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" PER Test Cmd\t: 0x{0:X2} ({1:D}) ({2:S})\n", num6, num6, this.devUtils.GetHciExtPERTestCommandStr(num6));
                        }
                    }
                    goto Label_2D9A;

                case 0xfd82:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ClientRxMTU\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        }
                    }
                    goto Label_2D9A;

                case 0xfd84:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfd86:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd88:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Type\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd8a:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd8c:
                    this.dspCmdUtils.AddConnectHandleOffset(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Type\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd8e:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Handles\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd90:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfd92:
                case 0xfdb6:
                case 0xfdb8:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                            this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfd96:
                    this.dspCmdUtils.AddConnectHandleOffset(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfd9b:
                case 0xfd9d:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Authentic\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapYesNoStr(num6));
                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                            this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfdb0:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfdb2:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfdb4:
                    this.dspCmdUtils.AddConnectStartEndHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Type\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfdba:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        num6 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Num Reqs\t: 0x{0:X2} ({1:D})\n", num6, num6);
                            if (num6 > 0)
                            {
                                for (int i = 0; i < num6; i++)
                                {
                                    byte num14 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    msg = msg + string.Format(" Value Len\t: 0x{0:X2} ({1:D})\n", num14, num14);
                                    num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    msg = msg + string.Format(" Handle\t\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (dataErr)
                                    {
                                        break;
                                    }
                                    msg = msg + string.Format(" Offset\t\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    if (num14 > 0)
                                    {
                                        msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, num14, ref dataErr));
                                        if (dataErr)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfdbc:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfdbe:
                    this.dspCmdUtils.AddConnectHandleOffset(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfdc0:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddOffset(data, ref index, ref dataErr, ref msg);
                        if (!dataErr)
                        {
                            this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfdc2:
                    this.dspCmdUtils.AddConnectHandleOffset(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfdfc:
                    num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" UUID\t\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" NumAttrst\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        }
                    }
                    goto Label_2D9A;

                case 0xfdfd:
                    this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfdfe:
                    msg = msg + string.Format(" UUID\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length + 4) - index) - 1, ref dataErr));
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Permissions\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGattPermissionsStr(num6));
                        }
                    }
                    goto Label_2D9A;

                case 0xfe00:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" ProfileRole\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapProfileStr(num6));
                        num6 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" MaxScanRsps\t: 0x{0:X2} ({1:D})\n", num6, num6) + string.Format(" IRK\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" CSRK\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                                if (!dataErr)
                                {
                                    num8 = this.dataUtils.Unload32Bits(data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" SignCounter\t: 0x{0:X8} ({1:D})\n", num8, num8);
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe03:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" AddrType\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAddrTypeStr(num6)) + string.Format(" Addr\t\t: 0x{0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, true, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfe04:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Mode\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapDiscoveryModeStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ActiveScan\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEnableDisableStr(num6));
                            this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" WhiteList\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEnableDisableStr(num6));
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe06:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" EventType\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEventTypeStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" InitAddrType\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAddrTypeStr(num6)) + string.Format(" InitAddrs\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 6, ref dataErr));
                            if (!dataErr)
                            {
                                this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" ChannelMap\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapChannelMapStr(num6));
                                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" FilterPolicy\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapFilterPolicyStr(num6));
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe07:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAdventAdTypeStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" DataLength\t: 0x{0:X2} ({1:D})\n", num6, num6) + string.Format(" AdvertData\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe09:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" HighDutyCycle\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEnableDisableStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" WhiteList\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEnableDisableStr(num6));
                            this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" AddrTypePeer\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAddrTypeStr(num6)) + string.Format(" PeerAddr\t\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, true, ref dataErr));
                                if (!dataErr)
                                {
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe0a:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                    }
                    goto Label_2D9A;

                case 0xfe0b:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" sec.ioCaps\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapIOCapsStr(num6));
                            this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" sec.oobAvail\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapTrueFalseStr(num6)) + string.Format(" sec.oob\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                                if (!dataErr)
                                {
                                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" sec.authReq\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAuthReqStr(num6));
                                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" sec.maxEKeySize\t: 0x{0:X2} ({1:D})\n", num6, num6);
                                            this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" sec.keyDist\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapKeyDiskStr(num6));
                                                this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    msg = msg + string.Format(" pair.Enable\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEnableDisableStr(num6));
                                                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                                    if (!dataErr)
                                                    {
                                                        msg = msg + string.Format(" pair.ioCaps\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapIOCapsStr(num6));
                                                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                                        if (!dataErr)
                                                        {
                                                            msg = msg + string.Format(" pair.oobDFlag\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapEnableDisableStr(num6));
                                                            this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                                            if (!dataErr)
                                                            {
                                                                msg = msg + string.Format(" pair.authReq\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAuthReqStr(num6));
                                                                this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                                                if (!dataErr)
                                                                {
                                                                    msg = msg + string.Format(" pair.maxEKeySize\t: 0x{0:X2} ({1:D})\n", num6, num6);
                                                                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                                                    if (!dataErr)
                                                                    {
                                                                        msg = msg + string.Format(" pair.keyDist\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapKeyDiskStr(num6));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe0c:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (dataErr)
                    {
                        goto Label_2D9A;
                    }
                    str3 = string.Empty;
                    for (num4 = 0; num4 < 6; num4++)
                    {
                        str3 = str3 + string.Format("{0:X2} ", this.dataUtils.Unload8Bits(data, ref index, ref dataErr));
                        if (dataErr)
                        {
                            break;
                        }
                    }
                    break;

                case 0xfe0d:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" AuthReq\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAuthReqStr(num6));
                        }
                    }
                    goto Label_2D9A;

                case 0xfe0e:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Authenticated\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAuthenticatedCsrkStr(num6)) + string.Format(" CSRK\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                            if (!dataErr)
                            {
                                num8 = this.dataUtils.Unload32Bits(data, ref index, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" SignCounter\t: 0x{0:X8} ({1:D})\n", num8, num8);
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe0f:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Authenticated\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapYesNoStr(num6)) + string.Format(" LongTermKey\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                            if (!dataErr)
                            {
                                num7 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" DIV\t\t: 0x{0:X4} ({1:D})\n", num7, num7) + string.Format(" Rand\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 8, ref dataErr));
                                    if (!dataErr)
                                    {
                                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" LTKSize\t\t: 0x{0:X2} ({1:D})\n", num6, num6);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe10:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Reason\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapSMPFailureTypesStr(num6));
                        }
                    }
                    goto Label_2D9A;

                case 0xfe11:
                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                    if (!dataErr)
                    {
                        this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" IntervalMin\t: 0x{0:X4} ({1:D})\n", num7, num7);
                            this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                            if (!dataErr)
                            {
                                msg = msg + string.Format(" IntervalMax\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" ConnLatency\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" ConnTimeout\t: 0x{0:X4} ({1:D})\n", num7, num7);
                                    }
                                }
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe30:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" ParamID\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapParamIdStr(num6));
                        this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ParamValue\t: 0x{0:X4} ({1:D})\n", num7, num7);
                        }
                    }
                    goto Label_2D9A;

                case 0xfe31:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" ParamID\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapParamIdStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfe32:
                    msg = msg + string.Format(" IRK\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Addr\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                        if (!dataErr)
                        {
                        }
                    }
                    goto Label_2D9A;

                case 0xfe33:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAdTypesStr(num6));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" AdvDataLen\t: 0x{0:X2} ({1:D})\n", num6, num6) + string.Format(" AdvData\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe34:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetGapAdTypesStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfe36:
                    this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" ParamID\t\t: 0x{0:X4} ({1:S})\n", num7, this.devUtils.GetGapBondParamIdStr(num7));
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" ParamLength\t: 0x{0:X2} ({1:D})\n", num6, num6);
                            this.dspCmdUtils.AddValue(data, ref index, ref dataErr, ref msg, length, 4);
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                case 0xfe37:
                    this.dataUtils.Unload16Bits(data, ref index, ref num7, ref dataErr, false);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" ParamID\t\t: 0x{0:X4} ({1:S})\n", num7, this.devUtils.GetGapBondParamIdStr(num7));
                    }
                    goto Label_2D9A;

                case 0xfe80:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" ResetType\t: 0x{0:X2} ({1:S})\n", num6, this.devUtils.GetUtilResetTypeStr(num6));
                    }
                    goto Label_2D9A;

                case 0xfe81:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" NvID\t\t: 0x{0:X2} ({1:D})\n", num6, num6);
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" NvDataLen\t: 0x{0:X2} ({1:D})\n", num6, num6);
                        }
                    }
                    goto Label_2D9A;

                case 0xfe82:
                    this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" NvID\t\t: 0x{0:X2} ({1:D})\n", num6, num6);
                        this.dataUtils.Unload8Bits(data, ref index, ref num6, ref dataErr);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" NvDataLen\t: 0x{0:X2} ({1:D})\n", num6, num6) + string.Format(" NvData\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + 4) - index, ref dataErr));
                            if (!dataErr)
                            {
                            }
                        }
                    }
                    goto Label_2D9A;

                default:
                    for (num4 = 4; (num4 < (length + 4)) && (num4 < data.Length); num4++)
                    {
                        str2 = str2 + string.Format("{0:X2} ", data[num4]);
                        this.devUtils.CheckLineLength(ref str2, (uint) (num4 - 4), true);
                    }
                    msg = msg + string.Format(" Raw\t\t: {0:S}\n", str2);
                    goto Label_2D9A;
            }
            str3 = str3.Trim();
            msg = msg + string.Format(" PassKey\t\t: {0:S}\n", this.devUtils.HexStr2UserDefinedStr(str3, SharedAppObjs.StringType.ASCII));
        Label_2D9A:
            if (this.DisplayMsgCallback != null)
            {
                if (dataErr)
                {
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, "Could Not Convert All The Data In The Following Message\n(Message Is Missing Data Bytes To Process)\n");
                }
                if (data.Length != index)
                {
                    int num15 = data.Length - index;
                    string str4 = string.Format("The Last {0} Bytes In This Message Were Not Decoded.\n", num15) + "(Message Has More Than The Expected Number Of Data Bytes)\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Warning, str4);
                }
                this.DisplayMsgTimeCallback(SharedAppObjs.MsgType.Outgoing, msg, txDataOut.time);
                if (displayBytes)
                {
                    string str5 = "";
                    uint num16 = 0;
                    foreach (byte num17 in data)
                    {
                        str5 = str5 + string.Format("{0:X2} ", num17);
                        this.devUtils.CheckLineLength(ref str5, num16++, false);
                    }
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.TxDump, str5);
                }
            }
        }
    }
}

