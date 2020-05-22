using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class JabaliStates : StatesFunctions<JabaliEnemy.JabaliInputs>
    {
        protected EState<JabaliEnemy.JabaliInputs> lastState;
        protected Animator anim;
        protected Transform root;
        protected Rigidbody rb;
        protected CombatDirector combatDirector;
        protected EnemyBase enemy;

        public JabaliStates(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm) : base(myState, _sm)
        {

        }

        #region Builder
        public JabaliStates SetAnimator(Animator _anim) { anim = _anim; return this; }

        public JabaliStates SetRigidbody(Rigidbody _rb) { rb = _rb; return this; }

        public JabaliStates SetRoot(Transform _root) { root = _root; return this; }

        public JabaliStates SetDirector(CombatDirector _director) { combatDirector = _director; return this; }

        public JabaliStates SetThis(EnemyBase _enemy) { enemy = _enemy; return this; }

        #endregion

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            lastState = input;
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
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
