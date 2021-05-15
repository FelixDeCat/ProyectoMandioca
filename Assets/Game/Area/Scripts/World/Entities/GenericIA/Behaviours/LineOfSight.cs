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
    public Vector3 targetOffset;

    public void Configurate(Transform _mytransform)
    {
        myTransform = _mytransform;
    }

    public void DeleteTarget()
    {
        target = null;
    }

    public bool OnSight(Transform _target, float viewDis = 0, float angle = 0)
    {
        viewDis = viewDis == 0 ? viewDistance : viewDis;
        angle = angle == 0 ? viewAngle : angle;

        target = _target;
        _distanceToTarget = Vector3.Distance(myTransform.position, target.position);
        if (_distanceToTarget > viewDis) return false;

        _directionToTarget = target.position + targetOffset - myTransform.position;
        _directionToTarget.Normalize();

        _angleToTarget = Vector3.Angle(myTransform.forward, _directionToTarget);
        if (_angleToTarget <= angle)
        {
            RaycastHit raycastInfo;
            bool obstaclesBetween = true;

            if (Physics.Raycast(myTransform.position + offset , _directionToTarget , out raycastInfo, viewDis, layermask, QueryTriggerInteraction.Ignore))
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
            Gizmos.DrawLine(myTransform.position+ offset, myTransform.position + offset + dir * viewDistance);
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
