﻿namespace BTool
{
    using System;
    using System.Threading;

    public class TxDataOutThread
    {
        public QueueMgr dataQ = new QueueMgr("TxDataOutThread");
        private const int dataTimeout = 40;
        public DeviceRxStopWaitDelegate DeviceRxStopWaitCallback;
        public DeviceTxDataDelegate DeviceTxDataCallback;
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public DisplayMsgDelegate DisplayMsgCallback;
        private HCIStopWait hCIStopWait = new HCIStopWait();
        private const string moduleName = "TxDataOutThread";
        private MsgBox msgBox = new MsgBox();
        public ShowProgressDelegate ShowProgressCallback;
        private HCIStopWait.StopWaitEvent stopWaitEvent;
        private bool stopWaitMsg;
        private ManualResetEvent stopWaitSuccessEvent = new ManualResetEvent(false);
        private Thread taskThread;
        public ThreadControl threadCtrl = new ThreadControl();
        private ThreadData threadData = new ThreadData();

        public TxDataOutThread()
        {
            this.taskThread = new Thread(new ParameterizedThreadStart(this.TaskThread));
            this.taskThread.Name = "TxDataOutThread";
            this.taskThread.Start(this.threadData);
            Thread.Sleep(0);
            while (!this.taskThread.IsAlive)
            {
            }
        }

        private void ClearTxQueueQuestion()
        {
            int qLength = this.dataQ.GetQLength();
            if (qLength > 0)
            {
                string msg = "There Are " + qLength.ToString() + " Pending Transmit Messages\nDo You Want To Clear All Pending Transmit Messages?\n";
                if (this.DisplayMsgCallback != null)
                {
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg);
                }
                MsgBox.MsgResult result = this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.YesNo, msg);
                msg = "UserResponse = " + result.ToString() + "\n";
                if (this.DisplayMsgCallback != null)
                {
                    this.DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg);
                }
                if (result == MsgBox.MsgResult.Yes)
                {
                    this.dataQ.ClearQ();
                }
            }
        }

        private void DeviceTxStopWait(bool foundData)
        {
            if (foundData)
            {
                if ((this.stopWaitEvent != null) && (this.stopWaitEvent.callback != null))
                {
                    this.stopWaitEvent.callback(true, this.stopWaitEvent.cmdName);
                }
            }
            else
            {
                if (this.DeviceRxStopWaitCallback != null)
                {
                    this.DeviceRxStopWaitCallback(false, null);
                }
                if ((this.stopWaitEvent != null) && (this.stopWaitEvent.callback != null))
                {
                    this.stopWaitEvent.callback(false, this.stopWaitEvent.cmdName);
                }
                else
                {
                    this.ClearTxQueueQuestion();
                }
            }
            if (this.ShowProgressCallback != null)
            {
                this.ShowProgressCallback(false);
            }
            this.stopWaitEvent = null;
            this.stopWaitMsg = false;
            this.stopWaitSuccessEvent.Set();
        }

        public void InitThread(DeviceForm deviceForm)
        {
            deviceForm.threadMgr.rxDataIn.DeviceTxStopWaitCallback = new DeviceTxStopWaitDelegate(this.DeviceTxStopWait);
        }

        private bool ProcessQData(TxDataOut txDataOut, ref bool dataFound)
        {
            dataFound = false;
            ushort cmdOpcode = txDataOut.cmdOpcode;
            if (this.hCIStopWait.cmdChkDict.ContainsKey(cmdOpcode))
            {
                HCIStopWait.TxCheck check = this.hCIStopWait.cmdChkDict[cmdOpcode];
                if (check.stopWait && this.hCIStopWait.cmdDict.ContainsKey(cmdOpcode))
                {
                    HCIStopWait.StopWaitData data = this.hCIStopWait.cmdDict[cmdOpcode];
                    this.stopWaitEvent = new HCIStopWait.StopWaitEvent();
                    this.stopWaitEvent.cmdName = txDataOut.cmdName;
                    this.stopWaitEvent.txOpcode = (HCICmds.HCICmdOpcode) cmdOpcode;
                    this.stopWaitEvent.reqEvt = data.reqEvt;
                    this.stopWaitEvent.rspEvt1 = data.rspEvt1;
                    this.stopWaitEvent.rspEvt2 = data.rspEvt2;
                    this.stopWaitEvent.extCmdStat = new HCIStopWait.ExtCmdStat();
                    this.stopWaitEvent.extCmdStat.msgComp = data.extCmdStat.msgComp;
                    this.stopWaitEvent.cmdGrp = data.cmdGrp;
                    this.stopWaitEvent.cmdType = txDataOut.cmdType;
                    this.stopWaitEvent.msgComp = data.msgComp;
                    this.stopWaitEvent.txTime = string.Empty;
                    this.stopWaitEvent.tag = txDataOut.tag;
                    this.stopWaitEvent.callback = txDataOut.callback;
                    if (this.ShowProgressCallback != null)
                    {
                        this.ShowProgressCallback(true);
                    }
                    if (this.DeviceRxStopWaitCallback != null)
                    {
                        this.DeviceRxStopWaitCallback(true, this.stopWaitEvent);
                    }
                    this.stopWaitMsg = true;
                    this.stopWaitSuccessEvent.Reset();
                }
            }
            txDataOut.time = DateTime.Now.ToString("hh:mm:ss.fff");
            if (this.stopWaitEvent != null)
            {
                this.stopWaitEvent.txTime = txDataOut.time;
            }
            this.DeviceTxDataCallback(txDataOut);
            dataFound = true;
            return true;
        }

        private bool QueueDataReady()
        {
            bool flag = true;
            TxDataOut txDataOut = new TxDataOut();
            object data = txDataOut;
            flag = this.dataQ.RemoveQHead(ref data);
            if (flag)
            {
                txDataOut = (TxDataOut) data;
                bool dataFound = false;
                flag = this.ProcessQData(txDataOut, ref dataFound);
                if (!flag)
                {
                }
            }
            Thread.Sleep(10);
            return flag;
        }

        [STAThread]
        private void TaskThread(object threadData)
        {
            try
            {
                bool flag = false;
                this.threadCtrl.Init();
                this.threadCtrl.runningThread = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Starting Thread");
                while (!flag && !this.threadCtrl.exitThread)
                {
                    if (this.threadCtrl.pauseThread)
                    {
                        this.threadCtrl.idleThread = true;
                        SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Pausing Thread");
                        this.threadCtrl.eventPause.WaitOne();
                        this.threadCtrl.idleThread = false;
                        if (this.threadCtrl.exitThread)
                        {
                            goto Label_038E;
                        }
                    }
                    if (!this.stopWaitMsg)
                    {
                        switch (WaitHandle.WaitAny(new WaitHandle[] { this.threadCtrl.eventExit, this.threadCtrl.eventPause, this.dataQ.qDataReadyEvent }))
                        {
                            case 0:
                            {
                                flag = true;
                                if (!this.threadCtrl.exitThread)
                                {
                                }
                                continue;
                            }
                            case 1:
                            {
                                this.threadCtrl.eventPause.Reset();
                                SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Resuming Thread");
                                continue;
                            }
                            case 2:
                            {
                                this.dataQ.qDataReadyEvent.Reset();
                                this.QueueDataReady();
                                continue;
                            }
                        }
                        flag = true;
                    }
                    else
                    {
                        TimeSpan timeout = new TimeSpan(0, 0, 0, 40);
                        switch (WaitHandle.WaitAny(new WaitHandle[] { this.threadCtrl.eventExit, this.threadCtrl.eventPause, this.stopWaitSuccessEvent }, timeout))
                        {
                            case 0:
                            {
                                flag = true;
                                if (!this.threadCtrl.exitThread)
                                {
                                }
                                continue;
                            }
                            case 1:
                            {
                                this.threadCtrl.eventPause.Reset();
                                SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Resuming Thread");
                                continue;
                            }
                            case 2:
                            {
                                this.stopWaitSuccessEvent.Reset();
                                this.stopWaitEvent = null;
                                this.stopWaitMsg = false;
                                continue;
                            }
                            case 0x102:
                            {
                                if (this.DeviceRxStopWaitCallback != null)
                                {
                                    this.DeviceRxStopWaitCallback(false, null);
                                }
                                if (this.stopWaitEvent != null)
                                {
                                    ushort txOpcode = (ushort) this.stopWaitEvent.txOpcode;
                                    string msg = "Message Response Timeout\nName = " + this.devUtils.GetOpCodeName((ushort) this.stopWaitEvent.txOpcode) + "\nOpcode = 0x" + txOpcode.ToString("X4") + "\nTx Time = " + this.stopWaitEvent.txTime + "\n";
                                    if (this.DisplayMsgCallback != null)
                                    {
                                        this.DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
                                    }
                                    if (this.stopWaitEvent.callback == null)
                                    {
                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                                        this.ClearTxQueueQuestion();
                                    }
                                    if (this.ShowProgressCallback != null)
                                    {
                                        this.ShowProgressCallback(false);
                                    }
                                    if (this.stopWaitEvent.callback != null)
                                    {
                                        this.stopWaitEvent.callback(false, this.stopWaitEvent.cmdName);
                                    }
                                }
                                this.stopWaitEvent = null;
                                this.stopWaitMsg = false;
                                continue;
                            }
                        }
                        flag = true;
                    }
                }
            }
            catch (Exception exception)
            {
                string str2 = "Task Thread Problem.\n" + exception.Message + "\nTxDataOutThread\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
            }
        Label_038E:
            SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Exiting Thread");
            this.threadCtrl.Exit();
        }

        private class ThreadData
        {
        }
    }
}

