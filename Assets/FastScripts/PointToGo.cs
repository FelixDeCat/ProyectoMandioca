using IA_Felix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToGo : MonoBehaviour
{
    public float radius_to_find_nodes = 1.5f;
    NodeFinder finder;

    private void Start()
    {
        finder = new NodeFinder(radius_to_find_nodes);
    }

    public Node GetPosition()
    {
        return finder.FindMostCloseNode(this.transform.position);
    }
}
