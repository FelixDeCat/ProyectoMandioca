using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookEnemy : MonoBehaviour
{
    public float rotationSpeed;
    Transform mytransform;

    public void Configure(Transform _mytransform, float _speed = 50f)
    {
        mytransform = _mytransform;
        rotationSpeed = _speed;
    }

    public void Execute(Vector3 target)
    {
        var dirv = target - mytransform.position;
        dirv.y = 0;
        dirv.Normalize();
        mytransform.forward = Vector3.Lerp(mytransform.forward, dirv, rotationSpeed * Time.deltaTime);
    }
    public void DirectClampExecute(Vector3 target)
    {
        var dirv = target - mytransform.position;
        dirv.y = 0;
        dirv.Normalize();
        mytransform.forward = dirv;
    }
}
