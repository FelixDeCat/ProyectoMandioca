using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class JabaliDeath : JabaliStates
    {
        RagdollComponent ragdoll;
        float timer;
        bool desactive;
        string deadSound;

        public JabaliDeath(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, RagdollComponent _ragdoll, string _deadSound) : base(myState, _sm)
        {
            ragdoll = _ragdoll;
            deadSound = _deadSound;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            combatDirector.DeadEntity(enemy, enemy.CurrentTarget());
            AudioManager.instance.PlaySound(deadSound);
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