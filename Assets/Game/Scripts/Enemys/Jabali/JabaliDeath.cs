using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        RagdollComponent ragdoll;
        float timer;
        bool desactive;

        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, RagdollComponent _ragdoll) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            ragdoll.Ragdoll(true);
        }

        protected override void Update()
        {
            base.Update();
            timer += Time.deltaTime;

            if (timer >= 5)
            {
                if (!desactive)
                {
                    ragdoll.DesactiveBones();
                    desactive = true;
                }
            }

            if (timer >= 8)
            {
                ragdoll.gameObject.SetActive(false);
            }
        }
    }
}