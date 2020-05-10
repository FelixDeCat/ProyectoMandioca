using UnityEngine;
namespace ToolsMandioca.StateMachine
{
    public class Boss_StatesBase : StatesFunctions<ABossGeneric.RangeDummyInput>
    {
        protected ABossGeneric myDummy;

        public Boss_StatesBase(EState<ABossGeneric.RangeDummyInput> myState, EventStateMachine<ABossGeneric.RangeDummyInput> _sm, ABossGeneric _myDummy) : base(myState, _sm) 
        {
            myDummy = _myDummy;
        }

        protected override void Enter(EState<ABossGeneric.RangeDummyInput> lastState) { }
        protected override void Exit(ABossGeneric.RangeDummyInput input) { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Update() { }
    }
}
