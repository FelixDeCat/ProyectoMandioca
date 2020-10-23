using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;
using System;

public class CharSwordAbilityRelease : CharacterStates
{
    Action toExecute;
    float timer = 0;  
    public CharSwordAbilityRelease(EState<CharacterHead.PlayerInputs> myState, Action _toExecute, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
    {
        toExecute = _toExecute;
    }

    protected override void Enter(EState<CharacterHead.PlayerInputs> input)
    {
        if (toExecute != null) toExecute.Invoke();
        Main.instance.GetChar().SetNormalSpeed();
    }

    protected override void Update()
    {
        charMove.MovementHorizontal(LeftHorizontal());
        charMove.MovementVertical(LeftVertical());
        charMove.RotateHorizontal(LeftHorizontal());
        charMove.RotateVertical(LeftVertical());
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;
            if (LeftHorizontal() == 0 && LeftVertical() == 0)
            {
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
            }
            else
            {
                sm.SendInput(CharacterHead.PlayerInputs.MOVE);
            }
        }
    }

    protected override void Exit(CharacterHead.PlayerInputs input)
    {
    }
}
