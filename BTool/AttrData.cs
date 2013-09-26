﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;

    public class AttrData
    {
        public SortedDictionary<string, DataAttr> attrDict = new SortedDictionary<string, DataAttr>();
        public Mutex attrDictAccess = new Mutex();
        public static Columns columns;
        public static Color defaultBackground = Color.White;
        public const int defaultConnHandleWidth = 0x37;
        public static Color defaultForeground = Color.Black;
        public const int defaultHandleWidth = 0x37;
        public const int defaultIndentLevel = 0;
        public const int defaultKeyWidth = 70;
        public const int defaultMaxNumPreparedWrites = 5;
        public const int defaultMaxPacketSize = 0x7f;
        public const int defaultPropertiesWidth = 0x90;
        public const byte defaultUnknownIndentLevel = 4;
        public const int defaultUuidDescWidth = 0xe1;
        public const int defaultUuidWidth = 0x37;
        public const int defaultValueDescWidth = 0xaf;
        public const ValueDisplay defaultValueDisplay = ValueDisplay.Hex;
        public const ValueEdit defaultValueEdit = ValueEdit.Editable;
        public const int defaultValueWidth = 150;
        public const int maxAttrData = 0x5dc;
        public const int packetSizeForPreparedWrites = 0x12;
        public bool sendAutoCmds = true;
        public static byte unknownIndentLevel = 4;
        public static WriteLimits writeLimits;

        static AttrData()
        {
            Columns columns = new Columns();
            columns.keyWidth = 70;
            columns.connHandleWidth = 0x37;
            columns.handleWidth = 0x37;
            columns.uuidWidth = 0x37;
            columns.uuidDescWidth = 0xe1;
            columns.valueWidth = 150;
            columns.valueDescWidth = 0xaf;
            columns.propertiesWidth = 0x90;
            AttrData.columns = columns;
            WriteLimits limits = new WriteLimits();
            limits.maxPacketSize = 0x7f;
            limits.maxNumPreparedWrites = 5;
            writeLimits = limits;
        }
    }
}

