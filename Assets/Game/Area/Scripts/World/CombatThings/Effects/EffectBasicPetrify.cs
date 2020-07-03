using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicPetrify : EffectBase
{
    [SerializeField] Animator anim = null;
    [SerializeField] ParticleSystem petrifyParticles = null;
    [SerializeField] ParticleSystem endPetrifyParticles = null;
    [SerializeField] Material petrifiedMat = null;
    [SerializeField] AudioClip clip_PetrifyStand = null;
    [SerializeField] AudioClip clip_petrifyEnd = null;

    Material[] myMat;
    SkinnedMeshRenderer smr;

    float currentAnimSpeed;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        AudioManager.instance.GetSoundPool("PetrifyStand", AudioGroups.GAME_FX, clip_PetrifyStand);
        AudioManager.instance.GetSoundPool("PetrifyEnd", AudioGroups.GAME_FX, clip_petrifyEnd);

        smr = anim.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    protected override void OnEffect()
    {
        currentAnimSpeed = anim.speed;
        anim.speed = 0;

        if (smr != null)
        {
            myMat = smr.materials;
            petrifyParticles.Play();
            AudioManager.instance.PlaySound("PetrifyStand");
            Material[] mats = smr.materials;
            mats[0] = petrifiedMat;
            smr.materials = mats;
        }
    }

    protected override void OffEffect()
    {
        anim.speed = currentAnimSpeed;
        if (smr != null)
        {
            smr.materials = myMat;
            petrifyParticles.Stop();
            AudioManager.instance.PlaySound("PetrifyEnd");
            endPetrifyParticles.Play();
        }
    }

    protected override void OnTickEffect(float cdPercent)
    {

    }
}
