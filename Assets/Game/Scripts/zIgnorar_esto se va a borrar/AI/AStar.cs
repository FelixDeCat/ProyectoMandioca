using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA2;

namespace IA_Felix
{
    public class AStar
    {

        public List<Node> open = new List<Node>();
        public List<Node> closed = new List<Node>();

        public List<Node> path = new List<Node>();

        Node current;

        public Node initial;
        public Node final;

        bool drawpath = false;

        private void OnDrawGizmos()
        {
            if (drawpath)
            {
                foreach (var n in path)
                {
                    if (n.parent != null)
                    {
                        Gizmos.DrawLine(n.transform.position, n.parent.transform.position);
                    }
                }
            }
        }

        public List<Node> Execute(Node initial, Node final)
        {
            open.Clear();
            closed.Clear();

            open.Add(initial);

            while (open.Count > 0)
            {
                float small = float.MaxValue;

                foreach (var n in open)
                {
                    if (n.costs.cost < small)
                    {
                        small = n.costs.cost;
                        current = n;
                    }
                }
                open.Remove(current);
                closed.Add(current);

                if (current == final)
                {
                    path.Clear();
                    Node curr = final;
                    while (!curr.Equals(initial))
                    {
                        path.Add(curr);
                        curr = curr.parent;
                    }
                    path.Reverse();
                    return path;
                }

                var vecinos = current.vecinos;

                if (current.vecinos.Count == 0)
                {

                }

                foreach (var v in vecinos)
                {
                    v.costs.fitness = Vector3.Distance(current.transform.position, v.transform.position);

                    if (final == null) return null;
                     v.costs.heuristic = Vector3.Distance(v.transform.position, final.transform.position);
                    v.costs.cost = v.costs.fitness + v.costs.heuristic;

                    if (!closed.Contains(v))
                    {
                        v.parent = current;
                        open.Add(v);
                    }
                }
            }

            return null;
        }
    }
}

