using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    

    public class CharRoll : CharacterStates
    {
        ParticleSystem evadepart;

        public CharRoll(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, ParticleSystem _evadepart) : base(myState, _sm)
        {
            evadepart = _evadepart;
        }

        string _input;

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {

            
            evadepart.Play();

            _input = input.Name;

            //if (input.Name == "Begin_Block" || input.Name == "Block")
            //{
            //    Debug.Log("ANIMACION COMPLETA DE DASH CON ESCUDO");
            //    charBlock.UpBlock();
            //}

            charMove.Dash();
        }

        protected override void Update()
        {
            base.Update();
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
            evadepart.Stop();

            if (_input == "Begin_Block" || _input == "Block")
            {
                charBlock.callback_UpBlock();
                charBlock.SetOnBlock(false);
            }

            base.Exit(input);
        }
    }
}
