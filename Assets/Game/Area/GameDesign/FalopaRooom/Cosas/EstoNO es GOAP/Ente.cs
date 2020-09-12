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
        public event Action OnReachDestinationNoParameters = delegate { };
        public event Action<Ente, Item> OnHitItem = delegate { };
        public event Action<Ente, Item> OnStayItem = delegate { };
        public event Action OnFinishAttack = delegate { };
        public event Action OnMeleeAttack = delegate { };
        public event Action OnTakeDmg = delegate { };
        public event Action<Vector3> OnDeath = delegate { };

        [SerializeField] Transform _root;
        public Transform Root() => _root;
        CharacterHead character;

        public bool canSpeedBuff = true;
        public AttackSensor attackSensor;
        public DamageReceiver damagereciever;

        Animator _anim;
        public AnimEvent _animEvent;

        public Animator Anim ()=> _anim; 
        public AnimEvent AnimEvent ()=> _animEvent;


        [Header("Feedback")]
        [SerializeField] ParticleSystem takeDamage_fb;

        void FixedUpdate()
        {
            transform.Translate(Time.fixedDeltaTime * _vel * currentSpeed);
        }

        private void Start()
        {
            
            damagereciever.Initialize(_root, IsDamaged, OnDeath, TakeDamageFeedback, GetComponent<Rigidbody>(), GetComponent<EnemyLifeSystem>());

            _anim = GetComponentInChildren<Animator>();
            //_animEvent = GetComponentInChildren<AnimEvent>();

            _animEvent.Add_Callback("meleeAttack", OnMeleeAttackHit);
            _animEvent.Add_Callback("finishAttack", OnFinishMeleeAttackAnimation);

            //prendo y apago el sensor cuando la animacion lo pide
            OnMeleeAttack += () => attackSensor.gameObject.SetActive(true);
            OnFinishAttack += () => attackSensor.gameObject.SetActive(false);

            OnReachDestination += Stop;

            GetComponent<Dude>().Initialize();
        }

        void OnMeleeAttackHit() => OnMeleeAttack?.Invoke();  
        void OnFinishMeleeAttackAnimation() => OnFinishAttack?.Invoke();


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

        void TakeDamageFeedback(DamageData dData)
        {
            _anim.SetTrigger("takeDamage");
            takeDamage_fb.Play();
            
        }

        public bool IsDamaged()
        {
            
            return false;
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
                        

                        while ((next - FloorPos(this)).sqrMagnitude >= 1f)//0.05f)
                        {
                            _vel = (next - FloorPos(this)).normalized;
                            _root.LookAt(next);
                            yield return null;
                        }
                       
                    }
                }
                reachedDst = path.Last();
                _root.LookAt(reachedDst.transform.position);
            }

            if (reachedDst == dstWp)
            {
                _vel = (FloorPos(destination) - FloorPos(this)).normalized;
                yield return new WaitUntil(() => (FloorPos(destination) - FloorPos(this)).sqrMagnitude < 1.5f);//0.05f);
            }

            _vel = Vector3.zero;
            OnReachDestination?.Invoke(this, reachedDst, reachedDst == dstWp);
            OnReachDestinationNoParameters?.Invoke();
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

