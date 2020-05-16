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
        float minDistance;
        GenericEnemyMove move;

        public JabaliPersuit(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, GenericEnemyMove _move, 
                             Func<Transform, bool> _OnSight, Func<bool> _IsChargeOk, float _distanceNormal, float _minDistance,float _distanceToPush) : base(myState, _sm)
        {
            move = _move;
            OnSight = _OnSight;
            IsChargeOk = _IsChargeOk;
            minDistance = _minDistance;
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
            if (!enemy.IsInPos())
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
                else
                    sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);
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
                    Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                    move.Rotation(fowardRotation);
                    move.MoveWRigidbodyV(fowardRotation);

                    Vector3 targetPos = enemy.CurrentTarget().transform.position;
                    Vector3 myPos = root.position;

                    if (Vector3.Distance(new Vector3(targetPos.x, 0, targetPos.z), new Vector3(myPos.x, 0, myPos.z)) <= minDistance)
                        sm.SendInput(JabaliEnemy.JabaliInputs.CHASING);
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
