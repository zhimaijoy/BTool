﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class HCIReplies
    {
        public TxDataOut.CmdType cmdType;
        public HCI_LE_ExtEvent hciLeExtEvent;
        public object objTag;

        [StructLayout(LayoutKind.Sequential)]
        public struct ATT_MsgHeader
        {
            public ushort connHandle;
            public byte pduLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HandleData
        {
            public ushort handle;
            public byte[] data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HandleHandleData
        {
            public ushort handle1;
            public ushort handle2;
            public byte[] data;
        }

        public class HCI_LE_ExtEvent
        {
            public ATT_ErrorRsp attErrorRsp;
            public ATT_ExecuteWriteRsp attExecuteWriteRsp;
            public ATT_FindByTypeValueRsp attFindByTypeValueRsp;
            public ATT_FindInfoRsp attFindInfoRsp;
            public ATT_HandleValueIndication attHandleValueIndication;
            public ATT_HandleValueNotification attHandleValueNotification;
            public ATT_PrepareWriteRsp attPrepareWriteRsp;
            public ATT_ReadBlobRsp attReadBlobRsp;
            public ATT_ReadByGrpTypeRsp attReadByGrpTypeRsp;
            public ATT_ReadByTypeRsp attReadByTypeRsp;
            public ATT_ReadRsp attReadRsp;
            public ATT_WriteRsp attWriteRsp;
            public GAP_HCI_ExtentionCommandStatus gapHciCmdStat;
            public HCIReplies.LE_ExtEventHeader header;

            public class ATT_ErrorRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public byte errorCode;
                public ushort handle;
                public byte reqOpCode;
            }

            public class ATT_ExecuteWriteRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
            }

            public class ATT_FindByTypeValueRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public ushort[] handle;
            }

            public class ATT_FindInfoRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public byte format;
                public List<HCIReplies.HandleData> handleData;
            }

            public class ATT_HandleValueIndication
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public ushort handle;
                public string value;
            }

            public class ATT_HandleValueNotification
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public ushort handle;
                public string value;
            }

            public class ATT_PrepareWriteRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public ushort handle;
                public ushort offset;
                public string value;
            }

            public class ATT_ReadBlobRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public byte[] data;
            }

            public class ATT_ReadByGrpTypeRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public List<HCIReplies.HandleHandleData> handleHandleData;
                public byte length;
            }

            public class ATT_ReadByTypeRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public List<HCIReplies.HandleData> handleData;
                public byte length;
            }

            public class ATT_ReadRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
                public byte[] data;
            }

            public class ATT_WriteRsp
            {
                public HCIReplies.ATT_MsgHeader attMsgHdr;
            }

            public class GAP_HCI_ExtentionCommandStatus
            {
                public ushort cmdOpCode;
                public byte dataLength;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LE_ExtEventHeader
        {
            public ushort eventCode;
            public byte eventStatus;
        }
    }
}
