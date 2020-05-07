namespace ProyectTools.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetBool("Dead", true);
        }
    }
}