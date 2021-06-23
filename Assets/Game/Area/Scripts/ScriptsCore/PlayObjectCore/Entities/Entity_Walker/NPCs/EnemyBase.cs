using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class EnemyBase : NPCBase
{
    #region Variables
    [HideInInspector] public bool death;
    public string CurrentScene;
    protected bool petrified = false;

    public virtual void StunStart() => petrified = true;
    public virtual void StunOver() => petrified = false;

    #endregion 

    public virtual float ChangeSpeed(float newSpeed) => 0;

    public UnityEvent OnDeath;

    #region Damage Receiver y Damage Data
    [Header("BaseThings")]
    [SerializeField] protected DamageData dmgData;
    [SerializeField] protected DamageReceiver dmgReceiver;
    [SerializeField] public GenericLifeSystem lifesystem = null;
    protected Rigidbody rb;
    [SerializeField] protected Transform rootTransform = null;
    [SerializeField] protected Animator animator = null;
    float currentAnimSpeed;
    [SerializeField] UnityEvent OnResetEnemy = null;

    protected override void OnInitialize()
    {
        rb = GetComponent<Rigidbody>();
        InitializePathFinder(rb);
        dmgData?.Initialize(this);
        dmgReceiver?.SetIsDamage(IsDamage).AddDead(Death).AddTakeDamage(TakeDamageFeedback).AddInmuneFeedback(InmuneFeedback).Initialize(rootTransform, rb, lifesystem);
    }

    public virtual void ResetEntity()
    {
        StopAllCoroutines();
        lifesystem?.ResetLifeSystem();
        OnResetEnemy?.Invoke();
        OnReset();
    }

    public void DEBUG_InstaKill() => dmgReceiver.InstaKill();

    protected abstract void OnReset();

    protected abstract void TakeDamageFeedback(DamageData data);
    protected void Death(Vector3 dir) { OnDeath?.Invoke(); Die(dir);  EnemyManager.Instance.DeleteEnemy(this); }
    protected abstract void Die(Vector3 dir);
    protected abstract bool IsDamage();
    protected virtual void InmuneFeedback(DamageData data) { }

    public IEnumerator OnHitted(float onHitFlashTime, Color onHitColor)
    {
        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            Material[] mats = smr.materials;
            Color originalColor = mats[0].GetColor("_EmissionColor");
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

    public IEnumerator OnHitted(float onHitFlashTime, Color onHitColor, SkinnedMeshRenderer[] meshes)
    {
        for (int i = 0; i < onHitFlashTime; i++)
        {
            if (i < (onHitFlashTime / 2f))
            {
                for (int x = 0; x < meshes.Length; x++)
                {
                    meshes[x].materials[0].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
                }
            }
            else
            {
                for (int x = 0; x < meshes.Length; x++)
                {
                    meshes[x].materials[0].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        for (int x = 0; x < meshes.Length; x++)
        {
            meshes[x].materials[0].SetColor("_EmissionColor", Color.black);
        }
    }

    protected override void OnPause()
    {
        if (animator == null) return;
        currentAnimSpeed = animator.speed;
        animator.speed = 0;
    }

    protected override void OnResume()
    {
        if (animator == null) return;
        animator.speed = currentAnimSpeed;
    }

    #endregion

    public virtual void SpawnEnemy() { }
}
