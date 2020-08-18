using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{

    public class CharDead : CharacterStates
    {
        public CharDead(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charAnim.Dead();
        }

        protected override void Update()
        {
            //Acá se puede hacer que después de cierto tiempo te tire la pantalla de muerte con eventos o algo así.
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
            base.Exit(input);
        }
    }
}
