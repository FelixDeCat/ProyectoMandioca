using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCorners : MonoBehaviour
{
    public CellCorner XiZi;
    public CellCorner XfZf;

    public DimensionSectorRange dimensionSectorRange;

    public bool drawgizmos;

    //al pedo hacerle un initialize...
    //porque vamos a usar los Position asi como viene

    //aca deveria hacerle el ONGUI

    public void Initialize()
    {
        drawgizmos = false;

        dimensionSectorRange = new DimensionSectorRange();
        dimensionSectorRange.x_min = (int)XiZi.transform.position.x;
        dimensionSectorRange.z_min = (int)XfZf.transform.position.z;

        dimensionSectorRange.x_max = (int)XfZf.transform.position.x;
        dimensionSectorRange.z_max = (int)XiZi.transform.position.z;
    }



    public void OnDrawGizmos()
    {
        if (XiZi) XiZi.ChangeColor(Color.red, drawgizmos);
        if (XfZf) XfZf.ChangeColor(Color.red, drawgizmos);

        if (!drawgizmos) return;

        if (XiZi && XfZf) {
            XiZi.ChangeColor(Color.green, drawgizmos);
            XfZf.ChangeColor(Color.green, drawgizmos);
           
        }
        else {
            if (XiZi) XiZi.ChangeColor(Color.red, drawgizmos);
            if (XfZf) XfZf.ChangeColor(Color.red, drawgizmos);
            return;
        }

        Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
        var v1 = XiZi.transform.position + offset;
        var v2 = XfZf.transform.position + offset;
        int same = 1;


        Gizmos.DrawLine(new Vector3(v1.x, same, v1.z), new Vector3(v2.x, same, v1.z));
        Gizmos.DrawLine(new Vector3(v1.x, same, v1.z), new Vector3(v1.x, same, v2.z));

        Gizmos.DrawLine(new Vector3(v2.x, same, v2.z), new Vector3(v2.x, same, v1.z));
        Gizmos.DrawLine(new Vector3(v2.x, same, v2.z), new Vector3(v1.x, same, v2.z));

    }
}
