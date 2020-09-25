using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CrowChasing : CrowStates
    {
        Func<bool> IsAttack;
        Func<Transform, bool> LineOfSight;
        float distanceToNormalAttack;
        float rotationSpeed;
        CombatDirectorElement enemy;

        public CrowChasing(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, Func<bool> _IsAttack,
                            float _distanceToAttack, float _rotationSpeed, CombatDirectorElement _enemy, Func<Transform, bool> _LineOfSight) : base(myState, _sm)
        {
            IsAttack = _IsAttack;
            distanceToNormalAttack = _distanceToAttack;
            rotationSpeed = _rotationSpeed;
            enemy = _enemy;
            LineOfSight = _LineOfSight;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);
            combatDirector.PrepareToAttack(enemy, enemy.CurrentTarget());
        }

        protected override void Exit(CrowEnemy.CrowInputs input)
        {
            base.Exit(input);

            if (enemy.CurrentTarget() != null)
                combatDirector.DeleteToPrepare(enemy, enemy.CurrentTarget());
        }

        protected override void Update()
        {
            base.Update();
            if (!enemy.CurrentTarget())
                sm.SendInput(CrowEnemy.CrowInputs.IDLE);

            if (IsAttack())
                sm.SendInput(CrowEnemy.CrowInputs.BEGIN_ATTACK);
            else
            {
                if (enemy.CurrentTarget() != null)
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                    Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);
                    root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);

                    if (Vector3.Distance(pos1, pos2) >= distanceToNormalAttack || !LineOfSight(enemy.CurrentTarget().transform))
                        sm.SendInput(CrowEnemy.CrowInputs.IDLE);
                }
            }
        }
    }
}
