﻿namespace BTool
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public class HCICmds
    {
        public const byte CmdHdrSize = 4;
        public const ushort CmdRspReqOCodeMask = 0xff;
        public const string ConnHandleAll = "0xFFFF";
        public const string ConnHandleDefault = "0xFFFE";
        public const string ConnHandleInit = "0xFFFE";
        public const string Empty16BytesStr = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
        public const string Empty2BytesStr = "00:00";
        public const string Empty6BytesStr = "00:00:00:00:00:00";
        public const string Empty8BytesStr = "00:00:00:00:00:00:00:00";
        public const string EmptyBDAStr = "00:00:00:00:00:00";
        public const ushort EndHandleDefault = 0xffff;
        public const string EndHandleDefaultStr = "0xFFFF";
        public const byte EvtHdrSize = 3;
        public const ushort HandleDefault = 1;
        public const string HandleDefaultStr = "0x0001";
        public const ushort HandleInvalid = 0;
        public const string HandleInvalidStr = "0x0000";
        public const ushort MaxUInt16 = 0xffff;
        public const ushort OffsetDefault = 0;
        public const string OffsetDefaultStr = "0x0000";
        public string[,] OpCodeLookupTable = new string[,] { 
            { "0x0001", "HCI_InquiryCompleteEvent" }, { "0x0002", "HCI_InquiryResultEvent" }, { "0x0003", "HCI_ConnectionCompleteEvent" }, { "0x0004", "HCI_ConnectionRequestEvent" }, { "0x0005", "HCI_DisconnectionCompleteEvent" }, { "0x0006", "HCI_AuthenticationCompleteEvent" }, { "0x0007", "HCI_RemoteNameRequestCompleteEvent" }, { "0x0008", "HCI_EncryptionChangeEvent" }, { "0x0009", "HCI_ChangeConnectionLinkKeyCompleteEvent" }, { "0x000A", "HCI_MasterLinkKeyCompleteEvent" }, { "0x000B", "HCI_ReadRemoteSupportedFeaturesCompleteEvent" }, { "0x000C", "HCI_ReadRemoteVersionInformationCompleteEvent" }, { "0x000D", "HCI_QoSSetupCompleteEvent" }, { "0x000E", "HCI_CommandCompleteEvent" }, { "0x000F", "HCI_CommandStatusEvent" }, { "0x0010", "HCI_HardwareErrorEvent" }, 
            { "0x0011", "HCI_FlushOccurredEvent" }, { "0x0012", "HCI_RoleChangeEvent" }, { "0x0013", "HCI_NumberOfCompletedPacketsEvent" }, { "0x0014", "HCI_ModeChangeEvent" }, { "0x0015", "HCI_ReturnLinkKeysEvent" }, { "0x0016", "HCI_PINCodeRequestEvent" }, { "0x0017", "HCI_LinkKeyRequestEvent" }, { "0x0018", "HCI_LinkKeyNotificationEvent" }, { "0x0019", "HCI_LoopbackCommandEvent" }, { "0x001A", "HCI_DataBufferOverflowEvent" }, { "0x001B", "HCI_MaxSlotsChangeEvent" }, { "0x001C", "HCI_ReadClockOffsetCompleteEvent" }, { "0x001D", "HCI_ConnectionPacketTypeChangedEvent" }, { "0x001E", "HCI_QoSViolationEvent" }, { "0x001F", "HCI_PageScanModeChangeEvent" }, { "0x0020", "HCI_PageScanRepetitionModeChangeEvent" }, 
            { "0x0021", "HCI_FlowSpecificationCompleteEvent" }, { "0x0022", "HCI_InquiryResultWithRSSIEvent" }, { "0x0023", "HCI_ReadRemoteExtendedFeaturesCompleteEvent" }, { "0x002C", "HCI_SynchronousConnectionCompleteEvent" }, { "0x002D", "HCI_SynchronousConnectionChangedEvent" }, { "0x002E", "HCI_SniffSubratingEvent" }, { "0x002F", "HCI_ExtendedInquiryResultEvent" }, { "0x0030", "HCI_EncryptionKeyRefreshCompleteEvent" }, { "0x0031", "HCI_IOCapabilityRequestEvent" }, { "0x0032", "HCI_IOCapabilityResponseEvent" }, { "0x0033", "HCI_UserConfirmationRequestEvent" }, { "0x0034", "HCI_UserPasskeyRequestEvent" }, { "0x0035", "HCI_RemoteOOBDataRequestEvent" }, { "0x0036", "HCI_SimplePairingCompleteEvent" }, { "0x0037", "HCI_RemoteOobResponseEvent" }, { "0x0038", "HCI_LinkSupervisionTimeoutChangedEvent" }, 
            { "0x0039", "HCI_EnhancedFlushCompleteEvent" }, { "0x003A", "HCI_SniffRequestEvent" }, { "0x003B", "HCI_UserPasskeyNotificationEvent" }, { "0x003C", "HCI_KeypressNotificationEvent" }, { "0x003D", "HCI_RemoteHostSupportedFeaturesNotificationEvent" }, { "0x0040", "HCI_PhysicalLinkCompleteEvent" }, { "0x0041", "HCI_ChannelSelectedEvent" }, { "0x0042", "HCI_DisconnectionPhysicalLinkCompleteEvent" }, { "0x0043", "HCI_PhysicalLinkLossEarlyWarningEvent" }, { "0x0044", "HCI_PhysicalLinkRecoveryEvent" }, { "0x0045", "HCI_LogicalLinkCompleteEvent" }, { "0x0046", "HCI_DisconnectionLogicalLinkCompleteEvent" }, { "0x0047", "HCI_FlowSpecModifyCompleteEvent" }, { "0x0048", "HCI_NumberOfCompletedDataBlocksEvent" }, { "0x004C", "HCI_ShortRangeModeChangeCompleteEvent" }, { "0x004D", "HCI_AMP_StatusChangeEvent" }, 
            { "0x0049", "HCI_AMP_StartTestEvent" }, { "0x004A", "HCI_AMP_TestEndEvent" }, { "0x004B", "HCI_AMP_ReceiverReportEvent" }, { "0x003E", "HCI_LE_ConnectionCompleteEvent" }, { "0x003E", "HCI_LE_AdvertisingReportEvent" }, { "0x003E", "HCI_LE_ConnectionUpdateCompleteEvent" }, { "0x003E", "HCI_LE_ReadRemoteUsedFeaturesCompleteEvent" }, { "0x003E", "HCI_LE_LongTermKeyRequestEvent" }, { "0x00FF", "HCI_LE_ExtEvent" }, { "0x0400", "HCIExt_SetRxGainDone" }, { "0x0401", "HCIExt_SetTxPowerDone" }, { "0x0402", "HCIExt_OnePktPerEvtDone" }, { "0x0403", "HCIExt_ClkDivideOnHaltDone" }, { "0x0404", "HCIExt_DeclareNvUsageDone" }, { "0x0405", "HCIExt_DecryptDone" }, { "0x0406", "HCIExt_SetLocalSupportedFeaturesDone" }, 
            { "0x0407", "HCIExt_SetFastTxRespTimeDone" }, { "0x0408", "HCIExt_ModemTestTxDone" }, { "0x0409", "HCIExt_ModemHopTestTxDone" }, { "0x040A", "HCIExt_ModemTestRxDone" }, { "0x040B", "HCIExt_EndModemTestDone" }, { "0x040C", "HCIExt_SetBDADDRDone" }, { "0x040D", "HCIExt_SetSCADone" }, { "0x040E", "HCIExt_EnablePTMDone" }, { "0x040F", "HCIExt_SetFreqTuneDone" }, { "0x0410", "HCIExt_SaveFreqTuneDone" }, { "0x0411", "HCIExt_SetMaxDtmTxPowerDone" }, { "0x0412", "HCIExt_MapPmIoPortDone" }, { "0x0413", "HCIExt_DisconnectImmed" }, { "0x0414", "HCIExt_PER" }, { "0x0481", "L2CAP_CmdReject" }, { "0x048B", "L2CAP_InfoRsp" }, 
            { "0x0493", "L2CAP_ConnParamUpdateRsp" }, { "0x0501", "ATT_ErrorRsp" }, { "0x0502", "ATT_ExchangeMTUReq" }, { "0x0503", "ATT_ExchangeMTURsp" }, { "0x0504", "ATT_FindInfoReq" }, { "0x0505", "ATT_FindInfoRsp" }, { "0x0506", "ATT_FindByTypeValueReq" }, { "0x0507", "ATT_FindByTypeValueRsp" }, { "0x0508", "ATT_ReadByTypeReq" }, { "0x0509", "ATT_ReadByTypeRsp" }, { "0x050A", "ATT_ReadReq" }, { "0x050B", "ATT_ReadRsp" }, { "0x050C", "ATT_ReadBlobReq" }, { "0x050D", "ATT_ReadBlobRsp" }, { "0x050E", "ATT_ReadMultiReq" }, { "0x050F", "ATT_ReadMultiRsp" }, 
            { "0x0510", "ATT_ReadByGrpTypeReq" }, { "0x0511", "ATT_ReadByGrpTypeRsp" }, { "0x0512", "ATT_WriteReq" }, { "0x0513", "ATT_WriteRsp" }, { "0x0516", "ATT_PrepareWriteReq" }, { "0x0517", "ATT_PrepareWriteRsp" }, { "0x0518", "ATT_ExecuteWriteReq" }, { "0x0519", "ATT_ExecuteWriteRsp" }, { "0x051B", "ATT_HandleValueNotification" }, { "0x051D", "ATT_HandleValueIndication" }, { "0x051E", "ATT_HandleValueConfirmation" }, { "0xFD88", "GATT_DiscCharsByUUID" }, { "0x0580", "GATT_ClientCharCfgUpdated" }, { "0x0600", "GAP_DeviceInitDone" }, { "0x0601", "GAP_DeviceDiscoveryDone" }, { "0x0602", "GAP_AdvertDataUpdate" }, 
            { "0x0603", "GAP_MakeDiscoverable" }, { "0x0604", "GAP_EndDiscoverable" }, { "0x0605", "GAP_EstablishLink" }, { "0x0606", "GAP_TerminateLink" }, { "0x0607", "GAP_LinkParamUpdate" }, { "0x0608", "GAP_RandomAddressChange" }, { "0x0609", "GAP_SignatureUpdate" }, { "0x060A", "GAP_AuthenticationComplete" }, { "0x060B", "GAP_PasskeyNeeded" }, { "0x060C", "GAP_SlaveRequestedSecurity" }, { "0x060D", "GAP_DeviceInformation" }, { "0x060E", "GAP_BondComplete" }, { "0x060F", "GAP_PairingRequested" }, { "0x067F", "GAP_HCI_ExtentionCommandStatus" }, { "0xFC00", "HCIExt_SetRxGain" }, { "0xFC01", "HCIExt_SetTxPower" }, 
            { "0xFC02", "HCIExt_OnePktPerEvt" }, { "0xFC03", "HCIExt_ClkDivideOnHalt" }, { "0xFC04", "HCIExt_DeclareNvUsage" }, { "0xFC05", "HCIExt_Decrypt" }, { "0xFC06", "HCIExt_SetLocalSupportedFeatures" }, { "0xFC07", "HCIExt_SetFastTxRespTime" }, { "0xFC08", "HCIExt_ModemTestTx" }, { "0xFC09", "HCIExt_ModemHopTestTx" }, { "0xFC0A", "HCIExt_ModemTestRx" }, { "0xFC0B", "HCIExt_EndModemTest" }, { "0xFC0C", "HCIExt_SetBDADDR" }, { "0xFC0D", "HCIExt_SetSCA" }, { "0xFC0E", "HCIExt_EnablePTM" }, { "0xFC0F", "HCIExt_SetFreqTune" }, { "0xFC10", "HCIExt_SaveFreqTune" }, { "0xFC11", "HCIExt_SetMaxDtmTxPower" }, 
            { "0xFC12", "HCIExt_MapPmIoPort" }, { "0xFC13", "HCIExt_DisconnectImmed" }, { "0xFC14", "HCIExt_PER" }, { "0xFC8A", "L2CAP_InfoReq" }, { "0xFC92", "L2CAP_ConnParamUpdateReq" }, { "0xFD01", "ATT_ErrorRsp" }, { "0xFD02", "ATT_ExchangeMTUReq" }, { "0xFD03", "ATT_ExchangeMTURsp" }, { "0xFD04", "ATT_FindInfoReq" }, { "0xFD05", "ATT_FindInfoRsp" }, { "0xFD06", "ATT_FindByTypeValueReq" }, { "0xFD07", "ATT_FindByTypeValueRsp" }, { "0xFD08", "ATT_ReadByTypeReq" }, { "0xFD09", "ATT_ReadByTypeRsp" }, { "0xFD0A", "ATT_ReadReq" }, { "0xFD0B", "ATT_ReadRsp" }, 
            { "0xFD0C", "ATT_ReadBlobReq" }, { "0xFD0D", "ATT_ReadBlobRsp" }, { "0xFD0E", "ATT_ReadMultiReq" }, { "0xFD0F", "ATT_ReadMultiRsp" }, { "0xFD10", "ATT_ReadByGrpTypeReq" }, { "0xFD11", "ATT_ReadByGrpTypeRsp" }, { "0xFD12", "ATT_WriteReq" }, { "0xFD13", "ATT_WriteRsp" }, { "0xFD16", "ATT_PrepareWriteReq" }, { "0xFD17", "ATT_PrepareWriteRsp" }, { "0xFD18", "ATT_ExecuteWriteReq" }, { "0xFD19", "ATT_ExecuteWriteRsp" }, { "0xFD1B", "ATT_HandleValueNotification" }, { "0xFD1D", "ATT_HandleValueIndication" }, { "0xFD1E", "ATT_HandleValueConfirmation" }, { "0xFD82", "GATT_ExchangeMTU" }, 
            { "0xFD90", "GATT_DiscAllPrimaryServices" }, { "0xFD86", "GATT_DiscPrimaryServiceByUUID" }, { "0xFDB0", "GATT_FindIncludedServices" }, { "0xFDB2", "GATT_DiscAllChars" }, { "0xFD88", "GATT_DiscCharsByUUID" }, { "0xFD84", "GATT_DiscAllCharDescs" }, { "0xFD8A", "GATT_ReadCharValue" }, { "0xFDB4", "GATT_ReadUsingCharUUID" }, { "0xFD8C", "GATT_ReadLongCharValue" }, { "0xFD8E", "GATT_ReadMultiCharValues" }, { "0xFDB6", "GATT_WriteNoRsp" }, { "0xFDB8", "GATT_SignedWriteNoRsp" }, { "0xFD92", "GATT_WriteCharValue" }, { "0xFD96", "GATT_WriteLongCharValue" }, { "0xFDBA", "GATT_ReliableWrites" }, { "0xFDBC", "GATT_ReadCharDesc" }, 
            { "0xFDBE", "GATT_ReadLongCharDesc" }, { "0xFDC0", "GATT_WriteCharDesc" }, { "0xFDC2", "GATT_WriteLongCharDesc" }, { "0xFD9B", "GATT_Notification" }, { "0xFD9D", "GATT_Indication" }, { "0xFDFC", "GATT_AddService" }, { "0xFDFD", "GATT_DelService" }, { "0xFDFE", "GATT_AddAttribute" }, { "0xFE00", "GAP_DeviceInit" }, { "0xFE03", "GAP_ConfigDeviceAddr" }, { "0xFE04", "GAP_DeviceDiscoveryRequest" }, { "0xFE05", "GAP_DeviceDiscoveryCancel" }, { "0xFE06", "GAP_MakeDiscoverable" }, { "0xFE07", "GAP_UpdateAdvertisingData" }, { "0xFE08", "GAP_EndDiscoverable" }, { "0xFE09", "GAP_EstablishLinkRequest" }, 
            { "0xFE0A", "GAP_TerminateLinkRequest" }, { "0xFE0B", "GAP_Authenticate" }, { "0xFE0C", "GAP_PasskeyUpdate" }, { "0xFE0D", "GAP_SlaveSecurityRequest" }, { "0xFE0E", "GAP_Signable" }, { "0xFE0F", "GAP_Bond" }, { "0xFE10", "GAP_TerminateAuth" }, { "0xFE11", "GAP_UpdateLinkParamReq" }, { "0xFE30", "GAP_SetParam" }, { "0xFE31", "GAP_GetParam" }, { "0xFE32", "GAP_ResolvePrivateAddr" }, { "0xFE33", "GAP_SetAdvToken" }, { "0xFE34", "GAP_RemoveAdvToken" }, { "0xFE35", "GAP_UpdateAdvTokens" }, { "0xFE36", "GAP_BondSetParam" }, { "0xFE37", "GAP_BondGetParam" }, 
            { "0xFE80", "UTIL_Reset" }, { "0xFE81", "UTIL_NVRead" }, { "0xFE82", "UTIL_NVWrite" }, { "0xFE83", "UTIL_ForceBoot" }, { "0x1405", "HCI_ReadRSSI" }, { "0x2010", "HCI_LEClearWhiteList" }, { "0x2011", "HCI_LEAddDeviceToWhiteList" }, { "0x2012", "HCI_LERemoveDeviceFromWhiteList" }, { "0x2013", "HCI_LEConnectionUpdate" }
         };
        public const ushort StartHandleDefault = 1;
        public const string StartHandleDefaultStr = "0x0001";
        private const string strAutoCalc = "This field is auto calculated when the command is sent.";
        private const string strCrLf = "\n";
        private const string strDone = "Done";
        public const string ZeroXStr = "0x";

        public enum ATT_ExecuteWriteFlags
        {
            Cancel_all_prepared_writes,
            Immediately_write_all_pending_prepared_values
        }

        public enum ATT_FindInfoFormat
        {
            HANDLE_BT_UUID_TYPE__handles_and_16_bit_Bluetooth_UUIDs = 1,
            HANDLE_UUID_TYPE__handles_and_128_bit_UUIDs = 2
        }

        public class ATTCmds
        {
            public class ATT_ErrorRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private HCICmds.HCI_ErrorRspCodes _errorCode = HCICmds.HCI_ErrorRspCodes.ATTR_NOT_FOUND;
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private byte _reqOpcode;
                private const byte _reqOpcode_default = 0;
                public string cmdName = "ATT_ErrorRsp";
                public const string constCmdName = "ATT_ErrorRsp";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd01;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ErrorCode (1 Byte) - The reason why the request has generated an error response"), DefaultValue(10)]
                public HCICmds.HCI_ErrorRspCodes errorCode
                {
                    get
                    {
                        return this._errorCode;
                    }
                    set
                    {
                        this._errorCode = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The attribute handle that generated this error response")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("ATT_ErrorRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Req Opcode (1 Byte) - The request that generated this error response"), DefaultValue((byte) 0)]
                public byte reqOpcode
                {
                    get
                    {
                        return this._reqOpcode;
                    }
                    set
                    {
                        this._reqOpcode = value;
                    }
                }
            }

            public class ATT_ExchangeMTUReq
            {
                private ushort _clientRxMTU = 0x17;
                private const string _clientRxMTU_default = "23";
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "ATT_ExchangeMTUReq";
                public const string constCmdName = "ATT_ExchangeMTUReq";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfd02;

                [Description("Client Rx MTU (2 Bytes) - Attribute client receive MTU size"), DefaultValue(typeof(ushort), "23")]
                public ushort clientRxMTU
                {
                    get
                    {
                        return this._clientRxMTU;
                    }
                    set
                    {
                        this._clientRxMTU = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_ExchangeMTUReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ExchangeMTURsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _serverRxMTU = 0x17;
                private const string _serverRxMTU_default = "23";
                public string cmdName = "ATT_ExchangeMTURsp";
                public const string constCmdName = "ATT_ExchangeMTURsp";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfd03;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_ExchangeMTURsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Server Rx MTU (2 Bytes) - Attribute server receive MTU size"), DefaultValue(typeof(ushort), "23")]
                public ushort serverRxMTU
                {
                    get
                    {
                        return this._serverRxMTU;
                    }
                    set
                    {
                        this._serverRxMTU = value;
                    }
                }
            }

            public class ATT_ExecuteWriteReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private HCICmds.ATT_ExecuteWriteFlags _flags;
                public string cmdName = "ATT_ExecuteWriteReq";
                public const string constCmdName = "ATT_ExecuteWriteReq";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfd18;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(0), Description("Flags (1 Byte) - Cancel or Write all values in the queue from this client")]
                public HCICmds.ATT_ExecuteWriteFlags flags
                {
                    get
                    {
                        return this._flags;
                    }
                    set
                    {
                        this._flags = value;
                    }
                }

                [Description("ATT_ExecuteWriteReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ExecuteWriteRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "ATT_ExecuteWriteRsp";
                public const string constCmdName = "ATT_ExecuteWriteRsp";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd19;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_ExecuteWriteRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_FindByTypeValueReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                private string _type = "00:00";
                private string _value = "00:00";
                public string cmdName = "ATT_FindByTypeValueReq";
                public const string constCmdName = "ATT_FindByTypeValueReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd06;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFF"), Description("End Handle (2 Bytes) - The end handle")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("ATT_FindByTypeValueReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - The start handle"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }

                [Description("Type (2 Bytes) - 'XX:XX' The UUID to find"), DefaultValue("00:00")]
                public string type
                {
                    get
                    {
                        return this._type;
                    }
                    set
                    {
                        this._type = value;
                    }
                }

                [Description("Value (x Bytes) - The attribute value to find"), DefaultValue("00:00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_FindByTypeValueRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _handlesInfo = "00:00";
                public string cmdName = "ATT_FindByTypeValueRsp";
                public const string constCmdName = "ATT_FindByTypeValueRsp";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd07;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("Handles Info (1 or more handles info) - 'XX:XX'...'XX:XX'")]
                public string handlesInfo
                {
                    get
                    {
                        return this._handlesInfo;
                    }
                    set
                    {
                        this._handlesInfo = value;
                    }
                }

                [Description("ATT_FindByTypeValueRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_FindInfoReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                public string cmdName = "ATT_FindInfoReq";
                public const string constCmdName = "ATT_FindInfoReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd04;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFF"), Description("End Handle (2 Bytes) - Last requested handle number")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("ATT_FindInfoReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Start Handle (2 Bytes) - First requested handle number")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }
            }

            public class ATT_FindInfoRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private HCICmds.ATT_FindInfoFormat _format = HCICmds.ATT_FindInfoFormat.HANDLE_BT_UUID_TYPE__handles_and_16_bit_Bluetooth_UUIDs;
                private string _info = "00:00:00:00";
                private const string _info_default = "00:00:00:00";
                public string cmdName = "ATT_FindInfoRsp";
                public const string constCmdName = "ATT_FindInfoRsp";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfd05;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Format (1 Byte) - The format of the information data"), DefaultValue(1)]
                public HCICmds.ATT_FindInfoFormat format
                {
                    get
                    {
                        return this._format;
                    }
                    set
                    {
                        this._format = value;
                    }
                }

                [DefaultValue("00:00:00:00"), Description("Info (x Bytes) - The information data whose format is determined by the format field")]
                public string info
                {
                    get
                    {
                        return this._info;
                    }
                    set
                    {
                        this._info = value;
                    }
                }

                [Description("ATT_FindInfoRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_HandleValueConfirmation
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "ATT_HandleValueConfirmation";
                public const string constCmdName = "ATT_HandleValueConfirmation";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd1e;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_HandleValueConfirmation")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_HandleValueIndication
            {
                private HCICmds.GAP_YesNo _authenticated;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "ATT_HandleValueIndication";
                public const string constCmdName = "ATT_HandleValueIndication";
                public byte dataLength = 5;
                public ushort opCodeValue = 0xfd1d;

                [DefaultValue(0), Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
                public HCICmds.GAP_YesNo authenticated
                {
                    get
                    {
                        return this._authenticated;
                    }
                    set
                    {
                        this._authenticated = value;
                    }
                }

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle of the attribute")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("ATT_HandleValueIndication")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes)- The value of the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_HandleValueNotification
            {
                private HCICmds.GAP_YesNo _authenticated;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "ATT_HandleValueNotification";
                public const string constCmdName = "ATT_HandleValueNotification";
                public byte dataLength = 5;
                public ushort opCodeValue = 0xfd1b;

                [DefaultValue(0), Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
                public HCICmds.GAP_YesNo authenticated
                {
                    get
                    {
                        return this._authenticated;
                    }
                    set
                    {
                        this._authenticated = value;
                    }
                }

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("ATT_HandleValueNotification")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The value of the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_PrepareWriteReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                private string _value = "00:00";
                public string cmdName = "ATT_PrepareWriteReq";
                public const string constCmdName = "ATT_PrepareWriteReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd16;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset (2 Bytes) - The offset of the first octet to be written")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("ATT_PrepareWriteReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - Part of the value of the attribute to be written"), DefaultValue("00:00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_PrepareWriteRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                private string _value = "00:00";
                public string cmdName = "ATT_PrepareWriteRsp";
                public const string constCmdName = "ATT_PrepareWriteRsp";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd17;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("Offset (2 Bytes) - The offset of the first octet to be written"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("ATT_PrepareWriteRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - Part of the value of the attribute to be written"), DefaultValue("00:00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_ReadBlobReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                public string cmdName = "ATT_ReadBlobReq";
                public const string constCmdName = "ATT_ReadBlobReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd0c;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset (2 Bytes) - The offset of the first octect to be read")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("ATT_ReadBlobReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ReadBlobRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _value = "00:00";
                public string cmdName = "ATT_ReadBlobRsp";
                public const string constCmdName = "ATT_ReadBlobRsp";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd0d;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_ReadBlobRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The value of the attribute with the handle given"), DefaultValue("00:00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_ReadByGrpTypeReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private string _groupType = "00:00";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                public string cmdName = "ATT_ReadByGrpTypeReq";
                public const string constCmdName = "ATT_ReadByGrpTypeReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd10;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("End Handle (2 Bytes) - The end handle of where values will be read"), DefaultValue(typeof(ushort), "0xFFFF")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("Group Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
                public string groupType
                {
                    get
                    {
                        return this._groupType;
                    }
                    set
                    {
                        this._groupType = value;
                    }
                }

                [Description("ATT_ReadByGrpTypeReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - The start handle where values will be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }
            }

            public class ATT_ReadByGrpTypeRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _dataList = "00:00";
                private byte _length;
                private const byte _length_default = 0;
                public string cmdName = "ATT_ReadByGrpTypeRsp";
                public const string constCmdName = "ATT_ReadByGrpTypeRsp";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfd11;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("DataList (x Bytes) - 'XX:XX...' - A list of Attribute Data (attribute handle, end group handle and attribute value sets)")]
                public string dataList
                {
                    get
                    {
                        return this._dataList;
                    }
                    set
                    {
                        this._dataList = value;
                    }
                }

                [ReadOnly(true), Description("Length (1 Byte) - The size of each Attribute Data (attribute handle, end group handle and attribute value set) This field is auto calculated when the command is sent."), DefaultValue((byte) 0)]
                public byte length
                {
                    get
                    {
                        return this._length;
                    }
                    set
                    {
                        this._length = value;
                    }
                }

                [Description("ATT_ReadByGrpTypeRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ReadByTypeReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                private string _type = "00:00";
                public string cmdName = "ATT_ReadByTypeReq";
                public const string constCmdName = "ATT_ReadByTypeReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd08;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFF"), Description("End Handle (2 Bytes) - The end handle of where values will be read")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("ATT_ReadByTypeReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Start Handle (2 Bytes) - The start handle where values will be read")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
                public string type
                {
                    get
                    {
                        return this._type;
                    }
                    set
                    {
                        this._type = value;
                    }
                }
            }

            public class ATT_ReadByTypeRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _dataList = "00:00";
                private byte _length = 2;
                private const byte _length_default = 2;
                public string cmdName = "ATT_ReadByTypeRsp";
                public const string constCmdName = "ATT_ReadByTypeRsp";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfd09;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("Data List (x Bytes) - A list of Attribute Data (handle-value pairs)")]
                public string dataList
                {
                    get
                    {
                        return this._dataList;
                    }
                    set
                    {
                        this._dataList = value;
                    }
                }

                [Description("Length (1 Byte) - The size of each attribute handle-value pair. This field is auto calculated when the command is sent."), DefaultValue((byte) 2), ReadOnly(true)]
                public byte length
                {
                    get
                    {
                        return this._length;
                    }
                    set
                    {
                        this._length = value;
                    }
                }

                [Description("ATT_ReadByTypeRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ReadMultiReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _handles = "0x0001;0x0002";
                private const string _handles_default = "0x0001;0x0002";
                public string cmdName = "ATT_ReadMultiReq";
                public const string constCmdName = "ATT_ReadMultiReq";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd0e;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handles (2 Bytes for each handle, seperated by ';') - The handles of the attributes"), DefaultValue("0x0001;0x0002")]
                public string handles
                {
                    get
                    {
                        return this._handles;
                    }
                    set
                    {
                        this._handles = value;
                    }
                }

                [Description("ATT_ReadMultiReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ReadMultiRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _values = "00:00";
                public string cmdName = "ATT_ReadMultiRsp";
                public const string constCmdName = "ATT_ReadMultiRsp";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd0f;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_ReadMultiRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Values (x Bytes) - The values of the attribute with the handle given"), DefaultValue("00:00")]
                public string values
                {
                    get
                    {
                        return this._values;
                    }
                    set
                    {
                        this._values = value;
                    }
                }
            }

            public class ATT_ReadReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                public string cmdName = "ATT_ReadReq";
                public const string constCmdName = "ATT_ReadReq";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfd0a;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("ATT_ReadReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class ATT_ReadRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _value = "00:00";
                public string cmdName = "ATT_ReadRsp";
                public const string constCmdName = "ATT_ReadRsp";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd0b;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_ReadRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The value of the attribute with the handle given"), DefaultValue("00:00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_WriteReq
            {
                private HCICmds.GAP_YesNo _command;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private HCICmds.GAP_YesNo _signature;
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "ATT_WriteReq";
                public const string constCmdName = "ATT_WriteReq";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd12;

                [Description("Command (1 Byte) - This is the Write Command"), DefaultValue(0)]
                public HCICmds.GAP_YesNo command
                {
                    get
                    {
                        return this._command;
                    }
                    set
                    {
                        this._command = value;
                    }
                }

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle of the attribute")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("ATT_WriteReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(0), Description("Signature (1 Byte) - Include Authentication Signature")]
                public HCICmds.GAP_YesNo signature
                {
                    get
                    {
                        return this._signature;
                    }
                    set
                    {
                        this._signature = value;
                    }
                }

                [Description("Value (x Bytes)- The value of the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class ATT_WriteRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "ATT_WriteRsp";
                public const string constCmdName = "ATT_WriteRsp";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd13;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("ATT_WriteRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }
        }

        public enum ConnHandle
        {
            All = 0xffff,
            Default = 0xfffe,
            Init = 0xfffe
        }

        public enum GAP_AddrType
        {
            Public,
            Static,
            PrivateNonResolve,
            PrivateResolve
        }

        public enum GAP_AdTypes
        {
            FLAGS = 1,
            LOCAL_NAME_COMPLETE = 9,
            LOCAL_NAME_SHORT = 8,
            MANUFACTURER_SPECIFIC = 0xff,
            OOB_CLASS_OF_DEVICE = 13,
            OOB_SIMPLE_PAIRING_HASHC = 14,
            OOB_SIMPLE_PAIRING_RANDR = 15,
            POWER_LEVEL = 10,
            SERVICE_DATA = 0x16,
            SERVICES_LIST_128BIT = 0x15,
            SERVICES_LIST_16BIT = 20,
            SIGNED_DATA = 0x13,
            SLAVE_CONN_INTERVAL_RANGE = 0x12,
            SM_OOB_FLAG = 0x11,
            SM_TK = 0x10,
            X128BIT_COMPLETE = 7,
            X128BIT_MORE = 6,
            X16BIT_COMPLETE = 3,
            X16BIT_MORE = 2,
            X32BIT_COMPLETE = 5,
            X32BIT_MORE = 4
        }

        public enum GAP_AuthenticatedCsrk
        {
            NOT_AUTHENTICATED,
            AUTHENTICATED
        }

        public enum GAP_AuthReq
        {
            Bonding = 1,
            Man_In_The_Middle = 4
        }

        public enum GAP_AvertAdType
        {
            SCAN_RSP_DATA,
            ADVERTISEMENT_DATA
        }

        public enum GAP_BondParamId
        {
            AUTO_FAIL_PAIRING = 0x40a,
            AUTO_FAIL_REASON = 0x40b,
            AUTO_SYNC_WL = 0x40d,
            BOND_COUNT = 0x40e,
            BONDING_ENABLED = 0x406,
            DEFAULT_PASSCODE = 0x408,
            ERASE_ALLBONDS = 0x409,
            INITIATE_WAIT = 0x401,
            IO_CAPABILITIES = 0x403,
            KEY_DIST_LIST = 0x407,
            KEYSIZE = 0x40c,
            MITM_PROTECTION = 0x402,
            OOB_DATA = 0x405,
            OOB_ENABLED = 0x404,
            PAIRING_MODE = 0x400
        }

        public enum GAP_ChannelMap
        {
            Channel_37,
            Channel_38,
            Channel_39
        }

        public enum GAP_DiscoveryMode
        {
            Nondiscoverable,
            General,
            Limited,
            All
        }

        public enum GAP_EnableDisable
        {
            Disable,
            Enable
        }

        public enum GAP_EventType
        {
            CONN_UNDIRECT_AD,
            CONN_DIRECT_AD,
            SCANABLE_UNDIRECT_AD,
            NON_CONN_UNDIRECT_AD,
            SCAN_RESPONSE
        }

        public enum GAP_FilterPolicy
        {
            All,
            WhiteScan,
            WhiteCon,
            White
        }

        public enum GAP_IOCaps
        {
            DisplayOnly,
            DisplayYesNo,
            KeyboardOnly,
            NoInputNoOutput,
            KeyboardDisplay
        }

        public enum GAP_KeyDisk
        {
            Master_Encryption_Key = 8,
            Master_Identification_Key = 0x10,
            Master_Signing_Key = 0x20,
            Slave_Encryption_Key = 1,
            Slave_Identification_Key = 2,
            Slave_Signing_Key = 4
        }

        public enum GAP_OobDataFlag
        {
            Not_Available,
            Available
        }

        public enum GAP_ParamId
        {
            GET_MEM_USED = 0xff,
            SET_RX_DEBUG = 0xfe,
            TGAP_ATT_TESTCODE = 0x65,
            TGAP_CONN_ADV_INT_MAX = 11,
            TGAP_CONN_ADV_INT_MIN = 10,
            TGAP_CONN_EST_ADV = 20,
            TGAP_CONN_EST_ADV_TIMEOUT = 4,
            TGAP_CONN_EST_INT_MAX = 0x16,
            TGAP_CONN_EST_INT_MIN = 0x15,
            TGAP_CONN_EST_LATENCY = 0x1a,
            TGAP_CONN_EST_MAX_CE_LEN = 0x1c,
            TGAP_CONN_EST_MIN_CE_LEN = 0x1b,
            TGAP_CONN_EST_SCAN_INT = 0x17,
            TGAP_CONN_EST_SCAN_WIND = 0x18,
            TGAP_CONN_EST_SUPERV_TIMEOUT = 0x19,
            TGAP_CONN_HIGH_SCAN_INT = 14,
            TGAP_CONN_HIGH_SCAN_WIND = 15,
            TGAP_CONN_PARAM_TIMEOUT = 5,
            TGAP_CONN_SCAN_INT = 12,
            TGAP_CONN_SCAN_WIND = 13,
            TGAP_FILTER_ADV_REPORTS = 0x21,
            TGAP_GAP_TESTCODE = 0x23,
            TGAP_GATT_TESTCODE = 100,
            TGAP_GEN_DISC_ADV_INT_MAX = 9,
            TGAP_GEN_DISC_ADV_INT_MIN = 8,
            TGAP_GEN_DISC_ADV_MIN = 0,
            TGAP_GEN_DISC_SCAN = 2,
            TGAP_GEN_DISC_SCAN_INT = 0x10,
            TGAP_GEN_DISC_SCAN_WIND = 0x11,
            TGAP_GGS_TESTCODE = 0x66,
            TGAP_LIM_ADV_TIMEOUT = 1,
            TGAP_LIM_DISC_ADV_INT_MAX = 7,
            TGAP_LIM_DISC_ADV_INT_MIN = 6,
            TGAP_LIM_DISC_SCAN = 3,
            TGAP_LIM_DISC_SCAN_INT = 0x12,
            TGAP_LIM_DISC_SCAN_WIND = 0x13,
            TGAP_PRIVATE_ADDR_INT = 0x1d,
            TGAP_SCAN_RSP_RSSI_MIN = 0x22,
            TGAP_SM_MAX_KEY_LEN = 0x20,
            TGAP_SM_MIN_KEY_LEN = 0x1f,
            TGAP_SM_TESTCODE = 0x24,
            TGAP_SM_TIMEOUT = 30
        }

        public enum GAP_Profile
        {
            Broadcaster = 1,
            Central = 8,
            Observer = 2,
            Peripheral = 4
        }

        public enum GAP_SMPFailureTypes
        {
            AUTH_REQ = 3,
            bleTimeout = 0x17,
            CMD_NOT_SUPPORTED = 7,
            CONFIRM_VALUE = 4,
            ENC_KEY_SIZE = 6,
            NOT_SUPPORTED = 5,
            OOB_NOT_AVAIL = 2,
            PASSKEY_ENTRY_FAILED = 1,
            REPEATED_ATTEMPTS = 9,
            SUCCESS = 0,
            UNSPECIFIED = 8
        }

        public enum GAP_TerminationReason
        {
            CONTROL_PKT_INSTANT_PASSED_TERM = 40,
            CONTROL_PKT_TIMEOUT_TERM = 0x22,
            FAILED_TO_ESTABLISH = 0x3e,
            HOST_REQUESTED_TERM = 0x16,
            LSTO_VIOLATION_TERM = 0x3b,
            MAC_CONN_FAILED = 0x3f,
            MIC_FAILURE_TERM = 0x3d,
            PEER_REQUESTED_TERM = 0x13,
            SUPERVISION_TIMEOUT_TERM = 8
        }

        public enum GAP_TrueFalse
        {
            False,
            True
        }

        public enum GAP_UiInput
        {
            DONT_ASK_TO_INPUT_PASSCODE,
            ASK_TO_INPUT_PASSCODE
        }

        public enum GAP_UiOutput
        {
            DONT_DISPLAY_PASSCODE,
            DISPLAY_PASSCODE
        }

        public enum GAP_YesNo
        {
            No,
            Yes
        }

        public class GAPCmds
        {
            public class GAP_Authenticate
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private byte _pairReq_authReq = 1;
                private const byte _pairReq_authReq_default = 1;
                private HCICmds.GAP_EnableDisable _pairReq_Enable;
                private HCICmds.GAP_IOCaps _pairReq_ioCaps = HCICmds.GAP_IOCaps.NoInputNoOutput;
                private byte _pairReq_keyDist = 0x3f;
                private const byte _pairReq_keyDist_default = 0x3f;
                private byte _pairReq_maxEncKeySize = 0x10;
                private const byte _pairReq_maxEncKeySize_default = 0x10;
                private HCICmds.GAP_EnableDisable _pairReq_oobDataFlag;
                private byte _secReq_authReq = 1;
                private HCICmds.GAP_IOCaps _secReq_ioCaps = HCICmds.GAP_IOCaps.NoInputNoOutput;
                private byte _secReq_keyDist = 0x3f;
                private const byte _secReq_keyDist_default = 0x3f;
                private byte _secReq_maxEncKeySize = 0x10;
                private const byte _secReq_maxEncKeySize_default = 0x10;
                private string _secReq_oob = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
                private const string _secReq_oob_default = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
                private HCICmds.GAP_TrueFalse _secReq_oobAvailable;
                public string cmdName = "GAP_Authenticate";
                public const string constCmdName = "GAP_Authenticate";
                public byte dataLength = 0x1d;
                public ushort opCodeValue = 0xfe0b;
                public const byte secReq_oobSize = 0x10;

                [Description("Connection Handle (2 Bytes) - Handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GAP_Authenticate")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue((byte) 1), Description("Auth Req (1 Byte) - Bit field that indicates the requested security properties\nfor STK and GAP bonding information.")]
                public byte pairReq_authReq
                {
                    get
                    {
                        return this._pairReq_authReq;
                    }
                    set
                    {
                        this._pairReq_authReq = value;
                    }
                }

                [Description("Pairing Request (1 Byte) - Enable - if Pairing Request has already been received\nand to respond with a Pairing Response.\n This should only be used in a Peripheral device."), DefaultValue(0)]
                public HCICmds.GAP_EnableDisable pairReq_Enable
                {
                    get
                    {
                        return this._pairReq_Enable;
                    }
                    set
                    {
                        this._pairReq_Enable = value;
                    }
                }

                [DefaultValue(3), Description("IO Capabilities (1 Byte) - Defines the values which are used when exchanging IO capabilities")]
                public HCICmds.GAP_IOCaps pairReq_ioCaps
                {
                    get
                    {
                        return this._pairReq_ioCaps;
                    }
                    set
                    {
                        this._pairReq_ioCaps = value;
                    }
                }

                [Description("Key Dist (1 Byte) - The Key Distribution field indicates which keys will be distributed."), DefaultValue((byte) 0x3f)]
                public byte pairReq_keyDist
                {
                    get
                    {
                        return this._pairReq_keyDist;
                    }
                    set
                    {
                        this._pairReq_keyDist = value;
                    }
                }

                [Description("Max Enc Key Size (1 Byte) - This value defines the maximun encryption key size in octects\nthat the device can support."), DefaultValue((byte) 0x10)]
                public byte pairReq_maxEncKeySize
                {
                    get
                    {
                        return this._pairReq_maxEncKeySize;
                    }
                    set
                    {
                        this._pairReq_maxEncKeySize = value;
                    }
                }

                [Description("OOB data Flag (1 Byte) - Enable if Out-of-band key available"), DefaultValue(0)]
                public HCICmds.GAP_EnableDisable pairReq_oobDataFlag
                {
                    get
                    {
                        return this._pairReq_oobDataFlag;
                    }
                    set
                    {
                        this._pairReq_oobDataFlag = value;
                    }
                }

                [DefaultValue((byte) 1), Description("Auth Req (1 Byte) - A bit field that indicates the requested security properties for STK and GAP bonding information.")]
                public byte secReq_authReq
                {
                    get
                    {
                        return this._secReq_authReq;
                    }
                    set
                    {
                        this._secReq_authReq = value;
                    }
                }

                [DefaultValue(3), Description("IOCaps (1 Byte) - Defines the values which are used when exchanging IO capabilities")]
                public HCICmds.GAP_IOCaps secReq_ioCaps
                {
                    get
                    {
                        return this._secReq_ioCaps;
                    }
                    set
                    {
                        this._secReq_ioCaps = value;
                    }
                }

                [DefaultValue((byte) 0x3f), Description("Key Distribution (1 Byte) - The Key Distribution field indicates which keys will be distributed.")]
                public byte secReq_keyDist
                {
                    get
                    {
                        return this._secReq_keyDist;
                    }
                    set
                    {
                        this._secReq_keyDist = value;
                    }
                }

                [DefaultValue((byte) 0x10), Description("Max Enc Key Size (16 Bytes) - This value defines the maximum encryption key size in octets\nthat the device can support.  Range: 7 to 16.")]
                public byte secReq_maxEncKeySize
                {
                    get
                    {
                        return this._secReq_maxEncKeySize;
                    }
                    set
                    {
                        this._secReq_maxEncKeySize = value;
                    }
                }

                [Description("OOB Key (16 Bytes) The OOB key value"), DefaultValue("4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00")]
                public string secReq_oob
                {
                    get
                    {
                        return this._secReq_oob;
                    }
                    set
                    {
                        this._secReq_oob = value;
                    }
                }

                [Description("OOB Available (1 Byte) - Enable if Out-of-band key available"), DefaultValue(0)]
                public HCICmds.GAP_TrueFalse secReq_oobAvailable
                {
                    get
                    {
                        return this._secReq_oobAvailable;
                    }
                    set
                    {
                        this._secReq_oobAvailable = value;
                    }
                }
            }

            public class GAP_Bond
            {
                private HCICmds.GAP_YesNo _authenticated;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _secInfo_DIV = 0x1111;
                private const string _secInfo_DIV_default = "0x1111";
                private string _secInfo_LTK = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
                private const string _secInfo_LTK_default = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
                private byte _secInfo_LTKSize = 0x10;
                private const byte _secInfo_LTKSize_default = 0x10;
                private string _secInfo_RAND = "11:22:33:44:55:66:77:88";
                private const string _secInfo_RAND_default = "11:22:33:44:55:66:77:88";
                public string cmdName = "GAP_Bond";
                public const string constCmdName = "GAP_Bond";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfe0f;
                public const byte secInfo_LTKLength = 0x10;
                public const byte secInfo_RANDSize = 8;

                [Description("Authenticated (1 Byte) - Yes if the bond was authenticated"), DefaultValue(0)]
                public HCICmds.GAP_YesNo authenticated
                {
                    get
                    {
                        return this._authenticated;
                    }
                    set
                    {
                        this._authenticated = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - Handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GAP_Bond")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(typeof(ushort), "0x1111"), Description("secInfo_DIV (2 Bytes) - Diversifier")]
                public ushort secInfo_DIV
                {
                    get
                    {
                        return this._secInfo_DIV;
                    }
                    set
                    {
                        this._secInfo_DIV = value;
                    }
                }

                [Description("secInfo_LTK (16 Bytes) - Long Term Key"), DefaultValue("4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00")]
                public string secInfo_LTK
                {
                    get
                    {
                        return this._secInfo_LTK;
                    }
                    set
                    {
                        this._secInfo_LTK = value;
                    }
                }

                [DefaultValue((byte) 0x10), Description("secInfo_LTKSize (1 Byte) - LTK Key Size in bytes")]
                public byte secInfo_LTKSize
                {
                    get
                    {
                        return this._secInfo_LTKSize;
                    }
                    set
                    {
                        this._secInfo_LTKSize = value;
                    }
                }

                [Description("secInfo_RAND (8 Bytes) - LTK Random pairing"), DefaultValue("11:22:33:44:55:66:77:88")]
                public string secInfo_RAND
                {
                    get
                    {
                        return this._secInfo_RAND;
                    }
                    set
                    {
                        this._secInfo_RAND = value;
                    }
                }
            }

            public class GAP_BondGetParam
            {
                private HCICmds.GAP_BondParamId _paramId = HCICmds.GAP_BondParamId.PAIRING_MODE;
                public string cmdName = "GAP_BondGetParam";
                public const string constCmdName = "GAP_BondGetParam";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfe37;

                [Description("GAP_BondGetParam")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(0x400), Description("Param Id (1 Byte) GAP Bond Parameter ID")]
                public HCICmds.GAP_BondParamId paramId
                {
                    get
                    {
                        return this._paramId;
                    }
                    set
                    {
                        this._paramId = value;
                    }
                }
            }

            public class GAP_BondSetParam
            {
                private byte _length;
                private const byte _length_default = 0;
                private HCICmds.GAP_BondParamId _paramId = HCICmds.GAP_BondParamId.PAIRING_MODE;
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GAP_BondSetParam";
                public const string constCmdName = "GAP_BondSetParam";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfe36;

                [Description("Param Length (1 Byte) - Length of the parameter. This field is auto calculated when the command is sent."), ReadOnly(true), DefaultValue((byte) 0)]
                public byte length
                {
                    get
                    {
                        return this._length;
                    }
                    set
                    {
                        this._length = value;
                    }
                }

                [Description("GAP_BondSetParam")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Param ID (1 Byte) - GAP Bond Parameter ID"), DefaultValue(0x400)]
                public HCICmds.GAP_BondParamId paramId
                {
                    get
                    {
                        return this._paramId;
                    }
                    set
                    {
                        this._paramId = value;
                    }
                }

                [Description("ParamData (x Bytes) - Param Data Field.  Ex. '02:FF' for 2 bytes"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GAP_ConfigDeviceAddr
            {
                private string _addr = "00:00:00:00:00:00";
                private HCICmds.GAP_AddrType _addrType;
                public string cmdName = "GAP_ConfigDeviceAddr";
                public const string constCmdName = "GAP_ConfigDeviceAddr";
                public byte dataLength = 7;
                public ushort opCodeValue = 0xfe03;

                [DefaultValue("00:00:00:00:00:00"), Description("Addr (6 Bytes) - BDA of the intended address")]
                public string addr
                {
                    get
                    {
                        return this._addr;
                    }
                    set
                    {
                        this._addr = value;
                    }
                }

                [Description("Addr Type (1 Byte) - Address type"), DefaultValue(0)]
                public HCICmds.GAP_AddrType addrType
                {
                    get
                    {
                        return this._addrType;
                    }
                    set
                    {
                        this._addrType = value;
                    }
                }

                [Description("GAP_ConfigDeviceAddr")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_DeviceDiscoveryCancel
            {
                public string cmdName = "GAP_DeviceDiscoveryCancel";
                public const string constCmdName = "GAP_DeviceDiscoveryCancel";
                public byte dataLength;
                public ushort opCodeValue = 0xfe05;

                [Description("GAP_DeviceDiscoveryCancel\nCancel the current device discovery")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_DeviceDiscoveryRequest
            {
                private HCICmds.GAP_EnableDisable _activeScan = HCICmds.GAP_EnableDisable.Enable;
                private HCICmds.GAP_DiscoveryMode _mode = HCICmds.GAP_DiscoveryMode.All;
                private HCICmds.GAP_EnableDisable _whiteList;
                public string cmdName = "GAP_DeviceDiscoveryRequest";
                public const string constCmdName = "GAP_DeviceDiscoveryRequest";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfe04;

                [Category("ActiveScan"), Description("Active Scan (1 Byte) - Active Scan Enable/Disable"), DefaultValue(1)]
                public HCICmds.GAP_EnableDisable activeScan
                {
                    get
                    {
                        return this._activeScan;
                    }
                    set
                    {
                        this._activeScan = value;
                    }
                }

                [Category("Mode"), Description("Mode (1 Byte) - Discovery Mode"), DefaultValue(3)]
                public HCICmds.GAP_DiscoveryMode mode
                {
                    get
                    {
                        return this._mode;
                    }
                    set
                    {
                        this._mode = value;
                    }
                }

                [Description("GAP_DeviceDiscoveryRequest")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("White List (1 byte) - White List Enable/Disable - Enabled to only allow advertisements from devices in the white list."), Category("White List"), DefaultValue(0)]
                public HCICmds.GAP_EnableDisable whiteList
                {
                    get
                    {
                        return this._whiteList;
                    }
                    set
                    {
                        this._whiteList = value;
                    }
                }
            }

            public class GAP_DeviceInit
            {
                private string _csrk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private string _irk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private byte _maxScanResponses = 5;
                private const byte _maxScanResponses_default = 5;
                private HCICmds.GAP_Profile _profileRole = HCICmds.GAP_Profile.Central;
                private uint _signCounter = 1;
                private const string _signCounter_default = "1";
                public string cmdName = "GAP_DeviceInit";
                public const string constCmdName = "GAP_DeviceInit";
                public const byte csrkSize = 0x10;
                public byte dataLength = 6;
                public const byte irkSize = 0x10;
                public ushort opCodeValue = 0xfe00;

                [DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00"), Description("CSRK (16 Bytes) - Connection Signature Resolving Key - 0 if generate the key ")]
                public string csrk
                {
                    get
                    {
                        return this._csrk;
                    }
                    set
                    {
                        this._csrk = value;
                    }
                }

                [Description("IRK (16 Bytes) - Identify Resolving Key - 0 if generate the key "), DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
                public string irk
                {
                    get
                    {
                        return this._irk;
                    }
                    set
                    {
                        this._irk = value;
                    }
                }

                [DefaultValue((byte) 5), Description("Max Scan Responses (1 Byte) - The maximun can responses we can receive during a device discovery.")]
                public byte maxScanResponses
                {
                    get
                    {
                        return this._maxScanResponses;
                    }
                    set
                    {
                        this._maxScanResponses = value;
                    }
                }

                [Description("GAP_DeviceInit")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(8), Description("Profile Role (1 Byte) - Bit Mask - GAP profile role"), Category("ProfileRole")]
                public HCICmds.GAP_Profile profileRole
                {
                    get
                    {
                        return this._profileRole;
                    }
                    set
                    {
                        this._profileRole = value;
                    }
                }

                [Description("Signature Counter (4 Bytes) - 32 bit Signature Counter"), DefaultValue(typeof(uint), "1")]
                public uint signCounter
                {
                    get
                    {
                        return this._signCounter;
                    }
                    set
                    {
                        this._signCounter = value;
                    }
                }
            }

            public class GAP_EndDiscoverable
            {
                public string cmdName = "GAP_EndDiscoverable";
                public const string constCmdName = "GAP_EndDiscoverable";
                public byte dataLength;
                public ushort opCodeValue = 0xfe08;

                [Description("GAP_EndDiscoverable")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_EstablishLinkRequest
            {
                private HCICmds.GAP_AddrType _addrTypePeer;
                private HCICmds.GAP_EnableDisable _highDutyCycle;
                private string _peerAddr = "00:00:00:00:00:00";
                private HCICmds.GAP_EnableDisable _whiteList;
                public string cmdName = "GAP_EstablishLinkRequest";
                public const string constCmdName = "GAP_EstablishLinkRequest";
                public byte dataLength = 9;
                public ushort opCodeValue = 0xfe09;

                [Description("Addr Type (1 Byte) - Address type"), DefaultValue(0)]
                public HCICmds.GAP_AddrType addrTypePeer
                {
                    get
                    {
                        return this._addrTypePeer;
                    }
                    set
                    {
                        this._addrTypePeer = value;
                    }
                }

                [Description("High Duty Cycle (1 Byte) - A Central Device may use high duty cycle scan parameters in order to achieve low latency connection time with a Peripheral device using Directed Link Establishment."), DefaultValue(0)]
                public HCICmds.GAP_EnableDisable highDutyCycle
                {
                    get
                    {
                        return this._highDutyCycle;
                    }
                    set
                    {
                        this._highDutyCycle = value;
                    }
                }

                [Description("GAP_EstablishLinkRequest")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue("00:00:00:00:00:00"), Description("Peer's Address (6 Bytes) - BDA of the peer")]
                public string peerAddr
                {
                    get
                    {
                        return this._peerAddr;
                    }
                    set
                    {
                        this._peerAddr = value;
                    }
                }

                [DefaultValue(0), Description("White List (1 Byte)")]
                public HCICmds.GAP_EnableDisable whiteList
                {
                    get
                    {
                        return this._whiteList;
                    }
                    set
                    {
                        this._whiteList = value;
                    }
                }
            }

            public class GAP_GetParam
            {
                private HCICmds.GAP_ParamId _paramId;
                public string cmdName = "GAP_GetParam";
                public const string constCmdName = "GAP_GetParam";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfe31;

                [Description("GAP_GetParam")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Param ID (1 Byte) - GAP parameter ID"), DefaultValue(0), Category("ParamID")]
                public HCICmds.GAP_ParamId paramId
                {
                    get
                    {
                        return this._paramId;
                    }
                    set
                    {
                        this._paramId = value;
                    }
                }
            }

            public class GAP_MakeDiscoverable
            {
                private byte _channelMap = 7;
                private const byte _channelMap_default = 7;
                private HCICmds.GAP_EventType _eventType;
                private HCICmds.GAP_FilterPolicy _filterPolicy;
                private string _initiatorAddr = "00:00:00:00:00:00";
                private HCICmds.GAP_AddrType _initiatorAddrType;
                public string cmdName = "GAP_MakeDiscoverable";
                public const string constCmdName = "GAP_MakeDiscoverable";
                public byte dataLength = 4;
                public const byte initiatorAddrSize = 6;
                public ushort opCodeValue = 0xfe06;

                [Description("Channel Map (1 Byte) - Bit mask - 0x07 all channels"), DefaultValue((byte) 7)]
                public byte channelMap
                {
                    get
                    {
                        return this._channelMap;
                    }
                    set
                    {
                        this._channelMap = value;
                    }
                }

                [DefaultValue(0), Description("Event Type (1 Byte) - Advertising event type")]
                public HCICmds.GAP_EventType eventType
                {
                    get
                    {
                        return this._eventType;
                    }
                    set
                    {
                        this._eventType = value;
                    }
                }

                [Description("Filter Policy (1 Byte) - Filer Policy. Ignored when directed advertising is used."), DefaultValue(0)]
                public HCICmds.GAP_FilterPolicy filterPolicy
                {
                    get
                    {
                        return this._filterPolicy;
                    }
                    set
                    {
                        this._filterPolicy = value;
                    }
                }

                [Description("Initiator's Address (6 Bytes) - BDA of the Initiator"), DefaultValue("00:00:00:00:00:00")]
                public string initiatorAddr
                {
                    get
                    {
                        return this._initiatorAddr;
                    }
                    set
                    {
                        this._initiatorAddr = value;
                    }
                }

                [Description("Initiator Address Type (1 Byte) - Address type"), DefaultValue(0)]
                public HCICmds.GAP_AddrType initiatorAddrType
                {
                    get
                    {
                        return this._initiatorAddrType;
                    }
                    set
                    {
                        this._initiatorAddrType = value;
                    }
                }

                [Description("GAP_MakeDiscoverable")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_PasskeyUpdate
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _passKey = "000000";
                private const string _passKey_default = "000000";
                public string cmdName = "GAP_PasskeyUpdate";
                public const string constCmdName = "GAP_PasskeyUpdate";
                public byte dataLength = 8;
                public ushort opCodeValue = 0xfe0c;
                public const byte passKeySize = 6;

                [Description("Connection Handle (2 Bytes) - Handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GAP_PasskeyUpdate")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Pairing Passkey (6 Bytes) - string of numbers 0-9. '019655' is a value of 0x4CC7\n"), DefaultValue("000000")]
                public string passKey
                {
                    get
                    {
                        return this._passKey;
                    }
                    set
                    {
                        this._passKey = value;
                    }
                }
            }

            public class GAP_RemoveAdvToken
            {
                private HCICmds.GAP_AdTypes _adType = HCICmds.GAP_AdTypes.FLAGS;
                public string cmdName = "GAP_RemoveAdvToken";
                public const string constCmdName = "GAP_RemoveAdvToken";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfe34;

                [Description("Ad Type (1 Byte) - Advertisement Data Type"), DefaultValue(1)]
                public HCICmds.GAP_AdTypes adType
                {
                    get
                    {
                        return this._adType;
                    }
                    set
                    {
                        this._adType = value;
                    }
                }

                [Description("GAP_RemoveAdvToken")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_ResolvePrivateAddr
            {
                private string _addr = "00:00:00:00:00:00";
                private string _irk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                public const byte addrSize = 6;
                public string cmdName = "GAP_ResolvePrivateAddr";
                public const string constCmdName = "GAP_ResolvePrivateAddr";
                public byte dataLength;
                public const byte irkSize = 0x10;
                public ushort opCodeValue = 0xfe32;

                [DefaultValue("00:00:00:00:00:00"), Description("Address (6 Bytes) - Random Private address to resolve")]
                public string addr
                {
                    get
                    {
                        return this._addr;
                    }
                    set
                    {
                        this._addr = value;
                    }
                }

                [Description("IRK (16 Bytes) - Identity Resolving Key of the device your looking for"), DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
                public string irk
                {
                    get
                    {
                        return this._irk;
                    }
                    set
                    {
                        this._irk = value;
                    }
                }

                [Description("GAP_ResolvePrivateAddr")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_SetAdvToken
            {
                private HCICmds.GAP_AdTypes _adType = HCICmds.GAP_AdTypes.FLAGS;
                private string _advData = "00:00";
                private byte _advDataLen;
                private const byte _advDataLen_default = 0;
                public string cmdName = "GAP_SetAdvToken";
                public const string constCmdName = "GAP_SetAdvToken";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfe33;

                [DefaultValue(1), Description("Ad Type (1 Byte) - Advertisement Data Type")]
                public HCICmds.GAP_AdTypes adType
                {
                    get
                    {
                        return this._adType;
                    }
                    set
                    {
                        this._adType = value;
                    }
                }

                [DefaultValue("00:00"), Description("Adv Data (x Bytes) - Advertisement token data (over-the-air format).")]
                public string advData
                {
                    get
                    {
                        return this._advData;
                    }
                    set
                    {
                        this._advData = value;
                    }
                }

                [ReadOnly(true), Description("Adv Data Len (1 Byte) - Length (in octets) of advData. This field is auto calculated when the command is sent."), DefaultValue((byte) 0)]
                public byte advDataLen
                {
                    get
                    {
                        return this._advDataLen;
                    }
                    set
                    {
                        this._advDataLen = value;
                    }
                }

                [Description("GAP_SetAdvToken")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_SetParam
            {
                private HCICmds.GAP_ParamId _paramId;
                private ushort _value;
                private const string _value_default = "0x0000";
                public string cmdName = "GAP_SetParam";
                public const string constCmdName = "GAP_SetParam";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfe30;

                [Description("GAP_SetParam")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(0), Description("Param Id (1 Byte) - GAP parameter ID")]
                public HCICmds.GAP_ParamId paramId
                {
                    get
                    {
                        return this._paramId;
                    }
                    set
                    {
                        this._paramId = value;
                    }
                }

                [Description("New Value (2 Bytes)"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GAP_Signable
            {
                private HCICmds.GAP_AuthenticatedCsrk _authenticated;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _csrk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private uint _signCounter;
                private const string _signCounter_default = "0";
                public string cmdName = "GAP_Signable";
                public const string constCmdName = "GAP_Signable";
                public const byte csrkSize = 0x10;
                public byte dataLength = 7;
                public ushort opCodeValue = 0xfe0e;

                [DefaultValue(0), Description("Authenticated (1 Byte) - Is the signing information authenticated.")]
                public HCICmds.GAP_AuthenticatedCsrk authenticated
                {
                    get
                    {
                        return this._authenticated;
                    }
                    set
                    {
                        this._authenticated = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - Handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("CSRK (16 Bytes) - Connection Signature Resolving Key for the connected device"), DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
                public string csrk
                {
                    get
                    {
                        return this._csrk;
                    }
                    set
                    {
                        this._csrk = value;
                    }
                }

                [Description("GAP_Signable")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(typeof(uint), "0"), Description("Signature Counter (4 Bytes) - Sign Counter for the connected device")]
                public uint signCounter
                {
                    get
                    {
                        return this._signCounter;
                    }
                    set
                    {
                        this._signCounter = value;
                    }
                }
            }

            public class GAP_SlaveSecurityRequest
            {
                private byte _authReq = 1;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "GAP_SlaveSecurityRequest";
                public const string constCmdName = "GAP_SlaveSecurityRequest";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfe0d;

                [Description("AuthReq (1 Byte) - A bit field that indicates the requested security properties bonding information."), DefaultValue((byte) 1)]
                public byte authReq
                {
                    get
                    {
                        return this._authReq;
                    }
                    set
                    {
                        this._authReq = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - Handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GAP_SlaveSecurityRequest")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_TerminateAuth
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private HCICmds.GAP_SMPFailureTypes _reason = HCICmds.GAP_SMPFailureTypes.AUTH_REQ;
                public string cmdName = "GAP_TerminateAuth";
                public const string constCmdName = "GAP_TerminateAuth";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfe10;

                [Description("Connection Handle (2 Bytes) - Handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GAP_TerminateAuth")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(3), Description("Reason (1 Byte) - Pairing Failed Message reason field.")]
                public HCICmds.GAP_SMPFailureTypes reason
                {
                    get
                    {
                        return this._reason;
                    }
                    set
                    {
                        this._reason = value;
                    }
                }
            }

            public class GAP_TerminateLinkRequest
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "GAP_TerminateLinkRequest";
                public const string constCmdName = "GAP_TerminateLinkRequest";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfe0a;

                [Description("Connection Handle (2 Bytes) - Handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GAP_TerminateLinkRequest")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_UpdateAdvertisingData
            {
                private HCICmds.GAP_AvertAdType _adType = HCICmds.GAP_AvertAdType.ADVERTISEMENT_DATA;
                private string _advertData = "02:01:06";
                private const string _advertData_default = "02:01:06";
                private byte _dataLen;
                private const byte _dataLen_default = 0;
                public string cmdName = "GAP_UpdateAdvertisingData";
                public const string constCmdName = "GAP_UpdateAdvertisingData";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfe07;

                [DefaultValue(1), Description("Ad Type (1 Byte)")]
                public HCICmds.GAP_AvertAdType adType
                {
                    get
                    {
                        return this._adType;
                    }
                    set
                    {
                        this._adType = value;
                    }
                }

                [DefaultValue("02:01:06"), Description("Advert Data (x Bytes) - Raw Advertising Data")]
                public string advertData
                {
                    get
                    {
                        return this._advertData;
                    }
                    set
                    {
                        this._advertData = value;
                    }
                }

                [Description("DataLen (1 Byte) - The length of the data (0 - 31) This field is auto calculated when the command is sent."), DefaultValue((byte) 0), ReadOnly(true)]
                public byte dataLen
                {
                    get
                    {
                        return this._dataLen;
                    }
                    set
                    {
                        this._dataLen = value;
                    }
                }

                [Description("GAP_UpdateAdvertisingData")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_UpdateAdvTokens
            {
                public string cmdName = "GAP_UpdateAdvTokens";
                public const string constCmdName = "GAP_UpdateAdvTokens";
                public byte dataLength;
                public ushort opCodeValue = 0xfe35;

                [Description("GAP_UpdateAdvTokens")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GAP_UpdateLinkParamReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _connLatency;
                private const string _connLatency_default = "0";
                private ushort _connTimeout = 0x3e8;
                private const string _connTimeout_default = "1000";
                private ushort _intervalMax = 160;
                private const string _intervalMax_default = "160";
                private ushort _intervalMin = 80;
                private const string _intervalMin_default = "80";
                public string cmdName = "GAP_UpdateLinkParamReq";
                public const string constCmdName = "GAP_UpdateLinkParamReq";
                public byte dataLength = 10;
                public ushort opCodeValue = 0xfe11;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - Handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0"), Description("ConnLatency (2 Bytes) - The maximum allowed connection latency")]
                public ushort connLatency
                {
                    get
                    {
                        return this._connLatency;
                    }
                    set
                    {
                        this._connLatency = value;
                    }
                }

                [Description("ConnTimeout (2 Bytes) - The link supervision timeout"), DefaultValue(typeof(ushort), "1000")]
                public ushort connTimeout
                {
                    get
                    {
                        return this._connTimeout;
                    }
                    set
                    {
                        this._connTimeout = value;
                    }
                }

                [Description("IntervalMax (2 Bytes) - The maximum allowed connection interval"), DefaultValue(typeof(ushort), "160")]
                public ushort intervalMax
                {
                    get
                    {
                        return this._intervalMax;
                    }
                    set
                    {
                        this._intervalMax = value;
                    }
                }

                [Description("IntervalMin (2 Bytes) - The minimum allowed connection interval"), DefaultValue(typeof(ushort), "80")]
                public ushort intervalMin
                {
                    get
                    {
                        return this._intervalMin;
                    }
                    set
                    {
                        this._intervalMin = value;
                    }
                }

                [Description("GAP_UpdateLinkParamReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }
        }

        public class GAPEvts
        {
            public class GAP_AuthenticationComplete
            {
                private byte _authState;
                private const byte _authState_default = 0;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _devSecInfo_DIV;
                private const string _devSecInfo_DIV_default = "0x0000";
                private HCICmds.GAP_EnableDisable _devSecInfo_enable;
                private string _devSecInfo_LTK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private byte _devSecInfo_LTKsize;
                private const byte _devSecInfo_LTKsize_default = 0;
                private string _devSecInfo_RAND = "00:00:00:00:00:00:00:00";
                private string _idInfo_BdAddr = "00:00:00:00:00:00";
                private HCICmds.GAP_EnableDisable _idInfo_enable;
                private string _idInfo_IRK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private ushort _secInfo_DIV;
                private const string _secInfo_DIV_default = "0x0000";
                private HCICmds.GAP_EnableDisable _secInfo_enable;
                private string _secInfo_LTK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private byte _secInfo_LTKsize;
                private const byte _secInfo_LTKsize_default = 0;
                private string _secInfo_RAND = "00:00:00:00:00:00:00:00";
                private uint _signCounter;
                private const string _signCounter_default = "0x00000000";
                private string _signInfo_CSRK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
                private HCICmds.GAP_EnableDisable _signInfo_enable;
                public string cmdName = "GAP_AuthenticationComplete";
                public const string constCmdName = "GAP_AuthenticationComplete";
                public byte dataLength = 0x11;
                public const int devSecInfo_LTKSize = 0x10;
                public const int devSecInfo_RANDSize = 8;
                public const int idInfo_BdAddrSize = 6;
                public const int idInfo_IRKSize = 0x10;
                public ushort opCodeValue = 0x60a;
                public const int secInfo_LTKSize = 0x10;
                public const int secInfo_RANDSize = 8;
                public const int signInfo_CSRKSize = 0x10;

                [DefaultValue((byte) 0), Description("Auth State (1 Byte)")]
                public byte authState
                {
                    get
                    {
                        return this._authState;
                    }
                    set
                    {
                        this._authState = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Dev Security Info DIV (2 Bytes)"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort devSecInfo_DIV
                {
                    get
                    {
                        return this._devSecInfo_DIV;
                    }
                    set
                    {
                        this._devSecInfo_DIV = value;
                    }
                }

                [Description("Dev Security Info (1 Byte)"), DefaultValue(0)]
                public HCICmds.GAP_EnableDisable devSecInfo_enable
                {
                    get
                    {
                        return this._devSecInfo_enable;
                    }
                    set
                    {
                        this._devSecInfo_enable = value;
                    }
                }

                [DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00"), Description("Dev Security Info LTK (16 Byte)")]
                public string devSecInfo_LTK
                {
                    get
                    {
                        return this._devSecInfo_LTK;
                    }
                    set
                    {
                        this._devSecInfo_LTK = value;
                    }
                }

                [Description("Dev Security Info LTK Size (1 Byte)"), DefaultValue((byte) 0)]
                public byte devSecInfo_LTKsize
                {
                    get
                    {
                        return this._devSecInfo_LTKsize;
                    }
                    set
                    {
                        this._devSecInfo_LTKsize = value;
                    }
                }

                [Description("Dev Security Info RAND (8 Byte)"), DefaultValue("00:00:00:00:00:00:00:00")]
                public string devSecInfo_RAND
                {
                    get
                    {
                        return this._devSecInfo_RAND;
                    }
                    set
                    {
                        this._devSecInfo_RAND = value;
                    }
                }

                [Description("Identity Info BD Address (6 Bytes)"), DefaultValue("00:00:00:00:00:00")]
                public string idInfo_BdAddr
                {
                    get
                    {
                        return this._idInfo_BdAddr;
                    }
                    set
                    {
                        this._idInfo_BdAddr = value;
                    }
                }

                [DefaultValue(0), Description("Identity Info Enable (1 Byte)")]
                public HCICmds.GAP_EnableDisable idInfo_enable
                {
                    get
                    {
                        return this._idInfo_enable;
                    }
                    set
                    {
                        this._idInfo_enable = value;
                    }
                }

                [DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00"), Description("Identity Info IRK (16 Bytes)")]
                public string idInfo_IRK
                {
                    get
                    {
                        return this._idInfo_IRK;
                    }
                    set
                    {
                        this._idInfo_IRK = value;
                    }
                }

                [Description("GAP_AuthenticationComplete")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Security Info DIV (2 Bytes)"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort secInfo_DIV
                {
                    get
                    {
                        return this._secInfo_DIV;
                    }
                    set
                    {
                        this._secInfo_DIV = value;
                    }
                }

                [DefaultValue(0), Description("Security Info Enable (1 Byte)")]
                public HCICmds.GAP_EnableDisable secInfo_enable
                {
                    get
                    {
                        return this._secInfo_enable;
                    }
                    set
                    {
                        this._secInfo_enable = value;
                    }
                }

                [DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00"), Description("Security Info LTK (16 Byte)")]
                public string secInfo_LTK
                {
                    get
                    {
                        return this._secInfo_LTK;
                    }
                    set
                    {
                        this._secInfo_LTK = value;
                    }
                }

                [Description("Security Info LTK Size (1 Byte)"), DefaultValue((byte) 0)]
                public byte secInfo_LTKsize
                {
                    get
                    {
                        return this._secInfo_LTKsize;
                    }
                    set
                    {
                        this._secInfo_LTKsize = value;
                    }
                }

                [Description("Security Info RAND (8 Bytes)"), DefaultValue("00:00:00:00:00:00:00:00")]
                public string secInfo_RAND
                {
                    get
                    {
                        return this._secInfo_RAND;
                    }
                    set
                    {
                        this._secInfo_RAND = value;
                    }
                }

                [Description("Sign Counter (4 Bytes)"), DefaultValue(typeof(uint), "0x00000000")]
                public uint signCounter
                {
                    get
                    {
                        return this._signCounter;
                    }
                    set
                    {
                        this._signCounter = value;
                    }
                }

                [DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00"), Description("Signing Info CSRK (16 Bytes)")]
                public string signInfo_CSRK
                {
                    get
                    {
                        return this._signInfo_CSRK;
                    }
                    set
                    {
                        this._signInfo_CSRK = value;
                    }
                }

                [Description("Signing Info Enable (1 Byte)"), DefaultValue(0)]
                public HCICmds.GAP_EnableDisable signInfo_enable
                {
                    get
                    {
                        return this._signInfo_enable;
                    }
                    set
                    {
                        this._signInfo_enable = value;
                    }
                }
            }
        }

        public enum GATT_Permissions
        {
            AUTHEN_READ = 4,
            AUTHEN_WRITE = 8,
            AUTHOR_READ = 0x10,
            AUTHOR_WRITE = 0x20,
            READ = 1,
            WRITE = 2
        }

        public enum GATT_ServiceUUID
        {
            PrimaryService = 0x2800,
            SecondaryService = 0x2801
        }

        public class GATTCmds
        {
            public class GATT_AddAttribute
            {
                private byte _permissions = 1;
                private const byte _permissions_default = 1;
                private string _uuid = "00:00";
                public string cmdName = "GATT_AddAttribute";
                public const string constCmdName = "GATT_AddAttribute";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfdfe;

                [Description("GATT_AddAttribute")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue((byte) 1), Description("Permissions (1 Byte) - Bit mask - Attribute permissions")]
                public byte permissions
                {
                    get
                    {
                        return this._permissions;
                    }
                    set
                    {
                        this._permissions = value;
                    }
                }

                [DefaultValue("00:00"), Description("UUID (x Bytes) - The type of the attribute to be added")]
                public string uuid
                {
                    get
                    {
                        return this._uuid;
                    }
                    set
                    {
                        this._uuid = value;
                    }
                }
            }

            public class GATT_AddService
            {
                private ushort _numAttrs = 2;
                private const string _numAttrs_default = "2";
                private HCICmds.GATT_ServiceUUID _uuid = HCICmds.GATT_ServiceUUID.PrimaryService;
                public string cmdName = "GATT_AddService";
                public const string constCmdName = "GATT_AddService";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfdfc;

                [Description("Num Attrs (2 Bytes) - The number attributes in the service (including the service attribute)"), DefaultValue(typeof(ushort), "2")]
                public ushort numAttrs
                {
                    get
                    {
                        return this._numAttrs;
                    }
                    set
                    {
                        this._numAttrs = value;
                    }
                }

                [Description("GATT_AddService")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(0x2800), Description("UUID (2 Bytes)")]
                public HCICmds.GATT_ServiceUUID uuid
                {
                    get
                    {
                        return this._uuid;
                    }
                    set
                    {
                        this._uuid = value;
                    }
                }
            }

            public class GATT_DelService
            {
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                public string cmdName = "GATT_DelService";
                public const string constCmdName = "GATT_DelService";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfdfd;

                [Description("Handle (2 Bytes) - The handle of the service to be deleted"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_DelService")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_DiscAllCharDescs
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                public string cmdName = "GATT_DiscAllCharDescs";
                public const string constCmdName = "GATT_DiscAllCharDescs";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd84;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("End Handle (2 Bytes) - Last requested handle number"), DefaultValue(typeof(ushort), "0xFFFF")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("GATT_DiscAllCharDescs")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - First requested handle number"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }
            }

            public class GATT_DiscAllChars
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                public string cmdName = "GATT_DiscAllChars";
                public const string constCmdName = "GATT_DiscAllChars";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfdb2;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("End Handle (2 Bytes) - Last requested handle number"), DefaultValue(typeof(ushort), "0xFFFF")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("GATT_DiscAllChars")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - First requested handle number"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }
            }

            public class GATT_DiscAllPrimaryServices
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "GATT_DiscAllPrimaryServices";
                public const string constCmdName = "GATT_DiscAllPrimaryServices";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd90;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GATT_DiscAllPrimaryServices")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_DiscCharsByUUID
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                private string _type = "00:00";
                public string cmdName = "GATT_DiscCharsByUUID";
                public const string constCmdName = "GATT_DiscCharsByUUID";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd88;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFF"), Description("End Handle (2 Bytes) - Last requested handle number")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("GATT_DiscCharsByUUID")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - First requested handle number"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
                public string type
                {
                    get
                    {
                        return this._type;
                    }
                    set
                    {
                        this._type = value;
                    }
                }
            }

            public class GATT_DiscPrimaryServiceByUUID
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_DiscPrimaryServiceByUUID";
                public const string constCmdName = "GATT_DiscPrimaryServiceByUUID";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd86;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GATT_DiscPrimaryServiceByUUID")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - Attribute Value To Find"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_ExchangeMTU
            {
                private ushort _clientRxMTU = 0x17;
                private const string _clientRxMTU_default = "23";
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "GATT_ExchangeMTU";
                public const string constCmdName = "GATT_ExchangeMTU";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfd82;

                [DefaultValue(typeof(ushort), "23"), Description("clientRxMTU (2 Bytes) - Attribute client receive MTU size")]
                public ushort clientRxMTU
                {
                    get
                    {
                        return this._clientRxMTU;
                    }
                    set
                    {
                        this._clientRxMTU = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("GATT_ExchangeMTU")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_FindIncludedServices
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                public string cmdName = "GATT_FindIncludedServices";
                public const string constCmdName = "GATT_FindIncludedServices";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfdb0;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("End Handle (2 Bytes) - Last requested handle number"), DefaultValue(typeof(ushort), "0xFFFF")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("GATT_FindIncludedServices")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - First requested handle number"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }
            }

            public class GATT_Indication
            {
                private HCICmds.GAP_YesNo _authenticated;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_Indication";
                public const string constCmdName = "GATT_Indication";
                public byte dataLength = 5;
                public ushort opCodeValue = 0xfd9d;

                [Description("Authenticated (1 Byte) - Whether or not an authenticated link is required"), DefaultValue(0)]
                public HCICmds.GAP_YesNo authenticated
                {
                    get
                    {
                        return this._authenticated;
                    }
                    set
                    {
                        this._authenticated = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle of the attribute")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_Indication")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The current value of the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_Notification
            {
                private HCICmds.GAP_YesNo _authenticated;
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_Notification";
                public const string constCmdName = "GATT_Notification";
                public byte dataLength = 5;
                public ushort opCodeValue = 0xfd9b;

                [DefaultValue(0), Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
                public HCICmds.GAP_YesNo authenticated
                {
                    get
                    {
                        return this._authenticated;
                    }
                    set
                    {
                        this._authenticated = value;
                    }
                }

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle of the attribute")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_Notification")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The current value of the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_ReadCharDesc
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                public string cmdName = "GATT_ReadCharDesc";
                public const string constCmdName = "GATT_ReadCharDesc";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfdbc;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_ReadCharDesc")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_ReadCharValue
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                public string cmdName = "GATT_ReadCharValue";
                public const string constCmdName = "GATT_ReadCharValue";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfd8a;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle of the attribute to be read")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_ReadCharValue")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_ReadLongCharDesc
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                public string cmdName = "GATT_ReadLongCharDesc";
                public const string constCmdName = "GATT_ReadLongCharDesc";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfdbe;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle of the attribute to be read")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("Offset (2 Bytes) - The offset of the first octet to be read"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("GATT_ReadLongCharDesc")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_ReadLongCharValue
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                public string cmdName = "GATT_ReadLongCharValue";
                public const string constCmdName = "GATT_ReadLongCharValue";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd8c;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset (2 Bytes) - The offset of the first octect to be read")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("GATT_ReadLongCharValue")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_ReadMultiCharValues
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private string _handles = "0x0001;0x0002";
                private const string _handles_default = "0x0001;0x0002";
                public string cmdName = "GATT_ReadMultiCharValues";
                public const string constCmdName = "GATT_ReadMultiCharValues";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfd8e;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handles (2 Bytes for each handle, seperated by ';') - The handles of the attributes"), DefaultValue("0x0001;0x0002")]
                public string handles
                {
                    get
                    {
                        return this._handles;
                    }
                    set
                    {
                        this._handles = value;
                    }
                }

                [Description("GATT_ReadMultiCharValues")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class GATT_ReadUsingCharUUID
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _endHandle = 0xffff;
                private const string _endHandle_default = "0xFFFF";
                private ushort _startHandle = 1;
                private const string _startHandle_default = "0x0001";
                private string _type = "00:00";
                public string cmdName = "GATT_ReadUsingCharUUID";
                public const string constCmdName = "GATT_ReadUsingCharUUID";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfdb4;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("End Handle (2 Bytes) - The end handle of where values will be read"), DefaultValue(typeof(ushort), "0xFFFF")]
                public ushort endHandle
                {
                    get
                    {
                        return this._endHandle;
                    }
                    set
                    {
                        this._endHandle = value;
                    }
                }

                [Description("GATT_ReadUsingCharUUID")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Start Handle (2 Bytes) - The start handle where values will be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort startHandle
                {
                    get
                    {
                        return this._startHandle;
                    }
                    set
                    {
                        this._startHandle = value;
                    }
                }

                [DefaultValue("00:00"), Description("Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
                public string type
                {
                    get
                    {
                        return this._type;
                    }
                    set
                    {
                        this._type = value;
                    }
                }
            }

            public class GATT_ReliableWrites
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private const ushort _handle_default = 1;
                private const string _handle1_default = "0x0001";
                private const string _handle2_default = "0x0001";
                private const string _handle3_default = "0x0001";
                private const string _handle4_default = "0x0001";
                private const string _handle5_default = "0x0001";
                private byte _numRequests;
                private const string _numRequests_default = "0";
                private const ushort _offset_default = 0;
                private const string _offset1_default = "0x0000";
                private const string _offset2_default = "0x0000";
                private const string _offset3_default = "0x0000";
                private const string _offset4_default = "0x0000";
                private const string _offset5_default = "0x0000";
                private const string _value_default = "";
                private const byte _valueLen_default = 0;
                private const string _valueLen_default_str = "0";
                private const string _valueLen1_default = "0";
                private const string _valueLen2_default = "0";
                private const string _valueLen3_default = "0";
                private const string _valueLen4_default = "0";
                private const string _valueLen5_default = "0";
                public string cmdName = "GATT_ReliableWrites";
                public const string constCmdName = "GATT_ReliableWrites";
                public byte dataLength = 3;
                public const int maxElements = 5;
                public ushort opCodeValue = 0xfdba;
                public WriteElement[] writeElement;

                public GATT_ReliableWrites()
                {
                    WriteElement[] elementArray = new WriteElement[5];
                    WriteElement element = new WriteElement();
                    element.valueLen = 0;
                    element.handle = 1;
                    element.offset = 0;
                    element.value = "";
                    elementArray[0] = element;
                    WriteElement element2 = new WriteElement();
                    element2.valueLen = 0;
                    element2.handle = 1;
                    element2.offset = 0;
                    element2.value = "";
                    elementArray[1] = element2;
                    WriteElement element3 = new WriteElement();
                    element3.valueLen = 0;
                    element3.handle = 1;
                    element3.offset = 0;
                    element3.value = "";
                    elementArray[2] = element3;
                    WriteElement element4 = new WriteElement();
                    element4.valueLen = 0;
                    element4.handle = 1;
                    element4.offset = 0;
                    element4.value = "";
                    elementArray[3] = element4;
                    WriteElement element5 = new WriteElement();
                    element5.valueLen = 0;
                    element5.handle = 1;
                    element5.offset = 0;
                    element5.value = "";
                    elementArray[4] = element5;
                    this.writeElement = elementArray;
                }

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle1 (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle1
                {
                    get
                    {
                        return this.writeElement[0].handle;
                    }
                    set
                    {
                        this.writeElement[0].handle = value;
                    }
                }

                [Description("Handle2 (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle2
                {
                    get
                    {
                        return this.writeElement[1].handle;
                    }
                    set
                    {
                        this.writeElement[1].handle = value;
                    }
                }

                [Description("Handle3 (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle3
                {
                    get
                    {
                        return this.writeElement[2].handle;
                    }
                    set
                    {
                        this.writeElement[2].handle = value;
                    }
                }

                [Description("Handle4 (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle4
                {
                    get
                    {
                        return this.writeElement[3].handle;
                    }
                    set
                    {
                        this.writeElement[3].handle = value;
                    }
                }

                [Description("Handle5 (2 Bytes) - The handle of the attribute to be written"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle5
                {
                    get
                    {
                        return this.writeElement[4].handle;
                    }
                    set
                    {
                        this.writeElement[4].handle = value;
                    }
                }

                [Description("Num Requests (1 Bytes) - Number of Prepare Write Requests"), DefaultValue(typeof(byte), "0")]
                public byte numRequests
                {
                    get
                    {
                        return this._numRequests;
                    }
                    set
                    {
                        this._numRequests = value;
                    }
                }

                [Description("Offset1 (2 Bytes) - The offset of the first octet to be written"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort offset1
                {
                    get
                    {
                        return this.writeElement[0].offset;
                    }
                    set
                    {
                        this.writeElement[0].offset = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset2 (2 Bytes) - The offset of the first octet to be written")]
                public ushort offset2
                {
                    get
                    {
                        return this.writeElement[1].offset;
                    }
                    set
                    {
                        this.writeElement[1].offset = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset3 (2 Bytes) - The offset of the first octet to be written")]
                public ushort offset3
                {
                    get
                    {
                        return this.writeElement[2].offset;
                    }
                    set
                    {
                        this.writeElement[2].offset = value;
                    }
                }

                [Description("Offset4 (2 Bytes) - The offset of the first octet to be written"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort offset4
                {
                    get
                    {
                        return this.writeElement[3].offset;
                    }
                    set
                    {
                        this.writeElement[3].offset = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset5 (2 Bytes) - The offset of the first octet to be written")]
                public ushort offset5
                {
                    get
                    {
                        return this.writeElement[4].offset;
                    }
                    set
                    {
                        this.writeElement[4].offset = value;
                    }
                }

                [Description("GATT_ReliableWrites")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(""), Description("Value1 (x Bytes)- The value to be written to the attribute")]
                public string value1
                {
                    get
                    {
                        return this.writeElement[0].value;
                    }
                    set
                    {
                        this.writeElement[0].value = value;
                    }
                }

                [DefaultValue(""), Description("Value2 (x Bytes)- The value to be written to the attribute")]
                public string value2
                {
                    get
                    {
                        return this.writeElement[1].value;
                    }
                    set
                    {
                        this.writeElement[1].value = value;
                    }
                }

                [DefaultValue(""), Description("Value3 (x Bytes)- The value to be written to the attribute")]
                public string value3
                {
                    get
                    {
                        return this.writeElement[2].value;
                    }
                    set
                    {
                        this.writeElement[2].value = value;
                    }
                }

                [DefaultValue(""), Description("Value4 (x Bytes)- The value to be written to the attribute")]
                public string value4
                {
                    get
                    {
                        return this.writeElement[3].value;
                    }
                    set
                    {
                        this.writeElement[3].value = value;
                    }
                }

                [Description("Value5 (x Bytes)- The value to be written to the attribute"), DefaultValue("")]
                public string value5
                {
                    get
                    {
                        return this.writeElement[4].value;
                    }
                    set
                    {
                        this.writeElement[4].value = value;
                    }
                }

                [DefaultValue(typeof(byte), "0"), Description("Value Len1 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent."), ReadOnly(true)]
                public byte valueLen1
                {
                    get
                    {
                        return this.writeElement[0].valueLen;
                    }
                    set
                    {
                        this.writeElement[0].valueLen = value;
                    }
                }

                [Description("Value Len2 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent."), DefaultValue(typeof(byte), "0"), ReadOnly(true)]
                public byte valueLen2
                {
                    get
                    {
                        return this.writeElement[1].valueLen;
                    }
                    set
                    {
                        this.writeElement[1].valueLen = value;
                    }
                }

                [ReadOnly(true), Description("Value Len3 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent."), DefaultValue(typeof(byte), "0")]
                public byte valueLen3
                {
                    get
                    {
                        return this.writeElement[2].valueLen;
                    }
                    set
                    {
                        this.writeElement[2].valueLen = value;
                    }
                }

                [ReadOnly(true), Description("Value Len4 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent."), DefaultValue(typeof(byte), "0")]
                public byte valueLen4
                {
                    get
                    {
                        return this.writeElement[3].valueLen;
                    }
                    set
                    {
                        this.writeElement[3].valueLen = value;
                    }
                }

                [ReadOnly(true), Description("Value Len5 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent."), DefaultValue(typeof(byte), "0")]
                public byte valueLen5
                {
                    get
                    {
                        return this.writeElement[4].valueLen;
                    }
                    set
                    {
                        this.writeElement[4].valueLen = value;
                    }
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct WriteElement
                {
                    public byte valueLen;
                    public ushort handle;
                    public ushort offset;
                    public string value;
                }
            }

            public class GATT_SignedWriteNoRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_SignedWriteNoRsp";
                public const string constCmdName = "GATT_SignedWriteNoRsp";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfdb8;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be set"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_SignedWriteNoRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes)- The value to be written to the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_WriteCharDesc
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_WriteCharDesc";
                public const string constCmdName = "GATT_WriteCharDesc";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfdc0;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Offset (2 Bytes) - The offset of the first octet to be read"), DefaultValue(typeof(ushort), "0x0000")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("GATT_WriteCharDesc")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The value of the attribute to be written"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_WriteCharValue
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_WriteCharValue";
                public const string constCmdName = "GATT_WriteCharValue";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfd92;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be set"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_WriteCharValue")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes)- The value to be written to the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_WriteLongCharDesc
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_WriteLongCharDesc";
                public const string constCmdName = "GATT_WriteLongCharDesc";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfdc2;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be read"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset (2 Bytes) - The offset of the first octet to be read")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("GATT_WriteLongCharDesc")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes) - The current value of the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_WriteLongCharValue
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _offset;
                private const string _offset_default = "0x0000";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_WriteLongCharValue";
                public const string constCmdName = "GATT_WriteLongCharValue";
                public byte dataLength = 6;
                public ushort opCodeValue = 0xfd96;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be set"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("Offset (2 Bytes) - The offset of the first octet to be written")]
                public ushort offset
                {
                    get
                    {
                        return this._offset;
                    }
                    set
                    {
                        this._offset = value;
                    }
                }

                [Description("GATT_WriteLongCharValue")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes)- The value to be written to the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }

            public class GATT_WriteNoRsp
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private string _value = "00";
                private const string _value_default = "00";
                public string cmdName = "GATT_WriteNoRsp";
                public const string constCmdName = "GATT_WriteNoRsp";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfdb6;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Handle (2 Bytes) - The handle of the attribute to be set"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("GATT_WriteNoRsp")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Value (x Bytes)- The value to be written to the attribute"), DefaultValue("00")]
                public string value
                {
                    get
                    {
                        return this._value;
                    }
                    set
                    {
                        this._value = value;
                    }
                }
            }
        }

        public enum HCI_ErrorRspCodes
        {
            ATTR_NOT_FOUND = 10,
            ATTR_NOT_LONG = 11,
            INSUFFICIENT_AUTHEN = 5,
            INSUFFICIENT_AUTHOR = 8,
            INSUFFICIENT_ENCRYPTION = 15,
            INSUFFICIENT_KEY_SIZE = 12,
            INSUFFICIENT_RESOURCES = 0x11,
            INVALID_HANDLE = 1,
            INVALID_OFFSET = 7,
            INVALID_PDU = 4,
            INVALID_SIZE = 13,
            INVALID_VALUE = 0x80,
            PREPARE_QUEUE_FULL = 9,
            READ_NOT_PERMITTED = 2,
            UNLIKELY_ERROR = 14,
            UNSUPPORTED_GRP_TYPE = 0x10,
            UNSUPPORTED_REQ = 6,
            WRITE_NOT_PERMITTED = 3
        }

        public enum HCI_LEAddressType
        {
            Public_Device,
            Random_Device
        }

        public enum HCI_StatusCodes
        {
            bleAlreadyInRequestedMode = 0x11,
            bleGAPBondRejected = 50,
            bleGAPConnNotAcceptable = 0x31,
            bleGAPUserCanceled = 0x30,
            bleIncorrectMode = 0x12,
            bleInsufficientAuthen = 0x41,
            bleInsufficientEncrypt = 0x42,
            bleInsufficientKeySize = 0x43,
            bleInvalidPDU = 0x40,
            bleInvalidRange = 0x18,
            bleLinkEncrypted = 0x19,
            bleMemAllocError = 0x13,
            bleNoResources = 0x15,
            bleNotConnected = 20,
            bleNotReady = 0x10,
            blePending = 0x16,
            bleProcedureComplete = 0x1a,
            bleTimeout = 0x17,
            ErrorCommandDisallowed = 12,
            Failure = 1,
            INVALID_TASK_ID = 0xff,
            InvalidEventId = 6,
            InvalidInteruptId = 7,
            InvalidMemSize = 11,
            InvalidMsgPointer = 5,
            InvalidParameter = 2,
            InvalidTask = 3,
            MsgBufferNotAvailable = 4,
            NoTimerAvail = 8,
            NVItemUnInit = 9,
            NVOpFailed = 10,
            Success = 0
        }

        public enum HCICmdOpcode
        {
            ATT_ErrorRsp = 0xfd01,
            ATT_ExchangeMTUReq = 0xfd02,
            ATT_ExchangeMTURsp = 0xfd03,
            ATT_ExecuteWriteReq = 0xfd18,
            ATT_ExecuteWriteRsp = 0xfd19,
            ATT_FindByTypeValueReq = 0xfd06,
            ATT_FindByTypeValueRsp = 0xfd07,
            ATT_FindInfoReq = 0xfd04,
            ATT_FindInfoRsp = 0xfd05,
            ATT_HandleValueConfirmation = 0xfd1e,
            ATT_HandleValueIndication = 0xfd1d,
            ATT_HandleValueNotification = 0xfd1b,
            ATT_PrepareWriteReq = 0xfd16,
            ATT_PrepareWriteRsp = 0xfd17,
            ATT_ReadBlobReq = 0xfd0c,
            ATT_ReadBlobRsp = 0xfd0d,
            ATT_ReadByGrpTypeReq = 0xfd10,
            ATT_ReadByGrpTypeRsp = 0xfd11,
            ATT_ReadByTypeReq = 0xfd08,
            ATT_ReadByTypeRsp = 0xfd09,
            ATT_ReadMultiReq = 0xfd0e,
            ATT_ReadMultiRsp = 0xfd0f,
            ATT_ReadReq = 0xfd0a,
            ATT_ReadRsp = 0xfd0b,
            ATT_WriteReq = 0xfd12,
            ATT_WriteRsp = 0xfd13,
            GAP_Authenticate = 0xfe0b,
            GAP_Bond = 0xfe0f,
            GAP_BondGetParam = 0xfe37,
            GAP_BondSetParam = 0xfe36,
            GAP_ConfigDeviceAddr = 0xfe03,
            GAP_DeviceDiscoveryCancel = 0xfe05,
            GAP_DeviceDiscoveryRequest = 0xfe04,
            GAP_DeviceInit = 0xfe00,
            GAP_EndDiscoverable = 0xfe08,
            GAP_EstablishLinkRequest = 0xfe09,
            GAP_GetParam = 0xfe31,
            GAP_MakeDiscoverable = 0xfe06,
            GAP_PasskeyUpdate = 0xfe0c,
            GAP_RemoveAdvToken = 0xfe34,
            GAP_ResolvePrivateAddr = 0xfe32,
            GAP_SetAdvToken = 0xfe33,
            GAP_SetParam = 0xfe30,
            GAP_Signable = 0xfe0e,
            GAP_SlaveSecurityRequest = 0xfe0d,
            GAP_TerminateAuth = 0xfe10,
            GAP_TerminateLinkRequest = 0xfe0a,
            GAP_UpdateAdvertisingData = 0xfe07,
            GAP_UpdateAdvTokens = 0xfe35,
            GAP_UpdateLinkParamReq = 0xfe11,
            GATT_AddAttribute = 0xfdfe,
            GATT_AddService = 0xfdfc,
            GATT_DelService = 0xfdfd,
            GATT_DiscAllCharDescs = 0xfd84,
            GATT_DiscAllChars = 0xfdb2,
            GATT_DiscAllPrimaryServices = 0xfd90,
            GATT_DiscCharsByUUID = 0xfd88,
            GATT_DiscPrimaryServiceByUUID = 0xfd86,
            GATT_ExchangeMTU = 0xfd82,
            GATT_FindIncludedServices = 0xfdb0,
            GATT_Indication = 0xfd9d,
            GATT_Notification = 0xfd9b,
            GATT_ReadCharDesc = 0xfdbc,
            GATT_ReadCharValue = 0xfd8a,
            GATT_ReadLongCharDesc = 0xfdbe,
            GATT_ReadLongCharValue = 0xfd8c,
            GATT_ReadMultiCharValues = 0xfd8e,
            GATT_ReadUsingCharUUID = 0xfdb4,
            GATT_ReliableWrites = 0xfdba,
            GATT_SignedWriteNoRsp = 0xfdb8,
            GATT_WriteCharDesc = 0xfdc0,
            GATT_WriteCharValue = 0xfd92,
            GATT_WriteLongCharDesc = 0xfdc2,
            GATT_WriteLongCharValue = 0xfd96,
            GATT_WriteNoRsp = 0xfdb6,
            HCIExt_ClkDivideOnHalt = 0xfc03,
            HCIExt_DeclareNvUsage = 0xfc04,
            HCIExt_Decrypt = 0xfc05,
            HCIExt_DisconnectImmed = 0xfc13,
            HCIExt_EnablePTM = 0xfc0e,
            HCIExt_EndModemTest = 0xfc0b,
            HCIExt_MapPmIoPort = 0xfc12,
            HCIExt_ModemHopTestTx = 0xfc09,
            HCIExt_ModemTestRx = 0xfc0a,
            HCIExt_ModemTestTx = 0xfc08,
            HCIExt_OnePktPerEvt = 0xfc02,
            HCIExt_PER = 0xfc14,
            HCIExt_SaveFreqTune = 0xfc10,
            HCIExt_SetBDADDR = 0xfc0c,
            HCIExt_SetFastTxRespTime = 0xfc07,
            HCIExt_SetFreqTune = 0xfc0f,
            HCIExt_SetLocalSupportedFeatures = 0xfc06,
            HCIExt_SetMaxDtmTxPower = 0xfc11,
            HCIExt_SetRxGain = 0xfc00,
            HCIExt_SetSCA = 0xfc0d,
            HCIExt_SetTxPower = 0xfc01,
            HCIOther_LEAddDeviceToWhiteList = 0x2011,
            HCIOther_LEClearWhiteList = 0x2010,
            HCIOther_LEConnectionUpdate = 0x2013,
            HCIOther_LERemoveDeviceFromWhiteList = 0x2012,
            HCIOther_ReadRSSI = 0x1405,
            InvalidCommandCode = 0,
            L2CAP_ConnParamUpdateReq = 0xfc92,
            L2CAP_InfoReq = 0xfc8a,
            UTIL_ForceBoot = 0xfe83,
            UTIL_NVRead = 0xfe81,
            UTIL_NVWrite = 0xfe82,
            UTIL_Reset = 0xfe80
        }

        public enum HCIEvtCode
        {
            HCI_AMP_ReceiverReportEvent = 0x4b,
            HCI_AMP_StartTestEvent = 0x49,
            HCI_AMP_StatusChangeEvent = 0x4d,
            HCI_AMP_TestEndEvent = 0x4a,
            HCI_AuthenticationCompleteEvent = 6,
            HCI_ChangeConnectionLinkKeyCompleteEvent = 9,
            HCI_ChannelSelectedEvent = 0x41,
            HCI_CommandCompleteEvent = 14,
            HCI_CommandStatusEvent = 15,
            HCI_ConnectionCompleteEvent = 3,
            HCI_ConnectionPacketTypeChangedEvent = 0x1d,
            HCI_ConnectionRequestEvent = 4,
            HCI_DataBufferOverflowEvent = 0x1a,
            HCI_DisconnectionCompleteEvent = 5,
            HCI_DisconnectionLogicalLinkCompleteEvent = 70,
            HCI_DisconnectionPhysicalLinkCompleteEvent = 0x42,
            HCI_EncryptionChangeEvent = 8,
            HCI_EncryptionKeyRefreshCompleteEvent = 0x30,
            HCI_EnhancedFlushCompleteEvent = 0x39,
            HCI_ExtendedInquiryResultEvent = 0x2f,
            HCI_FlowSpecificationCompleteEvent = 0x21,
            HCI_FlowSpecModifyCompleteEvent = 0x47,
            HCI_FlushOccurredEvent = 0x11,
            HCI_HardwareErrorEvent = 0x10,
            HCI_InquiryCompleteEvent = 1,
            HCI_InquiryResultEvent = 2,
            HCI_InquiryResultWithRSSIEvent = 0x22,
            HCI_IOCapabilityRequestEvent = 0x31,
            HCI_IOCapabilityResponseEvent = 50,
            HCI_KeypressNotificationEvent = 60,
            HCI_LE_ExtEvent = 0xff,
            HCI_LE_SpecialSubEvent = 0x3e,
            HCI_LinkKeyNotificationEvent = 0x18,
            HCI_LinkKeyRequestEvent = 0x17,
            HCI_LinkSupervisionTimeoutChangedEvent = 0x38,
            HCI_LogicalLinkCompleteEvent = 0x45,
            HCI_LoopbackCommandEvent = 0x19,
            HCI_MasterLinkKeyCompleteEvent = 10,
            HCI_MaxSlotsChangeEvent = 0x1b,
            HCI_ModeChangeEvent = 20,
            HCI_NumberOfCompletedDataBlocksEvent = 0x48,
            HCI_NumberOfCompletedPacketsEvent = 0x13,
            HCI_PageScanModeChangeEvent = 0x1f,
            HCI_PageScanRepetitionModeChangeEvent = 0x20,
            HCI_PhysicalLinkCompleteEvent = 0x40,
            HCI_PhysicalLinkLossEarlyWarningEvent = 0x43,
            HCI_PhysicalLinkRecoveryEvent = 0x44,
            HCI_PINCodeRequestEvent = 0x16,
            HCI_QoSSetupCompleteEvent = 13,
            HCI_QoSViolationEvent = 30,
            HCI_ReadClockOffsetCompleteEvent = 0x1c,
            HCI_ReadRemoteExtendedFeaturesCompleteEvent = 0x23,
            HCI_ReadRemoteSupportedFeaturesCompleteEvent = 11,
            HCI_ReadRemoteVersionInformationCompleteEvent = 12,
            HCI_RemoteHostSupportedFeaturesNotificationEvent = 0x3d,
            HCI_RemoteNameRequestCompleteEvent = 7,
            HCI_RemoteOOBDataRequestEvent = 0x35,
            HCI_RemoteOobResponseEvent = 0x37,
            HCI_ReturnLinkKeysEvent = 0x15,
            HCI_RoleChangeEvent = 0x12,
            HCI_ShortRangeModeChangeCompleteEvent = 0x4c,
            HCI_SimplePairingCompleteEvent = 0x36,
            HCI_SniffRequestEvent = 0x3a,
            HCI_SniffSubratingEvent = 0x2e,
            HCI_SynchronousConnectionChangedEvent = 0x2d,
            HCI_SynchronousConnectionCompleteEvent = 0x2c,
            HCI_UserConfirmationRequestEvent = 0x33,
            HCI_UserPasskeyNotificationEvent = 0x3b,
            HCI_UserPasskeyRequestEvent = 0x34
        }

        public enum HCIEvtOpCode
        {
            ATT_ErrorRsp = 0x501,
            ATT_ExchangeMTUReq = 0x502,
            ATT_ExchangeMTURsp = 0x503,
            ATT_ExecuteWriteReq = 0x518,
            ATT_ExecuteWriteRsp = 0x519,
            ATT_FindByTypeValueReq = 0x506,
            ATT_FindByTypeValueRsp = 0x507,
            ATT_FindInfoReq = 0x504,
            ATT_FindInfoRsp = 0x505,
            ATT_HandleValueConfirmation = 0x51e,
            ATT_HandleValueIndication = 0x51d,
            ATT_HandleValueNotification = 0x51b,
            ATT_PrepareWriteReq = 0x516,
            ATT_PrepareWriteRsp = 0x517,
            ATT_ReadBlobReq = 0x50c,
            ATT_ReadBlobRsp = 0x50d,
            ATT_ReadByGrpTypeReq = 0x510,
            ATT_ReadByGrpTypeRsp = 0x511,
            ATT_ReadByTypeReq = 0x508,
            ATT_ReadByTypeRsp = 0x509,
            ATT_ReadMultiReq = 0x50e,
            ATT_ReadMultiRsp = 0x50f,
            ATT_ReadReq = 0x50a,
            ATT_ReadRsp = 0x50b,
            ATT_WriteReq = 0x512,
            ATT_WriteRsp = 0x513,
            GAP_AdvertDataUpdate = 0x602,
            GAP_AuthenticationComplete = 0x60a,
            GAP_BondComplete = 0x60e,
            GAP_DeviceDiscoveryDone = 0x601,
            GAP_DeviceInformation = 0x60d,
            GAP_DeviceInitDone = 0x600,
            GAP_EndDiscoverable = 0x604,
            GAP_EstablishLink = 0x605,
            GAP_HCI_ExtentionCommandStatus = 0x67f,
            GAP_LinkParamUpdate = 0x607,
            GAP_MakeDiscoverable = 0x603,
            GAP_PairingRequested = 0x60f,
            GAP_PasskeyNeeded = 0x60b,
            GAP_RandomAddressChange = 0x608,
            GAP_SignatureUpdate = 0x609,
            GAP_SlaveRequestedSecurity = 0x60c,
            GAP_TerminateLink = 0x606,
            GATT_ClientCharCfgUpdated = 0x580,
            HCIExt_ClkDivideOnHaltDone = 0x403,
            HCIExt_DeclareNvUsageDone = 0x404,
            HCIExt_DecryptDone = 0x405,
            HCIExt_DisconnectImmedDone = 0x413,
            HCIExt_EnablePTMDone = 0x40e,
            HCIExt_EndModemTestDone = 0x40b,
            HCIExt_MapPmIoPortDone = 0x412,
            HCIExt_ModemHopTestTxDone = 0x409,
            HCIExt_ModemTestRxDone = 0x40a,
            HCIExt_ModemTestTxDone = 0x408,
            HCIExt_OnePktPerEvtDone = 0x402,
            HCIExt_PERDone = 0x414,
            HCIExt_SaveFreqTuneDone = 0x410,
            HCIExt_SetBDADDRDone = 0x40c,
            HCIExt_SetFastTxRespTimeDone = 0x407,
            HCIExt_SetFreqTuneDone = 0x40f,
            HCIExt_SetLocalSupportedFeaturesDone = 0x406,
            HCIExt_SetMaxDtmTxPowerDone = 0x411,
            HCIExt_SetRxGainDone = 0x400,
            HCIExt_SetSCADone = 0x40d,
            HCIExt_SetTxPowerDone = 0x401,
            InvalidEventCode = 0,
            L2CAP_CmdReject = 0x481,
            L2CAP_ConnParamUpdateRsp = 0x493,
            L2CAP_InfoRsp = 0x48b
        }

        public enum HCIExt_ClkDivideOnHaltCtrl
        {
            DISABLE_CLK_DIVIDE_ON_HALT,
            ENABLE_CLK_DIVIDE_ON_HALT
        }

        public enum HCIExt_CwMode
        {
            TX_MODULATED_CARRIER,
            TX_UNMODULATED_CARRIER
        }

        public enum HCIExt_DeclareNvUsageMode
        {
            NV_NOT_IN_USE,
            NV_IN_USE
        }

        public enum HCIExt_MapPmIoPortPort
        {
            PM_IO_PORT_0 = 0,
            PM_IO_PORT_1 = 1,
            PM_IO_PORT_2 = 2,
            PM_IO_PORT_NONE = 0xff
        }

        public enum HCIExt_OnePktPerEvtCtrl
        {
            DISABLE_ONE_PKT_PER_EVT,
            ENABLE_ONE_PKT_PER_EVT
        }

        public enum HCIExt_PERCmd
        {
            Reset_PER_Counters,
            Read_PER_Counters
        }

        public enum HCIExt_RxGain
        {
            GAIN_STD,
            GAIN_HIGH
        }

        public enum HCIExt_SetFastTxRespTimeCtrl
        {
            DISABLE_FAST_TX_RESP_TIME,
            ENABLE_FAST_TX_RESP_TIME
        }

        public enum HCIExt_SetFreqTuneValue
        {
            TUNE_FREQUENCY_DOWN,
            TUNE_FREQUENCY_UP
        }

        public enum HCIExt_StatusCodes
        {
            SUCCESS,
            UNKNOWN_HCI_CMD,
            UNKNOWN_CONN_ID,
            HW_FAILURE,
            PAGE_TIMEOUT,
            AUTH_FAILURE,
            PIN_KEY_MISSING,
            MEM_CAP_EXCEEDED,
            CONN_TIMEOUT,
            CONN_LIMIT_EXCEEDED,
            SYNCH_CONN_LIMIT_EXCEEDED,
            ACL_CONN_ALREADY_EXISTS,
            CMD_DISALLOWED,
            CONN_REJ_LIMITED_RESOURCES,
            CONN_REJECTED_SECURITY_REASONS,
            CONN_REJECTED_UNACCEPTABLE_BDADDR,
            CONN_ACCEPT_TIMEOUT_EXCEEDED,
            UNSUPPORTED_FEATURE_PARAM_VALUE,
            INVALID_HCI_CMD_PARAMS,
            REMOTE_USER_TERM_CONN,
            REMOTE_DEVICE_TERM_CONN_LOW_RESOURCES,
            REMOTE_DEVICE_TERM_CONN_POWER_OFF,
            CONN_TERM_BY_LOCAL_HOST,
            REPEATED_ATTEMPTS,
            PAIRING_NOT_ALLOWED,
            UNKNOWN_LMP_PDU,
            UNSUPPORTED_REMOTE_FEATURE,
            SCO_OFFSET_REJ,
            SCO_INTERVAL_REJ,
            SCO_AIR_MODE_REJ,
            INVALID_LMP_PARAMS,
            UNSPECIFIED_ERROR,
            UNSUPPORTED_LMP_PARAM_VAL,
            ROLE_CHANGE_NOT_ALLOWED,
            LMP_LL_RESP_TIMEOUT,
            LMP_ERR_TRANSACTION_COLLISION,
            LMP_PDU_NOT_ALLOWED,
            ENCRYPT_MODE_NOT_ACCEPTABLE,
            LINK_KEY_CAN_NOT_BE_CHANGED,
            REQ_QOS_NOT_SUPPORTED,
            INSTANT_PASSED,
            PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED,
            DIFFERENT_TRANSACTION_COLLISION,
            RESERVED1,
            QOS_UNACCEPTABLE_PARAM,
            QOS_REJ,
            CHAN_ASSESSMENT_NOT_SUPPORTED,
            INSUFFICIENT_SECURITY,
            PARAM_OUT_OF_MANDATORY_RANGE,
            RESERVED2,
            ROLE_SWITCH_PENDING,
            RESERVED3,
            RESERVED_SLOT_VIOLATION,
            ROLE_SWITCH_FAILED,
            EXTENDED_INQUIRY_RESP_TOO_LARGE,
            SIMPLE_PAIRING_NOT_SUPPORTED_BY_HOST,
            HOST_BUSY_PAIRING,
            CONN_REJ_NO_SUITABLE_CHAN_FOUND,
            CONTROLLER_BUSY,
            UNACCEPTABLE_CONN_INTERVAL,
            DIRECTED_ADV_TIMEOUT,
            CONN_TERM_MIC_FAILURE,
            CONN_FAILED_TO_ESTABLISH,
            MAC_CONN_FAILED
        }

        public enum HCIExt_TxPower
        {
            POWER_MINUS_23_DBM,
            POWER_MINUS_6_DBM,
            POWER_0_DBM,
            POWER_4_DBM
        }

        public class HCIExtCmds
        {
            public class HCIExt_ClkDivideOnHalt
            {
                private HCICmds.HCIExt_ClkDivideOnHaltCtrl _control;
                public string cmdName = "HCIExt_ClkDivideOnHalt";
                public const string constCmdName = "HCIExt_ClkDivideOnHalt";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc03;

                [Description("Control (1 Byte) - Enable or disable clock division on halt."), DefaultValue(0)]
                public HCICmds.HCIExt_ClkDivideOnHaltCtrl control
                {
                    get
                    {
                        return this._control;
                    }
                    set
                    {
                        this._control = value;
                    }
                }

                [Description("HCIExt_ClkDivideOnHalt")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_DeclareNvUsage
            {
                private HCICmds.HCIExt_DeclareNvUsageMode _mode;
                public string cmdName = "HCIExt_DeclareNvUsage";
                public const string constCmdName = "HCIExt_DeclareNvUsage";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc04;

                [Description("Mode (1 Byte) - Enable or disable NV In Use."), DefaultValue(0)]
                public HCICmds.HCIExt_DeclareNvUsageMode mode
                {
                    get
                    {
                        return this._mode;
                    }
                    set
                    {
                        this._mode = value;
                    }
                }

                [Description("HCIExt_DeclareNvUsage")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_Decrypt
            {
                private string _data = "66:C6:C2:27:8E:3B:8E:05:3E:7E:A3:26:52:1B:AD:99";
                private const string _data_default = "66:C6:C2:27:8E:3B:8E:05:3E:7E:A3:26:52:1B:AD:99";
                private string _key = "BF:01:FB:9D:4E:F3:BC:36:D8:74:F5:39:41:38:68:4C";
                private const string _key_default = "BF:01:FB:9D:4E:F3:BC:36:D8:74:F5:39:41:38:68:4C";
                public string cmdName = "HCIExt_Decrypt";
                public const string constCmdName = "HCIExt_Decrypt";
                public byte dataLength;
                public const byte dataSize = 0x10;
                public const byte keySize = 0x10;
                public ushort opCodeValue = 0xfc05;

                [Description("Data (16 Bytes) - 128 bit encrypted data to be decrypted"), DefaultValue("66:C6:C2:27:8E:3B:8E:05:3E:7E:A3:26:52:1B:AD:99")]
                public string data
                {
                    get
                    {
                        return this._data;
                    }
                    set
                    {
                        this._data = value;
                    }
                }

                [Description("Key (16 Bytes) - 128 bit key for the decryption of the data"), DefaultValue("BF:01:FB:9D:4E:F3:BC:36:D8:74:F5:39:41:38:68:4C")]
                public string key
                {
                    get
                    {
                        return this._key;
                    }
                    set
                    {
                        this._key = value;
                    }
                }

                [Description("HCIExt_Decrypt")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_DisconnectImmed
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                public string cmdName = "HCIExt_DisconnectImmed";
                public const string constCmdName = "HCIExt_DisconnectImmed";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfc13;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("HCIExt_DisconnectImmed")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_EnablePTM
            {
                public string cmdName = "HCIExt_EnablePTM";
                public const string constCmdName = "HCIExt_EnablePTM";
                public byte dataLength;
                public ushort opCodeValue = 0xfc0e;

                [Description("HCIExt_EnablePTM")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_EndModemTest
            {
                public string cmdName = "HCIExt_EndModemTest";
                public const string constCmdName = "HCIExt_EndModemTest";
                public byte dataLength;
                public ushort opCodeValue = 0xfc0b;

                [Description("HCIExt_EndModemTest")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_MapPmIoPort
            {
                private HCICmds.HCIExt_MapPmIoPortPort _pmIoPort;
                private byte _pmIoPortPin;
                private const string _pmIoPortPin_default = "0x00";
                public string cmdName = "HCIExt_MapPmIoPort";
                public const string constCmdName = "HCIExt_MapPmIoPort";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfc12;

                [Description("HCIExt_MapPmIoPort")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("PM IO Port (1 Byte) - Map PM IO Port To P0, P1, or P2."), DefaultValue(0)]
                public HCICmds.HCIExt_MapPmIoPortPort pmIoPort
                {
                    get
                    {
                        return this._pmIoPort;
                    }
                    set
                    {
                        this._pmIoPort = value;
                    }
                }

                [Description("PM IO Port Pin (1 Byte) - Map PM IO Port To 0 through 7."), DefaultValue(typeof(byte), "0x00")]
                public byte pmIoPortPin
                {
                    get
                    {
                        return this._pmIoPortPin;
                    }
                    set
                    {
                        this._pmIoPortPin = value;
                    }
                }
            }

            public class HCIExt_ModemHopTestTx
            {
                public string cmdName = "HCIExt_ModemHopTestTx";
                public const string constCmdName = "HCIExt_ModemHopTestTx";
                public byte dataLength;
                public ushort opCodeValue = 0xfc09;

                [Description("HCIExt_ModemHopTestTx")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_ModemTestRx
            {
                private byte _rxRfChannel;
                private const byte _rxRfChannel_default = 0;
                public string cmdName = "HCIExt_ModemTestRx";
                public const string constCmdName = "HCIExt_ModemTestRx";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc0a;

                [Description("HCIExt_ModemTestRx")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue((byte) 0), Description("Rx RF Channel (1 Byte) - Channel Number 0 to 39")]
                public byte rxRfChannel
                {
                    get
                    {
                        return this._rxRfChannel;
                    }
                    set
                    {
                        this._rxRfChannel = value;
                    }
                }
            }

            public class HCIExt_ModemTestTx
            {
                private HCICmds.HCIExt_CwMode _cwMode;
                private byte _txRfChannel;
                private const byte _txRfChannel_default = 0;
                public string cmdName = "HCIExt_ModemTestTx";
                public const string constCmdName = "HCIExt_ModemTestTx";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfc08;

                [DefaultValue(0), Description("CW Mode (1 Byte) - Set Modem Test CW modulation.")]
                public HCICmds.HCIExt_CwMode cwMode
                {
                    get
                    {
                        return this._cwMode;
                    }
                    set
                    {
                        this._cwMode = value;
                    }
                }

                [Description("HCIExt_ModemTestTx")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue((byte) 0), Description("Tx RF Channel (1 Byte) - Channel Number 0 to 39")]
                public byte txRfChannel
                {
                    get
                    {
                        return this._txRfChannel;
                    }
                    set
                    {
                        this._txRfChannel = value;
                    }
                }
            }

            public class HCIExt_OnePktPerEvt
            {
                private HCICmds.HCIExt_OnePktPerEvtCtrl _control;
                public string cmdName = "HCIExt_OnePktPerEvt";
                public const string constCmdName = "HCIExt_OnePktPerEvt";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc02;

                [DefaultValue(0), Description("Control (1 Byte) - Enable or disable allowing only one packet per event.")]
                public HCICmds.HCIExt_OnePktPerEvtCtrl control
                {
                    get
                    {
                        return this._control;
                    }
                    set
                    {
                        this._control = value;
                    }
                }

                [Description("HCIExt_OnePktPerEvt")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_PER
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private HCICmds.HCIExt_PERCmd _perTestCommand;
                public string cmdName = "HCIExt_PER";
                public const string constCmdName = "HCIExt_PER";
                public byte dataLength = 3;
                public ushort opCodeValue = 0xfc14;

                [DefaultValue(typeof(ushort), "0xFFFE"), Description("Connection Handle (2 Bytes) - The handle of the connection")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("HCIExt_PER")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("PER Test Command (1 Byte) - Reset Or Read The PER Counters."), DefaultValue(0)]
                public HCICmds.HCIExt_PERCmd perTestCommand
                {
                    get
                    {
                        return this._perTestCommand;
                    }
                    set
                    {
                        this._perTestCommand = value;
                    }
                }
            }

            public class HCIExt_SaveFreqTune
            {
                public string cmdName = "HCIExt_SaveFreqTune";
                public const string constCmdName = "HCIExt_SaveFreqTune";
                public byte dataLength;
                public ushort opCodeValue = 0xfc10;

                [Description("HCIExt_SaveFreqTune")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_SetBDADDR
            {
                private string _bleDevAddr = "00:00:00:00:00:00";
                public string cmdName = "HCIExt_SetBDADDR";
                public const string constCmdName = "HCIExt_SetBDADDR";
                public byte dataLength;
                public ushort opCodeValue = 0xfc0c;

                [Description("BLE Device Address (6 Bytes) - Set this device’s BLE address"), DefaultValue("00:00:00:00:00:00")]
                public string bleDevAddr
                {
                    get
                    {
                        return this._bleDevAddr;
                    }
                    set
                    {
                        this._bleDevAddr = value;
                    }
                }

                [Description("HCIExt_SetBDADDR")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_SetFastTxRespTime
            {
                private HCICmds.HCIExt_SetFastTxRespTimeCtrl _control;
                public string cmdName = "HCIExt_SetFastTxRespTime";
                public const string constCmdName = "HCIExt_SetFastTxRespTime";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc07;

                [Description("Control (1 Byte) - Enable or disable the fast Tx response time feature."), DefaultValue(0)]
                public HCICmds.HCIExt_SetFastTxRespTimeCtrl control
                {
                    get
                    {
                        return this._control;
                    }
                    set
                    {
                        this._control = value;
                    }
                }

                [Description("HCIExt_SetFastTxRespTime")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_SetFreqTune
            {
                private HCICmds.HCIExt_SetFreqTuneValue _setFreqTune = HCICmds.HCIExt_SetFreqTuneValue.TUNE_FREQUENCY_UP;
                public string cmdName = "HCIExt_SetFreqTune";
                public const string constCmdName = "HCIExt_SetFreqTune";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc0f;

                [Description("HCIExt_SetFreqTune")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(1), Description("Set Freq Tune (1 Byte) - Set Frequency Tuning Up Or Down.")]
                public HCICmds.HCIExt_SetFreqTuneValue setFreqTune
                {
                    get
                    {
                        return this._setFreqTune;
                    }
                    set
                    {
                        this._setFreqTune = value;
                    }
                }
            }

            public class HCIExt_SetLocalSupportedFeatures
            {
                private string _localFeatures = "01:00:00:00:00:00:00:00";
                private const string _localFeatures_default = "01:00:00:00:00:00:00:00";
                public string cmdName = "HCIExt_SetLocalSupportedFeatures";
                public const string constCmdName = "HCIExt_SetLocalSupportedFeatures";
                public byte dataLength = 8;
                public const byte localFeaturesSize = 8;
                public ushort opCodeValue = 0xfc06;

                [DefaultValue("01:00:00:00:00:00:00:00"), Description("Local Features (8 Bytes) - Set the Controller’s Local Supported Features.")]
                public string localFeatures
                {
                    get
                    {
                        return this._localFeatures;
                    }
                    set
                    {
                        this._localFeatures = value;
                    }
                }

                [Description("HCIExt_SetLocalSupportedFeatures")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIExt_SetMaxDtmTxPower
            {
                private HCICmds.HCIExt_TxPower _txPower = HCICmds.HCIExt_TxPower.POWER_4_DBM;
                public string cmdName = "HCIExt_SetMaxDtmTxPower";
                public const string constCmdName = "HCIExt_SetMaxDtmTxPower";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc11;

                [Description("HCIExt_SetMaxDtmTxPower")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(3), Description("TX Power (1 Byte) - Sets The TX Power To -23, -6, 0 or 4dbm.")]
                public HCICmds.HCIExt_TxPower txPower
                {
                    get
                    {
                        return this._txPower;
                    }
                    set
                    {
                        this._txPower = value;
                    }
                }
            }

            public class HCIExt_SetRxGain
            {
                private HCICmds.HCIExt_RxGain _rxGain;
                public string cmdName = "HCIExt_SetRxGain";
                public const string constCmdName = "HCIExt_SetRxGain";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc00;

                [Description("HCIExt_SetRxGain")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(0), Description("Rx Gain (1 Byte) - Set the RF receiver gain")]
                public HCICmds.HCIExt_RxGain rxGain
                {
                    get
                    {
                        return this._rxGain;
                    }
                    set
                    {
                        this._rxGain = value;
                    }
                }
            }

            public class HCIExt_SetSCA
            {
                private ushort _sca = 40;
                private const string _sca_default = "40";
                public string cmdName = "HCIExt_SetSCA";
                public const string constCmdName = "HCIExt_SetSCA";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfc0d;

                [Description("HCIExt_SetSCA")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("SCA (2 Bytes) - BLE Device Sleep Clock Accuracy, In PPM (0..500)"), DefaultValue(typeof(ushort), "40")]
                public ushort sca
                {
                    get
                    {
                        return this._sca;
                    }
                    set
                    {
                        this._sca = value;
                    }
                }
            }

            public class HCIExt_SetTxPower
            {
                private HCICmds.HCIExt_TxPower _txPower = HCICmds.HCIExt_TxPower.POWER_0_DBM;
                public string cmdName = "HCIExt_SetTxPower";
                public const string constCmdName = "HCIExt_SetTxPower";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfc01;

                [Description("HCIExt_SetTxPower")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Tx Power dBm (1 Byte) - Set the RF transmitter output power"), DefaultValue(2)]
                public HCICmds.HCIExt_TxPower txPower
                {
                    get
                    {
                        return this._txPower;
                    }
                    set
                    {
                        this._txPower = value;
                    }
                }
            }
        }

        public class HCIOtherCmds
        {
            public class HCIOther_LEAddDeviceToWhiteList
            {
                private HCICmds.HCI_LEAddressType _addrType;
                private string _devAddr = "00:00:00:00:00:00";
                public const byte addrSize = 6;
                public string cmdName = "HCI_LEAddDeviceToWhiteList";
                public const string constCmdName = "HCI_LEAddDeviceToWhiteList";
                public byte dataLength = 2;
                public ushort opCodeValue = 0x2011;

                [DefaultValue(0), Description("Device Address Type (1 Byte) - Indicates Device Address Type Of The Address Added To The List. 0x00 = Public Address, 0x01 = Random Address, 0x02-0xFF = Reserved")]
                public HCICmds.HCI_LEAddressType addrType
                {
                    get
                    {
                        return this._addrType;
                    }
                    set
                    {
                        this._addrType = value;
                    }
                }

                [Description("Device Address (6 Bytes) - Device Address That Is To Be Added To The White List."), DefaultValue("00:00:00:00:00:00")]
                public string devAddr
                {
                    get
                    {
                        return this._devAddr;
                    }
                    set
                    {
                        this._devAddr = value;
                    }
                }

                [Description("HCI_LEAddDeviceToWhiteList")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIOther_LEClearWhiteList
            {
                public string cmdName = "HCI_LEClearWhiteList";
                public const string constCmdName = "HCI_LEClearWhiteList";
                public byte dataLength;
                public ushort opCodeValue = 0x2010;

                [Description("HCI_LEClearWhiteList")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIOther_LEConnectionUpdate
            {
                private ushort _connInterval = 6;
                private const string _connInterval_default = "0x0006";
                private ushort _connIntervalMax = 6;
                private const string _connIntervalMax_default = "0x0006";
                private ushort _connLatency;
                private const string _connLatency_default = "0x0000";
                private ushort _connTimeout = 10;
                private const string _connTimeout_default = "0x000A";
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                private ushort _maximumLength;
                private const string _maximumLength_default = "0x0000";
                private ushort _minimumLength;
                private const string _minimumLength_default = "0x0000";
                public string cmdName = "HCI_LEConnectionUpdate";
                public const string constCmdName = "HCI_LEConnectionUpdate";
                public byte dataLength = 14;
                public ushort opCodeValue = 0x2013;

                [DefaultValue(typeof(ushort), "0x0006"), Description("ConnInterval (2 Bytes) - ConnInterval * 1.25 ms, ConnInterval range: 0x0006 to 0x0C80")]
                public ushort connInterval
                {
                    get
                    {
                        return this._connInterval;
                    }
                    set
                    {
                        this._connInterval = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0006"), Description("ConnIntervalMax (2 Bytes) - ConnInterval * 1.25 ms, ConnIntervalMax range: 0x0006 to 0x0C80, Shall be equal to or greater than the ConnIntervalMin.")]
                public ushort connIntervalMax
                {
                    get
                    {
                        return this._connIntervalMax;
                    }
                    set
                    {
                        this._connIntervalMax = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("ConnLatency (2 Bytes) - ConnLatency (as number of LL connection events). ConnLatency range: 0x0000 to 0x03E8.")]
                public ushort connLatency
                {
                    get
                    {
                        return this._connLatency;
                    }
                    set
                    {
                        this._connLatency = value;
                    }
                }

                [Description("ConnTimeout (2 Bytes) - ConnTimeout * 10 ms, ConnTimeout range: 0x000A to 0x0C80."), DefaultValue(typeof(ushort), "0x000A")]
                public ushort connTimeout
                {
                    get
                    {
                        return this._connTimeout;
                    }
                    set
                    {
                        this._connTimeout = value;
                    }
                }

                [Description("Handle (2 Bytes) - Local identifier of the LL connection"), DefaultValue(typeof(ushort), "0x0001")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "0x0000"), Description("MaximumLength (2 Bytes) - MaximumLength * 0.625 ms, MaximumLength range: 0x01 to 2*ConnInterval.")]
                public ushort maximumLength
                {
                    get
                    {
                        return this._maximumLength;
                    }
                    set
                    {
                        this._maximumLength = value;
                    }
                }

                [Description("MinimumLength (2 Bytes) - MinimumLength * 0.625 ms, MinimumLength range: 0x01 to 2*ConnInterval."), DefaultValue(typeof(ushort), "0x0000")]
                public ushort minimumLength
                {
                    get
                    {
                        return this._minimumLength;
                    }
                    set
                    {
                        this._minimumLength = value;
                    }
                }

                [Description("HCI_LEConnectionUpdate")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIOther_LERemoveDeviceFromWhiteList
            {
                private HCICmds.HCI_LEAddressType _addrType;
                private string _devAddr = "00:00:00:00:00:00";
                public const byte addrSize = 6;
                public string cmdName = "HCI_LERemoveDeviceFromWhiteList";
                public const string constCmdName = "HCI_LERemoveDeviceFromWhiteList";
                public byte dataLength = 2;
                public ushort opCodeValue = 0x2012;

                [DefaultValue(0), Description("Device Address Type (1 Byte) - Indicates Device Address Type Of The Address Added To The List. 0x00 = Public Address, 0x01 = Random Address, 0x02-0xFF = Reserved")]
                public HCICmds.HCI_LEAddressType addrType
                {
                    get
                    {
                        return this._addrType;
                    }
                    set
                    {
                        this._addrType = value;
                    }
                }

                [DefaultValue("00:00:00:00:00:00"), Description("Device Address (6 Bytes) - Device Address That Is To Be Added To The White List.")]
                public string devAddr
                {
                    get
                    {
                        return this._devAddr;
                    }
                    set
                    {
                        this._devAddr = value;
                    }
                }

                [Description("HCI_LERemoveDeviceFromWhiteList")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class HCIOther_ReadRSSI
            {
                private ushort _handle = 1;
                private const string _handle_default = "0x0001";
                public string cmdName = "HCI_ReadRSSI";
                public const string constCmdName = "HCI_ReadRSSI";
                public byte dataLength = 2;
                public ushort opCodeValue = 0x1405;

                [DefaultValue(typeof(ushort), "0x0001"), Description("Handle (2 Bytes) - The handle")]
                public ushort handle
                {
                    get
                    {
                        return this._handle;
                    }
                    set
                    {
                        this._handle = value;
                    }
                }

                [Description("HCI_ReadRSSI")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }
        }

        public enum HCIReqOpcode
        {
            ATT_ErrorRsp = 1,
            ATT_ExchangeMTUReq = 2,
            ATT_ExchangeMTURsp = 3,
            ATT_ExecuteWriteReq = 0x18,
            ATT_ExecuteWriteRsp = 0x19,
            ATT_FindByTypeValueReq = 6,
            ATT_FindByTypeValueRsp = 7,
            ATT_FindInfoReq = 4,
            ATT_FindInfoRsp = 5,
            ATT_HandleValueConfirmation = 30,
            ATT_HandleValueIndication = 0x1d,
            ATT_HandleValueNotification = 0x1b,
            ATT_PrepareWriteReq = 0x16,
            ATT_PrepareWriteRsp = 0x17,
            ATT_ReadBlobReq = 12,
            ATT_ReadBlobRsp = 13,
            ATT_ReadByGrpTypeReq = 0x10,
            ATT_ReadByGrpTypeRsp = 0x11,
            ATT_ReadByTypeReq = 8,
            ATT_ReadByTypeRsp = 9,
            ATT_ReadMultiReq = 14,
            ATT_ReadMultiRsp = 15,
            ATT_ReadReq = 10,
            ATT_ReadRsp = 11,
            ATT_WriteReq = 0x12,
            ATT_WriteRsp = 0x13
        }

        public enum L2CAP_ConnParamUpdateResult
        {
            ACCEPTED,
            REJECTED
        }

        public enum L2CAP_InfoTypes
        {
            CONNECTIONLESS_MTU = 1,
            EXTENDED_FEATURES = 2,
            FIXED_CHANNELS = 3
        }

        public enum L2CAP_RejectReasons
        {
            CMD_NOT_UNDERSTOOD,
            SIGNAL_MTU_EXCEED,
            INVALID_CID
        }

        public class L2CAPCmds
        {
            public class L2CAP_ConnParamUpdateReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private ushort _intervalMax = 160;
                private const string _intervalMax_default = "160";
                private ushort _intervalMin = 80;
                private const string _intervalMin_default = "80";
                private ushort _slaveLatency;
                private const string _slaveLatency_default = "0";
                private ushort _timeoutMultiplier = 0x3e8;
                private const string _timeoutMultiplier_default = "1000";
                public string cmdName = "L2CAP_ConnParamUpdateReq";
                public const string constCmdName = "L2CAP_ConnParamUpdateReq";
                public byte dataLength = 10;
                public ushort opCodeValue = 0xfc92;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [DefaultValue(typeof(ushort), "160"), Description("Interval Max (2 Bytes) - The maximum value for the connection event interval")]
                public ushort intervalMax
                {
                    get
                    {
                        return this._intervalMax;
                    }
                    set
                    {
                        this._intervalMax = value;
                    }
                }

                [DefaultValue(typeof(ushort), "80"), Description("Interval Min (2 Bytes) - The minimum value for the connection event interval")]
                public ushort intervalMin
                {
                    get
                    {
                        return this._intervalMin;
                    }
                    set
                    {
                        this._intervalMin = value;
                    }
                }

                [Description("L2CAP_ConnParamUpdateReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [Description("Slave Latency (2 Bytes) - The slave latency parameter"), DefaultValue(typeof(ushort), "0")]
                public ushort slaveLatency
                {
                    get
                    {
                        return this._slaveLatency;
                    }
                    set
                    {
                        this._slaveLatency = value;
                    }
                }

                [Description("Timeout Multiplier (2 Bytes) - The connection timeout parameter"), DefaultValue(typeof(ushort), "1000")]
                public ushort timeoutMultiplier
                {
                    get
                    {
                        return this._timeoutMultiplier;
                    }
                    set
                    {
                        this._timeoutMultiplier = value;
                    }
                }
            }

            public class L2CAP_InfoReq
            {
                private ushort _connHandle = 0xfffe;
                private const string _connHandle_default = "0xFFFE";
                private HCICmds.L2CAP_InfoTypes _infoType = HCICmds.L2CAP_InfoTypes.EXTENDED_FEATURES;
                public string cmdName = "L2CAP_InfoReq";
                public const string constCmdName = "L2CAP_InfoReq";
                public byte dataLength = 4;
                public ushort opCodeValue = 0xfc8a;

                [Description("Connection Handle (2 Bytes) - The handle of the connection"), DefaultValue(typeof(ushort), "0xFFFE")]
                public ushort connHandle
                {
                    get
                    {
                        return this._connHandle;
                    }
                    set
                    {
                        this._connHandle = value;
                    }
                }

                [Description("Info Type (2 Bytes) - The type of implementation specific information being requested"), DefaultValue(2)]
                public HCICmds.L2CAP_InfoTypes infoType
                {
                    get
                    {
                        return this._infoType;
                    }
                    set
                    {
                        this._infoType = value;
                    }
                }

                [Description("L2CAP_InfoReq")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }
        }

        public class MISCCmds
        {
            public class MISC_GenericCommand
            {
                private string _data = "00";
                private const string _data_default = "00";
                private byte _dataLength;
                private const byte _dataLength_default = 0;
                private string _opCode = "0x0000";
                private const string _opCode_default = "0x0000";
                public string cmdName = "MISC_GenericCommand";
                public const string constCmdName = "MISC_GenericCommand";

                [DefaultValue("00"), Description("Data (x Bytes) - The data")]
                public string data
                {
                    get
                    {
                        return this._data;
                    }
                    set
                    {
                        this._data = value;
                    }
                }

                [Description("DataLength (1 Byte) - The length of the data. This field is auto calculated when the command is sent."), ReadOnly(true), DefaultValue((byte) 0)]
                public byte dataLength
                {
                    get
                    {
                        return this._dataLength;
                    }
                    set
                    {
                        this._dataLength = value;
                    }
                }

                [DefaultValue("0x0000"), Description("Opcode (2 Bytes) - The opcode of the command\nFormat: 0x0000")]
                public string opCode
                {
                    get
                    {
                        return this._opCode;
                    }
                    set
                    {
                        this._opCode = value;
                    }
                }

                [Description("PacketType (1 Byte) -\n0x00 Command | 0x01 - Async | 0x02 - Sync | 0x03 - Event")]
                public HCICmds.PacketType packetType
                {
                    get
                    {
                        return HCICmds.PacketType.Command;
                    }
                }
            }

            public class MISC_RawTxMessage
            {
                private string _message = "00 00 00 00";
                private const string _message_default = "00 00 00 00";
                public string cmdName = "MISC_RawTxMessage";
                public const string constCmdName = "MISC_RawTxMessage";
                public byte dataLength;
                public const byte minMsgSize = 4;

                [DefaultValue("00 00 00 00"), Description("Raw Tx Message (> 4 Bytes) - The Raw Tx Message")]
                public string message
                {
                    get
                    {
                        return this._message;
                    }
                    set
                    {
                        this._message = value;
                    }
                }
            }
        }

        public enum PacketType
        {
            AsyncData = 2,
            Command = 1,
            Event = 4,
            SyncData = 3
        }

        public enum UTIL_ResetType
        {
            Hard_Reset,
            Soft_Reset
        }

        public class UTILCmds
        {
            public class UTIL_ForceBoot
            {
                public string cmdName = "UTIL_ForceBoot";
                public const string constCmdName = "UTIL_ForceBoot";
                public byte dataLength;
                public ushort opCodeValue = 0xfe83;

                [Description("UTIL_ForceBoot")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class UTIL_NVRead
            {
                private byte _nvDataLen;
                private const byte _nvDataLen_default = 0;
                private byte _nvId;
                private const byte _nvId_default = 0;
                public string cmdName = "UTIL_NVRead";
                public const string constCmdName = "UTIL_NVRead";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfe81;

                [DefaultValue((byte) 0), Description("NV Data Len (1 Byte) - NV Data Length")]
                public byte nvDataLen
                {
                    get
                    {
                        return this._nvDataLen;
                    }
                    set
                    {
                        this._nvDataLen = value;
                    }
                }

                [Description("NV ID (1 Byte) - NV ID Number"), DefaultValue((byte) 0)]
                public byte nvId
                {
                    get
                    {
                        return this._nvId;
                    }
                    set
                    {
                        this._nvId = value;
                    }
                }

                [Description("UTIL_NVRead")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class UTIL_NVWrite
            {
                private string _nvData = "00";
                private const string _nvData_default = "00";
                private byte _nvDataLen;
                private const byte _nvDataLen_default = 0;
                private byte _nvId;
                private const byte _nvId_default = 0;
                public string cmdName = "UTIL_NVWrite";
                public const string constCmdName = "UTIL_NVWrite";
                public byte dataLength = 2;
                public ushort opCodeValue = 0xfe82;

                [Description("NV Data (x Bytes) - NV Data depends on the NV ID"), DefaultValue("00")]
                public string nvData
                {
                    get
                    {
                        return this._nvData;
                    }
                    set
                    {
                        this._nvData = value;
                    }
                }

                [ReadOnly(true), Description("NV Data Len (1 Byte) - NV Data Length. This field is auto calculated when the command is sent."), DefaultValue((byte) 0)]
                public byte nvDataLen
                {
                    get
                    {
                        return this._nvDataLen;
                    }
                    set
                    {
                        this._nvDataLen = value;
                    }
                }

                [DefaultValue((byte) 0), Description("NV ID (1 Byte) - NV ID Number")]
                public byte nvId
                {
                    get
                    {
                        return this._nvId;
                    }
                    set
                    {
                        this._nvId = value;
                    }
                }

                [Description("UTIL_NVWrite")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }
            }

            public class UTIL_Reset
            {
                private HCICmds.UTIL_ResetType _resetType;
                public string cmdName = "UTIL_Reset";
                public const string constCmdName = "UTIL_Reset";
                public byte dataLength = 1;
                public ushort opCodeValue = 0xfe80;

                [Description("UTIL_Reset")]
                public string opCode
                {
                    get
                    {
                        return ("0x" + this.opCodeValue.ToString("X4"));
                    }
                }

                [DefaultValue(0), Description("Reset Type (1 Byte) - 0 = Hard and 1 = Soft ")]
                public HCICmds.UTIL_ResetType resetType
                {
                    get
                    {
                        return this._resetType;
                    }
                    set
                    {
                        this._resetType = value;
                    }
                }
            }
        }
    }
}
