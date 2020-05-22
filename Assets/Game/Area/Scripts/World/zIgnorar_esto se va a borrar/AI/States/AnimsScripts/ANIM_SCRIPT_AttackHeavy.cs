using UnityEngine;

public class ANIM_SCRIPT_AttackHeavy : ANIM_SCRIPT_Base 
{
    public bool isAtenea;

    protected override void OnAnimationExit(Animator anim)
    {
        if (!isAtenea) anim.SetBool("HeavyAttack", false);
    }
}
