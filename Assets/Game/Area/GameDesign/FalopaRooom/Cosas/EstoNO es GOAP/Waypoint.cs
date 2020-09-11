using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GOAP
{
    public class Waypoint : MonoBehaviour
    {
        public List<Waypoint> adyacent;
        public HashSet<Item> nearbyItems = new HashSet<Item>();

        void Start()
        {
            //Make bidirectional
            foreach (var wp in adyacent)
            {
                if (wp != null && wp.adyacent != null)
                {
                    if (!wp.adyacent.Contains(this))
                        wp.adyacent.Add(this);
                }
            }
            adyacent = adyacent.Where(x => x != null).Distinct().ToList();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
            Gizmos.color = Color.blue;
            foreach (var wp in adyacent)
            {
                Gizmos.DrawLine(transform.position, wp.transform.position);
            }
        }
    }
}

