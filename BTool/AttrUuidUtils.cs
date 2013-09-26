﻿namespace BTool
{
    using System;
    using System.Drawing;

    public class AttrUuidUtils
    {
        private const string moduleName = "AttrUuidUtils";
        private MsgBox msgBox = new MsgBox();

        public string GetAttrKey(ushort connHandle, ushort handle)
        {
            return (connHandle.ToString("X4") + "_" + handle.ToString("X4"));
        }

        public Color GetBackgroundColor(string uuid)
        {
            Color defaultBackground = AttrData.defaultBackground;
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    defaultBackground = data.backColor;
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Background Color\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                defaultBackground = AttrData.defaultBackground;
            }
            return defaultBackground;
        }

        public Color GetForegroundColor(string uuid)
        {
            Color defaultForeground = AttrData.defaultForeground;
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    defaultForeground = data.foreColor;
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Foreground Color\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                defaultForeground = AttrData.defaultForeground;
            }
            return defaultForeground;
        }

        public byte GetIndentLevel(string uuid)
        {
            byte indentLevel = 0;
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    indentLevel = data.indentLevel;
                }
                else
                {
                    indentLevel = AttrData.unknownIndentLevel;
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Indent Level\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                indentLevel = 0;
            }
            return indentLevel;
        }

        public string GetUuidDesc(string uuid)
        {
            string uuidDesc = "";
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    uuidDesc = data.uuidDesc;
                }
                else
                {
                    uuidDesc = "Unknown";
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Description\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                uuidDesc = "";
            }
            return uuidDesc;
        }

        public string GetUuidValueDesc(string uuid)
        {
            string valueDesc = "";
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    valueDesc = data.valueDesc;
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Value Description\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                valueDesc = "";
            }
            return valueDesc;
        }

        public ValueDisplay GetValueDsp(string uuid)
        {
            ValueDisplay hex = ValueDisplay.Hex;
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    hex = data.valueDsp;
                }
                else
                {
                    hex = ValueDisplay.Hex;
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Value Display\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                hex = ValueDisplay.Hex;
            }
            return hex;
        }

        public ValueEdit GetValueEdit(string uuid)
        {
            ValueEdit editable = ValueEdit.Editable;
            try
            {
                AttrUuid.uuidDictAccess.WaitOne();
                if (AttrUuid.uuidDict.ContainsKey(uuid))
                {
                    UuidData data = AttrUuid.uuidDict[uuid];
                    editable = data.valueEdit;
                }
                else
                {
                    editable = ValueEdit.Editable;
                }
                AttrUuid.uuidDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string msg = "UUID Data Dictionary Access Error\nProblem With Value Edit\n" + exception.Message + "\nAttrUuidUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                editable = ValueEdit.Editable;
            }
            return editable;
        }
    }
}

