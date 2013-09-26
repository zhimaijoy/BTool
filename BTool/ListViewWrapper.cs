﻿using System;
using System.Threading;
using System.Windows.Forms;

public class ListViewWrapper : ListView
{
    private const string moduleName = "ListViewWrapper";

    public event ScrollEventHandler scrollEventHandler;

    protected virtual void OnScroll(ScrollEventArgs e)
    {
        if (!base.ContainsFocus)
        {
            base.Focus();
        }
        if (this.scrollEventHandler != null)
        {
            this.scrollEventHandler(this, e);
        }
    }

    protected override void WndProc(ref Message message)
    {
        base.WndProc(ref message);
        if ((message.Msg == 0x115) || (message.Msg == 0x114))
        {
            this.OnScroll(new ScrollEventArgs(((ScrollEventType) message.WParam.ToInt32()) & ((ScrollEventType) 0xffff), 0));
        }
    }
}

