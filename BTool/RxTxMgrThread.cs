﻿namespace BTool
{
    using System;
    using System.Threading;

    public class RxTxMgrThread
    {
        public QueueMgr dataQ = new QueueMgr("RxTxMgrThread");
        public HandleRxTxMessageDelegate HandleRxTxMessageCallback;
        private const string moduleName = "RxTxMgrThread";
        private MsgBox msgBox = new MsgBox();
        private Thread taskThread;
        public ThreadControl threadCtrl = new ThreadControl();
        private ThreadData threadData = new ThreadData();

        public RxTxMgrThread()
        {
            this.taskThread = new Thread(new ParameterizedThreadStart(this.TaskThread));
            this.taskThread.Name = "RxTxMgrThread";
            this.taskThread.Start(this.threadData);
            Thread.Sleep(0);
            while (!this.taskThread.IsAlive)
            {
            }
        }

        private bool ProcessQData(RxTxMgrData rxTxMgrData, ref bool dataFound)
        {
            dataFound = false;
            this.HandleRxTxMessageCallback(rxTxMgrData);
            dataFound = true;
            return true;
        }

        private bool QueueDataReady()
        {
            bool flag = true;
            object obj2 = new RxTxMgrData();
            flag = this.dataQ.RemoveQHead(ref obj2);
            if (flag)
            {
                RxTxMgrData rxTxMgrData = (RxTxMgrData) obj2;
                bool dataFound = false;
                flag = this.ProcessQData(rxTxMgrData, ref dataFound);
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
                SharedObjects.log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Starting Thread");
                while (!flag && !this.threadCtrl.exitThread)
                {
                    if (this.threadCtrl.pauseThread)
                    {
                        this.threadCtrl.idleThread = true;
                        SharedObjects.log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Pausing Thread");
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
                            SharedObjects.log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Resuming Thread");
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
                string msg = "Task Thread Problem.\n" + exception.Message + "\nRxTxMgrThread\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
        Label_017B:
            SharedObjects.log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Exiting Thread");
            this.threadCtrl.Exit();
        }

        private class ThreadData
        {
        }
    }
}
