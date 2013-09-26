﻿namespace BTool
{
    using BTool.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class AttributesForm : Form
    {
        private AttrDataUtils attrDataUtils;
        private AttributeFormUtils attrFormUtils = new AttributeFormUtils();
        private ColumnHeader chConnHandle;
        private ColumnHeader chHandle;
        private ColumnHeader chKey;
        private ColumnHeader chProperties;
        private ColumnHeader chUuid;
        private ColumnHeader chUuidDesc;
        private ColumnHeader chValue;
        private ColumnHeader chValueDesc;
        private ContextMenuStrip cmsAttributes;
        private IContainer components;
        private bool dataUpdating;
        private DeviceForm devForm;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public DisplayMsgDelegate DisplayMsgCallback;
        private Mutex formDataAccess = new Mutex();
        private ListViewSort listViewSort = new ListViewSort();
        private ListViewWrapper lvAttributes;
        private MouseUtils lvAttributes_MouseUtils = new MouseUtils();
        private const string moduleName = "AttributesForm";
        private MsgBox msgBox = new MsgBox();
        private bool needDataUpdate;
        private SendCmds sendCmds;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem tsmiAutoResizeColumns;
        private ToolStripMenuItem tsmiAutoScroll;
        private ToolStripMenuItem tsmiClearAllAttrData;
        private ToolStripMenuItem tsmiReadValue;
        private ToolStripMenuItem tsmiRestoreDefaultColumnWidths;
        private ToolStripMenuItem tsmiSaveDataToCsvFile;
        private ToolStripMenuItem tsmiSendAutoCmds;
        private ToolStripMenuItem tsmiSortByConHndAndHandle;
        private ToolStripMenuItem tsmiWriteValue;

        public AttributesForm(DeviceForm deviceForm)
        {
            this.InitializeComponent();
            this.devForm = deviceForm;
            this.attrDataUtils = new AttrDataUtils(deviceForm);
            this.sendCmds = new SendCmds(deviceForm);
            this.ResetSort();
            this.devForm.threadMgr.rspDataIn.RspDataInChangedCallback = new RspDataInChangedDelegate(this.RspDataInChanged);
            this.lvAttributes_MouseUtils.MouseSingleClickCallback = new MouseUtils.MouseSingleClickDelegate(this.lvAttributes_MouseSingleClick);
            this.lvAttributes_MouseUtils.MouseDoubleClickCallback = new MouseUtils.MouseDoubleClickDelegate(this.lvAttributes_MouseDoubleClick);
            this.lvAttributes.MouseUp += new MouseEventHandler(this.lvAttributes_MouseUtils.MouseClick_MouseUp);
            this.chKey.Width = 0;
            this.ClearAll();
            this.tsmiRestoreDefaultColumnWidths_Click(null, EventArgs.Empty);
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
                    this.RspDataInChanged();
                }
                this.RestoreFormInput();
            }
        }

        private void AttributesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void AttributesForm_FormLoad(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.ShowAlways = true;
            tip.SetToolTip(this.lvAttributes, "Right Click For Menu Of Options");
            this.LoadUserSettings();
            this.tsmiSendAutoCmds.Visible = false;
            this.toolStripSeparator5.Visible = false;
        }

        private bool CheckForStringData(string checkString)
        {
            bool flag = true;
            return (((checkString != null) && !(checkString == string.Empty)) && flag);
        }

        private void ClearAll()
        {
            this.lvAttributes.BeginUpdate();
            this.lvAttributes.Items.Clear();
            this.lvAttributes.EndUpdate();
            this.lvAttributes.Update();
            this.ResetSort();
        }

        public void ClearAttributes()
        {
            this.ClearAll();
        }

        private void ClearRspDelegates()
        {
            this.devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = null;
            this.devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = null;
            this.devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = null;
        }

        private void cmsAttributes_Opening(object sender, CancelEventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.tsmiSendAutoCmds.Checked = this.devForm.attrData.sendAutoCmds;
                bool flag = false;
                if ((this.lvAttributes != null) && (this.lvAttributes.Items.Count > 0))
                {
                    flag = true;
                }
                this.tsmiClearAllAttrData.Enabled = flag;
                this.tsmiSortByConHndAndHandle.Enabled = flag;
                this.tsmiAutoResizeColumns.Enabled = flag;
                this.tsmiReadValue.Enabled = flag;
                this.tsmiWriteValue.Enabled = flag;
                this.tsmiSaveDataToCsvFile.Enabled = flag;
                this.formDataAccess.ReleaseMutex();
            }
            else
            {
                e.Cancel = true;
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

        private bool FindAttr(string key, ref int nodeIndex)
        {
            bool flag = false;
            try
            {
                if ((this.lvAttributes == null) || (this.lvAttributes.Items.Count <= 0))
                {
                    return flag;
                }
                int count = this.lvAttributes.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    string text = this.lvAttributes.Items[i].Text;
                    if (key == text)
                    {
                        nodeIndex = i;
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = "Cannot Find Attribute\n" + exception.Message + "\n";
                this.msgBox.UserMsgBox(this, MsgBox.MsgTypes.Error, msg);
            }
            return flag;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ListViewItem item = new ListViewItem(new string[] { "DDDD_DDDD", "0xDDDD", "0xDDDD", "0xDDDD", "1234567890123456789012345678901234567890", "11:22:33:44:55:66:77:88:99", "1234567890123456789012345678901234567890", "01234567891234567890" }, -1);
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AttributesForm));
            this.cmsAttributes = new ContextMenuStrip(this.components);
            this.tsmiAutoScroll = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsmiClearAllAttrData = new ToolStripMenuItem();
            this.tsmiSortByConHndAndHandle = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.tsmiAutoResizeColumns = new ToolStripMenuItem();
            this.tsmiRestoreDefaultColumnWidths = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.tsmiReadValue = new ToolStripMenuItem();
            this.tsmiWriteValue = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.tsmiSendAutoCmds = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.tsmiSaveDataToCsvFile = new ToolStripMenuItem();
            this.lvAttributes = new ListViewWrapper();
            this.chKey = new ColumnHeader();
            this.chConnHandle = new ColumnHeader();
            this.chHandle = new ColumnHeader();
            this.chUuid = new ColumnHeader();
            this.chUuidDesc = new ColumnHeader();
            this.chValue = new ColumnHeader();
            this.chValueDesc = new ColumnHeader();
            this.chProperties = new ColumnHeader();
            this.cmsAttributes.SuspendLayout();
            base.SuspendLayout();
            this.cmsAttributes.Items.AddRange(new ToolStripItem[] { this.tsmiAutoScroll, this.toolStripSeparator1, this.tsmiClearAllAttrData, this.tsmiSortByConHndAndHandle, this.toolStripSeparator2, this.tsmiAutoResizeColumns, this.tsmiRestoreDefaultColumnWidths, this.toolStripSeparator3, this.tsmiReadValue, this.tsmiWriteValue, this.toolStripSeparator4, this.tsmiSendAutoCmds, this.toolStripSeparator5, this.tsmiSaveDataToCsvFile });
            this.cmsAttributes.Name = "cmsAttributes";
            this.cmsAttributes.Size = new Size(0xf1, 0xe8);
            this.cmsAttributes.Opening += new CancelEventHandler(this.cmsAttributes_Opening);
            this.tsmiAutoScroll.Name = "tsmiAutoScroll";
            this.tsmiAutoScroll.Size = new Size(240, 0x16);
            this.tsmiAutoScroll.Text = "&Auto-Scroll";
            this.tsmiAutoScroll.ToolTipText = "Auto Scroll Lines As New Data Is Added";
            this.tsmiAutoScroll.Click += new EventHandler(this.tsmiAutoScroll_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xed, 6);
            this.tsmiClearAllAttrData.Name = "tsmiClearAllAttrData";
            this.tsmiClearAllAttrData.Size = new Size(240, 0x16);
            this.tsmiClearAllAttrData.Text = "&Clear All Attribute Data";
            this.tsmiClearAllAttrData.ToolTipText = "Clear All Data From This Table";
            this.tsmiClearAllAttrData.Click += new EventHandler(this.tsmiClearAllAttrData_Click);
            this.tsmiSortByConHndAndHandle.Name = "tsmiSortByConHndAndHandle";
            this.tsmiSortByConHndAndHandle.Size = new Size(240, 0x16);
            this.tsmiSortByConHndAndHandle.Text = "&Sort By ConHnd And Handle";
            this.tsmiSortByConHndAndHandle.ToolTipText = @"Sorts The List By Connection Handle And Handle Columns\n(Default Sort Method)";
            this.tsmiSortByConHndAndHandle.Click += new EventHandler(this.tsmiSortByConHndAndHandle_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xed, 6);
            this.tsmiAutoResizeColumns.Name = "tsmiAutoResizeColumns";
            this.tsmiAutoResizeColumns.Size = new Size(240, 0x16);
            this.tsmiAutoResizeColumns.Text = "Auto &Resize Columns";
            this.tsmiAutoResizeColumns.ToolTipText = "Resize All Columns Based On Data Width";
            this.tsmiAutoResizeColumns.Click += new EventHandler(this.tsmiAutoResizeColumns_Click);
            this.tsmiRestoreDefaultColumnWidths.Name = "tsmiRestoreDefaultColumnWidths";
            this.tsmiRestoreDefaultColumnWidths.Size = new Size(240, 0x16);
            this.tsmiRestoreDefaultColumnWidths.Text = "Restore Default Column Widths";
            this.tsmiRestoreDefaultColumnWidths.ToolTipText = "Restore Column Widths To Defaults";
            this.tsmiRestoreDefaultColumnWidths.Click += new EventHandler(this.tsmiRestoreDefaultColumnWidths_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(0xed, 6);
            this.tsmiReadValue.Name = "tsmiReadValue";
            this.tsmiReadValue.Size = new Size(240, 0x16);
            this.tsmiReadValue.Text = "R&ead Value (Single Click)";
            this.tsmiReadValue.ToolTipText = "Read A Value From The Device\r\n(Single Click Line)";
            this.tsmiReadValue.Click += new EventHandler(this.tsmiReadValue_Click);
            this.tsmiWriteValue.Name = "tsmiWriteValue";
            this.tsmiWriteValue.Size = new Size(240, 0x16);
            this.tsmiWriteValue.Text = "&Write Item (Double Click)";
            this.tsmiWriteValue.ToolTipText = "Expanded Attribute View Window\r\nWrite A Value To The Device\r\n(Double Click Line)";
            this.tsmiWriteValue.Click += new EventHandler(this.tsmiWriteValue_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(0xed, 6);
            this.tsmiSendAutoCmds.Name = "tsmiSendAutoCmds";
            this.tsmiSendAutoCmds.Size = new Size(240, 0x16);
            this.tsmiSendAutoCmds.Text = "Send A&uto Data Commands";
            this.tsmiSendAutoCmds.ToolTipText = "Automatically Send Commands To Get Data Based On Incoming Commands";
            this.tsmiSendAutoCmds.Click += new EventHandler(this.tsmiSendAutoCmds_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(0xed, 6);
            this.tsmiSaveDataToCsvFile.Name = "tsmiSaveDataToCsvFile";
            this.tsmiSaveDataToCsvFile.Size = new Size(240, 0x16);
            this.tsmiSaveDataToCsvFile.Text = "Save &Data To CSV File";
            this.tsmiSaveDataToCsvFile.Click += new EventHandler(this.tsmiSaveDataToCsvFile_Click);
            this.lvAttributes.Columns.AddRange(new ColumnHeader[] { this.chKey, this.chConnHandle, this.chHandle, this.chUuid, this.chUuidDesc, this.chValue, this.chValueDesc, this.chProperties });
            this.lvAttributes.Dock = DockStyle.Fill;
            this.lvAttributes.FullRowSelect = true;
            this.lvAttributes.GridLines = true;
            this.lvAttributes.Items.AddRange(new ListViewItem[] { item });
            this.lvAttributes.Location = new Point(0, 0);
            this.lvAttributes.MultiSelect = false;
            this.lvAttributes.Name = "lvAttributes";
            this.lvAttributes.Size = new Size(0x2b4, 0x106);
            this.lvAttributes.TabIndex = 1;
            this.lvAttributes.UseCompatibleStateImageBehavior = false;
            this.lvAttributes.View = View.Details;
            this.lvAttributes.ColumnClick += new ColumnClickEventHandler(this.lvAttributes_ColumnClick);
            this.chKey.Text = "Key";
            this.chKey.Width = 70;
            this.chConnHandle.Text = "ConHnd";
            this.chConnHandle.Width = 0x37;
            this.chHandle.Text = "Handle";
            this.chHandle.Width = 0x37;
            this.chUuid.Text = "Uuid";
            this.chUuid.Width = 0x37;
            this.chUuidDesc.Text = "Uuid Description";
            this.chUuidDesc.Width = 0xe1;
            this.chValue.Text = "Value";
            this.chValue.Width = 150;
            this.chValueDesc.Text = "Value Description";
            this.chValueDesc.Width = 0xaf;
            this.chProperties.Text = "Properties";
            this.chProperties.Width = 0x90;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x2b4, 0x106);
            this.ContextMenuStrip = this.cmsAttributes;
            base.Controls.Add(this.lvAttributes);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "AttributesForm";
            this.Text = "BLE Attributes";
            base.FormClosing += new FormClosingEventHandler(this.AttributesForm_FormClosing);
            base.Load += new EventHandler(this.AttributesForm_FormLoad);
            this.cmsAttributes.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InsertSubItem(ListViewItem lvItem, string data, Color fore, Color back)
        {
            if (data != null)
            {
                ListViewItem.ListViewSubItem lvSubItem = new ListViewItem.ListViewSubItem();
                lvSubItem.Text = data;
                this.UpdateItemColor(lvSubItem, fore, back);
                lvItem.SubItems.Add(lvSubItem);
            }
        }

        public void LoadUserSettings()
        {
            this.tsmiAutoScroll.Checked = Settings.Default.AttributesAutoScroll;
        }

        private void lvAttributes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.lvAttributes.ListViewItemSorter = this.listViewSort;
                if (e.Column == this.listViewSort.SortColumn)
                {
                    if (this.listViewSort.Order == SortOrder.Ascending)
                    {
                        this.listViewSort.Order = SortOrder.Descending;
                    }
                    else
                    {
                        this.listViewSort.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    this.listViewSort.SortColumn = e.Column;
                    this.listViewSort.Order = SortOrder.Ascending;
                }
                this.lvAttributes.Sort();
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void lvAttributes_MouseDoubleClick()
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                ListView.SelectedListViewItemCollection selectedItems = this.lvAttributes.SelectedItems;
                if (selectedItems.Count > 0)
                {
                    ListViewItem item = selectedItems[0];
                    string text = item.Text;
                    DataAttr dataAttr = new DataAttr();
                    bool dataChanged = false;
                    if (this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, text, "lvAttributes_DoubleClick") && dataChanged)
                    {
                        AttrDataItemForm form = new AttrDataItemForm(this.devForm);
                        form.DisplayMsgCallback = this.DisplayMsgCallback;
                        form.AttrDataItemChangedCallback = new AttrDataItemChangedDelegate(this.RspDataInChanged);
                        form.LoadData(text);
                        form.ShowDialog();
                    }
                }
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void lvAttributes_MouseSingleClick()
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.ReadSelectedValue();
                this.formDataAccess.ReleaseMutex();
            }
        }

        private bool ReadSelectedValue()
        {
            bool flag = true;
            ListView.SelectedListViewItemCollection selectedItems = this.lvAttributes.SelectedItems;
            if (selectedItems.Count > 0)
            {
                ListViewItem item = selectedItems[0];
                string text = item.Text;
                DataAttr dataAttr = new DataAttr();
                bool dataChanged = false;
                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, text, "lvAttributes_Click") || !dataChanged)
                {
                    return flag;
                }
                this.devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = new ExtCmdStatusDelegate(this.ExtCmdStatus);
                this.devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = new AttErrorRspDelegate(this.AttErrorRsp);
                this.devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = new AttReadBlobRspDelegate(this.AttReadBlobRsp);
                HCICmds.GATTCmds.GATT_ReadLongCharValue value2 = new HCICmds.GATTCmds.GATT_ReadLongCharValue();
                value2.connHandle = dataAttr.connHandle;
                value2.handle = dataAttr.handle;
                if (this.sendCmds.SendGATT(value2, TxDataOut.CmdType.General, new BTool.SendCmdResult(this.SendCmdResult)))
                {
                    base.Enabled = false;
                    return flag;
                }
                this.ClearRspDelegates();
            }
            return flag;
        }

        public void RemoveData(ushort connHandle)
        {
            this.dataUpdating = true;
            this.formDataAccess.WaitOne();
            this.devForm.attrData.attrDictAccess.WaitOne();
            this.lvAttributes.BeginUpdate();
            try
            {
                if ((this.devForm.attrData.attrDict != null) && (this.devForm.attrData.attrDict.Count > 0))
                {
                    string str = "0x" + connHandle.ToString("X4");
                    foreach (ListViewItem item in this.lvAttributes.Items)
                    {
                        if (item.SubItems[1].Text == str)
                        {
                            this.attrDataUtils.RemoveAttrDictItem(item.Text);
                            item.Remove();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = "Cannot Remove BLE Attributes\n" + exception.Message + "\n";
                this.msgBox.UserMsgBox(this, MsgBox.MsgTypes.Error, msg);
            }
            this.lvAttributes.EndUpdate();
            this.lvAttributes.Update();
            this.devForm.attrData.attrDictAccess.ReleaseMutex();
            this.formDataAccess.ReleaseMutex();
            this.dataUpdating = false;
        }

        public void ResetSort()
        {
            this.listViewSort.SortColumn = 0;
            this.listViewSort.Order = SortOrder.Ascending;
            this.lvAttributes.ListViewItemSorter = this.listViewSort;
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

        public void RspDataInChanged()
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.BeginInvoke(new RspDataInChangedDelegate(this.RspDataInChanged), new object[0]);
                }
                catch
                {
                }
            }
            else if (this.dataUpdating)
            {
                this.needDataUpdate = true;
            }
            else
            {
                this.UpdateData();
                while (this.needDataUpdate)
                {
                    this.needDataUpdate = false;
                    this.UpdateData();
                }
            }
        }

        private bool SaveCsvData()
        {
            bool flag = true;
            if ((this.lvAttributes != null) && (this.lvAttributes.Items.Count > 0))
            {
                try
                {
                    this.ResetSort();
                    List<AttributeFormUtils.CsvData> csvData = new List<AttributeFormUtils.CsvData>();
                    foreach (ListViewItem item in this.lvAttributes.Items)
                    {
                        AttributeFormUtils.CsvData data = new AttributeFormUtils.CsvData();
                        if (item.SubItems.Count > 1)
                        {
                            data.connectionHandle = item.SubItems[1].Text;
                        }
                        else
                        {
                            data.connectionHandle = string.Empty;
                        }
                        if (item.SubItems.Count > 2)
                        {
                            data.handle = item.SubItems[2].Text;
                        }
                        else
                        {
                            data.handle = string.Empty;
                        }
                        if (item.SubItems.Count > 3)
                        {
                            data.uuid = item.SubItems[3].Text;
                        }
                        else
                        {
                            data.uuid = string.Empty;
                        }
                        if (item.SubItems.Count > 4)
                        {
                            data.uuidDesc = item.SubItems[4].Text;
                        }
                        else
                        {
                            data.uuidDesc = string.Empty;
                        }
                        if (item.SubItems.Count > 5)
                        {
                            data.value = item.SubItems[5].Text;
                        }
                        else
                        {
                            data.value = string.Empty;
                        }
                        if (item.SubItems.Count > 6)
                        {
                            data.valueDesc = item.SubItems[6].Text;
                        }
                        else
                        {
                            data.valueDesc = string.Empty;
                        }
                        if (item.SubItems.Count > 7)
                        {
                            data.properties = item.SubItems[7].Text;
                        }
                        else
                        {
                            data.properties = string.Empty;
                        }
                        csvData.Add(data);
                    }
                    if (csvData.Count > 0)
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.RestoreDirectory = true;
                        dialog.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                        dialog.Title = "Save CSV File";
                        dialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                        dialog.FilterIndex = 1;
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            flag = this.attrFormUtils.WriteCsv(dialog.FileName, csvData);
                            if (flag)
                            {
                                string msg = "Csv File Save Completed\n";
                                msg = msg + "Location = " + dialog.FileName + "\n";
                                this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg);
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Info, msg);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    string str2 = "Cannot Save Csv File\n" + exception.Message + "\n";
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                    flag = false;
                }
            }
            return flag;
        }

        public void SaveUserSettings()
        {
            Settings.Default.AttributesAutoScroll = this.tsmiAutoScroll.Checked;
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
                int qLength = this.devForm.threadMgr.txDataOut.dataQ.GetQLength();
                if (qLength > 0)
                {
                    msg = "There Are " + qLength.ToString() + " Pending Transmit Messages\nDo You Want To Clear All Pending Transmit Messages?\n";
                    if (this.DisplayMsgCallback != null)
                    {
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg);
                    }
                    MsgBox.MsgResult result2 = this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.YesNo, msg);
                    msg = "UserResponse = " + result2.ToString() + "\n";
                    if (this.DisplayMsgCallback != null)
                    {
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg);
                    }
                    if (result2 == MsgBox.MsgResult.Yes)
                    {
                        this.devForm.threadMgr.txDataOut.dataQ.ClearQ();
                    }
                }
                this.RestoreFormInput();
            }
        }

        private void tsmiAutoResizeColumns_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                if (this.lvAttributes.Items.Count > 0)
                {
                    this.lvAttributes.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.lvAttributes.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.lvAttributes.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.lvAttributes.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.lvAttributes.AutoResizeColumn(5, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.lvAttributes.AutoResizeColumn(6, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.lvAttributes.AutoResizeColumn(7, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiAutoScroll_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                if (this.tsmiAutoScroll.Checked)
                {
                    this.tsmiAutoScroll.Checked = false;
                }
                else
                {
                    this.tsmiAutoScroll.Checked = true;
                }
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiClearAllAttrData_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.devForm.attrData.attrDict.Clear();
                this.ClearAttributes();
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiReadValue_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.ReadSelectedValue();
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiRestoreDefaultColumnWidths_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.chConnHandle.Width = AttrData.columns.connHandleWidth;
                this.chHandle.Width = AttrData.columns.handleWidth;
                this.chUuid.Width = AttrData.columns.uuidWidth;
                this.chUuidDesc.Width = AttrData.columns.uuidDescWidth;
                this.chValue.Width = AttrData.columns.valueWidth;
                this.chValueDesc.Width = AttrData.columns.valueDescWidth;
                this.chProperties.Width = AttrData.columns.propertiesWidth;
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiSaveDataToCsvFile_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.SaveCsvData();
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiSendAutoCmds_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                if (this.tsmiSendAutoCmds.Checked)
                {
                    this.tsmiSendAutoCmds.Checked = false;
                }
                else
                {
                    this.tsmiSendAutoCmds.Checked = true;
                }
                this.devForm.attrData.sendAutoCmds = this.tsmiSendAutoCmds.Checked;
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiSortByConHndAndHandle_Click(object sender, EventArgs e)
        {
            if (!this.dataUpdating)
            {
                this.formDataAccess.WaitOne();
                this.lvAttributes.ListViewItemSorter = this.listViewSort;
                this.listViewSort.SortColumn = 0;
                this.listViewSort.Order = SortOrder.Ascending;
                this.lvAttributes.Sort();
                this.formDataAccess.ReleaseMutex();
            }
        }

        private void tsmiWriteValue_Click(object sender, EventArgs e)
        {
            this.lvAttributes_MouseDoubleClick();
        }

        private void UpdateData()
        {
            this.dataUpdating = true;
            this.formDataAccess.WaitOne();
            this.devForm.attrData.attrDictAccess.WaitOne();
            this.lvAttributes.BeginUpdate();
            try
            {
                if ((this.devForm.attrData.attrDict != null) && (this.devForm.attrData.attrDict.Count > 0))
                {
                    Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                    foreach (KeyValuePair<string, DataAttr> pair in this.devForm.attrData.attrDict)
                    {
                        DataAttr attr = pair.Value;
                        if (attr.dataUpdate)
                        {
                            DataAttr attr2 = new DataAttr();
                            attr2 = attr;
                            attr2.dataUpdate = false;
                            tmpAttrDict.Add(attr.key, attr2);
                            int nodeIndex = 0;
                            string data = string.Empty;
                            ListSubItem connectionHandle = ListSubItem.ConnectionHandle;
                            if (this.lvAttributes.Items.Count >= 0x5dc)
                            {
                                string msg = string.Format("Attribute Data List At Maximum {0} Elements\nClear List Data\nAttributesForm\n", 0x5dc);
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
                                break;
                            }
                            Color defaultForeground = AttrData.defaultForeground;
                            Color defaultBackground = AttrData.defaultBackground;
                            if ((attr.foreColor != Color.Empty) && (attr.foreColor.ToKnownColor() != ((KnownColor) 0)))
                            {
                                defaultForeground = attr.foreColor;
                            }
                            if ((attr.backColor != Color.Empty) && (attr.backColor.ToKnownColor() != ((KnownColor) 0)))
                            {
                                defaultBackground = attr.backColor;
                            }
                            if (this.FindAttr(attr.key, ref nodeIndex))
                            {
                                try
                                {
                                    connectionHandle = ListSubItem.Key;
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.ConnectionHandle;
                                    if (attr.connHandle != 0xffff)
                                    {
                                        data = "0x" + attr.connHandle.ToString("X4");
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.Handle;
                                    if (attr.handle != 0)
                                    {
                                        data = "0x" + attr.handle.ToString("X4");
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.Uuid;
                                    if (this.CheckForStringData(attr.uuidHex))
                                    {
                                        data = "0x" + attr.uuidHex;
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.UuidDesc;
                                    if (this.CheckForStringData(attr.uuidDesc))
                                    {
                                        int indentLevel = attr.indentLevel;
                                        string str3 = "";
                                        for (int i = 0; i < indentLevel; i++)
                                        {
                                            str3 = str3 + " ";
                                        }
                                        data = str3 + attr.uuidDesc;
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.Value;
                                    if (this.CheckForStringData(attr.value))
                                    {
                                        string outStr = string.Empty;
                                        this.devUtils.ConvertDisplayTypes(ValueDisplay.Hex, attr.value, ref attr.valueDsp, ref outStr, false);
                                        data = outStr;
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.ValueDesc;
                                    if (this.CheckForStringData(attr.valueDesc))
                                    {
                                        data = attr.valueDesc;
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    connectionHandle = ListSubItem.Properties;
                                    if (this.CheckForStringData(attr.propertiesStr))
                                    {
                                        data = attr.propertiesStr;
                                        this.UpdateSubItem(nodeIndex, (int) connectionHandle, data, defaultForeground, defaultBackground);
                                    }
                                    this.UpdateItemColor(nodeIndex, (int) connectionHandle, defaultForeground, defaultBackground);
                                    continue;
                                }
                                catch (Exception exception)
                                {
                                    string str5 = "Cannot Update BLE Attributes\n" + exception.Message + "\n";
                                    this.msgBox.UserMsgBox(this, MsgBox.MsgTypes.Error, str5);
                                    break;
                                }
                            }
                            try
                            {
                                ListViewItem item = new ListViewItem();
                                item.UseItemStyleForSubItems = false;
                                connectionHandle = ListSubItem.Key;
                                item.Text = attr.key;
                                item.Tag = attr;
                                item.ForeColor = defaultForeground;
                                item.BackColor = defaultBackground;
                                int count = this.lvAttributes.Items.Count;
                                this.lvAttributes.Items.Insert(count, item);
                                data = string.Empty;
                                if (attr.connHandle != 0xffff)
                                {
                                    connectionHandle = ListSubItem.ConnectionHandle;
                                    data = "0x" + attr.connHandle.ToString("X4");
                                }
                                else
                                {
                                    data = "";
                                }
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                                if (attr.handle != 0)
                                {
                                    connectionHandle = ListSubItem.Handle;
                                    data = "0x" + attr.handle.ToString("X4");
                                }
                                else
                                {
                                    data = "";
                                }
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                                if (this.CheckForStringData(attr.uuidHex))
                                {
                                    data = "0x" + attr.uuidHex;
                                }
                                else
                                {
                                    data = "";
                                }
                                connectionHandle = ListSubItem.Uuid;
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                                connectionHandle = ListSubItem.UuidDesc;
                                int num5 = attr.indentLevel;
                                string str6 = "";
                                for (int j = 0; j < num5; j++)
                                {
                                    str6 = str6 + " ";
                                }
                                if (this.CheckForStringData(attr.uuidDesc))
                                {
                                    data = str6 + attr.uuidDesc;
                                }
                                else
                                {
                                    data = "";
                                }
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                                if (this.CheckForStringData(attr.value))
                                {
                                    data = attr.value;
                                }
                                else
                                {
                                    data = "";
                                }
                                string str7 = string.Empty;
                                this.devUtils.ConvertDisplayTypes(ValueDisplay.Hex, data, ref attr.valueDsp, ref str7, false);
                                data = str7;
                                connectionHandle = ListSubItem.Value;
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                                if (this.CheckForStringData(attr.valueDesc))
                                {
                                    data = attr.valueDesc;
                                }
                                else
                                {
                                    data = "";
                                }
                                connectionHandle = ListSubItem.ValueDesc;
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                                if (this.CheckForStringData(attr.propertiesStr))
                                {
                                    data = attr.propertiesStr;
                                }
                                else
                                {
                                    data = "";
                                }
                                connectionHandle = ListSubItem.Properties;
                                this.InsertSubItem(item, data, defaultForeground, defaultBackground);
                            }
                            catch (Exception exception2)
                            {
                                string str8 = "Cannot Add BLE Attributes\n" + exception2.Message + "\n";
                                this.msgBox.UserMsgBox(this, MsgBox.MsgTypes.Error, str8);
                                break;
                            }
                        }
                    }
                    this.attrDataUtils.UpdateAttrDict(tmpAttrDict);
                }
                if (this.tsmiAutoScroll.Checked && (this.lvAttributes.Items.Count > 0))
                {
                    this.lvAttributes.EnsureVisible(this.lvAttributes.Items.Count - 1);
                }
            }
            catch (Exception exception3)
            {
                string str9 = "Cannot Process BLE Attributes\n" + exception3.Message + "\n";
                this.msgBox.UserMsgBox(this, MsgBox.MsgTypes.Error, str9);
            }
            this.lvAttributes.EndUpdate();
            this.devForm.attrData.attrDictAccess.ReleaseMutex();
            this.formDataAccess.ReleaseMutex();
            this.dataUpdating = false;
        }

        private void UpdateItemColor(ListViewItem.ListViewSubItem lvSubItem, Color fore, Color back)
        {
            if (lvSubItem != null)
            {
                if (fore.ToKnownColor() == ((KnownColor) 0))
                {
                    fore = AttrData.defaultForeground;
                }
                lvSubItem.ForeColor = fore;
                if (back.ToKnownColor() == ((KnownColor) 0))
                {
                    back = AttrData.defaultBackground;
                }
                lvSubItem.BackColor = back;
            }
        }

        private void UpdateItemColor(int itemIndex, int subItemIndex, Color fore, Color back)
        {
            if ((this.lvAttributes.Items.Count > itemIndex) && (this.lvAttributes.Items[itemIndex].SubItems.Count > subItemIndex))
            {
                if (fore.ToKnownColor() == ((KnownColor) 0))
                {
                    fore = AttrData.defaultForeground;
                }
                this.lvAttributes.Items[itemIndex].SubItems[subItemIndex].ForeColor = fore;
                if (back.ToKnownColor() == ((KnownColor) 0))
                {
                    back = AttrData.defaultBackground;
                }
                this.lvAttributes.Items[itemIndex].SubItems[subItemIndex].BackColor = back;
            }
        }

        private void UpdateSubItem(int itemIndex, int subItemIndex, string data, Color fore, Color back)
        {
            if (this.lvAttributes.Items[itemIndex].SubItems.Count > subItemIndex)
            {
                this.lvAttributes.Items[itemIndex].SubItems[subItemIndex].Text = data;
            }
        }

        public enum ListSubItem
        {
            Key,
            ConnectionHandle,
            Handle,
            Uuid,
            UuidDesc,
            Value,
            ValueDesc,
            Properties
        }

        private delegate void RestoreFormInputDelegate();

        private delegate void SendCmdResultDelegate(bool result, string cmdName);
    }
}
