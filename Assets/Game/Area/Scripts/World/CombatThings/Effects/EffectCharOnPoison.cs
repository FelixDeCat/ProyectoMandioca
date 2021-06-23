using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCharOnPoison : EffectBase
{
    [SerializeField] ParticleSystem poisonBubbles = null;
    DamageReceiver lifeSystem = null;

    [SerializeField] float timeTick = 0.5f;
    [SerializeField] int damagePerTick = 1;
    float timerPerTick;

    [SerializeField] Color onHitColor = Color.magenta;
    [SerializeField] float onHitFlashTime = 20f;
    [SerializeField] Renderer[] mats = new Renderer[0];

    [SerializeField] Color barLifeColor = Color.magenta;
    [SerializeField] Stats2D_Bar barLife = null;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        lifeSystem = GetComponentInParent<DamageReceiver>();
        ParticlesManager.Instance.GetParticlePool(poisonBubbles.name, poisonBubbles);
    }

    protected override void OnEffect()
    {
        barLife?.ChangeBarColor(barLifeColor);
    }

    protected override void OffEffect()
    {
        barLife?.ReturnToNormalColor();
        timerPerTick = 0;
    }

    protected override void OnTickEffect(float cdPercent)
    {
        timerPerTick += Time.deltaTime;
        if (timerPerTick >= timeTick)
        {
            lifeSystem.DamageTick(damagePerTick, Damagetype.Poison);
            timerPerTick = 0;
            Main.instance.Vibrate();
            Main.instance.GetMyCamera().BeginShakeCamera();
            StartCoroutine(OnHitted());
            ParticlesManager.Instance.PlayParticle(poisonBubbles.name, transform.position, transform);
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
