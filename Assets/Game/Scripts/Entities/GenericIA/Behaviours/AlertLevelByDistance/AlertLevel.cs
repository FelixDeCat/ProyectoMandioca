using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLevel : MonoBehaviour
{

    Transform target;
    public void Configure(Transform _mytrans,Transform _target)
    {
        distance_to = transform.parent.GetComponentInChildren<DistanceTo>();
        distance_to.Configure(_mytrans);
        target = _target;
    }

    public DistanceTo distance_to;

    public float level;
    public float Max_level {
        get
        {
            return distance_to.RetTimeToCloseness(target.position);
        }
    }
}
