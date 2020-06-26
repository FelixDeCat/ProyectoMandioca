using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class BMinionStates : StatesFunctions<BasicMinion.BasicMinionInput>
    {
        protected EState<BasicMinion.BasicMinionInput> lastState;
        protected Animator anim;
        protected Transform root;
        protected Rigidbody rb;
        protected CombatDirector combatDirector;
        protected Minion minion;

        public BMinionStates(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm) : base(myState, _sm)
        {
        }

        #region Builder
        public BMinionStates SetAnimator(Animator _anim) { anim = _anim; return this; }
        public BMinionStates SetRigidbody(Rigidbody _rb) { rb = _rb; return this; }
        public BMinionStates SetRoot(Transform _root) { root = _root; return this; }
        public BMinionStates SetDirector(CombatDirector _director) { combatDirector = _director; return this; }
        public BMinionStates SetThis(Minion me) { minion = me; return this; }
        #endregion

        protected override void Enter(EState<BasicMinion.BasicMinionInput> _lastState)
        {
            lastState = _lastState;
        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
        }

        protected override void FixedUpdate()
        {
        }

        protected override void LateUpdate()
        {
        }

        protected override void Update()
        {
        }
    }
}
