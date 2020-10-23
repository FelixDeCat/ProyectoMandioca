using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GOAP
{
    public class Navigation : MonoBehaviour
    {
        public static Navigation instance;
        public List<Waypoint> _waypoints = new List<Waypoint>();
        public Transform floorZero;
        public Transform floorOne; 
        [SerializeField] LayerMask mask = 0;

        public void LocalizeNodes()
        {
            foreach (Waypoint node in _waypoints)
            {
                GetSurfacePos(node.transform);
            }
        }

        public void GetSurfacePos(Transform node)
        {
            RaycastHit hit;

            if (Physics.Raycast(node.position, Vector3.down, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore))
                node.position = hit.point;

        }

        void Awake()
        {
            instance = this;

            foreach (Transform xf in floorZero)
            {
                var wp = xf.GetComponent<Waypoint>();
                if (wp != null)
                {
                    _waypoints.Add(wp);
                }
            }

            foreach (Transform xf in floorOne)
            {
                var wp = xf.GetComponent<Waypoint>();
                if (wp != null)
                {
                    _waypoints.Add(wp);
                }
            }

            foreach (Waypoint wp in _waypoints)
            {
                wp.InitializeNodes();
            }
        }

        //public bool Reachable(Vector3 from, Vector3 to, List<Tuple<Vector3, Vector3>> debugRayList = null)
        //{
        //    var srcWp = NearestTo(from);
        //    var dstWp = NearestTo(to);

        //    Waypoint wp = srcWp;

        //    if (srcWp != dstWp)
        //    {
        //        var path = AStar<Waypoint>.Run(
        //              srcWp
        //            , dstWp
        //            , (wa, wb) => Vector3.Distance(wa.transform.position, wb.transform.position)
        //            , w => w == dstWp
        //            , w =>
        //                w.adyacent
        //                    //.Where(a => a.nearbyItems.All(it => it.type != ItemType.Door))
        //                    .Select(a => new AStar<Waypoint>.Arc(a, Vector3.Distance(a.transform.position, w.transform.position)))
        //        );
        //        if (path == null)
        //            return false;

        //        wp = path.Last();
        //    }
        //    //		Debug.Log("Reachable from " + wp.name);
        //    if (debugRayList != null) debugRayList.Add(Tuple.Create(wp.transform.position, to));

        //    var delta = (to - wp.transform.position);
        //    var distance = delta.magnitude;

        //    return !Physics.Raycast(wp.transform.position, delta / distance, distance, LayerMask.GetMask(new[] { "Blocking" }));
        //}

        public IEnumerable<Item> AllItems()
        {
            return All().Aggregate(FList.Create<Item>(), (a, wp) => a += wp.nearbyItems);
        }

        public IEnumerable<Waypoint> All()
        {
            return _waypoints;
        }

        public Waypoint Random()
        {
            return _waypoints[UnityEngine.Random.Range(0, _waypoints.Count)];
        }

        public Waypoint GetFarAwayWp(int heighLevel, Waypoint targetWp)
        {
            return _waypoints.Where((wp) => wp.heighLevel == heighLevel && Vector3.Distance(targetWp.transform.position, wp.transform.position) > 10).FirstOrDefault();
        }

        public Waypoint NearestTo(Vector3 pos)
        {
            return All()
                .OrderBy(wp => {
                    var d = wp.transform.position - pos;
                    //d.y = 0;
                    return d.sqrMagnitude;
                })
                .First();
        }

        public Waypoint NearestTo(Vector3 pos, int heighLevel)
        {
            return All().Where(wp => wp.heighLevel == heighLevel)
                .OrderBy(wp => {
                    var d = wp.transform.position - pos;
                    //d.y = 0;
                    return d.sqrMagnitude;
                })
                .First();
        }
    }
}


