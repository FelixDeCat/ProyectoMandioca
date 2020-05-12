using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class JabaliPersuit : JabaliStates
    {
        Func<Transform,bool> OnSight;
        Func<bool> IsChargeOk;
        float distanceNoCombat;
        float distanceAprox;
        GenericEnemyMove move;

        public JabaliPersuit(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, GenericEnemyMove _move, 
                             Func<Transform, bool> _OnSight, Func<bool> _IsChargeOk, float _distanceNormal, float _distanceToPush) : base(myState, _sm)
        {
            move = _move;
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
                    Vector3 forwardFix = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(forwardFix);
                    move.MoveWRigidbodyV(forwardFix);
                    if (Vector3.Distance(enemy.CurrentTarget().transform.position, root.position) <= distanceNoCombat)
                        sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                }
            }
            else
            {
                if (IsChargeOk())
                {

                    Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 forwardFix = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(forwardFix);
                    move.MoveWRigidbodyV(forwardFix);
                    if (Vector3.Distance(enemy.CurrentTarget().transform.position, root.position) <= distanceAprox && OnSight(enemy.CurrentTarget().transform))
                        sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                }
                else
                {
                    Vector3 dir = enemy.CurrentTargetPos() - root.position;
                    dir.Normalize();

                    Vector3 forwardFix = move.ObstacleAvoidance(new Vector3(dir.x, 0, dir.z));

                    move.Rotation(forwardFix);
                    move.MoveWRigidbodyV(forwardFix);

                    float distanceX = Mathf.Abs(enemy.CurrentTargetPos().x - root.position.x);
                    float distanceZ = Mathf.Abs(enemy.CurrentTargetPos().z - root.position.z);

                    if (distanceX < 0.7f && distanceZ < 0.7f)
                        sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
                }
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            base.Exit(input);
            anim.SetFloat("Speed", 0);
            move.StopMove();
        }
    }
}
