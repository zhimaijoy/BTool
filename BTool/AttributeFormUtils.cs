﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public class AttributeFormUtils
    {
        private const string moduleName = "DataUtils";
        private MsgBox msgBox = new MsgBox();

        public bool WriteCsv(string pathFileNameStr, List<CsvData> csvData)
        {
            bool flag = true;
            try
            {
                if ((csvData == null) || (csvData.Count <= 0))
                {
                    throw new ArgumentException(string.Format("There Is No Data To Save\n", new object[0]));
                }
                using (StreamWriter writer = new StreamWriter(pathFileNameStr))
                {
                    string str2 = string.Empty;
                    str2 = string.Format("=\"{0:S}\",=\"{1:S}\",=\"{2:S}\",=\"{3:S}\",=\"{4:S}\",=\"{5:S}\",=\"{6:S}\"", new object[] { AttributesForm.ListSubItem.ConnectionHandle.ToString(), AttributesForm.ListSubItem.Handle.ToString(), AttributesForm.ListSubItem.Uuid.ToString(), AttributesForm.ListSubItem.UuidDesc.ToString(), AttributesForm.ListSubItem.Value.ToString(), AttributesForm.ListSubItem.ValueDesc.ToString(), AttributesForm.ListSubItem.Properties.ToString() });
                    writer.WriteLine(str2);
                    foreach (CsvData data in csvData)
                    {
                        str2 = string.Format("=\"{0:S}\",=\"{1:S}\",=\"{2:S}\",=\"{3:S}\",=\"{4:S}\",=\"{5:S}\",=\"{6:S}\"", new object[] { data.connectionHandle, data.handle, data.uuid, data.uuidDesc, data.value, data.valueDesc, data.properties });
                        writer.WriteLine(str2);
                    }
                    return flag;
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Cannot Write The CSV File\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = false;
            }
            return flag;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CsvData
        {
            public string connectionHandle;
            public string handle;
            public string uuid;
            public string uuidDesc;
            public string value;
            public string valueDesc;
            public string properties;
        }
    }
}
