using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_animRecolect : MonoBehaviour
{
    bool anim;

    WalkingEntity postofollow;
    Action<WalkingEntity> endcollect;

    public float speed = 5;
    public float distance_to_collect;

    public void BeginRecollect(WalkingEntity collector, Action<WalkingEntity> _EndCollect)
    {
        postofollow = collector;
        endcollect = _EndCollect;
        anim = true;
    }

    private void Update()
    {
        if (anim)
        {
            var dir = postofollow.gameObject.transform.position - this.transform.position;
            dir.Normalize();
            transform.forward = dir;
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(postofollow.transform.position, this.transform.position) < distance_to_collect)
            {
                endcollect.Invoke(postofollow);
            }

        }
    }
}
