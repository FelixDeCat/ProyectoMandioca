using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cell2Pos : CellObject
{
    public enum Rot { vertical, horizontal }
    private Rot rot_type;
   

    public Rot Rot_type { get => rot_type; set { rot_type = value; } }

    public override void Refresh()
    {
        rotator.eulerAngles = new Vector3(0, Rot_type == Rot.horizontal ? 90 : 0, 0);
    }
}
