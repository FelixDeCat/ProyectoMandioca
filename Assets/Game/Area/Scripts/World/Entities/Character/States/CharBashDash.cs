using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools.StateMachine
{
    public class CharBashDash : CharacterStates
    {

        public CharBashDash(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.StartBashDash();
        }

        protected override void Update()
        {
            if (charAttack.ExecuteBashDash())
                charMove.StopBashDash();
        }
    }
}
