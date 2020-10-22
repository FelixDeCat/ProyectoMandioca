using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        RagdollComponent ragdoll;
        string deadSound;
        Action OnDissappear;

        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, RagdollComponent _ragdoll,
            string _deadSound, Action _OnDissappear) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
            deadSound = _deadSound;
            OnDissappear = _OnDissappear;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            combatDirector.DeadEntity(enemy, enemy.CurrentTarget());
            AudioManager.instance.PlaySound(deadSound);
            cdModule.AddCD("RagdollDissappear", ragdoll.DesactiveBones, 5);
            cdModule.AddCD("ReturnToPool", OnDissappear, 8);
        }
    }
}