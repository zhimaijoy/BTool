﻿namespace BTool
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class AttrDataItemForm : Form
    {
        public AttrDataItemChangedDelegate AttrDataItemChangedCallback;
        private AttrDataUtils attrDataUtils;
        private Button btnReadValue;
        private Button btnWriteValue;
        private ComboBox cbDataType;
        private IContainer components;
        private DataAttr dataAttr = new DataAttr();
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public DisplayMsgDelegate DisplayMsgCallback;
        private Mutex formDataAccess = new Mutex();
        private DataAttr gattWriteDataAttr = new DataAttr();
        private GroupBox gbProperties;
        private GroupBox gbSummary;
        private GroupBox gbUuid;
        private GroupBox gbValue;
        private string key = string.Empty;
        private ValueDisplay lastValueDisplay;
        private bool lastValueDisplaySet;
        private Label lblAuthenticatedSignedWrites;
        private Label lblBroadcast;
        private Label lblConnHnd;
        private Label lblExtendedProperties;
        private Label lblHandle;
        private Label lblIndicate;
        private Label lblNotify;
        private Label lblProperties;
        private Label lblProperties_gb;
        private Label lblRead;
        private Label lblSummary_gb;
        private Label lblUuid;
        private Label lblUuid_gb;
        private Label lblUuidDesc;
        private Label lblValue;
        private Label lblValue_gb;
        private Label lblValueDesc;
        private Label lblValueEdit;
        private Label lblWrite;
        private Label lblWriteWithoutResponse;
        private const string moduleName = "AttrDataItemForm";
        private MonoUtils monoUtils = new MonoUtils();
        private MsgBox msgBox = new MsgBox();
        private SendCmds sendCmds;
        private TextBox tbConnHnd;
        private TextBox tbHandle;
        private TextBox tbProperties;
        private TextBox tbUuid;
        private TextBox tbUuidDesc;
        private TextBox tbValue;
        private TextBox tbValueDesc;
        private TableLayoutPanel tlpProperties;
        private TableLayoutPanel tlpPropertiesBits;
        private TableLayoutPanel tlpSummary;
        private TableLayoutPanel tlpUuid_1;
        private TableLayoutPanel tlpUuid_2;
        private TableLayoutPanel tlpValue_1;
        private TableLayoutPanel tlpValue_2;
        private TableLayoutPanel tlpValue_3;

        public AttrDataItemForm(DeviceForm deviceForm)
        {
            this.InitializeComponent();
            this.devForm = deviceForm;
            this.sendCmds = new SendCmds(deviceForm);
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public void AttErrorRsp(BTool.AttErrorRsp.RspInfo rspInfo)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new AttErrorRspDelegate(this.AttErrorRsp), new object[] { rspInfo });
                }
                catch
                {
                }
            }
            else
            {
                this.ClearRspDelegates();
                string msg = "ATT Command Failed\n";
                if (rspInfo.aTT_ErrorRsp != null)
                {
                    msg = ((msg + "Command = " + this.devUtils.GetHciReqOpCodeStr(rspInfo.aTT_ErrorRsp.reqOpCode) + "\n") + "Handle = 0x" + rspInfo.aTT_ErrorRsp.handle.ToString("X4") + "\n") + "Error = " + this.devUtils.GetErrorStatusStr(rspInfo.aTT_ErrorRsp.errorCode, "") + "\n";
                }
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                this.RestoreFormInput();
            }
        }

        public void AttExecuteWriteRsp(BTool.AttExecuteWriteRsp.RspInfo rspInfo)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new AttExecuteWriteRspDelegate(this.AttExecuteWriteRsp), new object[] { rspInfo });
                }
                catch
                {
                }
            }
            else
            {
                this.ClearRspDelegates();
                if (!rspInfo.success)
                {
                    string msg = "Att Execute Write Command Failed\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
                else if (rspInfo.header.eventStatus != 0)
                {
                    string str2 = "Att Execute Write Command Failed\n";
                    str2 = str2 + "Status = " + this.devUtils.GetStatusStr(rspInfo.header.eventStatus) + "\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                }
                else
                {
                    this.formDataAccess.WaitOne();
                    this.gattWriteDataAttr.dataUpdate = true;
                    if (!this.attrDataUtils.UpdateAttrDictItem(this.gattWriteDataAttr))
                    {
                        string str3 = "Att Write Execute Command Data Update Failed\nAttribute Form Data For This Items Did Not Update\n";
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Warning, str3);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, str3);
                    }
                    else if (this.AttrDataItemChangedCallback != null)
                    {
                        this.AttrDataItemChangedCallback();
                    }
                    this.formDataAccess.ReleaseMutex();
                }
                this.RestoreFormInput();
            }
        }

        public void AttPrepareWriteRsp(BTool.AttPrepareWriteRsp.RspInfo rspInfo)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new AttPrepareWriteRspDelegate(this.AttPrepareWriteRsp), new object[] { rspInfo });
                }
                catch
                {
                }
            }
            else
            {
                this.ClearRspDelegates();
                if (!rspInfo.success)
                {
                    string msg = "Att Prepare Write Command Failed\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
                else
                {
                    string str2 = "Att Prepare Write Command Failed\n";
                    str2 = str2 + "Status = " + this.devUtils.GetStatusStr(rspInfo.header.eventStatus) + "\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                }
                this.RestoreFormInput();
            }
        }

        private void AttrDataItemForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ClearRspDelegates();
        }

        private void AttrDataItemForm_FormLoad(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.tbConnHnd, "Connection Handle Value");
            tip.SetToolTip(this.tbHandle, "Handle Value");
            tip.SetToolTip(this.tbUuid, "UUID Value");
            tip.SetToolTip(this.tbUuidDesc, "UUID Description");
            tip.SetToolTip(this.tbValue, "Value Entry");
            tip.SetToolTip(this.cbDataType, "Value Data Type");
            tip.SetToolTip(this.btnReadValue, "Read Value From Device");
            tip.SetToolTip(this.btnWriteValue, "Write Value From Device");
            tip.SetToolTip(this.tbValueDesc, "Value Description");
            tip.SetToolTip(this.tbProperties, "Short Abbreviations Of Each Bit Set\nFollowed By Property Value In Hex");
            string str = "\n(Green = Bit Set)\n(Red = Bit Clear)";
            tip.SetToolTip(this.lblBroadcast, "Broadcast Bit -> Bcst 0x01" + str);
            tip.SetToolTip(this.lblRead, "Read Bit -> Rd 0x02" + str);
            tip.SetToolTip(this.lblWriteWithoutResponse, "WriteWithoutResponse Bit -> Wwr 0x04" + str);
            tip.SetToolTip(this.lblWrite, "Write Bit -> Wr 0x08" + str);
            tip.SetToolTip(this.lblNotify, "Notify Bit -> Nfy 0x10" + str);
            tip.SetToolTip(this.lblIndicate, "Indicate Bit -> Ind 0x20" + str);
            tip.SetToolTip(this.lblAuthenticatedSignedWrites, "AuthenticatedSignedWrites Bit -> Asw 0x40" + str);
            tip.SetToolTip(this.lblExtendedProperties, "ExtendedProperties Bit -> Exp 0x80" + str);
            this.LoadUserSettings();
            this.monoUtils.SetMaximumSize(this);
        }

        public void AttReadBlobRsp(BTool.AttReadBlobRsp.RspInfo rspInfo)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new AttReadBlobRspDelegate(this.AttReadBlobRsp), new object[] { rspInfo });
                }
                catch
                {
                }
            }
            else
            {
                this.ClearRspDelegates();
                if (!rspInfo.success)
                {
                    string msg = "Att Read Blob Command Failed\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
                else if (rspInfo.header.eventStatus != 0x1a)
                {
                    string str2 = "Att Read Blob Command Failed\n";
                    str2 = str2 + "Status = " + this.devUtils.GetStatusStr(rspInfo.header.eventStatus) + "\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                }
                else
                {
                    this.LoadData(this.key);
                }
                this.RestoreFormInput();
            }
        }

        private void btnReadValue_Click(object sender, EventArgs e)
        {
            this.formDataAccess.WaitOne();
            this.devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = new ExtCmdStatusDelegate(this.ExtCmdStatus);
            this.devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = new AttErrorRspDelegate(this.AttErrorRsp);
            this.devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = new AttReadBlobRspDelegate(this.AttReadBlobRsp);
            HCICmds.GATTCmds.GATT_ReadLongCharValue value2 = new HCICmds.GATTCmds.GATT_ReadLongCharValue();
            value2.connHandle = this.dataAttr.connHandle;
            value2.handle = this.dataAttr.handle;
            if (this.sendCmds.SendGATT(value2, TxDataOut.CmdType.General, new BTool.SendCmdResult(this.SendCmdResult)))
            {
                base.Enabled = false;
            }
            else
            {
                this.ClearRspDelegates();
            }
            this.formDataAccess.ReleaseMutex();
        }

        private void btnWriteValue_Click(object sender, EventArgs e)
        {
            this.formDataAccess.WaitOne();
            if ((this.tbValue.Text == null) || (this.tbValue.Text == string.Empty))
            {
                string msg = "A Value Must Be Entered To Perform A Write\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
            }
            else
            {
                string outStr = string.Empty;
                ValueDisplay valueDsp = this.dataAttr.valueDsp;
                ValueDisplay hex = ValueDisplay.Hex;
                if (this.lastValueDisplaySet)
                {
                    valueDsp = this.lastValueDisplay;
                }
                if (this.devUtils.ConvertDisplayTypes(valueDsp, this.tbValue.Text, ref hex, ref outStr, true))
                {
                    string str = this.devUtils.HexStr2UserDefinedStr(outStr, SharedAppObjs.StringType.HEX);
                    switch (str)
                    {
                        case null:
                        case "":
                        {
                            string str4 = "Value Data Cannot Be Converted To Hex For Write Command\n";
                            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, str4);
                            goto Label_0313;
                        }
                    }
                    this.devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = new ExtCmdStatusDelegate(this.ExtCmdStatus);
                    this.devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = new AttErrorRspDelegate(this.AttErrorRsp);
                    this.devForm.threadMgr.rspDataIn.attPrepareWriteRsp.AttPrepareWriteRspCallback = new AttPrepareWriteRspDelegate(this.AttPrepareWriteRsp);
                    this.devForm.threadMgr.rspDataIn.attExecuteWriteRsp.AttExecuteWriteRspCallback = new AttExecuteWriteRspDelegate(this.AttExecuteWriteRsp);
                    HCICmds.GATTCmds.GATT_WriteLongCharValue value2 = new HCICmds.GATTCmds.GATT_WriteLongCharValue();
                    value2.connHandle = this.dataAttr.connHandle;
                    value2.handle = this.dataAttr.handle;
                    value2.value = str;
                    this.gattWriteDataAttr = this.dataAttr;
                    this.gattWriteDataAttr.value = str;
                    int length = 0;
                    if (AttrData.writeLimits.maxPacketSize < (AttrData.writeLimits.maxNumPreparedWrites * 0x12))
                    {
                        length = AttrData.writeLimits.maxPacketSize;
                    }
                    else
                    {
                        length = AttrData.writeLimits.maxNumPreparedWrites * 0x12;
                    }
                    byte[] sourceArray = this.devUtils.String2Bytes_LSBMSB(str, 0x10);
                    if (sourceArray == null)
                    {
                        this.sendCmds.DisplayInvalidValue(value2.value);
                    }
                    else
                    {
                        int num2 = 0;
                        int num3 = sourceArray.Length;
                        for (int i = 0; i < sourceArray.Length; i += num2)
                        {
                            byte[] destinationArray = null;
                            if (num3 > length)
                            {
                                destinationArray = new byte[length];
                                Array.Copy(sourceArray, i, destinationArray, 0, length);
                            }
                            else
                            {
                                destinationArray = new byte[num3];
                                Array.Copy(sourceArray, i, destinationArray, 0, num3);
                            }
                            value2.value = string.Empty;
                            value2.offset = (ushort) i;
                            if (this.sendCmds.SendGATT(value2, destinationArray, new BTool.SendCmdResult(this.SendCmdResult)))
                            {
                                base.Enabled = false;
                            }
                            else
                            {
                                string str5 = "GATT_WriteLongCharValue Command Failed\n";
                                if (i > 0)
                                {
                                    str5 = str5 + "Multi-Part Write Sequenece Error\n" + "All Requested Data May Not Have Been Written To The Device\n";
                                }
                                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str5);
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str5);
                                this.ClearRspDelegates();
                                break;
                            }
                            num2 = destinationArray.Length;
                            num3 -= destinationArray.Length;
                        }
                    }
                }
            }
        Label_0313:
            this.formDataAccess.ReleaseMutex();
        }

        private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.formDataAccess.WaitOne();
            ComboBox box = sender as ComboBox;
            string outStr = string.Empty;
            ValueDisplay selectedIndex = (ValueDisplay) box.SelectedIndex;
            bool flag = this.devUtils.ConvertDisplayTypes(this.lastValueDisplay, this.tbValue.Text, ref selectedIndex, ref outStr, true);
            box.SelectedIndex = (int) selectedIndex;
            if (flag)
            {
                this.lastValueDisplay = (ValueDisplay) box.SelectedIndex;
                this.lastValueDisplaySet = true;
            }
            else
            {
                box.SelectedIndex = (int) this.lastValueDisplay;
            }
            this.tbValue.Text = outStr;
            this.formDataAccess.ReleaseMutex();
        }

        private void ClearRspDelegates()
        {
            this.devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = null;
            this.devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = null;
            this.devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = null;
            this.devForm.threadMgr.rspDataIn.attExecuteWriteRsp.AttExecuteWriteRspCallback = null;
            this.devForm.threadMgr.rspDataIn.attPrepareWriteRsp.AttPrepareWriteRspCallback = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ExtCmdStatus(BTool.ExtCmdStatus.RspInfo rspInfo)
        {
            this.ClearRspDelegates();
            if (!rspInfo.success)
            {
                string msg = "Command Failed\n";
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            else
            {
                string str2 = "Command Failed\n";
                str2 = (str2 + "Status = " + this.devUtils.GetStatusStr(rspInfo.header.eventStatus) + "\n") + "Event = " + this.devUtils.GetOpCodeName(rspInfo.header.eventCode) + "\n";
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
            }
            this.RestoreFormInput();
        }

        private void InitializeComponent()
        {
			this.gbSummary = new System.Windows.Forms.GroupBox();
			this.tlpSummary = new System.Windows.Forms.TableLayoutPanel();
			this.tbHandle = new System.Windows.Forms.TextBox();
			this.lblHandle = new System.Windows.Forms.Label();
			this.tbConnHnd = new System.Windows.Forms.TextBox();
			this.lblConnHnd = new System.Windows.Forms.Label();
			this.lblSummary_gb = new System.Windows.Forms.Label();
			this.gbUuid = new System.Windows.Forms.GroupBox();
			this.tlpUuid_2 = new System.Windows.Forms.TableLayoutPanel();
			this.tbUuidDesc = new System.Windows.Forms.TextBox();
			this.lblUuidDesc = new System.Windows.Forms.Label();
			this.tlpUuid_1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbUuid = new System.Windows.Forms.TextBox();
			this.lblUuid = new System.Windows.Forms.Label();
			this.lblUuid_gb = new System.Windows.Forms.Label();
			this.gbValue = new System.Windows.Forms.GroupBox();
			this.tlpValue_3 = new System.Windows.Forms.TableLayoutPanel();
			this.tbValueDesc = new System.Windows.Forms.TextBox();
			this.lblValueDesc = new System.Windows.Forms.Label();
			this.tlpValue_2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblValueEdit = new System.Windows.Forms.Label();
			this.cbDataType = new System.Windows.Forms.ComboBox();
			this.btnReadValue = new System.Windows.Forms.Button();
			this.btnWriteValue = new System.Windows.Forms.Button();
			this.tlpValue_1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.lblValue_gb = new System.Windows.Forms.Label();
			this.gbProperties = new System.Windows.Forms.GroupBox();
			this.tlpPropertiesBits = new System.Windows.Forms.TableLayoutPanel();
			this.lblBroadcast = new System.Windows.Forms.Label();
			this.lblWriteWithoutResponse = new System.Windows.Forms.Label();
			this.lblNotify = new System.Windows.Forms.Label();
			this.lblAuthenticatedSignedWrites = new System.Windows.Forms.Label();
			this.lblRead = new System.Windows.Forms.Label();
			this.lblWrite = new System.Windows.Forms.Label();
			this.lblIndicate = new System.Windows.Forms.Label();
			this.lblExtendedProperties = new System.Windows.Forms.Label();
			this.tlpProperties = new System.Windows.Forms.TableLayoutPanel();
			this.tbProperties = new System.Windows.Forms.TextBox();
			this.lblProperties = new System.Windows.Forms.Label();
			this.lblProperties_gb = new System.Windows.Forms.Label();
			this.gbSummary.SuspendLayout();
			this.tlpSummary.SuspendLayout();
			this.gbUuid.SuspendLayout();
			this.tlpUuid_2.SuspendLayout();
			this.tlpUuid_1.SuspendLayout();
			this.gbValue.SuspendLayout();
			this.tlpValue_3.SuspendLayout();
			this.tlpValue_2.SuspendLayout();
			this.tlpValue_1.SuspendLayout();
			this.gbProperties.SuspendLayout();
			this.tlpPropertiesBits.SuspendLayout();
			this.tlpProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSummary
			// 
			this.gbSummary.Controls.Add(this.tlpSummary);
			this.gbSummary.Controls.Add(this.lblSummary_gb);
			this.gbSummary.Location = new System.Drawing.Point(17, 13);
			this.gbSummary.Name = "gbSummary";
			this.gbSummary.Size = new System.Drawing.Size(433, 63);
			this.gbSummary.TabIndex = 0;
			this.gbSummary.TabStop = false;
			// 
			// tlpSummary
			// 
			this.tlpSummary.ColumnCount = 4;
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.Controls.Add(this.tbHandle, 3, 0);
			this.tlpSummary.Controls.Add(this.lblHandle, 2, 0);
			this.tlpSummary.Controls.Add(this.tbConnHnd, 1, 0);
			this.tlpSummary.Controls.Add(this.lblConnHnd, 0, 0);
			this.tlpSummary.Location = new System.Drawing.Point(18, 19);
			this.tlpSummary.Name = "tlpSummary";
			this.tlpSummary.RowCount = 1;
			this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSummary.Size = new System.Drawing.Size(401, 33);
			this.tlpSummary.TabIndex = 1;
			// 
			// tbHandle
			// 
			this.tbHandle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHandle.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbHandle.Location = new System.Drawing.Point(303, 6);
			this.tbHandle.Name = "tbHandle";
			this.tbHandle.ReadOnly = true;
			this.tbHandle.Size = new System.Drawing.Size(95, 20);
			this.tbHandle.TabIndex = 5;
			// 
			// lblHandle
			// 
			this.lblHandle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHandle.AutoSize = true;
			this.lblHandle.Location = new System.Drawing.Point(203, 10);
			this.lblHandle.Name = "lblHandle";
			this.lblHandle.Size = new System.Drawing.Size(94, 13);
			this.lblHandle.TabIndex = 4;
			this.lblHandle.Text = "Handle:";
			this.lblHandle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbConnHnd
			// 
			this.tbConnHnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbConnHnd.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbConnHnd.Location = new System.Drawing.Point(103, 6);
			this.tbConnHnd.Name = "tbConnHnd";
			this.tbConnHnd.ReadOnly = true;
			this.tbConnHnd.Size = new System.Drawing.Size(94, 20);
			this.tbConnHnd.TabIndex = 3;
			// 
			// lblConnHnd
			// 
			this.lblConnHnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblConnHnd.AutoSize = true;
			this.lblConnHnd.Location = new System.Drawing.Point(3, 10);
			this.lblConnHnd.Name = "lblConnHnd";
			this.lblConnHnd.Size = new System.Drawing.Size(94, 13);
			this.lblConnHnd.TabIndex = 2;
			this.lblConnHnd.Text = "Connection:";
			this.lblConnHnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSummary_gb
			// 
			this.lblSummary_gb.AutoSize = true;
			this.lblSummary_gb.Location = new System.Drawing.Point(189, 0);
			this.lblSummary_gb.Name = "lblSummary_gb";
			this.lblSummary_gb.Size = new System.Drawing.Size(50, 13);
			this.lblSummary_gb.TabIndex = 0;
			this.lblSummary_gb.Text = "Summary";
			// 
			// gbUuid
			// 
			this.gbUuid.Controls.Add(this.tlpUuid_2);
			this.gbUuid.Controls.Add(this.tlpUuid_1);
			this.gbUuid.Controls.Add(this.lblUuid_gb);
			this.gbUuid.Location = new System.Drawing.Point(17, 82);
			this.gbUuid.Name = "gbUuid";
			this.gbUuid.Size = new System.Drawing.Size(433, 111);
			this.gbUuid.TabIndex = 1;
			this.gbUuid.TabStop = false;
			// 
			// tlpUuid_2
			// 
			this.tlpUuid_2.ColumnCount = 2;
			this.tlpUuid_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpUuid_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpUuid_2.Controls.Add(this.tbUuidDesc, 1, 0);
			this.tlpUuid_2.Controls.Add(this.lblUuidDesc, 0, 0);
			this.tlpUuid_2.Location = new System.Drawing.Point(18, 49);
			this.tlpUuid_2.Name = "tlpUuid_2";
			this.tlpUuid_2.RowCount = 1;
			this.tlpUuid_2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpUuid_2.Size = new System.Drawing.Size(401, 51);
			this.tlpUuid_2.TabIndex = 2;
			// 
			// tbUuidDesc
			// 
			this.tbUuidDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUuidDesc.Location = new System.Drawing.Point(103, 3);
			this.tbUuidDesc.Multiline = true;
			this.tbUuidDesc.Name = "tbUuidDesc";
			this.tbUuidDesc.ReadOnly = true;
			this.tbUuidDesc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbUuidDesc.Size = new System.Drawing.Size(295, 45);
			this.tbUuidDesc.TabIndex = 4;
			this.tbUuidDesc.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7";
			// 
			// lblUuidDesc
			// 
			this.lblUuidDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblUuidDesc.AutoSize = true;
			this.lblUuidDesc.Location = new System.Drawing.Point(3, 19);
			this.lblUuidDesc.Name = "lblUuidDesc";
			this.lblUuidDesc.Size = new System.Drawing.Size(94, 13);
			this.lblUuidDesc.TabIndex = 5;
			this.lblUuidDesc.Text = "UUID Description:";
			this.lblUuidDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tlpUuid_1
			// 
			this.tlpUuid_1.ColumnCount = 2;
			this.tlpUuid_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpUuid_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpUuid_1.Controls.Add(this.tbUuid, 1, 0);
			this.tlpUuid_1.Controls.Add(this.lblUuid, 0, 0);
			this.tlpUuid_1.Location = new System.Drawing.Point(18, 19);
			this.tlpUuid_1.Name = "tlpUuid_1";
			this.tlpUuid_1.RowCount = 1;
			this.tlpUuid_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpUuid_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tlpUuid_1.Size = new System.Drawing.Size(401, 28);
			this.tlpUuid_1.TabIndex = 1;
			// 
			// tbUuid
			// 
			this.tbUuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUuid.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbUuid.Location = new System.Drawing.Point(103, 4);
			this.tbUuid.Name = "tbUuid";
			this.tbUuid.ReadOnly = true;
			this.tbUuid.Size = new System.Drawing.Size(295, 20);
			this.tbUuid.TabIndex = 4;
			this.tbUuid.Text = "0xBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB";
			// 
			// lblUuid
			// 
			this.lblUuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblUuid.AutoSize = true;
			this.lblUuid.Location = new System.Drawing.Point(3, 7);
			this.lblUuid.Name = "lblUuid";
			this.lblUuid.Size = new System.Drawing.Size(94, 13);
			this.lblUuid.TabIndex = 3;
			this.lblUuid.Text = "UUID:";
			this.lblUuid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblUuid_gb
			// 
			this.lblUuid_gb.AutoSize = true;
			this.lblUuid_gb.Location = new System.Drawing.Point(195, 0);
			this.lblUuid_gb.Name = "lblUuid_gb";
			this.lblUuid_gb.Size = new System.Drawing.Size(34, 13);
			this.lblUuid_gb.TabIndex = 0;
			this.lblUuid_gb.Text = "UUID";
			// 
			// gbValue
			// 
			this.gbValue.Controls.Add(this.tlpValue_3);
			this.gbValue.Controls.Add(this.tlpValue_2);
			this.gbValue.Controls.Add(this.tlpValue_1);
			this.gbValue.Controls.Add(this.lblValue_gb);
			this.gbValue.Location = new System.Drawing.Point(17, 200);
			this.gbValue.Name = "gbValue";
			this.gbValue.Size = new System.Drawing.Size(433, 196);
			this.gbValue.TabIndex = 2;
			this.gbValue.TabStop = false;
			// 
			// tlpValue_3
			// 
			this.tlpValue_3.ColumnCount = 2;
			this.tlpValue_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpValue_3.Controls.Add(this.tbValueDesc, 1, 0);
			this.tlpValue_3.Controls.Add(this.lblValueDesc, 0, 0);
			this.tlpValue_3.Location = new System.Drawing.Point(21, 132);
			this.tlpValue_3.Name = "tlpValue_3";
			this.tlpValue_3.RowCount = 1;
			this.tlpValue_3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpValue_3.Size = new System.Drawing.Size(398, 52);
			this.tlpValue_3.TabIndex = 3;
			// 
			// tbValueDesc
			// 
			this.tbValueDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbValueDesc.Location = new System.Drawing.Point(102, 3);
			this.tbValueDesc.Multiline = true;
			this.tbValueDesc.Name = "tbValueDesc";
			this.tbValueDesc.ReadOnly = true;
			this.tbValueDesc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbValueDesc.Size = new System.Drawing.Size(293, 46);
			this.tbValueDesc.TabIndex = 8;
			this.tbValueDesc.Text = "1\r\n2\r\n3\r\n4\r\n5";
			// 
			// lblValueDesc
			// 
			this.lblValueDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblValueDesc.AutoSize = true;
			this.lblValueDesc.Location = new System.Drawing.Point(3, 19);
			this.lblValueDesc.Name = "lblValueDesc";
			this.lblValueDesc.Size = new System.Drawing.Size(93, 13);
			this.lblValueDesc.TabIndex = 6;
			this.lblValueDesc.Text = "Value Description:";
			this.lblValueDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tlpValue_2
			// 
			this.tlpValue_2.ColumnCount = 4;
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.Controls.Add(this.lblValueEdit, 0, 0);
			this.tlpValue_2.Controls.Add(this.cbDataType, 1, 0);
			this.tlpValue_2.Controls.Add(this.btnReadValue, 2, 0);
			this.tlpValue_2.Controls.Add(this.btnWriteValue, 3, 0);
			this.tlpValue_2.Location = new System.Drawing.Point(21, 100);
			this.tlpValue_2.Name = "tlpValue_2";
			this.tlpValue_2.RowCount = 1;
			this.tlpValue_2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpValue_2.Size = new System.Drawing.Size(398, 28);
			this.tlpValue_2.TabIndex = 2;
			// 
			// lblValueEdit
			// 
			this.lblValueEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblValueEdit.AutoSize = true;
			this.lblValueEdit.Location = new System.Drawing.Point(3, 7);
			this.lblValueEdit.Name = "lblValueEdit";
			this.lblValueEdit.Size = new System.Drawing.Size(93, 13);
			this.lblValueEdit.TabIndex = 6;
			this.lblValueEdit.Text = "Value Type:";
			this.lblValueEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbDataType
			// 
			this.cbDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbDataType.FormattingEnabled = true;
			this.cbDataType.Items.AddRange(new object[] {
            "Hex",
            "Decimal",
            "ASCII"});
			this.cbDataType.Location = new System.Drawing.Point(102, 3);
			this.cbDataType.Name = "cbDataType";
			this.cbDataType.Size = new System.Drawing.Size(93, 21);
			this.cbDataType.TabIndex = 7;
			this.cbDataType.SelectedIndexChanged += new System.EventHandler(this.cbDataType_SelectedIndexChanged);
			// 
			// btnReadValue
			// 
			this.btnReadValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReadValue.Location = new System.Drawing.Point(201, 3);
			this.btnReadValue.Name = "btnReadValue";
			this.btnReadValue.Size = new System.Drawing.Size(93, 22);
			this.btnReadValue.TabIndex = 9;
			this.btnReadValue.Text = "Read Value";
			this.btnReadValue.UseVisualStyleBackColor = true;
			this.btnReadValue.Click += new System.EventHandler(this.btnReadValue_Click);
			// 
			// btnWriteValue
			// 
			this.btnWriteValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWriteValue.Enabled = false;
			this.btnWriteValue.Location = new System.Drawing.Point(300, 3);
			this.btnWriteValue.Name = "btnWriteValue";
			this.btnWriteValue.Size = new System.Drawing.Size(95, 22);
			this.btnWriteValue.TabIndex = 8;
			this.btnWriteValue.Text = "Write Value";
			this.btnWriteValue.UseVisualStyleBackColor = true;
			this.btnWriteValue.Click += new System.EventHandler(this.btnWriteValue_Click);
			// 
			// tlpValue_1
			// 
			this.tlpValue_1.ColumnCount = 2;
			this.tlpValue_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpValue_1.Controls.Add(this.tbValue, 1, 0);
			this.tlpValue_1.Controls.Add(this.lblValue, 0, 0);
			this.tlpValue_1.Location = new System.Drawing.Point(21, 19);
			this.tlpValue_1.Name = "tlpValue_1";
			this.tlpValue_1.RowCount = 1;
			this.tlpValue_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpValue_1.Size = new System.Drawing.Size(398, 77);
			this.tlpValue_1.TabIndex = 1;
			// 
			// tbValue
			// 
			this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbValue.Location = new System.Drawing.Point(102, 3);
			this.tbValue.Multiline = true;
			this.tbValue.Name = "tbValue";
			this.tbValue.ReadOnly = true;
			this.tbValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbValue.Size = new System.Drawing.Size(293, 71);
			this.tbValue.TabIndex = 7;
			this.tbValue.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n";
			// 
			// lblValue
			// 
			this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(3, 32);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(93, 13);
			this.lblValue.TabIndex = 6;
			this.lblValue.Text = "Value:";
			this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblValue_gb
			// 
			this.lblValue_gb.AutoSize = true;
			this.lblValue_gb.Location = new System.Drawing.Point(191, 0);
			this.lblValue_gb.Name = "lblValue_gb";
			this.lblValue_gb.Size = new System.Drawing.Size(34, 13);
			this.lblValue_gb.TabIndex = 0;
			this.lblValue_gb.Text = "Value";
			// 
			// gbProperties
			// 
			this.gbProperties.Controls.Add(this.tlpPropertiesBits);
			this.gbProperties.Controls.Add(this.tlpProperties);
			this.gbProperties.Controls.Add(this.lblProperties_gb);
			this.gbProperties.Location = new System.Drawing.Point(17, 401);
			this.gbProperties.Name = "gbProperties";
			this.gbProperties.Size = new System.Drawing.Size(433, 135);
			this.gbProperties.TabIndex = 3;
			this.gbProperties.TabStop = false;
			// 
			// tlpPropertiesBits
			// 
			this.tlpPropertiesBits.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpPropertiesBits.ColumnCount = 4;
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.Controls.Add(this.lblBroadcast, 0, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblWriteWithoutResponse, 1, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblNotify, 2, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblAuthenticatedSignedWrites, 3, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblRead, 0, 1);
			this.tlpPropertiesBits.Controls.Add(this.lblWrite, 1, 1);
			this.tlpPropertiesBits.Controls.Add(this.lblIndicate, 2, 1);
			this.tlpPropertiesBits.Controls.Add(this.lblExtendedProperties, 3, 1);
			this.tlpPropertiesBits.Location = new System.Drawing.Point(18, 51);
			this.tlpPropertiesBits.Name = "tlpPropertiesBits";
			this.tlpPropertiesBits.RowCount = 2;
			this.tlpPropertiesBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPropertiesBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPropertiesBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpPropertiesBits.Size = new System.Drawing.Size(398, 67);
			this.tlpPropertiesBits.TabIndex = 3;
			// 
			// lblBroadcast
			// 
			this.lblBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblBroadcast.AutoSize = true;
			this.lblBroadcast.Location = new System.Drawing.Point(4, 10);
			this.lblBroadcast.Name = "lblBroadcast";
			this.lblBroadcast.Size = new System.Drawing.Size(92, 13);
			this.lblBroadcast.TabIndex = 8;
			this.lblBroadcast.Text = "Broadcast";
			this.lblBroadcast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblWriteWithoutResponse
			// 
			this.lblWriteWithoutResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblWriteWithoutResponse.AutoSize = true;
			this.lblWriteWithoutResponse.Location = new System.Drawing.Point(103, 4);
			this.lblWriteWithoutResponse.Name = "lblWriteWithoutResponse";
			this.lblWriteWithoutResponse.Size = new System.Drawing.Size(92, 26);
			this.lblWriteWithoutResponse.TabIndex = 10;
			this.lblWriteWithoutResponse.Text = "Write Without Response";
			this.lblWriteWithoutResponse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblNotify
			// 
			this.lblNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNotify.AutoSize = true;
			this.lblNotify.Location = new System.Drawing.Point(202, 10);
			this.lblNotify.Name = "lblNotify";
			this.lblNotify.Size = new System.Drawing.Size(92, 13);
			this.lblNotify.TabIndex = 12;
			this.lblNotify.Text = "Notify";
			this.lblNotify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAuthenticatedSignedWrites
			// 
			this.lblAuthenticatedSignedWrites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAuthenticatedSignedWrites.AutoSize = true;
			this.lblAuthenticatedSignedWrites.Location = new System.Drawing.Point(301, 4);
			this.lblAuthenticatedSignedWrites.Name = "lblAuthenticatedSignedWrites";
			this.lblAuthenticatedSignedWrites.Size = new System.Drawing.Size(93, 26);
			this.lblAuthenticatedSignedWrites.TabIndex = 11;
			this.lblAuthenticatedSignedWrites.Text = "Authenticated Signed Writes";
			this.lblAuthenticatedSignedWrites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblRead
			// 
			this.lblRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRead.AutoSize = true;
			this.lblRead.Location = new System.Drawing.Point(4, 43);
			this.lblRead.Name = "lblRead";
			this.lblRead.Size = new System.Drawing.Size(92, 13);
			this.lblRead.TabIndex = 9;
			this.lblRead.Text = "Read";
			this.lblRead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblWrite
			// 
			this.lblWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblWrite.AutoSize = true;
			this.lblWrite.Location = new System.Drawing.Point(103, 43);
			this.lblWrite.Name = "lblWrite";
			this.lblWrite.Size = new System.Drawing.Size(92, 13);
			this.lblWrite.TabIndex = 15;
			this.lblWrite.Text = "Write";
			this.lblWrite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblIndicate
			// 
			this.lblIndicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIndicate.AutoSize = true;
			this.lblIndicate.Location = new System.Drawing.Point(202, 43);
			this.lblIndicate.Name = "lblIndicate";
			this.lblIndicate.Size = new System.Drawing.Size(92, 13);
			this.lblIndicate.TabIndex = 13;
			this.lblIndicate.Text = "Indicate";
			this.lblIndicate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtendedProperties
			// 
			this.lblExtendedProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblExtendedProperties.AutoSize = true;
			this.lblExtendedProperties.Location = new System.Drawing.Point(301, 37);
			this.lblExtendedProperties.Name = "lblExtendedProperties";
			this.lblExtendedProperties.Size = new System.Drawing.Size(93, 26);
			this.lblExtendedProperties.TabIndex = 14;
			this.lblExtendedProperties.Text = "Extended Properties";
			this.lblExtendedProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tlpProperties
			// 
			this.tlpProperties.ColumnCount = 2;
			this.tlpProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpProperties.Controls.Add(this.tbProperties, 1, 0);
			this.tlpProperties.Controls.Add(this.lblProperties, 0, 0);
			this.tlpProperties.Location = new System.Drawing.Point(18, 21);
			this.tlpProperties.Name = "tlpProperties";
			this.tlpProperties.RowCount = 1;
			this.tlpProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpProperties.Size = new System.Drawing.Size(398, 25);
			this.tlpProperties.TabIndex = 2;
			// 
			// tbProperties
			// 
			this.tbProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbProperties.Location = new System.Drawing.Point(102, 3);
			this.tbProperties.Name = "tbProperties";
			this.tbProperties.ReadOnly = true;
			this.tbProperties.Size = new System.Drawing.Size(293, 20);
			this.tbProperties.TabIndex = 8;
			// 
			// lblProperties
			// 
			this.lblProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblProperties.AutoSize = true;
			this.lblProperties.Location = new System.Drawing.Point(3, 6);
			this.lblProperties.Name = "lblProperties";
			this.lblProperties.Size = new System.Drawing.Size(93, 13);
			this.lblProperties.TabIndex = 7;
			this.lblProperties.Text = "Abbrev(s) / Value:";
			this.lblProperties.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblProperties_gb
			// 
			this.lblProperties_gb.AutoSize = true;
			this.lblProperties_gb.Location = new System.Drawing.Point(179, 0);
			this.lblProperties_gb.Name = "lblProperties_gb";
			this.lblProperties_gb.Size = new System.Drawing.Size(54, 13);
			this.lblProperties_gb.TabIndex = 1;
			this.lblProperties_gb.Text = "Properties";
			// 
			// AttrDataItemForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(467, 547);
			this.Controls.Add(this.gbProperties);
			this.Controls.Add(this.gbValue);
			this.Controls.Add(this.gbUuid);
			this.Controls.Add(this.gbSummary);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(483, 586);
			this.MinimizeBox = false;
			this.Name = "AttrDataItemForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Attribute Data Item";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AttrDataItemForm_FormClosing);
			this.Load += new System.EventHandler(this.AttrDataItemForm_FormLoad);
			this.gbSummary.ResumeLayout(false);
			this.gbSummary.PerformLayout();
			this.tlpSummary.ResumeLayout(false);
			this.tlpSummary.PerformLayout();
			this.gbUuid.ResumeLayout(false);
			this.gbUuid.PerformLayout();
			this.tlpUuid_2.ResumeLayout(false);
			this.tlpUuid_2.PerformLayout();
			this.tlpUuid_1.ResumeLayout(false);
			this.tlpUuid_1.PerformLayout();
			this.gbValue.ResumeLayout(false);
			this.gbValue.PerformLayout();
			this.tlpValue_3.ResumeLayout(false);
			this.tlpValue_3.PerformLayout();
			this.tlpValue_2.ResumeLayout(false);
			this.tlpValue_2.PerformLayout();
			this.tlpValue_1.ResumeLayout(false);
			this.tlpValue_1.PerformLayout();
			this.gbProperties.ResumeLayout(false);
			this.gbProperties.PerformLayout();
			this.tlpPropertiesBits.ResumeLayout(false);
			this.tlpPropertiesBits.PerformLayout();
			this.tlpProperties.ResumeLayout(false);
			this.tlpProperties.PerformLayout();
			this.ResumeLayout(false);

        }

        public void LoadData(string dataKey)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new LoadDataDelegate(this.LoadData), new object[] { dataKey });
                }
                catch
                {
                }
            }
            else
            {
                this.formDataAccess.WaitOne();
                this.key = dataKey;
                this.dataAttr = new DataAttr();
                bool dataChanged = false;
                if (this.attrDataUtils.GetDataAttr(ref this.dataAttr, ref dataChanged, this.key, "LoadData"))
                {
                    if (dataChanged)
                    {
                        this.tbConnHnd.Text = "0x" + this.dataAttr.connHandle.ToString("X4");
                        this.tbHandle.Text = "0x" + this.dataAttr.handle.ToString("X4");
                        if ((this.dataAttr.uuidHex != string.Empty) && (this.dataAttr.uuidHex != null))
                        {
                            this.tbUuid.Text = "0x" + this.dataAttr.uuidHex;
                        }
                        this.tbUuidDesc.Text = this.dataAttr.uuidDesc;
                        string outStr = string.Empty;
                        if (this.lastValueDisplaySet)
                        {
                            this.devUtils.ConvertDisplayTypes(ValueDisplay.Hex, this.dataAttr.value, ref this.lastValueDisplay, ref outStr, false);
                        }
                        else
                        {
                            this.devUtils.ConvertDisplayTypes(ValueDisplay.Hex, this.dataAttr.value, ref this.dataAttr.valueDsp, ref outStr, false);
                            this.lastValueDisplay = this.dataAttr.valueDsp;
                            this.lastValueDisplaySet = true;
                            this.cbDataType.SelectedIndex = (int) this.lastValueDisplay;
                        }
                        this.tbValue.Text = outStr;
                        this.tbValueDesc.Text = this.dataAttr.valueDesc;
                        this.tbProperties.Text = this.dataAttr.propertiesStr;
                        bool flag2 = false;
                        if ((this.dataAttr.propertiesStr != null) && (this.dataAttr.propertiesStr != string.Empty))
                        {
                            flag2 = true;
                            Color green = Color.Green;
                            Color red = Color.Red;
                            byte num = 0;
                            num = 1;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblBroadcast.ForeColor = green;
                            }
                            else
                            {
                                this.lblBroadcast.ForeColor = red;
                            }
                            num = 2;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblRead.ForeColor = green;
                            }
                            else
                            {
                                this.lblRead.ForeColor = red;
                            }
                            num = 4;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblWriteWithoutResponse.ForeColor = green;
                            }
                            else
                            {
                                this.lblWriteWithoutResponse.ForeColor = red;
                            }
                            num = 8;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblWrite.ForeColor = green;
                            }
                            else
                            {
                                this.lblWrite.ForeColor = red;
                            }
                            num = 0x10;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblNotify.ForeColor = green;
                            }
                            else
                            {
                                this.lblNotify.ForeColor = red;
                            }
                            num = 0x20;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblIndicate.ForeColor = green;
                            }
                            else
                            {
                                this.lblIndicate.ForeColor = red;
                            }
                            num = 0x40;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblAuthenticatedSignedWrites.ForeColor = green;
                            }
                            else
                            {
                                this.lblAuthenticatedSignedWrites.ForeColor = red;
                            }
                            num = 0x80;
                            if ((this.dataAttr.properties & num) == num)
                            {
                                this.lblExtendedProperties.ForeColor = green;
                            }
                            else
                            {
                                this.lblExtendedProperties.ForeColor = red;
                            }
                        }
                        this.gbProperties.Enabled = flag2;
                        this.tbProperties.Enabled = flag2;
                        this.lblProperties.Enabled = flag2;
                        this.lblBroadcast.Enabled = flag2;
                        this.lblRead.Enabled = flag2;
                        this.lblWriteWithoutResponse.Enabled = flag2;
                        this.lblWrite.Enabled = flag2;
                        this.lblNotify.Enabled = flag2;
                        this.lblIndicate.Enabled = flag2;
                        this.lblAuthenticatedSignedWrites.Enabled = flag2;
                        this.lblExtendedProperties.Enabled = flag2;
                    }
                    if (this.dataAttr.valueEdit == ValueEdit.ReadOnly)
                    {
                        this.btnWriteValue.Enabled = false;
                        this.tbValue.ReadOnly = true;
                    }
                    else
                    {
                        this.btnWriteValue.Enabled = true;
                        this.tbValue.ReadOnly = false;
                    }
                    if (!this.lastValueDisplaySet)
                    {
                        this.cbDataType.SelectedIndex = (int) this.dataAttr.valueDsp;
                    }
                }
                this.formDataAccess.ReleaseMutex();
            }
        }

        public void LoadUserSettings()
        {
        }

        public void RestoreFormInput()
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new RestoreFormInputDelegate(this.RestoreFormInput), new object[0]);
                }
                catch
                {
                }
            }
            else
            {
                base.Enabled = true;
            }
        }

        public void SaveUserSettings()
        {
        }

        public void SendCmdResult(bool result, string cmdName)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new SendCmdResultDelegate(this.SendCmdResult), new object[] { result, cmdName });
                }
                catch
                {
                }
            }
            else if (!result)
            {
                string msg = "Send Command Failed\nMessage Name = " + cmdName + "\n";
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                this.msgBox.UserMsgBox(this, MsgBox.MsgTypes.Error, msg);
                this.RestoreFormInput();
            }
        }

        private delegate void LoadDataDelegate(string dataKey);

        private delegate void RestoreFormInputDelegate();

        private delegate void SendCmdResultDelegate(bool result, string cmdName);
    }
}
