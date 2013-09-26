﻿using System;
using System.Collections;
using System.Threading;

public class QueueMgr
{
    private string callingModuleName;
    private Queue dataQ;
    private const string moduleName = "QueueMgr";
    private MsgBox msgBox;
    private Mutex qDataMutex;
    public ManualResetEvent qDataReadyEvent;
    private Queue syncDataQ;

    public QueueMgr()
    {
        this.callingModuleName = "";
        this.msgBox = new MsgBox();
        this.dataQ = new Queue();
        this.qDataMutex = new Mutex();
        this.qDataReadyEvent = new ManualResetEvent(false);
        this.InitQueueMgr(string.Empty);
    }

    public QueueMgr(string tmpModuleName)
    {
        this.callingModuleName = "";
        this.msgBox = new MsgBox();
        this.dataQ = new Queue();
        this.qDataMutex = new Mutex();
        this.qDataReadyEvent = new ManualResetEvent(false);
        this.InitQueueMgr(tmpModuleName);
    }

    public bool AddQTail(object data)
    {
        this.qDataMutex.WaitOne();
        try
        {
            this.syncDataQ.Enqueue(data);
            this.qDataReadyEvent.Set();
        }
        catch
        {
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "AddQTail\nError Adding Element To Queue\n");
        }
        this.qDataMutex.ReleaseMutex();
        return true;
    }

    public bool ClearQ()
    {
        this.qDataMutex.WaitOne();
        bool flag = true;
        try
        {
            if (this.syncDataQ.Count > 0)
            {
                this.syncDataQ.Clear();
                this.qDataReadyEvent.Reset();
            }
            else
            {
                flag = false;
            }
        }
        catch
        {
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "ClearQ\nError Clearing Queue\n");
        }
        this.qDataMutex.ReleaseMutex();
        return flag;
    }

    public int GetQLength()
    {
        this.qDataMutex.WaitOne();
        int count = 0;
        try
        {
            count = this.syncDataQ.Count;
        }
        catch
        {
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "GetQLength\nError Getting Number Of Items In The Queue\n");
        }
        this.qDataMutex.ReleaseMutex();
        return count;
    }

    private void InitQueueMgr(string tmpModuleName)
    {
        this.callingModuleName = tmpModuleName;
        this.syncDataQ = Queue.Synchronized(this.dataQ);
        this.qDataReadyEvent.Reset();
    }

    public bool RemoveQHead(ref object data)
    {
        this.qDataMutex.WaitOne();
        bool flag = true;
        try
        {
            if (this.syncDataQ.Count > 0)
            {
                data = this.syncDataQ.Dequeue();
                if (this.syncDataQ.Count > 0)
                {
                    this.qDataReadyEvent.Set();
                }
            }
            else
            {
                flag = false;
            }
        }
        catch
        {
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "RemoveQHead\nError Removing Element From Queue\n");
        }
        this.qDataMutex.ReleaseMutex();
        return flag;
    }
}
