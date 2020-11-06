﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Extensions;
using IA2;
using IA_Felix;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPathFinder : GenericEnemyMove
{
    public bool canMove;

    [Header("Configuration")]
    public float movement_speed = 0.3f;
    public float radius_to_find_nodes = 1.5f;
    public float distance_to_close = 0.1f;
    public float forwardspeed = 5f;

    float auxspeed;
    public Transform roottt;
    public CharacterGroundSensor groundSensorrr;

    NodeFinder nodefinder;
    AStar astar = new AStar();
    Queue<Node> nodosASeguir = new Queue<Node>();
    Node currentNode;
    Node initialNode;
    Node finalNode;

    public Action callbackEndDinamic = delegate { };

    public void SpeedRelaxSearch() => movement_speed = auxspeed;
    public void SpeedAlertSearch(float _speed) => movement_speed = _speed;

    public void Initialize(Rigidbody _rb)
    {
        rb = _rb;
        rb.isKinematic = true;
        nodefinder = new NodeFinder(radius_to_find_nodes);
        auxspeed = movement_speed;
        Configure(roottt, rb, groundSensorrr);
    }

    public void AddCallbackEnd(Action _callbackEndDinamic)
    {
        callbackEndDinamic = _callbackEndDinamic;
    }
    internal void RemoveCallbackEnd()
    {
        callbackEndDinamic = delegate { };
    }

    public void Execute(Vector3 pos)
    {
        rb.isKinematic = false;

        initialNode = nodefinder.FindMostCloseNode(rb.transform.position);
        finalNode = nodefinder.FindMostCloseNode(pos);

        if (initialNode == null) { Debug.LogError("Initial node es nulo"); }
        if (finalNode == null) { Debug.LogError("final node es nulo"); }

        var col = astar.Execute(initialNode, finalNode);

        if (col == null) { Debug.Log("Tengo una Lista nula"); return; }

        nodosASeguir = new Queue<Node>(col);
        currentNode = initialNode;

        //render
        foreach (var n in nodosASeguir) n.render.PintarNegro();
        initialNode.render.PintarRojo();
        finalNode.render.PintarVerde();

        canMove = true;


    }

    bool dequeueNext = false;
    float currentdist = float.MaxValue;
    public void Refresh()
    {
        if (!canMove) return;

        if (currentNode == null) { Debug.LogError("El CurrentNode es Nulo"); }

        currentdist = Vector3.Distance(rb.transform.position, currentNode.transform.position);

        if (currentdist < distance_to_close)
        {

            currentNode.render.PintarNegro();
            if (currentNode == finalNode)
            {
                movement_speed = auxspeed;
                canMove = false;
                rb.isKinematic = true;
                callbackEndDinamic.Invoke();
            }
            dequeueNext = true;
        }

        if (dequeueNext)
        {
            dequeueNext = false;
            if (nodosASeguir.Count > 0)
            {
                currentNode = nodosASeguir.Dequeue();
            }
            else
            {
                return;
            }
        }

        Vector3 dir = currentNode.transform.position - rb.transform.position;
        Vector3 fowardRotation = ObstacleAvoidance(new Vector3(dir.x, 0, dir.z));
        Rotation(fowardRotation.normalized);
        MoveWRigidbodyV(ObstacleAvoidance(fowardRotation));


    }
}




















