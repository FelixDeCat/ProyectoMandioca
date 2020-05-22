using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCornerManager : MonoBehaviour
{
    public List<RangeCorners> ranges;

    public void Initialize()
    {
        foreach (var r in ranges)
        {
            r.Initialize();
        }
    }
}
