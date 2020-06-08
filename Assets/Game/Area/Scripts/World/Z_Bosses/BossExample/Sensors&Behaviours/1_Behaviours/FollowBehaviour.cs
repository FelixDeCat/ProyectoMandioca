using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericEnemyMove))]
public class FollowBehaviour : MonoBehaviour
{
    public Vector3 Destiny { get { return target.position; } }
    public Vector3 Origin { get { return root.position; } }
    public Vector3 Direction { get { return (Destiny - Origin).normalized; } }

    [SerializeField] GenericEnemyMove genericEnemyMove;

    Transform root;
    Rigidbody rb;
    Transform target;
    bool follow;
    bool scape;
    bool lookat;

    private void Awake()
    {
        genericEnemyMove = GetComponent<GenericEnemyMove>();
    }

    public void ConfigureFollowBehaviour(Transform root, Rigidbody rb, Transform target)
    {
        this.root = root;
        this.rb = rb;
        this.target = target;
        genericEnemyMove.Configure(root, rb);
    }

    public void ChangeTarget(Transform target) => this.target = target;
    public void StartFollow() { follow = true; }
    public void StopFollow() { follow = false; }
    public void StartScape() { scape = true; }
    public void StopScape() { scape = false; }
    public void StartLookAt() { lookat = true; }
    public void StopLookAt() { lookat = false; }

    void Update()
    {
        if (follow)
        {
            Vector3 forwardFix = genericEnemyMove.ObstacleAvoidance(new Vector3(Direction.x, 0, Direction.z));
            genericEnemyMove.MoveWRigidbodyV(forwardFix);
            genericEnemyMove.Rotation(forwardFix);
        }
        else if (scape)
        {
            Vector3 forwardFix = genericEnemyMove.ObstacleAvoidance(new Vector3(Direction.x, 0, Direction.z));
            genericEnemyMove.MoveWRigidbodyV(forwardFix *-1);
            genericEnemyMove.Rotation(forwardFix);
        }
        else
        {
            if (lookat)
            {
                genericEnemyMove.Rotation(Direction);
            }
        }
        
    }
}
