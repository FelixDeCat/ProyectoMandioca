using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyBase : NPCBase, ICombatDirector
{
    #region Variables
    //encapsular esto
    [HideInInspector] public bool attacking;

    public abstract void ToAttack();
    public abstract void IAInitialize(CombatDirector director);

    //por lo que veo... varios le estan diciendo death = true
    //habria que llevarlo mas arriba en la jerarquia
    [HideInInspector] public bool death;

    #endregion

    public abstract float ChangeSpeed(float newSpeed);
    protected bool IsAttack() => attacking;

    Action<EnemyBase> OnFinishFeedbackDeath;
    public void AddCallbackFinishFeedbackDeath(Action<EnemyBase> _callback) => OnFinishFeedbackDeath = _callback;

    public bool Invinsible;

    //hacer components
    #region Combat Sensor (hacer component)
    //estas dos cosas tambien tendrian que tener un component... un sensor mas que nada
    //son dos cosas que se estan usando para hacer checkeos, cuando
    //los checkeos los tienen que hacer los components
    [Header("sensor combat")]
    [SerializeField] protected float combatDistance = 20;
    public bool combat;
    #endregion
    #region Combat Director Functions (hacer component)
    //cuando haya tiempo hacer un combat director connector component
    [Header("Combat director")]
    [SerializeField, Range(0.5f, 15)] protected float distancePos = 1.5f;
    protected bool withPos;
    protected EntityBase entityTarget;

    public Vector3 CurrentPos() => transform.position;
    public void SetTarget(EntityBase entity) => entityTarget = entity;
    public bool IsInPos() => withPos;
    public EntityBase CurrentTarget() => entityTarget;
    public float GetDistance() => distancePos;
    public void SetBool(bool isPos) => withPos = isPos;

    public virtual void ResetCombat()
    {
        entityTarget = null;
        combat = false;
        SetBool(false);
    }
    #endregion

    #region Damage Receiver y Damage Data
    [Header("BaseThings")]
    [SerializeField] protected DamageData dmgData;
    [SerializeField] protected DamageReceiver dmgReceiver;
    [SerializeField] protected GenericLifeSystem lifesystem = null;
    protected Rigidbody rb;
    [SerializeField] protected Transform rootTransform = null;

    

    protected override void OnInitialize()
    {
        rb = GetComponent<Rigidbody>();
        dmgData.Initialize(this);
        dmgReceiver.Initialize(rootTransform, IsDamage, Death, TakeDamageFeedback, rb, lifesystem, InmuneFeedback);
    }

    public void ResetEntity()
    {
        ResetCombat();
        StopAllCoroutines();
        lifesystem.ResetLifeSystem();
        OnReset();
    }

    protected abstract void OnReset();

    protected abstract void TakeDamageFeedback(DamageData data);
    void Death(Vector3 dir) { Die(dir); ReturnToSpawner(); }
    void FinishDeath() { 
        //OnFinishFeedbackDeath.Invoke(this);
        //OnFinishFeedbackDeath = delegate { };
    }

    protected abstract void Die(Vector3 dir);
    protected abstract bool IsDamage();
    protected virtual void InmuneFeedback() { }

    #endregion

    public void AddForceToRb(Vector3 dir, float knockbackForce, ForceMode forceMode)
    {
        rb.AddForce(dir.normalized * knockbackForce, forceMode);
    }

    public IEnumerator OnHitted(Material[] myMat, float onHitFlashTime, Color onHitColor)
    {
        var smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            myMat = smr.materials;

            Material[] mats = smr.materials;
            smr.materials = mats; // ??
            for (int i = 0; i < onHitFlashTime; i++)
            {
                if (i < (onHitFlashTime / 2f))
                {
                    mats[1].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
                }
                else
                {
                    mats[1].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public virtual void SpawnEnemy() => OnInitialize();
}
