using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DamageReceiver), typeof(_Base_Life_System))]
public abstract class BaseDestructible : Environment
{
    [SerializeField] protected DestroyedVersion model_destroyedVersion;
    protected DestroyedVersion savedDestroyedVersion;

    [SerializeField] protected DamageReceiver damageReceiver;
    [SerializeField] protected _Base_Life_System _lifeSytstem;

    [SerializeField] AudioClip destroyedSound;

    [SerializeField] UnityEvent OnTakeDamage;
    [SerializeField] UnityEvent OnDestroyed;

    private void Start()
    {
        OnInitialize();
    }

    protected override void OnInitialize()
    {
        _lifeSytstem.Initialize( _lifeSytstem.life, ()=> { }, () => { }, () => { });

        damageReceiver.Initialize(transform,
            () => { return false; },
            (x) => { OnDestroyed.Invoke(); },
            (x) => { OnTakeDamage.Invoke(); },
            GetComponent<Rigidbody>(),
            _lifeSytstem);           

        AudioManager.instance.GetSoundPool(destroyedSound.name, AudioGroups.AMBIENT_FX, destroyedSound);
    }

    public void DestroyDestructible()
    {
        AudioManager.instance.PlaySound(destroyedSound.name, transform);
        OnDestroyDestructible();
    }
    protected abstract void OnDestroyDestructible();
    protected abstract void FeedbackDamage();

}