using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class JabaliIdle : JabaliStates
    {
        Func<bool> IsChargeOk;
        float distanceToPush;
        float distanceToNormalAttack;
        float maxDistance;
        GenericEnemyMove move;

        public JabaliIdle(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, GenericEnemyMove _move,float _distanceToPush, float _distanceToNormal,
                          float _maxDistance, Func<bool> _IsChargeOk) : base(myState, _sm)
        {
            move = _move;
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

                move.Rotation(forwardRotation);

                if (enemy.IsInPos())
                {

                    if (IsChargeOk())
                    {
                        if (Vector3.Distance(pos1, pos2) >= distanceToPush)
                            sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                        else
                            sm.SendInput(JabaliEnemy.JabaliInputs.CHASING);
                    }
                    else
                    {
                        if (Vector3.Distance(pos1, pos2) >= distanceToNormalAttack)
                            sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                        else
                            sm.SendInput(JabaliEnemy.JabaliInputs.CHASING);
                    }
                }
                else
                {
                    if (Vector3.Distance(pos1, pos2) >= maxDistance)
                        sm.SendInput(JabaliEnemy.JabaliInputs.PERSUIT);
                }
            }
        }
    }
}
