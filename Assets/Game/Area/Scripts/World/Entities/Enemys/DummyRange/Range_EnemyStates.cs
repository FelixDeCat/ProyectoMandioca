using UnityEngine;
namespace Tools.StateMachine
{
    public class Range_EnemyStates : StatesFunctions<RangeDummy.RangeDummyInput>
    {
        protected RangeDummy myDummy;

        public Range_EnemyStates(EState<RangeDummy.RangeDummyInput> myState, EventStateMachine<RangeDummy.RangeDummyInput> _sm, RangeDummy _myDummy) : base(myState, _sm) 
        {
            myDummy = _myDummy;
        }

        protected override void Enter(EState<RangeDummy.RangeDummyInput> lastState) { }
        protected override void Exit(RangeDummy.RangeDummyInput input) { }
        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Update() { }
    }
}
