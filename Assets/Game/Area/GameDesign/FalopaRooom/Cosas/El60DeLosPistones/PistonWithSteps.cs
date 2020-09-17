using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PistonWithSteps : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] List<Transform> nodes = new List<Transform>();
    [SerializeField] Transform parent;

    public float speed;

    Vector3 _dir;
    int currentNode = 0;
    bool isMoving;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMoving)
        {
            //Por que con el rb no se pega el player y con transform si?
            _rb.transform.position += _dir * speed * Time.fixedDeltaTime;
            //_rb.MovePosition(_rb.position + _dir * speed * Time.fixedDeltaTime);

            if (Vector3.Distance(_rb.position, nodes[currentNode].position) <= 0.5f)
            {
                _rb.position = nodes[currentNode].position;
                isMoving = false;
            }
        }
    }

    public void StickPlayerToPlatform()
    {
        Main.instance.GetChar().transform.parent = parent.transform;
    }

    public void RemovePlayerFromPlatform()
    {
        Main.instance.GetChar().transform.parent = null;
    }

    public void GoToNextNode()
    {
        if (isMoving) return;

        

        isMoving = true;
        

        if (nodes.Count -1 == currentNode)
        {
            nodes.Reverse();
            currentNode = 0;
        }

        currentNode++;
        var nextNode = nodes[currentNode];
        _dir = nextNode.position - _rb.position;
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
}
