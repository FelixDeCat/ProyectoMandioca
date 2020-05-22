using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DoorDir
{
    public const int LEFT = 0;
    public const int UP = 1;
    public const int DOWN = 2;
    public const int RIGHT = 3;

    public static int Counter(this int val)
    {
        if (val == 0) return 3;
        if (val == 1) return 2;
        if (val == 2) return 1;
        if (val == 3) return 0;

        Debug.LogError("ESTA MAAAAAAL");
        return -1;
    }

    public static Vector3 V3ToSpawn(this int val)
    {
        if (val == 0) return new Vector3(-1, 0, 0);
        if (val == 1) return new Vector3(0, 0, 1);
        if (val == 2) return new Vector3(0, 0, -1);
        if (val == 3) return new Vector3(1, 0, 0);

        Debug.LogError("ESTA MAAAAAAL");
        return new Vector3(3, 3, 3);
    }
}
