﻿using System;

public class XmlDataReaderUtils
{
    public const string moduleName = "XmlDataReaderUtils";
    private MsgBox msgBox = new MsgBox();

    public bool FileVersionError(string xmlFormatVersion, string fileVersion, string xmlFileName, string moduleName)
    {
        string msg = "XML File Version Error\nWas Expecting Version " + xmlFormatVersion + " But Read " + fileVersion + "\n" + xmlFileName + "\n" + moduleName + "\n";
        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        return false;
    }

    public bool InvalidTagValueFound(string tagName, string xmlFileName, string invalidValue, string defaultValue, string eMsg, string moduleName)
    {
        string msg = "Invalid " + tagName + " Value In XML File\n(Invalid Value = " + invalidValue + ")\n(Value Changed To Default = " + defaultValue + ")\n";
        if (eMsg != null)
        {
            msg = msg + eMsg + "\n";
        }
        msg = msg + "XML Filename = " + xmlFileName + "\n";
        if (moduleName != null)
        {
            msg = msg + moduleName + "\n";
        }
        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
        return true;
    }

    public bool NoTagValueFound(string tagName, string xmlFileName, string moduleName)
    {
        string msg = "XML File Read Error\nNo " + tagName + " Found\n" + xmlFileName + "\n";
        if (moduleName != null)
        {
            msg = msg + moduleName + "\n";
        }
        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        return false;
    }
}

