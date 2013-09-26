﻿namespace BTool
{
    using System;
    using System.Threading;

    public class RspDataInThread
    {
        public AttErrorRsp attErrorRsp = new AttErrorRsp();
        public AttExecuteWriteRsp attExecuteWriteRsp;
        private AttFindByTypeValueRsp attFindByTypeValueRsp;
        private AttFindInfoRsp attFindInfoRsp;
        public AttHandleValueIndication attHandleValueIndication;
        public AttHandleValueNotification attHandleValueNotification;
        public AttPrepareWriteRsp attPrepareWriteRsp;
        public AttReadBlobRsp attReadBlobRsp;
        private AttReadByGrpTypeRsp attReadByGrpTypeRsp;
        private AttReadByTypeRsp attReadByTypeRsp;
        public AttReadRsp attReadRsp;
        public AttWriteRsp attWriteRsp;
        public QueueMgr dataQ = new QueueMgr("RspDataInThread");
        public ExtCmdStatus extCmdStatus = new ExtCmdStatus();
        private const string moduleName = "RspDataInThread";
        private MsgBox msgBox = new MsgBox();
        public RspDataInChangedDelegate RspDataInChangedCallback;
        private Thread taskThread;
        public ThreadControl threadCtrl = new ThreadControl();
        private ThreadData threadData = new ThreadData();

        public RspDataInThread(DeviceForm deviceForm)
        {
            this.attFindInfoRsp = new AttFindInfoRsp(deviceForm);
            this.attFindByTypeValueRsp = new AttFindByTypeValueRsp(deviceForm);
            this.attReadByTypeRsp = new AttReadByTypeRsp(deviceForm);
            this.attReadRsp = new AttReadRsp(deviceForm);
            this.attReadBlobRsp = new AttReadBlobRsp(deviceForm);
            this.attReadByGrpTypeRsp = new AttReadByGrpTypeRsp(deviceForm);
            this.attWriteRsp = new AttWriteRsp();
            this.attPrepareWriteRsp = new AttPrepareWriteRsp();
            this.attExecuteWriteRsp = new AttExecuteWriteRsp();
            this.attHandleValueNotification = new AttHandleValueNotification(deviceForm);
            this.attHandleValueIndication = new AttHandleValueIndication(deviceForm);
            this.taskThread = new Thread(new ParameterizedThreadStart(this.TaskThread));
            this.taskThread.Name = "RspDataInThread";
            this.taskThread.Start(this.threadData);
            Thread.Sleep(0);
            while (!this.taskThread.IsAlive)
            {
            }
        }

        private bool ProcessQData(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if ((hciReplies == null) || (hciReplies.hciLeExtEvent == null))
            {
                return false;
            }
            ushort eventCode = hciReplies.hciLeExtEvent.header.eventCode;
            if (eventCode <= 0x493)
            {
                if (eventCode <= 0x481)
                {
                    switch (eventCode)
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
                if ((eventCode == 0x48b) || (eventCode == 0x493))
                {
                }
                return flag;
            }
            if (eventCode <= 0x580)
            {
                switch (eventCode)
                {
                    case 0x501:
                        return this.attErrorRsp.GetATT_ErrorRsp(hciReplies, ref dataFound);

                    case 0x502:
                    case 0x503:
                    case 0x504:
                    case 0x506:
                    case 0x508:
                    case 0x50a:
                    case 0x50c:
                    case 0x50e:
                    case 0x50f:
                    case 0x510:
                    case 0x512:
                    case 0x514:
                    case 0x515:
                    case 0x516:
                    case 0x518:
                    case 0x51a:
                    case 0x51c:
                    case 0x51e:
                        return flag;

                    case 0x505:
                        return this.attFindInfoRsp.GetATT_FindInfoRsp(hciReplies, ref dataFound);

                    case 0x507:
                        return this.attFindByTypeValueRsp.GetATT_FindByTypeValueRsp(hciReplies, ref dataFound);

                    case 0x509:
                        return this.attReadByTypeRsp.GetATT_ReadByTypeRsp(hciReplies, ref dataFound);

                    case 0x50b:
                        return this.attReadRsp.GetATT_ReadRsp(hciReplies, ref dataFound);

                    case 0x50d:
                        return this.attReadBlobRsp.GetATT_ReadBlobRsp(hciReplies, ref dataFound);

                    case 0x511:
                        return this.attReadByGrpTypeRsp.GetATT_ReadByGrpTypeRsp(hciReplies, ref dataFound);

                    case 0x513:
                        return this.attWriteRsp.GetATT_WriteRsp(hciReplies, ref dataFound);

                    case 0x517:
                        return this.attPrepareWriteRsp.GetATT_PrepareWriteRsp(hciReplies, ref dataFound);

                    case 0x519:
                        return this.attExecuteWriteRsp.GetATT_ExecuteWriteRsp(hciReplies, ref dataFound);

                    case 0x51b:
                        return this.attHandleValueNotification.GetATT_HandleValueNotification(hciReplies, ref dataFound);

                    case 0x51d:
                        return this.attHandleValueIndication.GetATT_HandleValueIndication(hciReplies, ref dataFound);

                    case 0x580:
                        return flag;
                }
                return flag;
            }
            switch (eventCode)
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
                    return this.extCmdStatus.GetExtensionCommandStatus(hciReplies, ref dataFound);
            }
            return flag;
        }

        private bool QueueDataReady()
        {
            bool flag = true;
            HCIReplies hciReplies = new HCIReplies();
            object data = hciReplies;
            flag = this.dataQ.RemoveQHead(ref data);
            if (flag)
            {
                hciReplies = (HCIReplies) data;
                bool dataFound = false;
                flag = this.ProcessQData(hciReplies, ref dataFound);
                if ((flag && dataFound) && (this.RspDataInChangedCallback != null))
                {
                    this.RspDataInChangedCallback();
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
                SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Starting Thread");
                while (!flag && !this.threadCtrl.exitThread)
                {
                    if (this.threadCtrl.pauseThread)
                    {
                        this.threadCtrl.idleThread = true;
                        SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Pausing Thread");
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
                            SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Resuming Thread");
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
                string msg = "Task Thread Problem.\n" + exception.Message + "\nRspDataInThread\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        Label_017B:
            SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Exiting Thread");
            this.threadCtrl.Exit();
        }

        private class ThreadData
        {
        }
    }
}

