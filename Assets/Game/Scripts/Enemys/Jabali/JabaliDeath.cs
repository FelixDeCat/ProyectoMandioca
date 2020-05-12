namespace ToolsMandioca.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        RagdollComponent ragdoll;
        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, RagdollComponent _ragdoll) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            ragdoll.Ragdoll(true);
        }
    }
}