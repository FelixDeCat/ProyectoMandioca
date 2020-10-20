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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            base.Enter(last);
            view.DebugText("Prepare Melee");
            viewTime = 0f;

        }

        protected override void Exit(WendigoEnemy.WendigoInputs input)
        {
            base.Exit(input);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Update()
        {
            base.Update();
            viewTime += Time.deltaTime;
            if (viewTime > 2)
            {
                sm.SendInput(WendigoEnemy.WendigoInputs.MELEEAR);
            }
        }
    }
}
