﻿namespace BTool
{
    using System;

    public enum GATT_CharProperties
    {
        AuthenticatedSignedWrites = 0x40,
        Broadcast = 1,
        ExtendedProperties = 0x80,
        Indicate = 0x20,
        Notify = 0x10,
        Read = 2,
        Write = 8,
        WriteWithoutResponse = 4
    }
}
