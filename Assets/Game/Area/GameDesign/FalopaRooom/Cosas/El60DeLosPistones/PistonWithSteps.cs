using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class PistonWithSteps : EntityBase
{
    [SerializeField] protected Transform _root = null;
    [SerializeField] protected List<Transform> nodes = new List<Transform>();
    [SerializeField] Transform parent = null;

    public float speed;

    protected Vector3 _dir;
    protected int currentNode = 0;
    bool isMoving = false;


    public UnityEvent OnEnd;

    public UnityEvent OnBegin;

    void  FixedUpdate()
    {
        if(isMoving)
        {
            Move();   
            
            if (Vector3.Distance(_root.position, nodes[currentNode].position) <= 0.5f)
            {
                var jump = nodes[currentNode].GetComponent<JumpNode>();
                if (jump != null)
                {
                    isMoving = false;
                    GoToNextNode();

                }
                else
                {
                    _root.position = nodes[currentNode].position;
                    isMoving = false;
                    OnStopMove();
                }
                
            }
        }
    }

    public virtual void OnBeginMove()
    {
        OnBegin.Invoke();
    }
    public virtual void OnStopMove()
    {
        OnEnd.Invoke();
    }

    public virtual void Move()
    {
        _root.transform.position += _dir * speed * Time.fixedDeltaTime;
    }

    public void StickPlayerToPlatform()
    {
        Main.instance.GetChar().transform.parent = parent.transform;
    }

    public void RemovePlayerFromPlatform()
    {
        if(Main.instance.GetChar().transform.parent == parent.transform)
            Main.instance.GetChar().transform.parent = Main.instance.GetChar().MyParent;
    }

    public void GoToNextNode()
    {



        if (isMoving) return;
        isMoving = true;

        OnBeginMove();

        if (nodes.Count -1 == currentNode)
        {
            nodes.Reverse();
            currentNode = 0;
        }

        currentNode++;
        var nextNode = nodes[currentNode];
        _dir = nextNode.position - _root.position;
        _dir = _dir.normalized;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (i + 1 == nodes.Count) continue;

            Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        }
    }

    protected override void OnInitialize() { }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnUpdate() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
}
