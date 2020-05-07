using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class CharTakeDmg : CharacterStates
    {
        float timer;
        float takeDamageRecall;

        public CharTakeDmg(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall) : base(myState, _sm)
        {
            takeDamageRecall = recall;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            //Tambien podría haber una rica animación, why not
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= takeDamageRecall)
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            timer = 0;
        }
    }
}
