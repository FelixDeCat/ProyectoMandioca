using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTo : MonoBehaviour
{
    [Header("CLOSE")]
    public float d_close = 2;
    public float tr_Close = 1;

    [Header("MID")]
    public float d_mid = 4;
    public float tr_Mid = 2;

    [Header("CLOSE")]
    public float d_far = 6;
    public float tr_Far = 3;

    Transform myTransform;

    public void Configure(Transform _mytransform)
    {
        myTransform = _mytransform;
    }

    //d : distance , t : time
    //menor a d_close retorno 0
    //mayor a d_close retorno t_close
    //mayor a d_mid retorno t_mid
    //menor a d_far retorno t_far
    public float RetTimeToCloseness(Vector3 _target)
    {
        float dist = Vector3.Distance(myTransform.position, _target);

        if (dist < d_close)
        {
            return 0;
        }
        else
        {
            if (dist > d_mid)
            {
                if (dist > d_far)
                {
                    return tr_Far;
                }
                else
                {
                    return tr_Mid;
                }
            }
            else
            {
                return tr_Close;
            }
        }
    }
}
