using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DamageReceiver), typeof(_Base_Life_System))]
public abstract class BaseDestructible : Environment
{
    [SerializeField] protected DestroyedVersion model_destroyedVersion = null;
    protected DestroyedVersion savedDestroyedVersion;

    [SerializeField] protected DamageReceiver damageReceiver = null;
    [SerializeField] protected _Base_Life_System _lifeSytstem = null;

    [SerializeField] AudioClip destroyedSound = null;

    [SerializeField] UnityEvent OnTakeDamage = null;
    [SerializeField] UnityEvent OnDestroyed = null;

    Transform target;

    protected override void OnInitialize()
    {
        _lifeSytstem.Initialize( _lifeSytstem.life, ()=> { }, () => { }, () => { });

        damageReceiver
            .AddDead((x) => { OnDestroyed.Invoke(); DestroyDestructible(); })
            .AddTakeDamage(TakeDamage)
            .Initialize(transform,GetComponent<Rigidbody>(),_lifeSytstem);

        AudioManager.instance.GetSoundPool(destroyedSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.AMBIENT_FX, destroyedSound);
    }

    void TakeDamage(DamageData data)
    {
        OnTakeDamage.Invoke();
        target = data.ownerRoot;
    }

    public void DestroyDestructible()
    {
        AudioManager.instance.PlaySound(destroyedSound.name, transform);
        OnDestroyDestructible(target ? target.position : transform.position);
    }
    protected abstract void OnDestroyDestructible(Vector3 destroyPoint = default);
    protected abstract void FeedbackDamage();
}