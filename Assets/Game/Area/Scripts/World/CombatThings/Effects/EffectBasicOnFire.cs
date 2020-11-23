using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicOnFire : EffectBase
{
    [SerializeField] ParticleSystem feedbackFireDot = null;
    DamageReceiver lifeSystem = null;

    [SerializeField] float timeTick = 0.5f;
    [SerializeField] int damagePerTick = 1;
    float timerPerTick;

    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20f;
    Material[] mats;


    protected override void OnInitialize()
    {
        base.OnInitialize();
        lifeSystem = GetComponentInParent<DamageReceiver>();
        mats = lifeSystem.GetComponentInChildren<SkinnedMeshRenderer>().materials;
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
            StartCoroutine(OnHitted());
        }
    }

    protected IEnumerator OnHitted()
    {
        for (int i = 0; i < onHitFlashTime; i++)
        {
            if (i < (onHitFlashTime / 2f))
            {
                mats[0].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
            }
            else
            {
                mats[0].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
            }
            yield return new WaitForSeconds(0.01f);
        }
        mats[0].SetColor("_EmissionColor", Color.black);
    }
}
