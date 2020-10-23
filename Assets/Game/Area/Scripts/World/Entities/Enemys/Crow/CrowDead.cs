using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CrowDead : CrowStates
    {
        RagdollComponent ragdoll;
        Action OnDissappear;

        public CrowDead(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, RagdollComponent _ragdoll, Action _OnDissappear) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
            OnDissappear = _OnDissappear;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);
            cdModule.AddCD("ragdollDesactive", ragdoll.DesactiveBones, 5);
            cdModule.AddCD("ragdollDissapear", OnDissappear, 8);
            combatDirector.DeadEntity(ragdoll.GetComponent<CombatDirectorElement>(), ragdoll.GetComponent<CombatDirectorElement>().CurrentTarget());
        }
    }
}
