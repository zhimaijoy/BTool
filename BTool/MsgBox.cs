﻿using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

public class MsgBox
{
    public const string ErrorStr = "BTool - Error";
    public const string FatalStr = "BTool - Fatal Error";
    public const string InfoStr = "BTool - Info";
    private const string moduleName = "MsgBox";
    public const string WarningStr = "BTool - Warning";

    private string GetMsgTypesStr(MsgTypes msgType)
    {
        switch (msgType)
        {
            case MsgTypes.Error:
                return "Error";

            case MsgTypes.Warning:
                return "Warning ";

            case MsgTypes.Info:
                return "Info";
        }
        return "Unknown";
    }

    public void UserMsgBox(MsgTypes msgType, string msg)
    {
        Form mainWin = SharedObjects.mainWin;
        MsgButtons ok = MsgButtons.Ok;
        this.UserMsgBox(mainWin, msgType, ok, msg);
    }

    public MsgResult UserMsgBox(MsgTypes msgType, MsgButtons msgButtons, string msg)
    {
        Form mainWin = SharedObjects.mainWin;
        return this.UserMsgBox(mainWin, msgType, msgButtons, msg);
    }

    public void UserMsgBox(Form owner, MsgTypes msgType, string msg)
    {
        MsgButtons ok = MsgButtons.Ok;
        this.UserMsgBox(owner, msgType, ok, msg);
    }

    public MsgResult UserMsgBox(Form owner, MsgTypes msgType, MsgButtons msgButtons, string msg)
    {
        MsgResult oK = MsgResult.OK;
        try
        {
            if (SharedObjects.mainWin.InvokeRequired)
            {
                try
                {
                    oK = (MsgResult) SharedObjects.mainWin.Invoke(new UserMsgBoxDelegate(this.UserMsgBox), new object[] { owner, msgType, msgButtons, msg });
                }
                catch
                {
                }
                return oK;
            }
            string text = "UserMsgBox Called With No Message!\n";
            if ((msg != null) && (msg.Length > 0))
            {
                text = msg;
            }
            MessageBoxButtons oKCancel = MessageBoxButtons.OK;
            switch (msgButtons)
            {
                case MsgButtons.Ok:
                    oKCancel = MessageBoxButtons.OK;
                    break;

                case MsgButtons.OkCancel:
                    oKCancel = MessageBoxButtons.OKCancel;
                    break;

                case MsgButtons.AbortRetryIgnore:
                    oKCancel = MessageBoxButtons.AbortRetryIgnore;
                    break;

                case MsgButtons.YesNoCancel:
                    oKCancel = MessageBoxButtons.YesNoCancel;
                    break;

                case MsgButtons.YesNo:
                    oKCancel = MessageBoxButtons.YesNo;
                    break;

                case MsgButtons.RetryCancel:
                    oKCancel = MessageBoxButtons.RetryCancel;
                    break;

                default:
                    oKCancel = MessageBoxButtons.OK;
                    break;
            }
            string caption = string.Empty;
            MessageBoxIcon hand = MessageBoxIcon.Hand;
            switch (msgType)
            {
                case MsgTypes.Fatal:
                    caption = "BTool - Fatal Error";
                    hand = MessageBoxIcon.Hand;
                    break;

                case MsgTypes.Error:
                    caption = "BTool - Error";
                    hand = MessageBoxIcon.Hand;
                    break;

                case MsgTypes.Warning:
                    caption = "BTool - Warning";
                    hand = MessageBoxIcon.Exclamation;
                    break;

                case MsgTypes.Info:
                    caption = "BTool - Info";
                    hand = MessageBoxIcon.Asterisk;
                    break;

                default:
                    caption = "BTool - Error";
                    hand = MessageBoxIcon.Hand;
                    break;
            }
            if (owner == null)
            {
                oK = (MsgResult) MessageBox.Show(text, caption, oKCancel, hand);
            }
            else
            {
                oK = (MsgResult) MessageBox.Show(owner, text, caption, oKCancel, hand);
            }
            if (msgType == MsgTypes.Fatal)
            {
                Environment.Exit(1);
            }
        }
        catch
        {
        }
        return oK;
    }

    public enum MsgButtons
    {
        Ok,
        OkCancel,
        AbortRetryIgnore,
        YesNoCancel,
        YesNo,
        RetryCancel
    }

    public enum MsgResult
    {
        None,
        OK,
        Cancel,
        Abort,
        Retry,
        Ignore,
        Yes,
        No
    }

    public enum MsgTypes
    {
        Fatal,
        Error,
        Warning,
        Info
    }

    private delegate MsgBox.MsgResult UserMsgBoxDelegate(Form owner, MsgBox.MsgTypes msgType, MsgBox.MsgButtons msgButtons, string msg);
}
