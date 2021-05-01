using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicElectrify : EffectBase
{
    [SerializeField] Animator anim = null;
    [SerializeField] Material electrifyMaat = null;
    EnemyBase owner = null;
    [SerializeField] DamageReceiver dmgReceiver = null;

    [SerializeField] float dmgPercentMultiplier = 2f;
    [SerializeField] ParticleSystem electricParticle;
    
    Material[] myMat;
    SkinnedMeshRenderer smr;

    float currentAnimSpeed;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        smr = anim.GetComponentInChildren<SkinnedMeshRenderer>();
        owner = GetComponentInParent<EnemyBase>();

        ParticlesManager.Instance.GetParticlePool(electricParticle.name, electricParticle);
    }

    protected override void OnEffect()
    {
        currentAnimSpeed = anim.speed;
        anim.speed = 0;

        if (smr != null)
        {
            myMat = smr.materials;
            Material[] mats = smr.materials;
            mats[0] = electrifyMaat;
            smr.materials = mats;
        }
        owner?.StunStart();
        dmgReceiver.AddTakeDamage(TakeDamageExtraFeedback);
        dmgReceiver.AddDebility(Damagetype.Normal, dmgPercentMultiplier);
        dmgReceiver.AddDebility(Damagetype.Heavy, dmgPercentMultiplier);


        //Feedback de cuando empieza el estado
        electrifyMaat.SetFloat("_pannerActivate", 1);
        ParticlesManager.Instance.PlayParticle(electricParticle.name, transform.position);
    }


    protected override void OffEffect()
    {
        anim.speed = currentAnimSpeed;
        if (smr != null)
        {
            smr.materials = myMat;
            AudioManager.instance.PlaySound("PetrifyEnd", transform);
        }
        owner?.StunOver();
        dmgReceiver.RestTakeDamage(TakeDamageExtraFeedback);
        dmgReceiver.RemoveDebility(Damagetype.Normal);
        dmgReceiver.RemoveDebility(Damagetype.Heavy);

        //Feedback de cuando termina el estado
        electrifyMaat.SetFloat("_pannerActivate", 0);
        ParticlesManager.Instance.StopParticle(electricParticle.name, electricParticle);
    }
        

    void TakeDamageExtraFeedback(DamageData dmgData)
    {
        //lo que quieran de feedback cuando me golpean
        OnEndEffect();
    }

    protected override void OnTickEffect(float cdPercent)
    {
        //Update
    }
}
