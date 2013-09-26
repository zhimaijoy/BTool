﻿namespace BTool
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;

    public class CommManager
    {
        private string _baudRate = string.Empty;
        private string _dataBits = string.Empty;
        private Handshake _handShake;
        private string _parity = string.Empty;
        private string _portName = string.Empty;
        private string _stopBits = string.Empty;
        private TransmissionType _transType;
        public SerialPort comPort = new SerialPort();
        public DisplayMsgDelegate DisplayMsgCallback;
        private FP_ReceiveDataInd fp_rxDataInd;
        private const string moduleName = "CommManager";
        private MsgBox msgBox = new MsgBox();
        private SharedObjects sharedObjs = new SharedObjects();
        private Thread taskThread;
        private ThreadControl threadCtrl = new ThreadControl();
        private ThreadData threadData = new ThreadData();

        private string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            foreach (byte num in comByte)
            {
                builder.Append(Convert.ToString(num, 0x10).PadLeft(2, '0').PadRight(3, ' '));
            }
            return builder.ToString().ToUpper();
        }

        public bool ClosePort()
        {
            if (this.comPort.IsOpen)
            {
                try
                {
                    if (this.sharedObjs.IsMonoRunning() && (this.taskThread != null))
                    {
                        this.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
                    }
                    this.comPort.Close();
                }
                catch (Exception exception)
                {
                    string msg = string.Format("Error closing {0:S}\n\n{1}\n", this.comPort.PortName, exception.Message);
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    return false;
                }
            }
            return true;
        }

        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            switch (this.CurrentTransmissionType)
            {
                case TransmissionType.Text:
                    return;

                case TransmissionType.Hex:
                    try
                    {
                        int bytesToRead = this.comPort.BytesToRead;
                        byte[] buffer = new byte[bytesToRead];
                        this.comPort.Read(buffer, 0, bytesToRead);
                        if (this.fp_rxDataInd != null)
                        {
                            this.fp_rxDataInd(buffer, (uint) buffer.Length);
                        }
                    }
                    catch (Exception exception2)
                    {
                        string msg = string.Format("Error Reading From {0:S} (Hex)\n" + exception2.Message + "\n", this.comPort.PortName);
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    }
                    break;
            }
        }

        [STAThread]
        private void DataRxPollThread(object threadData)
        {
            try
            {
                this.threadCtrl.Init();
                this.threadCtrl.runningThread = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "CommManager", "Starting Thread");
                while (!this.threadCtrl.exitThread)
                {
                    if (this.threadCtrl.pauseThread)
                    {
                        this.threadCtrl.idleThread = true;
                        SharedObjects.log.Write(Logging.MsgType.Debug, "CommManager", "Pausing Thread");
                        this.threadCtrl.eventPause.WaitOne();
                        this.threadCtrl.idleThread = false;
                        if (this.threadCtrl.exitThread)
                        {
                            goto Label_010E;
                        }
                    }
                    int bytesToRead = this.comPort.BytesToRead;
                    if (bytesToRead > 0)
                    {
                        byte[] buffer = new byte[bytesToRead];
                        this.comPort.Read(buffer, 0, bytesToRead);
                        if (this.fp_rxDataInd != null)
                        {
                            this.fp_rxDataInd(buffer, (uint) buffer.Length);
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = "Task Thread Problem.\n" + exception.Message + "\nCommManager\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        Label_010E:
            SharedObjects.log.Write(Logging.MsgType.Debug, "CommManager", "Exiting Thread");
            this.threadCtrl.Exit();
        }

        ~CommManager()
        {
            if (this.sharedObjs.IsMonoRunning() && (this.taskThread != null))
            {
                this.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
            }
        }

        private byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");
            byte[] buffer = new byte[msg.Length / 2];
            for (int i = 0; i < msg.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 0x10);
            }
            return buffer;
        }

        public void InitCommManager()
        {
            this._baudRate = string.Empty;
            this._parity = string.Empty;
            this._stopBits = string.Empty;
            this._dataBits = string.Empty;
            this._portName = "COM1";
            if (!this.sharedObjs.IsMonoRunning())
            {
                this.comPort.DataReceived += new SerialDataReceivedEventHandler(this.comPort_DataReceived);
            }
        }

        public void InitCommManager(string baud, string par, string sBits, string dBits, string name)
        {
            this._baudRate = baud;
            this._parity = par;
            this._stopBits = sBits;
            this._dataBits = dBits;
            this._portName = name;
            if (!this.sharedObjs.IsMonoRunning())
            {
                this.comPort.DataReceived += new SerialDataReceivedEventHandler(this.comPort_DataReceived);
            }
        }

        public bool OpenPort()
        {
            try
            {
                if (!this.comPort.IsOpen)
                {
                    this.comPort.BaudRate = int.Parse(this._baudRate);
                    this.comPort.DataBits = int.Parse(this._dataBits);
                    this.comPort.StopBits = (System.IO.Ports.StopBits) Enum.Parse(typeof(System.IO.Ports.StopBits), this._stopBits);
                    this.comPort.Parity = (System.IO.Ports.Parity) Enum.Parse(typeof(System.IO.Ports.Parity), this._parity);
                    this.comPort.PortName = this._portName;
                    this.comPort.Handshake = this._handShake;
                    try
                    {
                        this.comPort.Open();
                    }
                    catch (Exception exception)
                    {
                        string msg = string.Format("Com Port Open Error\n\n" + exception.Message + "\n", new object[0]);
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                        return false;
                    }
                    this.comPort.DiscardInBuffer();
                    this.comPort.DiscardOutBuffer();
                    this.comPort.WriteTimeout = 0x1388;
                    this.comPort.ReadTimeout = 0x1388;
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, "Port opened at " + DateTime.Now + "\n");
                    if (this.sharedObjs.IsMonoRunning())
                    {
                        this.taskThread = new Thread(new ParameterizedThreadStart(this.DataRxPollThread));
                        this.taskThread.Name = "CommManager";
                        this.taskThread.Start(this.threadData);
                        Thread.Sleep(0);
                        while (!this.taskThread.IsAlive)
                        {
                        }
                    }
                }
                return true;
            }
            catch (Exception exception2)
            {
                string str2 = string.Format("Com Port Open Process Error\n\n" + exception2.Message + "\n", new object[0]);
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                return false;
            }
        }

        public bool WriteData(string msg)
        {
            bool flag = true;
            if (!this.comPort.IsOpen && !this.OpenPort())
            {
                return false;
            }
            switch (this.CurrentTransmissionType)
            {
                case TransmissionType.Text:
                    try
                    {
                        this.comPort.Write(msg);
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Outgoing, msg + "\n");
                    }
                    catch (Exception exception)
                    {
                        string str = string.Format("Error Writing To {0:S} (Text)\n" + exception.Message + "\n", this.comPort.PortName);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                        flag = false;
                    }
                    return flag;

                case TransmissionType.Hex:
                    try
                    {
                        byte[] buffer = this.HexToByte(msg);
                        try
                        {
                            this.comPort.Write(buffer, 0, buffer.Length);
                        }
                        catch (Exception exception2)
                        {
                            string str2 = string.Format("Error Writing To {0:S} (Hex)\n" + exception2.Message + "\n", this.comPort.PortName);
                            this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str2);
                            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                            return false;
                        }
                    }
                    catch (Exception exception3)
                    {
                        string str3 = string.Format("Com Port Error\n Port Number = {0:S} (Hex)\n" + exception3.Message + "\n", this.comPort.PortName);
                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str3);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                        flag = false;
                    }
                    return flag;
            }
            try
            {
                this.comPort.Write(msg);
            }
            catch (Exception exception4)
            {
                string str4 = string.Format("Error Writing To {0:S} (Default)\n" + exception4.Message + "\n", this.comPort.PortName);
                this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, str4);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                flag = false;
            }
            return flag;
        }

        public string BaudRate
        {
            get
            {
                return this._baudRate;
            }
            set
            {
                this._baudRate = value;
            }
        }

        public TransmissionType CurrentTransmissionType
        {
            get
            {
                return this._transType;
            }
            set
            {
                this._transType = value;
            }
        }

        public string DataBits
        {
            get
            {
                return this._dataBits;
            }
            set
            {
                this._dataBits = value;
            }
        }

        public Handshake HandShake
        {
            get
            {
                return this._handShake;
            }
            set
            {
                this._handShake = value;
            }
        }

        public string Parity
        {
            get
            {
                return this._parity;
            }
            set
            {
                this._parity = value;
            }
        }

        public string PortName
        {
            get
            {
                return this._portName;
            }
            set
            {
                this._portName = value;
            }
        }

        public FP_ReceiveDataInd RxDataInd
        {
            get
            {
                return this.fp_rxDataInd;
            }
            set
            {
                this.fp_rxDataInd = value;
            }
        }

        public string StopBits
        {
            get
            {
                return this._stopBits;
            }
            set
            {
                this._stopBits = value;
            }
        }

        private class ThreadData
        {
        }

        public enum TransmissionType
        {
            Text,
            Hex
        }
    }
}
