﻿using System;
using System.Windows.Forms;

public class SharedObjects
{
    public static Logging log = new Logging();
    public static Form mainWin = null;
    private const string moduleName = "SharedObjects";
    public static bool programExit = false;

    public bool IsMonoRunning()
    {
        bool flag = false;
        if (System.Type.GetType("Mono.Runtime") != null)
        {
            flag = true;
        }
        return flag;
    }

    public bool IsVistaOrHigherOs()
    {
        bool flag = false;
        if (Environment.OSVersion.Version.Major > 5)
        {
            flag = true;
        }
        return flag;
    }
}
