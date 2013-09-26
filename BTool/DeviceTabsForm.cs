﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class DeviceTabsForm : Form
    {
        private Button btnEncryptLink;
        private Button btnEstablish;
        private Button btnEstablishCancel;
        private Button btnGetConnectionParams;
        private Button btnLoadLongTermKey;
        private Button btnReadGATTValue;
        private Button btnSaveLongTermKey;
        private Button btnScan;
        private Button btnScanCancel;
        private Button btnSendPairingRequest;
        private Button btnSendPasskey;
        private Button btnSendShared;
        private Button btnSetConnectionParams;
        private Button btnTerminate;
        private Button btnWriteGATTValue;
        private ComboBox cbConnAddrType;
        private ComboBox cbConnSlaveDeviceBDAddress;
        private ComboBox cbReadType;
        private ComboBox cbScanMode;
        private CheckBox ckBoxActiveScan;
        private CheckBox ckBoxAuthMitmEnabled;
        private CheckBox ckBoxBondingEnabled;
        private CheckBox ckBoxConnWhiteList;
        private CheckBox ckBoxWhiteList;
        private ContextMenuStrip cmsAdvTab;
        private IContainer components;
        private List<CsvData> csvKeyData = new List<CsvData>();
        private int CsvNumberOfLineElements = 5;
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public DiscoverConnectStatus discoverConnectStatus;
        public DisplayMsgDelegate DisplayMsgCallback;
        private GroupBox gbCharRead;
        private GroupBox gbCharWrite;
        private GroupBox gbConnSettings;
        private GroupBox gbDiscovery;
        private GroupBox gbEncryptLTKey;
        private GroupBox gbEstablishLink;
        private GroupBox gbInitParing;
        private GroupBox gbLinkControl;
        private GroupBox gbLongTermKeyData;
        private GroupBox gbPasskeyInput;
        private GroupBox gbReadArea;
        private GroupBox gbTerminateLink;
        private GroupBox gbWriteArea;
        private Label labelPairingStatus;
        private string lastAuthStr = string.Empty;
        private HCICmds.GAPEvts.GAP_AuthenticationComplete lastGAP_AuthenticationComplete = new HCICmds.GAPEvts.GAP_AuthenticationComplete();
        private Label lblAddrType;
        private Label lblAuthBond;
        private Label lblConnHandle;
        private Label lblConnHnd;
        private Label lblDeviceFound;
        private Label lblDevsFound;
        private Label lblEstablishLink;
        private Label lblLtk;
        private Label lblLtkConnHnd;
        private Label lblLtkDiv;
        private Label lblLtkRandom;
        private Label lblMaxConn;
        private Label lblMaxConnInt;
        private Label lblMinConn;
        private Label lblMinConnInt;
        private Label lblMode;
        private Label lblPairConnHnd;
        private Label lblPasskey;
        private Label lblPassRange;
        private Label lblReadCharUuid;
        private Label lblReadConnHnd;
        private Label lblReadEndHnd;
        private Label lblReadStartHnd;
        private Label lblReadStatus;
        private Label lblReadSubProc;
        private Label lblReadValueHnd;
        private Label lblSlaveBDA;
        private Label lblSlaveLat;
        private Label lblSuperTimeout;
        private Label lblSupervisionTimeout;
        private Label lblTerminateLink;
        private Label lblWriteConnHnd;
        private Label lblWriteHandle;
        private Label lblWriteStatus;
        private Label lblWriteValue;
        private Label lbReadValue;
        private List<LinkSlave> linkSlaves = new List<LinkSlave>();
        private ListSelectForm listSelectForm;
        public static string moduleName = "DeviceTabsForm";
        private MsgBox msgBox = new MsgBox();
        private NumericUpDown nudMaxConnInt;
        private NumericUpDown nudMinConnInt;
        private NumericUpDown nudSlaveLatency;
        private NumericUpDown nudSprVisionTimeout;
        private PairingStatus pairingStatus;
        private ProgressBar pbSharedDevice;
        private PropertyGrid pgAdvCmds;
        private RadioButton rbASCIIRead;
        private RadioButton rbASCIIWrite;
        private RadioButton rbAuthBondFalse;
        private RadioButton rbAuthBondTrue;
        private RadioButton rbDecimalRead;
        private RadioButton rbDecimalWrite;
        private RadioButton rbHexRead;
        private RadioButton rbHexWrite;
        private SplitContainer scTreeGrid;
        private SharedObjects sharedObjs = new SharedObjects();
        private ushort SlaveDeviceFound;
        private TextBox tbBondConnHandle;
        private TextBox tbLongTermKey;
        private TextBox tbLongTermKeyData;
        private TextBox tbLTKDiversifier;
        private TextBox tbLTKRandom;
        private TextBox tbPairingConnHandle;
        private TextBox tbPairingStatus;
        private TextBox tbPasskey;
        private TextBox tbPasskeyConnHandle;
        private TextBox tbReadAttrHandle;
        private TextBox tbReadConnHandle;
        private TextBox tbReadEndHandle;
        private TextBox tbReadStartHandle;
        private TextBox tbReadStatus;
        private TextBox tbReadUUID;
        private TextBox tbReadValue;
        private TextBox tbTermConnHandle;
        private TextBox tbWriteAttrHandle;
        private TextBox tbWriteConnHandle;
        private TextBox tbWriteStatus;
        private TextBox tbWriteValue;
        private TabControl tcDeviceTabs;
        private TabPage tpAdvCommands;
        private TabPage tpDiscoverConnect;
        private TabPage tpPairingBonding;
        private TabPage tpReadWrite;
        private ToolStripMenuItem tsmiSendAdvCmd;
        private TreeView tvAdvCmdList;

        public DeviceTabsForm(DeviceForm deviceForm)
        {
            this.InitializeComponent();
            this.devForm = deviceForm;
            this.listSelectForm = new ListSelectForm();
            this.btnSendShared.Visible = false;
            this.btnSendShared.Enabled = false;
            this.devForm.threadMgr.txDataOut.ShowProgressCallback = new ShowProgressDelegate(this.ShowProgress);
        }

        public void AddSlaveDevice(LinkSlave linkSlave)
        {
            bool dataErr = false;
            byte[] addr = new byte[6];
            string s = "";
            int index = 0;
            s = this.devUtils.UnloadDeviceAddr(linkSlave.slaveBDA, ref addr, ref index, false, ref dataErr);
            linkSlave.addrBDA = s;
            if (this.cbConnSlaveDeviceBDAddress.FindString(s) == -1)
            {
                this.SlaveDeviceFound = (ushort) (this.SlaveDeviceFound + 1);
                this.cbConnSlaveDeviceBDAddress.Items.Add(s);
                this.SetAddrType((byte) linkSlave.addrType);
                this.linkSlaves.Add(linkSlave);
            }
            if (this.cbConnSlaveDeviceBDAddress.Items.Count > 1)
            {
                this.cbConnSlaveDeviceBDAddress.SelectedIndex = 1;
                this.SetAddrType((byte) this.linkSlaves[this.cbConnSlaveDeviceBDAddress.SelectedIndex].addrType);
            }
            this.lblDeviceFound.Text = this.SlaveDeviceFound.ToString();
        }

        private bool AddToEndCsv(CsvData newCsvData, ref List<CsvData> csvData)
        {
            bool flag = false;
            try
            {
                if (newCsvData == null)
                {
                    throw new ArgumentException(string.Format("There Is No Data To Add.\n", new object[0]));
                }
                CsvData item = new CsvData();
                item = new CsvData();
                item.addr = newCsvData.addr;
                item.auth = newCsvData.auth;
                item.ltk = newCsvData.ltk;
                item.div = newCsvData.div;
                item.rand = newCsvData.rand;
                csvData.Add(item);
            }
            catch (Exception exception)
            {
                string msg = string.Format("Cannot Add Data To End Of The CSV List.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = true;
            }
            return flag;
        }

        private void btnEstablish_Click(object sender, EventArgs e)
        {
            HCICmds.GAPCmds.GAP_EstablishLinkRequest request = new HCICmds.GAPCmds.GAP_EstablishLinkRequest();
            request.highDutyCycle = HCICmds.GAP_EnableDisable.Disable;
            if (this.ckBoxConnWhiteList.Checked)
            {
                request.whiteList = HCICmds.GAP_EnableDisable.Enable;
            }
            else
            {
                request.whiteList = HCICmds.GAP_EnableDisable.Disable;
            }
            request.addrTypePeer = (HCICmds.GAP_AddrType) this.cbConnAddrType.SelectedIndex;
            if (this.cbConnSlaveDeviceBDAddress.Text == "None")
            {
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Select a Slave BDAddress\n");
                this.cbConnSlaveDeviceBDAddress.Focus();
            }
            else
            {
                this.discoverConnectStatus = DiscoverConnectStatus.Establish;
                this.DiscoverConnectUserInputControl();
                this.ShowProgress(true);
                this.devForm.StartTimer(DeviceForm.EventType.Establish);
                request.peerAddr = this.cbConnSlaveDeviceBDAddress.Text;
                if (!this.devForm.sendCmds.SendGAP(request))
                {
                    this.ShowProgress(false);
                    this.devForm.StopTimer(DeviceForm.EventType.Establish);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Slave BDA\n");
                    this.cbConnSlaveDeviceBDAddress.Focus();
                }
            }
        }

        private void btnEstablishCancel_Click(object sender, EventArgs e)
        {
            this.discoverConnectStatus = DiscoverConnectStatus.EstablishCancel;
            this.DiscoverConnectUserInputControl();
            HCICmds.GAPCmds.GAP_TerminateLinkRequest request = new HCICmds.GAPCmds.GAP_TerminateLinkRequest();
            try
            {
                request.connHandle = 0xfffe;
                this.devForm.sendCmds.SendGAP(request);
            }
            catch (Exception exception)
            {
                string msg = string.Format("Failed To Send Terminate Link Message.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        }

        private void btnGATTReadValue_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (this.cbReadType.SelectedIndex == 0)
            {
                HCICmds.GATTCmds.GATT_ReadCharValue value2 = new HCICmds.GATTCmds.GATT_ReadCharValue();
                this.tbReadValue.Tag = string.Empty;
                this.tbReadValue.Text = "";
                this.tbReadStatus.Text = "Reading...";
                try
                {
                    value2.connHandle = Convert.ToUInt16(this.tbReadConnHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception)
                {
                    string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", exception.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    this.tbReadConnHandle.Focus();
                    flag = true;
                }
                try
                {
                    value2.handle = Convert.ToUInt16(this.tbReadAttrHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception2)
                {
                    string str2 = string.Format("Invalid Characteristic Value Handle(s)\nFormat: 0x0000\n\n{0}\n", exception2.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                    this.tbReadAttrHandle.Focus();
                    flag = true;
                }
                if (!flag)
                {
                    this.devForm.sendCmds.SendGATT(value2, TxDataOut.CmdType.General, null);
                }
                else
                {
                    this.tbReadStatus.Text = "Error!!!";
                }
            }
            else if (this.cbReadType.SelectedIndex == 1)
            {
                HCICmds.GATTCmds.GATT_ReadUsingCharUUID ruuid = new HCICmds.GATTCmds.GATT_ReadUsingCharUUID();
                this.tbReadValue.Tag = string.Empty;
                this.tbReadValue.Text = "";
                this.tbReadStatus.Text = "Reading...";
                try
                {
                    ruuid.connHandle = Convert.ToUInt16(this.tbReadConnHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception3)
                {
                    string str3 = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", exception3.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                    this.tbReadConnHandle.Focus();
                    flag = true;
                }
                try
                {
                    ruuid.startHandle = Convert.ToUInt16(this.tbReadStartHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception4)
                {
                    string str4 = string.Format("Invalid Start Handle\nFormat: 0x0000\n\n{0}\n", exception4.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                    this.tbReadStartHandle.Focus();
                    flag = true;
                }
                try
                {
                    ruuid.endHandle = Convert.ToUInt16(this.tbReadEndHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception5)
                {
                    string str5 = string.Format("Invalid End Handle\nFormat: 0x0000\n\n{0}\n", exception5.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str5);
                    this.tbReadEndHandle.Focus();
                    flag = true;
                }
                try
                {
                    int length = this.tbReadUUID.Text.Length;
                    if ((length != 5) && (length != 0x2f))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    ruuid.type = this.tbReadUUID.Text;
                }
                catch (Exception exception6)
                {
                    string str6 = string.Format("Invalid UUID Entry.\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n\n{0}\n", exception6.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str6);
                    this.tbReadUUID.Focus();
                    flag = true;
                }
                if (!flag)
                {
                    this.devForm.sendCmds.SendGATT(ruuid);
                }
                else
                {
                    this.tbReadStatus.Text = "Error!!!";
                }
            }
            else if (this.cbReadType.SelectedIndex == 2)
            {
                HCICmds.GATTCmds.GATT_ReadMultiCharValues values = new HCICmds.GATTCmds.GATT_ReadMultiCharValues();
                this.tbReadValue.Tag = string.Empty;
                this.tbReadValue.Text = "";
                this.tbReadStatus.Text = "Reading...";
                try
                {
                    values.connHandle = Convert.ToUInt16(this.tbReadConnHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception7)
                {
                    string str7 = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", exception7.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str7);
                    this.tbReadConnHandle.Focus();
                    flag = true;
                }
                try
                {
                    values.handles = this.tbReadAttrHandle.Text.Trim();
                }
                catch (Exception exception8)
                {
                    string str8 = string.Format("Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n\n{0}\n", exception8.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str8);
                    this.tbReadAttrHandle.Focus();
                    flag = true;
                }
                if (!flag)
                {
                    this.devForm.sendCmds.SendGATT(values);
                }
                else
                {
                    this.tbReadStatus.Text = "Error!!!";
                }
            }
            else if (this.cbReadType.SelectedIndex == 3)
            {
                HCICmds.GATTCmds.GATT_DiscCharsByUUID yuuid = new HCICmds.GATTCmds.GATT_DiscCharsByUUID();
                this.tbReadValue.Tag = string.Empty;
                this.tbReadValue.Text = "";
                this.tbReadStatus.Text = "Reading...";
                try
                {
                    yuuid.connHandle = Convert.ToUInt16(this.tbReadConnHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception9)
                {
                    string str9 = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", exception9.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str9);
                    this.tbReadConnHandle.Focus();
                    flag = true;
                }
                try
                {
                    yuuid.startHandle = Convert.ToUInt16(this.tbReadStartHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception10)
                {
                    string str10 = string.Format("Invalid Start Handle\nFormat: 0x0000\n\n{0}\n", exception10.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str10);
                    this.tbReadStartHandle.Focus();
                    flag = true;
                }
                try
                {
                    yuuid.endHandle = Convert.ToUInt16(this.tbReadEndHandle.Text.Trim(), 0x10);
                }
                catch (Exception exception11)
                {
                    string str11 = string.Format("Invalid End Handle\nFormat: 0x0000\n\n{0}\n", exception11.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str11);
                    this.tbReadEndHandle.Focus();
                    flag = true;
                }
                try
                {
                    int num2 = this.tbReadUUID.Text.Length;
                    if ((num2 != 5) && (num2 != 0x2f))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    yuuid.type = this.tbReadUUID.Text.Trim();
                }
                catch (Exception exception12)
                {
                    string str12 = string.Format("Invalid UUID Entry.\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n\n{0}\n", exception12.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str12);
                    this.tbReadUUID.Focus();
                    flag = true;
                }
                if (!flag)
                {
                    this.devForm.sendCmds.SendGATT(yuuid);
                }
                else
                {
                    this.tbReadStatus.Text = "Error!!!";
                }
            }
        }

        private void btnGATTWriteValue_Click(object sender, EventArgs e)
        {
            bool flag = false;
            this.tbWriteStatus.Text = "Writing...";
            HCICmds.GATTCmds.GATT_WriteCharValue value2 = new HCICmds.GATTCmds.GATT_WriteCharValue();
            try
            {
                value2.connHandle = Convert.ToUInt16(this.tbWriteConnHandle.Text, 0x10);
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid Connection Handle\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.tbWriteConnHandle.Focus();
                flag = true;
            }
            try
            {
                value2.handle = Convert.ToUInt16(this.tbWriteAttrHandle.Text, 0x10);
            }
            catch (Exception exception2)
            {
                string str2 = string.Format("Invalid Characteristic Value Handle(s)\n\n{0}\n", exception2.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                this.tbWriteAttrHandle.Focus();
                flag = true;
            }
            if (this.GATTWriteValueValidation(this.tbWriteValue.Text) && !flag)
            {
                value2.value = (string) this.tbWriteValue.Tag;
                this.devForm.sendCmds.SendGATT(value2, null);
            }
            else
            {
                this.tbWriteValue.Focus();
                this.tbWriteStatus.Text = "Error!!!";
            }
        }

        private void btnGetParams_Click(object sender, EventArgs e)
        {
            this.discoverConnectStatus = DiscoverConnectStatus.GetSet;
            this.DiscoverConnectUserInputControl();
            this.GetConnectionParameters();
            this.discoverConnectStatus = DiscoverConnectStatus.Idle;
            this.DiscoverConnectUserInputControl();
        }

        private void btnInitiateBond_Click(object sender, EventArgs e)
        {
            this.PairBondFieldTabDisable(true);
            string str = string.Empty;
            try
            {
                str = this.tbLongTermKey.Text.Trim();
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid Long Term Key Entry.\n '{0}'\nNo Data Was Loaded.\nFormat Is 00:00....\n\n{1}", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.tbLongTermKey.Focus();
                this.PairBondUserInputControl();
                return;
            }
            if (str.Length == 0x2f)
            {
                ushort num = 0;
                try
                {
                    num = Convert.ToUInt16(this.tbLTKDiversifier.Text.Trim(), 0x10);
                }
                catch (Exception exception2)
                {
                    string str4 = string.Format("Invalid LTK Diversifier Entry.\nFormat: 0x0000\n\n{0}\n", exception2.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                    this.tbLTKDiversifier.Focus();
                    this.PairBondUserInputControl();
                    return;
                }
                string str5 = string.Empty;
                try
                {
                    str5 = this.tbLTKRandom.Text.Trim();
                }
                catch (Exception exception3)
                {
                    string str6 = string.Format("Invalid LTK Random Entry.\n'{0}'\nFormat Is 00:00....\n\n{1}\n", exception3.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str6);
                    this.tbLTKRandom.Focus();
                    this.PairBondUserInputControl();
                    return;
                }
                if (str5.Length != 0x17)
                {
                    string str7 = string.Format("Invalid LTK Random Length = {0:D} \nLength must be {1:D}\n", str5.Length, (byte) 8);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str7);
                    this.tbLTKRandom.Focus();
                    this.PairBondUserInputControl();
                }
                else
                {
                    HCICmds.GAPCmds.GAP_Bond bond = new HCICmds.GAPCmds.GAP_Bond();
                    bond.connHandle = 0;
                    try
                    {
                        bond.connHandle = Convert.ToUInt16(this.tbBondConnHandle.Text.Trim(), 0x10);
                    }
                    catch (Exception exception4)
                    {
                        string str8 = string.Format("Invalid Connection Handle\n\n{0}\n", exception4.Message);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str8);
                        this.tbBondConnHandle.Focus();
                        this.PairBondUserInputControl();
                        return;
                    }
                    if (this.rbAuthBondTrue.Checked)
                    {
                        bond.authenticated = HCICmds.GAP_YesNo.Yes;
                    }
                    else
                    {
                        bond.authenticated = HCICmds.GAP_YesNo.No;
                    }
                    bond.secInfo_LTK = str;
                    bond.secInfo_DIV = num;
                    bond.secInfo_RAND = str5;
                    this.ShowProgress(true);
                    this.devForm.StartTimer(DeviceForm.EventType.PairBond);
                    this.devForm.sendCmds.SendGAP(bond);
                }
            }
            else
            {
                string str3 = string.Format("Invalid Long Term Key Length = {0:D} \nLength must be {1:D}", str.Length, (byte) 0x10);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                this.tbLongTermKey.Focus();
                this.PairBondUserInputControl();
            }
        }

        private void btnLoadLongTermKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text Files|*.txt";
            dialog.Title = "Select a Long-Term Key Data File To Load";
            bool fileError = false;
            string userSelection = null;
            if ((dialog.ShowDialog() == DialogResult.OK) && (this.devForm.numConnections > 1))
            {
                List<string> dataItems = new List<string>();
                for (int i = 0; i < this.devForm.Connections.Count; i++)
                {
                    dataItems.Add(this.devForm.Connections[i].bDA);
                }
                this.listSelectForm.LoadFormData(dataItems);
                this.listSelectForm.ShowDialog();
                if (this.listSelectForm.DialogResult != DialogResult.OK)
                {
                    return;
                }
                userSelection = this.listSelectForm.GetUserSelection();
            }
            this.csvKeyData.Clear();
            this.csvKeyData = this.ReadCsv(dialog.FileName, ref fileError);
            if (!fileError)
            {
                ConnectInfo connectInfo = this.devForm.GetConnectInfo();
                CsvData data = new CsvData();
                if (this.devForm.numConnections > 1)
                {
                    data.addr = userSelection;
                }
                else
                {
                    data.addr = connectInfo.bDA;
                }
                if ((data.addr == null) || (data.addr.Length == 0))
                {
                    string msg = string.Format("Connection Address Is Invalid\nA Device Must Be Connected To Read Data\n", new object[0]);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
                else
                {
                    int csvIndex = -1;
                    if (!this.FindAddrInCsv(data.addr, this.csvKeyData, ref csvIndex))
                    {
                        if (csvIndex == -1)
                        {
                            string str3 = string.Format("Cannot Find The Device Address In The Specified File\nSearch Address = {0:S}\nNo Data Was Loaded.\n", data.addr);
                            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                        }
                        else
                        {
                            data = this.csvKeyData[csvIndex];
                            if (data.auth == "TRUE")
                            {
                                this.rbAuthBondTrue.Checked = true;
                                this.rbAuthBondFalse.Checked = false;
                            }
                            else
                            {
                                this.rbAuthBondTrue.Checked = false;
                                this.rbAuthBondFalse.Checked = true;
                            }
                            this.tbLongTermKey.Text = data.ltk;
                            this.tbLTKDiversifier.Text = data.div;
                            this.tbLTKRandom.Text = data.rand;
                        }
                    }
                }
            }
        }

        private void btnSaveLongTermKey_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text Files|*.txt";
            dialog.Title = "Select a Long-Term Key Data File To Save";
            bool fileError = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.csvKeyData.Clear();
                if (File.Exists(dialog.FileName))
                {
                    this.csvKeyData = this.ReadCsv(dialog.FileName, ref fileError);
                }
                ConnectInfo connectInfo = this.devForm.GetConnectInfo();
                CsvData newCsvData = new CsvData();
                newCsvData.addr = connectInfo.bDA;
                newCsvData.auth = this.lastAuthStr;
                newCsvData.ltk = this.lastGAP_AuthenticationComplete.devSecInfo_LTK;
                newCsvData.div = Convert.ToString((int) this.lastGAP_AuthenticationComplete.devSecInfo_DIV, 0x10).ToUpper();
                newCsvData.rand = this.lastGAP_AuthenticationComplete.devSecInfo_RAND;
                if ((newCsvData.addr == null) || (newCsvData.addr.Length == 0))
                {
                    string msg = string.Format("Connection Address Is Invalid\nDevice Must Be Connected To Save Data\n", new object[0]);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
                else
                {
                    int csvIndex = -1;
                    if (!this.FindAddrInCsv(newCsvData.addr, this.csvKeyData, ref csvIndex))
                    {
                        if (csvIndex == -1)
                        {
                            if (this.AddToEndCsv(newCsvData, ref this.csvKeyData))
                            {
                                return;
                            }
                        }
                        else if (this.ReplaceAddrDataInCsv(newCsvData, ref this.csvKeyData, csvIndex))
                        {
                            return;
                        }
                        this.WriteCsv(dialog.FileName, this.csvKeyData);
                    }
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (this.Cursor != Cursors.WaitCursor)
            {
                this.ShowProgress(true);
                this.devForm.StartTimer(DeviceForm.EventType.Scan);
                this.discoverConnectStatus = DiscoverConnectStatus.Scan;
                this.DiscoverConnectUserInputControl();
                this.ResetSlaveDevices();
                this.SlaveDeviceFound = 0;
                this.lblDeviceFound.Text = this.SlaveDeviceFound.ToString();
                HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest request = new HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest();
                if (this.ckBoxActiveScan.Checked)
                {
                    request.activeScan = HCICmds.GAP_EnableDisable.Enable;
                }
                else
                {
                    request.activeScan = HCICmds.GAP_EnableDisable.Disable;
                }
                if (this.ckBoxWhiteList.Checked)
                {
                    request.whiteList = HCICmds.GAP_EnableDisable.Enable;
                }
                else
                {
                    request.whiteList = HCICmds.GAP_EnableDisable.Disable;
                }
                request.mode = (HCICmds.GAP_DiscoveryMode) this.cbScanMode.SelectedIndex;
                this.devForm.sendCmds.SendGAP(request);
            }
        }

        private void btnScanCancel_Click(object sender, EventArgs e)
        {
            this.discoverConnectStatus = DiscoverConnectStatus.ScanCancel;
            this.DiscoverConnectUserInputControl();
            HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel cancel = new HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel();
            this.devForm.sendCmds.SendGAP(cancel);
        }

        private void btnSendPairingRequest_Click(object sender, EventArgs e)
        {
            this.PairBondFieldTabDisable(true);
            HCICmds.GAPCmds.GAP_Authenticate authenticate = new HCICmds.GAPCmds.GAP_Authenticate();
            authenticate.connHandle = this.devForm.devInfo.handle;
            try
            {
                authenticate.connHandle = Convert.ToUInt16(this.tbPairingConnHandle.Text.Trim(), 0x10);
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.tbPairingConnHandle.Focus();
                this.PairBondUserInputControl();
                return;
            }
            this.tbPasskeyConnHandle.Text = this.tbPairingConnHandle.Text;
            authenticate.secReq_ioCaps = HCICmds.GAP_IOCaps.KeyboardDisplay;
            authenticate.secReq_oobAvailable = HCICmds.GAP_TrueFalse.False;
            authenticate.secReq_oob = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
            byte num = 0;
            if (this.ckBoxBondingEnabled.Checked && this.ckBoxAuthMitmEnabled.Checked)
            {
                num = 5;
            }
            else if (this.ckBoxBondingEnabled.Checked)
            {
                num = 1;
            }
            else if (this.ckBoxAuthMitmEnabled.Checked)
            {
                num = 4;
            }
            authenticate.secReq_authReq = num;
            authenticate.secReq_maxEncKeySize = 0x10;
            authenticate.secReq_keyDist = 0x3f;
            authenticate.pairReq_Enable = HCICmds.GAP_EnableDisable.Disable;
            authenticate.pairReq_ioCaps = HCICmds.GAP_IOCaps.NoInputNoOutput;
            authenticate.pairReq_oobDataFlag = HCICmds.GAP_EnableDisable.Disable;
            authenticate.pairReq_authReq = 1;
            authenticate.pairReq_maxEncKeySize = 0x10;
            authenticate.pairReq_keyDist = 0x3f;
            this.ShowProgress(true);
            this.devForm.StartTimer(DeviceForm.EventType.PairBond);
            this.devForm.sendCmds.SendGAP(authenticate);
        }

        private void btnSendPassKey_Click(object sender, EventArgs e)
        {
            this.PairBondFieldTabDisable(true);
            HCICmds.GAPCmds.GAP_PasskeyUpdate update = new HCICmds.GAPCmds.GAP_PasskeyUpdate();
            try
            {
                update.connHandle = Convert.ToUInt16(this.tbPasskeyConnHandle.Text.Trim(), 0x10);
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.tbPasskeyConnHandle.Focus();
                this.PairBondUserInputControl();
                return;
            }
            update.passKey = this.tbPasskey.Text;
            this.ShowProgress(true);
            this.devForm.StartTimer(DeviceForm.EventType.PairBond);
            if (!this.devForm.sendCmds.SendGAP(update))
            {
                if (this.tcDeviceTabs.SelectedIndex == 2)
                {
                    this.devForm.StopTimer(DeviceForm.EventType.PairBond);
                    this.ShowProgress(false);
                    this.PairBondUserInputControl();
                }
                string str2 = string.Format("Invalid Passkey Length\nLength must be {0:D}", (byte) 6);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
            }
        }

        private void btnSendShared_Click(object sender, EventArgs e)
        {
            switch (this.tcDeviceTabs.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                    break;

                case 3:
                    this.tsmiSendAdvCmd_Click(sender, e);
                    break;

                default:
                    return;
            }
        }

        private void btnSetParams_Click(object sender, EventArgs e)
        {
            this.discoverConnectStatus = DiscoverConnectStatus.GetSet;
            this.DiscoverConnectUserInputControl();
            this.SetConnectionParameters();
            this.discoverConnectStatus = DiscoverConnectStatus.Idle;
            this.DiscoverConnectUserInputControl();
        }

        private void btnTerminate_Click(object sender, EventArgs e)
        {
            this.discoverConnectStatus = DiscoverConnectStatus.Terminate;
            this.DiscoverConnectUserInputControl();
            HCICmds.GAPCmds.GAP_TerminateLinkRequest request = new HCICmds.GAPCmds.GAP_TerminateLinkRequest();
            try
            {
                request.connHandle = Convert.ToUInt16(this.tbTermConnHandle.Text, 0x10);
                this.devForm.sendCmds.SendGAP(request);
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid Connection Handle\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.tbTermConnHandle.Focus();
            }
        }

        private void cbConnSlaveDeviceBDAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.linkSlaves.Count >= (this.cbConnSlaveDeviceBDAddress.SelectedIndex + 1))
            {
                this.SetAddrType((byte) this.linkSlaves[this.cbConnSlaveDeviceBDAddress.SelectedIndex].addrType);
            }
        }

        private void CheckHexKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Regex.IsMatch(e.KeyChar.ToString(), @"\b[0-9a-fA-F]+\b") && (e.KeyChar != '\b')) && ((e.KeyChar != ' ') && (e.KeyChar != ':')))
            {
                e.Handled = true;
            }
        }

        private void ConnSettingsUserInputControl(bool enabled)
        {
            this.nudMinConnInt.Enabled = enabled;
            this.nudMaxConnInt.Enabled = enabled;
            this.nudSlaveLatency.Enabled = enabled;
            this.nudSprVisionTimeout.Enabled = enabled;
            this.btnGetConnectionParams.Enabled = enabled;
            this.btnSetConnectionParams.Enabled = enabled;
        }

        private void DeviceTabsForm_Load(object sender, EventArgs e)
        {
            this.TabDiscoverConnectToolTips();
            this.TabReadWriteToolTips();
            this.TabPairBondToolTips();
            this.TabAdvCommandsToolTips();
            ushort num = 0xfffe;
            this.tbTermConnHandle.Text = "0x" + num.ToString("X4");
            this.tbReadConnHandle.Text = "0x" + num.ToString("X4");
            this.tbWriteConnHandle.Text = "0x" + num.ToString("X4");
            this.tbPairingConnHandle.Text = "0x" + num.ToString("X4");
            this.tbPasskeyConnHandle.Text = "0x" + num.ToString("X4");
            this.tbBondConnHandle.Text = "0x" + num.ToString("X4");
        }

        private void DeviceTabsForm_Scroll(object sender, ScrollEventArgs e)
        {
            if (!base.ContainsFocus)
            {
                base.Focus();
            }
        }

        public void DeviceTabsUpdate()
        {
            this.tcDeviceTabs.Update();
        }

        public void DiscoverConnectUserInputControl()
        {
            switch (this.discoverConnectStatus)
            {
                case DiscoverConnectStatus.Idle:
                    this.DiscoveryUserInputControl(true);
                    this.ConnSettingsUserInputControl(true);
                    this.EstablishLinkUserInputControl(true);
                    this.TerminateLinkUserInputControl(true);
                    return;

                case DiscoverConnectStatus.Scan:
                    this.DiscoveryUserInputControl(false);
                    this.ConnSettingsUserInputControl(false);
                    this.EstablishLinkUserInputControl(false);
                    this.TerminateLinkUserInputControl(false);
                    this.btnScanCancel.Enabled = true;
                    return;

                case DiscoverConnectStatus.ScanCancel:
                case DiscoverConnectStatus.GetSet:
                case DiscoverConnectStatus.EstablishCancel:
                case DiscoverConnectStatus.Terminate:
                    this.DiscoveryUserInputControl(false);
                    this.ConnSettingsUserInputControl(false);
                    this.EstablishLinkUserInputControl(false);
                    this.TerminateLinkUserInputControl(false);
                    return;

                case DiscoverConnectStatus.Establish:
                    this.DiscoveryUserInputControl(false);
                    this.ConnSettingsUserInputControl(false);
                    this.EstablishLinkUserInputControl(false);
                    this.TerminateLinkUserInputControl(false);
                    this.btnEstablishCancel.Enabled = true;
                    return;
            }
        }

        private void DiscoveryUserInputControl(bool enabled)
        {
            this.ckBoxActiveScan.Enabled = enabled;
            this.ckBoxWhiteList.Enabled = enabled;
            this.cbScanMode.Enabled = enabled;
            this.btnScan.Enabled = enabled;
            this.btnScanCancel.Enabled = enabled;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EstablishLinkUserInputControl(bool enabled)
        {
            this.cbConnAddrType.Enabled = enabled;
            this.cbConnSlaveDeviceBDAddress.Enabled = enabled;
            this.ckBoxConnWhiteList.Enabled = enabled;
            this.btnEstablish.Enabled = enabled;
            this.btnEstablishCancel.Enabled = enabled;
        }

        private bool FindAddrInCsv(string addr, List<CsvData> csvData, ref int csvIndex)
        {
            bool flag = false;
            csvIndex = -1;
            try
            {
                if (((addr == null) || (csvData == null)) || (csvData.Count <= 0))
                {
                    csvIndex = -1;
                    return flag;
                }
                int count = csvData.Count;
                CsvData data = new CsvData();
                for (int i = 0; i < count; i++)
                {
                    data = csvData[i];
                    if (data.addr == addr)
                    {
                        csvIndex = i;
                        return flag;
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Cannot Access The Data To Find The Addr In The CSV List\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = true;
            }
            return flag;
        }

        private bool GATTWriteValueValidation(string valStr)
        {
            byte index = 0;
            string str = string.Empty;
            if (this.rbHexWrite.Checked)
            {
                if (this.devUtils.String2Bytes_LSBMSB(valStr, 0x10) != null)
                {
                    this.tbWriteValue.Tag = valStr;
                    return true;
                }
                string msg = string.Format("Invalid Hex Value '{0}'\nFormat#1: 11:22:33:44:55:66\nFormat#2: 11 22 33 44 55 66\n", valStr);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                return false;
            }
            if (this.rbDecimalWrite.Checked)
            {
                try
                {
                    byte[] bytes = BitConverter.GetBytes(Convert.ToUInt32(valStr, 10));
                    int num4 = bytes.Length - 1;
                    for (int i = 0; i < (bytes.Length / 2); i++)
                    {
                        byte num3 = bytes[i];
                        bytes[i] = bytes[num4];
                        bytes[num4] = num3;
                        num4--;
                    }
                    if (bytes != null)
                    {
                        bool flag = false;
                        index = 0;
                        while (index < bytes.Length)
                        {
                            if (!flag)
                            {
                                if ((index < 3) && (bytes[index] == 0))
                                {
                                    goto Label_00E9;
                                }
                                flag = true;
                            }
                            str = str + string.Format("{0:X2} ", bytes[index]);
                        Label_00E9:
                            index = (byte) (index + 1);
                        }
                        str = str.Trim();
                        this.tbWriteValue.Tag = str;
                        return true;
                    }
                    string str3 = string.Format("Invalid Dec Value '{0}'\nValid Range 0 to 4294967295\n", valStr);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                    return false;
                }
                catch (Exception exception)
                {
                    string str4 = string.Format("Invalid Dec Value '{0}'\nValid Range 0 to 4294967295\n\n{1}", valStr, exception.Message);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                    return false;
                }
            }
            if (this.rbASCIIWrite.Checked)
            {
                byte[] buffer3 = this.devUtils.String2Bytes_LSBMSB(valStr, 0xff);
                if (buffer3 != null)
                {
                    for (index = 0; index < buffer3.Length; index = (byte) (index + 1))
                    {
                        str = str + string.Format("{0:X2} ", buffer3[index]);
                    }
                    str = str.Trim();
                    this.tbWriteValue.Tag = str;
                    return true;
                }
                string str5 = string.Format("Invalid ASCII Value '{0}'\nFormat: Sample\n", valStr);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str5);
            }
            return false;
        }

        private bool GetAuthenticationEnabled()
        {
            return this.rbAuthBondTrue.Checked;
        }

        private bool GetBondingEnabled()
        {
            return this.ckBoxBondingEnabled.Checked;
        }

        public void GetConnectionParameters()
        {
            this.devForm.ConnParamState = DeviceForm.GAPGetConnectionParams.MinConnIntSeq;
            HCICmds.GAPCmds.GAP_GetParam param = new HCICmds.GAPCmds.GAP_GetParam();
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MIN;
            this.devForm.sendCmds.SendGAP(param);
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MAX;
            this.devForm.sendCmds.SendGAP(param);
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_LATENCY;
            this.devForm.sendCmds.SendGAP(param);
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_SUPERV_TIMEOUT;
            this.devForm.sendCmds.SendGAP(param);
        }

        private string GetPairingStatusStr(PairingStatus status)
        {
            switch (status)
            {
                case PairingStatus.Empty:
                    return "";

                case PairingStatus.NotConnected:
                    return "Not Connected";

                case PairingStatus.NotPaired:
                    return "Not Paired";

                case PairingStatus.PasskeyNeeded:
                    return "Passkey Needed";

                case PairingStatus.DevicesPairedBonded:
                    return "Devices Paired And Bonded";

                case PairingStatus.DevicesPaired:
                    return "Devices Paired";

                case PairingStatus.PasskeyIncorrect:
                    return "Passkey Incorrect";

                case PairingStatus.ConnectionTimedOut:
                    return "Connection Timed Out";
            }
            return "Unknown Pairing Status";
        }

        public bool GetRbASCIIReadChecked()
        {
            return this.rbASCIIRead.Checked;
        }

        public bool GetRbDecimalReadChecked()
        {
            return this.rbDecimalRead.Checked;
        }

        public int GetSelectedTab()
        {
            return this.tcDeviceTabs.SelectedIndex;
        }

        public string GetTbReadStatusText()
        {
            return this.tbReadStatus.Text;
        }

        public string GetTbWriteStatusText()
        {
            return this.tbWriteStatus.Text;
        }

        public int GetTcDeviceTabsWidth()
        {
            return this.tcDeviceTabs.Width;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tcDeviceTabs = new TabControl();
            this.tpDiscoverConnect = new TabPage();
            this.gbLinkControl = new GroupBox();
            this.gbTerminateLink = new GroupBox();
            this.lblTerminateLink = new Label();
            this.btnTerminate = new Button();
            this.tbTermConnHandle = new TextBox();
            this.lblConnHandle = new Label();
            this.gbEstablishLink = new GroupBox();
            this.btnEstablishCancel = new Button();
            this.ckBoxConnWhiteList = new CheckBox();
            this.btnEstablish = new Button();
            this.cbConnSlaveDeviceBDAddress = new ComboBox();
            this.lblAddrType = new Label();
            this.lblSlaveBDA = new Label();
            this.cbConnAddrType = new ComboBox();
            this.lblEstablishLink = new Label();
            this.gbConnSettings = new GroupBox();
            this.btnSetConnectionParams = new Button();
            this.btnGetConnectionParams = new Button();
            this.nudSprVisionTimeout = new NumericUpDown();
            this.nudSlaveLatency = new NumericUpDown();
            this.nudMaxConnInt = new NumericUpDown();
            this.nudMinConnInt = new NumericUpDown();
            this.lblSupervisionTimeout = new Label();
            this.lblMaxConnInt = new Label();
            this.lblMinConnInt = new Label();
            this.lblSuperTimeout = new Label();
            this.lblSlaveLat = new Label();
            this.lblMaxConn = new Label();
            this.lblMinConn = new Label();
            this.gbDiscovery = new GroupBox();
            this.btnScanCancel = new Button();
            this.ckBoxWhiteList = new CheckBox();
            this.ckBoxActiveScan = new CheckBox();
            this.lblMode = new Label();
            this.cbScanMode = new ComboBox();
            this.lblDeviceFound = new Label();
            this.lblDevsFound = new Label();
            this.btnScan = new Button();
            this.tpReadWrite = new TabPage();
            this.gbCharWrite = new GroupBox();
            this.gbWriteArea = new GroupBox();
            this.lblWriteStatus = new Label();
            this.lblWriteValue = new Label();
            this.btnWriteGATTValue = new Button();
            this.tbWriteStatus = new TextBox();
            this.rbASCIIWrite = new RadioButton();
            this.rbHexWrite = new RadioButton();
            this.rbDecimalWrite = new RadioButton();
            this.tbWriteValue = new TextBox();
            this.tbWriteConnHandle = new TextBox();
            this.lblWriteConnHnd = new Label();
            this.lblWriteHandle = new Label();
            this.tbWriteAttrHandle = new TextBox();
            this.gbCharRead = new GroupBox();
            this.lblReadSubProc = new Label();
            this.cbReadType = new ComboBox();
            this.lblReadStartHnd = new Label();
            this.lblReadEndHnd = new Label();
            this.lblReadCharUuid = new Label();
            this.tbReadUUID = new TextBox();
            this.tbReadStartHandle = new TextBox();
            this.tbReadEndHandle = new TextBox();
            this.gbReadArea = new GroupBox();
            this.lbReadValue = new Label();
            this.lblReadStatus = new Label();
            this.rbASCIIRead = new RadioButton();
            this.tbReadStatus = new TextBox();
            this.rbHexRead = new RadioButton();
            this.btnReadGATTValue = new Button();
            this.rbDecimalRead = new RadioButton();
            this.tbReadValue = new TextBox();
            this.tbReadConnHandle = new TextBox();
            this.lblReadConnHnd = new Label();
            this.lblReadValueHnd = new Label();
            this.tbReadAttrHandle = new TextBox();
            this.tpPairingBonding = new TabPage();
            this.gbLongTermKeyData = new GroupBox();
            this.btnSaveLongTermKey = new Button();
            this.tbLongTermKeyData = new TextBox();
            this.gbEncryptLTKey = new GroupBox();
            this.tbBondConnHandle = new TextBox();
            this.lblLtkConnHnd = new Label();
            this.btnEncryptLink = new Button();
            this.btnLoadLongTermKey = new Button();
            this.tbLTKRandom = new TextBox();
            this.tbLTKDiversifier = new TextBox();
            this.tbLongTermKey = new TextBox();
            this.lblLtkRandom = new Label();
            this.lblLtkDiv = new Label();
            this.lblLtk = new Label();
            this.rbAuthBondFalse = new RadioButton();
            this.rbAuthBondTrue = new RadioButton();
            this.lblAuthBond = new Label();
            this.gbPasskeyInput = new GroupBox();
            this.lblConnHnd = new Label();
            this.tbPasskeyConnHandle = new TextBox();
            this.btnSendPasskey = new Button();
            this.lblPassRange = new Label();
            this.tbPasskey = new TextBox();
            this.lblPasskey = new Label();
            this.gbInitParing = new GroupBox();
            this.tbPairingConnHandle = new TextBox();
            this.lblPairConnHnd = new Label();
            this.btnSendPairingRequest = new Button();
            this.ckBoxAuthMitmEnabled = new CheckBox();
            this.ckBoxBondingEnabled = new CheckBox();
            this.labelPairingStatus = new Label();
            this.tbPairingStatus = new TextBox();
            this.tpAdvCommands = new TabPage();
            this.scTreeGrid = new SplitContainer();
            this.tvAdvCmdList = new TreeView();
            this.pgAdvCmds = new PropertyGrid();
            this.cmsAdvTab = new ContextMenuStrip(this.components);
            this.tsmiSendAdvCmd = new ToolStripMenuItem();
            this.btnSendShared = new Button();
            this.pbSharedDevice = new ProgressBar();
            this.tcDeviceTabs.SuspendLayout();
            this.tpDiscoverConnect.SuspendLayout();
            this.gbLinkControl.SuspendLayout();
            this.gbTerminateLink.SuspendLayout();
            this.gbEstablishLink.SuspendLayout();
            this.gbConnSettings.SuspendLayout();
            this.gbDiscovery.SuspendLayout();
            this.tpReadWrite.SuspendLayout();
            this.gbCharWrite.SuspendLayout();
            this.gbWriteArea.SuspendLayout();
            this.gbCharRead.SuspendLayout();
            this.gbReadArea.SuspendLayout();
            this.tpPairingBonding.SuspendLayout();
            this.gbLongTermKeyData.SuspendLayout();
            this.gbEncryptLTKey.SuspendLayout();
            this.gbPasskeyInput.SuspendLayout();
            this.gbInitParing.SuspendLayout();
            this.tpAdvCommands.SuspendLayout();
            this.scTreeGrid.Panel1.SuspendLayout();
            this.scTreeGrid.Panel2.SuspendLayout();
            this.scTreeGrid.SuspendLayout();
            this.cmsAdvTab.SuspendLayout();
            base.SuspendLayout();
            this.tcDeviceTabs.Controls.Add(this.tpDiscoverConnect);
            this.tcDeviceTabs.Controls.Add(this.tpReadWrite);
            this.tcDeviceTabs.Controls.Add(this.tpPairingBonding);
            this.tcDeviceTabs.Controls.Add(this.tpAdvCommands);
            this.tcDeviceTabs.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.tcDeviceTabs.Location = new Point(1, 1);
            this.tcDeviceTabs.Margin = new Padding(2, 3, 2, 3);
            this.tcDeviceTabs.Name = "tcDeviceTabs";
            this.tcDeviceTabs.SelectedIndex = 0;
            this.tcDeviceTabs.Size = new Size(0x18b, 0x216);
            this.tcDeviceTabs.TabIndex = 1;
            this.tcDeviceTabs.Selected += new TabControlEventHandler(this.tcDeviceTab_Selected);
            this.tpDiscoverConnect.BackColor = Color.Transparent;
            this.tpDiscoverConnect.Controls.Add(this.gbLinkControl);
            this.tpDiscoverConnect.Controls.Add(this.gbConnSettings);
            this.tpDiscoverConnect.Controls.Add(this.gbDiscovery);
            this.tpDiscoverConnect.Location = new Point(4, 0x16);
            this.tpDiscoverConnect.Margin = new Padding(2, 3, 2, 3);
            this.tpDiscoverConnect.Name = "tpDiscoverConnect";
            this.tpDiscoverConnect.Padding = new Padding(2, 3, 2, 3);
            this.tpDiscoverConnect.Size = new Size(0x183, 0x1fc);
            this.tpDiscoverConnect.TabIndex = 1;
            this.tpDiscoverConnect.Text = "Discover / Connect";
            this.tpDiscoverConnect.UseVisualStyleBackColor = true;
            this.gbLinkControl.Controls.Add(this.gbTerminateLink);
            this.gbLinkControl.Controls.Add(this.gbEstablishLink);
            this.gbLinkControl.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gbLinkControl.Location = new Point(4, 290);
            this.gbLinkControl.Margin = new Padding(2, 3, 2, 3);
            this.gbLinkControl.Name = "gbLinkControl";
            this.gbLinkControl.Padding = new Padding(2, 3, 2, 3);
            this.gbLinkControl.Size = new Size(0x179, 0xd5);
            this.gbLinkControl.TabIndex = 5;
            this.gbLinkControl.TabStop = false;
            this.gbLinkControl.Text = "Link Control";
            this.gbTerminateLink.BackColor = Color.Transparent;
            this.gbTerminateLink.Controls.Add(this.lblTerminateLink);
            this.gbTerminateLink.Controls.Add(this.btnTerminate);
            this.gbTerminateLink.Controls.Add(this.tbTermConnHandle);
            this.gbTerminateLink.Controls.Add(this.lblConnHandle);
            this.gbTerminateLink.Location = new Point(7, 0x8d);
            this.gbTerminateLink.Margin = new Padding(2, 3, 2, 3);
            this.gbTerminateLink.Name = "gbTerminateLink";
            this.gbTerminateLink.Padding = new Padding(2, 3, 2, 3);
            this.gbTerminateLink.Size = new Size(0x16d, 0x41);
            this.gbTerminateLink.TabIndex = 0x13;
            this.gbTerminateLink.TabStop = false;
            this.lblTerminateLink.AutoSize = true;
            this.lblTerminateLink.BackColor = SystemColors.ControlLight;
            this.lblTerminateLink.Location = new Point(0x86, 2);
            this.lblTerminateLink.Margin = new Padding(2, 0, 2, 0);
            this.lblTerminateLink.Name = "lblTerminateLink";
            this.lblTerminateLink.Size = new Size(0x4d, 13);
            this.lblTerminateLink.TabIndex = 0x13;
            this.lblTerminateLink.Text = "Terminate Link";
            this.btnTerminate.BackColor = SystemColors.Control;
            this.btnTerminate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnTerminate.Location = new Point(0xe3, 0x17);
            this.btnTerminate.Margin = new Padding(2, 3, 2, 3);
            this.btnTerminate.Name = "btnTerminate";
            this.btnTerminate.Size = new Size(0x7e, 0x1c);
            this.btnTerminate.TabIndex = 0;
            this.btnTerminate.Text = "Terminate";
            this.btnTerminate.UseVisualStyleBackColor = true;
            this.btnTerminate.Click += new EventHandler(this.btnTerminate_Click);
            this.tbTermConnHandle.Location = new Point(0x7e, 0x1a);
            this.tbTermConnHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbTermConnHandle.MaxLength = 6;
            this.tbTermConnHandle.Name = "tbTermConnHandle";
            this.tbTermConnHandle.Size = new Size(60, 20);
            this.tbTermConnHandle.TabIndex = 0x11;
            this.tbTermConnHandle.Text = "0x0000";
            this.tbTermConnHandle.TextAlign = HorizontalAlignment.Center;
            this.lblConnHandle.AutoSize = true;
            this.lblConnHandle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblConnHandle.Location = new Point(0x10, 0x1d);
            this.lblConnHandle.Margin = new Padding(2, 0, 2, 0);
            this.lblConnHandle.Name = "lblConnHandle";
            this.lblConnHandle.Size = new Size(0x65, 13);
            this.lblConnHandle.TabIndex = 0x12;
            this.lblConnHandle.Text = "Connection Handle:";
            this.gbEstablishLink.BackColor = Color.Transparent;
            this.gbEstablishLink.Controls.Add(this.btnEstablishCancel);
            this.gbEstablishLink.Controls.Add(this.ckBoxConnWhiteList);
            this.gbEstablishLink.Controls.Add(this.btnEstablish);
            this.gbEstablishLink.Controls.Add(this.cbConnSlaveDeviceBDAddress);
            this.gbEstablishLink.Controls.Add(this.lblAddrType);
            this.gbEstablishLink.Controls.Add(this.lblSlaveBDA);
            this.gbEstablishLink.Controls.Add(this.cbConnAddrType);
            this.gbEstablishLink.Controls.Add(this.lblEstablishLink);
            this.gbEstablishLink.Location = new Point(7, 15);
            this.gbEstablishLink.Margin = new Padding(2, 3, 2, 3);
            this.gbEstablishLink.Name = "gbEstablishLink";
            this.gbEstablishLink.Padding = new Padding(2, 3, 2, 3);
            this.gbEstablishLink.Size = new Size(0x16d, 0x7b);
            this.gbEstablishLink.TabIndex = 20;
            this.gbEstablishLink.TabStop = false;
            this.btnEstablishCancel.BackColor = SystemColors.Control;
            this.btnEstablishCancel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnEstablishCancel.Location = new Point(0xe5, 0x53);
            this.btnEstablishCancel.Margin = new Padding(2, 3, 2, 3);
            this.btnEstablishCancel.Name = "btnEstablishCancel";
            this.btnEstablishCancel.Size = new Size(0x7e, 0x1c);
            this.btnEstablishCancel.TabIndex = 0x12;
            this.btnEstablishCancel.Text = "Cancel";
            this.btnEstablishCancel.UseVisualStyleBackColor = true;
            this.btnEstablishCancel.Click += new EventHandler(this.btnEstablishCancel_Click);
            this.ckBoxConnWhiteList.AutoSize = true;
            this.ckBoxConnWhiteList.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ckBoxConnWhiteList.Location = new Point(0x113, 0x1c);
            this.ckBoxConnWhiteList.Margin = new Padding(2, 3, 2, 3);
            this.ckBoxConnWhiteList.Name = "ckBoxConnWhiteList";
            this.ckBoxConnWhiteList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckBoxConnWhiteList.Size = new Size(70, 0x11);
            this.ckBoxConnWhiteList.TabIndex = 14;
            this.ckBoxConnWhiteList.Text = "WhiteList";
            this.ckBoxConnWhiteList.UseVisualStyleBackColor = true;
            this.btnEstablish.BackColor = SystemColors.Control;
            this.btnEstablish.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnEstablish.Location = new Point(10, 0x53);
            this.btnEstablish.Margin = new Padding(2, 3, 2, 3);
            this.btnEstablish.Name = "btnEstablish";
            this.btnEstablish.Size = new Size(0x7e, 0x1c);
            this.btnEstablish.TabIndex = 1;
            this.btnEstablish.Text = "Establish";
            this.btnEstablish.UseVisualStyleBackColor = true;
            this.btnEstablish.Click += new EventHandler(this.btnEstablish_Click);
            this.cbConnSlaveDeviceBDAddress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cbConnSlaveDeviceBDAddress.FormattingEnabled = true;
            this.cbConnSlaveDeviceBDAddress.Location = new Point(0x60, 0x36);
            this.cbConnSlaveDeviceBDAddress.Margin = new Padding(2, 3, 2, 3);
            this.cbConnSlaveDeviceBDAddress.MaxLength = 0x11;
            this.cbConnSlaveDeviceBDAddress.Name = "cbConnSlaveDeviceBDAddress";
            this.cbConnSlaveDeviceBDAddress.Size = new Size(150, 0x15);
            this.cbConnSlaveDeviceBDAddress.TabIndex = 2;
            this.cbConnSlaveDeviceBDAddress.SelectedIndexChanged += new EventHandler(this.cbConnSlaveDeviceBDAddress_SelectedIndexChanged);
            this.lblAddrType.AutoSize = true;
            this.lblAddrType.BackColor = Color.Transparent;
            this.lblAddrType.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblAddrType.Location = new Point(0x23, 0x1c);
            this.lblAddrType.Margin = new Padding(2, 0, 2, 0);
            this.lblAddrType.Name = "lblAddrType";
            this.lblAddrType.Size = new Size(0x38, 13);
            this.lblAddrType.TabIndex = 0x10;
            this.lblAddrType.Text = "AddrType:";
            this.lblSlaveBDA.AutoSize = true;
            this.lblSlaveBDA.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSlaveBDA.Location = new Point(0x1d, 0x39);
            this.lblSlaveBDA.Margin = new Padding(2, 0, 2, 0);
            this.lblSlaveBDA.Name = "lblSlaveBDA";
            this.lblSlaveBDA.Size = new Size(0x3e, 13);
            this.lblSlaveBDA.TabIndex = 12;
            this.lblSlaveBDA.Text = "Slave BDA:";
            this.cbConnAddrType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbConnAddrType.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cbConnAddrType.FormattingEnabled = true;
            this.cbConnAddrType.Items.AddRange(new object[] { "0x00 (Public)", "0x01 (Static)", "0x02 (PrivateNonResolve)", "0x03 (PrivateResolve)" });
            this.cbConnAddrType.Location = new Point(0x60, 0x18);
            this.cbConnAddrType.Margin = new Padding(2, 3, 2, 3);
            this.cbConnAddrType.Name = "cbConnAddrType";
            this.cbConnAddrType.Size = new Size(150, 0x15);
            this.cbConnAddrType.TabIndex = 15;
            this.lblEstablishLink.AutoSize = true;
            this.lblEstablishLink.BackColor = SystemColors.ControlLight;
            this.lblEstablishLink.Location = new Point(0x8b, 0);
            this.lblEstablishLink.Margin = new Padding(2, 0, 2, 0);
            this.lblEstablishLink.Name = "lblEstablishLink";
            this.lblEstablishLink.Size = new Size(0x48, 13);
            this.lblEstablishLink.TabIndex = 0x11;
            this.lblEstablishLink.Text = "Establish Link";
            this.gbConnSettings.BackColor = Color.Transparent;
            this.gbConnSettings.Controls.Add(this.btnSetConnectionParams);
            this.gbConnSettings.Controls.Add(this.btnGetConnectionParams);
            this.gbConnSettings.Controls.Add(this.nudSprVisionTimeout);
            this.gbConnSettings.Controls.Add(this.nudSlaveLatency);
            this.gbConnSettings.Controls.Add(this.nudMaxConnInt);
            this.gbConnSettings.Controls.Add(this.nudMinConnInt);
            this.gbConnSettings.Controls.Add(this.lblSupervisionTimeout);
            this.gbConnSettings.Controls.Add(this.lblMaxConnInt);
            this.gbConnSettings.Controls.Add(this.lblMinConnInt);
            this.gbConnSettings.Controls.Add(this.lblSuperTimeout);
            this.gbConnSettings.Controls.Add(this.lblSlaveLat);
            this.gbConnSettings.Controls.Add(this.lblMaxConn);
            this.gbConnSettings.Controls.Add(this.lblMinConn);
            this.gbConnSettings.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gbConnSettings.Location = new Point(4, 0x74);
            this.gbConnSettings.Margin = new Padding(2, 3, 2, 3);
            this.gbConnSettings.Name = "gbConnSettings";
            this.gbConnSettings.Padding = new Padding(2, 3, 2, 3);
            this.gbConnSettings.Size = new Size(0x179, 0xab);
            this.gbConnSettings.TabIndex = 4;
            this.gbConnSettings.TabStop = false;
            this.gbConnSettings.Text = "Connection Settings";
            this.btnSetConnectionParams.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnSetConnectionParams.Location = new Point(0xec, 130);
            this.btnSetConnectionParams.Margin = new Padding(2, 3, 2, 3);
            this.btnSetConnectionParams.Name = "btnSetConnectionParams";
            this.btnSetConnectionParams.Size = new Size(0x7e, 0x1c);
            this.btnSetConnectionParams.TabIndex = 0x16;
            this.btnSetConnectionParams.Text = "Set";
            this.btnSetConnectionParams.UseVisualStyleBackColor = true;
            this.btnSetConnectionParams.Click += new EventHandler(this.btnSetParams_Click);
            this.btnGetConnectionParams.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnGetConnectionParams.Location = new Point(15, 130);
            this.btnGetConnectionParams.Margin = new Padding(2, 3, 2, 3);
            this.btnGetConnectionParams.Name = "btnGetConnectionParams";
            this.btnGetConnectionParams.Size = new Size(0x7e, 0x1c);
            this.btnGetConnectionParams.TabIndex = 0x15;
            this.btnGetConnectionParams.Text = "Get";
            this.btnGetConnectionParams.UseVisualStyleBackColor = true;
            this.btnGetConnectionParams.Click += new EventHandler(this.btnGetParams_Click);
            this.nudSprVisionTimeout.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.nudSprVisionTimeout.Location = new Point(0xeb, 0x65);
            this.nudSprVisionTimeout.Margin = new Padding(2, 3, 2, 3);
            int[] bits = new int[4];
            bits[0] = 0xc80;
            this.nudSprVisionTimeout.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 10;
            this.nudSprVisionTimeout.Minimum = new decimal(numArray2);
            this.nudSprVisionTimeout.Name = "nudSprVisionTimeout";
            this.nudSprVisionTimeout.Size = new Size(50, 20);
            this.nudSprVisionTimeout.TabIndex = 20;
            int[] numArray3 = new int[4];
            numArray3[0] = 10;
            this.nudSprVisionTimeout.Value = new decimal(numArray3);
            this.nudSprVisionTimeout.ValueChanged += new EventHandler(this.supervisionTimeout_Changed);
            this.nudSlaveLatency.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.nudSlaveLatency.Location = new Point(0xeb, 0x4b);
            this.nudSlaveLatency.Margin = new Padding(2, 3, 2, 3);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x1f3;
            this.nudSlaveLatency.Maximum = new decimal(numArray4);
            this.nudSlaveLatency.Name = "nudSlaveLatency";
            this.nudSlaveLatency.Size = new Size(50, 20);
            this.nudSlaveLatency.TabIndex = 0x13;
            this.nudMaxConnInt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.nudMaxConnInt.Location = new Point(0xeb, 0x2e);
            this.nudMaxConnInt.Margin = new Padding(2, 3, 2, 3);
            int[] numArray5 = new int[4];
            numArray5[0] = 0xc80;
            this.nudMaxConnInt.Maximum = new decimal(numArray5);
            int[] numArray6 = new int[4];
            numArray6[0] = 6;
            this.nudMaxConnInt.Minimum = new decimal(numArray6);
            this.nudMaxConnInt.Name = "nudMaxConnInt";
            this.nudMaxConnInt.Size = new Size(50, 20);
            this.nudMaxConnInt.TabIndex = 0x12;
            int[] numArray7 = new int[4];
            numArray7[0] = 6;
            this.nudMaxConnInt.Value = new decimal(numArray7);
            this.nudMaxConnInt.ValueChanged += new EventHandler(this.maxCI_Changed);
            this.nudMinConnInt.BorderStyle = BorderStyle.FixedSingle;
            this.nudMinConnInt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.nudMinConnInt.Location = new Point(0xeb, 20);
            this.nudMinConnInt.Margin = new Padding(2, 3, 2, 3);
            int[] numArray8 = new int[4];
            numArray8[0] = 0xc80;
            this.nudMinConnInt.Maximum = new decimal(numArray8);
            int[] numArray9 = new int[4];
            numArray9[0] = 6;
            this.nudMinConnInt.Minimum = new decimal(numArray9);
            this.nudMinConnInt.Name = "nudMinConnInt";
            this.nudMinConnInt.Size = new Size(50, 20);
            this.nudMinConnInt.TabIndex = 0x11;
            int[] numArray10 = new int[4];
            numArray10[0] = 0xc80;
            this.nudMinConnInt.Value = new decimal(numArray10);
            this.nudMinConnInt.ValueChanged += new EventHandler(this.minCI_Changed);
            this.lblSupervisionTimeout.AutoSize = true;
            this.lblSupervisionTimeout.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSupervisionTimeout.Location = new Point(0x121, 0x6a);
            this.lblSupervisionTimeout.Margin = new Padding(2, 0, 2, 0);
            this.lblSupervisionTimeout.Name = "lblSupervisionTimeout";
            this.lblSupervisionTimeout.Size = new Size(0x20, 13);
            this.lblSupervisionTimeout.TabIndex = 0x10;
            this.lblSupervisionTimeout.Text = "(0ms)";
            this.lblMaxConnInt.AutoSize = true;
            this.lblMaxConnInt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblMaxConnInt.Location = new Point(0x121, 0x31);
            this.lblMaxConnInt.Margin = new Padding(2, 0, 2, 0);
            this.lblMaxConnInt.Name = "lblMaxConnInt";
            this.lblMaxConnInt.Size = new Size(0x20, 13);
            this.lblMaxConnInt.TabIndex = 14;
            this.lblMaxConnInt.Text = "(0ms)";
            this.lblMinConnInt.AutoSize = true;
            this.lblMinConnInt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblMinConnInt.Location = new Point(0x121, 0x15);
            this.lblMinConnInt.Margin = new Padding(2, 0, 2, 0);
            this.lblMinConnInt.Name = "lblMinConnInt";
            this.lblMinConnInt.Size = new Size(0x20, 13);
            this.lblMinConnInt.TabIndex = 13;
            this.lblMinConnInt.Text = "(0ms)";
            this.lblSuperTimeout.AutoSize = true;
            this.lblSuperTimeout.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSuperTimeout.Location = new Point(70, 0x66);
            this.lblSuperTimeout.Margin = new Padding(2, 0, 2, 0);
            this.lblSuperTimeout.Name = "lblSuperTimeout";
            this.lblSuperTimeout.Size = new Size(0x9a, 13);
            this.lblSuperTimeout.TabIndex = 5;
            this.lblSuperTimeout.Text = "Supervision Timeout (10-3200):";
            this.lblSlaveLat.AutoSize = true;
            this.lblSlaveLat.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSlaveLat.Location = new Point(110, 0x4c);
            this.lblSlaveLat.Margin = new Padding(2, 0, 2, 0);
            this.lblSlaveLat.Name = "lblSlaveLat";
            this.lblSlaveLat.Size = new Size(0x72, 13);
            this.lblSlaveLat.TabIndex = 4;
            this.lblSlaveLat.Text = "Slave Latency (0-499):";
            this.lblMaxConn.AutoSize = true;
            this.lblMaxConn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblMaxConn.Location = new Point(0x38, 0x2f);
            this.lblMaxConn.Margin = new Padding(2, 0, 2, 0);
            this.lblMaxConn.Name = "lblMaxConn";
            this.lblMaxConn.Size = new Size(0xa7, 13);
            this.lblMaxConn.TabIndex = 3;
            this.lblMaxConn.Text = "Max Connection Interval (6-3200):";
            this.lblMinConn.AutoSize = true;
            this.lblMinConn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblMinConn.Location = new Point(60, 0x15);
            this.lblMinConn.Margin = new Padding(2, 0, 2, 0);
            this.lblMinConn.Name = "lblMinConn";
            this.lblMinConn.Size = new Size(0xa4, 13);
            this.lblMinConn.TabIndex = 2;
            this.lblMinConn.Text = "Min Connection Interval (6-3200):";
            this.gbDiscovery.Controls.Add(this.btnScanCancel);
            this.gbDiscovery.Controls.Add(this.ckBoxWhiteList);
            this.gbDiscovery.Controls.Add(this.ckBoxActiveScan);
            this.gbDiscovery.Controls.Add(this.lblMode);
            this.gbDiscovery.Controls.Add(this.cbScanMode);
            this.gbDiscovery.Controls.Add(this.lblDeviceFound);
            this.gbDiscovery.Controls.Add(this.lblDevsFound);
            this.gbDiscovery.Controls.Add(this.btnScan);
            this.gbDiscovery.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gbDiscovery.Location = new Point(4, 3);
            this.gbDiscovery.Margin = new Padding(2, 3, 2, 3);
            this.gbDiscovery.Name = "gbDiscovery";
            this.gbDiscovery.Padding = new Padding(2, 3, 2, 3);
            this.gbDiscovery.Size = new Size(0x179, 0x6d);
            this.gbDiscovery.TabIndex = 2;
            this.gbDiscovery.TabStop = false;
            this.gbDiscovery.Text = "Discovery";
            this.btnScanCancel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnScanCancel.Location = new Point(0xec, 0x43);
            this.btnScanCancel.Margin = new Padding(2, 3, 2, 3);
            this.btnScanCancel.Name = "btnScanCancel";
            this.btnScanCancel.Size = new Size(0x7e, 0x1c);
            this.btnScanCancel.TabIndex = 7;
            this.btnScanCancel.Text = "Cancel";
            this.btnScanCancel.UseVisualStyleBackColor = true;
            this.btnScanCancel.Click += new EventHandler(this.btnScanCancel_Click);
            this.ckBoxWhiteList.AutoSize = true;
            this.ckBoxWhiteList.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ckBoxWhiteList.Location = new Point(0x2a, 0x2c);
            this.ckBoxWhiteList.Margin = new Padding(2, 3, 2, 3);
            this.ckBoxWhiteList.Name = "ckBoxWhiteList";
            this.ckBoxWhiteList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckBoxWhiteList.Size = new Size(70, 0x11);
            this.ckBoxWhiteList.TabIndex = 6;
            this.ckBoxWhiteList.Text = "WhiteList";
            this.ckBoxWhiteList.UseVisualStyleBackColor = true;
            this.ckBoxActiveScan.AutoSize = true;
            this.ckBoxActiveScan.Checked = true;
            this.ckBoxActiveScan.CheckState = CheckState.Checked;
            this.ckBoxActiveScan.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ckBoxActiveScan.Location = new Point(0x2a, 0x15);
            this.ckBoxActiveScan.Margin = new Padding(2, 3, 2, 3);
            this.ckBoxActiveScan.Name = "ckBoxActiveScan";
            this.ckBoxActiveScan.Size = new Size(0x54, 0x11);
            this.ckBoxActiveScan.TabIndex = 5;
            this.ckBoxActiveScan.Text = "Active Scan";
            this.ckBoxActiveScan.UseVisualStyleBackColor = true;
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblMode.Location = new Point(180, 0x17);
            this.lblMode.Margin = new Padding(2, 0, 2, 0);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new Size(0x25, 13);
            this.lblMode.TabIndex = 4;
            this.lblMode.Text = "Mode:";
            this.cbScanMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbScanMode.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cbScanMode.FormattingEnabled = true;
            this.cbScanMode.Items.AddRange(new object[] { "0x00 (NonDiscoverable)", "0x01 (General)", "0x02 (Limited)", "0x03 (All)" });
            this.cbScanMode.Location = new Point(0xdb, 20);
            this.cbScanMode.Margin = new Padding(2, 3, 2, 3);
            this.cbScanMode.Name = "cbScanMode";
            this.cbScanMode.Size = new Size(0x8e, 0x15);
            this.cbScanMode.TabIndex = 3;
            this.lblDeviceFound.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblDeviceFound.Location = new Point(0xdd, 0x2e);
            this.lblDeviceFound.Margin = new Padding(2, 0, 2, 0);
            this.lblDeviceFound.Name = "lblDeviceFound";
            this.lblDeviceFound.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDeviceFound.Size = new Size(0x34, 13);
            this.lblDeviceFound.TabIndex = 2;
            this.lblDeviceFound.Text = "0";
            this.lblDeviceFound.TextAlign = ContentAlignment.MiddleLeft;
            this.lblDevsFound.AutoSize = true;
            this.lblDevsFound.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblDevsFound.Location = new Point(0x89, 0x2e);
            this.lblDevsFound.Margin = new Padding(2, 0, 2, 0);
            this.lblDevsFound.Name = "lblDevsFound";
            this.lblDevsFound.Size = new Size(0x52, 13);
            this.lblDevsFound.TabIndex = 1;
            this.lblDevsFound.Text = "Devices Found:";
            this.btnScan.BackColor = SystemColors.Control;
            this.btnScan.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnScan.Location = new Point(15, 0x43);
            this.btnScan.Margin = new Padding(2, 3, 2, 3);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new Size(0x7e, 0x1c);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new EventHandler(this.btnScan_Click);
            this.tpReadWrite.BackColor = Color.Transparent;
            this.tpReadWrite.Controls.Add(this.gbCharWrite);
            this.tpReadWrite.Controls.Add(this.gbCharRead);
            this.tpReadWrite.Location = new Point(4, 0x16);
            this.tpReadWrite.Margin = new Padding(2, 3, 2, 3);
            this.tpReadWrite.Name = "tpReadWrite";
            this.tpReadWrite.Size = new Size(0x183, 0x1fc);
            this.tpReadWrite.TabIndex = 5;
            this.tpReadWrite.Text = "Read / Write";
            this.tpReadWrite.UseVisualStyleBackColor = true;
            this.gbCharWrite.Controls.Add(this.gbWriteArea);
            this.gbCharWrite.Controls.Add(this.tbWriteConnHandle);
            this.gbCharWrite.Controls.Add(this.lblWriteConnHnd);
            this.gbCharWrite.Controls.Add(this.lblWriteHandle);
            this.gbCharWrite.Controls.Add(this.tbWriteAttrHandle);
            this.gbCharWrite.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gbCharWrite.Location = new Point(4, 0x120);
            this.gbCharWrite.Margin = new Padding(2, 3, 2, 3);
            this.gbCharWrite.Name = "gbCharWrite";
            this.gbCharWrite.Padding = new Padding(2, 3, 2, 3);
            this.gbCharWrite.Size = new Size(0x179, 210);
            this.gbCharWrite.TabIndex = 1;
            this.gbCharWrite.TabStop = false;
            this.gbCharWrite.Text = "Characteristic Write";
            this.gbWriteArea.Controls.Add(this.lblWriteStatus);
            this.gbWriteArea.Controls.Add(this.lblWriteValue);
            this.gbWriteArea.Controls.Add(this.btnWriteGATTValue);
            this.gbWriteArea.Controls.Add(this.tbWriteStatus);
            this.gbWriteArea.Controls.Add(this.rbASCIIWrite);
            this.gbWriteArea.Controls.Add(this.rbHexWrite);
            this.gbWriteArea.Controls.Add(this.rbDecimalWrite);
            this.gbWriteArea.Controls.Add(this.tbWriteValue);
            this.gbWriteArea.Location = new Point(14, 0x53);
            this.gbWriteArea.Margin = new Padding(2, 3, 2, 3);
            this.gbWriteArea.Name = "gbWriteArea";
            this.gbWriteArea.Padding = new Padding(2, 3, 2, 3);
            this.gbWriteArea.Size = new Size(0x15a, 0x68);
            this.gbWriteArea.TabIndex = 11;
            this.gbWriteArea.TabStop = false;
            this.lblWriteStatus.AutoSize = true;
            this.lblWriteStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblWriteStatus.Location = new Point(6, 0x3a);
            this.lblWriteStatus.Margin = new Padding(2, 0, 2, 0);
            this.lblWriteStatus.Name = "lblWriteStatus";
            this.lblWriteStatus.Size = new Size(0x25, 13);
            this.lblWriteStatus.TabIndex = 0x13;
            this.lblWriteStatus.Text = "Status";
            this.lblWriteValue.AutoSize = true;
            this.lblWriteValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblWriteValue.Location = new Point(6, 0x10);
            this.lblWriteValue.Margin = new Padding(2, 0, 2, 0);
            this.lblWriteValue.Name = "lblWriteValue";
            this.lblWriteValue.Size = new Size(0x22, 13);
            this.lblWriteValue.TabIndex = 14;
            this.lblWriteValue.Text = "Value";
            this.btnWriteGATTValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnWriteGATTValue.Location = new Point(0x109, 0x3a);
            this.btnWriteGATTValue.Margin = new Padding(2, 3, 2, 3);
            this.btnWriteGATTValue.Name = "btnWriteGATTValue";
            this.btnWriteGATTValue.Size = new Size(0x4a, 0x24);
            this.btnWriteGATTValue.TabIndex = 2;
            this.btnWriteGATTValue.Text = "Write";
            this.btnWriteGATTValue.UseVisualStyleBackColor = true;
            this.btnWriteGATTValue.Click += new EventHandler(this.btnGATTWriteValue_Click);
            this.tbWriteStatus.Location = new Point(8, 0x4b);
            this.tbWriteStatus.Margin = new Padding(2, 3, 2, 3);
            this.tbWriteStatus.Name = "tbWriteStatus";
            this.tbWriteStatus.ReadOnly = true;
            this.tbWriteStatus.Size = new Size(0xf6, 20);
            this.tbWriteStatus.TabIndex = 0x12;
            this.tbWriteStatus.TextAlign = HorizontalAlignment.Center;
            this.rbASCIIWrite.AutoSize = true;
            this.rbASCIIWrite.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rbASCIIWrite.Location = new Point(70, 11);
            this.rbASCIIWrite.Margin = new Padding(2, 3, 2, 3);
            this.rbASCIIWrite.Name = "rbASCIIWrite";
            this.rbASCIIWrite.Size = new Size(0x34, 0x11);
            this.rbASCIIWrite.TabIndex = 13;
            this.rbASCIIWrite.Text = "ASCII";
            this.rbASCIIWrite.UseVisualStyleBackColor = true;
            this.rbHexWrite.AutoSize = true;
            this.rbHexWrite.Checked = true;
            this.rbHexWrite.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rbHexWrite.Location = new Point(0x110, 11);
            this.rbHexWrite.Margin = new Padding(2, 3, 2, 3);
            this.rbHexWrite.Name = "rbHexWrite";
            this.rbHexWrite.Size = new Size(0x2c, 0x11);
            this.rbHexWrite.TabIndex = 12;
            this.rbHexWrite.TabStop = true;
            this.rbHexWrite.Text = "Hex";
            this.rbHexWrite.UseVisualStyleBackColor = true;
            this.rbDecimalWrite.AutoSize = true;
            this.rbDecimalWrite.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rbDecimalWrite.Location = new Point(0xa4, 11);
            this.rbDecimalWrite.Margin = new Padding(2, 3, 2, 3);
            this.rbDecimalWrite.Name = "rbDecimalWrite";
            this.rbDecimalWrite.Size = new Size(0x3f, 0x11);
            this.rbDecimalWrite.TabIndex = 11;
            this.rbDecimalWrite.Text = "Decimal";
            this.rbDecimalWrite.UseVisualStyleBackColor = true;
            this.tbWriteValue.Location = new Point(8, 0x20);
            this.tbWriteValue.Margin = new Padding(2, 3, 2, 3);
            this.tbWriteValue.Name = "tbWriteValue";
            this.tbWriteValue.Size = new Size(330, 20);
            this.tbWriteValue.TabIndex = 10;
            this.tbWriteValue.TextAlign = HorizontalAlignment.Center;
            this.tbWriteConnHandle.Location = new Point(0x128, 0x2a);
            this.tbWriteConnHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbWriteConnHandle.MaxLength = 6;
            this.tbWriteConnHandle.Name = "tbWriteConnHandle";
            this.tbWriteConnHandle.Size = new Size(60, 20);
            this.tbWriteConnHandle.TabIndex = 9;
            this.tbWriteConnHandle.TextAlign = HorizontalAlignment.Center;
            this.lblWriteConnHnd.AutoSize = true;
            this.lblWriteConnHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblWriteConnHnd.Location = new Point(0x112, 0x1a);
            this.lblWriteConnHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblWriteConnHnd.Name = "lblWriteConnHnd";
            this.lblWriteConnHnd.Size = new Size(0x62, 13);
            this.lblWriteConnHnd.TabIndex = 8;
            this.lblWriteConnHnd.Text = "Connection Handle";
            this.lblWriteHandle.AutoSize = true;
            this.lblWriteHandle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblWriteHandle.Location = new Point(20, 0x1a);
            this.lblWriteHandle.Margin = new Padding(2, 0, 2, 0);
            this.lblWriteHandle.Name = "lblWriteHandle";
            this.lblWriteHandle.Size = new Size(0x8a, 13);
            this.lblWriteHandle.TabIndex = 7;
            this.lblWriteHandle.Text = "Characteristic Value Handle";
            this.tbWriteAttrHandle.Location = new Point(0x17, 0x2a);
            this.tbWriteAttrHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbWriteAttrHandle.Name = "tbWriteAttrHandle";
            this.tbWriteAttrHandle.Size = new Size(0x100, 20);
            this.tbWriteAttrHandle.TabIndex = 6;
            this.tbWriteAttrHandle.Text = "0x0001";
            this.tbWriteAttrHandle.TextAlign = HorizontalAlignment.Center;
            this.gbCharRead.Controls.Add(this.lblReadSubProc);
            this.gbCharRead.Controls.Add(this.cbReadType);
            this.gbCharRead.Controls.Add(this.lblReadStartHnd);
            this.gbCharRead.Controls.Add(this.lblReadEndHnd);
            this.gbCharRead.Controls.Add(this.lblReadCharUuid);
            this.gbCharRead.Controls.Add(this.tbReadUUID);
            this.gbCharRead.Controls.Add(this.tbReadStartHandle);
            this.gbCharRead.Controls.Add(this.tbReadEndHandle);
            this.gbCharRead.Controls.Add(this.gbReadArea);
            this.gbCharRead.Controls.Add(this.tbReadConnHandle);
            this.gbCharRead.Controls.Add(this.lblReadConnHnd);
            this.gbCharRead.Controls.Add(this.lblReadValueHnd);
            this.gbCharRead.Controls.Add(this.tbReadAttrHandle);
            this.gbCharRead.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gbCharRead.Location = new Point(4, 6);
            this.gbCharRead.Margin = new Padding(2, 3, 2, 3);
            this.gbCharRead.Name = "gbCharRead";
            this.gbCharRead.Padding = new Padding(2, 3, 2, 3);
            this.gbCharRead.Size = new Size(0x179, 0x113);
            this.gbCharRead.TabIndex = 0;
            this.gbCharRead.TabStop = false;
            this.gbCharRead.Text = "Characteristic Read";
            this.lblReadSubProc.AutoSize = true;
            this.lblReadSubProc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadSubProc.Location = new Point(20, 0x17);
            this.lblReadSubProc.Margin = new Padding(2, 0, 2, 0);
            this.lblReadSubProc.Name = "lblReadSubProc";
            this.lblReadSubProc.Size = new Size(0x4e, 13);
            this.lblReadSubProc.TabIndex = 0x17;
            this.lblReadSubProc.Text = "Sub-Procedure";
            this.cbReadType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbReadType.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cbReadType.FormattingEnabled = true;
            this.cbReadType.Items.AddRange(new object[] { "Read Characteristic Value / Descriptor", "Read Using Characteristic UUID", "Read Multiple Characteristic Values", "Discover Characteristic by UUID" });
            this.cbReadType.Location = new Point(0x17, 0x25);
            this.cbReadType.Margin = new Padding(2, 3, 2, 3);
            this.cbReadType.Name = "cbReadType";
            this.cbReadType.Size = new Size(0x100, 0x15);
            this.cbReadType.TabIndex = 15;
            this.cbReadType.SelectedIndexChanged += new EventHandler(this.readType_Changed);
            this.lblReadStartHnd.AutoSize = true;
            this.lblReadStartHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadStartHnd.Location = new Point(0x124, 0x3e);
            this.lblReadStartHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblReadStartHnd.Name = "lblReadStartHnd";
            this.lblReadStartHnd.Size = new Size(0x42, 13);
            this.lblReadStartHnd.TabIndex = 0x16;
            this.lblReadStartHnd.Text = "Start Handle";
            this.lblReadEndHnd.AutoSize = true;
            this.lblReadEndHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadEndHnd.Location = new Point(0x125, 0x62);
            this.lblReadEndHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblReadEndHnd.Name = "lblReadEndHnd";
            this.lblReadEndHnd.Size = new Size(0x3f, 13);
            this.lblReadEndHnd.TabIndex = 0x15;
            this.lblReadEndHnd.Text = "End Handle";
            this.lblReadCharUuid.AutoSize = true;
            this.lblReadCharUuid.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadCharUuid.Location = new Point(20, 0x62);
            this.lblReadCharUuid.Margin = new Padding(2, 0, 2, 0);
            this.lblReadCharUuid.Name = "lblReadCharUuid";
            this.lblReadCharUuid.Size = new Size(0x65, 13);
            this.lblReadCharUuid.TabIndex = 0x13;
            this.lblReadCharUuid.Text = "Characteristic UUID";
            this.tbReadUUID.Location = new Point(0x17, 0x72);
            this.tbReadUUID.Margin = new Padding(2, 3, 2, 3);
            this.tbReadUUID.MaxLength = 0x2f;
            this.tbReadUUID.Name = "tbReadUUID";
            this.tbReadUUID.Size = new Size(0x100, 20);
            this.tbReadUUID.TabIndex = 0x12;
            this.tbReadUUID.Text = "00:2A";
            this.tbReadUUID.TextAlign = HorizontalAlignment.Center;
            this.tbReadStartHandle.Location = new Point(0x128, 0x4b);
            this.tbReadStartHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbReadStartHandle.MaxLength = 6;
            this.tbReadStartHandle.Name = "tbReadStartHandle";
            this.tbReadStartHandle.Size = new Size(60, 20);
            this.tbReadStartHandle.TabIndex = 0x11;
            this.tbReadStartHandle.Text = "0x0001";
            this.tbReadStartHandle.TextAlign = HorizontalAlignment.Center;
            this.tbReadEndHandle.Location = new Point(0x128, 0x72);
            this.tbReadEndHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbReadEndHandle.MaxLength = 6;
            this.tbReadEndHandle.Name = "tbReadEndHandle";
            this.tbReadEndHandle.Size = new Size(60, 20);
            this.tbReadEndHandle.TabIndex = 0x10;
            this.tbReadEndHandle.Text = "0xFFFF";
            this.tbReadEndHandle.TextAlign = HorizontalAlignment.Center;
            this.gbReadArea.Controls.Add(this.lbReadValue);
            this.gbReadArea.Controls.Add(this.lblReadStatus);
            this.gbReadArea.Controls.Add(this.rbASCIIRead);
            this.gbReadArea.Controls.Add(this.tbReadStatus);
            this.gbReadArea.Controls.Add(this.rbHexRead);
            this.gbReadArea.Controls.Add(this.btnReadGATTValue);
            this.gbReadArea.Controls.Add(this.rbDecimalRead);
            this.gbReadArea.Controls.Add(this.tbReadValue);
            this.gbReadArea.Location = new Point(14, 0x9a);
            this.gbReadArea.Margin = new Padding(2, 3, 2, 3);
            this.gbReadArea.Name = "gbReadArea";
            this.gbReadArea.Padding = new Padding(2, 3, 2, 3);
            this.gbReadArea.Size = new Size(0x15a, 0x62);
            this.gbReadArea.TabIndex = 15;
            this.gbReadArea.TabStop = false;
            this.lbReadValue.AutoSize = true;
            this.lbReadValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbReadValue.Location = new Point(5, 11);
            this.lbReadValue.Margin = new Padding(2, 0, 2, 0);
            this.lbReadValue.Name = "lbReadValue";
            this.lbReadValue.Size = new Size(0x22, 13);
            this.lbReadValue.TabIndex = 14;
            this.lbReadValue.Text = "Value";
            this.lblReadStatus.AutoSize = true;
            this.lblReadStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadStatus.Location = new Point(6, 0x36);
            this.lblReadStatus.Margin = new Padding(2, 0, 2, 0);
            this.lblReadStatus.Name = "lblReadStatus";
            this.lblReadStatus.Size = new Size(0x25, 13);
            this.lblReadStatus.TabIndex = 0x11;
            this.lblReadStatus.Text = "Status";
            this.rbASCIIRead.AutoSize = true;
            this.rbASCIIRead.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rbASCIIRead.Location = new Point(70, 10);
            this.rbASCIIRead.Margin = new Padding(2, 3, 2, 3);
            this.rbASCIIRead.Name = "rbASCIIRead";
            this.rbASCIIRead.Size = new Size(0x34, 0x11);
            this.rbASCIIRead.TabIndex = 13;
            this.rbASCIIRead.Text = "ASCII";
            this.rbASCIIRead.UseVisualStyleBackColor = true;
            this.rbASCIIRead.Click += new EventHandler(this.readFormat_Click);
            this.tbReadStatus.Location = new Point(8, 0x44);
            this.tbReadStatus.Margin = new Padding(2, 3, 2, 3);
            this.tbReadStatus.Name = "tbReadStatus";
            this.tbReadStatus.ReadOnly = true;
            this.tbReadStatus.Size = new Size(250, 20);
            this.tbReadStatus.TabIndex = 0x10;
            this.tbReadStatus.TextAlign = HorizontalAlignment.Center;
            this.rbHexRead.AutoSize = true;
            this.rbHexRead.Checked = true;
            this.rbHexRead.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rbHexRead.Location = new Point(0x110, 10);
            this.rbHexRead.Margin = new Padding(2, 3, 2, 3);
            this.rbHexRead.Name = "rbHexRead";
            this.rbHexRead.Size = new Size(0x2c, 0x11);
            this.rbHexRead.TabIndex = 12;
            this.rbHexRead.TabStop = true;
            this.rbHexRead.Text = "Hex";
            this.rbHexRead.UseVisualStyleBackColor = true;
            this.rbHexRead.Click += new EventHandler(this.readFormat_Click);
            this.btnReadGATTValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnReadGATTValue.Location = new Point(0x109, 0x36);
            this.btnReadGATTValue.Margin = new Padding(2, 3, 2, 3);
            this.btnReadGATTValue.Name = "btnReadGATTValue";
            this.btnReadGATTValue.Size = new Size(0x4a, 0x24);
            this.btnReadGATTValue.TabIndex = 1;
            this.btnReadGATTValue.Text = "Read";
            this.btnReadGATTValue.UseVisualStyleBackColor = true;
            this.btnReadGATTValue.Click += new EventHandler(this.btnGATTReadValue_Click);
            this.rbDecimalRead.AutoSize = true;
            this.rbDecimalRead.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rbDecimalRead.Location = new Point(0xa4, 10);
            this.rbDecimalRead.Margin = new Padding(2, 3, 2, 3);
            this.rbDecimalRead.Name = "rbDecimalRead";
            this.rbDecimalRead.Size = new Size(0x3f, 0x11);
            this.rbDecimalRead.TabIndex = 11;
            this.rbDecimalRead.Text = "Decimal";
            this.rbDecimalRead.UseVisualStyleBackColor = true;
            this.rbDecimalRead.Click += new EventHandler(this.readFormat_Click);
            this.tbReadValue.BackColor = SystemColors.Control;
            this.tbReadValue.Location = new Point(8, 0x1c);
            this.tbReadValue.Margin = new Padding(2, 3, 2, 3);
            this.tbReadValue.Name = "tbReadValue";
            this.tbReadValue.ReadOnly = true;
            this.tbReadValue.Size = new Size(330, 20);
            this.tbReadValue.TabIndex = 10;
            this.tbReadValue.TextAlign = HorizontalAlignment.Center;
            this.tbReadConnHandle.Enabled = false;
            this.tbReadConnHandle.Location = new Point(0x128, 0x25);
            this.tbReadConnHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbReadConnHandle.MaxLength = 6;
            this.tbReadConnHandle.Name = "tbReadConnHandle";
            this.tbReadConnHandle.Size = new Size(60, 20);
            this.tbReadConnHandle.TabIndex = 5;
            this.tbReadConnHandle.TextAlign = HorizontalAlignment.Center;
            this.lblReadConnHnd.AutoSize = true;
            this.lblReadConnHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadConnHnd.Location = new Point(0x115, 0x17);
            this.lblReadConnHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblReadConnHnd.Name = "lblReadConnHnd";
            this.lblReadConnHnd.Size = new Size(0x62, 13);
            this.lblReadConnHnd.TabIndex = 4;
            this.lblReadConnHnd.Text = "Connection Handle";
            this.lblReadValueHnd.AutoSize = true;
            this.lblReadValueHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblReadValueHnd.Location = new Point(20, 0x3e);
            this.lblReadValueHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblReadValueHnd.Name = "lblReadValueHnd";
            this.lblReadValueHnd.Size = new Size(0x8a, 13);
            this.lblReadValueHnd.TabIndex = 3;
            this.lblReadValueHnd.Text = "Characteristic Value Handle";
            this.tbReadAttrHandle.Location = new Point(0x17, 0x4b);
            this.tbReadAttrHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbReadAttrHandle.Name = "tbReadAttrHandle";
            this.tbReadAttrHandle.Size = new Size(0x100, 20);
            this.tbReadAttrHandle.TabIndex = 2;
            this.tbReadAttrHandle.Text = "0x0001";
            this.tbReadAttrHandle.TextAlign = HorizontalAlignment.Center;
            this.tpPairingBonding.Controls.Add(this.gbLongTermKeyData);
            this.tpPairingBonding.Controls.Add(this.gbEncryptLTKey);
            this.tpPairingBonding.Controls.Add(this.gbPasskeyInput);
            this.tpPairingBonding.Controls.Add(this.gbInitParing);
            this.tpPairingBonding.Controls.Add(this.labelPairingStatus);
            this.tpPairingBonding.Controls.Add(this.tbPairingStatus);
            this.tpPairingBonding.Location = new Point(4, 0x16);
            this.tpPairingBonding.Margin = new Padding(2, 3, 2, 3);
            this.tpPairingBonding.Name = "tpPairingBonding";
            this.tpPairingBonding.Padding = new Padding(2, 3, 2, 3);
            this.tpPairingBonding.Size = new Size(0x183, 0x1fc);
            this.tpPairingBonding.TabIndex = 6;
            this.tpPairingBonding.Text = "Pairing / Bonding";
            this.tpPairingBonding.UseVisualStyleBackColor = true;
            this.gbLongTermKeyData.Controls.Add(this.btnSaveLongTermKey);
            this.gbLongTermKeyData.Controls.Add(this.tbLongTermKeyData);
            this.gbLongTermKeyData.Location = new Point(6, 0x161);
            this.gbLongTermKeyData.Margin = new Padding(2, 3, 2, 3);
            this.gbLongTermKeyData.Name = "gbLongTermKeyData";
            this.gbLongTermKeyData.Padding = new Padding(2, 3, 2, 3);
            this.gbLongTermKeyData.Size = new Size(0x176, 0x95);
            this.gbLongTermKeyData.TabIndex = 4;
            this.gbLongTermKeyData.TabStop = false;
            this.gbLongTermKeyData.Text = "Long-Term Key (LTK) Data";
            this.btnSaveLongTermKey.Location = new Point(13, 120);
            this.btnSaveLongTermKey.Margin = new Padding(2, 3, 2, 3);
            this.btnSaveLongTermKey.Name = "btnSaveLongTermKey";
            this.btnSaveLongTermKey.Size = new Size(0xcb, 0x17);
            this.btnSaveLongTermKey.TabIndex = 1;
            this.btnSaveLongTermKey.Text = "Save Long-Term Key Data To File";
            this.btnSaveLongTermKey.UseVisualStyleBackColor = true;
            this.btnSaveLongTermKey.Click += new EventHandler(this.btnSaveLongTermKey_Click);
            this.tbLongTermKeyData.Location = new Point(13, 0x1a);
            this.tbLongTermKeyData.Margin = new Padding(2, 3, 2, 3);
            this.tbLongTermKeyData.Multiline = true;
            this.tbLongTermKeyData.Name = "tbLongTermKeyData";
            this.tbLongTermKeyData.ReadOnly = true;
            this.tbLongTermKeyData.Size = new Size(350, 0x58);
            this.tbLongTermKeyData.TabIndex = 9;
            this.gbEncryptLTKey.Controls.Add(this.tbBondConnHandle);
            this.gbEncryptLTKey.Controls.Add(this.lblLtkConnHnd);
            this.gbEncryptLTKey.Controls.Add(this.btnEncryptLink);
            this.gbEncryptLTKey.Controls.Add(this.btnLoadLongTermKey);
            this.gbEncryptLTKey.Controls.Add(this.tbLTKRandom);
            this.gbEncryptLTKey.Controls.Add(this.tbLTKDiversifier);
            this.gbEncryptLTKey.Controls.Add(this.tbLongTermKey);
            this.gbEncryptLTKey.Controls.Add(this.lblLtkRandom);
            this.gbEncryptLTKey.Controls.Add(this.lblLtkDiv);
            this.gbEncryptLTKey.Controls.Add(this.lblLtk);
            this.gbEncryptLTKey.Controls.Add(this.rbAuthBondFalse);
            this.gbEncryptLTKey.Controls.Add(this.rbAuthBondTrue);
            this.gbEncryptLTKey.Controls.Add(this.lblAuthBond);
            this.gbEncryptLTKey.Location = new Point(6, 0xab);
            this.gbEncryptLTKey.Margin = new Padding(2, 3, 2, 3);
            this.gbEncryptLTKey.Name = "gbEncryptLTKey";
            this.gbEncryptLTKey.Padding = new Padding(2, 3, 2, 3);
            this.gbEncryptLTKey.Size = new Size(0x176, 0xb0);
            this.gbEncryptLTKey.TabIndex = 3;
            this.gbEncryptLTKey.TabStop = false;
            this.gbEncryptLTKey.Text = "Encrypt Using Long-Term Key";
            this.tbBondConnHandle.Enabled = false;
            this.tbBondConnHandle.Location = new Point(0x74, 0x12);
            this.tbBondConnHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbBondConnHandle.MaxLength = 6;
            this.tbBondConnHandle.Name = "tbBondConnHandle";
            this.tbBondConnHandle.Size = new Size(60, 20);
            this.tbBondConnHandle.TabIndex = 12;
            this.tbBondConnHandle.Text = "0x0000";
            this.tbBondConnHandle.TextAlign = HorizontalAlignment.Center;
            this.lblLtkConnHnd.AutoSize = true;
            this.lblLtkConnHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblLtkConnHnd.Location = new Point(10, 0x17);
            this.lblLtkConnHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblLtkConnHnd.Name = "lblLtkConnHnd";
            this.lblLtkConnHnd.Size = new Size(0x65, 13);
            this.lblLtkConnHnd.TabIndex = 11;
            this.lblLtkConnHnd.Text = "Connection Handle:";
            this.btnEncryptLink.Location = new Point(0xfb, 0x8f);
            this.btnEncryptLink.Margin = new Padding(2, 3, 2, 3);
            this.btnEncryptLink.Name = "btnEncryptLink";
            this.btnEncryptLink.Size = new Size(0x66, 0x17);
            this.btnEncryptLink.TabIndex = 10;
            this.btnEncryptLink.Text = "Encrypt Link";
            this.btnEncryptLink.UseVisualStyleBackColor = true;
            this.btnEncryptLink.Click += new EventHandler(this.btnInitiateBond_Click);
            this.btnLoadLongTermKey.Location = new Point(13, 0x8f);
            this.btnLoadLongTermKey.Margin = new Padding(2, 3, 2, 3);
            this.btnLoadLongTermKey.Name = "btnLoadLongTermKey";
            this.btnLoadLongTermKey.Size = new Size(0xcb, 0x17);
            this.btnLoadLongTermKey.TabIndex = 9;
            this.btnLoadLongTermKey.Text = "Load Long-Term Key Data From File";
            this.btnLoadLongTermKey.UseVisualStyleBackColor = true;
            this.btnLoadLongTermKey.Click += new EventHandler(this.btnLoadLongTermKey_Click);
            this.tbLTKRandom.Location = new Point(0x92, 0x72);
            this.tbLTKRandom.Margin = new Padding(2, 3, 2, 3);
            this.tbLTKRandom.MaxLength = 0x17;
            this.tbLTKRandom.Name = "tbLTKRandom";
            this.tbLTKRandom.Size = new Size(0xa5, 20);
            this.tbLTKRandom.TabIndex = 8;
            this.tbLTKRandom.KeyPress += new KeyPressEventHandler(this.tbLTKRandom_KeyPress);
            this.tbLTKDiversifier.Location = new Point(150, 0x58);
            this.tbLTKDiversifier.Margin = new Padding(2, 3, 2, 3);
            this.tbLTKDiversifier.MaxLength = 4;
            this.tbLTKDiversifier.Name = "tbLTKDiversifier";
            this.tbLTKDiversifier.Size = new Size(0x34, 20);
            this.tbLTKDiversifier.TabIndex = 7;
            this.tbLTKDiversifier.KeyPress += new KeyPressEventHandler(this.tbLTKDiversifier_KeyPress);
            this.tbLongTermKey.Location = new Point(13, 0x3e);
            this.tbLongTermKey.Margin = new Padding(2, 3, 2, 3);
            this.tbLongTermKey.MaxLength = 0x2f;
            this.tbLongTermKey.Name = "tbLongTermKey";
            this.tbLongTermKey.Size = new Size(0x150, 20);
            this.tbLongTermKey.TabIndex = 6;
            this.tbLongTermKey.KeyPress += new KeyPressEventHandler(this.tbLongTermKey_KeyPress);
            this.lblLtkRandom.AutoSize = true;
            this.lblLtkRandom.Location = new Point(14, 0x75);
            this.lblLtkRandom.Margin = new Padding(2, 0, 2, 0);
            this.lblLtkRandom.Name = "lblLtkRandom";
            this.lblLtkRandom.Size = new Size(0x74, 13);
            this.lblLtkRandom.TabIndex = 5;
            this.lblLtkRandom.Text = "LTK Random (8 bytes):";
            this.lblLtkDiv.AutoSize = true;
            this.lblLtkDiv.Location = new Point(14, 0x5b);
            this.lblLtkDiv.Margin = new Padding(2, 0, 2, 0);
            this.lblLtkDiv.Name = "lblLtkDiv";
            this.lblLtkDiv.Size = new Size(0x88, 13);
            this.lblLtkDiv.TabIndex = 4;
            this.lblLtkDiv.Text = "LTK Diversifier (2 bytes): 0x";
            this.lblLtk.AutoSize = true;
            this.lblLtk.Location = new Point(12, 0x2e);
            this.lblLtk.Margin = new Padding(2, 0, 2, 0);
            this.lblLtk.Name = "lblLtk";
            this.lblLtk.Size = new Size(0x83, 13);
            this.lblLtk.TabIndex = 3;
            this.lblLtk.Text = "Long Term Key (16 bytes):";
            this.rbAuthBondFalse.AutoSize = true;
            this.rbAuthBondFalse.Location = new Point(0x12e, 0x24);
            this.rbAuthBondFalse.Margin = new Padding(2, 3, 2, 3);
            this.rbAuthBondFalse.Name = "rbAuthBondFalse";
            this.rbAuthBondFalse.Size = new Size(50, 0x11);
            this.rbAuthBondFalse.TabIndex = 2;
            this.rbAuthBondFalse.TabStop = true;
            this.rbAuthBondFalse.Text = "False";
            this.rbAuthBondFalse.UseVisualStyleBackColor = true;
            this.rbAuthBondTrue.AutoSize = true;
            this.rbAuthBondTrue.Location = new Point(0x12e, 20);
            this.rbAuthBondTrue.Margin = new Padding(2, 3, 2, 3);
            this.rbAuthBondTrue.Name = "rbAuthBondTrue";
            this.rbAuthBondTrue.Size = new Size(0x2f, 0x11);
            this.rbAuthBondTrue.TabIndex = 1;
            this.rbAuthBondTrue.TabStop = true;
            this.rbAuthBondTrue.Text = "True";
            this.rbAuthBondTrue.UseVisualStyleBackColor = true;
            this.lblAuthBond.AutoSize = true;
            this.lblAuthBond.Location = new Point(0xc1, 0x15);
            this.lblAuthBond.Margin = new Padding(2, 0, 2, 0);
            this.lblAuthBond.Name = "lblAuthBond";
            this.lblAuthBond.Size = new Size(0x68, 13);
            this.lblAuthBond.TabIndex = 0;
            this.lblAuthBond.Text = "Authenticated Bond:";
            this.gbPasskeyInput.Controls.Add(this.lblConnHnd);
            this.gbPasskeyInput.Controls.Add(this.tbPasskeyConnHandle);
            this.gbPasskeyInput.Controls.Add(this.btnSendPasskey);
            this.gbPasskeyInput.Controls.Add(this.lblPassRange);
            this.gbPasskeyInput.Controls.Add(this.tbPasskey);
            this.gbPasskeyInput.Controls.Add(this.lblPasskey);
            this.gbPasskeyInput.Location = new Point(6, 0x56);
            this.gbPasskeyInput.Margin = new Padding(2, 3, 2, 3);
            this.gbPasskeyInput.Name = "gbPasskeyInput";
            this.gbPasskeyInput.Padding = new Padding(2, 3, 2, 3);
            this.gbPasskeyInput.Size = new Size(0x174, 80);
            this.gbPasskeyInput.TabIndex = 2;
            this.gbPasskeyInput.TabStop = false;
            this.gbPasskeyInput.Text = "Passkey Input";
            this.lblConnHnd.AutoSize = true;
            this.lblConnHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblConnHnd.Location = new Point(14, 0x15);
            this.lblConnHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblConnHnd.Name = "lblConnHnd";
            this.lblConnHnd.Size = new Size(0x65, 13);
            this.lblConnHnd.TabIndex = 13;
            this.lblConnHnd.Text = "Connection Handle:";
            this.tbPasskeyConnHandle.Enabled = false;
            this.tbPasskeyConnHandle.Location = new Point(0x76, 0x12);
            this.tbPasskeyConnHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbPasskeyConnHandle.MaxLength = 6;
            this.tbPasskeyConnHandle.Name = "tbPasskeyConnHandle";
            this.tbPasskeyConnHandle.Size = new Size(60, 20);
            this.tbPasskeyConnHandle.TabIndex = 13;
            this.tbPasskeyConnHandle.Text = "0x0000";
            this.tbPasskeyConnHandle.TextAlign = HorizontalAlignment.Center;
            this.btnSendPasskey.Location = new Point(0xfb, 15);
            this.btnSendPasskey.Margin = new Padding(2, 3, 2, 3);
            this.btnSendPasskey.Name = "btnSendPasskey";
            this.btnSendPasskey.Size = new Size(0x66, 0x17);
            this.btnSendPasskey.TabIndex = 3;
            this.btnSendPasskey.Text = "Send Passkey";
            this.btnSendPasskey.UseVisualStyleBackColor = true;
            this.btnSendPasskey.Click += new EventHandler(this.btnSendPassKey_Click);
            this.lblPassRange.AutoSize = true;
            this.lblPassRange.Location = new Point(170, 0x31);
            this.lblPassRange.Margin = new Padding(2, 0, 2, 0);
            this.lblPassRange.Name = "lblPassRange";
            this.lblPassRange.Size = new Size(0x7f, 13);
            this.lblPassRange.TabIndex = 2;
            this.lblPassRange.Text = "(000000 through 999999)";
            this.tbPasskey.Location = new Point(0x76, 0x2e);
            this.tbPasskey.Margin = new Padding(2, 3, 2, 3);
            this.tbPasskey.MaxLength = 6;
            this.tbPasskey.Name = "tbPasskey";
            this.tbPasskey.Size = new Size(0x2e, 20);
            this.tbPasskey.TabIndex = 1;
            this.tbPasskey.Text = "000000";
            this.tbPasskey.KeyPress += new KeyPressEventHandler(this.tbPasskey_KeyPress);
            this.lblPasskey.AutoSize = true;
            this.lblPasskey.Location = new Point(0x41, 0x31);
            this.lblPasskey.Margin = new Padding(2, 0, 2, 0);
            this.lblPasskey.Name = "lblPasskey";
            this.lblPasskey.Size = new Size(50, 13);
            this.lblPasskey.TabIndex = 0;
            this.lblPasskey.Text = "Passkey:";
            this.gbInitParing.Controls.Add(this.tbPairingConnHandle);
            this.gbInitParing.Controls.Add(this.lblPairConnHnd);
            this.gbInitParing.Controls.Add(this.btnSendPairingRequest);
            this.gbInitParing.Controls.Add(this.ckBoxAuthMitmEnabled);
            this.gbInitParing.Controls.Add(this.ckBoxBondingEnabled);
            this.gbInitParing.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gbInitParing.Location = new Point(6, 6);
            this.gbInitParing.Margin = new Padding(2, 3, 2, 3);
            this.gbInitParing.Name = "gbInitParing";
            this.gbInitParing.Padding = new Padding(2, 3, 2, 3);
            this.gbInitParing.Size = new Size(0x174, 0x4b);
            this.gbInitParing.TabIndex = 1;
            this.gbInitParing.TabStop = false;
            this.gbInitParing.Text = "Initiate Pairing";
            this.tbPairingConnHandle.Enabled = false;
            this.tbPairingConnHandle.Location = new Point(0x80, 0x2a);
            this.tbPairingConnHandle.Margin = new Padding(2, 3, 2, 3);
            this.tbPairingConnHandle.MaxLength = 6;
            this.tbPairingConnHandle.Name = "tbPairingConnHandle";
            this.tbPairingConnHandle.Size = new Size(60, 20);
            this.tbPairingConnHandle.TabIndex = 15;
            this.tbPairingConnHandle.Text = "0x0000";
            this.tbPairingConnHandle.TextAlign = HorizontalAlignment.Center;
            this.lblPairConnHnd.AutoSize = true;
            this.lblPairConnHnd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblPairConnHnd.Location = new Point(0x1a, 0x2e);
            this.lblPairConnHnd.Margin = new Padding(2, 0, 2, 0);
            this.lblPairConnHnd.Name = "lblPairConnHnd";
            this.lblPairConnHnd.Size = new Size(0x65, 13);
            this.lblPairConnHnd.TabIndex = 14;
            this.lblPairConnHnd.Text = "Connection Handle:";
            this.btnSendPairingRequest.Location = new Point(0xd3, 0x29);
            this.btnSendPairingRequest.Margin = new Padding(2, 3, 2, 3);
            this.btnSendPairingRequest.Name = "btnSendPairingRequest";
            this.btnSendPairingRequest.Size = new Size(0x8e, 0x17);
            this.btnSendPairingRequest.TabIndex = 2;
            this.btnSendPairingRequest.Text = "Send Pairing Request";
            this.btnSendPairingRequest.UseVisualStyleBackColor = true;
            this.btnSendPairingRequest.Click += new EventHandler(this.btnSendPairingRequest_Click);
            this.ckBoxAuthMitmEnabled.AutoSize = true;
            this.ckBoxAuthMitmEnabled.Location = new Point(0xa9, 0x12);
            this.ckBoxAuthMitmEnabled.Margin = new Padding(2, 3, 2, 3);
            this.ckBoxAuthMitmEnabled.Name = "ckBoxAuthMitmEnabled";
            this.ckBoxAuthMitmEnabled.Size = new Size(0xad, 0x11);
            this.ckBoxAuthMitmEnabled.TabIndex = 1;
            this.ckBoxAuthMitmEnabled.Text = "Authentication (MITM) Enabled";
            this.ckBoxAuthMitmEnabled.UseVisualStyleBackColor = true;
            this.ckBoxBondingEnabled.AutoSize = true;
            this.ckBoxBondingEnabled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ckBoxBondingEnabled.Location = new Point(0x1d, 0x12);
            this.ckBoxBondingEnabled.Margin = new Padding(2, 3, 2, 3);
            this.ckBoxBondingEnabled.Name = "ckBoxBondingEnabled";
            this.ckBoxBondingEnabled.Size = new Size(0x6b, 0x11);
            this.ckBoxBondingEnabled.TabIndex = 0;
            this.ckBoxBondingEnabled.Text = "Bonding Enabled";
            this.ckBoxBondingEnabled.UseVisualStyleBackColor = true;
            this.labelPairingStatus.AutoSize = true;
            this.labelPairingStatus.Location = new Point(0x3e, 0x1d4);
            this.labelPairingStatus.Margin = new Padding(2, 0, 2, 0);
            this.labelPairingStatus.Name = "labelPairingStatus";
            this.labelPairingStatus.Size = new Size(0x4b, 13);
            this.labelPairingStatus.TabIndex = 5;
            this.labelPairingStatus.Text = "Pairing Status:";
            this.labelPairingStatus.Visible = false;
            this.tbPairingStatus.Location = new Point(0x90, 0x1d1);
            this.tbPairingStatus.Margin = new Padding(2, 3, 2, 3);
            this.tbPairingStatus.Name = "tbPairingStatus";
            this.tbPairingStatus.ReadOnly = true;
            this.tbPairingStatus.Size = new Size(0xb6, 20);
            this.tbPairingStatus.TabIndex = 7;
            this.tbPairingStatus.TextAlign = HorizontalAlignment.Center;
            this.tbPairingStatus.Visible = false;
            this.tpAdvCommands.BackColor = Color.Transparent;
            this.tpAdvCommands.Controls.Add(this.scTreeGrid);
            this.tpAdvCommands.Location = new Point(4, 0x16);
            this.tpAdvCommands.Margin = new Padding(2, 3, 2, 3);
            this.tpAdvCommands.Name = "tpAdvCommands";
            this.tpAdvCommands.Size = new Size(0x183, 0x1fc);
            this.tpAdvCommands.TabIndex = 2;
            this.tpAdvCommands.Text = "Adv.Commands";
            this.tpAdvCommands.UseVisualStyleBackColor = true;
            this.scTreeGrid.Dock = DockStyle.Fill;
            this.scTreeGrid.Location = new Point(0, 0);
            this.scTreeGrid.Margin = new Padding(2, 3, 2, 3);
            this.scTreeGrid.Name = "scTreeGrid";
            this.scTreeGrid.Orientation = Orientation.Horizontal;
            this.scTreeGrid.Panel1.Controls.Add(this.tvAdvCmdList);
            this.scTreeGrid.Panel2.Controls.Add(this.pgAdvCmds);
            this.scTreeGrid.Size = new Size(0x183, 0x1fc);
            this.scTreeGrid.SplitterDistance = 0x100;
            this.scTreeGrid.SplitterWidth = 3;
            this.scTreeGrid.TabIndex = 2;
            this.tvAdvCmdList.Dock = DockStyle.Fill;
            this.tvAdvCmdList.HideSelection = false;
            this.tvAdvCmdList.Location = new Point(0, 0);
            this.tvAdvCmdList.Margin = new Padding(2, 3, 2, 3);
            this.tvAdvCmdList.Name = "tvAdvCmdList";
            this.tvAdvCmdList.Size = new Size(0x183, 0x100);
            this.tvAdvCmdList.TabIndex = 1;
            this.tvAdvCmdList.AfterSelect += new TreeViewEventHandler(this.treeViewCmdList_AfterSelect);
            this.pgAdvCmds.Dock = DockStyle.Fill;
            this.pgAdvCmds.Location = new Point(0, 0);
            this.pgAdvCmds.Margin = new Padding(2, 3, 2, 3);
            this.pgAdvCmds.Name = "pgAdvCmds";
            this.pgAdvCmds.PropertySort = PropertySort.NoSort;
            this.pgAdvCmds.Size = new Size(0x183, 0xf9);
            this.pgAdvCmds.TabIndex = 2;
            this.pgAdvCmds.ToolbarVisible = false;
            this.pgAdvCmds.Layout += new LayoutEventHandler(this.pgAdvCmds_Layout);
            this.cmsAdvTab.Items.AddRange(new ToolStripItem[] { this.tsmiSendAdvCmd });
            this.cmsAdvTab.Name = "contextMenuStrip1";
            this.cmsAdvTab.Size = new Size(0x65, 0x1a);
            this.tsmiSendAdvCmd.Name = "tsmiSendAdvCmd";
            this.tsmiSendAdvCmd.Size = new Size(100, 0x16);
            this.tsmiSendAdvCmd.Text = "Send";
            this.tsmiSendAdvCmd.Click += new EventHandler(this.tsmiSendAdvCmd_Click);
            this.btnSendShared.Location = new Point(0x10, 0x21d);
            this.btnSendShared.Margin = new Padding(2, 3, 2, 3);
            this.btnSendShared.Name = "btnSendShared";
            this.btnSendShared.Size = new Size(0x61, 0x17);
            this.btnSendShared.TabIndex = 3;
            this.btnSendShared.Text = "Send Command";
            this.btnSendShared.UseVisualStyleBackColor = true;
            this.btnSendShared.Click += new EventHandler(this.btnSendShared_Click);
            this.pbSharedDevice.Location = new Point(0x86, 0x21d);
            this.pbSharedDevice.Margin = new Padding(2, 3, 2, 3);
            this.pbSharedDevice.Name = "pbSharedDevice";
            this.pbSharedDevice.Size = new Size(0x106, 0x17);
            this.pbSharedDevice.Step = 2;
            this.pbSharedDevice.Style = ProgressBarStyle.Marquee;
            this.pbSharedDevice.TabIndex = 4;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            base.ClientSize = new Size(0x19b, 570);
            base.Controls.Add(this.pbSharedDevice);
            base.Controls.Add(this.btnSendShared);
            base.Controls.Add(this.tcDeviceTabs);
            base.Name = "DeviceTabsForm";
            this.Text = "Device Tabs Form";
            base.Load += new EventHandler(this.DeviceTabsForm_Load);
            base.Scroll += new ScrollEventHandler(this.DeviceTabsForm_Scroll);
            this.tcDeviceTabs.ResumeLayout(false);
            this.tpDiscoverConnect.ResumeLayout(false);
            this.gbLinkControl.ResumeLayout(false);
            this.gbTerminateLink.ResumeLayout(false);
            this.gbTerminateLink.PerformLayout();
            this.gbEstablishLink.ResumeLayout(false);
            this.gbEstablishLink.PerformLayout();
            this.gbConnSettings.ResumeLayout(false);
            this.gbConnSettings.PerformLayout();
            this.gbDiscovery.ResumeLayout(false);
            this.gbDiscovery.PerformLayout();
            this.tpReadWrite.ResumeLayout(false);
            this.gbCharWrite.ResumeLayout(false);
            this.gbCharWrite.PerformLayout();
            this.gbWriteArea.ResumeLayout(false);
            this.gbWriteArea.PerformLayout();
            this.gbCharRead.ResumeLayout(false);
            this.gbCharRead.PerformLayout();
            this.gbReadArea.ResumeLayout(false);
            this.gbReadArea.PerformLayout();
            this.tpPairingBonding.ResumeLayout(false);
            this.tpPairingBonding.PerformLayout();
            this.gbLongTermKeyData.ResumeLayout(false);
            this.gbLongTermKeyData.PerformLayout();
            this.gbEncryptLTKey.ResumeLayout(false);
            this.gbEncryptLTKey.PerformLayout();
            this.gbPasskeyInput.ResumeLayout(false);
            this.gbPasskeyInput.PerformLayout();
            this.gbInitParing.ResumeLayout(false);
            this.gbInitParing.PerformLayout();
            this.tpAdvCommands.ResumeLayout(false);
            this.scTreeGrid.Panel1.ResumeLayout(false);
            this.scTreeGrid.Panel2.ResumeLayout(false);
            this.scTreeGrid.ResumeLayout(false);
            this.cmsAdvTab.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitiateBondUserInputControl(bool enabled)
        {
            this.tbBondConnHandle.Enabled = enabled;
            this.rbAuthBondTrue.Enabled = enabled;
            this.rbAuthBondFalse.Enabled = enabled;
            this.tbLongTermKey.Enabled = enabled;
            this.tbLTKDiversifier.Enabled = enabled;
            this.tbLTKRandom.Enabled = enabled;
            this.btnLoadLongTermKey.Enabled = enabled;
            this.btnEncryptLink.Enabled = enabled;
        }

        private void InitiatePairingUserInputControl(bool enabled)
        {
            this.ckBoxBondingEnabled.Enabled = enabled;
            this.ckBoxAuthMitmEnabled.Enabled = enabled;
            this.tbPairingConnHandle.Enabled = enabled;
            this.btnSendPairingRequest.Enabled = enabled;
        }

        private void LoadHCICmds()
        {
            TreeNode node = new TreeNode();
            node.Text = node.Name = "HCI Extended";
            this.tvAdvCmdList.Nodes.Add(node);
            TreeNode node2 = new TreeNode();
            node2.Name = node2.Text = this.devForm.HCIExt_SetRxGain.cmdName;
            node2.Tag = this.devForm.HCIExt_SetRxGain;
            node.Nodes.Add(node2);
            TreeNode node3 = new TreeNode();
            node3.Name = node3.Text = this.devForm.HCIExt_SetTxPower.cmdName;
            node3.Tag = this.devForm.HCIExt_SetTxPower;
            node.Nodes.Add(node3);
            TreeNode node4 = new TreeNode();
            node4.Name = node4.Text = this.devForm.HCIExt_OnePktPerEvt.cmdName;
            node4.Tag = this.devForm.HCIExt_OnePktPerEvt;
            node.Nodes.Add(node4);
            TreeNode node5 = new TreeNode();
            node5.Name = node5.Text = this.devForm.HCIExt_ClkDivideOnHalt.cmdName;
            node5.Tag = this.devForm.HCIExt_ClkDivideOnHalt;
            node.Nodes.Add(node5);
            TreeNode node6 = new TreeNode();
            node6.Name = node6.Text = this.devForm.HCIExt_DeclareNvUsage.cmdName;
            node6.Tag = this.devForm.HCIExt_DeclareNvUsage;
            node.Nodes.Add(node6);
            TreeNode node7 = new TreeNode();
            node7.Name = node7.Text = this.devForm.HCIExt_Decrypt.cmdName;
            node7.Tag = this.devForm.HCIExt_Decrypt;
            node.Nodes.Add(node7);
            TreeNode node8 = new TreeNode();
            node8.Name = node8.Text = this.devForm.HCIExt_SetLocalSupportedFeatures.cmdName;
            node8.Tag = this.devForm.HCIExt_SetLocalSupportedFeatures;
            node.Nodes.Add(node8);
            TreeNode node9 = new TreeNode();
            node9.Name = node9.Text = this.devForm.HCIExt_SetFastTxRespTime.cmdName;
            node9.Tag = this.devForm.HCIExt_SetFastTxRespTime;
            node.Nodes.Add(node9);
            TreeNode node10 = new TreeNode();
            node10.Name = node10.Text = this.devForm.HCIExt_ModemTestTx.cmdName;
            node10.Tag = this.devForm.HCIExt_ModemTestTx;
            node.Nodes.Add(node10);
            TreeNode node11 = new TreeNode();
            node11.Name = node11.Text = this.devForm.HCIExt_ModemHopTestTx.cmdName;
            node11.Tag = this.devForm.HCIExt_ModemHopTestTx;
            node.Nodes.Add(node11);
            TreeNode node12 = new TreeNode();
            node12.Name = node12.Text = this.devForm.HCIExt_ModemTestRx.cmdName;
            node12.Tag = this.devForm.HCIExt_ModemTestRx;
            node.Nodes.Add(node12);
            TreeNode node13 = new TreeNode();
            node13.Name = node13.Text = this.devForm.HCIExt_EndModemTest.cmdName;
            node13.Tag = this.devForm.HCIExt_EndModemTest;
            node.Nodes.Add(node13);
            TreeNode node14 = new TreeNode();
            node14.Name = node14.Text = this.devForm.HCIExt_SetBDADDR.cmdName;
            node14.Tag = this.devForm.HCIExt_SetBDADDR;
            node.Nodes.Add(node14);
            TreeNode node15 = new TreeNode();
            node15.Name = node15.Text = this.devForm.HCIExt_SetSCA.cmdName;
            node15.Tag = this.devForm.HCIExt_SetSCA;
            node.Nodes.Add(node15);
            TreeNode node16 = new TreeNode();
            node16.Name = node16.Text = this.devForm.HCIExt_EnablePTM.cmdName;
            node16.Tag = this.devForm.HCIExt_EnablePTM;
            node.Nodes.Add(node16);
            TreeNode node17 = new TreeNode();
            node17.Name = node17.Text = this.devForm.HCIExt_SetFreqTune.cmdName;
            node17.Tag = this.devForm.HCIExt_SetFreqTune;
            node.Nodes.Add(node17);
            TreeNode node18 = new TreeNode();
            node18.Name = node18.Text = this.devForm.HCIExt_SaveFreqTune.cmdName;
            node18.Tag = this.devForm.HCIExt_SaveFreqTune;
            node.Nodes.Add(node18);
            TreeNode node19 = new TreeNode();
            node19.Name = node19.Text = this.devForm.HCIExt_SetMaxDtmTxPower.cmdName;
            node19.Tag = this.devForm.HCIExt_SetMaxDtmTxPower;
            node.Nodes.Add(node19);
            TreeNode node20 = new TreeNode();
            node20.Name = node20.Text = this.devForm.HCIExt_MapPmIoPort.cmdName;
            node20.Tag = this.devForm.HCIExt_MapPmIoPort;
            node.Nodes.Add(node20);
            TreeNode node21 = new TreeNode();
            node21.Name = node21.Text = this.devForm.HCIExt_DisconnectImmed.cmdName;
            node21.Tag = this.devForm.HCIExt_DisconnectImmed;
            node.Nodes.Add(node21);
            TreeNode node22 = new TreeNode();
            node22.Name = node22.Text = this.devForm.HCIExt_PER.cmdName;
            node22.Tag = this.devForm.HCIExt_PER;
            node.Nodes.Add(node22);
            TreeNode node23 = new TreeNode();
            node23.Text = node23.Name = "L2CAP";
            this.tvAdvCmdList.Nodes.Add(node23);
            TreeNode node24 = new TreeNode();
            node24.Name = node24.Text = this.devForm.L2CAP_InfoReq.cmdName;
            node24.Tag = this.devForm.L2CAP_InfoReq;
            node23.Nodes.Add(node24);
            TreeNode node25 = new TreeNode();
            node25.Name = node25.Text = this.devForm.L2CAP_ConnParamUpdateReq.cmdName;
            node25.Tag = this.devForm.L2CAP_ConnParamUpdateReq;
            node23.Nodes.Add(node25);
            TreeNode node26 = new TreeNode();
            node26.Text = node26.Name = "ATT";
            this.tvAdvCmdList.Nodes.Add(node26);
            TreeNode node27 = new TreeNode();
            node27.Name = node27.Text = this.devForm.ATT_ErrorRsp.cmdName;
            node27.Tag = this.devForm.ATT_ErrorRsp;
            node26.Nodes.Add(node27);
            TreeNode node28 = new TreeNode();
            node28.Name = node28.Text = this.devForm.ATT_ExchangeMTUReq.cmdName;
            node28.Tag = this.devForm.ATT_ExchangeMTUReq;
            node26.Nodes.Add(node28);
            TreeNode node29 = new TreeNode();
            node29.Name = node29.Text = this.devForm.ATT_ExchangeMTURsp.cmdName;
            node29.Tag = this.devForm.ATT_ExchangeMTURsp;
            node26.Nodes.Add(node29);
            TreeNode node30 = new TreeNode();
            node30.Name = node30.Text = this.devForm.ATT_FindInfoReq.cmdName;
            node30.Tag = this.devForm.ATT_FindInfoReq;
            node26.Nodes.Add(node30);
            TreeNode node31 = new TreeNode();
            node31.Name = node31.Text = this.devForm.ATT_FindInfoRsp.cmdName;
            node31.Tag = this.devForm.ATT_FindInfoRsp;
            node26.Nodes.Add(node31);
            TreeNode node32 = new TreeNode();
            node32.Name = node32.Text = this.devForm.ATT_FindByTypeValueReq.cmdName;
            node32.Tag = this.devForm.ATT_FindByTypeValueReq;
            node26.Nodes.Add(node32);
            TreeNode node33 = new TreeNode();
            node33.Name = node33.Text = this.devForm.ATT_FindByTypeValueRsp.cmdName;
            node33.Tag = this.devForm.ATT_FindByTypeValueRsp;
            node26.Nodes.Add(node33);
            TreeNode node34 = new TreeNode();
            node34.Name = node34.Text = this.devForm.ATT_ReadByTypeReq.cmdName;
            node34.Tag = this.devForm.ATT_ReadByTypeReq;
            node26.Nodes.Add(node34);
            TreeNode node35 = new TreeNode();
            node35.Name = node35.Text = this.devForm.ATT_ReadByTypeRsp.cmdName;
            node35.Tag = this.devForm.ATT_ReadByTypeRsp;
            node26.Nodes.Add(node35);
            TreeNode node36 = new TreeNode();
            node36.Name = node36.Text = this.devForm.ATT_ReadReq.cmdName;
            node36.Tag = this.devForm.ATT_ReadReq;
            node26.Nodes.Add(node36);
            TreeNode node37 = new TreeNode();
            node37.Name = node37.Text = this.devForm.ATT_ReadRsp.cmdName;
            node37.Tag = this.devForm.ATT_ReadRsp;
            node26.Nodes.Add(node37);
            TreeNode node38 = new TreeNode();
            node38.Name = node38.Text = this.devForm.ATT_ReadBlobReq.cmdName;
            node38.Tag = this.devForm.ATT_ReadBlobReq;
            node26.Nodes.Add(node38);
            TreeNode node39 = new TreeNode();
            node39.Name = node39.Text = this.devForm.ATT_ReadBlobRsp.cmdName;
            node39.Tag = this.devForm.ATT_ReadBlobRsp;
            node26.Nodes.Add(node39);
            TreeNode node40 = new TreeNode();
            node40.Name = node40.Text = this.devForm.ATT_ReadMultiReq.cmdName;
            node40.Tag = this.devForm.ATT_ReadMultiReq;
            node26.Nodes.Add(node40);
            TreeNode node41 = new TreeNode();
            node41.Name = node41.Text = this.devForm.ATT_ReadMultiRsp.cmdName;
            node41.Tag = this.devForm.ATT_ReadMultiRsp;
            node26.Nodes.Add(node41);
            TreeNode node42 = new TreeNode();
            node42.Name = node42.Text = this.devForm.ATT_ReadByGrpTypeReq.cmdName;
            node42.Tag = this.devForm.ATT_ReadByGrpTypeReq;
            node26.Nodes.Add(node42);
            TreeNode node43 = new TreeNode();
            node43.Name = node43.Text = this.devForm.ATT_ReadByGrpTypeRsp.cmdName;
            node43.Tag = this.devForm.ATT_ReadByGrpTypeRsp;
            node26.Nodes.Add(node43);
            TreeNode node44 = new TreeNode();
            node44.Name = node44.Text = this.devForm.ATT_WriteReq.cmdName;
            node44.Tag = this.devForm.ATT_WriteReq;
            node26.Nodes.Add(node44);
            TreeNode node45 = new TreeNode();
            node45.Name = node45.Text = this.devForm.ATT_WriteRsp.cmdName;
            node45.Tag = this.devForm.ATT_WriteRsp;
            node26.Nodes.Add(node45);
            TreeNode node46 = new TreeNode();
            node46.Name = node46.Text = this.devForm.ATT_PrepareWriteReq.cmdName;
            node46.Tag = this.devForm.ATT_PrepareWriteReq;
            node26.Nodes.Add(node46);
            TreeNode node47 = new TreeNode();
            node47.Name = node47.Text = this.devForm.ATT_PrepareWriteRsp.cmdName;
            node47.Tag = this.devForm.ATT_PrepareWriteRsp;
            node26.Nodes.Add(node47);
            TreeNode node48 = new TreeNode();
            node48.Name = node48.Text = this.devForm.ATT_ExecuteWriteReq.cmdName;
            node48.Tag = this.devForm.ATT_ExecuteWriteReq;
            node26.Nodes.Add(node48);
            TreeNode node49 = new TreeNode();
            node49.Name = node49.Text = this.devForm.ATT_ExecuteWriteRsp.cmdName;
            node49.Tag = this.devForm.ATT_ExecuteWriteRsp;
            node26.Nodes.Add(node49);
            TreeNode node50 = new TreeNode();
            node50.Name = node50.Text = this.devForm.ATT_HandleValueNotification.cmdName;
            node50.Tag = this.devForm.ATT_HandleValueNotification;
            node26.Nodes.Add(node50);
            TreeNode node51 = new TreeNode();
            node51.Name = node51.Text = this.devForm.ATT_HandleValueIndication.cmdName;
            node51.Tag = this.devForm.ATT_HandleValueIndication;
            node26.Nodes.Add(node51);
            TreeNode node52 = new TreeNode();
            node52.Name = node52.Text = this.devForm.ATT_HandleValueConfirmation.cmdName;
            node52.Tag = this.devForm.ATT_HandleValueConfirmation;
            node26.Nodes.Add(node52);
            TreeNode node53 = new TreeNode();
            node53.Text = "GATT";
            node53.Name = "GATT";
            this.tvAdvCmdList.Nodes.Add(node53);
            TreeNode node54 = new TreeNode();
            node54.Name = node54.Text = this.devForm.GATT_ExchangeMTU.cmdName;
            node54.Tag = this.devForm.GATT_ExchangeMTU;
            node53.Nodes.Add(node54);
            TreeNode node55 = new TreeNode();
            node55.Name = node55.Text = this.devForm.GATT_DiscAllPrimaryServices.cmdName;
            node55.Tag = this.devForm.GATT_DiscAllPrimaryServices;
            node53.Nodes.Add(node55);
            TreeNode node56 = new TreeNode();
            node56.Name = node56.Text = this.devForm.GATT_DiscPrimaryServiceByUUID.cmdName;
            node56.Tag = this.devForm.GATT_DiscPrimaryServiceByUUID;
            node53.Nodes.Add(node56);
            TreeNode node57 = new TreeNode();
            node57.Name = node57.Text = this.devForm.GATT_FindIncludedServices.cmdName;
            node57.Tag = this.devForm.GATT_FindIncludedServices;
            node53.Nodes.Add(node57);
            TreeNode node58 = new TreeNode();
            node58.Name = node58.Text = this.devForm.GATT_DiscAllChars.cmdName;
            node58.Tag = this.devForm.GATT_DiscAllChars;
            node53.Nodes.Add(node58);
            TreeNode node59 = new TreeNode();
            node59.Name = node59.Text = this.devForm.GATT_DiscCharsByUUID.cmdName;
            node59.Tag = this.devForm.GATT_DiscCharsByUUID;
            node53.Nodes.Add(node59);
            TreeNode node60 = new TreeNode();
            node60.Name = node60.Text = this.devForm.GATT_DiscAllCharDescs.cmdName;
            node60.Tag = this.devForm.GATT_DiscAllCharDescs;
            node53.Nodes.Add(node60);
            TreeNode node61 = new TreeNode();
            node61.Name = node61.Text = this.devForm.GATT_ReadCharValue.cmdName;
            node61.Tag = this.devForm.GATT_ReadCharValue;
            node53.Nodes.Add(node61);
            TreeNode node62 = new TreeNode();
            node62.Name = node62.Text = this.devForm.GATT_ReadUsingCharUUID.cmdName;
            node62.Tag = this.devForm.GATT_ReadUsingCharUUID;
            node53.Nodes.Add(node62);
            TreeNode node63 = new TreeNode();
            node63.Name = node63.Text = this.devForm.GATT_ReadLongCharValue.cmdName;
            node63.Tag = this.devForm.GATT_ReadLongCharValue;
            node53.Nodes.Add(node63);
            TreeNode node64 = new TreeNode();
            node64.Name = node64.Text = this.devForm.GATT_ReadMultiCharValues.cmdName;
            node64.Tag = this.devForm.GATT_ReadMultiCharValues;
            node53.Nodes.Add(node64);
            TreeNode node65 = new TreeNode();
            node65.Name = node65.Text = this.devForm.GATT_WriteNoRsp.cmdName;
            node65.Tag = this.devForm.GATT_WriteNoRsp;
            node53.Nodes.Add(node65);
            TreeNode node66 = new TreeNode();
            node66.Name = node66.Text = this.devForm.GATT_SignedWriteNoRsp.cmdName;
            node66.Tag = this.devForm.GATT_SignedWriteNoRsp;
            node53.Nodes.Add(node66);
            TreeNode node67 = new TreeNode();
            node67.Name = node67.Text = this.devForm.GATT_WriteCharValue.cmdName;
            node67.Tag = this.devForm.GATT_WriteCharValue;
            node53.Nodes.Add(node67);
            TreeNode node68 = new TreeNode();
            node68.Name = node68.Text = this.devForm.GATT_WriteLongCharValue.cmdName;
            node68.Tag = this.devForm.GATT_WriteLongCharValue;
            node53.Nodes.Add(node68);
            TreeNode node69 = new TreeNode();
            node69.Name = node69.Text = this.devForm.GATT_ReliableWrites.cmdName;
            node69.Tag = this.devForm.GATT_ReliableWrites;
            node53.Nodes.Add(node69);
            TreeNode node70 = new TreeNode();
            node70.Name = node70.Text = this.devForm.GATT_ReadCharDesc.cmdName;
            node70.Tag = this.devForm.GATT_ReadCharDesc;
            node53.Nodes.Add(node70);
            TreeNode node71 = new TreeNode();
            node71.Name = node71.Text = this.devForm.GATT_ReadLongCharDesc.cmdName;
            node71.Tag = this.devForm.GATT_ReadLongCharDesc;
            node53.Nodes.Add(node71);
            TreeNode node72 = new TreeNode();
            node72.Name = node72.Text = this.devForm.GATT_WriteCharDesc.cmdName;
            node72.Tag = this.devForm.GATT_WriteCharDesc;
            node53.Nodes.Add(node72);
            TreeNode node73 = new TreeNode();
            node73.Name = node73.Text = this.devForm.GATT_WriteLongCharDesc.cmdName;
            node73.Tag = this.devForm.GATT_WriteLongCharDesc;
            node53.Nodes.Add(node73);
            TreeNode node74 = new TreeNode();
            node74.Name = node74.Text = this.devForm.GATT_Notification.cmdName;
            node74.Tag = this.devForm.GATT_Notification;
            node53.Nodes.Add(node74);
            TreeNode node75 = new TreeNode();
            node75.Name = node75.Text = this.devForm.GATT_Indication.cmdName;
            node75.Tag = this.devForm.GATT_Indication;
            node53.Nodes.Add(node75);
            TreeNode node76 = new TreeNode();
            node76.Name = node76.Text = this.devForm.GATT_AddService.cmdName;
            node76.Tag = this.devForm.GATT_AddService;
            node53.Nodes.Add(node76);
            TreeNode node77 = new TreeNode();
            node77.Name = node77.Text = this.devForm.GATT_DelService.cmdName;
            node77.Tag = this.devForm.GATT_DelService;
            node53.Nodes.Add(node77);
            TreeNode node78 = new TreeNode();
            node78.Name = node78.Text = this.devForm.GATT_AddAttribute.cmdName;
            node78.Tag = this.devForm.GATT_AddAttribute;
            node53.Nodes.Add(node78);
            TreeNode node79 = new TreeNode();
            node79.Text = node79.Name = "GAP";
            this.tvAdvCmdList.Nodes.Add(node79);
            TreeNode node80 = new TreeNode();
            node80.Name = node80.Text = this.devForm.GAP_DeviceInit.cmdName;
            node80.Tag = this.devForm.GAP_DeviceInit;
            node79.Nodes.Add(node80);
            TreeNode node81 = new TreeNode();
            node81.Name = node81.Text = this.devForm.GAP_ConfigDeviceAddr.cmdName;
            node81.Tag = this.devForm.GAP_ConfigDeviceAddr;
            node79.Nodes.Add(node81);
            TreeNode node82 = new TreeNode();
            node82.Name = node82.Text = this.devForm.GAP_DeviceDiscoveryRequest.cmdName;
            node82.Tag = this.devForm.GAP_DeviceDiscoveryRequest;
            node79.Nodes.Add(node82);
            TreeNode node83 = new TreeNode();
            node83.Name = node83.Text = this.devForm.GAP_DeviceDiscoveryCancel.cmdName;
            node83.Tag = this.devForm.GAP_DeviceDiscoveryCancel;
            node79.Nodes.Add(node83);
            TreeNode node84 = new TreeNode();
            node84.Name = node84.Text = this.devForm.GAP_MakeDiscoverable.cmdName;
            node84.Tag = this.devForm.GAP_MakeDiscoverable;
            node79.Nodes.Add(node84);
            TreeNode node85 = new TreeNode();
            node85.Name = node85.Text = this.devForm.GAP_UpdateAdvertisingData.cmdName;
            node85.Tag = this.devForm.GAP_UpdateAdvertisingData;
            node79.Nodes.Add(node85);
            TreeNode node86 = new TreeNode();
            node86.Name = node86.Text = this.devForm.GAP_EndDiscoverable.cmdName;
            node86.Tag = this.devForm.GAP_EndDiscoverable;
            node79.Nodes.Add(node86);
            TreeNode node87 = new TreeNode();
            node87.Name = node87.Text = this.devForm.GAP_EstablishLinkRequest.cmdName;
            node87.Tag = this.devForm.GAP_EstablishLinkRequest;
            node79.Nodes.Add(node87);
            TreeNode node88 = new TreeNode();
            node88.Name = node88.Text = this.devForm.GAP_TerminateLinkRequest.cmdName;
            node88.Tag = this.devForm.GAP_TerminateLinkRequest;
            node79.Nodes.Add(node88);
            TreeNode node89 = new TreeNode();
            node89.Name = node89.Text = this.devForm.GAP_Authenticate.cmdName;
            node89.Tag = this.devForm.GAP_Authenticate;
            node79.Nodes.Add(node89);
            TreeNode node90 = new TreeNode();
            node90.Name = node90.Text = this.devForm.GAP_PasskeyUpdate.cmdName;
            node90.Tag = this.devForm.GAP_PasskeyUpdate;
            node79.Nodes.Add(node90);
            TreeNode node91 = new TreeNode();
            node91.Name = node91.Text = this.devForm.GAP_SlaveSecurityRequest.cmdName;
            node91.Tag = this.devForm.GAP_SlaveSecurityRequest;
            node79.Nodes.Add(node91);
            TreeNode node92 = new TreeNode();
            node92.Name = node92.Text = this.devForm.GAP_Signable.cmdName;
            node92.Tag = this.devForm.GAP_Signable;
            node79.Nodes.Add(node92);
            TreeNode node93 = new TreeNode();
            node93.Name = node93.Text = this.devForm.GAP_Bond.cmdName;
            node93.Tag = this.devForm.GAP_Bond;
            node79.Nodes.Add(node93);
            TreeNode node94 = new TreeNode();
            node94.Name = node94.Text = this.devForm.GAP_TerminateAuth.cmdName;
            node94.Tag = this.devForm.GAP_TerminateAuth;
            node79.Nodes.Add(node94);
            TreeNode node95 = new TreeNode();
            node95.Name = node95.Text = this.devForm.GAP_UpdateLinkParamReq.cmdName;
            node95.Tag = this.devForm.GAP_UpdateLinkParamReq;
            node79.Nodes.Add(node95);
            TreeNode node96 = new TreeNode();
            node96.Name = node96.Text = this.devForm.GAP_SetParam.cmdName;
            node96.Tag = this.devForm.GAP_SetParam;
            node79.Nodes.Add(node96);
            TreeNode node97 = new TreeNode();
            node97.Name = node97.Text = this.devForm.GAP_GetParam.cmdName;
            node97.Tag = this.devForm.GAP_GetParam;
            node79.Nodes.Add(node97);
            TreeNode node98 = new TreeNode();
            node98.Name = node98.Text = this.devForm.GAP_ResolvePrivateAddr.cmdName;
            node98.Tag = this.devForm.GAP_ResolvePrivateAddr;
            node79.Nodes.Add(node98);
            TreeNode node99 = new TreeNode();
            node99.Name = node99.Text = this.devForm.GAP_SetAdvToken.cmdName;
            node99.Tag = this.devForm.GAP_SetAdvToken;
            node79.Nodes.Add(node99);
            TreeNode node100 = new TreeNode();
            node100.Name = node100.Text = this.devForm.GAP_RemoveAdvToken.cmdName;
            node100.Tag = this.devForm.GAP_RemoveAdvToken;
            node79.Nodes.Add(node100);
            TreeNode node101 = new TreeNode();
            node101.Name = node101.Text = this.devForm.GAP_UpdateAdvTokens.cmdName;
            node101.Tag = this.devForm.GAP_UpdateAdvTokens;
            node79.Nodes.Add(node101);
            TreeNode node102 = new TreeNode();
            node102.Name = node102.Text = this.devForm.GAP_BondSetParam.cmdName;
            node102.Tag = this.devForm.GAP_BondSetParam;
            node79.Nodes.Add(node102);
            TreeNode node103 = new TreeNode();
            node103.Name = node103.Text = this.devForm.GAP_BondGetParam.cmdName;
            node103.Tag = this.devForm.GAP_BondGetParam;
            node79.Nodes.Add(node103);
            TreeNode node104 = new TreeNode();
            node104.Text = node104.Name = "Util";
            this.tvAdvCmdList.Nodes.Add(node104);
            TreeNode node105 = new TreeNode();
            node105.Name = node105.Text = this.devForm.UTIL_Reset.cmdName;
            node105.Tag = this.devForm.UTIL_Reset;
            node104.Nodes.Add(node105);
            TreeNode node106 = new TreeNode();
            node106.Name = node106.Text = this.devForm.UTIL_NVRead.cmdName;
            node106.Tag = this.devForm.UTIL_NVRead;
            node104.Nodes.Add(node106);
            TreeNode node107 = new TreeNode();
            node107.Name = node107.Text = this.devForm.UTIL_NVWrite.cmdName;
            node107.Tag = this.devForm.UTIL_NVWrite;
            node104.Nodes.Add(node107);
            TreeNode node108 = new TreeNode();
            node108.Name = node108.Text = this.devForm.UTIL_ForceBoot.cmdName;
            node108.Tag = this.devForm.UTIL_ForceBoot;
            node104.Nodes.Add(node108);
            TreeNode node109 = new TreeNode();
            node109.Text = node109.Name = "HCI";
            this.tvAdvCmdList.Nodes.Add(node109);
            TreeNode node110 = new TreeNode();
            node110.Name = node110.Text = this.devForm.HCIOther_ReadRSSI.cmdName;
            node109.Tag = this.devForm.HCIOther_ReadRSSI;
            node109.Nodes.Add(node110);
            TreeNode node111 = new TreeNode();
            node111.Name = node111.Text = this.devForm.HCIOther_LEClearWhiteList.cmdName;
            node109.Tag = this.devForm.HCIOther_LEClearWhiteList;
            node109.Nodes.Add(node111);
            TreeNode node112 = new TreeNode();
            node112.Name = node112.Text = this.devForm.HCIOther_LEAddDeviceToWhiteList.cmdName;
            node109.Tag = this.devForm.HCIOther_LEAddDeviceToWhiteList;
            node109.Nodes.Add(node112);
            TreeNode node113 = new TreeNode();
            node113.Name = node113.Text = this.devForm.HCIOther_LERemoveDeviceFromWhiteList.cmdName;
            node109.Tag = this.devForm.HCIOther_LERemoveDeviceFromWhiteList;
            node109.Nodes.Add(node113);
            TreeNode node114 = new TreeNode();
            node114.Name = node114.Text = this.devForm.HCIOther_LEConnectionUpdate.cmdName;
            node109.Tag = this.devForm.HCIOther_LEConnectionUpdate;
            node109.Nodes.Add(node114);
            TreeNode node115 = new TreeNode();
            node115.Text = "Misc";
            node115.Name = "Misc";
            this.tvAdvCmdList.Nodes.Add(node115);
            TreeNode node116 = new TreeNode();
            node116.Name = node116.Text = this.devForm.MISC_GenericCommand.cmdName;
            node116.Tag = this.devForm.MISC_GenericCommand;
            node115.Nodes.Add(node116);
            TreeNode node117 = new TreeNode();
            node117.Name = node117.Text = this.devForm.MISC_RawTxMessage.cmdName;
            node117.Tag = this.devForm.MISC_RawTxMessage;
            node115.Nodes.Add(node117);
        }

        private void maxCI_Changed(object sender, EventArgs e)
        {
            this.SetMaxConnectionInterval((uint) this.nudMaxConnInt.Value);
        }

        private void minCI_Changed(object sender, EventArgs e)
        {
            this.SetMinConnectionInterval((uint) this.nudMinConnInt.Value);
        }

        private void PairBondFieldControl()
        {
            this.UserTabAccess(true);
            switch (this.pairingStatus)
            {
                case PairingStatus.Empty:
                case PairingStatus.NotConnected:
                    this.PairBondFieldTabDisable(false);
                    break;

                case PairingStatus.NotPaired:
                    this.InitiatePairingUserInputControl(true);
                    this.PasskeyInputUserInputControl(false);
                    this.InitiateBondUserInputControl(true);
                    break;

                case PairingStatus.PasskeyNeeded:
                    this.InitiatePairingUserInputControl(false);
                    this.PasskeyInputUserInputControl(true);
                    this.InitiateBondUserInputControl(false);
                    break;

                case PairingStatus.DevicesPairedBonded:
                case PairingStatus.DevicesPaired:
                case PairingStatus.PasskeyIncorrect:
                    this.InitiatePairingUserInputControl(true);
                    this.PasskeyInputUserInputControl(false);
                    this.InitiateBondUserInputControl(true);
                    break;

                case PairingStatus.ConnectionTimedOut:
                    this.PairBondFieldTabDisable(false);
                    break;
            }
            if (this.tbLongTermKeyData.Text.Length == 0)
            {
                this.btnSaveLongTermKey.Enabled = false;
            }
            else
            {
                this.btnSaveLongTermKey.Enabled = true;
            }
        }

        private void PairBondFieldTabDisable(bool disableTabAcccess)
        {
            if (disableTabAcccess)
            {
                this.UserTabAccess(true);
            }
            this.InitiatePairingUserInputControl(false);
            this.PasskeyInputUserInputControl(false);
            this.InitiateBondUserInputControl(false);
            this.btnSaveLongTermKey.Enabled = false;
        }

        public void PairBondUserInputControl()
        {
            if (this.devForm.GetConnectInfo().bDA == "00:00:00:00:00:00")
            {
                this.devForm.StopTimer(DeviceForm.EventType.PairBond);
                this.pairingStatus = PairingStatus.NotConnected;
                this.UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
                this.ShowProgress(false);
            }
            switch (this.pairingStatus)
            {
                case PairingStatus.Empty:
                    this.pairingStatus = PairingStatus.NotConnected;
                    break;

                case PairingStatus.NotConnected:
                    this.UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
                    if (this.devForm.GetConnectInfo().bDA != "00:00:00:00:00:00")
                    {
                        this.pairingStatus = PairingStatus.NotPaired;
                    }
                    break;

                case PairingStatus.NotPaired:
                    this.UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
                    break;

                case PairingStatus.DevicesPairedBonded:
                case PairingStatus.DevicesPaired:
                case PairingStatus.PasskeyIncorrect:
                case PairingStatus.ConnectionTimedOut:
                    this.UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
                    break;
            }
            this.tbPairingStatus.Text = this.GetPairingStatusStr(this.pairingStatus);
            this.PairBondFieldControl();
        }

        private void PasskeyInputUserInputControl(bool enabled)
        {
            this.tbPasskey.Enabled = enabled;
            this.btnSendPasskey.Enabled = enabled;
        }

        private void pgAdvCmds_Layout(object sender, LayoutEventArgs e)
        {
            if (this.sharedObjs.IsMonoRunning())
            {
                this.pgAdvCmds.ToolbarVisible = false;
                this.pgAdvCmds.PropertySort = PropertySort.NoSort;
                this.pgAdvCmds.ContextMenu = null;
            }
        }

        private List<CsvData> ReadCsv(string pathFileNameStr, ref bool fileError)
        {
            List<CsvData> list = new List<CsvData>();
            fileError = false;
            try
            {
                if (pathFileNameStr == null)
                {
                    throw new ArgumentException(string.Format("There Is No Filename And/Or Path For Reading Csv Data.\n", new object[0]));
                }
                using (StreamReader reader = new StreamReader(pathFileNameStr))
                {
                    string str2 = string.Empty;
                    int num = 0;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        num++;
                        string[] strArray = str2.Split(new char[] { ',' });
                        if (strArray.Length != this.CsvNumberOfLineElements)
                        {
                            throw new ArgumentException(string.Format("Not Enough Data Items On Line {0:D}\nExpected {1:D} Data Items On Each Line.\n", num, this.CsvNumberOfLineElements));
                        }
                        for (int i = 0; i < this.CsvNumberOfLineElements; i++)
                        {
                            strArray[i] = strArray[i].Trim();
                            strArray[i] = strArray[i].Replace("\"", "");
                        }
                        CsvData item = new CsvData();
                        item = new CsvData();
                        item.addr = strArray[0];
                        item.auth = strArray[1];
                        item.ltk = strArray[2];
                        item.div = strArray[3];
                        item.rand = strArray[4];
                        list.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Cannot Load Or Parse The CSV File.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                fileError = true;
            }
            return list;
        }

        private void readFormat_Click(object sender, EventArgs e)
        {
            if (this.rbASCIIRead.Checked)
            {
                this.tbReadValue.Text = this.devUtils.HexStr2UserDefinedStr((string) this.tbReadValue.Tag, SharedAppObjs.StringType.ASCII);
            }
            else if (this.rbDecimalRead.Checked)
            {
                this.tbReadValue.Text = this.devUtils.HexStr2UserDefinedStr((string) this.tbReadValue.Tag, SharedAppObjs.StringType.DEC);
            }
            else
            {
                this.tbReadValue.Text = this.devUtils.HexStr2UserDefinedStr((string) this.tbReadValue.Tag, SharedAppObjs.StringType.HEX);
            }
        }

        private void readType_Changed(object sender, EventArgs e)
        {
            switch (this.cbReadType.SelectedIndex)
            {
                case 0:
                    this.tbReadAttrHandle.Enabled = true;
                    this.tbReadConnHandle.Enabled = true;
                    this.tbReadEndHandle.Enabled = false;
                    this.tbReadStartHandle.Enabled = false;
                    this.tbReadUUID.Enabled = false;
                    if (!string.IsNullOrEmpty(this.tbReadAttrHandle.Text))
                    {
                        break;
                    }
                    this.tbReadAttrHandle.Text = this.devForm.GATT_ReadCharValue.handle.ToString();
                    return;

                case 1:
                    this.tbReadAttrHandle.Enabled = false;
                    this.tbReadConnHandle.Enabled = true;
                    this.tbReadEndHandle.Enabled = true;
                    this.tbReadStartHandle.Enabled = true;
                    this.tbReadUUID.Enabled = true;
                    if (!string.IsNullOrEmpty(this.tbReadUUID.Text))
                    {
                        break;
                    }
                    this.tbReadUUID.Text = this.devForm.GATT_ReadUsingCharUUID.type;
                    return;

                case 2:
                    this.tbReadAttrHandle.Enabled = true;
                    this.tbReadConnHandle.Enabled = true;
                    this.tbReadEndHandle.Enabled = false;
                    this.tbReadStartHandle.Enabled = false;
                    this.tbReadUUID.Enabled = false;
                    if (!string.IsNullOrEmpty(this.tbReadAttrHandle.Text))
                    {
                        break;
                    }
                    this.tbReadAttrHandle.Text = this.devForm.GATT_ReadMultiCharValues.handles;
                    return;

                case 3:
                    this.tbReadAttrHandle.Enabled = false;
                    this.tbReadConnHandle.Enabled = true;
                    this.tbReadEndHandle.Enabled = true;
                    this.tbReadStartHandle.Enabled = true;
                    this.tbReadUUID.Enabled = true;
                    if (string.IsNullOrEmpty(this.tbReadUUID.Text))
                    {
                        this.tbReadUUID.Text = this.devForm.GATT_DiscCharsByUUID.type;
                    }
                    break;

                default:
                    return;
            }
        }

        private bool ReplaceAddrDataInCsv(CsvData newCsvData, ref List<CsvData> csvData, int csvIndex)
        {
            bool flag = false;
            try
            {
                if ((csvData == null) || (csvData.Count <= 0))
                {
                    throw new ArgumentException(string.Format("There Is No Csv Data To Replace\n", new object[0]));
                }
                CsvData data = new CsvData();
                data = csvData[csvIndex];
                if (data.addr != newCsvData.addr)
                {
                    throw new ArgumentException(string.Format("The Addresses Do Not Match\nCSV Replace Is Cancelled\nExpected {0:S}\nFound {1:S}\n", data.addr, newCsvData.addr));
                }
                data.addr = newCsvData.addr;
                data.auth = newCsvData.auth;
                data.ltk = newCsvData.ltk;
                data.div = newCsvData.div;
                data.rand = newCsvData.rand;
                csvData[csvIndex] = data;
            }
            catch (Exception exception)
            {
                string msg = string.Format("Cannot Access The Data To Replace The Addr In The CSV List\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = true;
            }
            return flag;
        }

        private void ResetSlaveDevices()
        {
            this.SlaveDeviceFound = 0;
            this.cbConnSlaveDeviceBDAddress.Items.Clear();
            this.cbConnSlaveDeviceBDAddress.Items.Add("None");
            this.cbConnSlaveDeviceBDAddress.SelectedIndex = 0;
            this.cbConnSlaveDeviceBDAddress.Update();
            this.linkSlaves.Clear();
            LinkSlave item = new LinkSlave();
            item.addrBDA = "None";
            item.addrType = HCICmds.GAP_AddrType.Public;
            this.linkSlaves.Add(item);
        }

        public void SetAddrType(byte addrType)
        {
            switch (addrType)
            {
                case 0:
                    this.cbConnAddrType.SelectedIndex = 0;
                    return;

                case 1:
                    this.cbConnAddrType.SelectedIndex = 1;
                    return;

                case 2:
                    this.cbConnAddrType.SelectedIndex = 2;
                    return;

                case 3:
                    this.cbConnAddrType.SelectedIndex = 3;
                    return;
            }
            this.cbConnAddrType.SelectedIndex = 0;
        }

        public void SetAuthenticatedBond(bool state)
        {
            if (state)
            {
                this.rbAuthBondTrue.Checked = true;
                this.rbAuthBondFalse.Checked = false;
            }
            else
            {
                this.rbAuthBondTrue.Checked = false;
                this.rbAuthBondFalse.Checked = true;
            }
        }

        private void SetConnectionParameters()
        {
            HCICmds.GAPCmds.GAP_SetParam param = new HCICmds.GAPCmds.GAP_SetParam();
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MIN;
            param.value = (ushort) this.nudMinConnInt.Value;
            this.devForm.sendCmds.SendGAP(param);
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MAX;
            param.value = (ushort) this.nudMaxConnInt.Value;
            this.devForm.sendCmds.SendGAP(param);
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_LATENCY;
            param.value = (ushort) this.nudSlaveLatency.Value;
            this.devForm.sendCmds.SendGAP(param);
            param.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_SUPERV_TIMEOUT;
            param.value = (ushort) this.nudSprVisionTimeout.Value;
            this.devForm.sendCmds.SendGAP(param);
        }

        public bool SetConnHandles(ushort handle)
        {
            this.devForm.HCIExt_DisconnectImmed.connHandle = handle;
            this.devForm.HCIExt_PER.connHandle = handle;
            this.devForm.L2CAP_InfoReq.connHandle = handle;
            this.devForm.L2CAP_ConnParamUpdateReq.connHandle = handle;
            this.devForm.ATT_ErrorRsp.connHandle = handle;
            this.devForm.ATT_ExchangeMTUReq.connHandle = handle;
            this.devForm.ATT_ExchangeMTURsp.connHandle = handle;
            this.devForm.ATT_FindInfoReq.connHandle = handle;
            this.devForm.ATT_FindInfoRsp.connHandle = handle;
            this.devForm.ATT_FindByTypeValueReq.connHandle = handle;
            this.devForm.ATT_FindByTypeValueRsp.connHandle = handle;
            this.devForm.ATT_ReadByTypeReq.connHandle = handle;
            this.devForm.ATT_ReadByTypeRsp.connHandle = handle;
            this.devForm.ATT_ReadReq.connHandle = handle;
            this.devForm.ATT_ReadRsp.connHandle = handle;
            this.devForm.ATT_ReadBlobReq.connHandle = handle;
            this.devForm.ATT_ReadBlobRsp.connHandle = handle;
            this.devForm.ATT_ReadMultiReq.connHandle = handle;
            this.devForm.ATT_ReadMultiRsp.connHandle = handle;
            this.devForm.ATT_ReadByGrpTypeReq.connHandle = handle;
            this.devForm.ATT_ReadByGrpTypeRsp.connHandle = handle;
            this.devForm.ATT_WriteReq.connHandle = handle;
            this.devForm.ATT_WriteRsp.connHandle = handle;
            this.devForm.ATT_PrepareWriteReq.connHandle = handle;
            this.devForm.ATT_PrepareWriteRsp.connHandle = handle;
            this.devForm.ATT_ExecuteWriteReq.connHandle = handle;
            this.devForm.ATT_ExecuteWriteRsp.connHandle = handle;
            this.devForm.ATT_HandleValueNotification.connHandle = handle;
            this.devForm.ATT_HandleValueIndication.connHandle = handle;
            this.devForm.ATT_HandleValueConfirmation.connHandle = handle;
            this.devForm.GATT_ExchangeMTU.connHandle = handle;
            this.devForm.GATT_DiscAllPrimaryServices.connHandle = handle;
            this.devForm.GATT_DiscPrimaryServiceByUUID.connHandle = handle;
            this.devForm.GATT_FindIncludedServices.connHandle = handle;
            this.devForm.GATT_DiscAllChars.connHandle = handle;
            this.devForm.GATT_DiscCharsByUUID.connHandle = handle;
            this.devForm.GATT_DiscAllCharDescs.connHandle = handle;
            this.devForm.GATT_ReadCharValue.connHandle = handle;
            this.devForm.GATT_ReadUsingCharUUID.connHandle = handle;
            this.devForm.GATT_ReadLongCharValue.connHandle = handle;
            this.devForm.GATT_ReadMultiCharValues.connHandle = handle;
            this.devForm.GATT_WriteNoRsp.connHandle = handle;
            this.devForm.GATT_SignedWriteNoRsp.connHandle = handle;
            this.devForm.GATT_WriteCharValue.connHandle = handle;
            this.devForm.GATT_WriteLongCharValue.connHandle = handle;
            this.devForm.GATT_ReliableWrites.connHandle = handle;
            this.devForm.GATT_ReadCharDesc.connHandle = handle;
            this.devForm.GATT_ReadLongCharDesc.connHandle = handle;
            this.devForm.GATT_WriteCharDesc.connHandle = handle;
            this.devForm.GATT_WriteLongCharDesc.connHandle = handle;
            this.devForm.GATT_Notification.connHandle = handle;
            this.devForm.GATT_Indication.connHandle = handle;
            this.devForm.GAP_TerminateLinkRequest.connHandle = handle;
            this.devForm.GAP_Authenticate.connHandle = handle;
            this.devForm.GAP_PasskeyUpdate.connHandle = handle;
            this.devForm.GAP_SlaveSecurityRequest.connHandle = handle;
            this.devForm.GAP_Signable.connHandle = handle;
            this.devForm.GAP_Bond.connHandle = handle;
            this.devForm.GAP_TerminateAuth.connHandle = handle;
            this.devForm.GAP_UpdateLinkParamReq.connHandle = handle;
            this.pgAdvCmds.Invalidate();
            this.pgAdvCmds.Refresh();
            this.tvAdvCmdList.Invalidate();
            this.tvAdvCmdList.Refresh();
            this.tbTermConnHandle.Text = "0x" + handle.ToString("X4");
            this.tbReadConnHandle.Text = "0x" + handle.ToString("X4");
            this.tbWriteConnHandle.Text = "0x" + handle.ToString("X4");
            this.tbPairingConnHandle.Text = "0x" + handle.ToString("X4");
            this.tbPasskeyConnHandle.Text = "0x" + handle.ToString("X4");
            this.tbBondConnHandle.Text = "0x" + handle.ToString("X4");
            return true;
        }

        public void SetGapAuthCompleteInfo(HCICmds.GAPEvts.GAP_AuthenticationComplete obj)
        {
            this.lastGAP_AuthenticationComplete = obj;
            string str = string.Empty;
            if (this.GetAuthenticationEnabled())
            {
                str = "TRUE";
            }
            else
            {
                str = "FALSE";
            }
            this.lastAuthStr = str;
            string str2 = string.Empty;
            str2 = string.Format("Authenticated Bond: {0:S}\r\n", str);
            this.tbLongTermKeyData.Text = str2;
            str = obj.devSecInfo_LTK;
            str2 = string.Format("Long-Term Key:\r\n{0:S}\r\n", str);
            this.tbLongTermKeyData.Text = this.tbLongTermKeyData.Text + str2;
            str2 = string.Format("LTK Diversifier: 0x{0:X}\r\n", obj.devSecInfo_DIV);
            this.tbLongTermKeyData.Text = this.tbLongTermKeyData.Text + str2;
            str = obj.devSecInfo_RAND;
            str2 = string.Format("LTK Random: {0:S}\r\n", str);
            this.tbLongTermKeyData.Text = this.tbLongTermKeyData.Text + str2;
            str = obj.idInfo_BdAddr;
            str2 = string.Format("Identity Info BD Address: {0:S}", str);
            this.tbLongTermKeyData.Text = this.tbLongTermKeyData.Text + str2;
            this.PairBondUserInputControl();
        }

        public void SetMaxConnectionInterval(uint interval)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new SetMaxConnectionIntervalDelegate(this.SetMaxConnectionInterval), new object[] { interval });
            }
            else if ((interval >= 6) && (interval <= 0xc80))
            {
                this.nudMaxConnInt.Value = interval;
                this.lblMaxConnInt.Text = string.Format("({0:f}ms)", interval * 1.25);
            }
        }

        public void SetMinConnectionInterval(uint interval)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new SetMinConnectionIntervalDelegate(this.SetMinConnectionInterval), new object[] { interval });
            }
            else if ((interval >= 6) && (interval <= 0xc80))
            {
                this.nudMinConnInt.Value = interval;
                this.lblMinConnInt.Text = string.Format("({0:f}ms)", interval * 1.25);
            }
        }

        public void SetNudMaxConnIntValue(int value)
        {
            this.nudMaxConnInt.Value = value;
        }

        public void SetNudMinConnIntValue(int value)
        {
            this.nudMinConnInt.Value = value;
        }

        public void SetNudSlaveLatencyValue(int value)
        {
            this.nudSlaveLatency.Value = value;
        }

        public void SetNudSprVisionTimeoutValue(int value)
        {
            this.nudSprVisionTimeout.Value = value;
        }

        public void SetPairingStatus(PairingStatus state)
        {
            this.pairingStatus = state;
            this.PairBondUserInputControl();
        }

        public void SetSlaveLatency(uint latency)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new SetSlaveLatencyDelegate(this.SetSlaveLatency), new object[] { latency });
            }
            else if ((latency >= 0) && (latency <= 0x3e8))
            {
                this.nudSlaveLatency.Value = latency;
            }
        }

        public void SetSupervisionTimeout(uint timeout)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new SetSupervisionTimeoutDelegate(this.SetSupervisionTimeout), new object[] { timeout });
            }
            else if ((timeout >= 10) && (timeout <= 0xc80))
            {
                this.nudSprVisionTimeout.Value = timeout;
                this.lblSupervisionTimeout.Text = string.Format("({0:D}ms)", timeout * 10);
            }
        }

        public void SetTbReadAttrHandleText(string text)
        {
            this.tbReadAttrHandle.Text = text;
        }

        public void SetTbReadStatusText(string text)
        {
            this.tbReadStatus.Text = text;
        }

        public void SetTbReadValueTag(object tag)
        {
            this.tbReadValue.Tag = tag;
        }

        public void SetTbReadValueText(string text)
        {
            this.tbReadValue.Text = text;
        }

        public void SetTbWriteStatusText(string text)
        {
            this.tbWriteStatus.Text = text;
        }

        public void ShowProgress(bool enable)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new ShowProgressDelegate(this.ShowProgress), new object[] { enable });
            }
            else if (enable)
            {
                this.pbSharedDevice.Style = ProgressBarStyle.Marquee;
                this.pbSharedDevice.Enabled = true;
                this.btnSendShared.Enabled = false;
            }
            else
            {
                this.Cursor = Cursors.Default;
                this.discoverConnectStatus = DiscoverConnectStatus.Idle;
                this.DiscoverConnectUserInputControl();
                this.pbSharedDevice.Style = ProgressBarStyle.Continuous;
                this.pbSharedDevice.Enabled = false;
                this.btnSendShared.Enabled = true;
            }
        }

        private void supervisionTimeout_Changed(object sender, EventArgs e)
        {
            this.SetSupervisionTimeout((uint) this.nudSprVisionTimeout.Value);
        }

        public void TabAdvCommandsInitValues()
        {
            this.LoadHCICmds();
        }

        private void TabAdvCommandsToolTips()
        {
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.tvAdvCmdList, "Select A Command To Send");
            tip.SetToolTip(this.pgAdvCmds, "Modify/View Command Data Before Sending");
            tip.SetToolTip(this.btnSendShared, "Send The Command");
            tip.SetToolTip(this.pbSharedDevice, "Device Operation Progress Bar");
        }

        public void TabDiscoverConnectInitValues()
        {
            this.ckBoxActiveScan.Checked = true;
            this.ckBoxWhiteList.Checked = false;
            this.cbScanMode.SelectedIndex = 3;
            this.ResetSlaveDevices();
            this.cbConnAddrType.SelectedIndex = 0;
            this.ckBoxConnWhiteList.Checked = false;
            this.discoverConnectStatus = DiscoverConnectStatus.Idle;
            this.DiscoverConnectUserInputControl();
        }

        private void TabDiscoverConnectToolTips()
        {
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.tpDiscoverConnect, "Discover And Connect To Devices");
            tip.SetToolTip(this.ckBoxActiveScan, "Use Active Scan");
            tip.SetToolTip(this.ckBoxWhiteList, "Use White List");
            tip.SetToolTip(this.cbScanMode, "Device Scan Mode");
            tip.SetToolTip(this.btnScan, "Scan For Devices");
            tip.SetToolTip(this.btnScanCancel, "Cancel Device Scan");
            tip.SetToolTip(this.nudMinConnInt, "Minimum Connection Interval");
            tip.SetToolTip(this.nudMaxConnInt, "Maximum Connection Interval");
            tip.SetToolTip(this.nudSlaveLatency, "Slave Latency");
            tip.SetToolTip(this.nudSprVisionTimeout, "Supervision Timeout");
            tip.SetToolTip(this.btnGetConnectionParams, "Get Connection Parameteres");
            tip.SetToolTip(this.btnSetConnectionParams, "Set Connection Parameters");
            tip.SetToolTip(this.cbConnAddrType, "List Of Address Types");
            tip.SetToolTip(this.cbConnSlaveDeviceBDAddress, "List Of Slave BDA Addresses");
            tip.SetToolTip(this.ckBoxConnWhiteList, "Use Connection White List");
            tip.SetToolTip(this.btnEstablish, "Establish Link");
            tip.SetToolTip(this.btnEstablishCancel, "Cancel Link Establish In Progress");
            tip.SetToolTip(this.tbTermConnHandle, "Link Handle To Terminate");
            tip.SetToolTip(this.btnTerminate, "Terminate Link");
            tip.SetToolTip(this.pbSharedDevice, "Device Operation Progress Bar");
        }

        public void TabPairBondInitValues()
        {
            this.devForm.StopTimer(DeviceForm.EventType.PairBond);
            this.pairingStatus = PairingStatus.Empty;
            this.UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
            this.ShowProgress(false);
            this.SetAuthenticatedBond(false);
            this.PairBondUserInputControl();
        }

        private void TabPairBondToolTips()
        {
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.ckBoxBondingEnabled, "Select If Bonding Is Enabled");
            tip.SetToolTip(this.ckBoxAuthMitmEnabled, "Select If Authentication (MITM) Is Enabled");
            tip.SetToolTip(this.tbPairingConnHandle, "Paring Connection Handle");
            tip.SetToolTip(this.btnSendPairingRequest, "Send The Pairing Request");
            tip.SetToolTip(this.tbPasskeyConnHandle, "The Passkey Connection Handle");
            tip.SetToolTip(this.btnSendPasskey, "Send The Passkey");
            tip.SetToolTip(this.tbPasskey, "The Passkey Value To Send");
            tip.SetToolTip(this.tbBondConnHandle, "Bond Connection Handle");
            tip.SetToolTip(this.rbAuthBondTrue, "Authenticated Bond Is True");
            tip.SetToolTip(this.rbAuthBondFalse, "Authenticated Bond Is False");
            tip.SetToolTip(this.tbLongTermKey, "The Long Term Key To Send");
            tip.SetToolTip(this.tbLTKDiversifier, "Long Term Key Diversifier (Hexidecimal Value)");
            tip.SetToolTip(this.tbLTKRandom, "Long Term Key Random Value");
            tip.SetToolTip(this.btnLoadLongTermKey, "Load The Long Term Key From A File");
            tip.SetToolTip(this.btnEncryptLink, "Encrypt Link");
            tip.SetToolTip(this.tbLongTermKeyData, "Long Term Key Data");
            tip.SetToolTip(this.btnSaveLongTermKey, "Save Long Term Key Data");
            tip.SetToolTip(this.pbSharedDevice, "Device Operation Progress Bar");
        }

        public void TabReadWriteInitValues()
        {
            this.cbReadType.SelectedIndex = 0;
        }

        private void TabReadWriteToolTips()
        {
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.cbReadType, "Type Of Read");
            tip.SetToolTip(this.tbReadAttrHandle, "Read Attribute Handle");
            tip.SetToolTip(this.tbReadUUID, "Read UUID Value");
            tip.SetToolTip(this.tbReadConnHandle, "Connection Handle");
            tip.SetToolTip(this.tbReadStartHandle, "Start Handle");
            tip.SetToolTip(this.tbReadEndHandle, "End Handle");
            tip.SetToolTip(this.rbASCIIRead, "Display As ASCII Text");
            tip.SetToolTip(this.rbDecimalRead, "Display As Decimal");
            tip.SetToolTip(this.rbHexRead, "Display As Hex");
            tip.SetToolTip(this.tbReadValue, "Value Read From The Device");
            tip.SetToolTip(this.tbReadStatus, "Device Read Status");
            tip.SetToolTip(this.btnReadGATTValue, "Perform Read From Device");
            tip.SetToolTip(this.tbWriteAttrHandle, "Handle To Write");
            tip.SetToolTip(this.tbWriteConnHandle, "Connection Handle");
            tip.SetToolTip(this.rbASCIIWrite, "ASCII Text (Like This)");
            tip.SetToolTip(this.rbDecimalWrite, "Decimal (Valid Range = 0 to 4,294,967,295)");
            tip.SetToolTip(this.rbHexWrite, "Hex (xx:xx... or xx xx...)");
            tip.SetToolTip(this.tbWriteValue, "Value To Write To The Device");
            tip.SetToolTip(this.tbWriteStatus, "Device Write Status");
            tip.SetToolTip(this.btnWriteGATTValue, "Perform Write To Device");
            tip.SetToolTip(this.pbSharedDevice, "Device Operation Progress Bar");
        }

        private void tbLongTermKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckHexKeyPress(sender, e);
        }

        private void tbLTKDiversifier_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckHexKeyPress(sender, e);
        }

        private void tbLTKRandom_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckHexKeyPress(sender, e);
        }

        private void tbPasskey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"\d+") && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        private void tcDeviceTab_Selected(object sender, TabControlEventArgs e)
        {
            this.btnSendShared.Visible = false;
            this.btnSendShared.Enabled = false;
            switch (this.tcDeviceTabs.SelectedIndex)
            {
                case 0:
                case 1:
                    break;

                case 2:
                    this.PairBondUserInputControl();
                    return;

                case 3:
                    this.btnSendShared.Visible = true;
                    this.btnSendShared.Enabled = true;
                    break;

                default:
                    return;
            }
        }

        private void TerminateLinkUserInputControl(bool enabled)
        {
            this.tbTermConnHandle.Enabled = enabled;
            this.btnTerminate.Enabled = enabled;
        }

        private void treeViewCmdList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (this.tvAdvCmdList.SelectedNode.Text)
            {
                case "HCIExt_SetRxGain":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetRxGain;
                    break;

                case "HCIExt_SetTxPower":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetTxPower;
                    break;

                case "HCIExt_OnePktPerEvt":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_OnePktPerEvt;
                    break;

                case "HCIExt_ClkDivideOnHalt":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_ClkDivideOnHalt;
                    break;

                case "HCIExt_DeclareNvUsage":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_DeclareNvUsage;
                    break;

                case "HCIExt_Decrypt":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_Decrypt;
                    break;

                case "HCIExt_SetLocalSupportedFeatures":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetLocalSupportedFeatures;
                    break;

                case "HCIExt_SetFastTxRespTime":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetFastTxRespTime;
                    break;

                case "HCIExt_ModemTestTx":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_ModemTestTx;
                    break;

                case "HCIExt_ModemHopTestTx":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_ModemHopTestTx;
                    break;

                case "HCIExt_ModemTestRx":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_ModemTestRx;
                    break;

                case "HCIExt_EndModemTest":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_EndModemTest;
                    break;

                case "HCIExt_SetBDADDR":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetBDADDR;
                    break;

                case "HCIExt_SetSCA":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetSCA;
                    break;

                case "HCIExt_EnablePTM":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_EnablePTM;
                    break;

                case "HCIExt_SetFreqTune":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetFreqTune;
                    break;

                case "HCIExt_SaveFreqTune":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SaveFreqTune;
                    break;

                case "HCIExt_SetMaxDtmTxPower":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_SetMaxDtmTxPower;
                    break;

                case "HCIExt_MapPmIoPort":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_MapPmIoPort;
                    break;

                case "HCIExt_DisconnectImmed":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_DisconnectImmed;
                    break;

                case "HCIExt_PER":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIExt_PER;
                    break;

                case "L2CAP_InfoReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.L2CAP_InfoReq;
                    break;

                case "L2CAP_ConnParamUpdateReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.L2CAP_ConnParamUpdateReq;
                    break;

                case "ATT_ErrorRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ErrorRsp;
                    break;

                case "ATT_ExchangeMTUReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ExchangeMTUReq;
                    break;

                case "ATT_ExchangeMTURsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ExchangeMTURsp;
                    break;

                case "ATT_FindInfoReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_FindInfoReq;
                    break;

                case "ATT_FindInfoRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_FindInfoRsp;
                    break;

                case "ATT_FindByTypeValueReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_FindByTypeValueReq;
                    break;

                case "ATT_FindByTypeValueRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_FindByTypeValueRsp;
                    break;

                case "ATT_ReadByTypeReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadByTypeReq;
                    break;

                case "ATT_ReadByTypeRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadByTypeRsp;
                    break;

                case "ATT_ReadReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadReq;
                    break;

                case "ATT_ReadRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadRsp;
                    break;

                case "ATT_ReadBlobReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadBlobReq;
                    break;

                case "ATT_ReadBlobRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadBlobRsp;
                    break;

                case "ATT_ReadMultiReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadMultiReq;
                    break;

                case "ATT_ReadMultiRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadMultiRsp;
                    break;

                case "ATT_ReadByGrpTypeReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadByGrpTypeReq;
                    break;

                case "ATT_ReadByGrpTypeRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ReadByGrpTypeRsp;
                    break;

                case "ATT_WriteReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_WriteReq;
                    break;

                case "ATT_WriteRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_WriteRsp;
                    break;

                case "ATT_PrepareWriteReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_PrepareWriteReq;
                    break;

                case "ATT_PrepareWriteRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_PrepareWriteRsp;
                    break;

                case "ATT_ExecuteWriteReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ExecuteWriteReq;
                    break;

                case "ATT_ExecuteWriteRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_ExecuteWriteRsp;
                    break;

                case "ATT_HandleValueNotification":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_HandleValueNotification;
                    break;

                case "ATT_HandleValueIndication":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_HandleValueIndication;
                    break;

                case "ATT_HandleValueConfirmation":
                    this.pgAdvCmds.SelectedObject = this.devForm.ATT_HandleValueConfirmation;
                    break;

                case "GATT_ExchangeMTU":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ExchangeMTU;
                    break;

                case "GATT_DiscAllPrimaryServices":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_DiscAllPrimaryServices;
                    break;

                case "GATT_DiscPrimaryServiceByUUID":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_DiscPrimaryServiceByUUID;
                    break;

                case "GATT_FindIncludedServices":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_FindIncludedServices;
                    break;

                case "GATT_DiscAllChars":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_DiscAllChars;
                    break;

                case "GATT_DiscCharsByUUID":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_DiscCharsByUUID;
                    break;

                case "GATT_DiscAllCharDescs":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_DiscAllCharDescs;
                    break;

                case "GATT_ReadCharValue":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReadCharValue;
                    break;

                case "GATT_ReadUsingCharUUID":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReadUsingCharUUID;
                    break;

                case "GATT_ReadLongCharValue":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReadLongCharValue;
                    break;

                case "GATT_ReadMultiCharValues":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReadMultiCharValues;
                    break;

                case "GATT_WriteNoRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_WriteNoRsp;
                    break;

                case "GATT_SignedWriteNoRsp":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_SignedWriteNoRsp;
                    break;

                case "GATT_WriteCharValue":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_WriteCharValue;
                    break;

                case "GATT_WriteLongCharValue":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_WriteLongCharValue;
                    break;

                case "GATT_ReliableWrites":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReliableWrites;
                    break;

                case "GATT_ReadCharDesc":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReadCharDesc;
                    break;

                case "GATT_ReadLongCharDesc":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_ReadLongCharDesc;
                    break;

                case "GATT_WriteCharDesc":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_WriteCharDesc;
                    break;

                case "GATT_WriteLongCharDesc":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_WriteLongCharDesc;
                    break;

                case "GATT_Notification":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_Notification;
                    break;

                case "GATT_Indication":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_Indication;
                    break;

                case "GATT_AddService":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_AddService;
                    break;

                case "GATT_DelService":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_DelService;
                    break;

                case "GATT_AddAttribute":
                    this.pgAdvCmds.SelectedObject = this.devForm.GATT_AddAttribute;
                    break;

                case "GAP_DeviceInit":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_DeviceInit;
                    break;

                case "GAP_ConfigDeviceAddr":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_ConfigDeviceAddr;
                    break;

                case "GAP_DeviceDiscoveryRequest":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_DeviceDiscoveryRequest;
                    break;

                case "GAP_DeviceDiscoveryCancel":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_DeviceDiscoveryCancel;
                    break;

                case "GAP_MakeDiscoverable":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_MakeDiscoverable;
                    break;

                case "GAP_UpdateAdvertisingData":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_UpdateAdvertisingData;
                    break;

                case "GAP_EndDiscoverable":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_EndDiscoverable;
                    break;

                case "GAP_EstablishLinkRequest":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_EstablishLinkRequest;
                    break;

                case "GAP_TerminateLinkRequest":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_TerminateLinkRequest;
                    break;

                case "GAP_Authenticate":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_Authenticate;
                    break;

                case "GAP_PasskeyUpdate":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_PasskeyUpdate;
                    break;

                case "GAP_SlaveSecurityRequest":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_SlaveSecurityRequest;
                    break;

                case "GAP_Signable":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_Signable;
                    break;

                case "GAP_Bond":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_Bond;
                    break;

                case "GAP_TerminateAuth":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_TerminateAuth;
                    break;

                case "GAP_UpdateLinkParamReq":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_UpdateLinkParamReq;
                    break;

                case "GAP_SetParam":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_SetParam;
                    break;

                case "GAP_GetParam":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_GetParam;
                    break;

                case "GAP_ResolvePrivateAddr":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_ResolvePrivateAddr;
                    break;

                case "GAP_SetAdvToken":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_SetAdvToken;
                    break;

                case "GAP_RemoveAdvToken":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_RemoveAdvToken;
                    break;

                case "GAP_UpdateAdvTokens":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_UpdateAdvTokens;
                    break;

                case "GAP_BondSetParam":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_BondSetParam;
                    break;

                case "GAP_BondGetParam":
                    this.pgAdvCmds.SelectedObject = this.devForm.GAP_BondGetParam;
                    break;

                case "UTIL_Reset":
                    this.pgAdvCmds.SelectedObject = this.devForm.UTIL_Reset;
                    break;

                case "UTIL_NVRead":
                    this.pgAdvCmds.SelectedObject = this.devForm.UTIL_NVRead;
                    break;

                case "UTIL_NVWrite":
                    this.pgAdvCmds.SelectedObject = this.devForm.UTIL_NVWrite;
                    break;

                case "UTIL_ForceBoot":
                    this.pgAdvCmds.SelectedObject = this.devForm.UTIL_ForceBoot;
                    break;

                case "HCI_ReadRSSI":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIOther_ReadRSSI;
                    break;

                case "HCI_LEClearWhiteList":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIOther_LEClearWhiteList;
                    break;

                case "HCI_LEAddDeviceToWhiteList":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIOther_LEAddDeviceToWhiteList;
                    break;

                case "HCI_LERemoveDeviceFromWhiteList":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIOther_LERemoveDeviceFromWhiteList;
                    break;

                case "HCI_LEConnectionUpdate":
                    this.pgAdvCmds.SelectedObject = this.devForm.HCIOther_LEConnectionUpdate;
                    break;

                case "MISC_GenericCommand":
                    this.pgAdvCmds.SelectedObject = this.devForm.MISC_GenericCommand;
                    break;

                case "MISC_RawTxMessage":
                    this.pgAdvCmds.SelectedObject = this.devForm.MISC_RawTxMessage;
                    break;

                case "Send All Msgs":
                case "Send All Events":
                case "Send All Forever":
                case "Send Attr Data Cmds":
                case "Test Case":
                    this.pgAdvCmds.SelectedObject = this.tvAdvCmdList.SelectedNode.Text;
                    break;

                default:
                    this.pgAdvCmds.SelectedObject = null;
                    break;
            }
            if (this.pgAdvCmds.SelectedObject == null)
            {
                this.tvAdvCmdList.ContextMenuStrip = null;
            }
            else
            {
                this.tvAdvCmdList.ContextMenuStrip = this.cmsAdvTab;
            }
        }

        private void tsmiSendAdvCmd_Click(object sender, EventArgs e)
        {
            try
            {
                string str;
                switch (this.tvAdvCmdList.SelectedNode.Text)
                {
                    case "HCIExt_SetRxGain":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetRxGain);
                        return;

                    case "HCIExt_SetTxPower":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetTxPower);
                        return;

                    case "HCIExt_OnePktPerEvt":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_OnePktPerEvt);
                        return;

                    case "HCIExt_ClkDivideOnHalt":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_ClkDivideOnHalt);
                        return;

                    case "HCIExt_DeclareNvUsage":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_DeclareNvUsage);
                        return;

                    case "HCIExt_Decrypt":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_Decrypt);
                        return;

                    case "HCIExt_SetLocalSupportedFeatures":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetLocalSupportedFeatures);
                        return;

                    case "HCIExt_SetFastTxRespTime":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetFastTxRespTime);
                        return;

                    case "HCIExt_ModemTestTx":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_ModemTestTx);
                        return;

                    case "HCIExt_ModemHopTestTx":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_ModemHopTestTx);
                        return;

                    case "HCIExt_ModemTestRx":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_ModemTestRx);
                        return;

                    case "HCIExt_EndModemTest":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_EndModemTest);
                        return;

                    case "HCIExt_SetBDADDR":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetBDADDR);
                        return;

                    case "HCIExt_SetSCA":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetSCA);
                        return;

                    case "HCIExt_EnablePTM":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_EnablePTM);
                        return;

                    case "HCIExt_SetFreqTune":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetFreqTune);
                        return;

                    case "HCIExt_SaveFreqTune":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SaveFreqTune);
                        return;

                    case "HCIExt_SetMaxDtmTxPower":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_SetMaxDtmTxPower);
                        return;

                    case "HCIExt_MapPmIoPort":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_MapPmIoPort);
                        return;

                    case "HCIExt_DisconnectImmed":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_DisconnectImmed);
                        return;

                    case "HCIExt_PER":
                        this.devForm.sendCmds.SendHCIExt(this.devForm.HCIExt_PER);
                        return;

                    case "L2CAP_InfoReq":
                        this.devForm.sendCmds.SendL2CAP(this.devForm.L2CAP_InfoReq);
                        return;

                    case "L2CAP_ConnParamUpdateReq":
                        this.devForm.sendCmds.SendL2CAP(this.devForm.L2CAP_ConnParamUpdateReq);
                        return;

                    case "ATT_ErrorRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ErrorRsp);
                        return;

                    case "ATT_ExchangeMTUReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ExchangeMTUReq);
                        return;

                    case "ATT_ExchangeMTURsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ExchangeMTURsp);
                        return;

                    case "ATT_FindInfoReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_FindInfoReq, TxDataOut.CmdType.General);
                        return;

                    case "ATT_FindInfoRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_FindInfoRsp);
                        return;

                    case "ATT_FindByTypeValueReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_FindByTypeValueReq);
                        return;

                    case "ATT_FindByTypeValueRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_FindByTypeValueRsp);
                        return;

                    case "ATT_ReadByTypeReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadByTypeReq);
                        return;

                    case "ATT_ReadByTypeRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadByTypeRsp);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "ATT_ReadReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadReq, TxDataOut.CmdType.General, null);
                        return;

                    case "ATT_ReadRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadRsp);
                        return;

                    case "ATT_ReadBlobReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadBlobReq, TxDataOut.CmdType.General, null);
                        return;

                    case "ATT_ReadBlobRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadBlobRsp);
                        return;

                    case "ATT_ReadMultiReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadMultiReq);
                        return;

                    case "ATT_ReadMultiRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadMultiRsp);
                        return;

                    case "ATT_ReadByGrpTypeReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadByGrpTypeReq, TxDataOut.CmdType.General);
                        return;

                    case "ATT_ReadByGrpTypeRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ReadByGrpTypeRsp);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "ATT_WriteReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_WriteReq, null);
                        return;

                    case "ATT_WriteRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_WriteRsp);
                        return;

                    case "ATT_PrepareWriteReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_PrepareWriteReq);
                        return;

                    case "ATT_PrepareWriteRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_PrepareWriteRsp);
                        return;

                    case "ATT_ExecuteWriteReq":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ExecuteWriteReq, null);
                        return;

                    case "ATT_ExecuteWriteRsp":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_ExecuteWriteRsp);
                        return;

                    case "ATT_HandleValueNotification":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_HandleValueNotification);
                        return;

                    case "ATT_HandleValueIndication":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_HandleValueIndication);
                        return;

                    case "ATT_HandleValueConfirmation":
                        this.devForm.sendCmds.SendATT(this.devForm.ATT_HandleValueConfirmation);
                        return;

                    case "GATT_ExchangeMTU":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ExchangeMTU);
                        return;

                    case "GATT_DiscAllPrimaryServices":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_DiscAllPrimaryServices, TxDataOut.CmdType.General);
                        return;

                    case "GATT_DiscPrimaryServiceByUUID":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_DiscPrimaryServiceByUUID);
                        return;

                    case "GATT_FindIncludedServices":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_FindIncludedServices);
                        return;

                    case "GATT_DiscAllChars":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_DiscAllChars);
                        return;

                    case "GATT_DiscCharsByUUID":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_DiscCharsByUUID);
                        return;

                    case "GATT_DiscAllCharDescs":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_DiscAllCharDescs, TxDataOut.CmdType.General);
                        return;

                    case "GATT_ReadCharValue":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReadCharValue, TxDataOut.CmdType.General, null);
                        return;

                    case "GATT_ReadUsingCharUUID":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReadUsingCharUUID);
                        return;

                    case "GATT_ReadLongCharValue":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReadLongCharValue, TxDataOut.CmdType.General, null);
                        return;

                    case "GATT_ReadMultiCharValues":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReadMultiCharValues);
                        return;

                    case "GATT_WriteNoRsp":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_WriteNoRsp);
                        return;

                    case "GATT_SignedWriteNoRsp":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_SignedWriteNoRsp);
                        return;

                    case "GATT_WriteCharValue":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_WriteCharValue, null);
                        return;

                    case "GATT_WriteLongCharValue":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_WriteLongCharValue, null, null);
                        return;

                    case "GATT_ReliableWrites":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReliableWrites);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "GATT_ReadCharDesc":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReadCharDesc);
                        return;

                    case "GATT_ReadLongCharDesc":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_ReadLongCharDesc);
                        return;

                    case "GATT_WriteCharDesc":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_WriteCharDesc);
                        return;

                    case "GATT_WriteLongCharDesc":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_WriteLongCharDesc);
                        return;

                    case "GATT_Notification":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_Notification);
                        return;

                    case "GATT_Indication":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_Indication);
                        return;

                    case "GATT_AddService":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_AddService);
                        return;

                    case "GATT_DelService":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_DelService);
                        return;

                    case "GATT_AddAttribute":
                        this.devForm.sendCmds.SendGATT(this.devForm.GATT_AddAttribute);
                        return;

                    case "GAP_DeviceInit":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_DeviceInit);
                        return;

                    case "GAP_ConfigDeviceAddr":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_ConfigDeviceAddr);
                        return;

                    case "GAP_DeviceDiscoveryRequest":
                        this.ShowProgress(true);
                        this.devForm.StartTimer(DeviceForm.EventType.Scan);
                        this.discoverConnectStatus = DiscoverConnectStatus.Scan;
                        this.DiscoverConnectUserInputControl();
                        this.ResetSlaveDevices();
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_DeviceDiscoveryRequest);
                        return;

                    case "GAP_DeviceDiscoveryCancel":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_DeviceDiscoveryCancel);
                        return;

                    case "GAP_MakeDiscoverable":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_MakeDiscoverable);
                        return;

                    case "GAP_UpdateAdvertisingData":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_UpdateAdvertisingData);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "GAP_EndDiscoverable":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_EndDiscoverable);
                        return;

                    case "GAP_EstablishLinkRequest":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_EstablishLinkRequest);
                        return;

                    case "GAP_TerminateLinkRequest":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_TerminateLinkRequest);
                        return;

                    case "GAP_Authenticate":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_Authenticate);
                        return;

                    case "GAP_PasskeyUpdate":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_PasskeyUpdate);
                        return;

                    case "GAP_SlaveSecurityRequest":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_SlaveSecurityRequest);
                        return;

                    case "GAP_Signable":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_Signable);
                        return;

                    case "GAP_Bond":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_Bond);
                        return;

                    case "GAP_TerminateAuth":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_TerminateAuth);
                        return;

                    case "GAP_UpdateLinkParamReq":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_UpdateLinkParamReq);
                        return;

                    case "GAP_SetParam":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_SetParam);
                        return;

                    case "GAP_GetParam":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_GetParam);
                        return;

                    case "GAP_ResolvePrivateAddr":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_ResolvePrivateAddr);
                        return;

                    case "GAP_SetAdvToken":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_SetAdvToken);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "GAP_RemoveAdvToken":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_RemoveAdvToken);
                        return;

                    case "GAP_UpdateAdvTokens":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_UpdateAdvTokens);
                        return;

                    case "GAP_BondSetParam":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_BondSetParam);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "GAP_BondGetParam":
                        this.devForm.sendCmds.SendGAP(this.devForm.GAP_BondGetParam);
                        return;

                    case "UTIL_Reset":
                        this.devForm.sendCmds.SendUTIL(this.devForm.UTIL_Reset);
                        return;

                    case "UTIL_NVRead":
                        this.devForm.sendCmds.SendUTIL(this.devForm.UTIL_NVRead);
                        return;

                    case "UTIL_NVWrite":
                        this.devForm.sendCmds.SendUTIL(this.devForm.UTIL_NVWrite);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "UTIL_ForceBoot":
                        str = "This Command Will Invalidate The Image On The Device\nDo You Wish To Send The Command?\n";
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Warning, str);
                        if (this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.OkCancel, str) != MsgBox.MsgResult.OK)
                        {
                            goto Label_167A;
                        }
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, "User Selected OK\n");
                        if (!this.devForm.sendCmds.SendUTIL(this.devForm.UTIL_ForceBoot))
                        {
                            break;
                        }
                        str = "Command Sent\n";
                        str = ((((str + "\n") + "There Should Be No Response To This Command\n" + "(If There Is A Response -> There Is No BootLoader On The Device)\n") + "\n" + "After Noting That There Is No Response\n") + "Start The 'Serial Bootloader' Tool To Download The New Firmware\n\n" + "You May Close BTool Now\n") + "<Or>\n" + "You Can 'Close Device' Then 'Start Device' In BTool After The Serial Bootloader Is Complete\n";
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Warning, str);
                        this.msgBox.UserMsgBox(MsgBox.MsgTypes.Info, str);
                        return;

                    case "HCI_ReadRSSI":
                        this.devForm.sendCmds.SendHCIOther(this.devForm.HCIOther_ReadRSSI);
                        return;

                    case "HCI_LEClearWhiteList":
                        this.devForm.sendCmds.SendHCIOther(this.devForm.HCIOther_LEClearWhiteList);
                        return;

                    case "HCI_LEAddDeviceToWhiteList":
                        this.devForm.sendCmds.SendHCIOther(this.devForm.HCIOther_LEAddDeviceToWhiteList);
                        return;

                    case "HCI_LERemoveDeviceFromWhiteList":
                        this.devForm.sendCmds.SendHCIOther(this.devForm.HCIOther_LERemoveDeviceFromWhiteList);
                        return;

                    case "HCI_LEConnectionUpdate":
                        this.devForm.sendCmds.SendHCIOther(this.devForm.HCIOther_LEConnectionUpdate);
                        return;

                    case "MISC_GenericCommand":
                        this.devForm.sendCmds.SendMISC(this.devForm.MISC_GenericCommand);
                        this.pgAdvCmds.Refresh();
                        return;

                    case "MISC_RawTxMessage":
                        this.devForm.sendCmds.SendMISC(this.devForm.MISC_RawTxMessage);
                        return;

                    case "Send All Msgs":
                        this.devForm.SendAllMsgs();
                        return;

                    case "Send All Events":
                        this.devForm.SendEventWaves(false);
                        return;

                    case "Send All Forever":
                        this.devForm.SendAllForever();
                        return;

                    case "Send Attr Data Cmds":
                        this.devForm.SendAttrDataCmds();
                        return;

                    case "Test Case":
                        this.devForm.TestCase();
                        return;

                    default:
                        goto Label_17DE;
                }
                str = "Command Failed\nSerial Bootloader Setup Failed\n";
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str);
                this.msgBox.UserMsgBox(MsgBox.MsgTypes.Error, str);
                return;
            Label_167A:
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, "User Selected Cancel\n");
                str = "Operation Aborted\n";
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, str);
                this.msgBox.UserMsgBox(MsgBox.MsgTypes.Info, str);
                return;
            Label_17DE:
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, "Select A Command First\n");
            }
            catch
            {
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, "Select A Command First\n");
            }
        }

        public void UsePasskeySecurity(HCICmds.GAP_UiOutput state)
        {
            if (state == HCICmds.GAP_UiOutput.DONT_DISPLAY_PASSCODE)
            {
                this.tbPasskey.PasswordChar = '*';
            }
            else
            {
                this.tbPasskey.PasswordChar = '\0';
            }
        }

        public void UserTabAccess(bool tabAcccess)
        {
            this.tcDeviceTabs.Enabled = tabAcccess;
        }

        private bool WriteCsv(string pathFileNameStr, List<CsvData> csvData)
        {
            bool flag = false;
            try
            {
                if ((csvData == null) || (csvData.Count <= 0))
                {
                    throw new ArgumentException(string.Format("There Is No Data To Save\n", new object[0]));
                }
                using (StreamWriter writer = new StreamWriter(pathFileNameStr))
                {
                    int count = csvData.Count;
                    string str2 = string.Empty;
                    CsvData data = new CsvData();
                    for (int i = 0; i < count; i++)
                    {
                        data = csvData[i];
                        str2 = string.Format("{0:S},{1:S},{2:S},{3:S},{4:S}", new object[] { data.addr, data.auth, data.ltk, data.div, data.rand });
                        writer.WriteLine(str2);
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Cannot Write The CSV File\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = true;
            }
            return flag;
        }

        private class CsvData
        {
            private string _addr = string.Empty;
            private string _auth = string.Empty;
            private string _div = string.Empty;
            private string _ltk = string.Empty;
            private string _rand = string.Empty;

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

            public string auth
            {
                get
                {
                    return this._auth;
                }
                set
                {
                    this._auth = value;
                }
            }

            public string div
            {
                get
                {
                    return this._div;
                }
                set
                {
                    this._div = value;
                }
            }

            public string ltk
            {
                get
                {
                    return this._ltk;
                }
                set
                {
                    this._ltk = value;
                }
            }

            public string rand
            {
                get
                {
                    return this._rand;
                }
                set
                {
                    this._rand = value;
                }
            }
        }

        public enum DeviceTabs
        {
            DiscoverConnect,
            ReadWrite,
            PairingBonding,
            AdvCommands
        }

        public enum DiscoverConnectStatus
        {
            Idle,
            Scan,
            ScanCancel,
            GetSet,
            Establish,
            EstablishCancel,
            Terminate
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LinkSlave
        {
            public byte[] slaveBDA;
            public string addrBDA;
            public HCICmds.GAP_AddrType addrType;
        }

        public enum PairingStatus
        {
            Empty,
            NotConnected,
            NotPaired,
            PasskeyNeeded,
            DevicesPairedBonded,
            DevicesPaired,
            PasskeyIncorrect,
            ConnectionTimedOut
        }

        private enum ReadSubProc
        {
            ReadCharacteristicValueDescriptor,
            ReadUsingCharacteristicUuid,
            ReadMultipleCharacteristicValues,
            DiscoverCharacteristicByUuid
        }

        private delegate void SetMaxConnectionIntervalDelegate(uint interval);

        private delegate void SetMinConnectionIntervalDelegate(uint interval);

        private delegate void SetSlaveLatencyDelegate(uint latency);

        public delegate void SetSupervisionTimeoutDelegate(uint timeout);
    }
}

