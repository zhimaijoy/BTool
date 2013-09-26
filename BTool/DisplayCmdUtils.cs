﻿namespace BTool
{
    using System;

    public class DisplayCmdUtils
    {
        private DataUtils dataUtils = new DataUtils();
        private DeviceFormUtils devUtils = new DeviceFormUtils();
        public const string moduleName = "DisplayCmdUtils";

        public void AddConnectHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            ushort num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
            if (!dataErr)
            {
                msg = msg + string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num, num);
            }
        }

        public void AddConnectHandleOffset(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            AddConnectHandle(data, ref index, ref dataErr, ref msg);
            if (!dataErr)
            {
                AddHandleOffset(data, ref index, ref dataErr, ref msg);
            }
        }

        public void AddConnectStartEndHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            this.AddConnectHandle(data, ref index, ref dataErr, ref msg);
            if (!dataErr)
            {
                this.AddStartEndHandle(data, ref index, ref dataErr, ref msg);
            }
        }

        public void AddEndHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            ushort num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
            if (!dataErr)
            {
                msg = msg + string.Format(" EndHandle\t: 0x{0:X4} ({1:D})\n", num, num);
            }
        }

        public ushort AddHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            ushort num = 0;
            num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
            if (!dataErr)
            {
                msg = msg + string.Format(" Handle\t\t: 0x{0:X4} ({1:D})\n", num, num);
            }
            return num;
        }

        public void AddHandleOffset(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            this.AddHandle(data, ref index, ref dataErr, ref msg);
            if (!dataErr)
            {
                this.AddOffset(data, ref index, ref dataErr, ref msg);
            }
        }

        public void AddOffset(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            ushort num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
            if (!dataErr)
            {
                msg = msg + string.Format(" Offset\t\t: 0x{0:X4} ({1:D})\n", num, num);
            }
        }

        public void AddStartEndHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            this.AddStartHandle(data, ref index, ref dataErr, ref msg);
            if (!dataErr)
            {
                this.AddEndHandle(data, ref index, ref dataErr, ref msg);
            }
        }

        public void AddStartHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
        {
            ushort num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
            if (!dataErr)
            {
                msg = msg + string.Format(" StartHandle\t: 0x{0:X4} ({1:D})\n", num, num);
            }
        }

        public void AddValue(byte[] data, ref int index, ref bool dataErr, ref string msg, int length, int headerSize)
        {
            msg = msg + string.Format(" Value\t\t: {0:S}\n", this.devUtils.UnloadColonData(data, ref index, (length + headerSize) - index, ref dataErr));
        }
    }
}

