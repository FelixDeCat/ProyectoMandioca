using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [Header("For Line of Sight")]
    public float viewDistance = 5;
    public float viewAngle = 75;
    float _distanceToTarget;

    Vector3 _directionToTarget;
    float _angleToTarget;

    Transform myTransform;
    Transform target;

    public LayerMask layermask;
    public Vector3 offset;

    public void Configurate(Transform _mytransform)
    {
        myTransform = _mytransform;
    }

    public void DeleteTarget()
    {
        target = null;
    }

    public bool OnSight(Transform _target)
    {
        target = _target;
        _distanceToTarget = Vector3.Distance(myTransform.position, target.position);
        if (_distanceToTarget > viewDistance) return false;

        _directionToTarget = target.position - myTransform.position;
        _directionToTarget.Normalize();

        _angleToTarget = Vector3.Angle(myTransform.forward, _directionToTarget);
        if (_angleToTarget <= viewAngle)
        {
            RaycastHit raycastInfo;
            bool obstaclesBetween = true;

            if (Physics.Raycast(myTransform.position + offset , _directionToTarget , out raycastInfo, 100, layermask, QueryTriggerInteraction.Ignore))
            {
                if (raycastInfo.transform == _target)
                    obstaclesBetween = false;
                else
                    obstaclesBetween = true;
            }
            if (obstaclesBetween) return false;
            else return true;
        }
        else return false;
    }

    Vector3 dir;
    public bool DrawGizmos; 
    protected virtual void OnDrawGizmos()
    {
        if (!DrawGizmos) return;

        //if (_playerInSight)
        //    Gizmos.color = Color.green;
        //else
        //    Gizmos.color = Color.red;

        if (!myTransform) return;
        if (target)
        {
            dir = target.position - myTransform.position;
            dir.Normalize();

            Gizmos.DrawSphere(target.position, 0.1f);
            //   Gizmos.DrawLine(myTransform.position + offset, target.position + offset);
            Gizmos.DrawRay(myTransform.position+ offset, dir +offset);
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
