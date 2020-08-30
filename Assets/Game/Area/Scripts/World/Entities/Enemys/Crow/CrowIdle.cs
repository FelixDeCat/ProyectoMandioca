using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CrowIdle : CrowStates
    {
        float distanceMin;
        float distanceMax;
        float rotationSpeed;
        ICombatDirector enemy;

        public CrowIdle(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, float _disInCom, float _rotationSpeed,
            ICombatDirector _enemy) : base(myState, _sm)
        {
            distanceMin = _disInCom;
            rotationSpeed = _rotationSpeed;
            enemy = _enemy;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);
        }

        protected override void Exit(CrowEnemy.CrowInputs input)
        {
            base.Exit(input);
        }

        protected override void Update()
        {
            base.Update();

            if (enemy.CurrentTarget() != null)
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);


                if (enemy.IsInPos())
                {
                    Vector3 pos1 = new Vector3(root.position.x, 0, root.position.z);
                    Vector3 pos2 = new Vector3(enemy.CurrentTarget().transform.position.x, 0, enemy.CurrentTarget().transform.position.z);

                    if (Vector3.Distance(pos1, pos2) >= distanceMin)
                        sm.SendInput(CrowEnemy.CrowInputs.CHASING);
                }
            }
        }
    }
}
