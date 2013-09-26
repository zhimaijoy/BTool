﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class AttrUuid
    {
        public const ushort InvalidConnHandle = 0xffff;
        public const string InvalidData = "";
        public const ushort InvalidHandle = 0;
        public static Dictionary<string, UuidData> uuidDict = new Dictionary<string, UuidData>();
        public static Mutex uuidDictAccess = new Mutex();
    }
}
