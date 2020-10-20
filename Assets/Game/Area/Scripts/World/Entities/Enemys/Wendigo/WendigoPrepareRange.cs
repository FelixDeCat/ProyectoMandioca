using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{

    public class WendigoPrepareRange : WendigoStates
    {

        float viewTime;
        public WendigoPrepareRange(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            viewTime = 0f;
            view.DebugText("PrepareRange");
        }
        protected override void Update()
        {

            viewTime += Time.deltaTime;
            if (viewTime > 2)
            {
                sm.SendInput(WendigoEnemy.WendigoInputs.RANGEAR);
            }
        }
    }

}