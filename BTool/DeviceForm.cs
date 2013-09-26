﻿namespace BTool
{
    using BTool.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class DeviceForm : Form
    {
        public BTool.HCICmds.ATTCmds.ATT_ErrorRsp ATT_ErrorRsp = new BTool.HCICmds.ATTCmds.ATT_ErrorRsp();
        public BTool.HCICmds.ATTCmds.ATT_ExchangeMTUReq ATT_ExchangeMTUReq = new BTool.HCICmds.ATTCmds.ATT_ExchangeMTUReq();
        public BTool.HCICmds.ATTCmds.ATT_ExchangeMTURsp ATT_ExchangeMTURsp = new BTool.HCICmds.ATTCmds.ATT_ExchangeMTURsp();
        public BTool.HCICmds.ATTCmds.ATT_ExecuteWriteReq ATT_ExecuteWriteReq = new BTool.HCICmds.ATTCmds.ATT_ExecuteWriteReq();
        public BTool.HCICmds.ATTCmds.ATT_ExecuteWriteRsp ATT_ExecuteWriteRsp = new BTool.HCICmds.ATTCmds.ATT_ExecuteWriteRsp();
        public BTool.HCICmds.ATTCmds.ATT_FindByTypeValueReq ATT_FindByTypeValueReq = new BTool.HCICmds.ATTCmds.ATT_FindByTypeValueReq();
        public BTool.HCICmds.ATTCmds.ATT_FindByTypeValueRsp ATT_FindByTypeValueRsp = new BTool.HCICmds.ATTCmds.ATT_FindByTypeValueRsp();
        public BTool.HCICmds.ATTCmds.ATT_FindInfoReq ATT_FindInfoReq = new BTool.HCICmds.ATTCmds.ATT_FindInfoReq();
        public BTool.HCICmds.ATTCmds.ATT_FindInfoRsp ATT_FindInfoRsp = new BTool.HCICmds.ATTCmds.ATT_FindInfoRsp();
        public BTool.HCICmds.ATTCmds.ATT_HandleValueConfirmation ATT_HandleValueConfirmation = new BTool.HCICmds.ATTCmds.ATT_HandleValueConfirmation();
        public BTool.HCICmds.ATTCmds.ATT_HandleValueIndication ATT_HandleValueIndication = new BTool.HCICmds.ATTCmds.ATT_HandleValueIndication();
        public BTool.HCICmds.ATTCmds.ATT_HandleValueNotification ATT_HandleValueNotification = new BTool.HCICmds.ATTCmds.ATT_HandleValueNotification();
        public BTool.HCICmds.ATTCmds.ATT_PrepareWriteReq ATT_PrepareWriteReq = new BTool.HCICmds.ATTCmds.ATT_PrepareWriteReq();
        public BTool.HCICmds.ATTCmds.ATT_PrepareWriteRsp ATT_PrepareWriteRsp = new BTool.HCICmds.ATTCmds.ATT_PrepareWriteRsp();
        public BTool.HCICmds.ATTCmds.ATT_ReadBlobReq ATT_ReadBlobReq = new BTool.HCICmds.ATTCmds.ATT_ReadBlobReq();
        public BTool.HCICmds.ATTCmds.ATT_ReadBlobRsp ATT_ReadBlobRsp = new BTool.HCICmds.ATTCmds.ATT_ReadBlobRsp();
        public BTool.HCICmds.ATTCmds.ATT_ReadByGrpTypeReq ATT_ReadByGrpTypeReq = new BTool.HCICmds.ATTCmds.ATT_ReadByGrpTypeReq();
        public BTool.HCICmds.ATTCmds.ATT_ReadByGrpTypeRsp ATT_ReadByGrpTypeRsp = new BTool.HCICmds.ATTCmds.ATT_ReadByGrpTypeRsp();
        public BTool.HCICmds.ATTCmds.ATT_ReadByTypeReq ATT_ReadByTypeReq = new BTool.HCICmds.ATTCmds.ATT_ReadByTypeReq();
        public BTool.HCICmds.ATTCmds.ATT_ReadByTypeRsp ATT_ReadByTypeRsp = new BTool.HCICmds.ATTCmds.ATT_ReadByTypeRsp();
        public BTool.HCICmds.ATTCmds.ATT_ReadMultiReq ATT_ReadMultiReq = new BTool.HCICmds.ATTCmds.ATT_ReadMultiReq();
        public BTool.HCICmds.ATTCmds.ATT_ReadMultiRsp ATT_ReadMultiRsp = new BTool.HCICmds.ATTCmds.ATT_ReadMultiRsp();
        public BTool.HCICmds.ATTCmds.ATT_ReadReq ATT_ReadReq = new BTool.HCICmds.ATTCmds.ATT_ReadReq();
        public BTool.HCICmds.ATTCmds.ATT_ReadRsp ATT_ReadRsp = new BTool.HCICmds.ATTCmds.ATT_ReadRsp();
        public BTool.HCICmds.ATTCmds.ATT_WriteReq ATT_WriteReq = new BTool.HCICmds.ATTCmds.ATT_WriteReq();
        public BTool.HCICmds.ATTCmds.ATT_WriteRsp ATT_WriteRsp = new BTool.HCICmds.ATTCmds.ATT_WriteRsp();
        public AttrData attrData = new AttrData();
        private AttributesForm attributesForm;
        public string BDAddressStr = "";
        private CommManager commMgr = new CommManager();
        private CommParser commParser = new CommParser();
        private CommSelectForm commSelectForm;
        private IContainer components;
        private ConnectInfo connectInfo = new ConnectInfo();
        public List<ConnectInfo> Connections = new List<ConnectInfo>();
        public GAPGetConnectionParams ConnParamState;
        private DataUtils dataUtils = new DataUtils();
        private bool DeviceStarted;
        public DeviceInfo devInfo = new DeviceInfo();
        public DeviceTabsForm devTabsForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public ConnectInfo disconnectInfo = new ConnectInfo();
        private DisplayCmdUtils dspCmdUtils = new DisplayCmdUtils();
        private Mutex dspMsgMutex = new Mutex();
        private DisplayTxCmds dspTxCmds = new DisplayTxCmds();
        private System.Windows.Forms.Timer establishTimer;
        private int[] EventTimeout = new int[] { 0x2710, 0x124f8, 0x7530, 0xc350 };
        private bool formClosing;
        public BTool.HCICmds.GAPCmds.GAP_Authenticate GAP_Authenticate = new BTool.HCICmds.GAPCmds.GAP_Authenticate();
        public BTool.HCICmds.GAPCmds.GAP_Bond GAP_Bond = new BTool.HCICmds.GAPCmds.GAP_Bond();
        public BTool.HCICmds.GAPCmds.GAP_BondGetParam GAP_BondGetParam = new BTool.HCICmds.GAPCmds.GAP_BondGetParam();
        public BTool.HCICmds.GAPCmds.GAP_BondSetParam GAP_BondSetParam = new BTool.HCICmds.GAPCmds.GAP_BondSetParam();
        public BTool.HCICmds.GAPCmds.GAP_ConfigDeviceAddr GAP_ConfigDeviceAddr = new BTool.HCICmds.GAPCmds.GAP_ConfigDeviceAddr();
        public BTool.HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel GAP_DeviceDiscoveryCancel = new BTool.HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel();
        public BTool.HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest GAP_DeviceDiscoveryRequest = new BTool.HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest();
        public BTool.HCICmds.GAPCmds.GAP_DeviceInit GAP_DeviceInit = new BTool.HCICmds.GAPCmds.GAP_DeviceInit();
        public BTool.HCICmds.GAPCmds.GAP_EndDiscoverable GAP_EndDiscoverable = new BTool.HCICmds.GAPCmds.GAP_EndDiscoverable();
        public BTool.HCICmds.GAPCmds.GAP_EstablishLinkRequest GAP_EstablishLinkRequest = new BTool.HCICmds.GAPCmds.GAP_EstablishLinkRequest();
        public BTool.HCICmds.GAPCmds.GAP_GetParam GAP_GetParam = new BTool.HCICmds.GAPCmds.GAP_GetParam();
        public BTool.HCICmds.GAPCmds.GAP_MakeDiscoverable GAP_MakeDiscoverable = new BTool.HCICmds.GAPCmds.GAP_MakeDiscoverable();
        public BTool.HCICmds.GAPCmds.GAP_PasskeyUpdate GAP_PasskeyUpdate = new BTool.HCICmds.GAPCmds.GAP_PasskeyUpdate();
        public BTool.HCICmds.GAPCmds.GAP_RemoveAdvToken GAP_RemoveAdvToken = new BTool.HCICmds.GAPCmds.GAP_RemoveAdvToken();
        public BTool.HCICmds.GAPCmds.GAP_ResolvePrivateAddr GAP_ResolvePrivateAddr = new BTool.HCICmds.GAPCmds.GAP_ResolvePrivateAddr();
        public BTool.HCICmds.GAPCmds.GAP_SetAdvToken GAP_SetAdvToken = new BTool.HCICmds.GAPCmds.GAP_SetAdvToken();
        public BTool.HCICmds.GAPCmds.GAP_SetParam GAP_SetParam = new BTool.HCICmds.GAPCmds.GAP_SetParam();
        public BTool.HCICmds.GAPCmds.GAP_Signable GAP_Signable = new BTool.HCICmds.GAPCmds.GAP_Signable();
        public BTool.HCICmds.GAPCmds.GAP_SlaveSecurityRequest GAP_SlaveSecurityRequest = new BTool.HCICmds.GAPCmds.GAP_SlaveSecurityRequest();
        public BTool.HCICmds.GAPCmds.GAP_TerminateAuth GAP_TerminateAuth = new BTool.HCICmds.GAPCmds.GAP_TerminateAuth();
        public BTool.HCICmds.GAPCmds.GAP_TerminateLinkRequest GAP_TerminateLinkRequest = new BTool.HCICmds.GAPCmds.GAP_TerminateLinkRequest();
        public BTool.HCICmds.GAPCmds.GAP_UpdateAdvertisingData GAP_UpdateAdvertisingData = new BTool.HCICmds.GAPCmds.GAP_UpdateAdvertisingData();
        public BTool.HCICmds.GAPCmds.GAP_UpdateAdvTokens GAP_UpdateAdvTokens = new BTool.HCICmds.GAPCmds.GAP_UpdateAdvTokens();
        public BTool.HCICmds.GAPCmds.GAP_UpdateLinkParamReq GAP_UpdateLinkParamReq = new BTool.HCICmds.GAPCmds.GAP_UpdateLinkParamReq();
        public BTool.HCICmds.GATTCmds.GATT_AddAttribute GATT_AddAttribute = new BTool.HCICmds.GATTCmds.GATT_AddAttribute();
        public BTool.HCICmds.GATTCmds.GATT_AddService GATT_AddService = new BTool.HCICmds.GATTCmds.GATT_AddService();
        public BTool.HCICmds.GATTCmds.GATT_DelService GATT_DelService = new BTool.HCICmds.GATTCmds.GATT_DelService();
        public BTool.HCICmds.GATTCmds.GATT_DiscAllCharDescs GATT_DiscAllCharDescs = new BTool.HCICmds.GATTCmds.GATT_DiscAllCharDescs();
        public BTool.HCICmds.GATTCmds.GATT_DiscAllChars GATT_DiscAllChars = new BTool.HCICmds.GATTCmds.GATT_DiscAllChars();
        public BTool.HCICmds.GATTCmds.GATT_DiscAllPrimaryServices GATT_DiscAllPrimaryServices = new BTool.HCICmds.GATTCmds.GATT_DiscAllPrimaryServices();
        public BTool.HCICmds.GATTCmds.GATT_DiscCharsByUUID GATT_DiscCharsByUUID = new BTool.HCICmds.GATTCmds.GATT_DiscCharsByUUID();
        public BTool.HCICmds.GATTCmds.GATT_DiscPrimaryServiceByUUID GATT_DiscPrimaryServiceByUUID = new BTool.HCICmds.GATTCmds.GATT_DiscPrimaryServiceByUUID();
        public BTool.HCICmds.GATTCmds.GATT_ExchangeMTU GATT_ExchangeMTU = new BTool.HCICmds.GATTCmds.GATT_ExchangeMTU();
        public BTool.HCICmds.GATTCmds.GATT_FindIncludedServices GATT_FindIncludedServices = new BTool.HCICmds.GATTCmds.GATT_FindIncludedServices();
        public BTool.HCICmds.GATTCmds.GATT_Indication GATT_Indication = new BTool.HCICmds.GATTCmds.GATT_Indication();
        public BTool.HCICmds.GATTCmds.GATT_Notification GATT_Notification = new BTool.HCICmds.GATTCmds.GATT_Notification();
        public BTool.HCICmds.GATTCmds.GATT_ReadCharDesc GATT_ReadCharDesc = new BTool.HCICmds.GATTCmds.GATT_ReadCharDesc();
        public BTool.HCICmds.GATTCmds.GATT_ReadCharValue GATT_ReadCharValue = new BTool.HCICmds.GATTCmds.GATT_ReadCharValue();
        public BTool.HCICmds.GATTCmds.GATT_ReadLongCharDesc GATT_ReadLongCharDesc = new BTool.HCICmds.GATTCmds.GATT_ReadLongCharDesc();
        public BTool.HCICmds.GATTCmds.GATT_ReadLongCharValue GATT_ReadLongCharValue = new BTool.HCICmds.GATTCmds.GATT_ReadLongCharValue();
        public BTool.HCICmds.GATTCmds.GATT_ReadMultiCharValues GATT_ReadMultiCharValues = new BTool.HCICmds.GATTCmds.GATT_ReadMultiCharValues();
        public BTool.HCICmds.GATTCmds.GATT_ReadUsingCharUUID GATT_ReadUsingCharUUID = new BTool.HCICmds.GATTCmds.GATT_ReadUsingCharUUID();
        public BTool.HCICmds.GATTCmds.GATT_ReliableWrites GATT_ReliableWrites = new BTool.HCICmds.GATTCmds.GATT_ReliableWrites();
        public BTool.HCICmds.GATTCmds.GATT_SignedWriteNoRsp GATT_SignedWriteNoRsp = new BTool.HCICmds.GATTCmds.GATT_SignedWriteNoRsp();
        public BTool.HCICmds.GATTCmds.GATT_WriteCharDesc GATT_WriteCharDesc = new BTool.HCICmds.GATTCmds.GATT_WriteCharDesc();
        public BTool.HCICmds.GATTCmds.GATT_WriteCharValue GATT_WriteCharValue = new BTool.HCICmds.GATTCmds.GATT_WriteCharValue();
        public BTool.HCICmds.GATTCmds.GATT_WriteLongCharDesc GATT_WriteLongCharDesc = new BTool.HCICmds.GATTCmds.GATT_WriteLongCharDesc();
        public BTool.HCICmds.GATTCmds.GATT_WriteLongCharValue GATT_WriteLongCharValue = new BTool.HCICmds.GATTCmds.GATT_WriteLongCharValue();
        public BTool.HCICmds.GATTCmds.GATT_WriteNoRsp GATT_WriteNoRsp = new BTool.HCICmds.GATTCmds.GATT_WriteNoRsp();
        public BTool.HCICmds.HCIExtCmds.HCIExt_ClkDivideOnHalt HCIExt_ClkDivideOnHalt = new BTool.HCICmds.HCIExtCmds.HCIExt_ClkDivideOnHalt();
        public BTool.HCICmds.HCIExtCmds.HCIExt_DeclareNvUsage HCIExt_DeclareNvUsage = new BTool.HCICmds.HCIExtCmds.HCIExt_DeclareNvUsage();
        public BTool.HCICmds.HCIExtCmds.HCIExt_Decrypt HCIExt_Decrypt = new BTool.HCICmds.HCIExtCmds.HCIExt_Decrypt();
        public BTool.HCICmds.HCIExtCmds.HCIExt_DisconnectImmed HCIExt_DisconnectImmed = new BTool.HCICmds.HCIExtCmds.HCIExt_DisconnectImmed();
        public BTool.HCICmds.HCIExtCmds.HCIExt_EnablePTM HCIExt_EnablePTM = new BTool.HCICmds.HCIExtCmds.HCIExt_EnablePTM();
        public BTool.HCICmds.HCIExtCmds.HCIExt_EndModemTest HCIExt_EndModemTest = new BTool.HCICmds.HCIExtCmds.HCIExt_EndModemTest();
        public BTool.HCICmds.HCIExtCmds.HCIExt_MapPmIoPort HCIExt_MapPmIoPort = new BTool.HCICmds.HCIExtCmds.HCIExt_MapPmIoPort();
        public BTool.HCICmds.HCIExtCmds.HCIExt_ModemHopTestTx HCIExt_ModemHopTestTx = new BTool.HCICmds.HCIExtCmds.HCIExt_ModemHopTestTx();
        public BTool.HCICmds.HCIExtCmds.HCIExt_ModemTestRx HCIExt_ModemTestRx = new BTool.HCICmds.HCIExtCmds.HCIExt_ModemTestRx();
        public BTool.HCICmds.HCIExtCmds.HCIExt_ModemTestTx HCIExt_ModemTestTx = new BTool.HCICmds.HCIExtCmds.HCIExt_ModemTestTx();
        public BTool.HCICmds.HCIExtCmds.HCIExt_OnePktPerEvt HCIExt_OnePktPerEvt = new BTool.HCICmds.HCIExtCmds.HCIExt_OnePktPerEvt();
        public BTool.HCICmds.HCIExtCmds.HCIExt_PER HCIExt_PER = new BTool.HCICmds.HCIExtCmds.HCIExt_PER();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SaveFreqTune HCIExt_SaveFreqTune = new BTool.HCICmds.HCIExtCmds.HCIExt_SaveFreqTune();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetBDADDR HCIExt_SetBDADDR = new BTool.HCICmds.HCIExtCmds.HCIExt_SetBDADDR();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetFastTxRespTime HCIExt_SetFastTxRespTime = new BTool.HCICmds.HCIExtCmds.HCIExt_SetFastTxRespTime();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetFreqTune HCIExt_SetFreqTune = new BTool.HCICmds.HCIExtCmds.HCIExt_SetFreqTune();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetLocalSupportedFeatures HCIExt_SetLocalSupportedFeatures = new BTool.HCICmds.HCIExtCmds.HCIExt_SetLocalSupportedFeatures();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetMaxDtmTxPower HCIExt_SetMaxDtmTxPower = new BTool.HCICmds.HCIExtCmds.HCIExt_SetMaxDtmTxPower();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetRxGain HCIExt_SetRxGain = new BTool.HCICmds.HCIExtCmds.HCIExt_SetRxGain();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetSCA HCIExt_SetSCA = new BTool.HCICmds.HCIExtCmds.HCIExt_SetSCA();
        public BTool.HCICmds.HCIExtCmds.HCIExt_SetTxPower HCIExt_SetTxPower = new BTool.HCICmds.HCIExtCmds.HCIExt_SetTxPower();
        public BTool.HCICmds.HCIOtherCmds.HCIOther_LEAddDeviceToWhiteList HCIOther_LEAddDeviceToWhiteList = new BTool.HCICmds.HCIOtherCmds.HCIOther_LEAddDeviceToWhiteList();
        public BTool.HCICmds.HCIOtherCmds.HCIOther_LEClearWhiteList HCIOther_LEClearWhiteList = new BTool.HCICmds.HCIOtherCmds.HCIOther_LEClearWhiteList();
        public BTool.HCICmds.HCIOtherCmds.HCIOther_LEConnectionUpdate HCIOther_LEConnectionUpdate = new BTool.HCICmds.HCIOtherCmds.HCIOther_LEConnectionUpdate();
        public BTool.HCICmds.HCIOtherCmds.HCIOther_LERemoveDeviceFromWhiteList HCIOther_LERemoveDeviceFromWhiteList = new BTool.HCICmds.HCIOtherCmds.HCIOther_LERemoveDeviceFromWhiteList();
        public BTool.HCICmds.HCIOtherCmds.HCIOther_ReadRSSI HCIOther_ReadRSSI = new BTool.HCICmds.HCIOtherCmds.HCIOther_ReadRSSI();
        private System.Windows.Forms.Timer initTimer;
        public BTool.HCICmds.L2CAPCmds.L2CAP_ConnParamUpdateReq L2CAP_ConnParamUpdateReq = new BTool.HCICmds.L2CAPCmds.L2CAP_ConnParamUpdateReq();
        public BTool.HCICmds.L2CAPCmds.L2CAP_InfoReq L2CAP_InfoReq = new BTool.HCICmds.L2CAPCmds.L2CAP_InfoReq();
        public BTool.HCICmds.MISCCmds.MISC_GenericCommand MISC_GenericCommand = new BTool.HCICmds.MISCCmds.MISC_GenericCommand();
        public BTool.HCICmds.MISCCmds.MISC_RawTxMessage MISC_RawTxMessage = new BTool.HCICmds.MISCCmds.MISC_RawTxMessage();
        public static string moduleName = "DeviceForm";
        private MsgBox msgBox = new MsgBox();
        private MsgLogForm msgLogForm;
        public int numConnections;
        private System.Windows.Forms.Timer pairBondTimer;
        private Panel plAttributes;
        private Panel plLog;
        private Panel plUserTabs;
        private Thread processRxProc;
        private FP_ReceiveDataInd ReceiveDataInd;
        private System.Windows.Forms.Timer scanTimer;
        private SplitContainer scTopBottom;
        private SplitContainer scTopLeftRight;
        public SendCmds sendCmds;
        private SharedObjects sharedObjs = new SharedObjects();
        public ThreadMgr threadMgr;
        public BTool.HCICmds.UTILCmds.UTIL_ForceBoot UTIL_ForceBoot = new BTool.HCICmds.UTILCmds.UTIL_ForceBoot();
        public BTool.HCICmds.UTILCmds.UTIL_NVRead UTIL_NVRead = new BTool.HCICmds.UTILCmds.UTIL_NVRead();
        public BTool.HCICmds.UTILCmds.UTIL_NVWrite UTIL_NVWrite = new BTool.HCICmds.UTILCmds.UTIL_NVWrite();
        public BTool.HCICmds.UTILCmds.UTIL_Reset UTIL_Reset = new BTool.HCICmds.UTILCmds.UTIL_Reset();

        public event EventHandler BDAddressNotify;

        public event EventHandler ChangeActiveRoot;

        public event EventHandler CloseActiveDevice;

        public event EventHandler ConnectionNotify;

        public event EventHandler DisconnectionNotify;

        public DeviceForm()
        {
            this.devInfo.devForm = this;
            this.connectInfo.bDA = "00:00:00:00:00:00";
            this.connectInfo.handle = 0;
            this.connectInfo.addrType = 0;
            this.disconnectInfo.bDA = "00:00:00:00:00:00";
            this.disconnectInfo.handle = 0;
            this.disconnectInfo.addrType = 0;
            this.Connections.Clear();
            this.commMgr.InitCommManager();
            this.msgLogForm = new MsgLogForm(this);
            this.commSelectForm = new CommSelectForm();
            this.InitializeComponent();
            this.Text = FormMain.ProgramTitle + FormMain.ProgramVersion;
            this.threadMgr = new ThreadMgr(this);
            this.sendCmds = new SendCmds(this);
            this.attrData.sendAutoCmds = false;
            this.attributesForm = new AttributesForm(this);
            this.devTabsForm = new DeviceTabsForm(this);
            this.LoadUserInitializeValues();
            this.LoadUserSettings();
            this.sendCmds.DisplayMsgCallback = new DisplayMsgDelegate(this.DisplayMsg);
            this.threadMgr.txDataOut.DeviceTxDataCallback = new DeviceTxDataDelegate(this.DeviceTxData);
            this.threadMgr.txDataOut.DisplayMsgCallback = new DisplayMsgDelegate(this.DisplayMsg);
            this.threadMgr.rxDataIn.DeviceRxDataCallback = new DeviceRxDataDelegate(this.DeviceRxData);
            this.threadMgr.rxTxMgr.HandleRxTxMessageCallback = new HandleRxTxMessageDelegate(this.HandleRxTxMessage);
            this.dspTxCmds.DisplayMsgCallback = new DisplayMsgDelegate(this.DisplayMsg);
            this.dspTxCmds.DisplayMsgTimeCallback = new DisplayMsgTimeDelegate(this.DisplayMsgTime);
            this.attributesForm.DisplayMsgCallback = new DisplayMsgDelegate(this.DisplayMsg);
            this.msgLogForm.DisplayMsgCallback = new DisplayMsgDelegate(this.DisplayMsg);
            this.threadMgr.Init(this);
            this.msgLogForm.TopLevel = false;
            this.msgLogForm.Parent = this.plLog;
            this.msgLogForm.Visible = true;
            this.msgLogForm.Dock = DockStyle.Fill;
            this.msgLogForm.ControlBox = false;
            this.msgLogForm.ShowIcon = false;
            this.msgLogForm.FormBorderStyle = FormBorderStyle.None;
            this.msgLogForm.StartPosition = FormStartPosition.Manual;
            this.msgLogForm.Show();
            this.devTabsForm.TopLevel = false;
            this.devTabsForm.Parent = this.plUserTabs;
            this.devTabsForm.Visible = true;
            this.devTabsForm.Dock = DockStyle.Fill;
            this.devTabsForm.ControlBox = false;
            this.devTabsForm.ShowIcon = false;
            this.devTabsForm.FormBorderStyle = FormBorderStyle.None;
            this.devTabsForm.StartPosition = FormStartPosition.Manual;
            this.devTabsForm.Show();
            this.attributesForm.TopLevel = false;
            this.attributesForm.Parent = this.plAttributes;
            this.attributesForm.Visible = true;
            this.attributesForm.Dock = DockStyle.Fill;
            this.attributesForm.ControlBox = false;
            this.attributesForm.ShowIcon = false;
            this.attributesForm.FormBorderStyle = FormBorderStyle.None;
            this.attributesForm.StartPosition = FormStartPosition.Manual;
            this.attributesForm.Show();
        }

        private void deviceForm_Activated(object sender, EventArgs e)
        {
            this.ChangeActiveRoot(this, null);
        }

        private void DeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DeviceFormClose(true);
        }

        private void DeviceForm_Load(object sender, EventArgs e)
        {
            if (this.sharedObjs.IsMonoRunning())
            {
                this.scTopBottom.SplitterDistance = 550;
            }
            else
            {
                this.scTopBottom.SplitterDistance = 530;
            }
        }

        private void DeviceForm_LocationChanged(object sender, EventArgs e)
        {
            base.Location = new Point(0, 0);
        }

        public void DeviceFormClose(bool closeDevice)
        {
            if (closeDevice && !this.formClosing)
            {
                this.formClosing = true;
                this.CloseActiveDevice(this, null);
            }
            this.threadMgr.PauseThreads();
            this.threadMgr.WaitForPause();
            this.threadMgr.ClearQueues();
            this.commMgr.ClosePort();
            if (this.processRxProc != null)
            {
                while (this.processRxProc.IsAlive)
                {
                }
            }
            this.msgLogForm.ResetMsgNumber();
            this.threadMgr.ExitThreads();
            this.SaveUserSettings();
        }

        public bool DeviceFormInit()
        {
            this.commSelectForm.ShowDialog();
            if (this.commSelectForm.DialogResult == DialogResult.OK)
            {
                this.commMgr.PortName = this.commSelectForm.cbPorts.Text;
                this.commMgr.BaudRate = this.commSelectForm.cbBaud.Text;
                this.commMgr.DataBits = this.commSelectForm.cbDataBits.Text;
                this.commMgr.Parity = this.commSelectForm.cbParity.Text;
                this.commMgr.StopBits = this.commSelectForm.cbStopBits.Text;
                this.commMgr.HandShake = (Handshake) this.commSelectForm.cbFlow.SelectedIndex;
                this.commMgr.CurrentTransmissionType = CommManager.TransmissionType.Hex;
                this.commMgr.DisplayMsgCallback = new DisplayMsgDelegate(this.DisplayMsg);
                if (this.commMgr.OpenPort())
                {
                    this.Text = this.commSelectForm.cbPorts.Text;
                    this.devInfo.devName = this.commMgr.PortName;
                    this.devInfo.connectStatus = "None";
                    this.devInfo.comPortInfo.baudRate = this.commMgr.BaudRate;
                    this.devInfo.comPortInfo.comPort = this.commMgr.PortName;
                    this.devInfo.comPortInfo.flow = this.commSelectForm.cbFlow.Text;
                    this.devInfo.comPortInfo.dataBits = this.commMgr.DataBits;
                    this.devInfo.comPortInfo.parity = this.commMgr.Parity;
                    this.devInfo.comPortInfo.stopBits = this.commMgr.StopBits;
                    this.ReceiveDataInd = new FP_ReceiveDataInd(this.RxDataHandler);
                    this.commMgr.RxDataInd = this.ReceiveDataInd;
                    this.processRxProc = new Thread(new ThreadStart(this.ProcessRxProc));
                    this.processRxProc.Name = "ProcessRxProcThread";
                    this.processRxProc.Start();
                    while (!this.processRxProc.IsAlive)
                    {
                    }
                    return true;
                }
                string msg = string.Format("Failed Connecting To {0}\n", this.commSelectForm.cbPorts.SelectedItem);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.DisplayMsg(SharedAppObjs.MsgType.Error, msg);
                return false;
            }
            return false;
        }

        private void DeviceRxData(RxDataIn rxDataIn)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.BeginInvoke(new DeviceRxDataDelegate(this.DeviceRxData), new object[] { rxDataIn });
                }
                catch
                {
                }
            }
            else if (!this.formClosing)
            {
                RxTxMgrData data = new RxTxMgrData();
                data.rxDataIn = rxDataIn;
                data.txDataOut = null;
                this.threadMgr.rxTxMgr.dataQ.AddQTail(data);
            }
        }

        private void DeviceTxData(TxDataOut txDataOut)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.BeginInvoke(new DeviceTxDataDelegate(this.DeviceTxData), new object[] { txDataOut });
                }
                catch
                {
                }
            }
            else if (!this.formClosing)
            {
                RxTxMgrData data = new RxTxMgrData();
                data.rxDataIn = null;
                data.txDataOut = txDataOut;
                this.threadMgr.rxTxMgr.dataQ.AddQTail(data);
            }
        }

        public void DisplayMsg(SharedAppObjs.MsgType msgType, string msg)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.BeginInvoke(new DisplayMsgDelegate(this.DisplayMsg), new object[] { msgType, msg });
                }
                catch
                {
                }
            }
            else
            {
                this.msgLogForm.DisplayLogMsg(msgType, msg, null);
            }
        }

        public void DisplayMsgTime(SharedAppObjs.MsgType msgType, string msg, string time)
        {
            this.dspMsgMutex.WaitOne();
            if (base.InvokeRequired)
            {
                try
                {
                    base.BeginInvoke(new DisplayMsgTimeDelegate(this.DisplayMsgTime), new object[] { msgType, msg, time });
                }
                catch
                {
                }
            }
            else
            {
                this.msgLogForm.DisplayLogMsg(msgType, msg, time);
            }
            this.dspMsgMutex.ReleaseMutex();
        }

        private void DisplayRxCmd(RxDataIn rxDataIn, bool displayBytes)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new DisplayRxCmdDelegate(this.DisplayRxCmd), new object[] { rxDataIn, displayBytes });
                return;
            }
            byte type = rxDataIn.type;
            ushort cmdOpcode = rxDataIn.cmdOpcode;
            ushort eventOpcode = rxDataIn.eventOpcode;
            byte length = rxDataIn.length;
            byte[] data = rxDataIn.data;
            string msg = string.Empty;
            string str2 = string.Empty;
            byte[] addr = new byte[6];
            uint num5 = 0;
            if (type == 4)
            {
                msg = msg + string.Format("-Type\t\t: 0x{0:X2} ({1:S})\n-EventCode\t: 0x{2:X2} ({3:S})\n-Data Length\t: 0x{4:X2} ({5:D}) bytes(s)\n", new object[] { type, this.devUtils.GetPacketTypeStr(type), cmdOpcode, this.devUtils.GetOpCodeName(cmdOpcode), length, length });
            }
            else
            {
                msg = msg + string.Format("-Type\t\t: 0x{0:X2} ({1:S})\n-OpCode\t\t: 0x{2:X4} ({3:S})\n-Data Length\t: 0x{4:X2} ({5:D}) bytes(s)\n", new object[] { type, this.devUtils.GetPacketTypeStr(type), cmdOpcode, this.devUtils.GetOpCodeName(cmdOpcode), length, length });
            }
            int index = 0;
            byte bits = 0;
            ushort num8 = 0;
            uint num9 = 0;
            string str3 = string.Empty;
            bool dataErr = false;
            switch (cmdOpcode)
            {
                case 14:
                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                    if (!dataErr)
                    {
                        msg = msg + string.Format(" Packets\t\t: 0x{0:X2} ({1:D})\n", bits, bits);
                        ushort num10 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        if (!dataErr)
                        {
                            msg = msg + string.Format(" Opcode\t\t: 0x{0:X4} ({1:S})\n", num10, this.devUtils.GetOpCodeName(num10));
                            switch (num10)
                            {
                                case 0x2010:
                                case 0x2011:
                                case 0x2012:
                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Status\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetStatusStr(bits));
                                    }
                                    goto Label_3170;

                                case 0x1405:
                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Status\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetStatusStr(bits));
                                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                        if (!dataErr)
                                        {
                                            this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" RSSI\t\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                            }
                                        }
                                    }
                                    goto Label_3170;

                                case 0xfc0e:
                                case 0xfc0f:
                                case 0xfc10:
                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Status\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetHCIExtStatusStr(bits));
                                    }
                                    goto Label_3170;
                            }
                            this.devUtils.BuildRawDataStr(data, ref msg, data.Length);
                        }
                    }
                    goto Label_3170;

                case 0x13:
                    if (length == 5)
                    {
                        this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                        if (dataErr)
                        {
                            goto Label_3170;
                        }
                        msg = msg + string.Format(" NumOfHandles\t: 0x{0:X2} ({1:D})\n", bits, bits);
                        this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                        if (dataErr)
                        {
                            goto Label_3170;
                        }
                        this.dataUtils.Unload16Bits(data, ref index, ref num8, ref dataErr, false);
                        if (dataErr)
                        {
                            goto Label_3170;
                        }
                        msg = msg + string.Format(" PktsCompleted\t: 0x{0:X4} ({1:D})\n", num8, num8);
                    }
                    this.devUtils.BuildRawDataStr(data, ref msg, data.Length);
                    goto Label_3170;

                case 0xff:
                {
                    ushort num11 = (ushort) (eventOpcode & 0x380);
                    num11 = (ushort) (num11 >> 7);
                    byte status = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                    if (!dataErr)
                    {
                        byte num13;
                        int uuidLength;
                        int num18;
                        byte num26;
                        string hCIExtStatusStr = string.Empty;
                        if (num11 == 0)
                        {
                            hCIExtStatusStr = this.devUtils.GetHCIExtStatusStr(status);
                        }
                        else
                        {
                            hCIExtStatusStr = this.devUtils.GetStatusStr(status);
                        }
                        msg = msg + string.Format(" Event\t\t: 0x{0:X4} ({1:S})\n Status\t\t: 0x{2:X2} ({3:S})\n", new object[] { eventOpcode, this.devUtils.GetOpCodeName(eventOpcode), status, hCIExtStatusStr });
                        ushort num30 = eventOpcode;
                        switch (num30)
                        {
                            case 0x48b:
                                this.dataUtils.Unload16Bits(data, ref index, ref num8, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" Cmd Opcode\t: 0x{0:X4} ({1:S})\n", num8, this.devUtils.GetOpCodeName(num8));
                                }
                                goto Label_3170;

                            case 0x493:
                                this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    this.dataUtils.Unload16Bits(data, ref index, ref num8, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Result\t\t: 0x{0:X4} ({1:S})\n", num8, this.devUtils.GetL2CapConnParamUpdateResultStr(num8));
                                    }
                                }
                                goto Label_3170;

                            case 0x400:
                            case 0x401:
                            case 0x402:
                            case 0x403:
                            case 0x404:
                            case 0x405:
                            case 0x406:
                            case 0x407:
                            case 0x408:
                            case 0x409:
                            case 0x40a:
                            case 0x40b:
                            case 0x40c:
                            case 0x40d:
                            case 0x40e:
                            case 0x40f:
                            case 0x410:
                            case 0x411:
                            case 0x412:
                            case 0x413:
                            case 0x414:
                                this.dataUtils.Unload16Bits(data, ref index, ref num8, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" Cmd Opcode\t: 0x{0:X4} ({1:S})\n", num8, this.devUtils.GetOpCodeName(num8));
                                }
                                goto Label_3170;

                            case 0x481:
                                this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    this.dataUtils.Unload16Bits(data, ref index, ref num8, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" RejectReason\t: 0x{0:X4} ({1:S})\n", num8, this.devUtils.GetL2CapRejectReasonsStr(num8));
                                    }
                                }
                                goto Label_3170;

                            case 0x501:
                                num13 = 0;
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    byte num14 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" ReqOpcode\t: 0x{0:X2} ({1:S})\n", num14, this.devUtils.GetHciReqOpCodeStr(num14));
                                        num8 = this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                        if (!dataErr)
                                        {
                                            byte num15 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" ErrorCode\t: 0x{0:X2} ({1:S})\n", num15, this.devUtils.GetShortErrorStatusStr(num15)) + string.Format("       \t\t: {0:S}\n", this.devUtils.GetErrorStatusStr(num15));
                                                if (this.devTabsForm.GetSelectedTab() == 1)
                                                {
                                                    switch (num14)
                                                    {
                                                        case 10:
                                                        case 8:
                                                            this.devTabsForm.SetTbReadStatusText(string.Format("{0:S}", this.devUtils.GetShortErrorStatusStr(num15)));
                                                            break;
                                                    }
                                                    if (num14 == 0x12)
                                                    {
                                                        this.devTabsForm.SetTbWriteStatusText(string.Format("{0:S}", this.devUtils.GetShortErrorStatusStr(num15)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x502:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" ClientRxMTU\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                    }
                                }
                                goto Label_3170;

                            case 0x503:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" ServerRxMTU\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                    }
                                }
                                goto Label_3170;

                            case 0x504:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddStartEndHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x505:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    byte num16 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Format\t\t: 0x{0:X2} ({1:S})\n", num16, this.devUtils.GetFindFormatStr(num16));
                                        uuidLength = this.devUtils.GetUuidLength(num16, ref dataErr);
                                        if (!dataErr)
                                        {
                                            uuidLength += 2;
                                            num18 = length - index;
                                            msg = msg + this.devUtils.UnloadHandleValueData(data, ref index, num18, uuidLength, ref dataErr, "Uuid");
                                            if (!dataErr)
                                            {
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x506:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddStartEndHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" Type\t\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x507:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    if (num13 >= 2)
                                    {
                                        int num19 = num13 / 2;
                                        for (num5 = 0; (num5 < num19) && !dataErr; num5++)
                                        {
                                            num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                            if (dataErr)
                                            {
                                                break;
                                            }
                                            msg = msg + string.Format(" Handle\t\t: {0:X2}:{1:X2}\n", (byte) (num8 & 0xff), (byte) (num8 >> 8));
                                        }
                                    }
                                    if (dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x508:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddStartEndHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Type\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                        if (!dataErr)
                                        {
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x509:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    uuidLength = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Length\t\t: 0x{0:X2} ({1:D})\n", uuidLength, uuidLength);
                                        num13 = (byte) (num13 - 1);
                                        if (uuidLength != 0)
                                        {
                                            string handleStr = string.Empty;
                                            string valueStr = string.Empty;
                                            msg = msg + this.devUtils.UnloadHandleValueData(data, ref index, num13, uuidLength, ref handleStr, ref valueStr, ref dataErr, "Data");
                                            if (!dataErr && (this.devTabsForm.GetSelectedTab() == 1))
                                            {
                                                this.devTabsForm.SetTbReadValueTag(valueStr);
                                                if (!this.devTabsForm.GetRbASCIIReadChecked())
                                                {
                                                    if (this.devTabsForm.GetRbDecimalReadChecked())
                                                    {
                                                        this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(valueStr, SharedAppObjs.StringType.DEC));
                                                    }
                                                    else
                                                    {
                                                        this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(valueStr, SharedAppObjs.StringType.HEX));
                                                    }
                                                }
                                                else
                                                {
                                                    this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(valueStr, SharedAppObjs.StringType.ASCII));
                                                }
                                                if (!string.IsNullOrEmpty(handleStr))
                                                {
                                                    this.devTabsForm.SetTbReadAttrHandleText(handleStr);
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x50a:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x50b:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    str2 = string.Empty;
                                    for (num5 = 0; (num5 < num13) && !dataErr; num5++)
                                    {
                                        bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                        str2 = str2 + string.Format("{0:X2} ", bits);
                                    }
                                    if (!dataErr)
                                    {
                                        str2.Trim();
                                        msg = msg + string.Format(" Value\t\t: {0:S}\n", str2);
                                        if (this.devTabsForm.GetSelectedTab() == 1)
                                        {
                                            this.devTabsForm.SetTbReadValueTag(str2);
                                            if (this.devTabsForm.GetRbASCIIReadChecked())
                                            {
                                                this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(str2, SharedAppObjs.StringType.ASCII));
                                            }
                                            else if (this.devTabsForm.GetRbDecimalReadChecked())
                                            {
                                                this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(str2, SharedAppObjs.StringType.DEC));
                                            }
                                            else
                                            {
                                                this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(str2, SharedAppObjs.StringType.HEX));
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x50c:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddHandleOffset(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x50d:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                    if (!dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x50e:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    for (num5 = 0; (num5 < num13) && !dataErr; num5++)
                                    {
                                        str2 = str2 + string.Format("{0:X2} ", this.dataUtils.Unload8Bits(data, ref index, ref dataErr));
                                    }
                                    if (!dataErr)
                                    {
                                        str2.Trim();
                                        msg = msg + string.Format(" Handles\t\t: {0:S}\n", str2);
                                    }
                                }
                                goto Label_3170;

                            case 0x50f:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    num5 = 0;
                                    while ((num5 < num13) && !dataErr)
                                    {
                                        str2 = str2 + string.Format("{0:X2} ", this.dataUtils.Unload8Bits(data, ref index, ref dataErr));
                                        num5++;
                                    }
                                    if (!dataErr)
                                    {
                                        str2.Trim();
                                        msg = msg + string.Format(" Values\t\t: {0:S}\n", str2);
                                        if (this.devTabsForm.GetSelectedTab() == 1)
                                        {
                                            this.devTabsForm.SetTbReadValueTag(str2);
                                            if (this.devTabsForm.GetRbASCIIReadChecked())
                                            {
                                                this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(str2, SharedAppObjs.StringType.ASCII));
                                            }
                                            else if (this.devTabsForm.GetRbDecimalReadChecked())
                                            {
                                                this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(str2, SharedAppObjs.StringType.DEC));
                                            }
                                            else
                                            {
                                                this.devTabsForm.SetTbReadValueText(this.devUtils.HexStr2UserDefinedStr(str2, SharedAppObjs.StringType.HEX));
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x510:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddStartEndHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" GroupType\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                        if (!dataErr)
                                        {
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x511:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    byte num20 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Length\t\t: 0x{0:X2} ({1:D})\n", num20, num20);
                                        if (num20 != 0)
                                        {
                                            uuidLength = num20;
                                            num18 = ((length - 3) - index) + 1;
                                            msg = msg + string.Format(" DataList\t:\n{0:S}\n", this.devUtils.UnloadHandleHandleValueData(data, ref index, num18, uuidLength, ref dataErr));
                                            if (!dataErr)
                                            {
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x512:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Signature\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetSigAuthStr(bits));
                                        bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" Command\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapYesNoStr(bits));
                                            this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                            if (!dataErr)
                                            {
                                                str2.Trim();
                                                msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                                if (!dataErr)
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x513:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) == 0) || !dataErr)
                                {
                                }
                                goto Label_3170;

                            case 0x516:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddHandleOffset(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                        if (!dataErr)
                                        {
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x517:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddHandleOffset(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                        if (!dataErr)
                                        {
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x518:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    msg = msg + string.Format(" Flages\t\t: 0x{0:X2}\n", this.dataUtils.Unload8Bits(data, ref index, ref dataErr));
                                    if (!dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x519:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) == 0) || !dataErr)
                                {
                                }
                                goto Label_3170;

                            case 0x51b:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                {
                                    this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                        if (!dataErr)
                                        {
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x51d:
                                try
                                {
                                    if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && !dataErr)
                                    {
                                        this.dspCmdUtils.AddHandle(data, ref index, ref dataErr, ref msg);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, ((length - 3) - index) + 1, ref dataErr));
                                            if (!dataErr)
                                            {
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    string str7 = string.Format("Message Data Conversion Issue.\n\n{0}\n", exception.Message);
                                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str7);
                                    this.DisplayMsg(SharedAppObjs.MsgType.Error, "Could Not Convert All The Data In The Following Message\n(Message Is Missing Data Bytes To Process)\n");
                                    dataErr = true;
                                }
                                goto Label_3170;

                            case 0x51e:
                                if (((num13 = this.devUtils.UnloadAttMsgHeader(ref data, ref index, ref msg, ref dataErr)) != 0) && dataErr)
                                {
                                }
                                goto Label_3170;

                            case 0x580:
                                this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" PduLen\t\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                        num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" AttrHandle\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                            this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" Value\t\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x600:
                                str3 = this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" DevAddr\t\t: {0:S}\n", str3);
                                    this.OnBDAddressNotify(str3);
                                    num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" DataPktLen\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                        bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" NumDataPkts\t: 0x{0:X2} ({1:D})\n", bits, bits) + string.Format(" IRK\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" CSRK\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr));
                                                if (!dataErr && !this.DeviceStarted)
                                                {
                                                    this.StopTimer(EventType.Init);
                                                    this.devTabsForm.ShowProgress(false);
                                                    this.devTabsForm.UserTabAccess(true);
                                                    this.DeviceStarted = true;
                                                    this.devTabsForm.GetConnectionParameters();
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x601:
                            {
                                this.StopTimer(EventType.Scan);
                                this.devTabsForm.ShowProgress(false);
                                if ((status != 0) && (status != 0x30))
                                {
                                    string str8 = string.Format("GAP_DeviceDiscoveryDone Failed.\n{0}\n", hCIExtStatusStr);
                                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str8);
                                }
                                byte num21 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" NumDevs\t: 0x{0:X2} ({1:D})\n", num21, num21);
                                    if (num21 > 0)
                                    {
                                        for (num5 = 0; (num5 < num21) && !dataErr; num5++)
                                        {
                                            DeviceTabsForm.LinkSlave slave;
                                            msg = msg + string.Format(" Device #{0:D}\n", num5);
                                            byte num22 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                            msg = msg + string.Format(" EventType\t: 0x{0:X2} ({1:S})\n", num22, this.devUtils.GetGapEventTypeStr(num22));
                                            byte num23 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                            msg = msg + string.Format(" AddrType\t: 0x{0:X2} ({1:S})\n", num23, this.devUtils.GetGapAddrTypeStr(num23)) + string.Format(" Addr\t\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr));
                                            slave.slaveBDA = addr;
                                            slave.addrBDA = "";
                                            slave.addrType = (HCICmds.GAP_AddrType) num23;
                                            this.devTabsForm.AddSlaveDevice(slave);
                                        }
                                        if (!dataErr)
                                        {
                                        }
                                    }
                                }
                                goto Label_3170;
                            }
                            case 0x602:
                                this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapAdventAdTypeStr(bits));
                                }
                                goto Label_3170;

                            case 0x603:
                            case 0x604:
                                goto Label_3170;

                            case 0x605:
                                this.StopTimer(EventType.Establish);
                                this.devTabsForm.ShowProgress(false);
                                bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" DevAddrType\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapAddrTypeStr(bits));
                                    str3 = this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" DevAddr\t\t: {0:S}\n", str3);
                                        num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                            if (status == 0)
                                            {
                                                ConnectInfo tmpConnectInfo = new ConnectInfo();
                                                tmpConnectInfo.handle = num8;
                                                tmpConnectInfo.addrType = bits;
                                                tmpConnectInfo.bDA = str3;
                                                this.OnConnectionNotify(ref tmpConnectInfo);
                                                this.devTabsForm.SetConnHandles(tmpConnectInfo.handle);
                                            }
                                            num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" ConnInterval\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                                num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                                if (!dataErr)
                                                {
                                                    msg = msg + string.Format(" ConnLatency\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                                    num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                                    if (!dataErr)
                                                    {
                                                        msg = msg + string.Format(" ConnTimeout\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                                        bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                        if (!dataErr)
                                                        {
                                                            msg = msg + string.Format(" ClockAccuracy\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x606:
                                num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" Reason\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapTerminationReasonStr(bits));
                                        if (status == 0)
                                        {
                                            ConnectInfo tmpDisconnectInfo = new ConnectInfo();
                                            tmpDisconnectInfo.handle = num8;
                                            tmpDisconnectInfo.bDA = "00:00:00:00:00:00";
                                            tmpDisconnectInfo.addrType = 0;
                                            this.OnDisconnectionNotify(ref tmpDisconnectInfo);
                                            this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.NotConnected);
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x607:
                                this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" ConnInterval\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                        num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" ConnLatency\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                            num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" ConnTimeout\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x608:
                                this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" AddrType\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapAddrTypeStr(bits)) + string.Format(" NewRandAddr\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr));
                                    if (!dataErr)
                                    {
                                    }
                                }
                                goto Label_3170;

                            case 0x609:
                                this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" AddrType\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapAddrTypeStr(bits)) + string.Format(" DevAddr\t\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr));
                                    if (!dataErr)
                                    {
                                        num9 = this.dataUtils.Unload32Bits(data, ref index, ref dataErr, false);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" SignCounter\t: 0x{0:X8} ({1:D})\n", num9, num9);
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x60a:
                            {
                                HCICmds.GAPEvts.GAP_AuthenticationComplete complete = new HCICmds.GAPEvts.GAP_AuthenticationComplete();
                                this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    complete.connHandle = num8;
                                    bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" AuthState\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapAuthReqStr(bits));
                                        complete.authState = bits;
                                        bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" SecInf.Enable\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                            complete.secInfo_enable = (HCICmds.GAP_EnableDisable) bits;
                                            bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" SecInf.LTKSize\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                complete.secInfo_LTKsize = bits;
                                                str3 = this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    msg = msg + string.Format(" SecInf.LTK\t: {0:S}\n", str3);
                                                    complete.secInfo_LTK = str3;
                                                    num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                                    if (!dataErr)
                                                    {
                                                        msg = msg + string.Format(" SecInf.DIV\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                                        complete.secInfo_DIV = num8;
                                                        str3 = this.devUtils.UnloadColonData(data, ref index, 8, ref dataErr);
                                                        if (!dataErr)
                                                        {
                                                            msg = msg + string.Format(" SecInf.Rand\t: {0:S}\n", str3);
                                                            complete.secInfo_RAND = str3;
                                                            bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                            if (!dataErr)
                                                            {
                                                                msg = msg + string.Format(" DSInf.Enable\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                                complete.devSecInfo_enable = (HCICmds.GAP_EnableDisable) bits;
                                                                bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                                if (!dataErr)
                                                                {
                                                                    msg = msg + string.Format(" DSInf.LTKSize\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                                    complete.devSecInfo_LTKsize = bits;
                                                                    str3 = this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr);
                                                                    if (!dataErr)
                                                                    {
                                                                        msg = msg + string.Format(" DSInf.LTK\t: {0:S}\n", str3);
                                                                        complete.devSecInfo_LTK = str3;
                                                                        num8 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                                                        if (!dataErr)
                                                                        {
                                                                            msg = msg + string.Format(" DSInf.DIV\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                                                            complete.devSecInfo_DIV = num8;
                                                                            str3 = this.devUtils.UnloadColonData(data, ref index, 8, ref dataErr);
                                                                            if (!dataErr)
                                                                            {
                                                                                msg = msg + string.Format(" DSInf.Rand\t: {0:S}\n", str3);
                                                                                complete.devSecInfo_RAND = str3;
                                                                                bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                                                if (!dataErr)
                                                                                {
                                                                                    msg = msg + string.Format(" IdInfo.Enable\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                                                    complete.idInfo_enable = (HCICmds.GAP_EnableDisable) bits;
                                                                                    str3 = this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr);
                                                                                    if (!dataErr)
                                                                                    {
                                                                                        msg = msg + string.Format(" IdInfo.IRK\t: {0:S}\n", str3);
                                                                                        complete.idInfo_IRK = str3;
                                                                                        str3 = this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr);
                                                                                        if (!dataErr)
                                                                                        {
                                                                                            msg = msg + string.Format(" IdInfo.BD_Addr\t: {0:S}\n", str3);
                                                                                            complete.idInfo_BdAddr = str3;
                                                                                            bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                                                            if (!dataErr)
                                                                                            {
                                                                                                msg = msg + string.Format(" SignInfo.Enable\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                                                                complete.signInfo_enable = (HCICmds.GAP_EnableDisable) bits;
                                                                                                str3 = this.devUtils.UnloadColonData(data, ref index, 0x10, ref dataErr);
                                                                                                if (!dataErr)
                                                                                                {
                                                                                                    msg = msg + string.Format(" SignInfo.CSRK\t: {0:S}\n", str3);
                                                                                                    complete.signInfo_CSRK = str3;
                                                                                                    num9 = this.dataUtils.Unload32Bits(data, ref index, ref dataErr, false);
                                                                                                    if (!dataErr)
                                                                                                    {
                                                                                                        msg = msg + string.Format(" SignCounter\t: 0x{0:X8} ({1:D})\n", num9, num9);
                                                                                                        complete.signCounter = num9;
                                                                                                        if (this.devTabsForm.GetSelectedTab() == 2)
                                                                                                        {
                                                                                                            this.StopTimer(EventType.PairBond);
                                                                                                            this.devTabsForm.ShowProgress(false);
                                                                                                            if (status == 0x17)
                                                                                                            {
                                                                                                                this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.NotPaired);
                                                                                                            }
                                                                                                            else if (status == 4)
                                                                                                            {
                                                                                                                this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.PasskeyIncorrect);
                                                                                                            }
                                                                                                            else if (status == 0)
                                                                                                            {
                                                                                                                bits = 1;
                                                                                                                if ((complete.authState & bits) == bits)
                                                                                                                {
                                                                                                                    this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.DevicesPairedBonded);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.DevicesPaired);
                                                                                                                }
                                                                                                                bits = 4;
                                                                                                                if ((complete.authState & bits) == bits)
                                                                                                                {
                                                                                                                    this.devTabsForm.SetAuthenticatedBond(true);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    this.devTabsForm.SetAuthenticatedBond(false);
                                                                                                                }
                                                                                                                this.devTabsForm.SetGapAuthCompleteInfo(complete);
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
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;
                            }
                            case 0x60b:
                                msg = msg + string.Format(" DevAddr\t\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr));
                                if (!dataErr)
                                {
                                    this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                    if (!dataErr)
                                    {
                                        this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" UiInput\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapUiInputStr(bits));
                                            if ((this.devTabsForm.GetSelectedTab() == 2) && (bits == 1))
                                            {
                                                this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.PasskeyNeeded);
                                            }
                                            this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" UiOutput\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapUiOutputStr(bits));
                                                if (this.devTabsForm.GetSelectedTab() == 2)
                                                {
                                                    this.StopTimer(EventType.PairBond);
                                                    this.devTabsForm.ShowProgress(false);
                                                    this.devTabsForm.UsePasskeySecurity((HCICmds.GAP_UiOutput) bits);
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x60c:
                                this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" DevAddr\t\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr));
                                    if (!dataErr)
                                    {
                                        this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" AuthReq\t\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x60d:
                            {
                                byte num24 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" EventType\t: 0x{0:X2} ({1:S})\n", num24, this.devUtils.GetGapEventTypeStr(num24));
                                    byte num25 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" AddrType\t: 0x{0:X2} ({1:S})\n", num25, this.devUtils.GetGapAddrTypeStr(num25)) + string.Format(" Addr\t\t: {0:S}\n", this.devUtils.UnloadDeviceAddr(data, ref addr, ref index, false, ref dataErr));
                                        if (!dataErr)
                                        {
                                            bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" Rssi\t\t: 0x{0:X2} ({1:D})\n", bits, bits);
                                                num26 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    msg = msg + string.Format(" DataLength\t: 0x{0:X2} ({1:D})\n", num26, num26);
                                                    if (num26 != 0)
                                                    {
                                                        msg = msg + string.Format(" Data\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, num26, ref dataErr));
                                                        if (!dataErr && ((num24 == 0) || (num24 == 4)))
                                                        {
                                                            DeviceTabsForm.LinkSlave slave2;
                                                            slave2.slaveBDA = addr;
                                                            slave2.addrBDA = "";
                                                            slave2.addrType = (HCICmds.GAP_AddrType) num25;
                                                            this.devTabsForm.AddSlaveDevice(slave2);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;
                            }
                            case 0x60e:
                                this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr && (this.devTabsForm.GetSelectedTab() == 2))
                                {
                                    this.StopTimer(EventType.PairBond);
                                    this.devTabsForm.ShowProgress(false);
                                    if (status != 0)
                                    {
                                        this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.NotPaired);
                                        string str9 = string.Format("GAP_BondComplete: Failed.\n{0}\n", hCIExtStatusStr);
                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str9);
                                    }
                                    else
                                    {
                                        this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.DevicesPairedBonded);
                                    }
                                    this.devTabsForm.PairBondUserInputControl();
                                }
                                goto Label_3170;

                            case 0x60f:
                                this.dspCmdUtils.AddConnectHandle(data, ref index, ref dataErr, ref msg);
                                if (!dataErr)
                                {
                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" IOCap\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapIOCapsStr(bits));
                                        this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                        if (!dataErr)
                                        {
                                            msg = msg + string.Format(" OobDataFlag\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapOobDataFlagStr(bits));
                                            this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                            if (!dataErr)
                                            {
                                                msg = msg + string.Format(" AuthReq\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapAuthReqStr(bits));
                                                bits = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                                if (!dataErr)
                                                {
                                                    msg = msg + string.Format(" MaxEncKeySiz\t: 0x{0:X4} ({1:D})\n", bits, bits);
                                                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                                                    if (!dataErr)
                                                    {
                                                        msg = msg + string.Format(" KeyDist\t\t: 0x{0:X2} ({1:S})\n", bits, this.devUtils.GetGapKeyDiskStr(bits));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                goto Label_3170;

                            case 0x67f:
                            {
                                ushort num27 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                                if (!dataErr)
                                {
                                    msg = msg + string.Format(" OpCode\t\t: 0x{0:X4} ({1:S})\n", num27, this.devUtils.GetOpCodeName(num27));
                                    num26 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                                    if (!dataErr)
                                    {
                                        msg = msg + string.Format(" DataLength\t: 0x{0:X2} ({1:D})\n", num26, num26);
                                        num30 = num27;
                                        if (num30 > 0xfd96)
                                        {
                                            switch (num30)
                                            {
                                                case 0xfd9b:
                                                case 0xfd9d:
                                                case 0xfdb0:
                                                case 0xfdb2:
                                                case 0xfdb6:
                                                case 0xfdb8:
                                                case 0xfdba:
                                                case 0xfdbc:
                                                case 0xfdbe:
                                                case 0xfdc0:
                                                case 0xfdc2:
                                                case 0xfdfc:
                                                case 0xfdfd:
                                                case 0xfdfe:
                                                case 0xfe00:
                                                case 0xfe03:
                                                case 0xfe06:
                                                case 0xfe07:
                                                case 0xfe08:
                                                case 0xfe0c:
                                                case 0xfe0d:
                                                case 0xfe0e:
                                                case 0xfe10:
                                                case 0xfe11:
                                                case 0xfe32:
                                                case 0xfe33:
                                                case 0xfe34:
                                                case 0xfe35:
                                                case 0xfe36:
                                                case 0xfe37:
                                                case 0xfe80:
                                                case 0xfe82:
                                                case 0xfe83:
                                                    goto Label_3170;

                                                case 0xfdb4:
                                                    if (this.devTabsForm.GetTbReadStatusText() == "Reading...")
                                                    {
                                                        this.devTabsForm.SetTbReadStatusText(string.Format("{0:S}", this.devUtils.GetStatusStr(status)));
                                                    }
                                                    goto Label_3170;

                                                case 0xfe04:
                                                    if (status != 0)
                                                    {
                                                        string str10 = string.Format("GAP_DeviceDiscoveryRequest Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str10);
                                                        this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
                                                        this.devTabsForm.DiscoverConnectUserInputControl();
                                                    }
                                                    goto Label_3170;

                                                case 0xfe05:
                                                    if (status != 0)
                                                    {
                                                        string str11 = string.Format("GAP_DeviceDiscoveryCancel Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str11);
                                                    }
                                                    this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
                                                    this.devTabsForm.DiscoverConnectUserInputControl();
                                                    goto Label_3170;

                                                case 0xfe09:
                                                    this.StopTimer(EventType.Establish);
                                                    this.devTabsForm.ShowProgress(false);
                                                    if (status != 0)
                                                    {
                                                        string str12 = string.Format("GAP_EstablishLinkRequest Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str12);
                                                    }
                                                    this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
                                                    this.devTabsForm.DiscoverConnectUserInputControl();
                                                    goto Label_3170;

                                                case 0xfe0a:
                                                    if (status != 0)
                                                    {
                                                        string str13 = string.Format("GAP_TerminateLinkRequest Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str13);
                                                    }
                                                    this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
                                                    this.devTabsForm.DiscoverConnectUserInputControl();
                                                    goto Label_3170;

                                                case 0xfe0b:
                                                    if (status != 0)
                                                    {
                                                        this.StopTimer(EventType.PairBond);
                                                        this.devTabsForm.ShowProgress(false);
                                                        this.Cursor = Cursors.Default;
                                                        this.devTabsForm.TabPairBondInitValues();
                                                        this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.NotPaired);
                                                        this.devTabsForm.PairBondUserInputControl();
                                                        string str14 = string.Format("GAP Authenticate Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str14);
                                                    }
                                                    goto Label_3170;

                                                case 0xfe0f:
                                                    if ((this.devTabsForm.GetSelectedTab() == 2) && (status != 0))
                                                    {
                                                        this.StopTimer(EventType.PairBond);
                                                        this.devTabsForm.ShowProgress(false);
                                                        this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.NotPaired);
                                                        this.devTabsForm.PairBondUserInputControl();
                                                        string str15 = string.Format("GAP_Bond: Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str15);
                                                    }
                                                    goto Label_3170;

                                                case 0xfe30:
                                                    if (status != 0)
                                                    {
                                                        string str16 = string.Format("GAP_SetParam: Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str16);
                                                    }
                                                    this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
                                                    this.devTabsForm.DiscoverConnectUserInputControl();
                                                    goto Label_3170;

                                                case 0xfe31:
                                                    if (status != 0)
                                                    {
                                                        string str17 = string.Format("GAP_GetParam: Failed.\n{0}\n", hCIExtStatusStr);
                                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str17);
                                                    }
                                                    this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
                                                    this.devTabsForm.DiscoverConnectUserInputControl();
                                                    if (num26 != 0)
                                                    {
                                                        this.dataUtils.Unload16Bits(data, ref index, ref num8, ref dataErr, false);
                                                        if (!dataErr)
                                                        {
                                                            msg = msg + string.Format(" ParamValue\t: 0x{0:X4} ({1:D})\n", num8, num8);
                                                            switch (this.ConnParamState)
                                                            {
                                                                case GAPGetConnectionParams.MinConnIntSeq:
                                                                    this.devTabsForm.SetMinConnectionInterval(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.MaxConnIntSeq;
                                                                    break;

                                                                case GAPGetConnectionParams.MaxConnIntSeq:
                                                                    this.devTabsForm.SetMaxConnectionInterval(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.SlaveLatencySeq;
                                                                    break;

                                                                case GAPGetConnectionParams.SlaveLatencySeq:
                                                                    this.devTabsForm.SetSlaveLatency(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.SupervisionTimeoutSeq;
                                                                    break;

                                                                case GAPGetConnectionParams.SupervisionTimeoutSeq:
                                                                    this.devTabsForm.SetSupervisionTimeout(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.None;
                                                                    break;

                                                                case GAPGetConnectionParams.MinConnIntSingle:
                                                                    this.devTabsForm.SetNudMinConnIntValue(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.None;
                                                                    break;

                                                                case GAPGetConnectionParams.MaxConnIntSingle:
                                                                    this.devTabsForm.SetNudMaxConnIntValue(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.None;
                                                                    break;

                                                                case GAPGetConnectionParams.SlaveLatencySingle:
                                                                    this.devTabsForm.SetNudSlaveLatencyValue(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.None;
                                                                    break;

                                                                case GAPGetConnectionParams.SupervisionTimeoutSingle:
                                                                    this.devTabsForm.SetNudSprVisionTimeoutValue(num8);
                                                                    this.ConnParamState = GAPGetConnectionParams.None;
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    goto Label_3170;

                                                case 0xfe81:
                                                    if (num26 != 0)
                                                    {
                                                        msg = msg + string.Format(" nvData\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, num26, ref dataErr));
                                                        if (!dataErr)
                                                        {
                                                        }
                                                    }
                                                    goto Label_3170;
                                            }
                                        }
                                        else
                                        {
                                            switch (num30)
                                            {
                                                case 0xfc8a:
                                                case 0xfc92:
                                                case 0xfd01:
                                                case 0xfd02:
                                                case 0xfd03:
                                                case 0xfd04:
                                                case 0xfd05:
                                                case 0xfd06:
                                                case 0xfd07:
                                                case 0xfd08:
                                                case 0xfd09:
                                                case 0xfd0a:
                                                case 0xfd0b:
                                                case 0xfd0c:
                                                case 0xfd0d:
                                                case 0xfd0e:
                                                case 0xfd0f:
                                                case 0xfd10:
                                                case 0xfd11:
                                                case 0xfd12:
                                                case 0xfd13:
                                                case 0xfd16:
                                                case 0xfd17:
                                                case 0xfd18:
                                                case 0xfd19:
                                                case 0xfd1b:
                                                case 0xfd1d:
                                                case 0xfd1e:
                                                case 0xfd82:
                                                case 0xfd84:
                                                case 0xfd86:
                                                case 0xfd8c:
                                                case 0xfd90:
                                                case 0xfd96:
                                                    goto Label_3170;

                                                case 0xfd88:
                                                    if (this.devTabsForm.GetTbReadStatusText() == "Reading...")
                                                    {
                                                        this.devTabsForm.SetTbReadStatusText(string.Format("{0:S}", this.devUtils.GetStatusStr(status)));
                                                    }
                                                    goto Label_3170;

                                                case 0xfd8a:
                                                    if (this.devTabsForm.GetTbReadStatusText() == "Reading...")
                                                    {
                                                        this.devTabsForm.SetTbReadStatusText(string.Format("{0:S}", this.devUtils.GetStatusStr(status)));
                                                    }
                                                    goto Label_3170;

                                                case 0xfd8e:
                                                    if (this.devTabsForm.GetTbReadStatusText() == "Reading...")
                                                    {
                                                        this.devTabsForm.SetTbReadStatusText(string.Format("{0:S}", this.devUtils.GetStatusStr(status)));
                                                    }
                                                    goto Label_3170;

                                                case 0xfd92:
                                                    if (this.devTabsForm.GetTbWriteStatusText() == "Writing...")
                                                    {
                                                        this.devTabsForm.SetTbWriteStatusText(string.Format("{0:S}", this.devUtils.GetStatusStr(status)));
                                                    }
                                                    goto Label_3170;
                                            }
                                        }
                                        this.devUtils.BuildRawDataStr(data, ref msg, data.Length);
                                    }
                                }
                                goto Label_3170;
                            }
                        }
                        this.devUtils.BuildRawDataStr(data, ref msg, data.Length);
                    }
                    goto Label_3170;
                }
            }
            this.devUtils.BuildRawDataStr(data, ref msg, data.Length);
        Label_3170:
            if (dataErr)
            {
                this.DisplayMsg(SharedAppObjs.MsgType.Error, "Could Not Convert All The Data In The Following Message\n(Message Is Missing Data Bytes To Process)\n");
            }
            this.DisplayMsgTime(SharedAppObjs.MsgType.Incoming, msg, rxDataIn.time);
            if (displayBytes)
            {
                string str18 = string.Empty;
                str18 = string.Format("{0:X2} {1:X2} {2:X2} ", type, cmdOpcode & 0xff, length);
                switch (cmdOpcode)
                {
                    case 0x13:
                    case 0xff:
                        str18 = string.Format("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} ", new object[] { type, cmdOpcode & 0xff, length, eventOpcode & 0xff, (eventOpcode >> 8) & 0xff });
                        break;
                }
                byte lineIndex = 5;
                foreach (byte num29 in data)
                {
                    str18 = str18 + string.Format("{0:X2} ", num29);
                    lineIndex = (byte) (lineIndex + 1);
                    this.devUtils.CheckLineLength(ref str18, lineIndex, false);
                }
                this.DisplayMsg(SharedAppObjs.MsgType.RxDump, str18);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        ~DeviceForm()
        {
            this.DeviceFormClose(true);
        }

        public ConnectInfo GetConnectInfo()
        {
            return this.connectInfo;
        }

        private bool HandleRxTxMessage(RxTxMgrData rxTxMgrData)
        {
            bool flag = true;
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new HandleRxTxMessageDelegate(this.HandleRxTxMessage), new object[] { rxTxMgrData });
                }
                catch
                {
                }
                return flag;
            }
            if (this.formClosing)
            {
                return flag;
            }
            if (rxTxMgrData.rxDataIn != null)
            {
                this.DisplayRxCmd(rxTxMgrData.rxDataIn, this.msgLogForm.GetDisplayRxDumps());
                return flag;
            }
            if (rxTxMgrData.txDataOut == null)
            {
                return flag;
            }
            if (this.commMgr.comPort.IsOpen)
            {
                this.dspTxCmds.DisplayTxCmd(rxTxMgrData.txDataOut, this.msgLogForm.GetDisplayTxDumps());
                string str = "";
                foreach (byte num in rxTxMgrData.txDataOut.data)
                {
                    str = str + string.Format("{0:X2} ", num);
                }
                str = str.Trim();
                flag = this.commMgr.WriteData(str);
                if (!flag && (this.threadMgr.rxDataIn.DeviceTxStopWaitCallback != null))
                {
                    this.threadMgr.rxDataIn.DeviceTxStopWaitCallback(false);
                }
                return flag;
            }
            string msg = string.Format("Attempt To Send Empty Message Detected\nRequest Ignored\n", this.commSelectForm.cbPorts.SelectedItem);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DeviceForm));
            this.scTopLeftRight = new SplitContainer();
            this.plLog = new Panel();
            this.plUserTabs = new Panel();
            this.scanTimer = new System.Windows.Forms.Timer(this.components);
            this.initTimer = new System.Windows.Forms.Timer(this.components);
            this.establishTimer = new System.Windows.Forms.Timer(this.components);
            this.pairBondTimer = new System.Windows.Forms.Timer(this.components);
            this.scTopBottom = new SplitContainer();
            this.plAttributes = new Panel();
            this.scTopLeftRight.Panel1.SuspendLayout();
            this.scTopLeftRight.Panel2.SuspendLayout();
            this.scTopLeftRight.SuspendLayout();
            this.scTopBottom.Panel1.SuspendLayout();
            this.scTopBottom.Panel2.SuspendLayout();
            this.scTopBottom.SuspendLayout();
            base.SuspendLayout();
            this.scTopLeftRight.BackColor = SystemColors.Highlight;
            this.scTopLeftRight.Dock = DockStyle.Fill;
            this.scTopLeftRight.Location = new Point(0, 0);
            this.scTopLeftRight.Margin = new Padding(2, 3, 2, 3);
            this.scTopLeftRight.Name = "scTopLeftRight";
            this.scTopLeftRight.Panel1.Controls.Add(this.plLog);
            this.scTopLeftRight.Panel2.Controls.Add(this.plUserTabs);
            this.scTopLeftRight.Panel2.SizeChanged += new EventHandler(this.scTopLeftRightPanel2_SizeChanged);
            this.scTopLeftRight.Size = new Size(0x314, 0x203);
            this.scTopLeftRight.SplitterDistance = 380;
            this.scTopLeftRight.TabIndex = 11;
            this.plLog.BackColor = SystemColors.Control;
            this.plLog.Dock = DockStyle.Fill;
            this.plLog.Location = new Point(0, 0);
            this.plLog.Name = "plLog";
            this.plLog.Size = new Size(380, 0x203);
            this.plLog.TabIndex = 0;
            this.plUserTabs.AutoScroll = true;
            this.plUserTabs.BackColor = SystemColors.Control;
            this.plUserTabs.Dock = DockStyle.Fill;
            this.plUserTabs.Location = new Point(0, 0);
            this.plUserTabs.Name = "plUserTabs";
            this.plUserTabs.Size = new Size(0x194, 0x203);
            this.plUserTabs.TabIndex = 0;
            this.scTopBottom.BackColor = SystemColors.Highlight;
            this.scTopBottom.Dock = DockStyle.Fill;
            this.scTopBottom.Location = new Point(0, 0);
            this.scTopBottom.Name = "scTopBottom";
            this.scTopBottom.Orientation = Orientation.Horizontal;
            this.scTopBottom.Panel1.Controls.Add(this.scTopLeftRight);
            this.scTopBottom.Panel2.Controls.Add(this.plAttributes);
            this.scTopBottom.Size = new Size(0x314, 0x29b);
            this.scTopBottom.SplitterDistance = 0x203;
            this.scTopBottom.TabIndex = 1;
            this.plAttributes.BackColor = SystemColors.Control;
            this.plAttributes.Dock = DockStyle.Fill;
            this.plAttributes.Location = new Point(0, 0);
            this.plAttributes.Name = "plAttributes";
            this.plAttributes.Size = new Size(0x314, 0x94);
            this.plAttributes.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x314, 0x29b);
            base.Controls.Add(this.scTopBottom);
            this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Margin = new Padding(2, 3, 2, 3);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DeviceForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Device";
            base.Activated += new EventHandler(this.deviceForm_Activated);
            base.FormClosing += new FormClosingEventHandler(this.DeviceForm_FormClosing);
            base.Load += new EventHandler(this.DeviceForm_Load);
            base.LocationChanged += new EventHandler(this.DeviceForm_LocationChanged);
            this.scTopLeftRight.Panel1.ResumeLayout(false);
            this.scTopLeftRight.Panel2.ResumeLayout(false);
            this.scTopLeftRight.ResumeLayout(false);
            this.scTopBottom.Panel1.ResumeLayout(false);
            this.scTopBottom.Panel2.ResumeLayout(false);
            this.scTopBottom.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadUserInitializeValues()
        {
            this.scanTimer.Interval = this.EventTimeout[1];
            this.scanTimer.Tick += new EventHandler(this.timerScanEvent);
            this.initTimer.Interval = this.EventTimeout[0];
            this.initTimer.Tick += new EventHandler(this.timerInitEvent);
            this.establishTimer.Interval = this.EventTimeout[2];
            this.establishTimer.Tick += new EventHandler(this.timerEstablishEvent);
            this.pairBondTimer.Interval = this.EventTimeout[3];
            this.pairBondTimer.Tick += new EventHandler(this.timerPairBondEvent);
            this.devTabsForm.TabAdvCommandsInitValues();
            this.devTabsForm.TabDiscoverConnectInitValues();
            this.devTabsForm.TabPairBondInitValues();
            this.devTabsForm.TabReadWriteInitValues();
        }

        private void LoadUserSettings()
        {
            this.attributesForm.LoadUserSettings();
        }

        protected void OnBDAddressNotify(string deviceBDAddressStr)
        {
            this.BDAddressStr = deviceBDAddressStr;
            this.BDAddressNotify(this, EventArgs.Empty);
        }

        protected void OnConnectionNotify(ref ConnectInfo tmpConnectInfo)
        {
            this.numConnections++;
            this.connectInfo = tmpConnectInfo;
            this.Connections.Add(this.connectInfo);
            this.ConnectionNotify(this, EventArgs.Empty);
            string msg = "Device Connected\nHandle = 0x" + this.connectInfo.handle.ToString("X4") + "\nAddr Type = 0x" + this.connectInfo.addrType.ToString("X2") + " (" + this.devUtils.GetGapAddrTypeStr(this.connectInfo.addrType) + ")\nBDAddr = " + this.connectInfo.bDA + "\n";
            this.DisplayMsg(SharedAppObjs.MsgType.Info, msg);
            this.attributesForm.RemoveData(this.connectInfo.handle);
        }

        protected void OnDisconnectionNotify(ref ConnectInfo tmpDisconnectInfo)
        {
            this.disconnectInfo = tmpDisconnectInfo;
            for (int i = 0; i < this.Connections.Count; i++)
            {
                if (this.Connections[i].handle == this.disconnectInfo.handle)
                {
                    string msg = "Device Disconnected\nHandle = 0x" + this.disconnectInfo.handle.ToString("X4") + "\nAddr Type = 0x" + this.Connections[i].addrType.ToString("X2") + " (" + this.devUtils.GetGapAddrTypeStr(this.Connections[i].addrType) + ")\nBDAddr = " + this.Connections[i].bDA + "\n";
                    this.DisplayMsg(SharedAppObjs.MsgType.Info, msg);
                    this.Connections.RemoveAt(i);
                    this.DisconnectionNotify(this, EventArgs.Empty);
                    if (this.numConnections > 0)
                    {
                        this.numConnections--;
                    }
                    this.attributesForm.RemoveData(this.disconnectInfo.handle);
                    return;
                }
            }
        }

        private void ProcessRxProc()
        {
            byte type = 0;
            ushort opCode = 0xffff;
            ushort eventOpCode = 0xffff;
            byte length = 0;
            byte[] data = null;
            SharedObjects.log.Write(Logging.MsgType.Debug, "ProcessRxProc", "Starting Thread");
            while (!this.formClosing)
            {
                if (this.commParser.GetDataSize() != 0)
                {
                    if (this.commParser.ParseData(ref type, ref opCode, ref eventOpCode, ref length, ref data))
                    {
                        if (this.formClosing)
                        {
                            break;
                        }
                        RxDataIn @in = new RxDataIn();
                        @in.type = type;
                        @in.cmdOpcode = opCode;
                        @in.eventOpcode = eventOpCode;
                        @in.length = length;
                        @in.data = data;
                        this.threadMgr.rxDataIn.dataQ.AddQTail(@in);
                        type = 0;
                        opCode = 0xffff;
                        eventOpCode = 0xffff;
                        length = 0;
                        data = null;
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            SharedObjects.log.Write(Logging.MsgType.Debug, "ProcessRxProc", "Exiting Thread");
        }

        internal void RxDataHandler(byte[] data, uint length)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new RxDataHandlerDelegate(this.RxDataHandler), new object[] { data, length });
            }
            else
            {
                this.commParser.EnQueueData(data);
            }
        }

        private void SaveUserSettings()
        {
            this.attributesForm.SaveUserSettings();
            Settings.Default.Save();
        }

        private void scTopLeftRightPanel2_SizeChanged(object sender, EventArgs e)
        {
            int width = this.scTopLeftRight.Panel2.Width;
            int num2 = 0x1ab;
            if (this.devTabsForm != null)
            {
                num2 = this.devTabsForm.GetTcDeviceTabsWidth() + 15;
            }
            int num3 = (base.Width - num2) - 10;
            if ((width > num2) && (num3 > 1))
            {
                this.scTopLeftRight.SplitterDistance = num3;
            }
            this.scTopLeftRight.Update();
            if (this.devTabsForm != null)
            {
                this.devTabsForm.DeviceTabsUpdate();
            }
        }

        public void SendAllEvents(byte[] data, byte length)
        {
            int index = 0;
            bool dataErr = false;
            RxDataIn rxDataIn = new RxDataIn();
            rxDataIn.type = 4;
            rxDataIn.cmdOpcode = 0xff;
            rxDataIn.length = length;
            rxDataIn.data = data;
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x400;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x401;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x402;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x403;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x404;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x405;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x406;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x407;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x408;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x409;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x40a;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x40b;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x40c;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x40d;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x40e;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x40f;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x410;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x411;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x412;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x413;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x414;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x481;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x48b;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x493;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x501;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x502;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x503;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x504;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x505;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x506;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x507;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x508;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x509;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50a;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50b;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50c;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50d;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50e;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50f;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x50f;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x510;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x511;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x512;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x513;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x516;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x517;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x518;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x519;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x51b;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x51d;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x51e;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x600;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x601;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x602;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x603;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x604;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x605;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x606;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x607;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x608;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x609;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x60a;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x60b;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x60c;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x60d;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x60e;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x60f;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.eventOpcode = 0x67f;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            index = 0;
            rxDataIn.cmdOpcode = 14;
            rxDataIn.eventOpcode = 0;
            this.dataUtils.Load8Bits(ref data, ref index, 1, ref dataErr);
            this.dataUtils.Load16Bits(ref data, ref index, 0x1405, ref dataErr, false);
            rxDataIn.data = data;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            index = 0;
            this.dataUtils.Load8Bits(ref data, ref index, 1, ref dataErr);
            this.dataUtils.Load16Bits(ref data, ref index, 0x2010, ref dataErr, false);
            rxDataIn.data = data;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            index = 0;
            this.dataUtils.Load8Bits(ref data, ref index, 1, ref dataErr);
            this.dataUtils.Load16Bits(ref data, ref index, 0x2011, ref dataErr, false);
            rxDataIn.data = data;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            index = 0;
            this.dataUtils.Load8Bits(ref data, ref index, 1, ref dataErr);
            this.dataUtils.Load16Bits(ref data, ref index, 0x2012, ref dataErr, false);
            rxDataIn.data = data;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            index = 0;
            this.dataUtils.Load8Bits(ref data, ref index, 1, ref dataErr);
            this.dataUtils.Load16Bits(ref data, ref index, 0x2013, ref dataErr, false);
            rxDataIn.data = data;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            rxDataIn.cmdOpcode = 0x13;
            this.DisplayRxCmd(rxDataIn, true);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
        }

        public void SendAllForever()
        {
            int num = 0;
            string logMsg = string.Empty;
            while (true)
            {
                logMsg = string.Format("Msg Loop # {0:D}", num++);
                this.msgLogForm.AppendLog(logMsg);
                this.SendAllMsgs();
                this.SendEventWaves(true);
                Thread.Sleep(0x3e8);
            }
        }

        public void SendAllMsgs()
        {
            this.sendCmds.SendHCIExt(this.HCIExt_SetRxGain);
            this.sendCmds.SendHCIExt(this.HCIExt_SetTxPower);
            this.sendCmds.SendHCIExt(this.HCIExt_OnePktPerEvt);
            this.sendCmds.SendHCIExt(this.HCIExt_ClkDivideOnHalt);
            this.sendCmds.SendHCIExt(this.HCIExt_DeclareNvUsage);
            this.sendCmds.SendHCIExt(this.HCIExt_Decrypt);
            this.sendCmds.SendHCIExt(this.HCIExt_SetLocalSupportedFeatures);
            this.sendCmds.SendHCIExt(this.HCIExt_SetFastTxRespTime);
            this.sendCmds.SendHCIExt(this.HCIExt_ModemTestTx);
            this.sendCmds.SendHCIExt(this.HCIExt_ModemHopTestTx);
            this.sendCmds.SendHCIExt(this.HCIExt_ModemTestRx);
            this.sendCmds.SendHCIExt(this.HCIExt_EndModemTest);
            this.sendCmds.SendHCIExt(this.HCIExt_SetBDADDR);
            this.sendCmds.SendHCIExt(this.HCIExt_SetSCA);
            this.sendCmds.SendHCIExt(this.HCIExt_EnablePTM);
            this.sendCmds.SendHCIExt(this.HCIExt_SetFreqTune);
            this.sendCmds.SendHCIExt(this.HCIExt_SaveFreqTune);
            this.sendCmds.SendHCIExt(this.HCIExt_SetMaxDtmTxPower);
            this.sendCmds.SendHCIExt(this.HCIExt_MapPmIoPort);
            this.sendCmds.SendHCIExt(this.HCIExt_DisconnectImmed);
            this.sendCmds.SendHCIExt(this.HCIExt_PER);
            this.sendCmds.SendL2CAP(this.L2CAP_InfoReq);
            this.sendCmds.SendL2CAP(this.L2CAP_ConnParamUpdateReq);
            this.sendCmds.SendATT(this.ATT_ErrorRsp);
            this.sendCmds.SendATT(this.ATT_ExchangeMTUReq);
            this.sendCmds.SendATT(this.ATT_ExchangeMTURsp);
            this.sendCmds.SendATT(this.ATT_FindInfoReq, TxDataOut.CmdType.General);
            this.sendCmds.SendATT(this.ATT_FindInfoRsp);
            this.sendCmds.SendATT(this.ATT_FindByTypeValueReq);
            this.sendCmds.SendATT(this.ATT_FindByTypeValueRsp);
            this.sendCmds.SendATT(this.ATT_ReadByTypeReq);
            this.sendCmds.SendATT(this.ATT_ReadByTypeRsp);
            this.sendCmds.SendATT(this.ATT_ReadReq, TxDataOut.CmdType.General, null);
            this.sendCmds.SendATT(this.ATT_ReadRsp);
            this.sendCmds.SendATT(this.ATT_ReadBlobReq, TxDataOut.CmdType.General, null);
            this.sendCmds.SendATT(this.ATT_ReadBlobRsp);
            this.sendCmds.SendATT(this.ATT_ReadMultiReq);
            this.sendCmds.SendATT(this.ATT_ReadMultiRsp);
            this.sendCmds.SendATT(this.ATT_ReadByGrpTypeReq, TxDataOut.CmdType.General);
            this.sendCmds.SendATT(this.ATT_ReadByGrpTypeRsp);
            this.sendCmds.SendATT(this.ATT_WriteReq, null);
            this.sendCmds.SendATT(this.ATT_WriteRsp);
            this.sendCmds.SendATT(this.ATT_PrepareWriteReq);
            this.sendCmds.SendATT(this.ATT_PrepareWriteRsp);
            this.sendCmds.SendATT(this.ATT_ExecuteWriteReq, null);
            this.sendCmds.SendATT(this.ATT_ExecuteWriteRsp);
            this.sendCmds.SendATT(this.ATT_HandleValueNotification);
            this.sendCmds.SendATT(this.ATT_HandleValueIndication);
            this.sendCmds.SendATT(this.ATT_HandleValueConfirmation);
            this.sendCmds.SendGATT(this.GATT_ExchangeMTU);
            this.sendCmds.SendGATT(this.GATT_DiscAllPrimaryServices, TxDataOut.CmdType.General);
            this.sendCmds.SendGATT(this.GATT_DiscPrimaryServiceByUUID);
            this.sendCmds.SendGATT(this.GATT_FindIncludedServices);
            this.sendCmds.SendGATT(this.GATT_DiscAllChars);
            this.sendCmds.SendGATT(this.GATT_DiscCharsByUUID);
            this.sendCmds.SendGATT(this.GATT_DiscAllCharDescs, TxDataOut.CmdType.General);
            this.sendCmds.SendGATT(this.GATT_ReadCharValue, TxDataOut.CmdType.General, null);
            this.sendCmds.SendGATT(this.GATT_ReadUsingCharUUID);
            this.sendCmds.SendGATT(this.GATT_ReadLongCharValue, TxDataOut.CmdType.General, null);
            this.sendCmds.SendGATT(this.GATT_ReadMultiCharValues);
            this.sendCmds.SendGATT(this.GATT_WriteNoRsp);
            this.sendCmds.SendGATT(this.GATT_SignedWriteNoRsp);
            this.sendCmds.SendGATT(this.GATT_WriteCharValue, null);
            this.sendCmds.SendGATT(this.GATT_WriteLongCharValue, null, null);
            this.sendCmds.SendGATT(this.GATT_ReliableWrites);
            this.sendCmds.SendGATT(this.GATT_ReadCharDesc);
            this.sendCmds.SendGATT(this.GATT_ReadLongCharDesc);
            this.sendCmds.SendGATT(this.GATT_WriteCharDesc);
            this.sendCmds.SendGATT(this.GATT_WriteLongCharDesc);
            this.sendCmds.SendGATT(this.GATT_Notification);
            this.sendCmds.SendGATT(this.GATT_Indication);
            this.sendCmds.SendGATT(this.GATT_AddService);
            this.sendCmds.SendGATT(this.GATT_DelService);
            this.sendCmds.SendGATT(this.GATT_AddAttribute);
            this.sendCmds.SendGAP(this.GAP_DeviceInit);
            this.sendCmds.SendGAP(this.GAP_ConfigDeviceAddr);
            this.sendCmds.SendGAP(this.GAP_DeviceDiscoveryRequest);
            this.sendCmds.SendGAP(this.GAP_DeviceDiscoveryCancel);
            this.sendCmds.SendGAP(this.GAP_MakeDiscoverable);
            this.sendCmds.SendGAP(this.GAP_UpdateAdvertisingData);
            this.sendCmds.SendGAP(this.GAP_EndDiscoverable);
            this.sendCmds.SendGAP(this.GAP_EstablishLinkRequest);
            this.sendCmds.SendGAP(this.GAP_TerminateLinkRequest);
            this.sendCmds.SendGAP(this.GAP_Authenticate);
            this.sendCmds.SendGAP(this.GAP_PasskeyUpdate);
            this.sendCmds.SendGAP(this.GAP_SlaveSecurityRequest);
            this.sendCmds.SendGAP(this.GAP_Signable);
            this.sendCmds.SendGAP(this.GAP_Bond);
            this.sendCmds.SendGAP(this.GAP_TerminateAuth);
            this.sendCmds.SendGAP(this.GAP_UpdateLinkParamReq);
            this.sendCmds.SendGAP(this.GAP_SetParam);
            this.sendCmds.SendGAP(this.GAP_GetParam);
            this.sendCmds.SendGAP(this.GAP_ResolvePrivateAddr);
            this.sendCmds.SendGAP(this.GAP_SetAdvToken);
            this.sendCmds.SendGAP(this.GAP_RemoveAdvToken);
            this.sendCmds.SendGAP(this.GAP_UpdateAdvTokens);
            this.sendCmds.SendGAP(this.GAP_BondSetParam);
            this.sendCmds.SendGAP(this.GAP_BondGetParam);
            this.sendCmds.SendUTIL(this.UTIL_NVRead);
            this.sendCmds.SendUTIL(this.UTIL_NVWrite);
            this.sendCmds.SendHCIOther(this.HCIOther_ReadRSSI);
            this.sendCmds.SendHCIOther(this.HCIOther_LEClearWhiteList);
            this.sendCmds.SendHCIOther(this.HCIOther_LEAddDeviceToWhiteList);
            this.sendCmds.SendHCIOther(this.HCIOther_LERemoveDeviceFromWhiteList);
            this.sendCmds.SendHCIOther(this.HCIOther_LEConnectionUpdate);
        }

        public void SendAttrDataCmds()
        {
            this.sendCmds.SendGATT(this.GATT_DiscAllPrimaryServices, TxDataOut.CmdType.General);
            this.sendCmds.SendGATT(this.GATT_DiscPrimaryServiceByUUID);
            this.sendCmds.SendGATT(this.GATT_FindIncludedServices);
            this.sendCmds.SendGATT(this.GATT_DiscAllChars);
            this.sendCmds.SendGATT(this.GATT_DiscCharsByUUID);
            this.sendCmds.SendGATT(this.GATT_DiscAllCharDescs, TxDataOut.CmdType.General);
        }

        public void SendEventWaves(bool skipCase)
        {
            byte length = 0xff;
            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = (byte) i;
            }
            length = (byte) (length - 4);
            this.SendAllEvents(data, length);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            if (!skipCase)
            {
                int num3 = length - 1;
                int index = 0;
                while (index < length)
                {
                    data[index] = (byte) num3;
                    index++;
                    num3--;
                }
                length = (byte) (length - 4);
                this.SendAllEvents(data, length);
                this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            }
            for (int j = 0; j < length; j++)
            {
                data[j] = 0;
            }
            length = (byte) (length - 4);
            this.SendAllEvents(data, length);
            this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            if (!skipCase)
            {
                for (int k = 0; k < length; k++)
                {
                    data[k] = 0xff;
                }
                length = (byte) (length - 4);
                this.SendAllEvents(data, length);
                this.msgLogForm.AppendLog("------------------------------------------------------------------------------------------------------------------------\n");
            }
        }

        public void SendGAPDeviceInit()
        {
            this.devTabsForm.ShowProgress(true);
            this.devTabsForm.UserTabAccess(false);
            this.StartTimer(EventType.Init);
            this.sendCmds.SendGAP(this.GAP_DeviceInit);
        }

        public void StartTimer(EventType eType)
        {
            switch (eType)
            {
                case EventType.Init:
                    this.initTimer.Start();
                    return;

                case EventType.Scan:
                    this.scanTimer.Start();
                    return;

                case EventType.Establish:
                    this.establishTimer.Start();
                    return;

                case EventType.PairBond:
                    this.pairBondTimer.Start();
                    return;
            }
        }

        public void StopTimer(EventType eType)
        {
            switch (eType)
            {
                case EventType.Init:
                    this.initTimer.Stop();
                    return;

                case EventType.Scan:
                    this.scanTimer.Stop();
                    return;

                case EventType.Establish:
                    this.establishTimer.Stop();
                    return;

                case EventType.PairBond:
                    this.pairBondTimer.Stop();
                    return;
            }
        }

        public void TestCase()
        {
            BTool.HCICmds.UTILCmds.UTIL_Reset reset = new BTool.HCICmds.UTILCmds.UTIL_Reset();
            reset.resetType = HCICmds.UTIL_ResetType.Hard_Reset;
            this.sendCmds.SendUTIL(reset);
            BTool.HCICmds.HCIExtCmds.HCIExt_SetBDADDR tbdaddr = new BTool.HCICmds.HCIExtCmds.HCIExt_SetBDADDR();
            tbdaddr.bleDevAddr = "70:55:44:33:22:11";
            this.sendCmds.SendHCIExt(tbdaddr);
            BTool.HCICmds.GAPCmds.GAP_DeviceInit init = new BTool.HCICmds.GAPCmds.GAP_DeviceInit();
            init.profileRole = HCICmds.GAP_Profile.Central;
            init.maxScanResponses = 3;
            init.irk = "33:42:CF:14:BC:55:17:31:75:4F:BB:A4:C7:F2:8C:13";
            init.csrk = "45:0A:F4:B0:03:07:B0:40:87:F4:18:23:75:4A:FB:A4";
            init.signCounter = 0;
            this.sendCmds.SendGAP(init);
            BTool.HCICmds.GAPCmds.GAP_EstablishLinkRequest request = new BTool.HCICmds.GAPCmds.GAP_EstablishLinkRequest();
            request.highDutyCycle = HCICmds.GAP_EnableDisable.Disable;
            request.whiteList = HCICmds.GAP_EnableDisable.Disable;
            request.addrTypePeer = HCICmds.GAP_AddrType.Public;
            request.peerAddr = "60:55:44:33:22:11";
            this.sendCmds.SendGAP(request);
            BTool.HCICmds.GAPCmds.GAP_Authenticate authenticate = new BTool.HCICmds.GAPCmds.GAP_Authenticate();
            authenticate.connHandle = 0;
            authenticate.secReq_ioCaps = HCICmds.GAP_IOCaps.KeyboardDisplay;
            authenticate.secReq_oobAvailable = HCICmds.GAP_TrueFalse.False;
            authenticate.secReq_oob = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
            authenticate.secReq_authReq = 1;
            authenticate.secReq_maxEncKeySize = 0x10;
            authenticate.secReq_keyDist = 0;
            authenticate.pairReq_Enable = HCICmds.GAP_EnableDisable.Disable;
            authenticate.pairReq_ioCaps = HCICmds.GAP_IOCaps.KeyboardDisplay;
            authenticate.pairReq_oobDataFlag = HCICmds.GAP_EnableDisable.Disable;
            authenticate.pairReq_authReq = 1;
            authenticate.secReq_maxEncKeySize = 0x10;
            authenticate.secReq_keyDist = 0;
            this.sendCmds.SendGAP(authenticate);
            BTool.HCICmds.GAPCmds.GAP_TerminateLinkRequest request2 = new BTool.HCICmds.GAPCmds.GAP_TerminateLinkRequest();
            request2.connHandle = 0;
            this.sendCmds.SendGAP(request2);
        }

        private void timerEstablishEvent(object obj, EventArgs args)
        {
            this.StopTimer(EventType.Establish);
            this.devTabsForm.ShowProgress(false);
            this.Cursor = Cursors.Default;
            string msg = "GAP Link Establish Request Timeout.\n";
            this.DisplayMsg(SharedAppObjs.MsgType.Warning, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private void timerInitEvent(object obj, EventArgs args)
        {
            this.StopTimer(EventType.Init);
            this.devTabsForm.ShowProgress(false);
            this.Cursor = Cursors.Default;
            string msg = "GAP Device Initialization Timeout.\nDevice May Not Function Properly.\n";
            this.DisplayMsg(SharedAppObjs.MsgType.Warning, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private void timerPairBondEvent(object obj, EventArgs args)
        {
            this.StopTimer(EventType.PairBond);
            this.devTabsForm.ShowProgress(false);
            this.Cursor = Cursors.Default;
            this.devTabsForm.TabPairBondInitValues();
            this.devTabsForm.SetPairingStatus(DeviceTabsForm.PairingStatus.NotPaired);
            this.devTabsForm.PairBondUserInputControl();
            string msg = "Pairing Bonding Request Timeout.\n";
            this.DisplayMsg(SharedAppObjs.MsgType.Warning, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private void timerScanEvent(object obj, EventArgs args)
        {
            this.StopTimer(EventType.Scan);
            this.devTabsForm.ShowProgress(false);
            this.Cursor = Cursors.Default;
            this.devTabsForm.discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
            this.devTabsForm.DiscoverConnectUserInputControl();
            string msg = "Device Scan Timeout.\n";
            this.DisplayMsg(SharedAppObjs.MsgType.Warning, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }

        private delegate void DisplayRxCmdDelegate(RxDataIn rxDataIn, bool displayBytes);

        public enum EventType
        {
            Init,
            Scan,
            Establish,
            PairBond
        }

        public enum GAPGetConnectionParams
        {
            None,
            MinConnIntSeq,
            MaxConnIntSeq,
            SlaveLatencySeq,
            SupervisionTimeoutSeq,
            MinConnIntSingle,
            MaxConnIntSingle,
            SlaveLatencySingle,
            SupervisionTimeoutSingle
        }

        private delegate void RxDataHandlerDelegate(byte[] data, uint length);
    }
}

