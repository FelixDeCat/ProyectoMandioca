using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CharReleaseAttack : CharacterStates
    {
        float attackRecall;
        Func<bool> IsHeavy;
        Action<bool> ChangeHeavy;
        Action<bool> ChangeAttacking;
        float timer;
        bool enter;
        CharacterAnimator anim;

        public CharReleaseAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall,
                                 Func<bool> _isHeavy, Action<bool> _ChangeHeavy, Action<bool> _ChangeAttacking, CharacterAnimator _anim) : base(myState, _sm)
        {
            attackRecall = recall;
            IsHeavy = _isHeavy;
            ChangeHeavy = _ChangeHeavy;
            ChangeAttacking = _ChangeAttacking;
            anim = _anim;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            if (IsHeavy())
            {
                charMove.MovementHorizontal(0);
                charMove.MovementVertical(0);
                feedbacks.particles.feedbackDashHeavy.Play();
            }
            enter = true;
            anim.ForceAttack(false);
        }

        protected override void Update()
        {
            if (!IsHeavy())
            {
                charMove.RotateHorizontal(LeftHorizontal());
                charMove.RotateVertical(LeftVertical());
            }

            if (enter)
            {
                if (timer < attackRecall)
                {
                    timer = timer + 1 * Time.deltaTime;
                }
                else
                {
                    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
                    enter = false;
                    timer = 0;
                }
            }
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
            enter = true;
            timer = 0;
            ChangeHeavy(false);
            feedbacks.particles.feedbackDashHeavy.Stop();

            if (input != CharacterHead.PlayerInputs.CHARGE_ATTACK)
                ChangeAttacking?.Invoke(false);
        }
    }
}
