using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionSectorManager : MonoBehaviour
{

    public Transform t1, t2, lookat;

    //son para optimizar proceso
    public int x_global_max = int.MinValue;
    public int x_global_min = int.MaxValue;
    public int z_global_max = int.MinValue;
    public int z_global_min = int.MaxValue;

    public List<DimensionSectorRange> sector_range = new List<DimensionSectorRange>();

    public void OnUpdate()
    {
        if (t1) t1.position = new Vector3(x_global_min, 0, z_global_min);
        if (t1) t2.position = new Vector3(x_global_max, 0, z_global_max);

        var xpos = x_global_min + (x_global_max - x_global_min) / 2;
        var zpos = z_global_min + (z_global_max - z_global_min) / 2;

        if (lookat) lookat.position = new Vector3(xpos, 0, zpos);
    }

    public void ONReset()
    {
        x_global_max = int.MinValue;
        x_global_min = int.MaxValue;
        z_global_max = int.MinValue;
        z_global_min = int.MaxValue;

        sector_range.Clear();
    }

    public void AddDimensionRange(DimensionSectorRange toadd)
    {
        //optimizador   
        x_global_max = toadd.x_max > x_global_max ? toadd.x_max : x_global_max;
        x_global_min = toadd.x_min < x_global_min ? toadd.x_min : x_global_min;
        z_global_max = toadd.z_max > z_global_max ? toadd.z_max : z_global_max;
        z_global_min = toadd.z_min < z_global_min ? toadd.z_min : z_global_min;

        sector_range.Add(toadd);
    }

    public bool CanAddDimensionRange(DimensionSectorRange current)
    {

        //optimizador
        if (current.x_max < x_global_min) return true;
        if (current.x_min > x_global_max) return true;
        if (current.z_max < z_global_min) return true;
        if (current.z_min > z_global_max) return true;

        //si no se cumple ninguna de las 4, es una lastima xq no se puede optimizar

        //Debug.Log("Sector Range Count: " + sector_range.Count);

        foreach (var s in sector_range)
        {
            if (current.EstaDentroDe(s))
            {

                return false;
            }
        }
        return true;
    }



}

[System.Serializable]
public struct DimensionSectorRange
{
    public int x_max;
    public int x_min;
    public int z_max;
    public int z_min;

    public override string ToString()
    {
        return "xmax: " + x_max + " xmin: " + x_min + " zmax: " + z_max + " xmin: " + z_min;
    }
}


public static class RangeTools
{
    public static bool EstaDentroDe(this DimensionSectorRange curr, DimensionSectorRange i)
    {
        bool res = false;

        if (curr.x_min.Inside(i.x_min, i.x_max) || curr.x_max.Inside(i.x_min, i.x_max))
        {
            if (curr.z_min.Inside(i.z_min, i.z_max) || curr.z_max.Inside(i.z_min, i.z_max))
            {
                res = true;
            }
            else
            {
                if (i.z_min.Inside(curr.z_min, curr.z_max) || i.z_max.Inside(curr.z_min, curr.z_max))
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
            }
        }
        else
        {
            if (curr.z_min.Inside(i.z_min, i.z_max) || curr.z_max.Inside(i.z_min, i.z_max))
            {
                if (curr.x_min.Inside(i.x_min, i.x_max) || curr.x_max.Inside(i.x_min, i.x_max))
                {
                    res = true;
                }
                else
                {
                    if (i.x_min.Inside(curr.x_min, curr.x_max) || i.x_max.Inside(curr.x_min, curr.x_max))
                    {
                        res = true;
                    }
                    else
                    {
                        res = false;
                    }
                }
            }
            else
            {
                res = false;
            }
        }


        return res;


        //bool res =
        //    (
        //        (
        //            (curr.x_min.Inside(i.x_min, i.x_max) || curr.x_max.Inside(i.x_min, i.x_max)) 
        //        )
        //            &&
        //        (
        //            (curr.z_min.Inside(i.z_min, i.z_max) || curr.z_max.Inside(i.z_min, i.z_max))
        //        )
        //    );

        //return res;
    }

    public static bool Inside(this int value, int min, int max) => (value >= min && value <= max);
}