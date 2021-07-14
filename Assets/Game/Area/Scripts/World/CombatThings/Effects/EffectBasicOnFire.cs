using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBasicOnFire : EffectBase
{
    [SerializeField] ParticleSystem feedbackFireDot = null;
    DamageReceiver lifeSystem = null;
    [SerializeField] SkinnedMeshRenderer mesh;

    [SerializeField] float timeTick = 0.5f;
    [SerializeField] int damagePerTick = 1;
    float timerPerTick;

    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20f;
    Material[] mats;
    [SerializeField] AudioClip _feedBack = null;
    Color originalColor;
    protected override void OnInitialize()
    {
        base.OnInitialize();
        lifeSystem = GetComponentInParent<DamageReceiver>();
        if (!mesh) mesh = lifeSystem.GetComponentInChildren<SkinnedMeshRenderer>();
        mats = mesh.materials;
        AudioManager.instance.GetSoundPool(_feedBack.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _feedBack);
        originalColor = mats[0].GetColor("_EmissionColor");
    }

    protected override void OffEffect()
    {
        feedbackFireDot.gameObject.SetActive(false);
        timerPerTick = 0;
        AudioManager.instance.StopAllSounds(_feedBack.name);
    }

    protected override void OnEffect()
    {
        feedbackFireDot.gameObject.SetActive(true);
        AudioManager.instance.PlaySound(_feedBack.name, transform);
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

    public IEnumerator OnHitted()
    {
        for (int i = 0; i < onHitFlashTime; i++)
        {
            if (i < (onHitFlashTime / 2f))
            {
                mats[0].SetColor("_EmissionColor", Color.Lerp(originalColor, onHitColor, i / (onHitFlashTime / 2f)));
            }
            else
            {
                mats[0].SetColor("_EmissionColor", Color.Lerp(onHitColor, originalColor, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
            }
            yield return new WaitForSeconds(0.01f);
        }
        mats[0].SetColor("_EmissionColor", originalColor);
    }
}
