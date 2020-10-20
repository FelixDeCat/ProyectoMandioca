using System;
namespace Tools.StateMachine
{
    public class WendigoMelee : WendigoStates
    {
        Action rotation;
        public WendigoMelee(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, Action _rot, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
            rotation = _rot;
        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            view.Kick();
            view.DebugText("MELEE");
        }
        protected override void Update()
        {
            rotation();
        }

    }
}

