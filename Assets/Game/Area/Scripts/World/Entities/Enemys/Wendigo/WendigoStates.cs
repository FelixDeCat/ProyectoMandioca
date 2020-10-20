using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoStates : StatesFunctions<WendigoEnemy.WendigoInputs>
    {
        protected EState<WendigoEnemy.WendigoInputs> lastState;
        protected WendigoView view;
        protected Transform root;
        protected Rigidbody rb;
        protected CombatDirector combatDirector;

        public WendigoStates(EState<WendigoEnemy.WendigoInputs> myState, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm) { }
        #region Builder
        public WendigoStates SetView(WendigoView _view) { view = _view; return this; }
        public WendigoStates SetRigidbody(Rigidbody _rb) { rb = _rb; return this; }
        public WendigoStates SetRoot(Transform _root) { root = _root; return this; }
        public WendigoStates SetDirector(CombatDirector _director) { combatDirector = _director; return this; }
        #endregion
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last) { lastState = last; }
        protected override void Exit(WendigoEnemy.WendigoInputs input) { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Update() { }

    }

}