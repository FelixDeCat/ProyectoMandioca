using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class CharElectricRelease : CharacterStates
{
    CharacterAnimator anim = null;
    float timer = 0;  
    public CharElectricRelease(EState<CharacterHead.PlayerInputs> myState, CharacterAnimator _anim, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
    {
        anim = _anim;
    }

    protected override void Enter(EState<CharacterHead.PlayerInputs> input)
    {
        Main.instance.GetChar().SetNormalSpeed();
        Debug.Log("Enter ReleaseCharActive");
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
        Debug.Log("Exit ReleaseCharActive");
    }
}
