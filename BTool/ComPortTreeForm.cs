﻿namespace BTool
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ComPortTreeForm : Form
    {
        private Font boldFont;
        private ContextMenuStrip cmsTreeBda;
        private ContextMenuStrip cmsTreeComPort;
        private ContextMenuStrip cmsTreeHandle;
        private IContainer components;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public GetActiveDeviceFormDelegate GetActiveDeviceFormCallback;
        private const ushort HostHandle = 0xfffe;
        public static string moduleName = "ComPortTreeForm";
        private MsgBox msgBox = new MsgBox();
        private const string NodeNames_Baudrate = "Baudrate";
        private const string NodeNames_ConnectionInfo = "ConnectionInfo";
        private const string NodeNames_DataBits = "DataBits";
        private const string NodeNames_DeviceInfo = "DeviceInfo";
        private const string NodeNames_FlowControl = "FlowControl";
        private const string NodeNames_HostBda = "HostBda";
        private const string NodeNames_HostHandle = "HostHandle";
        private const string NodeNames_Parity = "Parity";
        private const string NodeNames_Port = "Port";
        private const string NodeNames_PortInfo = "PortInfo";
        private const string NodeNames_PortName = "PortName";
        private const string NodeNames_SlaveAddrType = "SlaveAddrType";
        private const string NodeNames_SlaveBda = "SlaveBda";
        private const string NodeNames_SlaveHandle = "SlaveHandle";
        private const string NodeNames_StopBits = "StopBits";
        private Font regularFont;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private TreeViewUtils treeViewUtils = new TreeViewUtils();
        private ToolStripMenuItem tsmiClearTransmitQueue;
        private ToolStripMenuItem tsmiClearTransmitQueue2;
        private ToolStripMenuItem tsmiCopyAddress;
        private ToolStripMenuItem tsmiCopyHandle;
        private ToolStripMenuItem tsmiDiscoverAllUuids;
        private ToolStripMenuItem tsmiDiscoverUuids;
        private ToolStripMenuItem tsmiReadAllValues;
        private ToolStripMenuItem tsmiReadValues;
        private ToolStripMenuItem tsmiSetConnectionHandle;
        private TreeViewWrapper tvPorts;
        private Font underlineFont;

        public ComPortTreeForm()
        {
            this.InitializeComponent();
            this.boldFont = new Font(this.tvPorts.Font, FontStyle.Bold);
            this.underlineFont = new Font(this.tvPorts.Font, FontStyle.Underline);
            this.regularFont = new Font(this.tvPorts.Font, FontStyle.Regular);
            this.tvPorts.ShowNodeToolTips = true;
            this.tvPorts.ContextMenuStrip = null;
        }

        public bool AddConnectionInfo(DeviceForm devForm)
        {
            bool flag = true;
            ConnectInfo connectInfo = devForm.GetConnectInfo();
            if (devForm != null)
            {
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    DeviceInfo tag = (DeviceInfo) node.Tag;
                    if (tag.comPortInfo.comPort == devForm.devInfo.comPortInfo.comPort)
                    {
                        TreeNode node2 = new TreeNode();
                        node2.Name = NodeNames.ConnectionInfo.ToString();
                        node2.Text = string.Format("Connection Info:", new object[0]);
                        node2.NodeFont = this.underlineFont;
                        node2.Tag = node.Tag;
                        node2.ToolTipText = string.Format("Device Connection Information (Over the Air Connection)", new object[0]);
                        TreeNode node3 = new TreeNode();
                        node3.Name = NodeNames.SlaveHandle.ToString();
                        node3.Text = string.Format("Handle: 0x{0:X4}", connectInfo.handle);
                        tag.connectInfo.handle = connectInfo.handle;
                        node3.Tag = node.Tag;
                        node3.ToolTipText = string.Format("Connection Handle\nSelect Handle Then Right Click To See Options.", new object[0]);
                        TreeNode node4 = new TreeNode();
                        node4.Name = NodeNames.SlaveAddrType.ToString();
                        node4.Text = string.Format("Addr Type: 0x{0:X2} ({1:S})", connectInfo.addrType, this.devUtils.GetGapAddrTypeStr(connectInfo.addrType));
                        node4.Tag = node.Tag;
                        node4.ToolTipText = string.Format("Address Type", new object[0]);
                        TreeNode node5 = new TreeNode();
                        node5.Name = NodeNames.SlaveBda.ToString();
                        node5.Text = string.Format("Slave BDA: {0:S}", connectInfo.bDA);
                        node5.Tag = node.Tag;
                        node5.ToolTipText = string.Format("Slave Bluetooth Device Address\nSelect Address Then Right Click To See Options.", new object[0]);
                        node.Nodes.Add(node2);
                        node2.Nodes.Add(node3);
                        node2.Nodes.Add(node4);
                        node2.Nodes.Add(node5);
                        node2.Expand();
                    }
                }
                return flag;
            }
            return false;
        }

        public bool AddDeviceInfo(DeviceForm devForm)
        {
            bool flag = true;
            string bDAddressStr = devForm.BDAddressStr;
            if (devForm != null)
            {
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    DeviceInfo tag = (DeviceInfo) node.Tag;
                    if (tag.comPortInfo.comPort == devForm.devInfo.comPortInfo.comPort)
                    {
                        TreeNode node2 = new TreeNode();
                        node2.Name = NodeNames.DeviceInfo.ToString();
                        node2.Text = string.Format("Device Info:", new object[0]);
                        node2.NodeFont = this.underlineFont;
                        node2.Tag = node.Tag;
                        node2.ToolTipText = string.Format("Information About The Direct Connect Device.", new object[0]);
                        TreeNode node3 = new TreeNode();
                        node3.Name = NodeNames.HostHandle.ToString();
                        node3.Text = string.Format("Handle: 0x{0:X4}", (ushort) 0xfffe);
                        tag.handle = 0xfffe;
                        node3.Tag = node.Tag;
                        node3.ToolTipText = string.Format("Device Handle\nSelect Handle Then Right Click To See Options.", new object[0]);
                        TreeNode node4 = new TreeNode();
                        node4.Name = NodeNames.HostBda.ToString();
                        node4.Text = string.Format("BDAddr: {0:S}", bDAddressStr);
                        node4.Tag = node.Tag;
                        node4.ToolTipText = string.Format("Bluetooth Device Address\nSelect Address Then Right Click To See Options.", new object[0]);
                        if (node.FirstNode.NextNode == null)
                        {
                            node.Nodes.Add(node2);
                            node2.Nodes.Add(node3);
                            node2.Nodes.Add(node4);
                            node2.Expand();
                        }
                    }
                }
                return flag;
            }
            return false;
        }

        public bool AddPortInfo(DeviceInfo devInfo)
        {
            TreeNode node = new TreeNode();
            node.Name = NodeNames.PortName.ToString();
            node.Text = devInfo.comPortInfo.comPort;
            node.Tag = devInfo;
            node.ToolTipText = string.Format("Device Port Name\nSelect Port Name To Switch View To This Device\nSelect Port Name Then Right Click To See Options.", new object[0]);
            this.tvPorts.Nodes.Add(node);
            node.NodeFont = this.boldFont;
            TreeNode node2 = new TreeNode();
            node2.Name = NodeNames.PortInfo.ToString();
            node2.Text = "Port Info";
            node2.Tag = devInfo;
            node2.ToolTipText = string.Format("Information About The Device Port", new object[0]);
            node.Nodes.Add(node2);
            node2.NodeFont = this.underlineFont;
            TreeNode node3 = new TreeNode();
            node3.Name = NodeNames.Port.ToString();
            node3.Text = string.Format("Port: {0:S}", devInfo.comPortInfo.comPort);
            node3.Tag = devInfo;
            node3.ToolTipText = string.Format("Port Name", new object[0]);
            node2.Nodes.Add(node3);
            TreeNode node4 = new TreeNode();
            node4.Name = NodeNames.Baudrate.ToString();
            node4.Text = string.Format("Baudrate: {0:S}", devInfo.comPortInfo.baudRate);
            node4.Tag = devInfo;
            node4.ToolTipText = string.Format("Port Baudrate", new object[0]);
            node2.Nodes.Add(node4);
            TreeNode node5 = new TreeNode();
            node5.Name = NodeNames.FlowControl.ToString();
            node5.Text = string.Format("Flow Control: {0:S}", devInfo.comPortInfo.flow);
            node5.Tag = devInfo;
            node5.ToolTipText = string.Format("Port Flow Of Control Method", new object[0]);
            node2.Nodes.Add(node5);
            TreeNode node6 = new TreeNode();
            node6.Name = NodeNames.DataBits.ToString();
            node6.Text = string.Format("Data Bits: {0:S}", devInfo.comPortInfo.dataBits);
            node6.Tag = devInfo;
            node6.ToolTipText = string.Format("Port Data Bits", new object[0]);
            node2.Nodes.Add(node6);
            TreeNode node7 = new TreeNode();
            node7.Name = NodeNames.Parity.ToString();
            node7.Text = string.Format("Parity: {0:S}", devInfo.comPortInfo.parity);
            node7.ToolTipText = string.Format("Port Parity Bits", new object[0]);
            node7.Tag = devInfo;
            node2.Nodes.Add(node7);
            TreeNode node8 = new TreeNode();
            node8.Name = NodeNames.StopBits.ToString();
            node8.Text = string.Format("Stop Bits: {0:S}", devInfo.comPortInfo.stopBits);
            node8.Tag = devInfo;
            node8.ToolTipText = string.Format("Port Stop Bits", new object[0]);
            node2.Nodes.Add(node8);
            node.Expand();
            return true;
        }

        public bool ChangeActiveRoot(DeviceForm devForm)
        {
            bool flag = false;
            if (devForm != null)
            {
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    DeviceInfo tag = (DeviceInfo) node.Tag;
                    if (tag.devName == devForm.devInfo.devName)
                    {
                        node.NodeFont = this.underlineFont;
                    }
                    else
                    {
                        node.NodeFont = this.regularFont;
                    }
                }
                return flag;
            }
            return false;
        }

        public void ClearSelectedNode()
        {
            this.treeViewUtils.ClearSelectedNode(this.tvPorts);
        }

        public bool DisconnectDevice(DeviceForm devForm)
        {
            bool flag = false;
            ConnectInfo disconnectInfo = devForm.disconnectInfo;
            if (devForm != null)
            {
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    DeviceInfo tag = (DeviceInfo) node.Tag;
                    if (tag.comPortInfo.comPort == devForm.devInfo.comPortInfo.comPort)
                    {
                        string target = "";
                        target = string.Format("Handle: 0x{0:X4}", disconnectInfo.handle);
                        SharedObjects.log.Write(Logging.MsgType.Debug, moduleName, "Disconnecting Device " + target);
                        if (flag = this.treeViewUtils.TreeNodeTextSearchAndDestroy(node, target))
                        {
                            return flag;
                        }
                    }
                    if (flag)
                    {
                        return flag;
                    }
                }
                return flag;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool FindNodeToOpen()
        {
            foreach (TreeNode node in this.tvPorts.Nodes)
            {
                DeviceInfo tag = (DeviceInfo) node.Tag;
                DeviceForm devForm = tag.devForm;
                if (devForm != null)
                {
                    devForm.Show();
                    node.NodeFont = this.underlineFont;
                }
            }
            return true;
        }

        private void GetTreeTextRecursive_DiscoverAllUuids(TreeNode treeNode)
        {
            this.SendGattDiscoverCmds(treeNode, TxDataOut.CmdType.DiscUuidOnly);
            foreach (TreeNode node in treeNode.Nodes)
            {
                this.GetTreeTextRecursive_DiscoverAllUuids(node);
            }
        }

        private void GetTreeTextRecursive_ReadAllValues(TreeNode treeNode)
        {
            this.SendGattDiscoverCmds(treeNode, TxDataOut.CmdType.DiscUuidAndValues);
            foreach (TreeNode node in treeNode.Nodes)
            {
                this.GetTreeTextRecursive_ReadAllValues(node);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tvPorts = new TreeViewWrapper();
            this.cmsTreeHandle = new ContextMenuStrip(this.components);
            this.tsmiSetConnectionHandle = new ToolStripMenuItem();
            this.tsmiCopyHandle = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.tsmiDiscoverUuids = new ToolStripMenuItem();
            this.tsmiReadValues = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.tsmiClearTransmitQueue2 = new ToolStripMenuItem();
            this.cmsTreeBda = new ContextMenuStrip(this.components);
            this.tsmiCopyAddress = new ToolStripMenuItem();
            this.cmsTreeComPort = new ContextMenuStrip(this.components);
            this.tsmiDiscoverAllUuids = new ToolStripMenuItem();
            this.tsmiReadAllValues = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.tsmiClearTransmitQueue = new ToolStripMenuItem();
            this.cmsTreeHandle.SuspendLayout();
            this.cmsTreeBda.SuspendLayout();
            this.cmsTreeComPort.SuspendLayout();
            base.SuspendLayout();
            this.tvPorts.Dock = DockStyle.Fill;
            this.tvPorts.Indent = 12;
            this.tvPorts.Location = new Point(0, 0);
            this.tvPorts.Name = "tvPorts";
            this.tvPorts.Size = new Size(210, 0x242);
            this.tvPorts.TabIndex = 4;
            this.tvPorts.AfterSelect += new TreeViewEventHandler(this.tvPorts_AfterSelect);
            this.cmsTreeHandle.Items.AddRange(new ToolStripItem[] { this.tsmiSetConnectionHandle, this.tsmiCopyHandle, this.toolStripSeparator2, this.tsmiDiscoverUuids, this.tsmiReadValues, this.toolStripSeparator4, this.tsmiClearTransmitQueue2 });
            this.cmsTreeHandle.Name = "contextMenuStrip1";
            this.cmsTreeHandle.Size = new Size(0xc5, 0x7e);
            this.tsmiSetConnectionHandle.Name = "tsmiSetConnectionHandle";
            this.tsmiSetConnectionHandle.Size = new Size(0xc4, 0x16);
            this.tsmiSetConnectionHandle.Text = "&Set Connection Handle";
            this.tsmiSetConnectionHandle.ToolTipText = "Set The Connection Handle Used By BTool";
            this.tsmiSetConnectionHandle.Click += new EventHandler(this.tsmiSetConnectionHandle_Click);
            this.tsmiCopyHandle.Name = "tsmiCopyHandle";
            this.tsmiCopyHandle.Size = new Size(0xc4, 0x16);
            this.tsmiCopyHandle.Text = "&Copy Handle";
            this.tsmiCopyHandle.ToolTipText = "Copy The Handle To The Clipboard";
            this.tsmiCopyHandle.Click += new EventHandler(this.tsmiCopyHandle_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0xc1, 6);
            this.tsmiDiscoverUuids.Name = "tsmiDiscoverUuids";
            this.tsmiDiscoverUuids.Size = new Size(0xc4, 0x16);
            this.tsmiDiscoverUuids.Text = "&Discover UUIDs";
            this.tsmiDiscoverUuids.ToolTipText = "Start A Message Sequence To Discover UUID's";
            this.tsmiDiscoverUuids.Click += new EventHandler(this.tsmiDiscoverUuids_Click);
            this.tsmiReadValues.Name = "tsmiReadValues";
            this.tsmiReadValues.Size = new Size(0xc4, 0x16);
            this.tsmiReadValues.Text = "&Read Values";
            this.tsmiReadValues.ToolTipText = "Start A Message Sequence To Discover UUID's And Read Values";
            this.tsmiReadValues.Click += new EventHandler(this.tsmiReadValues_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(0xc1, 6);
            this.tsmiClearTransmitQueue2.Name = "tsmiClearTransmitQueue2";
            this.tsmiClearTransmitQueue2.Size = new Size(0xc4, 0x16);
            this.tsmiClearTransmitQueue2.Text = "Clear Transmit &Queue";
            this.tsmiClearTransmitQueue2.ToolTipText = "Clears All Pending Transmit Commands";
            this.tsmiClearTransmitQueue2.Click += new EventHandler(this.tsmiClearTransmitQ_Click);
            this.cmsTreeBda.Items.AddRange(new ToolStripItem[] { this.tsmiCopyAddress });
            this.cmsTreeBda.Name = "cmsTreeBda";
            this.cmsTreeBda.Size = new Size(0x94, 0x1a);
            this.tsmiCopyAddress.Name = "tsmiCopyAddress";
            this.tsmiCopyAddress.Size = new Size(0x93, 0x16);
            this.tsmiCopyAddress.Text = "&Copy Address";
            this.tsmiCopyAddress.ToolTipText = "Copy The Address To The Clipboard";
            this.tsmiCopyAddress.Click += new EventHandler(this.tsmiCopyAddress_Click);
            this.cmsTreeComPort.Items.AddRange(new ToolStripItem[] { this.tsmiDiscoverAllUuids, this.tsmiReadAllValues, this.toolStripSeparator3, this.tsmiClearTransmitQueue });
            this.cmsTreeComPort.Name = "cmsTreeComPort";
            this.cmsTreeComPort.Size = new Size(0x15f, 0x4c);
            this.tsmiDiscoverAllUuids.Name = "tsmiDiscoverAllUuids";
            this.tsmiDiscoverAllUuids.Size = new Size(350, 0x16);
            this.tsmiDiscoverAllUuids.Text = "Discover &UUIDs (All Devices Connected To This Port)";
            this.tsmiDiscoverAllUuids.ToolTipText = "Start A Message Sequence To Discover UUID's \r\nOn All Connected Devices To This Port";
            this.tsmiDiscoverAllUuids.Click += new EventHandler(this.tsmiDiscoverAllUuids_Click);
            this.tsmiReadAllValues.Name = "tsmiReadAllValues";
            this.tsmiReadAllValues.Size = new Size(350, 0x16);
            this.tsmiReadAllValues.Text = "Read &Values (All Devices Connected To This Port)";
            this.tsmiReadAllValues.ToolTipText = "Start A Message Sequence To Discover UUID's And Read Values \r\nOn All Connected Devices To This Port\r\n";
            this.tsmiReadAllValues.Click += new EventHandler(this.tsmiReadAllValues_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(0x15b, 6);
            this.tsmiClearTransmitQueue.Name = "tsmiClearTransmitQueue";
            this.tsmiClearTransmitQueue.Size = new Size(350, 0x16);
            this.tsmiClearTransmitQueue.Text = "&Clear Transmit Queue";
            this.tsmiClearTransmitQueue.ToolTipText = "Clears All Pending Transmit Commands";
            this.tsmiClearTransmitQueue.Click += new EventHandler(this.tsmiClearTransmitQ_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(210, 0x242);
            base.Controls.Add(this.tvPorts);
            base.Name = "ComPortTreeForm";
            this.Text = "Com Port Tree";
            this.cmsTreeHandle.ResumeLayout(false);
            this.cmsTreeBda.ResumeLayout(false);
            this.cmsTreeComPort.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public bool RemoveAll()
        {
            bool flag = false;
            if (this.tvPorts.Nodes != null)
            {
                DeviceForm devForm = null;
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    if (node != null)
                    {
                        DeviceInfo tag = (DeviceInfo) node.Tag;
                        devForm = tag.devForm;
                        devForm.DeviceFormClose(true);
                        devForm.Close();
                        this.treeViewUtils.RemoveTextFromTree(this.tvPorts, devForm.devInfo.comPortInfo.comPort);
                    }
                }
                return flag;
            }
            return false;
        }

        public bool RemovePort(string portName)
        {
            this.treeViewUtils.RemoveTextFromTree(this.tvPorts, portName);
            return true;
        }

        private void SendGattDiscoverCmds(TreeNode treeNode, TxDataOut.CmdType cmdType)
        {
            DeviceForm form = this.GetActiveDeviceFormCallback();
            if (((form != null) && (treeNode != null)) && ((treeNode.Name == "HostHandle") || (treeNode.Name == "SlaveHandle")))
            {
                string str = treeNode.Text.Replace("Handle: ", "");
                if (str != null)
                {
                    try
                    {
                        ushort num = Convert.ToUInt16(str, 0x10);
                        HCICmds.GATTCmds.GATT_DiscAllPrimaryServices services = new HCICmds.GATTCmds.GATT_DiscAllPrimaryServices();
                        services.connHandle = num;
                        form.sendCmds.SendGATT(services, cmdType);
                        HCICmds.GATTCmds.GATT_DiscAllCharDescs descs = new HCICmds.GATTCmds.GATT_DiscAllCharDescs();
                        descs.connHandle = num;
                        form.sendCmds.SendGATT(descs, cmdType);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void tsmiClearTransmitQ_Click(object sender, EventArgs e)
        {
            DeviceForm form = this.GetActiveDeviceFormCallback();
            if (form != null)
            {
                int qLength = form.threadMgr.txDataOut.dataQ.GetQLength();
                form.threadMgr.txDataOut.dataQ.ClearQ();
                string msg = "Pending Transmit Messages Cleared\n" + qLength.ToString() + " Messages Were Discarded\n";
                form.DisplayMsg(SharedAppObjs.MsgType.Info, msg);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Info, msg);
            }
        }

        private void tsmiCopyAddress_Click(object sender, EventArgs e)
        {
            if (this.GetActiveDeviceFormCallback() != null)
            {
                TreeNode selectedNode = this.tvPorts.SelectedNode;
                if (selectedNode != null)
                {
                    string text = null;
                    if (selectedNode.Name == "HostBda")
                    {
                        text = selectedNode.Text.Replace("BDAddr: ", "");
                    }
                    if (selectedNode.Name == "SlaveBda")
                    {
                        text = selectedNode.Text.Replace("Slave BDA: ", "");
                    }
                    if (text != null)
                    {
                        Clipboard.SetText(text);
                    }
                }
            }
        }

        private void tsmiCopyHandle_Click(object sender, EventArgs e)
        {
            if (this.GetActiveDeviceFormCallback() != null)
            {
                TreeNode selectedNode = this.tvPorts.SelectedNode;
                if ((selectedNode != null) && ((selectedNode.Name == "HostHandle") || (selectedNode.Name == "SlaveHandle")))
                {
                    string text = selectedNode.Text.Replace("Handle: ", "");
                    if (text != null)
                    {
                        Clipboard.SetText(text);
                    }
                }
            }
        }

        private void tsmiDiscoverAllUuids_Click(object sender, EventArgs e)
        {
            if (this.tvPorts != null)
            {
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    this.GetTreeTextRecursive_DiscoverAllUuids(node);
                }
            }
        }

        private void tsmiDiscoverUuids_Click(object sender, EventArgs e)
        {
            if ((this.GetActiveDeviceFormCallback() != null) && (this.tvPorts != null))
            {
                TreeNode selectedNode = this.tvPorts.SelectedNode;
                this.SendGattDiscoverCmds(selectedNode, TxDataOut.CmdType.DiscUuidOnly);
            }
        }

        private void tsmiReadAllValues_Click(object sender, EventArgs e)
        {
            if (this.tvPorts != null)
            {
                foreach (TreeNode node in this.tvPorts.Nodes)
                {
                    this.GetTreeTextRecursive_ReadAllValues(node);
                }
            }
        }

        private void tsmiReadValues_Click(object sender, EventArgs e)
        {
            if ((this.GetActiveDeviceFormCallback() != null) && (this.tvPorts != null))
            {
                TreeNode selectedNode = this.tvPorts.SelectedNode;
                this.SendGattDiscoverCmds(selectedNode, TxDataOut.CmdType.DiscUuidAndValues);
            }
        }

        private void tsmiSetConnectionHandle_Click(object sender, EventArgs e)
        {
            DeviceForm form = this.GetActiveDeviceFormCallback();
            if (form != null)
            {
                TreeNode selectedNode = this.tvPorts.SelectedNode;
                if ((selectedNode != null) && ((selectedNode.Name == "HostHandle") || (selectedNode.Name == "SlaveHandle")))
                {
                    string str = selectedNode.Text.Replace("Handle: ", "");
                    if (str != null)
                    {
                        ushort handle = Convert.ToUInt16(str, 0x10);
                        form.devTabsForm.SetConnHandles(handle);
                    }
                }
            }
        }

        private void tvPorts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string name = e.Node.Name;
            DeviceInfo tag = (DeviceInfo) e.Node.Tag;
            this.tvPorts.ContextMenuStrip = null;
            DeviceForm form = this.GetActiveDeviceFormCallback();
            if (form != null)
            {
                if (tag.comPortInfo.comPort != form.devInfo.comPortInfo.comPort)
                {
                    tag.devForm.Show();
                    form.devInfo.devForm.Hide();
                }
                switch (name)
                {
                    case "PortName":
                        if (tag.comPortInfo.comPort == form.devInfo.comPortInfo.comPort)
                        {
                            this.tvPorts.ContextMenuStrip = this.cmsTreeComPort;
                        }
                        return;

                    case "PortInfo":
                    case "Port":
                    case "Baudrate":
                    case "FlowControl":
                    case "DataBits":
                    case "Parity":
                    case "StopBits":
                    case "DeviceInfo":
                    case "ConnectionInfo":
                    case "SlaveAddrType":
                        return;

                    case "HostHandle":
                    case "SlaveHandle":
                        if (tag.comPortInfo.comPort == form.devInfo.comPortInfo.comPort)
                        {
                            this.tvPorts.ContextMenuStrip = this.cmsTreeHandle;
                        }
                        return;

                    case "HostBda":
                    case "SlaveBda":
                        if (tag.comPortInfo.comPort == form.devInfo.comPortInfo.comPort)
                        {
                            this.tvPorts.ContextMenuStrip = this.cmsTreeBda;
                        }
                        return;
                }
                string msg = string.Format("Unknown Tree Node Name = {0}\n", name);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        }

        private enum NodeNames
        {
            PortName,
            PortInfo,
            Port,
            Baudrate,
            FlowControl,
            DataBits,
            Parity,
            StopBits,
            DeviceInfo,
            HostHandle,
            HostBda,
            ConnectionInfo,
            SlaveHandle,
            SlaveAddrType,
            SlaveBda
        }
    }
}
