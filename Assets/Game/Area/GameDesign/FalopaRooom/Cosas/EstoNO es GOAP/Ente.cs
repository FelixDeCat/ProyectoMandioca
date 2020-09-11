using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GOAP
{
    public class Ente : MonoBehaviour
    {
        public float health;
        public float healthCurrent;

        public event Action<Ente, Waypoint, bool> OnReachDestination = delegate { };
        public event Action<Ente, Item> OnHitItem = delegate { };
        public event Action<Ente, Item> OnStayItem = delegate { };
        public event Action OnTakeDmg = delegate { };

        [SerializeField] Transform _root;


        Animator _anim;

        public Animator Anim ()=> _anim; 

        void FixedUpdate()
        {
            transform.Translate(Time.fixedDeltaTime * _vel * currentSpeed);
        }

        private void Start()
        {
            _anim = GetComponentInChildren<Animator>();

            OnReachDestination += Stop;
        }

        private void Update()
        {
            _anim.SetFloat("speed", currentSpeed);
           
        }

        #region Health

        public void ModifyHealth(int dmg)
        {
            if (dmg < 0)
                OnTakeDmg?.Invoke();

            healthCurrent += dmg;

            if (healthCurrent <= 0)
                Debug.Log("Dead");
        }

        #endregion

        #region Momevent

        Vector3 _vel;
        public float speed = 2f;
        float currentSpeed;
        Waypoint _gizmoRealTarget;
        IEnumerable<Waypoint> _gizmoPath;
        Coroutine _navCR;
        Vector3 FloorPos(MonoBehaviour b)
        {
            return FloorPos(b.transform.position);
        }
        Vector3 FloorPos(Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);//Vector3(v.x, 0f, v.z);
        }


        public void GoTo(Vector3 destination)
        {
            _navCR = StartCoroutine(Navigate(destination));
            currentSpeed = speed;
        }

        public void Stop()
        {
            if (_navCR != null) StopCoroutine(_navCR);
            _vel = Vector3.zero;
            currentSpeed = 0;
        }

        public void Stop(Ente ente, Waypoint wp, bool b)
        {
            if (_navCR != null) StopCoroutine(_navCR);
            _vel = Vector3.zero;
            currentSpeed = 0;
        }

        protected virtual IEnumerator Navigate(Vector3 destination)
        {
            var srcWp = Navigation.instance.NearestTo(transform.position);
            var dstWp = Navigation.instance.NearestTo(destination);

            _gizmoRealTarget = dstWp;
            Waypoint reachedDst = srcWp;

            if (srcWp != dstWp)
            {
                var path = _gizmoPath = AStar<Waypoint>.Run(
                    srcWp
                    , dstWp
                    , (wa, wb) => Vector3.Distance(wa.transform.position, wb.transform.position)
                    , w => w == dstWp
                    , w =>
                        //w.nearbyItems.Any(it => it.type == ItemType.Door)
                        //? null
                        //:
                        w.adyacent
                            //.Where(a => a.nearbyItems.All(it => it.type != ItemType.Door))
                            .Select(a => new AStar<Waypoint>.Arc(a, Vector3.Distance(a.transform.position, w.transform.position)))
                );
                if (path != null)
                {
                    // Debug.Log("COUNT" + path.Count());
                    foreach (var next in path.Select(w => FloorPos(w)))
                    {
                        

                        while ((next - FloorPos(this)).sqrMagnitude >= 0.05f)
                        {
                            _vel = (next - FloorPos(this)).normalized;
                            _root.LookAt(next);
                            yield return null;
                        }
                       
                    }
                }
                reachedDst = path.Last();
            }

            if (reachedDst == dstWp)
            {
                _vel = (FloorPos(destination) - FloorPos(this)).normalized;
                yield return new WaitUntil(() => (FloorPos(destination) - FloorPos(this)).sqrMagnitude < 0.05f);
            }

            _vel = Vector3.zero;
            OnReachDestination(this, reachedDst, reachedDst == dstWp);
        }


        #endregion

        void OnCollisionEnter(Collision col)
        {
            var item = col.collider.GetComponentInParent<Item>();

            if (item)
            {
                OnHitItem(this, item);
            }

        }


        private void OnTriggerStay(Collider other)
        {
            var item = other.GetComponentInParent<Item>();

            if (item)
            {
                OnStayItem(this, item);
            }
        }


        #region Gizmos

        
        void OnDrawGizmos()
        {
            if (_gizmoPath == null)
                return;

            Gizmos.color = Color.blue;
            var points = _gizmoPath.Select(w => FloorPos(w));
            Vector3 last = points.First();
            foreach (var p in points.Skip(1))
            {
                Gizmos.DrawLine(p + Vector3.up, last + Vector3.up);
                last = p;
            }
            if (_gizmoRealTarget != null)
                Gizmos.DrawCube(_gizmoRealTarget.transform.position + Vector3.up * 1f, Vector3.one * 0.3f);

        }

        #endregion

    }

}

