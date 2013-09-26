﻿namespace BTool
{
    using System;
    using System.Threading;

    public class ThreadMgr
    {
        private const string moduleName = "ThreadMgr";
        public RspDataInThread rspDataIn;
        public RxDataInThread rxDataIn;
        public RxTxMgrThread rxTxMgr;
        public TxDataOutThread txDataOut;

        public ThreadMgr(DeviceForm deviceForm)
        {
            this.rspDataIn = new RspDataInThread(deviceForm);
            this.txDataOut = new TxDataOutThread();
            this.rxDataIn = new RxDataInThread(deviceForm);
            this.rxTxMgr = new RxTxMgrThread();
        }

        public bool CheckForIdle()
        {
            bool flag = false;
            if (((this.rspDataIn.dataQ.GetQLength() <= 0) && (this.txDataOut.dataQ.GetQLength() <= 0)) && ((this.rxDataIn.dataQ.GetQLength() <= 0) && (this.rxTxMgr.dataQ.GetQLength() <= 0)))
            {
                flag = true;
            }
            return flag;
        }

        public bool ClearQueues()
        {
            this.rspDataIn.dataQ.ClearQ();
            this.txDataOut.dataQ.ClearQ();
            this.rxDataIn.dataQ.ClearQ();
            this.rxTxMgr.dataQ.ClearQ();
            return true;
        }

        public bool ExitThreads()
        {
            if (this.rspDataIn != null)
            {
                this.rspDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
            }
            if (this.txDataOut != null)
            {
                this.txDataOut.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
            }
            if (this.rxDataIn != null)
            {
                this.rxDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
            }
            if (this.rxTxMgr != null)
            {
                this.rxTxMgr.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
            }
            return true;
        }

        ~ThreadMgr()
        {
            this.ExitThreads();
        }

        public void Init(DeviceForm deviceForm)
        {
            this.txDataOut.InitThread(deviceForm);
            this.rxDataIn.InitThread(deviceForm);
        }

        public bool PauseThreads()
        {
            this.rspDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Pause);
            this.txDataOut.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Pause);
            this.rxDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Pause);
            this.rxTxMgr.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Pause);
            return true;
        }

        public bool ResumeThreads()
        {
            this.rspDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Resume);
            this.txDataOut.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Resume);
            this.rxDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Resume);
            this.rxTxMgr.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Resume);
            return true;
        }

        public bool StopThreads()
        {
            this.rspDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Stop);
            this.txDataOut.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Stop);
            this.rxDataIn.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Stop);
            this.rxTxMgr.threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Stop);
            return true;
        }

        public bool WaitForPause()
        {
            do
            {
                Thread.Sleep(100);
            }
            while ((!this.rspDataIn.threadCtrl.CheckForThreadIdle(ThreadControl.CheckIdleModes.Paused) || !this.txDataOut.threadCtrl.CheckForThreadIdle(ThreadControl.CheckIdleModes.Paused)) || (!this.rxDataIn.threadCtrl.CheckForThreadIdle(ThreadControl.CheckIdleModes.Paused) || !this.rxTxMgr.threadCtrl.CheckForThreadIdle(ThreadControl.CheckIdleModes.Paused)));
            return true;
        }
    }
}

