﻿namespace BTool
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class FormMain : Form
    {
        private Thread addDeviceThread;
        public static string cmdArgNoVersion = "NoVersion";
        private IContainer components;
        private ComPortTreeForm comPortTreeForm;
        private static Mutex formMainMutex = new Mutex();
        private const ushort HostHandle = 0xfffe;
        private MsgBox msgBox = new MsgBox();
        private MenuStrip msMainMenu;
        private Panel plComPortTree;
        private Panel plDevice;
        public static string ProgramTitle = "BTool - Bluetooth Low Energy PC Application";
        public static string ProgramVersion = " - v1.40.2";
        private SplitContainer scLeftRight;
        private SharedObjects sharedObjs = new SharedObjects();
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiAbout;
        private ToolStripMenuItem tsmiCloseItem;
        private ToolStripMenuItem tsmiDevice;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem tsmiNewDevice;

        public FormMain(CmdLineArgs cmdLineArgs)
        {
            this.InitializeComponent();
            this.Text = ProgramTitle;
            if (!cmdLineArgs.FindCmdLineArg(cmdArgNoVersion))
            {
                this.Text = this.Text + ProgramVersion;
            }
            SharedObjects.mainWin = this;
            this.comPortTreeForm = new ComPortTreeForm();
            this.comPortTreeForm.TopLevel = false;
            this.comPortTreeForm.Parent = this.plComPortTree;
            this.comPortTreeForm.Visible = true;
            this.comPortTreeForm.Dock = DockStyle.Fill;
            this.comPortTreeForm.ControlBox = false;
            this.comPortTreeForm.ShowIcon = false;
            this.comPortTreeForm.FormBorderStyle = FormBorderStyle.None;
            this.comPortTreeForm.StartPosition = FormStartPosition.Manual;
            this.comPortTreeForm.Show();
            this.comPortTreeForm.GetActiveDeviceFormCallback = new GetActiveDeviceFormDelegate(this.GetActiveDeviceForm);
        }

        private void AddDeviceForm()
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new AddDeviceFormDelegate(this.AddDeviceForm), new object[0]);
                }
                catch
                {
                }
            }
            else
            {
                formMainMutex.WaitOne();
                DeviceForm formObj = new DeviceForm();
                if (formObj != null)
                {
                    formObj.BDAddressNotify += new EventHandler(this.DeviceBDAddressNotify);
                    formObj.ConnectionNotify += new EventHandler(this.DeviceConnectionNotify);
                    formObj.DisconnectionNotify += new EventHandler(this.DeviceDisconnectionNotify);
                    formObj.ChangeActiveRoot += new EventHandler(this.DeviceChangeActiveRoot);
                    formObj.CloseActiveDevice += new EventHandler(this.DeviceCloseActiveDevice);
                    if (!formObj.DeviceFormInit())
                    {
                        formObj.DeviceFormClose(false);
                    }
                    else
                    {
                        formObj.TopLevel = false;
                        formObj.Parent = this.plDevice;
                        formObj.Dock = DockStyle.Fill;
                        foreach (Control control in this.plDevice.Controls)
                        {
                            if (control.GetType().BaseType == typeof(Form))
                            {
                                Form form2 = (Form) control;
                                if (form2.Visible)
                                {
                                    form2.Hide();
                                    break;
                                }
                            }
                        }
                        formObj.Show();
                        this.AddToTreeDeviceInfo(formObj.devInfo, formObj);
                        this.comPortTreeForm.ClearSelectedNode();
                        formObj.SendGAPDeviceInit();
                    }
                    formMainMutex.ReleaseMutex();
                }
            }
        }

        private void AddToTreeDeviceInfo(DeviceInfo devInfo, object formObj)
        {
            formMainMutex.WaitOne();
            this.comPortTreeForm.AddPortInfo(devInfo);
            this.DeviceChangeActiveRoot(formObj, null);
            formMainMutex.ReleaseMutex();
        }

        private void DeviceBDAddressNotify(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new DeviceBDAddressNotifyDelegate(this.DeviceBDAddressNotify), new object[] { sender, e });
                }
                catch
                {
                }
            }
            else
            {
                formMainMutex.WaitOne();
                DeviceForm devForm = (DeviceForm) sender;
                if (devForm != null)
                {
                    this.comPortTreeForm.AddDeviceInfo(devForm);
                }
                formMainMutex.ReleaseMutex();
            }
        }

        private void DeviceChangeActiveRoot(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new DeviceChangeActiveRootDelegate(this.DeviceChangeActiveRoot), new object[] { sender, e });
                }
                catch
                {
                }
            }
            else
            {
                formMainMutex.WaitOne();
                DeviceForm devForm = (DeviceForm) sender;
                if (devForm != null)
                {
                    this.comPortTreeForm.ChangeActiveRoot(devForm);
                }
                formMainMutex.ReleaseMutex();
            }
        }

        private void DeviceCloseActiveDevice(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new DeviceCloseActiveDeviceDelegate(this.DeviceCloseActiveDevice), new object[] { sender, e });
                }
                catch
                {
                }
            }
            else
            {
                this.tsmiCloseDevice_Click(sender, null);
            }
        }

        private void DeviceConnectionNotify(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new DeviceConnectionNotifyDelegate(this.DeviceConnectionNotify), new object[] { sender, e });
                }
                catch
                {
                }
            }
            else
            {
                formMainMutex.WaitOne();
                DeviceForm devForm = (DeviceForm) sender;
                if (devForm != null)
                {
                    this.comPortTreeForm.AddConnectionInfo(devForm);
                }
                formMainMutex.ReleaseMutex();
            }
        }

        private void DeviceDisconnectionNotify(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new DeviceDisconnectionNotifyDelegate(this.DeviceConnectionNotify), new object[] { sender, e });
                }
                catch
                {
                }
            }
            else
            {
                formMainMutex.WaitOne();
                DeviceForm devForm = (DeviceForm) sender;
                if (devForm != null)
                {
                    this.comPortTreeForm.DisconnectDevice(devForm);
                }
                formMainMutex.ReleaseMutex();
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

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            formMainMutex.WaitOne();
            this.comPortTreeForm.RemoveAll();
            formMainMutex.ReleaseMutex();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!this.sharedObjs.IsMonoRunning())
            {
                this.tsmiNewDevice_Click(sender, e);
            }
            XmlDataReader reader = new XmlDataReader();
            if (!reader.Read("BToolGattUuid.xml"))
            {
                string msg = "BTool Cannot Read Config Data File\nThe Program Cannot Continue To Run\nHave A Nice Day\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                Environment.Exit(1);
            }
        }

        private DeviceForm GetActiveDeviceForm()
        {
            DeviceForm form = null;
            if (base.InvokeRequired)
            {
                try
                {
                    base.Invoke(new GetActiveDeviceFormDelegate(this.GetActiveDeviceForm));
                }
                catch
                {
                }
                return form;
            }
            formMainMutex.WaitOne();
            foreach (Control control in this.plDevice.Controls)
            {
                if (control.GetType().BaseType == typeof(Form))
                {
                    Form form2 = (Form) control;
                    if (form2.Visible)
                    {
                        form = (DeviceForm) form2;
                        break;
                    }
                }
            }
            formMainMutex.ReleaseMutex();
            return form;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FormMain));
            this.msMainMenu = new MenuStrip();
            this.tsmiDevice = new ToolStripMenuItem();
            this.tsmiNewDevice = new ToolStripMenuItem();
            this.tsmiCloseItem = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripSeparator();
            this.tsmiAbout = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsmiExit = new ToolStripMenuItem();
            this.scLeftRight = new SplitContainer();
            this.plComPortTree = new Panel();
            this.plDevice = new Panel();
            this.msMainMenu.SuspendLayout();
            this.scLeftRight.Panel1.SuspendLayout();
            this.scLeftRight.Panel2.SuspendLayout();
            this.scLeftRight.SuspendLayout();
            base.SuspendLayout();
            this.msMainMenu.Items.AddRange(new ToolStripItem[] { this.tsmiDevice });
            this.msMainMenu.Location = new Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new Size(0x470, 0x18);
            this.msMainMenu.TabIndex = 0;
            this.msMainMenu.Text = "menuStrip1";
            this.tsmiDevice.DropDownItems.AddRange(new ToolStripItem[] { this.tsmiNewDevice, this.tsmiCloseItem, this.toolStripMenuItem2, this.tsmiAbout, this.toolStripSeparator1, this.tsmiExit });
            this.tsmiDevice.MergeAction = MergeAction.Replace;
            this.tsmiDevice.Name = "tsmiDevice";
            this.tsmiDevice.Size = new Size(0x36, 20);
            this.tsmiDevice.Text = "&Device";
            this.tsmiNewDevice.Name = "tsmiNewDevice";
            this.tsmiNewDevice.Size = new Size(0x98, 0x16);
            this.tsmiNewDevice.Text = "&New Device";
            this.tsmiNewDevice.Click += new EventHandler(this.tsmiNewDevice_Click);
            this.tsmiCloseItem.Name = "tsmiCloseItem";
            this.tsmiCloseItem.Size = new Size(0x98, 0x16);
            this.tsmiCloseItem.Text = "&Close Device";
            this.tsmiCloseItem.Click += new EventHandler(this.tsmiCloseDevice_Click);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(0x95, 6);
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new Size(0x98, 0x16);
            this.tsmiAbout.Text = "&About";
            this.tsmiAbout.Click += new EventHandler(this.tsmiAbout_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0x95, 6);
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new Size(0x98, 0x16);
            this.tsmiExit.Text = "&Exit";
            this.tsmiExit.Click += new EventHandler(this.tsmiExit_Click);
            this.scLeftRight.BackColor = SystemColors.Control;
            this.scLeftRight.Dock = DockStyle.Fill;
            this.scLeftRight.Location = new Point(0, 0x18);
            this.scLeftRight.Name = "scLeftRight";
            this.scLeftRight.Panel1.AutoScroll = true;
            this.scLeftRight.Panel1.Controls.Add(this.plComPortTree);
            this.scLeftRight.Panel2.AutoScroll = true;
            this.scLeftRight.Panel2.Controls.Add(this.plDevice);
            this.scLeftRight.Size = new Size(0x470, 0x2fa);
            this.scLeftRight.SplitterDistance = 0xdb;
            this.scLeftRight.TabIndex = 7;
            this.plComPortTree.BackColor = SystemColors.Window;
            this.plComPortTree.Dock = DockStyle.Fill;
            this.plComPortTree.Location = new Point(0, 0);
            this.plComPortTree.Name = "plComPortTree";
            this.plComPortTree.Size = new Size(0xdb, 0x2fa);
            this.plComPortTree.TabIndex = 0;
            this.plDevice.BackColor = SystemColors.AppWorkspace;
            this.plDevice.Dock = DockStyle.Fill;
            this.plDevice.Location = new Point(0, 0);
            this.plDevice.Name = "plDevice";
            this.plDevice.Size = new Size(0x391, 0x2fa);
            this.plDevice.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x470, 0x312);
            base.Controls.Add(this.scLeftRight);
            base.Controls.Add(this.msMainMenu);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.IsMdiContainer = true;
            base.MainMenuStrip = this.msMainMenu;
            base.Name = "FormMain";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.FormClosing += new FormClosingEventHandler(this.FormMain_FormClosing);
            base.Load += new EventHandler(this.FormMain_Load);
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.scLeftRight.Panel1.ResumeLayout(false);
            this.scLeftRight.Panel2.ResumeLayout(false);
            this.scLeftRight.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void tsmiCloseDevice_Click(object sender, EventArgs e)
        {
            formMainMutex.WaitOne();
            DeviceForm activeDeviceForm = this.GetActiveDeviceForm();
            if (activeDeviceForm != null)
            {
                activeDeviceForm.DeviceFormClose(true);
                activeDeviceForm.Close();
                this.comPortTreeForm.RemovePort(activeDeviceForm.devInfo.comPortInfo.comPort);
            }
            this.comPortTreeForm.FindNodeToOpen();
            formMainMutex.ReleaseMutex();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            formMainMutex.WaitOne();
            base.Close();
            formMainMutex.ReleaseMutex();
        }

        private void tsmiNewDevice_Click(object sender, EventArgs e)
        {
            formMainMutex.WaitOne();
            this.addDeviceThread = new Thread(new ThreadStart(this.AddDeviceForm));
            this.addDeviceThread.Name = "AddDeviceFormThread";
            this.addDeviceThread.Start();
            while (!this.addDeviceThread.IsAlive)
            {
                Thread.Sleep(10);
            }
            formMainMutex.ReleaseMutex();
        }

        private delegate void AddDeviceFormDelegate();

        private delegate void DeviceBDAddressNotifyDelegate(object sender, EventArgs e);

        private delegate void DeviceChangeActiveRootDelegate(object sender, EventArgs e);

        private delegate void DeviceCloseActiveDeviceDelegate(object sender, EventArgs e);

        private delegate void DeviceConnectionNotifyDelegate(object sender, EventArgs e);

        private delegate void DeviceDisconnectionNotifyDelegate(object sender, EventArgs e);
    }
}

