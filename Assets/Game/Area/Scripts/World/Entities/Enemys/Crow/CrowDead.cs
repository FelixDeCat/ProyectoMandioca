using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CrowDead : CrowStates
    {
        RagdollComponent ragdoll;
        float timer;
        bool desactive;

        public CrowDead(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, RagdollComponent _ragdoll) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);
            combatDirector.DeadEntity(ragdoll.GetComponent<EnemyBase>(), ragdoll.GetComponent<EnemyBase>().CurrentTarget());
        }

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;

            if (timer >= 5)
            {
                if (!desactive)
                {
                    ragdoll.DesactiveBones();
                    desactive = true;
                }
            }

            if (timer >= 8)
                ragdoll.gameObject.SetActive(false);
        }
    }
}
