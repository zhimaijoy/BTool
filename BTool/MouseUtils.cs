﻿using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class MouseUtils
{
    private const string moduleName = "MouseUtils";
    private bool mouseClickInit;
    private int mouseClicks;
    private Timer mouseClickTimer = new Timer();
    public MouseDoubleClickDelegate MouseDoubleClickCallback;
    public MouseSingleClickDelegate MouseSingleClickCallback;

    public void MouseClick_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            if (!this.mouseClickInit)
            {
                this.MouseClickInit();
                this.mouseClickInit = true;
            }
            this.mouseClickTimer.Stop();
            this.mouseClicks++;
            this.mouseClickTimer.Start();
        }
    }

    private void MouseClickInit()
    {
        this.mouseClicks = 0;
        this.mouseClickInit = false;
        this.mouseClickTimer.Interval = SystemInformation.DoubleClickTime;
        this.mouseClickTimer.Tick += new EventHandler(this.MouseClickTimer_Tick);
    }

    private void MouseClickTimer_Tick(object sender, EventArgs e)
    {
        this.mouseClickTimer.Stop();
        if (this.mouseClicks > 1)
        {
            if (this.MouseDoubleClickCallback != null)
            {
                this.MouseDoubleClickCallback();
            }
        }
        else if (this.MouseSingleClickCallback != null)
        {
            this.MouseSingleClickCallback();
        }
        this.mouseClicks = 0;
    }

    public delegate void MouseDoubleClickDelegate();

    public delegate void MouseSingleClickDelegate();
}

