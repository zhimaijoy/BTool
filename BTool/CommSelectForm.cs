﻿namespace BTool
{
    using BTool.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO.Ports;
    using System.Windows.Forms;

    public class CommSelectForm : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        public ComboBox cbBaud;
        public ComboBox cbDataBits;
        public ComboBox cbFlow;
        public ComboBox cbParity;
        public ComboBox cbPorts;
        public ComboBox cbStopBits;
        private IContainer components;
        private GroupBox gbPortSettings;
        private Label lblBaud;
        private Label lblDataBits;
        private Label lblFlow;
        private Label lblParity;
        private Label lblPort;
        private Label lblStopBits;
        private MonoUtils monoUtils = new MonoUtils();
        private MsgBox msgBox = new MsgBox();
        private SharedObjects sharedObjs = new SharedObjects();

        public CommSelectForm()
        {
            this.InitializeComponent();
        }

        private void commSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            string str = string.Empty;
            str = this.cbPorts.Items[this.cbPorts.SelectedIndex].ToString();
            Settings.Default.ComPortName = str;
            str = this.cbBaud.Items[this.cbBaud.SelectedIndex].ToString();
            Settings.Default.Baud = str;
            str = this.cbFlow.Items[this.cbFlow.SelectedIndex].ToString();
            Settings.Default.Flow = str;
            str = this.cbParity.Items[this.cbParity.SelectedIndex].ToString();
            Settings.Default.Parity = str;
            str = this.cbStopBits.Items[this.cbStopBits.SelectedIndex].ToString();
            Settings.Default.StopBits = str;
            str = this.cbDataBits.Items[this.cbDataBits.SelectedIndex].ToString();
            Settings.Default.DataBits = str;
            Settings.Default.Save();
        }

        private void commSelect_FormLoad(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();
            if (!this.sharedObjs.IsMonoRunning())
            {
                this.SortComPorts(portNames);
            }
            try
            {
                string comPortName = Settings.Default.ComPortName;
            }
            catch
            {
            }
            int num = 0;
            int num2 = 0;
            try
            {
                if ((portNames.Length > 0) && (Settings.Default.ComPortName != null))
                {
                    foreach (string str in portNames)
                    {
                        this.cbPorts.Items.Add(str);
                        if (str == Settings.Default.ComPortName)
                        {
                            num2 = num;
                        }
                        num++;
                    }
                }
                else
                {
                    this.cbPorts.Items.Add("No Ports Found");
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid COM Port Name Found During Form Load.\nPort Name Load Stopped Before Completion.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                if (num == 0)
                {
                    this.cbPorts.Items.Add("No Ports Found");
                }
            }
            this.cbPorts.SelectedIndex = num2;
            num = 0;
            num2 = -1;
            if ((this.cbBaud.Items.Count > 0) && (Settings.Default.Baud != null))
            {
                while (num < this.cbBaud.Items.Count)
                {
                    if (this.cbBaud.Items[num].ToString() == Settings.Default.Baud)
                    {
                        num2 = num;
                        break;
                    }
                    num++;
                }
            }
            if (num2 != -1)
            {
                this.cbBaud.SelectedIndex = num2;
            }
            else
            {
                this.cbBaud.SelectedIndex = 2;
            }
            num = 0;
            num2 = -1;
            if ((this.cbDataBits.Items.Count > 0) && (Settings.Default.DataBits != null))
            {
                while (num < this.cbDataBits.Items.Count)
                {
                    if (this.cbDataBits.Items[num].ToString() == Settings.Default.DataBits)
                    {
                        num2 = num;
                        break;
                    }
                    num++;
                }
            }
            if (num2 != -1)
            {
                this.cbDataBits.SelectedIndex = num2;
            }
            else
            {
                this.cbDataBits.SelectedIndex = 1;
            }
            num = 0;
            num2 = -1;
            if ((this.cbParity.Items.Count > 0) && (Settings.Default.Parity != null))
            {
                while (num < this.cbParity.Items.Count)
                {
                    if (this.cbParity.Items[num].ToString() == Settings.Default.Parity)
                    {
                        num2 = num;
                        break;
                    }
                    num++;
                }
            }
            if (num2 != -1)
            {
                this.cbParity.SelectedIndex = num2;
            }
            else
            {
                this.cbParity.SelectedIndex = 0;
            }
            num = 0;
            num2 = -1;
            if ((this.cbStopBits.Items.Count > 0) && (Settings.Default.StopBits != null))
            {
                while (num < this.cbStopBits.Items.Count)
                {
                    if (this.cbStopBits.Items[num].ToString() == Settings.Default.StopBits)
                    {
                        num2 = num;
                        break;
                    }
                    num++;
                }
            }
            if (num2 != -1)
            {
                this.cbStopBits.SelectedIndex = num2;
            }
            else
            {
                this.cbStopBits.SelectedIndex = 1;
            }
            num = 0;
            num2 = -1;
            if ((this.cbFlow.Items.Count > 0) && (Settings.Default.Flow != null))
            {
                while (num < this.cbFlow.Items.Count)
                {
                    if (this.cbFlow.Items[num].ToString() == Settings.Default.Flow)
                    {
                        num2 = num;
                        break;
                    }
                    num++;
                }
            }
            if (num2 != -1)
            {
                this.cbFlow.SelectedIndex = num2;
            }
            else
            {
                this.cbFlow.SelectedIndex = 2;
            }
            this.monoUtils.SetMaximumSize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CommSelectForm));
            this.cbPorts = new ComboBox();
            this.buttonOK = new Button();
            this.lblPort = new Label();
            this.cbBaud = new ComboBox();
            this.lblBaud = new Label();
            this.cbParity = new ComboBox();
            this.cbStopBits = new ComboBox();
            this.cbDataBits = new ComboBox();
            this.lblParity = new Label();
            this.lblStopBits = new Label();
            this.lblDataBits = new Label();
            this.gbPortSettings = new GroupBox();
            this.lblFlow = new Label();
            this.cbFlow = new ComboBox();
            this.buttonCancel = new Button();
            this.gbPortSettings.SuspendLayout();
            base.SuspendLayout();
            this.cbPorts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPorts.FormattingEnabled = true;
            this.cbPorts.Location = new Point(0x44, 0x10);
            this.cbPorts.Name = "cbPorts";
            this.cbPorts.Size = new Size(140, 0x15);
            this.cbPorts.TabIndex = 0;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x19, 0xcc);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new Point(0x21, 0x13);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new Size(0x1d, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            this.cbBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbBaud.FormattingEnabled = true;
            this.cbBaud.Items.AddRange(new object[] { "38400", "57600", "115200" });
            this.cbBaud.Location = new Point(0x44, 0x2b);
            this.cbBaud.Name = "cbBaud";
            this.cbBaud.Size = new Size(140, 0x15);
            this.cbBaud.TabIndex = 3;
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new Point(0x1b, 0x2e);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new Size(0x23, 13);
            this.lblBaud.TabIndex = 4;
            this.lblBaud.Text = "Baud:";
            this.cbParity.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
            this.cbParity.Location = new Point(0x44, 0x61);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new Size(140, 0x15);
            this.cbParity.TabIndex = 5;
            this.cbStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Items.AddRange(new object[] { "None", "One", "Two" });
            this.cbStopBits.Location = new Point(0x44, 0x7c);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new Size(140, 0x15);
            this.cbStopBits.TabIndex = 6;
            this.cbDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] { "7", "8", "9" });
            this.cbDataBits.Location = new Point(0x44, 0x94);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new Size(140, 0x15);
            this.cbDataBits.TabIndex = 7;
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new Point(0x1a, 100);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new Size(0x24, 13);
            this.lblParity.TabIndex = 8;
            this.lblParity.Text = "Parity:";
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new Point(10, 0x7f);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new Size(0x34, 13);
            this.lblStopBits.TabIndex = 9;
            this.lblStopBits.Text = "Stop Bits:";
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new Point(9, 0x97);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new Size(0x35, 13);
            this.lblDataBits.TabIndex = 10;
            this.lblDataBits.Text = "Data Bits:";
            this.gbPortSettings.Controls.Add(this.lblFlow);
            this.gbPortSettings.Controls.Add(this.cbFlow);
            this.gbPortSettings.Controls.Add(this.lblPort);
            this.gbPortSettings.Controls.Add(this.lblDataBits);
            this.gbPortSettings.Controls.Add(this.cbPorts);
            this.gbPortSettings.Controls.Add(this.lblStopBits);
            this.gbPortSettings.Controls.Add(this.cbBaud);
            this.gbPortSettings.Controls.Add(this.lblParity);
            this.gbPortSettings.Controls.Add(this.lblBaud);
            this.gbPortSettings.Controls.Add(this.cbDataBits);
            this.gbPortSettings.Controls.Add(this.cbParity);
            this.gbPortSettings.Controls.Add(this.cbStopBits);
            this.gbPortSettings.Location = new Point(12, 5);
            this.gbPortSettings.Name = "gbPortSettings";
            this.gbPortSettings.Size = new Size(0xdf, 0xb8);
            this.gbPortSettings.TabIndex = 11;
            this.gbPortSettings.TabStop = false;
            this.lblFlow.AutoSize = true;
            this.lblFlow.Location = new Point(30, 0x49);
            this.lblFlow.Name = "lblFlow";
            this.lblFlow.Size = new Size(0x20, 13);
            this.lblFlow.TabIndex = 12;
            this.lblFlow.Text = "Flow:";
            this.cbFlow.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbFlow.FormattingEnabled = true;
            this.cbFlow.Items.AddRange(new object[] { "None", "XON/XOFF", "CTS/RTS", "XON/XOFF + CTS/RTS" });
            this.cbFlow.Location = new Point(0x44, 70);
            this.cbFlow.Name = "cbFlow";
            this.cbFlow.Size = new Size(140, 0x15);
            this.cbFlow.TabIndex = 11;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x90, 0xcd);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0xf7, 0xf5);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.gbPortSettings);
            base.Controls.Add(this.buttonOK);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0xfd, 0x111);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0xfd, 0x111);
            base.Name = "CommSelectForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = " Serial Port Settings";
            base.FormClosing += new FormClosingEventHandler(this.commSelect_FormClosing);
            base.Load += new EventHandler(this.commSelect_FormLoad);
            this.gbPortSettings.ResumeLayout(false);
            this.gbPortSettings.PerformLayout();
            base.ResumeLayout(false);
        }

        private int SortComPorts(string[] rgstrPorts)
        {
            try
            {
                Array.Sort<string>(rgstrPorts, delegate (string strA, string strB) {
                    int num = int.Parse(strA.Substring(3));
                    int num2 = int.Parse(strB.Substring(3));
                    return num.CompareTo(num2);
                });
            }
            catch (Exception exception)
            {
                string msg = string.Format("Invalid COM Port Name Found During Sort.\nSort Terminated.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            return 0;
        }
    }
}

