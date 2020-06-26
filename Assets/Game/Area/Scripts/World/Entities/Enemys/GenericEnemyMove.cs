using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyMove : MonoBehaviour
{
    float currentSpeed;
    [SerializeField] float initialSpeed = 5;
    Rigidbody rb;
    Transform root;
    [SerializeField] float avoidWeight = 1.5f;
    [SerializeField] float avoidanceRadious = 2;
    [SerializeField] float rootSpeed = 2;
    [SerializeField] LayerMask avoidMask = 0;
    [SerializeField] Vector3 mySensorPos = new Vector3(0, 0.2f, 0.5f);
    [SerializeField] bool drawLines = true;
    [SerializeField] float mySideSensorPos = 0.3f;
    [SerializeField] float mySideSensorAngle = 30;

    public void Configure(Transform _root, Rigidbody _rb) { rb = _rb; Configure(_root); }

    public void Configure(Transform _root) { root = _root; currentSpeed = initialSpeed; }

    public float GetCurrentSpeed() => currentSpeed;
    public float GetInitSpeed() => initialSpeed;
    public void SetCurrentSpeed(float _speed) => currentSpeed = _speed;
    public float SumOrSubsSpeed(float _speed) => currentSpeed += _speed;
    public float MultiplySpeed(float _speed) => currentSpeed *= _speed;
    public float DivideSpeed(float _speed) => currentSpeed /= _speed;
    public void SetDefaultSpeed() => currentSpeed = initialSpeed;


    ///<summary> Movement with Rigidbody. Returns a promedy with the values of the vector result (for animations or rotation).
    ///</summary>
    public float MoveWRigidbodyF(Vector3 dir)
    {
        float y = rb.velocity.y;

        rb.velocity = new Vector3(dir.x * currentSpeed, y, dir.z * currentSpeed);

        return dir.normalized.x + dir.normalized.z;
    }

    ///<summary> Movement with Rigidbody. Returns the vector result normalized (for animations or rotation).
    ///</summary>
    public Vector3 MoveWRigidbodyV(Vector3 dir)
    {
        float y = rb.velocity.y;

        rb.velocity = new Vector3(dir.x * currentSpeed, y, dir.z * currentSpeed);

        return dir.normalized;
    }

    ///<summary> Rotation with Transform with lerp (forward/dir value).
    ///</summary>
    public void Rotation(Vector3 forward)
    {
        root.forward = Vector3.Lerp(root.forward, forward, rootSpeed * Time.deltaTime);
    }

    ///<summary> Stop the velocity of the Rigidbody.
    ///</summary>
    public void StopMove()
    {
        rb.velocity = Vector3.zero;
    }

    ///<summary> Movement with transform. Returns a promedy with the values of the vector result (for animations or rotation).
    ///</summary>
    public float MoveWTransformF(Vector3 dir)
    {
        root.position += dir * Time.deltaTime * currentSpeed;

        return dir.normalized.x + dir.normalized.z;
    }

    ///<summary> Movement with Rigidbody. Returns the vector result normalized (for animations or rotation).
    ///</summary>
    public Vector3 MoveWTransformV(Vector3 dir)
    {
        root.position += dir * Time.deltaTime * currentSpeed;

        return dir.normalized;
    }


    ///<summary> Basic Obstacle Avoidance. Returns a new vector result.
    ///</summary>
    //bool obs = false;
    //public Vector3 ObstacleAvoidance(Vector3 dir)
    //{
    //    //Avoidance con Raycast
    //    Vector3 myDir = dir;
    //    RaycastHit hit;
    //    Vector3 startSensorPos = root.position;
    //    startSensorPos += root.forward * mySensorPos.z;
    //    startSensorPos += root.up * mySensorPos.y;
    //    float totalWeight = 0;
    //    obs = false;


    //    startSensorPos += root.right * mySideSensorPos;
    //    if (Physics.Raycast(startSensorPos, root.forward, out hit, avoidanceRadious, avoidMask))
    //    {
    //        if (drawLines)
    //            Debug.DrawLine(startSensorPos, hit.point);

    //        obs = true;
    //        totalWeight -= avoidWeight;
    //    }
    //    else if (Physics.Raycast(startSensorPos,Quaternion.AngleAxis(mySideSensorAngle, root.up) * root.forward, out hit, avoidanceRadious, avoidMask))
    //    {
    //        if (drawLines)
    //            Debug.DrawLine(startSensorPos, hit.point);

    //        obs = true;
    //        totalWeight -= avoidWeight / 2;
    //    }

    //    startSensorPos -= root.right * mySideSensorPos * 2;
    //    if (Physics.Raycast(startSensorPos, root.forward, out hit, avoidanceRadious, avoidMask))
    //    {
    //        if (drawLines)
    //            Debug.DrawLine(startSensorPos, hit.point);

    //        obs = true;
    //        totalWeight += avoidWeight;
    //    }
    //    else if (Physics.Raycast(startSensorPos, Quaternion.AngleAxis(mySideSensorAngle, root.up) * root.forward, out hit, avoidanceRadious, avoidMask))
    //    {
    //        if (drawLines)
    //            Debug.DrawLine(startSensorPos, hit.point);

    //        obs = true;
    //        totalWeight += avoidWeight / 2;
    //    }

    //    if(totalWeight == 0)
    //    {
    //        startSensorPos += root.right * mySideSensorPos;
    //        if (Physics.Raycast(startSensorPos, root.forward, out hit, avoidanceRadious, avoidMask))
    //        {
    //            if (drawLines)
    //                Debug.DrawLine(startSensorPos, hit.point);

    //            obs = true;
    //            if(hit.normal.x<0)
    //                totalWeight -= avoidWeight;
    //            else
    //                totalWeight += avoidWeight;
    //        }
    //    }

    //    if (obs)
    //        myDir *= totalWeight;

    //    return myDir;
    //}

    Transform obs = null;
    public Vector3 ObstacleAvoidance(Vector3 dir)
    {
        obs = null;
        var friends = Physics.OverlapSphere(root.position, avoidanceRadious, avoidMask, QueryTriggerInteraction.Ignore);
        if (friends.Length > 0)
        {
            foreach (var item in friends)
            {
                if (item.gameObject != this.gameObject)
                {
                    if (!obs)
                        obs = item.transform;
                    else if (Vector3.Distance(item.transform.position, root.position) < Vector3.Distance(obs.position, root.position))
                        obs = item.transform;
                }
            }
        }

        if (obs)
        {
            Vector3 diraux = (root.position - obs.position).normalized;

            diraux = new Vector3(diraux.x, 0, diraux.z);

            dir += diraux * avoidWeight;
        }

        return dir;
    }
}
