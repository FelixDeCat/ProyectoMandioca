using UnityEngine;
using System;

namespace ProyectTools.StateMachine
{
    public class JabaliIdle : JabaliStates
    {
        Func<bool> IsAttack;
        Func<bool> IsChargeOk;
        float distanceToPush;
        float distanceToNormalAttack;
        float maxDistance;

        public JabaliIdle(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _distanceToPush, float _distanceToNormal,
                          float _maxDistance, Func<bool> _IsAttack, Func<bool> _IsChargeOk) : base(myState, _sm)
        {
            IsAttack = _IsAttack;
            IsChargeOk = _IsChargeOk;
            distanceToPush = _distanceToPush;
            distanceToNormalAttack = _distanceToNormal;
            maxDistance = _maxDistance;
        }

        protected override void Update()
        {

            if (enemy.CurrentTarget() != null)
            {
                Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                Root(forwardRotation);

                if (enemy.IsInPos())
                {

                    if (IsChargeOk())
                    {
                        if (Vector3.Distance(pos1, pos2) >= distanceToPush)
                        {
                            combatDirector.GetNewNearPos(enemy);
                            sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                        }
                    }
                    else
                    {
                        Vector3 pos3 = new Vector3(enemy.CurrentTargetPos().position.x, 0, enemy.CurrentTargetPos().position.z);

                        if (Vector3.Distance(pos1, pos2) >= distanceToNormalAttack && Vector3.Distance(pos1, pos3) >= 1)
                        {
                            combatDirector.GetNewNearPos(enemy);
                            sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                        }
                    }
                }
                else
                {
                    if (Vector3.Distance(pos1, pos2) >= maxDistance)
                        sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                }

                if (IsAttack())
                {
                    if (IsChargeOk())
                        sm.SendInput(JabaliEnemy.JabaliInputs.CHARGE_PUSH);
                    else
                        sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ANTICIP);
                }
            }
        }
    }
}
