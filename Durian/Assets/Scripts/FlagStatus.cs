using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagStatus {

    private static bool flagAvaliable = true;

    public static bool FlagAvaliable
    {
        get
        {
            return flagAvaliable;
        }

        set
        {
            flagAvaliable = value;
        }
    }
}
