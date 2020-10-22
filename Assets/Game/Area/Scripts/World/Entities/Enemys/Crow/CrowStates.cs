using UnityEngine;
namespace Tools.StateMachine
{
    public class CrowStates : StatesFunctions<CrowEnemy.CrowInputs>
    {
        protected EState<CrowEnemy.CrowInputs> lastState;
        protected Animator anim;
        protected Transform root;
        protected Rigidbody rb;
        protected CombatDirector combatDirector;
        protected CDModule cdModule;

        public CrowStates(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm) : base(myState, _sm) { }
        #region Builder
        public CrowStates SetAnimator(Animator _anim) { anim = _anim; return this; }
        public CrowStates SetRigidbody(Rigidbody _rb) { rb = _rb; return this; }
        public CrowStates SetRoot(Transform _root) { root = _root; return this; }
        public CrowStates SetDirector(CombatDirector _director) { combatDirector = _director; return this; }
        public CrowStates SetCD(CDModule _cd) { cdModule = _cd; return this; }
        #endregion
        protected override void Enter(EState<CrowEnemy.CrowInputs> last) { lastState = last; }
        protected override void Exit(CrowEnemy.CrowInputs input) { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Update() { }
    }
}
