using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCharOnFire : EffectBase
{
    [SerializeField] ParticleSystem feedbackFireDot = null;
    DamageReceiver lifeSystem = null;

    [SerializeField] float timeTick = 0.5f;
    [SerializeField] int damagePerTick = 1;
    float timerPerTick;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        lifeSystem = GetComponentInParent<DamageReceiver>();
    }

    protected override void OffEffect()
    {
        feedbackFireDot.gameObject.SetActive(false);
        timerPerTick = 0;
    }

    protected override void OnEffect()
    {
        feedbackFireDot.gameObject.SetActive(true);
    }

    protected override void OnTickEffect(float cdPercent)
    {
        timerPerTick += Time.deltaTime;
        if (timerPerTick >= timeTick)
        {
            lifeSystem.DamageTick(damagePerTick, Damagetype.Fire);
            timerPerTick = 0;
        }
    }
}
