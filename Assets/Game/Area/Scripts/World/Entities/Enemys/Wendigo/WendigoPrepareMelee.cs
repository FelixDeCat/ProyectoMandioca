using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoPrepareMelee : WendigoStates
    {
        float viewTime;

        public WendigoPrepareMelee(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            base.Enter(last);
            view.DebugText("Prepare Melee");
            viewTime = 0f;

        }

        protected override void Update()
        {
            base.Update();
            viewTime += Time.deltaTime;
            if (viewTime > 1f)
            {
                sm.SendInput(WendigoEnemy.WendigoInputs.MELEEAR);
            }
        }
    }
}
