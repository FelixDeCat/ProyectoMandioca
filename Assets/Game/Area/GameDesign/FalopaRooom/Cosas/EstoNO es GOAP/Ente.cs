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
        public event Action OnMeleeRangeWithPlayer = delegate { };
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
        GenericLifeSystem _lifeSystem;
        public GenericLifeSystem Life => _lifeSystem;

        Animator _anim;
        public AnimEvent _animEvent;

        public Animator Anim() => _anim;
        public AnimEvent AnimEvent() => _animEvent;

        public CaronteSkill_Manager skillManager;

        public Sensor meleeRange_sensor;
        Rigidbody _rb;

        [Header("Movement")]
        public Vector3 _dir;
        public Vector3 dest_pos;
        public float rootSpeed;
        public float speed = 2f;
        float currentSpeed;

        [Header("Feedback")]
        [SerializeField] ParticleSystem takeDamage_fb;

        void FixedUpdate()
        {
            if(dest_pos != Vector3.zero)
            {
                _rb.velocity += _dir * currentSpeed * Time.fixedDeltaTime;
                Rotation(_dir);
            }
        }

        public void Rotation(Vector3 forward)
        {
            _root.forward = Vector3.Lerp(_root.forward, forward, rootSpeed * Time.deltaTime);
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _lifeSystem = GetComponent<GenericLifeSystem>();
            meleeRange_sensor.AddCallback_OnTriggerEnter(OnPlayerInMeleeRange);
            damagereciever.SetIsDamage(IsDamaged).AddDead(OnDeath).AddTakeDamage(TakeDamageFeedback).Initialize(_root, _rb, _lifeSystem);

            _anim = GetComponentInChildren<Animator>();
            //_animEvent = GetComponentInChildren<AnimEvent>();

            _animEvent.Add_Callback("meleeAttack", OnMeleeAttackHit);
            _animEvent.Add_Callback("finishAttack", OnFinishMeleeAttackAnimation);

            //prendo y apago el sensor cuando la animacion lo pide
            OnMeleeAttack += () => attackSensor.gameObject.SetActive(true);
            OnFinishAttack += () => attackSensor.gameObject.SetActive(false);

            OnReachDestination += Stop;

        }

        public void Initialize()
        {
            GetComponent<Dude>().Initialize();
        }

        void OnPlayerInMeleeRange(GameObject go) { OnMeleeRangeWithPlayer?.Invoke(); }
        void OnMeleeAttackHit() => OnMeleeAttack?.Invoke();
        void OnFinishMeleeAttackAnimation() => OnFinishAttack?.Invoke();


        private void Update()
        {
            _anim.SetFloat("speed", currentSpeed);
        }

        #region Health

        void TakeDamageFeedback(DamageData dData)
        {
            _anim.SetTrigger("takeDamage");
            takeDamage_fb.Play();
        }

        public bool IsDamaged(){return false;}

        #endregion

        #region Momevent

        Waypoint _gizmoRealTarget;
        IEnumerable<Waypoint> _gizmoPath;
        Coroutine _navCR;

        public void GoTo(Vector3 destination)
        {
            _navCR = StartCoroutine(Navigate(destination));
            currentSpeed = speed;
        }

        public void Stop()
        {
            if (_navCR != null) StopCoroutine(_navCR);
            dest_pos = Vector3.zero;
            currentSpeed = 0;
        }

        public void Stop(Ente ente, Waypoint wp, bool b)
        {
            if (_navCR != null) StopCoroutine(_navCR);
            dest_pos = Vector3.zero;
            currentSpeed = 0;
        }

        protected virtual IEnumerator Navigate(Vector3 destination)
        {
            
            nodeDebug = destination;
            
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
                    foreach (var w in path)
                    {
                        dest_pos = w.transform.position;
                        _dir = dest_pos - _root.position;
                        _dir.Normalize();


                        while ((dest_pos - _root.position).sqrMagnitude >= 2f)
                        {
                            yield return null;
                        }

                    }
                }
                reachedDst = path.Last();
            }

            if (reachedDst == dstWp)
            {
                dest_pos = destination;
                _dir = dest_pos - _root.position;
                _dir.Normalize();

                yield return new WaitUntil(() => (dest_pos - _root.position).sqrMagnitude < 2f);
            }

            dest_pos = Vector3.zero;
            OnReachDestination?.Invoke(this, reachedDst, reachedDst == dstWp);
            OnReachDestinationNoParameters?.Invoke();
        }


        #endregion

        void OnTriggerEnter(Collider col)
        {
            var item = col.GetComponentInParent<Item>();

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

        Vector3 nodeDebug;
        void OnDrawGizmos()
        {
            if (dest_pos == Vector3.zero) return;

            //Gizmos.DrawSphere(_root.position, 1);
            //izmos.DrawSphere(nodeDebug, 1);

            

            Gizmos.color = Color.red;           
            Gizmos.DrawLine(_root.position, dest_pos);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(dest_pos, 0.5f);

            if (_gizmoPath == null)
                return;


            // Gizmos.color = Color.blue;
            //var points = _gizmoPath.Select(w => FloorPos(w));
            //Vector3 last = points.First();
            //foreach (var p in points.Skip(1))
            //{
            //    Gizmos.DrawLine(p + Vector3.up, last + Vector3.up);
            //    last = p;
            //}
            //if (_gizmoRealTarget != null)
            //    Gizmos.DrawCube(_gizmoRealTarget.transform.position + Vector3.up * 1f, Vector3.one * 0.3f);

        }


        #endregion

    }

}

