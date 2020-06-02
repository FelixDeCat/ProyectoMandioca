using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class CharParry : CharacterStates
    {
        float timer;
        float parryRecall;


        public CharParry(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float recall, AudioClip parrySound) : base(myState, _sm)
        {
            parryRecall = recall;
            AudioManager.instance.GetSoundPool("parrySound", AudioGroups.GAME_FX, parrySound);
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
            charBlock.SetOnBlock(false);
            charBlock.OnParry();
            AudioManager.instance.PlaySound("parrySound");
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= parryRecall)
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
        }

        protected override void FixedUpdate()
        {

        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            timer = 0;
            charBlock.UpBlock();
        }
    }
}
