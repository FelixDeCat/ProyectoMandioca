using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor_RangeAttack : MonoBehaviour
{
    Transform mytransform;
    public float range = 1;

    Transform target;

    public void Configure(Transform _mytransform)
    {
        mytransform = _mytransform;
    }

    public bool InRangeToAttack(Transform _target)
    {
        target = _target;
        DrawGizmos = true;
        return Vector3.Distance(mytransform.position, target.position) < range;
    }

    bool DrawGizmos;

    protected virtual void OnDrawGizmos()
    {
        if (!DrawGizmos) return;

        Gizmos.color = Color.red;

        Vector3 dir = target.transform.position - mytransform.position;
        dir.Normalize();
        dir *= range;

        Gizmos.DrawLine(mytransform.position, dir);

        DrawGizmos = false;
    }
}
