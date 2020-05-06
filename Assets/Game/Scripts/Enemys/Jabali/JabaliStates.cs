using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class JabaliStates : StatesFunctions<JabaliEnemy.JabaliInputs>
    {
        protected EState<JabaliEnemy.JabaliInputs> lastState;
        protected Animator anim;
        protected Transform root;
        protected Rigidbody rb;
        protected CombatDirector combatDirector;
        protected EnemyBase enemy;

        public JabaliStates(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm) : base(myState, _sm)
        {

        }

        #region Builder
        float avoidRadious;
        float avoidWeight;
        Func<float> CurrentSpeed;
        LayerMask myAvoidMask;

        public JabaliStates SetMovement(Rigidbody _rb, EnemyBase _me,float _avoidRadoius, float _avoidWeight, Func<float> _speed, LayerMask _avoidMask)
        {
            rb = _rb;
            enemy = _me;
            avoidRadious = _avoidRadoius;
            avoidWeight = _avoidWeight;
            CurrentSpeed = _speed;
            myAvoidMask = _avoidMask;

            return this;
        }

        float rotSpeed;
        public JabaliStates SetRotation(Transform _root, float _rotSpeed)
        {
            root = _root;
            rotSpeed = _rotSpeed;

            return this;
        }

        public JabaliStates SetAnimator(Animator _anim) { anim = _anim; return this; }

        public JabaliStates SetRigidbody(Rigidbody _rb) { rb = _rb; return this; }

        public JabaliStates SetRoot(Transform _root) { root = _root; return this; }

        public JabaliStates SetDirector(CombatDirector _director) { combatDirector = _director; return this; }

        public JabaliStates SetThis(EnemyBase _enemy) { enemy = _enemy; return this; }

        #endregion

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            lastState = input;
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {

        }

        protected override void FixedUpdate()
        {

        }

        protected override void LateUpdate()
        {

        }

        protected override void Update()
        {

        }

        protected void StopMove()
        {
            rb.velocity = Vector3.zero;
        }

        protected virtual Vector3 Move(Vector3 dir)
        {
            return ObstacleAvoidance(dir);
        }

        Transform obs;
        protected Vector3 ObstacleAvoidance(Vector3 dir)
        {
            obs = null;
            var friends = Physics.OverlapSphere(root.position, avoidRadious, myAvoidMask);
            if (friends.Length > 0)
            {
                foreach (var item in friends)
                {
                    if (item.GetComponent<EnemyBase>() != enemy)
                    {
                        if (!obs)
                            obs = item.transform;
                        else if (Vector3.Distance(item.transform.position, root.position) < Vector3.Distance(obs.position, root.position))
                            obs = item.transform;
                    }
                }
            }

            if (obs)
            {
                Vector3 diraux = (root.position - obs.position).normalized;
                //diraux

                diraux = new Vector3(diraux.x, 0, diraux.z);

                dir += diraux * avoidWeight;
            }

            rb.velocity = new Vector3(dir.x * CurrentSpeed(), rb.velocity.y, dir.z * CurrentSpeed());

            return new Vector3(dir.normalized.x, 0, dir.normalized.z);
        }

        protected virtual void Root(Vector3 _forward)
        {
            root.forward = Vector3.Lerp(root.forward, _forward, rotSpeed * Time.deltaTime);
        }
    }
}
