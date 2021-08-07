using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{

    public class CharDead : CharacterStates
    {
        float timeToScreen;
        float timer;

        bool execute_timer;

        public CharDead(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm, float _timeToScreen) : base(myState, _sm)
        {
            timeToScreen = _timeToScreen;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charAnim.Dead(true);
            execute_timer = true;
            Debug.Log("Entro a morir");
        }

        protected override void Update()
        {
            if (execute_timer)
            {
                timer += Time.deltaTime;
                if (timer >= timeToScreen)
                {
                    execute_timer = false;
                    GameLoop.instance.OnPlayerDeath();
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
            base.Exit(input);
            UnityEngine.Debug.Log("a ver por donde salgo la ptm");
            charAnim.CancelAttackAnimations();
            charAnim.Dead(false);
            timer = 0;
        }
    }
}
