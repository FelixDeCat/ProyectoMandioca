using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Tools.StateMachine
{
    public class WendigoRange : WendigoStates
    {
        float throwTime;
        Action throwThing;
        public WendigoRange(EState<WendigoEnemy.WendigoInputs> myState, Action _thow, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
            throwThing = _thow;
        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            view.Throw();
            Debug.Log("pewpewpepw");
            view.DebugText("Ranged");
            throwTime = 0f;
        }
        protected override void Update()
        {
            throwTime += Time.deltaTime;
            if (throwTime > 1)
            {
                sm.SendInput(WendigoEnemy.WendigoInputs.OBSERVATION);
                throwThing();
                Debug.Log("OBSERVE");
            }
        }
    }

}