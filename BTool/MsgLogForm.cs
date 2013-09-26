﻿namespace BTool
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class MsgLogForm : Form
    {
        private ContextMenuStrip cmsRtbLog;
        private IContainer components;
        private DeviceForm devForm;
        public DisplayMsgDelegate DisplayMsgCallback;
        private Mutex dspMsgMutex = new Mutex();
        private Color[] MessageColor = new Color[] { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red, Color.Black, Color.Black };
        public static string moduleName = "MsgLogForm";
        public const string MsgBorderStr = "------------------------------------------------------------------------------------------------------------------------\n";
        private MsgBox msgBox = new MsgBox();
        private ulong msgNumber;
        public RichTextBox rtbMsgBox;
        private bool rtbUpdate;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem tsmiClearLog;
        private ToolStripMenuItem tsmiClearTransmitQueue;
        private ToolStripMenuItem tsmiCopy;
        private ToolStripMenuItem tsmiDisplayRxDumps;
        private ToolStripMenuItem tsmiDisplayRxPackets;
        private ToolStripMenuItem tsmiDisplayTxDumps;
        private ToolStripMenuItem tsmiDisplayTxPackets;
        private ToolStripMenuItem tsmiSave;
        private ToolStripMenuItem tsmiSelectAll;

        public MsgLogForm(DeviceForm deviceForm)
        {
            this.InitializeComponent();
            this.devForm = deviceForm;
        }

        public void AppendLog(string logMsg)
        {
            this.dspMsgMutex.WaitOne();
            if (logMsg != null)
            {
                this.rtbMsgBox.AppendText(logMsg);
            }
            this.dspMsgMutex.ReleaseMutex();
        }

        public void DisplayLogMsg(SharedAppObjs.MsgType msgType, string msg, string time)
        {
            this.dspMsgMutex.WaitOne();
            this.rtbUpdate = true;
            if (base.InvokeRequired)
            {
                try
                {
                    base.BeginInvoke(new DisplayLogMsgDelegate(this.DisplayLogMsg), new object[] { msgType, msg, time });
                }
                catch
                {
                }
            }
            else
            {
                string str = string.Empty;
                bool flag = false;
                switch (msgType)
                {
                    case SharedAppObjs.MsgType.Incoming:
                        str = "<Rx> - ";
                        if (!this.tsmiDisplayRxPackets.Checked)
                        {
                            flag = true;
                        }
                        break;

                    case SharedAppObjs.MsgType.Outgoing:
                        str = "<Tx> - ";
                        if (!this.tsmiDisplayTxPackets.Checked)
                        {
                            flag = true;
                        }
                        break;

                    case SharedAppObjs.MsgType.Info:
                        str = "<Info> - ";
                        break;

                    case SharedAppObjs.MsgType.Warning:
                        str = "<Warning> - ";
                        break;

                    case SharedAppObjs.MsgType.Error:
                        str = "<Error> - ";
                        break;

                    case SharedAppObjs.MsgType.RxDump:
                        str = "Dump(Rx):\n";
                        if (!this.tsmiDisplayRxDumps.Checked)
                        {
                            flag = true;
                        }
                        break;

                    case SharedAppObjs.MsgType.TxDump:
                        str = "Dump(Tx):\n";
                        if (!this.tsmiDisplayTxDumps.Checked)
                        {
                            flag = true;
                        }
                        break;

                    default:
                        str = "<Unknown> - ";
                        break;
                }
                if (!flag)
                {
                    this.rtbMsgBox.SuspendLayout();
                    try
                    {
                        this.rtbMsgBox.SelectionStart = this.rtbMsgBox.TextLength;
                        this.rtbMsgBox.SelectionLength = 0;
                        if ((msgType != SharedAppObjs.MsgType.RxDump) && (msgType != SharedAppObjs.MsgType.TxDump))
                        {
                            this.rtbMsgBox.SelectionColor = this.MessageColor[(int) msgType];
                            this.msgNumber += (ulong) 1L;
                            string str2 = string.Empty;
                            if (time == null)
                            {
                                str2 = DateTime.Now.ToString("hh:mm:ss.fff");
                            }
                            else
                            {
                                str2 = time;
                            }
                            this.rtbMsgBox.AppendText("[" + this.msgNumber.ToString() + "] : " + str + str2 + "\n" + msg);
                        }
                        else
                        {
                            this.rtbMsgBox.SelectionColor = Color.Black;
                            this.rtbMsgBox.AppendText(str + msg + "\n");
                        }
                        if ((((msgType != SharedAppObjs.MsgType.Incoming) && (msgType != SharedAppObjs.MsgType.Outgoing)) || ((msgType == SharedAppObjs.MsgType.Incoming) && !this.tsmiDisplayRxDumps.Checked)) || ((msgType == SharedAppObjs.MsgType.Outgoing) && !this.tsmiDisplayTxDumps.Checked))
                        {
                            this.rtbMsgBox.AppendText("------------------------------------------------------------------------------------------------------------------------\n");
                        }
                    }
                    catch
                    {
                    }
                    this.rtbMsgBox.ResumeLayout();
                }
            }
            this.rtbUpdate = false;
            this.dspMsgMutex.ReleaseMutex();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool GetDisplayRxDumps()
        {
            return this.tsmiDisplayRxDumps.Checked;
        }

        public bool GetDisplayTxDumps()
        {
            return this.tsmiDisplayTxDumps.Checked;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.rtbMsgBox = new RichTextBox();
            this.cmsRtbLog = new ContextMenuStrip(this.components);
            this.tsmiDisplayRxDumps = new ToolStripMenuItem();
            this.tsmiDisplayTxDumps = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.tsmiDisplayRxPackets = new ToolStripMenuItem();
            this.tsmiDisplayTxPackets = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.tsmiSelectAll = new ToolStripMenuItem();
            this.tsmiCopy = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsmiClearLog = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.tsmiSave = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.tsmiClearTransmitQueue = new ToolStripMenuItem();
            this.cmsRtbLog.SuspendLayout();
            base.SuspendLayout();
            this.rtbMsgBox.BackColor = SystemColors.ControlLightLight;
            this.rtbMsgBox.ContextMenuStrip = this.cmsRtbLog;
            this.rtbMsgBox.Cursor = Cursors.SizeNS;
            this.rtbMsgBox.Dock = DockStyle.Fill;
            this.rtbMsgBox.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.rtbMsgBox.HideSelection = false;
            this.rtbMsgBox.Location = new Point(0, 0);
            this.rtbMsgBox.Margin = new Padding(2, 3, 2, 3);
            this.rtbMsgBox.Name = "rtbMsgBox";
            this.rtbMsgBox.ReadOnly = true;
            this.rtbMsgBox.Size = new Size(0x162, 0x21e);
            this.rtbMsgBox.TabIndex = 1;
            this.rtbMsgBox.Text = "";
            this.rtbMsgBox.WordWrap = false;
            this.rtbMsgBox.HScroll += new EventHandler(this.rtbMsgBox_HScroll);
            this.rtbMsgBox.VScroll += new EventHandler(this.rtbMsgBox_VScroll);
            this.cmsRtbLog.Items.AddRange(new ToolStripItem[] { this.tsmiDisplayRxDumps, this.tsmiDisplayTxDumps, this.toolStripSeparator2, this.tsmiDisplayRxPackets, this.tsmiDisplayTxPackets, this.toolStripSeparator4, this.tsmiSelectAll, this.tsmiCopy, this.toolStripSeparator1, this.tsmiClearLog, this.toolStripSeparator3, this.tsmiSave, this.toolStripSeparator5, this.tsmiClearTransmitQueue });
            this.cmsRtbLog.Name = "contextMenuStrip2";
            this.cmsRtbLog.Size = new Size(0xbb, 0xe8);
            this.tsmiDisplayRxDumps.Checked = true;
            this.tsmiDisplayRxDumps.CheckState = CheckState.Checked;
            this.tsmiDisplayRxDumps.Name = "tsmiDisplayRxDumps";
            this.tsmiDisplayRxDumps.Size = new Size(0xba, 0x16);
            this.tsmiDisplayRxDumps.Text = "&Display Rx Dumps";
            this.tsmiDisplayRxDumps.ToolTipText = "Display Rx Data Dumps";
            this.tsmiDisplayRxDumps.Click += new EventHandler(this.tsmiDisplayRxDumps_Click);
            this.tsmiDisplayTxDumps.Checked = true;
            this.tsmiDisplayTxDumps.CheckState = CheckState.Checked;
            this.tsmiDisplayTxDumps.Name = "tsmiDisplayTxDumps";
            this.tsmiDisplayTxDumps.Size = new Size(0xba, 0x16);
            this.tsmiDisplayTxDumps.Text = "D&isplay Tx Dumps";
            this.tsmiDisplayTxDumps.ToolTipText = "Display Tx Data Dumps";
            this.tsmiDisplayTxDumps.Click += new EventHandler(this.tsmiDisplayTxDumps_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xb7, 6);
            this.tsmiDisplayRxPackets.Checked = true;
            this.tsmiDisplayRxPackets.CheckState = CheckState.Checked;
            this.tsmiDisplayRxPackets.Name = "tsmiDisplayRxPackets";
            this.tsmiDisplayRxPackets.Size = new Size(0xba, 0x16);
            this.tsmiDisplayRxPackets.Text = "Display &Rx Packets";
            this.tsmiDisplayRxPackets.ToolTipText = "Display Rx Packet Information";
            this.tsmiDisplayRxPackets.Click += new EventHandler(this.tsmiDisplayRxPackets_Click);
            this.tsmiDisplayTxPackets.Checked = true;
            this.tsmiDisplayTxPackets.CheckState = CheckState.Checked;
            this.tsmiDisplayTxPackets.Name = "tsmiDisplayTxPackets";
            this.tsmiDisplayTxPackets.Size = new Size(0xba, 0x16);
            this.tsmiDisplayTxPackets.Text = "Display &Tx Packets";
            this.tsmiDisplayTxPackets.ToolTipText = "Display Tx Packet Information";
            this.tsmiDisplayTxPackets.Click += new EventHandler(this.tsmiDisplayTxPackets_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(0xb7, 6);
            this.tsmiSelectAll.Name = "tsmiSelectAll";
            this.tsmiSelectAll.Size = new Size(0xba, 0x16);
            this.tsmiSelectAll.Text = "Select &All";
            this.tsmiSelectAll.ToolTipText = "Select All Text In Log";
            this.tsmiSelectAll.Click += new EventHandler(this.tsmiSelectAll_Click);
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new Size(0xba, 0x16);
            this.tsmiCopy.Text = "&Copy";
            this.tsmiCopy.ToolTipText = "Copy To Clipboard";
            this.tsmiCopy.Click += new EventHandler(this.tsmiCopy_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xb7, 6);
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new Size(0xba, 0x16);
            this.tsmiClearLog.Text = "C&lear Log";
            this.tsmiClearLog.ToolTipText = "Clear Log Area";
            this.tsmiClearLog.Click += new EventHandler(this.tsmiClearLog_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(0xb7, 6);
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.Size = new Size(0xba, 0x16);
            this.tsmiSave.Text = "&Save";
            this.tsmiSave.ToolTipText = "Save Log To File";
            this.tsmiSave.Click += new EventHandler(this.tsmiSave_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(0xb7, 6);
            this.tsmiClearTransmitQueue.Name = "tsmiClearTransmitQueue";
            this.tsmiClearTransmitQueue.Size = new Size(0xba, 0x16);
            this.tsmiClearTransmitQueue.Text = "ClearTransmit &Queue";
            this.tsmiClearTransmitQueue.ToolTipText = "Clears All Pending Transmit Commands";
            this.tsmiClearTransmitQueue.Click += new EventHandler(this.tsmiClearTransmitQueue_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x162, 0x21e);
            base.Controls.Add(this.rtbMsgBox);
            base.Name = "MsgLogForm";
            this.Text = "Msg Log Form";
            this.cmsRtbLog.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void ResetMsgNumber()
        {
            this.dspMsgMutex.WaitOne();
            this.msgNumber = 0L;
            this.dspMsgMutex.ReleaseMutex();
        }

        private void rtbMsgBox_HScroll(object sender, EventArgs e)
        {
            if (!this.rtbUpdate && !this.rtbMsgBox.ContainsFocus)
            {
                this.rtbMsgBox.Focus();
            }
        }

        private void rtbMsgBox_VScroll(object sender, EventArgs e)
        {
            if (!this.rtbUpdate && !this.rtbMsgBox.ContainsFocus)
            {
                this.rtbMsgBox.Focus();
            }
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            this.rtbMsgBox.Clear();
            this.msgNumber = 0L;
        }

        private void tsmiClearTransmitQueue_Click(object sender, EventArgs e)
        {
            int qLength = this.devForm.threadMgr.txDataOut.dataQ.GetQLength();
            this.devForm.threadMgr.txDataOut.dataQ.ClearQ();
            string msg = "Pending Transmit Messages Cleared\n" + qLength.ToString() + " Messages Were Discarded\n";
            this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Info, msg);
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            this.rtbMsgBox.Copy();
        }

        private void tsmiDisplayRxDumps_Click(object sender, EventArgs e)
        {
            if (this.tsmiDisplayRxDumps.Checked)
            {
                this.tsmiDisplayRxDumps.Checked = false;
            }
            else
            {
                this.tsmiDisplayRxDumps.Checked = true;
            }
        }

        private void tsmiDisplayRxPackets_Click(object sender, EventArgs e)
        {
            if (this.tsmiDisplayRxPackets.Checked)
            {
                this.tsmiDisplayRxPackets.Checked = false;
            }
            else
            {
                this.tsmiDisplayRxPackets.Checked = true;
            }
        }

        private void tsmiDisplayTxDumps_Click(object sender, EventArgs e)
        {
            if (this.tsmiDisplayTxDumps.Checked)
            {
                this.tsmiDisplayTxDumps.Checked = false;
            }
            else
            {
                this.tsmiDisplayTxDumps.Checked = true;
            }
        }

        private void tsmiDisplayTxPackets_Click(object sender, EventArgs e)
        {
            if (this.tsmiDisplayTxPackets.Checked)
            {
                this.tsmiDisplayTxPackets.Checked = false;
            }
            else
            {
                this.tsmiDisplayTxPackets.Checked = true;
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "*.ble";
            dialog.Filter = "BLE Log Files(*.ble)|*.ble|Text Files(*.txt)|*.txt|All Files|*.*";
            if ((dialog.ShowDialog() == DialogResult.OK) && (dialog.FileName.Length > 0))
            {
                this.rtbMsgBox.SaveFile(dialog.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            this.rtbMsgBox.SelectAll();
        }
    }
}

