using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRoot : EffectBase
{
    CharacterMovement charMove = null;
    CharacterAnimator anim = null;

    private void Start()
    {
        base.Start();
        charMove = Main.instance.GetChar().GetCharMove();
        anim = Main.instance.GetChar().charanim;
    }

    protected override void OffEffect()
    {
        charMove.EnableRotation();
        charMove.SetSpeed();
        Main.instance.GetChar().BlockRoll = false;
    }

    protected override void OnEffect()
    {
        charMove.CancelRotation();
        charMove.SetSpeed(0);
        Main.instance.GetChar().BlockRoll = true;
    }
  
    protected override void OnTickEffect(float cdPercent)
    {
        charMove.SetSpeed(0);
    }
}
