﻿namespace BTool
{
    using System;
    using System.Threading;

    public class RxDataInThread
    {
        public QueueMgr dataQ = new QueueMgr("RxDataInThread");
        private DataUtils dataUtils = new DataUtils();
        public DeviceRxDataDelegate DeviceRxDataCallback;
        public DeviceTxStopWaitDelegate DeviceTxStopWaitCallback;
        private const string moduleName = "RxDataInThread";
        private MsgBox msgBox = new MsgBox();
        private RxDataInRspData rxDataInRspData;
        private HCIStopWait.StopWaitEvent stopWaitEvent;
        private bool stopWaitMsg;
        private Mutex stopWaitMutex = new Mutex();
        private Thread taskThread;
        public ThreadControl threadCtrl = new ThreadControl();
        private ThreadData threadData = new ThreadData();

        public RxDataInThread(DeviceForm deviceForm)
        {
            this.rxDataInRspData = new RxDataInRspData(deviceForm);
            this.taskThread = new Thread(new ParameterizedThreadStart(this.TaskThread));
            this.taskThread.Name = "RxDataInThread";
            this.taskThread.Start(this.threadData);
            Thread.Sleep(0);
            while (!this.taskThread.IsAlive)
            {
            }
        }

        private bool CheckMsgComplete(HCIStopWait.MsgComp msgComp, byte eventStatus)
        {
            bool flag = false;
            switch (msgComp)
            {
                case HCIStopWait.MsgComp.NotUsed:
                    return flag;

                case HCIStopWait.MsgComp.AnyStatVal:
                    return true;

                case HCIStopWait.MsgComp.AnyStatNotSucc:
                    if (eventStatus != 0)
                    {
                        flag = true;
                    }
                    return flag;
            }
            return flag;
        }

        private void DeviceRxStopWait(bool startStop, HCIStopWait.StopWaitEvent newStopWaitEvent)
        {
            this.stopWaitMutex.WaitOne();
            this.stopWaitMsg = startStop;
            if (newStopWaitEvent != null)
            {
                this.stopWaitEvent = new HCIStopWait.StopWaitEvent();
                this.stopWaitEvent = newStopWaitEvent;
            }
            else
            {
                this.stopWaitEvent = null;
            }
            this.stopWaitMutex.ReleaseMutex();
        }

        private bool FindStopWait(RxDataIn rxDataIn)
        {
            bool flag = false;
            try
            {
                int num;
                bool flag2;
                int num2;
                byte num3;
                HCIReplies.ATT_MsgHeader header;
                ushort num5;
                if (rxDataIn.type == 4)
                {
                    num = 0;
                    flag2 = false;
                    num2 = 0;
                    ushort cmdOpcode = rxDataIn.cmdOpcode;
                    if (((cmdOpcode == 14) || (cmdOpcode == 0x13)) || (cmdOpcode != 0xff))
                    {
                        return flag;
                    }
                    num3 = this.dataUtils.Unload8Bits(rxDataIn.data, ref num, ref flag2);
                    if (flag2)
                    {
                        return flag;
                    }
                    ushort eventOpcode = rxDataIn.eventOpcode;
                    if (eventOpcode <= 0x493)
                    {
                        if (eventOpcode <= 0x481)
                        {
                            switch (eventOpcode)
                            {
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
                                    return flag;

                                case 0x481:
                                    return flag;
                            }
                            return flag;
                        }
                        if ((eventOpcode == 0x48b) || (eventOpcode == 0x493))
                        {
                        }
                        return flag;
                    }
                    if (eventOpcode <= 0x580)
                    {
                        switch (eventOpcode)
                        {
                            case 0x501:
                                goto Label_0203;

                            case 0x502:
                            case 0x503:
                            case 0x504:
                            case 0x505:
                            case 0x506:
                            case 0x507:
                            case 0x508:
                            case 0x509:
                            case 0x50a:
                            case 0x50b:
                            case 0x50c:
                            case 0x50d:
                            case 0x50e:
                            case 0x50f:
                            case 0x510:
                            case 0x511:
                            case 0x512:
                            case 0x513:
                            case 0x516:
                            case 0x517:
                            case 0x518:
                            case 0x519:
                            case 0x51b:
                            case 0x51d:
                            case 0x51e:
                                goto Label_0298;

                            case 0x514:
                            case 0x515:
                            case 0x51a:
                            case 0x51c:
                                return flag;

                            case 0x580:
                                return flag;
                        }
                        return flag;
                    }
                    switch (eventOpcode)
                    {
                        case 0x600:
                        case 0x601:
                        case 0x602:
                        case 0x603:
                        case 0x604:
                        case 0x605:
                        case 0x606:
                        case 0x607:
                        case 0x608:
                        case 0x609:
                        case 0x60a:
                        case 0x60b:
                        case 0x60c:
                        case 0x60d:
                        case 0x60e:
                        case 0x60f:
                            return flag;

                        case 0x67f:
                            goto Label_02D6;
                    }
                }
                return flag;
            Label_0203:
                header = new HCIReplies.ATT_MsgHeader();
                if (((num2 = this.rxDataInRspData.UnloadAttMsgHeader(ref rxDataIn.data, ref num, ref flag2, ref header)) == 0) && (num2 == 0))
                {
                    return flag;
                }
                byte num4 = this.dataUtils.Unload8Bits(rxDataIn.data, ref num, ref flag2);
                if (flag2 || ((((num4 & 0x80) != ((byte) (this.stopWaitEvent.txOpcode & ((HCICmds.HCICmdOpcode) 0xff80)))) && ((num4 & 0x80) != ((byte) (this.stopWaitEvent.reqEvt & ((HCICmds.HCIEvtOpCode) 0xff80))))) && (this.stopWaitEvent.reqEvt != HCICmds.HCIEvtOpCode.InvalidEventCode)))
                {
                    return flag;
                }
                return true;
            Label_0298:
                if ((rxDataIn.eventOpcode != ((ushort) this.stopWaitEvent.rspEvt1)) && (rxDataIn.eventOpcode != ((ushort) this.stopWaitEvent.rspEvt2)))
                {
                    return flag;
                }
                return this.CheckMsgComplete(this.stopWaitEvent.msgComp, num3);
            Label_02D6:
                num5 = this.dataUtils.Unload16Bits(rxDataIn.data, ref num, ref flag2, false);
                if (!flag2 && (num5 == ((ushort) this.stopWaitEvent.txOpcode)))
                {
                    flag = this.CheckMsgComplete(this.stopWaitEvent.extCmdStat.msgComp, num3);
                }
            }
            catch (Exception exception)
            {
                string msg = "Find Stop Wait Problem.\n" + exception.Message + "\nRxDataInThread\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            return flag;
        }

        public void InitThread(DeviceForm deviceForm)
        {
            deviceForm.threadMgr.txDataOut.DeviceRxStopWaitCallback = new DeviceRxStopWaitDelegate(this.DeviceRxStopWait);
        }

        private bool ProcessQData(RxDataIn rxDataIn, ref bool dataFound)
        {
            dataFound = false;
            try
            {
                this.stopWaitMutex.WaitOne();
                this.rxDataInRspData.GetRspData(rxDataIn, this.stopWaitEvent);
                if ((this.stopWaitMsg && this.FindStopWait(rxDataIn)) && (this.DeviceTxStopWaitCallback != null))
                {
                    this.stopWaitMsg = false;
                    this.stopWaitEvent = null;
                    this.DeviceTxStopWaitCallback(true);
                }
                rxDataIn.time = DateTime.Now.ToString("hh:mm:ss.fff");
                this.DeviceRxDataCallback(rxDataIn);
                dataFound = true;
                this.stopWaitMutex.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "Process Queue Data Problem.\n" + exception.Message + "\nRxDataInThread\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            return true;
        }

        private bool QueueDataReady()
        {
            bool flag = true;
            RxDataIn rxDataIn = new RxDataIn();
            object data = rxDataIn;
            flag = this.dataQ.RemoveQHead(ref data);
            if (flag)
            {
                rxDataIn = (RxDataIn) data;
                bool dataFound = false;
                flag = this.ProcessQData(rxDataIn, ref dataFound);
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
                SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Starting Thread");
                while (!flag && !this.threadCtrl.exitThread)
                {
                    if (this.threadCtrl.pauseThread)
                    {
                        this.threadCtrl.idleThread = true;
                        SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Pausing Thread");
                        this.threadCtrl.eventPause.WaitOne();
                        this.threadCtrl.idleThread = false;
                        if (this.threadCtrl.exitThread)
                        {
                            goto Label_017B;
                        }
                    }
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
                            SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Resuming Thread");
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
            }
            catch (Exception exception)
            {
                string msg = "Task Thread Problem.\n" + exception.Message + "\nRxDataInThread\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        Label_017B:
            SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Exiting Thread");
            this.threadCtrl.Exit();
        }

        private class ThreadData
        {
        }
    }
}
