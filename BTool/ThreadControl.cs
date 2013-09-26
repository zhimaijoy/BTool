﻿using System;
using System.Threading;

public class ThreadControl
{
    public ManualResetEvent eventExit = new ManualResetEvent(false);
    public ManualResetEvent eventPause = new ManualResetEvent(false);
    public bool exitThread;
    public bool idleThread;
    private const string moduleName = "ThreadControl";
    public bool pauseThread;
    public bool runningThread;
    public bool stopInProgress;

    public bool CheckForThreadIdle(CheckIdleModes idleMode)
    {
        if (idleMode == CheckIdleModes.PausedWithoutData)
        {
            if (!this.idleThread)
            {
                return false;
            }
        }
        else if (!this.pauseThread || !this.idleThread)
        {
            return false;
        }
        return true;
    }

    public bool ControlThread(ThreadCtrl threadCtrlMode)
    {
        bool flag = false;
        switch (threadCtrlMode)
        {
            case ThreadCtrl.Pause:
                this.pauseThread = true;
                this.eventPause.Set();
                return flag;

            case ThreadCtrl.Resume:
                this.pauseThread = false;
                this.eventPause.Set();
                return flag;

            case ThreadCtrl.Stop:
                this.stopInProgress = true;
                this.pauseThread = true;
                this.stopInProgress = false;
                return flag;

            case ThreadCtrl.Exit:
                this.exitThread = true;
                this.eventPause.Set();
                this.eventExit.Set();
                while (this.runningThread)
                {
                    Thread.Sleep(100);
                }
                return flag;
        }
        return flag;
    }

    public void Exit()
    {
        this.pauseThread = true;
        this.idleThread = true;
        this.runningThread = false;
        this.eventExit.Set();
    }

    public void Init()
    {
        this.exitThread = false;
        this.pauseThread = false;
        this.idleThread = true;
        this.runningThread = false;
        this.stopInProgress = false;
        this.eventPause.Reset();
        this.eventExit.Reset();
    }

    public enum CheckIdleModes
    {
        Paused,
        PausedWithoutData
    }

    public enum ThreadCtrl
    {
        Pause,
        Resume,
        Stop,
        Exit
    }
}

