using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Tools.StateMachine
{
    public class WendigoRange : WendigoStates
    {
        Action rotation;
        public WendigoRange(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, Action _rot, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
            rotation = _rot;
        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            view.Throw();
            view.DebugText("Ranged");
        }
        protected override void Update()
        {
            rotation();
        }
    }

}