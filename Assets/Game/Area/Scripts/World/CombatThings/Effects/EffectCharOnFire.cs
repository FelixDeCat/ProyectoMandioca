using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCharOnFire : EffectBase
{
    [SerializeField] ParticleSystem feedbackFireDot = null;
    DamageReceiver lifeSystem = null;

    [SerializeField] Renderer[] mats = new Renderer[0];
    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20f;

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
            StartCoroutine(OnHitted());
            Main.instance.Vibrate();
            Main.instance.GetMyCamera().BeginShakeCamera();
            timerPerTick = 0;
        }
    }

    protected IEnumerator OnHitted()
    {
        for (int i = 0; i < onHitFlashTime; i++)
        {
            if (i < (onHitFlashTime / 2f))
            {
                for (int z = 0; z < mats.Length; z++)
                    mats[z].materials[0].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
            }
            else
            {
                for (int z = 0; z < mats.Length; z++)
                    mats[z].materials[0].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int z = 0; z < mats.Length; z++)
            mats[z].materials[0].SetColor("_EmissionColor", Color.black);
    }
}
