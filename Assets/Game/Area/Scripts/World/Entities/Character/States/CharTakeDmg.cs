using System;
using UnityEngine;

namespace Tools.StateMachine
{
    public class CharTakeDmg : CharacterStates
    {
        float timer;
        CharacterAnimator anim;
        Func<float> TakeDamageRecall;
        float takeDamageRecall;

        public CharTakeDmg(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, Func<float> _recall, CharacterAnimator _anim) : base(myState, _sm)
        {
            TakeDamageRecall = _recall;
            anim = _anim;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            //Tambien podría haber una rica animación, why not
            anim.OnHit();
            takeDamageRecall = TakeDamageRecall();
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
