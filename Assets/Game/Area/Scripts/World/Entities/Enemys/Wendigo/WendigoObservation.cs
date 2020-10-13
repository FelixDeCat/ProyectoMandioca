using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoObservation : WendigoStates
    {
        WendigoView view;
        public WendigoObservation(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            base.Enter(last);
        }

        protected override void Update()
        {
            view.Sign("Observation");
            base.Update();
        }
    }

}