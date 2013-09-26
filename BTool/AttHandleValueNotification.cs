﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttHandleValueNotification
    {
        public AttHandleValueNotificationDelegate AttHandleValueNotificationCallback;
        private AttrDataUtils attrDataUtils;
        private AttrUuidUtils attrUuidUtils = new AttrUuidUtils();
        private const string moduleName = "AttHandleValueNotification";
        private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

        public AttHandleValueNotification(DeviceForm deviceForm)
        {
            this.attrDataUtils = new AttrDataUtils(deviceForm);
        }

        public bool GetATT_HandleValueNotification(HCIReplies hciReplies, ref bool dataFound)
        {
            bool flag = true;
            dataFound = false;
            if (flag = this.rspHdlrsUtils.CheckValidResponse(hciReplies))
            {
                HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
                HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification attHandleValueNotification = hciLeExtEvent.attHandleValueNotification;
                HCIReplies.LE_ExtEventHeader header = hciLeExtEvent.header;
                if (attHandleValueNotification != null)
                {
                    dataFound = true;
                    switch (header.eventStatus)
                    {
                        case 0:
                            if (attHandleValueNotification.value != null)
                            {
                                Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
                                string attrKey = this.attrUuidUtils.GetAttrKey(attHandleValueNotification.attMsgHdr.connHandle, attHandleValueNotification.handle);
                                DataAttr dataAttr = new DataAttr();
                                bool dataChanged = false;
                                if (!this.attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttHandleValueNotification"))
                                {
                                    flag = false;
                                }
                                else
                                {
                                    dataAttr.key = attrKey;
                                    dataAttr.connHandle = attHandleValueNotification.attMsgHdr.connHandle;
                                    dataAttr.handle = attHandleValueNotification.handle;
                                    dataAttr.value = attHandleValueNotification.value;
                                    if (!this.attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
                                    {
                                        flag = false;
                                    }
                                    else if (!this.attrDataUtils.UpdateAttrDict(tmpAttrDict))
                                    {
                                        flag = false;
                                    }
                                    else
                                    {
                                        this.SendRspCallback(hciReplies, true);
                                    }
                                }
                            }
                            goto Label_012F;
                    }
                    flag = this.rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttHandleValueNotification");
                }
            }
        Label_012F:
            if (!flag && dataFound)
            {
                this.SendRspCallback(hciReplies, false);
            }
            return flag;
        }

        private void SendRspCallback(HCIReplies hciReplies, bool success)
        {
            if (this.AttHandleValueNotificationCallback != null)
            {
                RspInfo rspInfo = new RspInfo();
                rspInfo.success = success;
                rspInfo.header = hciReplies.hciLeExtEvent.header;
                rspInfo.aTT_HandleValueNotification = hciReplies.hciLeExtEvent.attHandleValueNotification;
                this.AttHandleValueNotificationCallback(rspInfo);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RspInfo
        {
            public bool success;
            public HCIReplies.LE_ExtEventHeader header;
            public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification aTT_HandleValueNotification;
        }
    }
}

