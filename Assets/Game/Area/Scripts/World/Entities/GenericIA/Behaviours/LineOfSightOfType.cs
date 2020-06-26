using System.Collections;
using System.Collections.Generic;
using Tools.Extensions;
using UnityEngine;
using System;
using System.Linq;

public class LineOfSightOfType<T> : MonoBehaviour where T : Component
{

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    Transform myTransform;
    Action<T> objectFinded;
    Func<T, bool> condition;
    LayerMask layermask;

    float viewDistance = 10;
    float viewAngle = 45;
    float radiusToFind = 5;
    Vector3 offset = new Vector3(0, 0.3f, 0);

    float _angleToTarget;
    Vector3 _directionToTarget;
    float _distanceToTarget;
    Transform target;

    public void Configurate(Transform _mytransform,
                            Action<T> _ObjectFinded,
                            Func<T, bool> _condition,
                            LayerMask _layermask,
                            float _viewDistance = 10,
                            float _viewAngle = 45,
                            float _radiusToFind = 5,
                            float _x_offset = 0,
                            float _y_offset = 0.3f,
                            float _z_offest = 0)
    {
        myTransform = _mytransform;
        objectFinded = _ObjectFinded;
        condition = _condition;
        layermask = _layermask;
        viewDistance = _viewDistance;
        viewAngle = _viewAngle;
        radiusToFind = _radiusToFind;
        offset = new Vector3(_x_offset, _y_offset, _z_offest);
    }

    public void FindNews()
    {
        var col = new List<T>();
        foreach (var v in Physics.OverlapSphere(myTransform.position, radiusToFind))
        {
            if (v.GetComponent<T>() != null) col.Add(v.GetComponent<T>());
        }

        for (int i = 0; i < col.Count; i++)
        {
            target = col[i].transform;

            _distanceToTarget = Vector3.Distance(myTransform.position, target.position);
            if (_distanceToTarget > viewDistance) return;

            _directionToTarget = target.position - myTransform.position;
            _directionToTarget.Normalize();

            _angleToTarget = Vector3.Angle(myTransform.forward, _directionToTarget);
            if (_angleToTarget <= viewAngle)
            {
                RaycastHit raycastInfo;

                if (Physics.Raycast(myTransform.position + offset, _directionToTarget, out raycastInfo, 100, layermask))
                {
                    if (raycastInfo.collider.gameObject.GetComponent<T>() != null)
                    {
                        if (condition(col[i]))
                        {
                            objectFinded(col[i]);
                        }
                    }
                }
            }
        }
    }
    public bool OnSight(T obj)
    {
        
        _distanceToTarget = Vector3.Distance(myTransform.position, obj.transform.position);
        if (_distanceToTarget > viewDistance) return false;

        _directionToTarget = obj.transform.position - myTransform.position;
        _directionToTarget.Normalize();

        _angleToTarget = Vector3.Angle(myTransform.forward, _directionToTarget);
        if (_angleToTarget <= viewAngle)
        {
            RaycastHit raycastInfo;

            if (Physics.Raycast(myTransform.position + offset, _directionToTarget, out raycastInfo, 100, layermask))
            {
                var detected = raycastInfo.collider.gameObject.GetComponent<T>();

                if (detected != null)
                {
                    if (detected.Equals(obj))
                        return true;
                }
            }
        }
        return false;
    }


    public bool DrawGizmos;
    protected virtual void OnDrawGizmos()
    {
        if (!DrawGizmos) return;
        Vector3 dir = new Vector3();
        if (!myTransform) return;
        if (target)
        {
            dir = target.position - myTransform.position;
            dir.Normalize();

            Gizmos.DrawSphere(target.position, 0.1f);
            //   Gizmos.DrawLine(myTransform.position + offset, target.position + offset);
            Gizmos.DrawRay(myTransform.position + offset, dir + offset);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(myTransform.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(myTransform.position, myTransform.position + (myTransform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, myTransform.up) * myTransform.forward;
        Gizmos.DrawLine(myTransform.position, myTransform.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, myTransform.up) * myTransform.forward;
        Gizmos.DrawLine(myTransform.position, myTransform.position + (leftLimit * viewDistance));
    }
}
