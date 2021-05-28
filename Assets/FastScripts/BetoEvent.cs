using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetoEvent : MonoBehaviour
{
    [SerializeField] DamageReceiver damagereciever = null;
    [SerializeField] Transform root = null;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] _Base_Life_System life = null;
    [SerializeField] Animator anim = null;
    bool _isDamaged = false;
    [SerializeField] ParticleSystem takeDamage_fb = null;
    [SerializeField] SkinnedMeshRenderer skinnedMeshrenderer = null;
    [SerializeField] Color onHitColor = Color.red;
    [SerializeField] float onHitFlashTime = 20;

    private void Start()
    {
        damagereciever.SetIsDamage(IsDamaged).AddTakeDamage(TakeDamageFeedback).Initialize(root, rb, life);
    }

    void TakeDamageFeedback(DamageData dData)
    {
        StartCoroutine(OnHitted(onHitFlashTime, onHitColor));
        if (takeDamage_fb) takeDamage_fb.Play();

        anim.Play("GetDamage");
    }

    public IEnumerator OnHitted(float onHitFlashTime, Color onHitColor)
    {
        Material[] mats = skinnedMeshrenderer.materials;

        for (int j = 0; j < onHitFlashTime; j++)
        {
            if (j < (onHitFlashTime / 2f))
            {
                mats[0].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, j / (onHitFlashTime / 2f)));
            }
            else
            {
                mats[0].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (j - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
            }
            yield return new WaitForSeconds(0.01f);
        }
        mats[0].SetColor("_EmissionColor", Color.black);

    }

    public bool IsDamaged() { return _isDamaged; }
}
