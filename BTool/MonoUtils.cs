﻿using System;
using System.Drawing;
using System.Windows.Forms;

internal class MonoUtils
{
    private const string moduleName = "MonoUtils";
    private SharedObjects sharedObjs = new SharedObjects();

    public bool SetMaximumSize(Form form)
    {
        if (this.sharedObjs.IsMonoRunning())
        {
            form.MaximumSize = new Size(form.Size.Width, form.Size.Height);
        }
        return true;
    }
}
