using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicOnFire : EffectBase
{
    [SerializeField] ParticleSystem feedbackFireDot = null;
    [SerializeField] GenericLifeSystem lifeSystem = null;

    [SerializeField] float timeTick = 0.5f;
    [SerializeField] int damagePerTick = 1;

    protected override void OffEffect()
    {
        feedbackFireDot.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    protected override void OnEffect()
    {
        feedbackFireDot.gameObject.SetActive(true);
        StartCoroutine(DamageTick());
    }

    IEnumerator DamageTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeTick);
            lifeSystem.Hit(damagePerTick);
        }
    }

    protected override void OnTickEffect(float cdPercent)
    {
    }
}
