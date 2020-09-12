using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Tools.Extensions;

namespace GOAP
{
    public class Waypoint : MonoBehaviour
    {
        public List<Waypoint> adyacent;
        public HashSet<Item> nearbyItems = new HashSet<Item>();
        public float radious;

        public void InitializeNodes()
        {
            GetAdyacent();

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

        void GetAdyacent()
        {
            adyacent = Extensions.FindInRadiusNoPhysics<Waypoint>(transform.position, radious, Navigation.instance._waypoints);

            if (adyacent.Contains(this)) adyacent.Remove(this);
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

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, radious);
        }
    }
}

