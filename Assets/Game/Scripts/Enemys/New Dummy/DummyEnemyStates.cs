using UnityEngine;
namespace ProyectTools.StateMachine
{
    public class DummyEnemyStates : StatesFunctions<TrueDummyEnemy.DummyEnemyInputs>
    {
        protected EState<TrueDummyEnemy.DummyEnemyInputs> lastState;
        protected Animator anim;
        protected Transform root;
        protected Rigidbody rb;
        protected CombatDirector combatDirector;

        public DummyEnemyStates(EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm) : base(myState, _sm) { }
        #region Builder
        public DummyEnemyStates SetAnimator(Animator _anim) { anim = _anim; return this; }
        public DummyEnemyStates SetRigidbody(Rigidbody _rb) { rb = _rb; return this; }
        public DummyEnemyStates SetRoot(Transform _root) { root = _root; return this; }
        public DummyEnemyStates SetDirector(CombatDirector _director) { combatDirector = _director; return this; }
        #endregion
        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> last) { lastState = last; }
        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input) { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Update() { }
    }
}
