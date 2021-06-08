using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class SimplePath : MonoBehaviour
{
    public bool refresh;
    public Transform[] nodes;
    private void Update()
    {
        if (refresh)
        {
            refresh = false;
            var aux = new HashSet<Transform>();
            foreach(Transform t in this.transform)
            {
                aux.Add(t);
                t.gameObject.name = "node [" + t.GetSiblingIndex().ToString("00") + "]";
            }
            nodes = aux.ToArray();
        }
    }

    private void OnDrawGizmos()
    {
        if (nodes.Length > 1)
        {
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                var current = nodes[i];
                var next = nodes[i + 1];

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(current.position, next.position);
            }
        }
    }
}
