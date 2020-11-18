using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicOnFire : EffectBase
{
    [SerializeField] ParticleSystem feedbackFireDot = null;
    DamageReceiver lifeSystem = null;

    [SerializeField] float timeTick = 0.5f;
    [SerializeField] int damagePerTick = 1;
    float timer;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        lifeSystem = GetComponentInParent<DamageReceiver>();
    }

    protected override void OffEffect()
    {
        feedbackFireDot.gameObject.SetActive(false);
        timer = 0;
    }

    protected override void OnEffect()
    {
        feedbackFireDot.gameObject.SetActive(true);
    }

    protected override void OnTickEffect(float cdPercent)
    {
        timer += Time.deltaTime;
        if (timer >= timeTick)
        {
            lifeSystem.DamageTick(damagePerTick, Damagetype.Fire);
            timer = 0;
        }
    }
}
