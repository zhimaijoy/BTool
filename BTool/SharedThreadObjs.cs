﻿using System;

public class SharedThreadObjs
{
    private const string moduleName = "SharedThreadObjs";

    public enum HandleIndex
    {
        Handle_Custom = 3,
        Handle_Data = 2,
        Handle_Exit = 0,
        Handle_Pause = 1,
        Timeout = 0x102
    }
}

