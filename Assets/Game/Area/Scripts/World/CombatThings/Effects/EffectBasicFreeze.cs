using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicFreeze : EffectBase
{
    [SerializeField] GenericEnemyMove movement = null;
    [SerializeField] Animator anim = null;
    [SerializeField] Material freezeMat = null;

    [SerializeField] float freezeSpeedSlowed = 0.5f;

    SkinnedMeshRenderer smr;
    Material[] myMat;

    protected override void OffEffect()
    {
        movement.DivideSpeed(freezeSpeedSlowed);
        anim.speed /= freezeSpeedSlowed;
        if (smr != null)
        {
            smr.materials = myMat;
        }
    }

    protected override void OnEffect()
    {
        movement.MultiplySpeed(freezeSpeedSlowed);
        anim.speed *= freezeSpeedSlowed;

        if (smr != null)
        {
            myMat = smr.materials;
            smr.material = freezeMat;
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        smr = anim.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    protected override void OnTickEffect(float cdPercent)
    {
    }
}
