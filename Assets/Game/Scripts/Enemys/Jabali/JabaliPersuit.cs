using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class JabaliPersuit : JabaliStates
    {
        Func<Transform,bool> OnSight;
        Func<bool> IsChargeOk;
        float distanceNoCombat;
        float distanceAprox;

        public JabaliPersuit(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, Func<Transform, bool> _OnSight,
                             Func<bool> _IsChargeOk, float _distanceNormal, float _distanceToPush) : base(myState, _sm)
        {
            OnSight = _OnSight;
            IsChargeOk = _IsChargeOk;
            distanceNoCombat = _distanceNormal;
            distanceAprox = _distanceToPush;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);
            anim.SetFloat("Speed", 0.3f);
        }

        protected override void Update()
        {
            if (enemy.CurrentTargetPos() == null)
            {
                if (enemy.CurrentTarget() != null)
                {
                    Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = new Vector3(dirForward.x, 0, dirForward.z);

                    Root(Move(fowardRotation));
                    if (Vector3.Distance(enemy.CurrentTarget().transform.position, root.position) <= distanceNoCombat)
                        sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                }
            }
            else
            {
                if (IsChargeOk())
                {

                    Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = new Vector3(dirForward.x, 0, dirForward.z);

                    Root(Move(fowardRotation));

                    if (Vector3.Distance(enemy.CurrentTarget().transform.position, root.position) <= distanceAprox && OnSight(enemy.CurrentTarget().transform))
                        sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                }
                else
                {
                    Vector3 dir = enemy.CurrentTargetPos().position - root.position;
                    dir.Normalize();

                    Vector3 dirFix = new Vector3(dir.x, 0, dir.z);

                    Root(Move(dirFix));

                    float distanceX = Mathf.Abs(enemy.CurrentTargetPos().transform.position.x - root.position.x);
                    float distanceZ = Mathf.Abs(enemy.CurrentTargetPos().transform.position.z - root.position.z);

                    if (distanceX < 0.7f && distanceZ < 0.7f)
                        sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                }
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            base.Exit(input);
            anim.SetFloat("Speed", 0);
            StopMove();
        }
    }
}
