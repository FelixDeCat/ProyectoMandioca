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
        Animator anim;
        float timer;
        bool enter;

        public CharReleaseAttack(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall,
                                 Func<bool> _isHeavy, Action<bool> _ChangeHeavy, Animator _anim) : base(myState, _sm)
        {
            attackRecall = recall;
            IsHeavy = _isHeavy;
            ChangeHeavy = _ChangeHeavy;
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
        }

        protected override void Update()
        {
            if (!IsHeavy())
            {

                charMove.RotateHorizontal(RightHorizontal());
                charMove.RotateVertical(RightVertical());
                charMove.RotateHorizontal(LeftHorizontal());
                charMove.RotateVertical(LeftVertical());
                charMove.MovementHorizontal(0);
                charMove.MovementVertical(0);

                //charMove.MovementHorizontal(0);
                //charMove.MovementVertical(0);
                //charMove.RotateHorizontal(0);
                //charMove.RotateVertical(0);
                //charMove.MovementHorizontal(LeftHorizontal());
                //charMove.MovementVertical(LeftVertical());
                //charMove.RotateHorizontal(RightHorizontal());
                //charMove.RotateVertical(RightVertical());
            }

            var info = anim.GetCurrentAnimatorStateInfo(2);

            if (info.IsName("CentralState"))
            {
                if (LeftHorizontal() == 0 && LeftVertical() == 0)
                {
                    sm.SendInput(CharacterHead.PlayerInputs.IDLE);
                }
                else
                {
                    sm.SendInput(CharacterHead.PlayerInputs.MOVE);
                }
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
        }
    }
}
