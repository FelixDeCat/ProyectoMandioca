using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharBashDash : CharacterStates
    {
        Action<bool> ChangeAttacking;
        CharacterAnimator anim;
        ParticleSystem trail;

        public CharBashDash(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Action<bool> _ChangeAttack, CharacterAnimator _anim, ParticleSystem _trail) : base(myState, _sm)
        {
            ChangeAttacking = _ChangeAttack;
            anim = _anim;
            trail = _trail;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.StartBashDash();
            trail.gameObject.SetActive(true);
        }

        protected override void Update()
        {
            if (charAttack.ExecuteBashDash())
                charMove.StopBashDash();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            base.Exit(input);
            trail.gameObject.SetActive(false);
            ChangeAttacking?.Invoke(false);
            charBlock.SetOnBlock(false);
            anim.Block(false);
        }
    }
}
