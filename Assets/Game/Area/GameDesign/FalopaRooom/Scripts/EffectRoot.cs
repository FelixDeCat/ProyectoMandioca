using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRoot : EffectBase
{
    CharacterMovement charMove = null;
    CharacterAnimator anim = null;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        charMove = Main.instance.GetChar().GetCharMove();
        anim = Main.instance.GetChar().charanim;
    }

    protected override void OffEffect()
    {
        charMove.EnableRotation();
        charMove.StopForceBool();
        charMove.SetSpeed();
        Main.instance.GetChar().BlockRoll = false;
    }

    protected override void OnEffect()
    {
        charMove.CancelRotation();
        charMove.SetSpeed(0);
        charMove.StopForce();
        charMove.StopForceBool(true);
        Main.instance.GetChar().BlockRoll = true;
    }
  
    protected override void OnTickEffect(float cdPercent)
    {
        charMove.SetSpeed(0);
    }
}
