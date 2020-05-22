using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell4Pos : CellObject
{
    
    public enum Rot { LeftSup, RightSup, LeftInf, RightInf }
    [Header("Cell4Pos")] public Rot rot_type;

    public override void Refresh()
    {
        switch (rot_type)
        {
            case Rot.LeftSup:
                rotator.eulerAngles = new Vector3(0, 270, 0);
                break;
            case Rot.RightSup:
                rotator.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Rot.LeftInf:
                rotator.eulerAngles = new Vector3(0, 180, 0);
                break;
            case Rot.RightInf:
                rotator.eulerAngles = new Vector3(0, 90, 0);
                break;
        }
    }
}
