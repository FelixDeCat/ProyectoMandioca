using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class BBeginAttack : BMinionStates
    {
        float cd;
        float timer;
        GenericEnemyMove move;
        public BBeginAttack(EState<BasicMinion.BasicMinionInput> myState, EventStateMachine<BasicMinion.BasicMinionInput> _sm,
                            GenericEnemyMove _move) : base(myState, _sm)
        {
            cd = 5;
            move = _move;
        }

        protected override void Enter(EState<BasicMinion.BasicMinionInput> lastState)
        {
            base.Enter(lastState);
            if (lastState.Name != "Stun")
            {
                anim.SetBool("Attack", true);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (minion.CurrentTarget() != null)
            {
                Vector3 myForward = (minion.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

                move.Rotation(forwardRotation);
            }

            timer += Time.deltaTime;

            if (timer >= cd)
                sm.SendInput(BasicMinion.BasicMinionInput.ATTACK);
        }

        protected override void Exit(BasicMinion.BasicMinionInput input)
        {
            base.Exit(input);

            if (input != BasicMinion.BasicMinionInput.STUN)
                timer = 0;
        }
    }
}
