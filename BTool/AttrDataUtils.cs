﻿namespace BTool
{
    using System;
    using System.Collections.Generic;

    public class AttrDataUtils
    {
        private DeviceForm devForm;
        private const string moduleName = "AttrDataUtils";
        private MsgBox msgBox = new MsgBox();

        public AttrDataUtils(DeviceForm deviceForm)
        {
            this.devForm = deviceForm;
        }

        public bool GetDataAttr(ref DataAttr dataAttr, ref bool dataChanged, string key, string funcName)
        {
            bool flag = true;
            dataChanged = false;
            this.devForm.attrData.attrDictAccess.WaitOne();
            if (this.devForm.attrData.attrDict.ContainsKey(key))
            {
                try
                {
                    dataAttr = this.devForm.attrData.attrDict[key];
                    dataChanged = true;
                }
                catch (Exception exception)
                {
                    string msg = "Attribute Dictionary Access Error\nGetDataAttr()\n" + funcName + "\n" + exception.Message + "\nAttrDataUtils\n";
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                    flag = false;
                }
            }
            this.devForm.attrData.attrDictAccess.ReleaseMutex();
            return flag;
        }

        public bool RemoveAttrDictItem(string key)
        {
            bool flag = true;
            this.devForm.attrData.attrDictAccess.WaitOne();
            if (this.devForm.attrData.attrDict.ContainsKey(key))
            {
                flag = this.devForm.attrData.attrDict.Remove(key);
            }
            this.devForm.attrData.attrDictAccess.ReleaseMutex();
            return flag;
        }

        public bool UpdateAttrDict(Dictionary<string, DataAttr> tmpAttrDict)
        {
            this.devForm.attrData.attrDictAccess.WaitOne();
            foreach (KeyValuePair<string, DataAttr> pair in tmpAttrDict)
            {
                this.devForm.attrData.attrDict[pair.Value.key] = tmpAttrDict[pair.Value.key];
            }
            this.devForm.attrData.attrDictAccess.ReleaseMutex();
            return true;
        }

        public bool UpdateAttrDictItem(DataAttr dataAttr)
        {
            bool flag = true;
            this.devForm.attrData.attrDictAccess.WaitOne();
            if (this.devForm.attrData.attrDict.ContainsKey(dataAttr.key))
            {
                this.devForm.attrData.attrDict[dataAttr.key] = dataAttr;
            }
            else
            {
                string msg = string.Format("Attribute Dictionary Update Error\nItem Does Not Exist In Dictionary\nAttrDataUtils\n", new object[0]);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = false;
            }
            this.devForm.attrData.attrDictAccess.ReleaseMutex();
            return flag;
        }

        public bool UpdateTmpAttrDict(ref Dictionary<string, DataAttr> tmpAttrDict, DataAttr dataAttr, bool dataChanged, string key)
        {
            bool flag = true;
            try
            {
                if (dataChanged)
                {
                    dataAttr.dataUpdate = true;
                    tmpAttrDict.Add(key, dataAttr);
                    return flag;
                }
                if (this.devForm.attrData.attrDict.Count >= 0x5dc)
                {
                    string msg = string.Format("Attribute Dictionary At Maximum {0} Elements\nData Lost\nAttrDataUtils\n", 0x5dc);
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
                    return false;
                }
                this.devForm.attrData.attrDictAccess.WaitOne();
                dataAttr.dataUpdate = true;
                this.devForm.attrData.attrDict.Add(key, dataAttr);
                this.devForm.attrData.attrDictAccess.ReleaseMutex();
            }
            catch (Exception exception)
            {
                string str2 = "Attribute Dictionary Access Error\nUpdateTmpAttrDict()\n" + exception.Message + "\nAttrDataUtils\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                flag = false;
            }
            return flag;
        }
    }
}

