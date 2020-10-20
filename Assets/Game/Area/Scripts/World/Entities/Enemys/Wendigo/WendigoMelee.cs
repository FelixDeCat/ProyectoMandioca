using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoMelee : WendigoStates
    {
        public WendigoMelee(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;

        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            view.Kick();
            view.DebugText("MELEE");
        }

    }
}

