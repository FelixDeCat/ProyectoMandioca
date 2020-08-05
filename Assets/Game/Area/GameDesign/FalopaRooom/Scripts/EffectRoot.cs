using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRoot : EffectBase
{
    CharacterMovement charMove = null;
    CharacterAnimator anim;

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
        anim.Stun(false);
    }

    protected override void OnEffect()
    {
        charMove.CancelRotation();
        charMove.SetSpeed(0);
        anim.Stun(true);
    }
  

    protected override void OnTickEffect(float cdPercent)
    {
    }
}
