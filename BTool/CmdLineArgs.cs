﻿using System;
using System.Runtime.InteropServices;

public class CmdLineArgs
{
    private string[] cmdLineArgs;
    private const string moduleName = "CmdLineArgs";
    private MsgBox msgBox = new MsgBox();

    public bool FindCmdLineArg(string cmdArg)
    {
        bool flag = false;
        try
        {
            if ((this.cmdLineArgs != null) && (this.cmdLineArgs.Length > 0))
            {
                foreach (string str in this.cmdLineArgs)
                {
                    if (str.ToUpper() == cmdArg.ToUpper())
                    {
                        return true;
                    }
                }
                return flag;
            }
            flag = false;
        }
        catch (Exception exception)
        {
            string msg = "FindCmdLineArg Problem\n" + exception.Message + "\nCmdLineArgs\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
        return flag;
    }

    public bool GetCmdLineArgs(out string[] cmdArgs)
    {
        bool flag = true;
        cmdArgs = null;
        try
        {
            if ((this.cmdLineArgs != null) && (this.cmdLineArgs.Length > 0))
            {
                cmdArgs = new string[this.cmdLineArgs.Length];
                cmdArgs = this.cmdLineArgs;
                return flag;
            }
            flag = false;
        }
        catch (Exception exception)
        {
            string msg = "GetCmdLineArgs Problem\n" + exception.Message + "\nCmdLineArgs\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
        return flag;
    }

    public bool SetCmdLineArgs(string[] cmdArgs)
    {
        bool flag = true;
        try
        {
            if ((cmdArgs != null) && (cmdArgs.Length > 0))
            {
                this.cmdLineArgs = new string[cmdArgs.Length];
                this.cmdLineArgs = cmdArgs;
                return flag;
            }
            return false;
        }
        catch (Exception exception)
        {
            string msg = "SetCmdLineArgs Problem\n" + exception.Message + "\nCmdLineArgs\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return false;
        }
    }
}

