using System;
namespace Tools.StateMachine
{
    public class WendigoGrabRock : WendigoStates
    {
        //Aca deberia hacer el scan de los que tiene cerca, ir a buscarlo, agarrarlo como el old
        //Y pasar de nuevo a observacion 
        ThrowRock throwable;
        Action owo;

        public WendigoGrabRock(EState<WendigoEnemy.WendigoInputs> myState, Action checkRock, WendigoView _view, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
            owo = checkRock;
        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            //overlap de buscar una piedra y hacerle un set a throwable
            view.DebugText("GrabbingThing");
            view.GrabRock();
            owo();
            sm.SendInput(WendigoEnemy.WendigoInputs.OBSERVATION);
        }
        protected override void Update()
        {

            /*
            if (throwable)
            {
                //lavasabuscar y cuando estas cerca la agarras
            }
            else
            {
                sm.SendInput(WendigoEnemy.WendigoInputs.OBSERVATION);
            }
*/
        }
    }

}