using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoIdle : WendigoStates
    {
        public WendigoIdle(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            base.Enter(last);

        }

        protected override void Update()
        {
            view.DebugText("Idle");
            base.Update();
        }

    }


}