using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Tools.StateMachine
{
    public class WendigoRange : WendigoStates
    {
        public WendigoRange(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            view.Throw();
            Debug.Log("pewpewpepw");
            view.DebugText("Ranged");
        }
        protected override void Update()
        {

        }
    }

}