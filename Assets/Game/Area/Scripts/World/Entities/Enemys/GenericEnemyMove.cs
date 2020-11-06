using UnityEngine;
public class GenericEnemyMove : MonoBehaviour
{
    
    public Rigidbody rb;
    Transform root;

    [SerializeField] float initialSpeed = 5;
    [SerializeField] float avoidWeight = 1.5f;
    [SerializeField] float avoidanceRadious = 2;
    [SerializeField] float rootSpeed = 2;
    [SerializeField] LayerMask avoidMask = 0;
    float currentSpeed;

    CharacterGroundSensor groundSensor;
    bool applyForce;
    float timerForce;

    public void Configure(Transform _root, Rigidbody _rb = null, CharacterGroundSensor _groundSensor = null)
    {
        root = _root;
        rb = _rb;
        groundSensor = _groundSensor;
        currentSpeed = initialSpeed;
    }
    public float GetCurrentSpeed() => currentSpeed;
    public float GetInitSpeed() => initialSpeed;
    public void SetCurrentSpeed(float _speed) => currentSpeed = _speed;
    public float SumOrSubsSpeed(float _speed) => currentSpeed += _speed;
    public float MultiplySpeed(float _speed) => currentSpeed *= _speed;
    public float DivideSpeed(float _speed) => currentSpeed /= _speed;
    public void SetDefaultSpeed() => currentSpeed = initialSpeed;


    ///<summary> Movement with Rigidbody. Returns a promedy with the values of the vector result (for animations or rotation). </summary>
    public float MoveWRigidbodyF(Vector3 dir)
    {
        float y = groundSensor != null ? groundSensor.VelY : rb.velocity.y;
        if (!applyForce) rb.velocity = new Vector3(dir.x * currentSpeed, y, dir.z * currentSpeed);
        return dir.normalized.x + dir.normalized.z;
    }

    ///<summary> Movement with Rigidbody. Returns the vector result normalized (for animations or rotation). </summary>
    public Vector3 MoveWRigidbodyV(Vector3 dir)
    {
        float y = groundSensor != null ? groundSensor.VelY : rb.velocity.y;
        if (!applyForce) rb.velocity = new Vector3(dir.x * currentSpeed, y, dir.z * currentSpeed);

        return dir.normalized;
    }

    ///<summary> Rotation with Transform with lerp (forward/dir value). </summary>
    public void Rotation(Vector3 forward)
    {
        root.forward = Vector3.Lerp(root.forward, forward, rootSpeed * Time.deltaTime);
    }

    ///<summary> Stop the velocity of the Rigidbody. </summary>
    public void StopMove() => rb.velocity = Vector3.zero;

    ///<summary> Movement with transform. Returns a promedy with the values of the vector result (for animations or rotation). </summary>
    public float MoveWTransformF(Vector3 dir)
    {
        root.position += dir * Time.deltaTime * currentSpeed;
        return dir.normalized.x + dir.normalized.z;
    }

    ///<summary> Movement with Rigidbody. Returns the vector result normalized (for animations or rotation). </summary>
    public Vector3 MoveWTransformV(Vector3 dir)
    {
        root.position += dir * Time.deltaTime * currentSpeed;
        return dir.normalized;
    }

    public void ApplyForceToVelocity(Vector3 force)
    {
        ResetForce();
        applyForce = true;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void ResetForce()
    {
        timerForce = 0;
        applyForce = false;
        rb.velocity = Vector3.zero;
    }

    public void OnUpdate()
    {
        if (applyForce)
        {
            timerForce += Time.deltaTime;

            if (timerForce >= 0.3f) ResetForce();
        }
    }

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
